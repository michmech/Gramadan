using System;
using System.Collections.Generic;
using System.Xml;

namespace Gramadan
{
	//A noun:
	public class Noun
	{
		public string disambig="";
		public string getNickname() {
			string ret=getLemma();
			ret+=(this.getGender()==Gender.Masc ? " masc" : " fem");
			ret+=(this.declension>0 ? this.declension.ToString() : "");
			if(this.disambig!="") ret+=" "+this.disambig;
			ret=ret.Replace(" ", "_");
			return ret;
		}
		public string getFriendlyNickname()
		{
			string ret=getLemma();
			ret+=" (";
			ret+=(this.getGender()==Gender.Masc ? "masc" : "fem");
			ret+=(this.declension>0 ? this.declension.ToString() : "");
			if(this.disambig!="") ret+=" "+this.disambig;
			ret+=")";
			return ret;
		}
		
		//The noun's traditional declension class (not actually used for anything); default is 0 meaning "none":
		public int declension=0;
		
		//Noun forms in the singular:
		public List<FormSg> sgNom=new List<FormSg>();
		public List<FormSg> sgGen=new List<FormSg>();
		public List<FormSg> sgVoc=new List<FormSg>();
		public List<FormSg> sgDat=new List<FormSg>();

		//Noun forms in the plural:
		public List<Form> plNom=new List<Form>();
		public List<FormPlGen> plGen=new List<FormPlGen>();
		public List<Form> plVoc=new List<Form>();

		//Noun form for counting (if any):
		public List<Form> count=new List<Form>();

		//Whether this is a proper name:
		public bool isProper=false; //If true, then all article-less genitives are always lenited, no matter what.

		//Whether this noun cannot be mutated (overrides isProper):
		public bool isImmutable=false; //Eg. "blitz", genitive singular "an blitz"

        //Whether this noun is already definite, even without an article:
        public bool isDefinite=false; //If true, then no articled forms will be generated when this noun is converted into a noun phrase.

        //For definite noun (isDefinite==true), whether an articled genitive may be generated
        public bool allowArticledGenitive=false; //Eg. "na hÉireann", "na Gaillimhe"

        //Returns the noun's lemma:
        public string getLemma()
        {
            string ret="";
            Form lemmaForm=this.sgNom[0];
            if(lemmaForm!=null) {
                ret=lemmaForm.value;
            }
            return ret;
        }

		//Returns the noun's gender:
		public Gender getGender()
		{
			return this.sgNom[0].gender;
		}

