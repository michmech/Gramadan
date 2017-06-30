using System;
using System.Collections.Generic;
using System.Text;
using Gramadan;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace Tester
{
	class Consistency
	{
		public static void Nouns()
		{
			StreamWriter writer=new StreamWriter(@"C:\MBM\Gramadan\consistency-report-nouns.txt");
			string[] folderNames= { "noun", "nounNew" };

			int count=0;
			int countNoPl=0;

			foreach(string folderName in folderNames) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folderName)) {
					XmlDocument doc=new XmlDocument(); doc.Load(file);
					Noun noun=new Noun(doc);

					count++;
					if(noun.sgNom.Count==0) {
						writer.WriteLine(folderName+"\t"+noun.getNickname()+"\tUatha in easnamh.");
					}
					if(noun.sgNom.Count>0 && noun.sgGen.Count==0) {
						writer.WriteLine(folderName+"\t"+noun.getNickname()+"\tGinideach uatha in easnamh.");
					}
					if(noun.plNom.Count==0) {
						countNoPl++;
					}
					if(noun.plNom.Count>0 && noun.plGen.Count==0) {
						writer.WriteLine(folderName+"\t"+noun.getNickname()+"\tGinideach iolra in easnamh.");
					}

				}
			}
			writer.WriteLine("----");
			writer.WriteLine("Iomlán:\t"+count);
			writer.WriteLine("Gan iolra:\t"+countNoPl);
			writer.Close();
		}
		public static void Adjectives()
		{
			StreamWriter writer=new StreamWriter(@"C:\MBM\Gramadan\consistency-report-adjectives.txt");
			string[] folderNames= { "adjective", "adjectiveNew" };

			int count=0;

			foreach(string folderName in folderNames) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folderName)) {
					XmlDocument doc=new XmlDocument(); doc.Load(file);
					Adjective adj=new Adjective(doc);

					count++;
					if(adj.sgNom.Count==0) {
						writer.WriteLine(folderName+"\t"+adj.getNickname()+"\tUatha in easnamh.");
					}
					if(adj.sgNom.Count>0 && adj.sgGenMasc.Count==0) {
						writer.WriteLine(folderName+"\t"+adj.getNickname()+"\tGinideach uatha fir. in easnamh.");
					}
					if(adj.sgNom.Count>0 && adj.sgGenFem.Count==0) {
						writer.WriteLine(folderName+"\t"+adj.getNickname()+"\tGinideach uatha bain. in easnamh.");
					}
					if(adj.plNom.Count==0) {
						writer.WriteLine(folderName+"\t"+adj.getNickname()+"\tIolra in easnamh.");
					}
					if(adj.graded.Count==0) {
						writer.WriteLine(folderName+"\t"+adj.getNickname()+"\tFoirmeacha céimithe in easnamh.");
					}
				}
			}
			writer.WriteLine("----");
			writer.WriteLine("Iomlán:\t"+count);
			writer.Close();
		}
		public static void Similarity()
		{
			StreamWriter writer=new StreamWriter(@"C:\MBM\Gramadan\consistency-report-similarity.txt");
			string[] folderNames= { "noun", "nounNew", "adjective", "adjectiveNew", "verb", "verbNew", "verbNew2" };

			foreach(string folderName in folderNames) {
				Console.WriteLine(folderName);
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folderName)) {
					XmlDocument doc=new XmlDocument(); doc.Load(file);

					string nickname=Path.GetFileNameWithoutExtension(file);
					string lemma=doc.SelectSingleNode("/*/@default").Value;

					string lemmaStart=lemma.Substring(0, 1);

					foreach(XmlAttribute att in doc.SelectNodes("/*/*/@default")) {
						string form=att.Value;
						if(!form.StartsWith(lemmaStart)) {
							writer.WriteLine(folderName+"\t"+nickname+"\tFoirm éagosúil:\t"+form);
						}
					}
				}
			}

			writer.Close();
		}
		public static void VerbalNouns()
		{
			StreamWriter writer=new StreamWriter(@"C:\MBM\Gramadan\consistency-vns.txt");
			string[] folderNames= { "verbNew" };

			int count=0;
			Dictionary<string, string> collector=new Dictionary<string, string>();

			foreach(string folderName in folderNames) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folderName)) {
					XmlDocument doc=new XmlDocument(); doc.Load(file);
					Verb v=new Verb(doc);

					string vn=v.verbalAdjective[0].value;
					if(!collector.ContainsKey(vn)) {
						collector.Add(vn, v.getNickname());
					} else {
						writer.WriteLine(folderName+"\t"+vn+"\t"+collector[vn]+"\t"+v.getNickname());
						count++;
					}
				}
			}
			writer.WriteLine("----");
			writer.WriteLine("Iomlán:\t"+count);
			writer.Close();
		}
		public static void VerbalWhitespace()
		{
			StreamWriter writer=new StreamWriter(@"C:\MBM\Gramadan\consistency-whitespace.txt");
			string[] folderNames= { "verbNew", "verbNew2" };

			int count=0;
			Dictionary<string, string> collector=new Dictionary<string, string>();

			foreach(string folderName in folderNames) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folderName)) {
					XmlDocument doc=new XmlDocument(); doc.Load(file);
					foreach(XmlAttribute attr in doc.SelectNodes("//*/@*")) {
						if(Regex.IsMatch(attr.Value, "ie")) {
							writer.WriteLine(file);
						}
					}

					//writer.WriteLine(folderName+"\t"+vn+"\t"+collector[vn]+"\t"+v.getNickname());
				}
			}
			writer.WriteLine("----");
			writer.WriteLine("Iomlán:\t"+count);
			writer.Close();
		}
	}
}
