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
		public List<Form> full=new List<Form>();
		public List<Form> apos=new List<Form>();

		//The mutaion it causes:
		public Mutation mutation=Mutation.None;

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
