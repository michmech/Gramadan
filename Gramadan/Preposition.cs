using System;
using System.Collections.Generic;
using System.Xml;

namespace Gramadan
{
	//A class for a preposition:
	public class Preposition
	{
		public string disambig="";
		public string getNickname()
		{
			string ret=this.lemma+" prep";
			if(this.disambig!="") ret+=" "+this.disambig;
			ret=ret.Replace(" ", "_");
			return ret;
		}

		//The lemma:
		public string lemma;
		public string getLemma() {return this.lemma;}

		//Inflected forms (for number, person and gender):
		public List<Form> sg1=new List<Form>();
		public List<Form> sg2=new List<Form>();
		public List<Form> sg3Masc=new List<Form>();
		public List<Form> sg3Fem=new List<Form>();
		public List<Form> pl1=new List<Form>();
		public List<Form> pl2=new List<Form>();
		public List<Form> pl3=new List<Form>();

		//Returns true if the preposition has no infected forms:
		public bool isEmpty()
		{
			return this.sg1.Count+this.sg2.Count+this.sg3Masc.Count+this.sg3Fem.Count+this.pl1.Count+this.pl2.Count+this.pl3.Count==0;
		}

		//Constructor:
		public Preposition(string lemma, string sg1, string sg2, string sg3Masc, string sg3Fem, string pl1, string pl2, string pl3)
		{
			this.lemma=lemma;
			if(sg1!="") this.sg1.Add(new Form(sg1));
			if(sg2!="") this.sg2.Add(new Form(sg2));
			if(sg3Masc!="") this.sg3Masc.Add(new Form(sg3Masc));
			if(sg3Fem!="") this.sg3Fem.Add(new Form(sg3Fem));
			if(pl1!="") this.pl1.Add(new Form(pl1));
			if(pl2!="") this.pl2.Add(new Form(pl2));
			if(pl3!="") this.pl3.Add(new Form(pl3));
		}
		public Preposition(XmlDocument doc)
		{
			this.lemma=doc.DocumentElement.GetAttribute("default");
			this.disambig=doc.DocumentElement.GetAttribute("disambig");
			foreach(XmlElement el in doc.SelectNodes("/*/sg1")) this.sg1.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sg2")) this.sg2.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sg3Masc")) this.sg3Masc.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/sg3Fem")) this.sg3Fem.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/pl1")) this.pl1.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/pl2")) this.pl2.Add(new Form(el.GetAttribute("default")));
			foreach(XmlElement el in doc.SelectNodes("/*/pl3")) this.pl3.Add(new Form(el.GetAttribute("default")));
		}
		public Preposition(string fileName) : this(Utils.LoadXml(fileName)) { }

		//Prints the preposition in BuNaMo format:
		public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<preposition/>");
			doc.DocumentElement.SetAttribute("default", this.lemma);
			doc.DocumentElement.SetAttribute("disambig", this.disambig);
			foreach(Form f in this.sg1) {
				XmlElement el=doc.CreateElement("sg1");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sg2) {
				XmlElement el=doc.CreateElement("sg2");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sg3Masc) {
				XmlElement el=doc.CreateElement("sg3Masc");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.sg3Fem) {
				XmlElement el=doc.CreateElement("sg3Fem");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.pl1) {
				XmlElement el=doc.CreateElement("pl1");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.pl2) {
				XmlElement el=doc.CreateElement("pl2");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.pl3) {
				XmlElement el=doc.CreateElement("pl3");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			return doc;
		}
	}
}