		//Constructors:
		public Noun() { }
		public Noun(SingularInfo si) : this(si, null) { } 
		public Noun(SingularInfo si, PluralInfo pi)
		{ 
			foreach(Form wf in si.nominative) this.sgNom.Add(new FormSg(wf.value, si.gender));
			foreach(Form wf in si.genitive) this.sgGen.Add(new FormSg(wf.value, si.gender));
			foreach(Form wf in si.vocative) this.sgVoc.Add(new FormSg(wf.value, si.gender));
			foreach(Form wf in si.dative) this.sgDat.Add(new FormSg(wf.value, si.gender));
			if(pi!=null) {
				foreach(Form wf in pi.nominative) this.plNom.Add(new Form(wf.value));
				foreach(Form wf in pi.genitive) this.plGen.Add(new FormPlGen(wf.value, pi.strength));
				foreach(Form wf in pi.vocative) this.plVoc.Add(new Form(wf.value));
			}
			AddDative(this);
		}
		public Noun(Gender gender, string sgNom, string sgGen, string sgVoc, Strength strength, string plNom, string plGen, string plVoc)
		{
			this.sgNom.Add(new FormSg(sgNom, gender));
			this.sgGen.Add(new FormSg(sgGen, gender));
			this.sgVoc.Add(new FormSg(sgVoc, gender));
			this.sgDat.Add(new FormSg(sgNom, gender));
			this.plNom.Add(new Form(plNom));
			this.plGen.Add(new FormPlGen(plGen, strength));
			this.plVoc.Add(new Form(plVoc));
			AddDative(this);
		}
		public Noun(XmlDocument doc)
		{
			this.disambig=doc.DocumentElement.GetAttribute("disambig");
			int.TryParse(doc.DocumentElement.GetAttribute("declension"), out this.declension);
			this.isProper=(doc.DocumentElement.GetAttribute("isProper")=="1" ? true : false);
			this.isImmutable=(doc.DocumentElement.GetAttribute("isImmutable")=="1" ? true : false);
			this.isDefinite=(doc.DocumentElement.GetAttribute("isDefinite")=="1" ? true : false);
			this.allowArticledGenitive=(doc.DocumentElement.GetAttribute("allowArticledGenitive")=="1" ? true : false);
			foreach(XmlElement el in doc.SelectNodes("/*/sgNom")) {
				this.sgNom.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgGen")) {
				this.sgGen.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgVoc")) {
				this.sgVoc.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgDat")) {
				this.sgDat.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plNom")) {
				this.plNom.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plGen")) {
				this.plGen.Add(new FormPlGen(el.GetAttribute("default"), (el.GetAttribute("strength")=="weak" ? Strength.Weak : Strength.Strong)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plVoc")) {
				this.plVoc.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/count")) {
				this.count.Add(new Form(el.GetAttribute("default")));
			}
			AddDative(this);
		}
		public Noun(string fileName) : this(Utils.LoadXml(fileName)) { }
		
		//Called from each constructor to make sure the noun has a dative singular:
		private static void AddDative(Noun n)
		{
			if(n.sgDat.Count==0) foreach(FormSg f in n.sgNom) n.sgDat.Add(new FormSg(f.value, f.gender));
		}

		//Prints a user-friendly summary of the noun's forms:
		public string print()
		{
			string ret="";
			ret+="sgNom: "; foreach(Form f in this.sgNom) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="sgGen: "; foreach(Form f in this.sgGen) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="sgVoc: "; foreach(Form f in this.sgVoc) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="sgDat: "; foreach(Form f in this.sgDat) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plNom: "; foreach(Form f in this.plNom) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plGen: "; foreach(Form f in this.plGen) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plVoc: "; foreach(Form f in this.plVoc) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			return ret;
		}

		//Prints the noun in BuNaMo format:
		public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<noun/>");
			doc.DocumentElement.SetAttribute("default", this.getLemma());
			doc.DocumentElement.SetAttribute("declension", this.declension.ToString());
			doc.DocumentElement.SetAttribute("disambig", this.disambig);
			doc.DocumentElement.SetAttribute("isProper", (this.isProper ? "1" : "0"));
			doc.DocumentElement.SetAttribute("isImmutable", (this.isImmutable ? "1" : "0"));
			doc.DocumentElement.SetAttribute("isDefinite", (this.isDefinite ? "1" : "0"));
			doc.DocumentElement.SetAttribute("allowArticledGenitive", (this.allowArticledGenitive ? "1" : "0"));
			foreach(FormSg f in this.sgNom) {
				XmlElement el=doc.CreateElement("sgNom");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgGen) {
				XmlElement el=doc.CreateElement("sgGen");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgVoc) {
				XmlElement el=doc.CreateElement("sgVoc");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgDat) {
				XmlElement el=doc.CreateElement("sgDat");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plNom) {
				XmlElement el=doc.CreateElement("plNom");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormPlGen f in this.plGen) {
				XmlElement el=doc.CreateElement("plGen");
				el.SetAttribute("default", f.value);
				el.SetAttribute("strength", (f.strength==Strength.Strong ? "strong" : "weak"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plVoc) {
				XmlElement el=doc.CreateElement("plVoc");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.count) {
				XmlElement el=doc.CreateElement("count");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			return doc;
		}
	}

}
