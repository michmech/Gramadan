using System;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

namespace Gramadan
{
	//A class for a prepositional phrase:
	public class PP
	{
		//Forms:
        public List<FormSg> sg=new List<FormSg>();     //singular, no article
		public List<FormSg> sgArtN=new List<FormSg>(); //singular, with article, northern system
		public List<FormSg> sgArtS=new List<FormSg>(); //singular, with article, southern system
        public List<Form> pl=new List<Form>();         //plural, no article
        public List<Form> plArt=new List<Form>();      //plural, with article

		//The nickname of the preposition from which this prepositional phrase was created:
		public string prepNick="";

        //Returns the prepositional phrase's lemma:
        public string getLemma()
        {
            string ret="";
			if(this.sg.Count!=0) ret=this.sg[0].value;
			if(ret=="" && this.sgArtS.Count!=0) ret=this.sgArtS[0].value;
			if(ret=="" && this.sgArtN.Count!=0) ret=this.sgArtN[0].value;
			if(ret=="" && this.pl.Count!=0) ret=this.pl[0].value;
			if(ret=="" && this.plArt.Count!=0) ret=this.plArt[0].value;
			return ret;
        }

		public string getNickname()
		{
			string ret=getLemma()+" PP";
			ret=ret.Replace(" ", "_");
			return ret;
		}

		//Returns the prepositional phrase's gender:
		public Gender getGender()
		{
			Gender ret=Gender.Masc;
			if(this.sg.Count!=0) ret=this.sg[0].gender;
			else if(this.sgArtS.Count!=0) ret=this.sgArtS[0].gender;
			else if(this.sgArtN.Count!=0) ret=this.sgArtN[0].gender;
			return ret;
		}
		public bool hasGender() {
			bool ret=false;
			if(this.sg.Count!=0 || this.sgArtS.Count!=0 || this.sgArtN.Count!=0) ret=true;
			return ret;
		}

		//Is the prepositional phrase invalid? This can happen if it has been constructed from an unsupported preposition:
		public bool isInvalid()
		{
            bool ret=true;
			if(this.sg.Count!=0) ret=false;
			if(ret && this.sgArtS.Count!=0) ret=false;
			if(ret && this.sgArtN.Count!=0) ret=false;
			if(ret && this.pl.Count!=0) ret=false;
			if(ret && this.plArt.Count!=0) ret=false;
			return ret;
		}

        //Prints a user-friendly summary of the prepositional phrase's forms:
        public string print()
        {
            string ret="";
            ret+="uatha, gan alt:                  "; foreach(Form f in this.sg) { ret+="["+f.value +"] "; } ret+=Environment.NewLine;
			ret+="uatha, alt, córas lárnach:       "; foreach(Form f in this.sgArtS) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="uatha, alt, córas an tséimhithe: "; foreach(Form f in this.sgArtN) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
            ret+="iolra, gan alt:                  "; foreach(Form f in this.pl) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="iolra, alt:                      "; foreach(Form f in this.plArt) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret=ret.Replace("] [", ", ").Replace("[", "").Replace("] ", "");
            return ret;
        }

		public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<prepositionalPhrase/>");
			doc.DocumentElement.SetAttribute("default", this.getLemma());
			doc.DocumentElement.SetAttribute("prepNick", this.prepNick);
			foreach(FormSg f in this.sg) {
				XmlElement el=doc.CreateElement("sg");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgArtS) {
				XmlElement el=doc.CreateElement("sgArtS");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgArtN) {
				XmlElement el=doc.CreateElement("sgArtN");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.pl) {
				XmlElement el=doc.CreateElement("pl");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plArt) {
				XmlElement el=doc.CreateElement("plArt");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			return doc;
		}


		//Creates a prepositional phrase from a preposition and a noun phrase:
        public PP(Preposition prep, NP np)
        {
			if(!np.isPossessed) populateFromUnpossNP(prep, np);
			else populateFromPossNP(prep, np);
		}

