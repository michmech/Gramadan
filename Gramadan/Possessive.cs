using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Gramadan
{
	//A possessive pronoun:
	public class Possessive
	{
		public string disambig="";
		public string getNickname() {
			string ret=getLemma();
			if(this.disambig!="") ret+=" "+this.disambig;
			ret+=" poss";
			ret=ret.Replace(" ", "_");
			return ret;
		}
		public string getFriendlyNickname()
		{
			string ret=getLemma();
			if(this.disambig!="") ret+=" ("+this.disambig+")";
			return ret;
		}
		
		//Its forms:
		public List<Form> full=new List<Form>(); //the full form: mo, do, ...
		public List<Form> apos=new List<Form>(); //the apostrofied form, if any: m', d', ...

		//The mutation it causes:
		public Mutation mutation=Mutation.None;

		//The emphasizer it puts on emphasized nouns:
		public Emphasizer emphasizer = Emphasizer.SaSe;

        //Returns the noun's lemma:
        public string getLemma()
        {
            string ret="";
            Form lemmaForm=this.full[0];
            if(lemmaForm!=null) {
                ret=lemmaForm.value;
            }
            return ret;
        }

		//Constructors:
		public Possessive(string s, Mutation mutation)
		{
			this.full.Add(new Form(s));
			this.apos.Add(new Form(s));
			this.mutation=mutation;
		}
		public Possessive(string full, string apos, Mutation mutation)
		{
			this.full.Add(new Form(full));
			this.apos.Add(new Form(apos));
			this.mutation=mutation;
		}
		public Possessive(XmlDocument doc)
		{
			this.disambig=doc.DocumentElement.GetAttribute("disambig");
			this.mutation=(Mutation)Enum.Parse(typeof(Mutation), Utils.UpperInit(doc.DocumentElement.GetAttribute("mutation")));
			this.emphasizer=(Emphasizer)Enum.Parse(typeof(Emphasizer), Utils.UpperInit(doc.DocumentElement.GetAttribute("emphasizer")));
			foreach(XmlElement el in doc.SelectNodes("/*/full")) {
				this.full.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/apos")) {
				this.apos.Add(new Form(el.GetAttribute("default")));
			}
		}
		public Possessive(string fileName) : this(Utils.LoadXml(fileName)) { }
		
		//Prints the possessive pronoun in BuNaMo format:
		public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<possessive/>");
			doc.DocumentElement.SetAttribute("default", this.getLemma());
			doc.DocumentElement.SetAttribute("disambig", this.disambig);
			doc.DocumentElement.SetAttribute("mutation", Utils.LowerInit(this.mutation.ToString()));
			doc.DocumentElement.SetAttribute("emphasizer", Utils.LowerInit(this.emphasizer.ToString()));
			foreach(Form f in this.full) {
				XmlElement el=doc.CreateElement("full");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.apos) {
				XmlElement el=doc.CreateElement("apos");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			return doc;
		}
	}

}
