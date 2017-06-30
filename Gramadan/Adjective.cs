using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace Gramadan
{
	//An adjective:
	public class Adjective
	{
		public string disambig="";
		public string getNickname()
		{
			string ret=getLemma();
			ret+=" adj";
			ret+=(this.declension>0 ? this.declension.ToString() : "");
			if(this.disambig!="") ret+=" "+this.disambig;
			ret=ret.Replace(" ", "_");
			return ret;
		}
		public string getFriendlyNickname()
		{
			string ret=getLemma();
			ret+=" (adj";
			ret+=(this.declension>0 ? this.declension.ToString() : "");
			if(this.disambig!="") ret+=" "+this.disambig;
			ret+=")";
			return ret;
		}

		//The adjective's traditional declension class (not actually used for anything); default is 0 meaning none or unknown:
		public int declension=0;

		//Forms of the adjective:
		public List<Form> sgNom=new List<Form>();
		public List<Form> sgGenMasc=new List<Form>();
		public List<Form> sgGenFem=new List<Form>();
		public List<Form> sgVocMasc=new List<Form>();
		public List<Form> sgVocFem=new List<Form>();

		//Adjective forms in the plural:
		public List<Form> plNom=new List<Form>();

        //Form for grading:
        public List<Form> graded=new List<Form>();

		//Related abstract noun:
		public List<Form> abstractNoun=new List<Form>();

		//Whether the adjective is a prefix (like "sean"):
		public bool isPre=false;

        //Returns the adjective's lemma:
        public string getLemma()
        {
            string ret="";
            Form lemmaForm=this.sgNom[0];
            if(lemmaForm!=null) {
                ret=lemmaForm.value;
            }
            return ret;
        }

        //These return graded forms of the adjective:
        public List<Form> getComparPres() //comparative present, eg. "níos mó"
        {
            List<Form> ret=new List<Form>();
            foreach(Form gradedForm in graded) {
                ret.Add(new Form("níos "+gradedForm.value));
            }
            return ret;
        }
        public List<Form> getSuperPres() //superlative present, eg. "is mó"
        {
            List<Form> ret=new List<Form>();
            foreach(Form gradedForm in graded) {
                ret.Add(new Form("is "+gradedForm.value));
            }
            return ret;
        }
        public List<Form> getComparPast() //comparative past/conditional, eg. "ní ba mhó"
        {
            List<Form> ret=new List<Form>();
            foreach(Form gradedForm in graded) {
                string form="";
                if(Regex.IsMatch(gradedForm.value, "^[aeiouáéíóúAEIOUÁÉÍÓÚ]")) form="ní b'"+gradedForm.value;
				else if(Regex.IsMatch(gradedForm.value, "^f[aeiouáéíóúAEIOUÁÉÍÓÚ]")) form="ní b'"+Opers.Mutate(Mutation.Len1, gradedForm.value);
                else form="ní ba "+Opers.Mutate(Mutation.Len1, gradedForm.value);
                ret.Add(new Form(form));
            }
            return ret;
        }
        public List<Form> getSuperPast() //superlative past/conditional, eg. "ba mhó"
        {
            List<Form> ret=new List<Form>();
            foreach(Form gradedForm in graded) {
                string form="";
                if(Regex.IsMatch(gradedForm.value, "^[aeiouáéíóúAEIOUÁÉÍÓÚ]")) form="ab "+gradedForm.value;
                else if(Regex.IsMatch(gradedForm.value, "^f")) form="ab "+Opers.Mutate(Mutation.Len1, gradedForm.value);
                else form="ba "+Opers.Mutate(Mutation.Len1, gradedForm.value);
                ret.Add(new Form(form));
            }
            return ret;
        }

		//Constructors:
		public Adjective() { }
		public Adjective(SingularInfo sgMasc, SingularInfo sgFem, string graded) : this(sgMasc, sgFem, null, graded) { }
		public Adjective(SingularInfo sgMasc, SingularInfo sgFem, string plural, string graded)
		{
			this.sgNom=sgMasc.nominative;
			this.sgGenMasc=sgMasc.genitive;
			this.sgGenFem=sgFem.genitive;
			this.sgVocMasc=sgMasc.vocative;
			this.sgVocFem=sgFem.vocative;
			if(plural!=null)  this.plNom.Add(new Form(plural));
            this.graded.Add(new Form(graded));
		}
		public Adjective(XmlDocument doc)
		{
			this.disambig=doc.DocumentElement.GetAttribute("disambig");
			int.TryParse(doc.DocumentElement.GetAttribute("declension"), out this.declension);
			bool.TryParse(doc.DocumentElement.GetAttribute("isPre"), out this.isPre);
			foreach(XmlElement el in doc.SelectNodes("/*/sgNom")) this.sgNom.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sgGenMasc")) this.sgGenMasc.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sgGenFem")) this.sgGenFem.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sgVocMasc")) this.sgVocMasc.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sgVocFem")) this.sgVocFem.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/plNom")) this.plNom.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/graded")) this.graded.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/abstractNoun")) this.abstractNoun.Add(new Form(el.GetAttribute("default")));
		}
		public Adjective(string fileName) : this(Utils.LoadXml(fileName)) { }

		//Prints the adjective in BuNaMo format:
		public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<adjective/>");
			doc.DocumentElement.SetAttribute("default", this.getLemma());
			doc.DocumentElement.SetAttribute("declension", this.declension.ToString());
			doc.DocumentElement.SetAttribute("disambig", this.disambig);
			doc.DocumentElement.SetAttribute("isPre", this.isPre.ToString());
			foreach(Form f in this.sgNom) {
				XmlElement el=doc.CreateElement("sgNom");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sgGenMasc) {
				XmlElement el=doc.CreateElement("sgGenMasc");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sgGenFem) {
				XmlElement el=doc.CreateElement("sgGenFem");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sgVocMasc) {
				XmlElement el=doc.CreateElement("sgVocMasc");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sgVocFem) {
				XmlElement el=doc.CreateElement("sgVocFem");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plNom) {
				XmlElement el=doc.CreateElement("plNom");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.graded) {
				XmlElement el=doc.CreateElement("graded");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.abstractNoun) {
				XmlElement el=doc.CreateElement("abstractNoun");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			return doc;
		}
	}
}