		//Populates "this" as a prepositional phrase composed from a preposition and an unpossessed noun phrase:
		private void populateFromUnpossNP(Preposition prep, NP np)
		{
			this.prepNick=prep.getNickname();
			if(this.prepNick=="ag_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("ag "+f.value, f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("ag "+f.value));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("ag an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("ag an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("ag na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="ar_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("ar "+Opers.Mutate(Mutation.Len1, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("ar "+Opers.Mutate(Mutation.Len1, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("ar an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("ar an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("ar na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="thar_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("thar "+Opers.Mutate(Mutation.Len1, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("thar "+Opers.Mutate(Mutation.Len1, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("thar an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("thar an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("thar na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="as_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("as "+f.value, f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("as "+f.value));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("as an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("as an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("as na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="chuig_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("chuig "+f.value, f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("chuig "+f.value));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("chuig an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("chuig an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("chuig na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="de_prep") {
				foreach(FormSg f in np.sgDat) {
					string txt=Opers.Mutate(Mutation.Len1, f.value); if(Opers.StartsVowelFhx(txt)) txt="d'"+txt; else txt="de "+txt;
					this.sg.Add(new FormSg(txt, f.gender));
				}
				foreach(Form f in np.plDat) {
					string txt=Opers.Mutate(Mutation.Len1, f.value); if(Opers.StartsVowelFhx(txt)) txt="d'"+txt; else txt="de "+txt;
					this.pl.Add(new Form(txt));
				}
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("den "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("den "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Len3:Mutation.Len2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("de na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="do_prep") {
				foreach(FormSg f in np.sgDat) {
					string txt=Opers.Mutate(Mutation.Len1, f.value); if(Opers.StartsVowelFhx(txt)) txt="d'"+txt; else txt="do "+txt;
					this.sg.Add(new FormSg(txt, f.gender));
				}
				foreach(Form f in np.plDat) {
					string txt=Opers.Mutate(Mutation.Len1, f.value); if(Opers.StartsVowelFhx(txt)) txt="d'"+txt; else txt="do "+txt;
					this.pl.Add(new Form(txt));
				}
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("don "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("don "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Len3:Mutation.Len2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("do na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="faoi_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("faoi "+Opers.Mutate(Mutation.Len1, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("faoi "+Opers.Mutate(Mutation.Len1, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("faoin "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("faoin "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("faoi na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="i_prep") {
				foreach(FormSg f in np.sgDat) {
					if(Opers.StartsVowel(f.value)) this.sg.Add(new FormSg("in "+f.value, f.gender));
					else this.sg.Add(new FormSg("i "+Opers.Mutate(Mutation.Ecl1x, f.value), f.gender));
				}
				foreach(Form f in np.plDat) {
					if(Opers.StartsVowel(f.value)) this.pl.Add(new Form("in "+f.value));
					else this.pl.Add(new Form("i "+Opers.Mutate(Mutation.Ecl1x, f.value)));
				}
				foreach(FormSg f in np.sgDatArtN) {
					string txt=Opers.Mutate(Mutation.Len3, f.value);
					if(Opers.StartsVowelFhx(txt)) txt="san "+txt; else txt="sa "+txt;
					this.sgArtN.Add(new FormSg(txt, f.gender));
				}
				foreach(FormSg f in np.sgDatArtS) {
					string txt=Opers.Mutate((f.gender==Gender.Fem?Mutation.Len3:Mutation.Len2), f.value);
					if(Opers.StartsVowelFhx(txt)) txt="san "+txt; else txt="sa "+txt;
					this.sgArtS.Add(new FormSg(txt, f.gender));
				}
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("sna "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="le_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("le "+Opers.Mutate(Mutation.PrefH, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("le "+Opers.Mutate(Mutation.PrefH, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("leis an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("leis an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("leis na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="ó_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("ó "+Opers.Mutate(Mutation.Len1, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("ó "+Opers.Mutate(Mutation.Len1, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("ón "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("ón "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("ó na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="roimh_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("roimh "+Opers.Mutate(Mutation.Len1, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("roimh "+Opers.Mutate(Mutation.Len1, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("roimh an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("roimh an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("roimh na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="trí_prep") {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg("trí "+Opers.Mutate(Mutation.Len1, f.value), f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form("trí "+Opers.Mutate(Mutation.Len1, f.value)));
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("tríd an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("tríd an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("trí na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
			if(this.prepNick=="um_prep") {
				foreach(FormSg f in np.sgDat) {
					string txt=f.value; if(!Opers.StartsBilabial(txt)) txt=Opers.Mutate(Mutation.Len1, txt);
					this.sg.Add(new FormSg("um "+txt, f.gender));
				}
				foreach(Form f in np.plDat) {
					string txt=f.value; if(!Opers.StartsBilabial(txt)) txt=Opers.Mutate(Mutation.Len1, txt);
					this.pl.Add(new Form("um "+txt));
				}
				foreach(FormSg f in np.sgDatArtN) this.sgArtN.Add(new FormSg("um an "+Opers.Mutate(Mutation.Len3, f.value), f.gender));
				foreach(FormSg f in np.sgDatArtS) this.sgArtS.Add(new FormSg("um an "+Opers.Mutate((f.gender==Gender.Fem?Mutation.Ecl3:Mutation.Ecl2), f.value), f.gender));
				foreach(Form f in np.plDatArt) this.plArt.Add(new Form("um na "+Opers.Mutate(Mutation.PrefH, f.value)));
			}
		}

		//Populates "this" as a prepositional phrase composed from a preposition and a possessed noun phrase:
		private void populateFromPossNP(Preposition prep, NP np)
		{
			this.prepNick = prep.getNickname();
			if(this.prepNick=="de_prep" || this.prepNick=="do_prep") {
				foreach(FormSg f in np.sgDat){
					if(f.value.StartsWith("a ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^a ", "dá "), f.gender));
					if(f.value.StartsWith("ár ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^ár ", "dár "), f.gender));
					else this.sg.Add(new FormSg(prep.getLemma()+" " + f.value, f.gender));
				}
				foreach(Form f in np.plDat){
					if(f.value.StartsWith("a ")) this.pl.Add(new Form(Regex.Replace(f.value, "^a ", "dá ")));
					if(f.value.StartsWith("ár ")) this.pl.Add(new Form(Regex.Replace(f.value, "^ár ", "dár ")));
					else this.pl.Add(new Form(prep.getLemma()+" " + f.value));
				}
			}
			else if(this.prepNick=="faoi_prep") {
				foreach(FormSg f in np.sgDat){
					if(f.value.StartsWith("a ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^a ", "faoina "), f.gender));
					if(f.value.StartsWith("ár ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^ár ", "faoinár "), f.gender));
					else this.sg.Add(new FormSg(prep.getLemma()+" " + f.value, f.gender));
				}
				foreach(Form f in np.plDat){
					if(f.value.StartsWith("a ")) this.pl.Add(new Form(Regex.Replace(f.value, "^a ", "faoina ")));
					if(f.value.StartsWith("ár ")) this.pl.Add(new Form(Regex.Replace(f.value, "^ár ", "faoinár ")));
					else this.pl.Add(new Form(prep.getLemma()+" " + f.value));
				}
			}
			else if(this.prepNick=="i_prep") {
				foreach(FormSg f in np.sgDat){
					if(f.value.StartsWith("a ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^a ", "ina "), f.gender));
					if(f.value.StartsWith("ár ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^ár ", "inár "), f.gender));
					if(f.value.StartsWith("bhur ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^bhur ", "in bhur "), f.gender));
					else this.sg.Add(new FormSg(prep.getLemma()+" " + f.value, f.gender));
				}
				foreach(Form f in np.plDat){
					if(f.value.StartsWith("a ")) this.pl.Add(new Form(Regex.Replace(f.value, "^a ", "ina ")));
					if(f.value.StartsWith("ár ")) this.pl.Add(new Form(Regex.Replace(f.value, "^ár ", "inár ")));
					if(f.value.StartsWith("bhur ")) this.pl.Add(new Form(Regex.Replace(f.value, "^bhur ", "in bhur ")));
					else this.pl.Add(new Form(prep.getLemma()+" " + f.value));
				}
			}
			else if(this.prepNick=="le_prep") {
				foreach(FormSg f in np.sgDat){
					if(f.value.StartsWith("a ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^a ", "lena "), f.gender));
					if(f.value.StartsWith("ár ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^ár ", "lenár "), f.gender));
					else this.sg.Add(new FormSg(prep.getLemma()+" " + f.value, f.gender));
				}
				foreach(Form f in np.plDat){
					if(f.value.StartsWith("a ")) this.pl.Add(new Form(Regex.Replace(f.value, "^a ", "lena ")));
					if(f.value.StartsWith("ár ")) this.pl.Add(new Form(Regex.Replace(f.value, "^ár ", "lenár ")));
					else this.pl.Add(new Form(prep.getLemma()+" " + f.value));
				}
			}
			else if(this.prepNick=="ó_prep") {
				foreach(FormSg f in np.sgDat){
					if(f.value.StartsWith("a ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^a ", "óna "), f.gender));
					if(f.value.StartsWith("ár ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^ár ", "ónár "), f.gender));
					else this.sg.Add(new FormSg(prep.getLemma()+" " + f.value, f.gender));
				}
				foreach(Form f in np.plDat){
					if(f.value.StartsWith("a ")) this.pl.Add(new Form(Regex.Replace(f.value, "^a ", "óna ")));
					if(f.value.StartsWith("ár ")) this.pl.Add(new Form(Regex.Replace(f.value, "^ár ", "ónár ")));
					else this.pl.Add(new Form(prep.getLemma()+" " + f.value));
				}
			}
			else if(this.prepNick=="trí_prep") {
				foreach(FormSg f in np.sgDat){
					if(f.value.StartsWith("a ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^a ", "trína "), f.gender));
					if(f.value.StartsWith("ár ")) this.sg.Add(new FormSg(Regex.Replace(f.value, "^ár ", "trínár "), f.gender));
					else this.sg.Add(new FormSg(prep.getLemma()+" " + f.value, f.gender));
				}
				foreach(Form f in np.plDat){
					if(f.value.StartsWith("a ")) this.pl.Add(new Form(Regex.Replace(f.value, "^a ", "trína ")));
					if(f.value.StartsWith("ár ")) this.pl.Add(new Form(Regex.Replace(f.value, "^ár ", "trínár ")));
					else this.pl.Add(new Form(prep.getLemma()+" " + f.value));
				}
			}
			else {
				foreach(FormSg f in np.sgDat) this.sg.Add(new FormSg(prep.getLemma()+" "+f.value, f.gender));
				foreach(Form f in np.plDat) this.pl.Add(new Form(prep.getLemma()+" "+f.value));
			}
		}

		//Constructs a prepositional phrase from an XML file in BuNaMo format:
		public PP(XmlDocument doc)
		{
			this.prepNick=doc.DocumentElement.GetAttribute("prepNick");
			foreach(XmlElement el in doc.SelectNodes("/*/sg")) {
				this.sg.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgArtN")) {
				this.sgArtN.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgArtS")) {
				this.sgArtS.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/pl")) {
				this.pl.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plArt")) {
				this.plArt.Add(new Form(el.GetAttribute("default")));
			}
		}
		public PP(string fileName) : this(Utils.LoadXml(fileName)) { }
    }
}
