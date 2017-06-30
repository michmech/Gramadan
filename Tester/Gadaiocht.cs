using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Tester
{
	class Gadaiocht
	{
		public static void GoidTearmaAinmfhocail()
		{
			StreamWriter writer=new StreamWriter(@"C:\deleteme\output.txt");
			
			//Léigh liostaFoc agus stóráil mar liosta é:
			List<string> liostaFoc=new List<string>();
			{
				StreamReader reader=new StreamReader(@"E:\deleteme\liostaFoc.txt");
				while(reader.Peek()>-1) { //a fhad is atá téacs éigin fós fágtha go deireadh an chomhaid
					string line=reader.ReadLine().Trim(); //léigh focal as an chomhad
					if(line!="" && !liostaFoc.Contains(line)) liostaFoc.Add(line); //mura folamh, agus mura bhfuil an focal ar an liosta cheana, cuir an focal leis an liosta
				}
				reader.Close();
			}

			//Léigh an comhad TBX:
			XmlTextReader xmlReader=new XmlTextReader(@"E:\deleteme\fullTearma.txt");
			xmlReader.Namespaces=false; //ná bac leis na hainmspáis sa chomhad
			while(xmlReader.Read()) { //léigh an chéad nód eile, a fhad is atá aon nód fágtha go deireadh an chomhaid
				if(xmlReader.NodeType==XmlNodeType.Element && xmlReader.Name=="tig") { //más eilimint an nód reatha agus más 'tig' is ainm dó
					XmlDocument tig=new XmlDocument(); tig.Load(xmlReader.ReadSubtree()); //lódáil <tig>...</tig> mar dhoiciméad XML

					//Léigh an téarma:
					string lemma=GetXPath(tig, "/tig/term/text()", ""); //faigh an focal
					string nod=GetXPath(tig, "/tig/termNote[@type='partOfSpeech']/text()", ""); //faigh an nod gramadaí
					bool isNoun=(!lemma.Contains(" ") && lemma.ToLower()==lemma && (nod.StartsWith("fir") || nod.StartsWith("bain"))); //an ainmfhocal é?
					if(isNoun && liostaFoc.Contains(lemma)) { //más ainmfhocal é agus más é seo ceann de na focail atá uainn

						//Faigh eolas maidir leis an téarma:
						Gramadan.Gender gender=Gramadan.Gender.Masc; if(nod.StartsWith("bain")) gender=Gramadan.Gender.Fem; //cén inscne an téarma?
						int declension=0; for(int i=1; i<6; i++) if(nod.EndsWith(i.ToString())) declension=i; //cén díochlanadh é?
						Console.WriteLine(lemma+" "+gender+" "+declension);

						//Faigh foirmeacha infhillte an téarma:
						string gu=GetXPath(tig, "/tig/termNote[@type='gu']/text()", ""); //ginideach uatha
						string ai=GetXPath(tig, "/tig/termNote[@type='ai']/text()", ""); //ainmneacha iolra
						string gi=GetXPath(tig, "/tig/termNote[@type='gi']/text()", ""); //ginideach iolra
						if(ai=="" && gi=="") { //mura bhfuil ainmneach iolra agus ginideach iolra luaite, b'fhéidir go bhfuil iolra luaite:
							ai=GetXPath(tig, "/tig/termNote[@type='iol']/text()", "");
							gi=GetXPath(tig, "/tig/termNote[@type='iol']/text()", "");
						}
						Gramadan.Strength strength=Gramadan.Strength.Strong; if(gi==lemma) strength=Gramadan.Strength.Weak; //an lagiolra nó tréaniolra é?
						Console.WriteLine(" gu: "+gu);
						Console.WriteLine(" ai: "+gi);
						Console.WriteLine(" gi: "+gi);

						//Más úsáideach an t-eolas atá aimsithe againn:
						if(gu!="" || ai!="" || gi!="") {

							//Cruthaigh ainmfhocal nua i nGramadán:
							Gramadan.Noun n=new Gramadan.Noun();
							n.declension=declension;
							n.sgNom.Add(new Gramadan.FormSg(lemma, gender));
							if(gu!="") n.sgGen.Add(new Gramadan.FormSg(gu, gender));
							if(ai!="") n.plNom.Add(new Gramadan.Form(ai));
							if(gi!="") n.plGen.Add(new Gramadan.FormPlGen(gi, strength));

							//Sábháil an t-ainmfhocal:
							n.printXml().Save(@"C:\deleteme\"+n.getNickname()+".xml");
							writer.WriteLine(lemma+"\t"+gu+"\t"+ai+"\t"+gi);

							//Bain an focal as liostaFoc mar tá sé déanta againn cheana:
							liostaFoc.Remove(lemma);
						}
					}
				}
			}

			writer.Close();
		}

		//Úsáid an slonn XPath chun luach a aimsiú sa doiciméad XML; murab ann dó, tabhair dom ifNull:
		private static string GetXPath(XmlDocument doc, string xpath, string ifNull)
		{
			string ret=ifNull;
			if(doc.SelectSingleNode(xpath)!=null) ret=doc.SelectSingleNode(xpath).Value;
			return ret;
		}
	}
}
