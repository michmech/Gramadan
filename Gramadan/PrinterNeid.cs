using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Gramadan
{
    public class PrinterNeid
    {
		public PrinterNeid(bool withXmlDeclarations) { this.withXmlDeclarations=withXmlDeclarations; }
		public PrinterNeid() : this(true) { }
		public bool withXmlDeclarations=true;
		
		public string printNounXml(Noun n)
        {
            NP np=new NP(n);
            string nl=Environment.NewLine;

            string ret="";
            if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
            ret+="<Lemma lemma='"+clean4xml(n.getLemma())+"' uid='"+clean4xml(n.getNickname())+"'>"+nl;
            ret+="<noun gender='"+(n.getGender()==Gender.Masc ? "masc" : "fem")+"' declension='"+n.declension+"'>"+nl;
            //Singular nominative:
            for(int i=0; i<Math.Max(np.sgNom.Count, np.sgNomArt.Count); i++) {
                ret+="\t<sgNom>"+nl;
                if(np.sgNom.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.sgNom[i].value)+"</articleNo>"+nl;
                if(np.sgNomArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.sgNomArt[i].value)+"</articleYes>"+nl;
                ret+="\t</sgNom>"+nl;
            }
            //Singular genitive:
            for(int i=0; i<Math.Max(np.sgGen.Count, np.sgGenArt.Count); i++) {
                ret+="\t<sgGen>"+nl;
                if(np.sgGen.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.sgGen[i].value)+"</articleNo>"+nl;
                if(np.sgGenArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.sgGenArt[i].value)+"</articleYes>"+nl;
                ret+="\t</sgGen>"+nl;
            }
            //Plural nominative:
            for(int i=0; i<Math.Max(np.plNom.Count, np.plNomArt.Count); i++) {
                ret+="\t<plNom>"+nl;
                if(np.plNom.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.plNom[i].value)+"</articleNo>"+nl;
                if(np.plNomArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.plNomArt[i].value)+"</articleYes>"+nl;
                ret+="\t</plNom>"+nl;
            }
            //Plural genitive:
            for(int i=0; i<Math.Max(np.plGen.Count, np.plGenArt.Count); i++) {
                ret+="\t<plGen>"+nl;
                if(np.plGen.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.plGen[i].value)+"</articleNo>"+nl;
                if(np.plGenArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.plGenArt[i].value)+"</articleYes>"+nl;
                ret+="\t</plGen>"+nl;
            }
            ret+="</noun>"+nl;
            ret+="</Lemma>";
            return ret;
        }
        public string printAdjectiveXml(Adjective a)
        {
            string ret="";
            string nl=Environment.NewLine;
			if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
			ret+="<Lemma lemma='"+clean4xml(a.getLemma())+"' uid='"+clean4xml(a.getNickname())+"'>"+nl;
			ret+="<adjective declension='"+a.declension+"'>"+nl;
            foreach(Form f in a.sgNom) ret+="\t<sgNomMasc>"+clean4xml(f.value)+"</sgNomMasc>"+nl;
			foreach(Form f in a.sgNom) ret+="\t<sgNomFem>"+clean4xml(Opers.Mutate(Mutation.Len1, f.value))+"</sgNomFem>"+nl;
			foreach(Form f in a.sgGenMasc) ret+="\t<sgGenMasc>"+clean4xml(Opers.Mutate(Mutation.Len1, f.value))+"</sgGenMasc>"+nl;
            foreach(Form f in a.sgGenFem) ret+="\t<sgGenFem>"+clean4xml(f.value)+"</sgGenFem>"+nl;
            foreach(Form f in a.plNom) ret+="\t<plNom>"+clean4xml(f.value)+"</plNom>"+nl;
			foreach(Form f in a.plNom) ret+="\t<plNomSlen>"+clean4xml(Opers.Mutate(Mutation.Len1, f.value))+"</plNomSlen>"+nl;
			foreach(Form f in a.plNom) ret+="\t<plGenStrong>"+clean4xml(f.value)+"</plGenStrong>"+nl;
            foreach(Form f in a.sgNom) ret+="\t<plGenWeak>"+clean4xml(f.value)+"</plGenWeak>"+nl;
            foreach(Form f in a.getComparPres()) ret+="\t<comparPres>"+clean4xml(f.value)+"</comparPres>"+nl;
            foreach(Form f in a.getComparPast()) ret+="\t<comparPast>"+clean4xml(f.value)+"</comparPast>"+nl;
            foreach(Form f in a.getSuperPres()) ret+="\t<superPres>"+clean4xml(f.value)+"</superPres>"+nl;
            foreach(Form f in a.getSuperPast()) ret+="\t<superPast>"+clean4xml(f.value)+"</superPast>"+nl;
			foreach(Form f in a.abstractNoun) ret+="\t<abstractNoun>"+clean4xml(f.value)+"</abstractNoun>"+nl;
			foreach(Form f in a.abstractNoun) {
				ret+="\t<abstractNounExamples>"+nl;
				ret+="\t\t<example>"+clean4xml("dá "+Opers.Mutate(Mutation.Len1, f.value))+"</example>"+nl;
				if(Regex.IsMatch(f.value, "^["+Opers.Vowels+"]"))
					ret+="\t\t<example>"+clean4xml("ag dul in "+Opers.Mutate(Mutation.None, f.value))+"</example>"+nl;
				else
					ret+="\t\t<example>"+clean4xml("ag dul i "+Opers.Mutate(Mutation.Ecl1, f.value))+"</example>"+nl;
				ret+="\t</abstractNounExamples>"+nl;
			}
			ret+="</adjective>"+nl;
            ret+="</Lemma>";
            return ret;
        }
        public string printNPXml(NP np)
        {
            string nl=Environment.NewLine;

            string ret="";
			if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
			ret+="<Lemma lemma='"+clean4xml(np.getLemma())+"' uid='"+clean4xml(np.getNickname())+"'>"+nl;
			ret+="<nounPhrase";
			if(np.hasGender()) ret+=" gender='"+(np.getGender()==Gender.Masc ? "masc" : "fem")+"'";
			ret+=" forceNominative='"+(np.forceNominative ? "1" : "0")+"'";
			ret+=" isPossessed='"+(np.isPossessed ? "1" : "0")+"'";
			ret+=">"+nl;
            //Singular nominative:
            for(int i=0; i<Math.Max(np.sgNom.Count, np.sgNomArt.Count); i++) {
                ret+="\t<sgNom>"+nl;
                if(np.sgNom.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.sgNom[i].value)+"</articleNo>"+nl;
                if(np.sgNomArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.sgNomArt[i].value)+"</articleYes>"+nl;
                ret+="\t</sgNom>"+nl;
            }
            //Singular genitive:
            for(int i=0; i<Math.Max(np.sgGen.Count, np.sgGenArt.Count); i++) {
                ret+="\t<sgGen>"+nl;
                if(np.sgGen.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.sgGen[i].value)+"</articleNo>"+nl;
                if(np.sgGenArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.sgGenArt[i].value)+"</articleYes>"+nl;
                ret+="\t</sgGen>"+nl;
            }
            //Plural nominative:
            for(int i=0; i<Math.Max(np.plNom.Count, np.plNomArt.Count); i++) {
                ret+="\t<plNom>"+nl;
                if(np.plNom.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.plNom[i].value)+"</articleNo>"+nl;
                if(np.plNomArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.plNomArt[i].value)+"</articleYes>"+nl;
                ret+="\t</plNom>"+nl;
            }
            //Plural genitive:
            for(int i=0; i<Math.Max(np.plGen.Count, np.plGenArt.Count); i++) {
                ret+="\t<plGen>"+nl;
                if(np.plGen.Count>i) ret+="\t\t<articleNo>"+clean4xml(np.plGen[i].value)+"</articleNo>"+nl;
                if(np.plGenArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(np.plGenArt[i].value)+"</articleYes>"+nl;
                ret+="\t</plGen>"+nl;
            }
            ret+="</nounPhrase>"+nl;
            ret+="</Lemma>";
            return ret;
        }
		public string printPPXml(PP pp)
		{
			string nl=Environment.NewLine;

			string ret="";
			if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
			ret+="<Lemma lemma='"+clean4xml(pp.getLemma())+"' uid='"+clean4xml(pp.getNickname())+"'>"+nl;
			ret+="<prepositionalPhrase>"+nl;
			//Singular nominative:
			for(int i=0; i<Math.Max(pp.sg.Count, Math.Max(pp.sgArtS.Count, pp.sgArtN.Count)); i++) {
				ret+="\t<sg>"+nl;
				if(pp.sg.Count>i) ret+="\t\t<articleNo>"+clean4xml(pp.sg[i].value)+"</articleNo>"+nl;
				if(pp.sgArtS.Count>i && pp.sgArtN.Count>i) {
					if(pp.sgArtS[i].value==pp.sgArtN[i].value) {
						ret+="\t\t<articleYes>"+clean4xml(pp.sgArtS[i].value)+"</articleYes>"+nl;
					} else {
						ret+="\t\t<articleYes var='south'>"+clean4xml(pp.sgArtS[i].value)+"</articleYes>"+nl;
						ret+="\t\t<articleYes var='north'>"+clean4xml(pp.sgArtN[i].value)+"</articleYes>"+nl;
					}
				} else {
					if(pp.sgArtS.Count>i) ret+="\t\t<articleYes>"+clean4xml(pp.sgArtS[i].value)+"</articleYes>"+nl;
					if(pp.sgArtN.Count>i) ret+="\t\t<articleYes>"+clean4xml(pp.sgArtN[i].value)+"</articleYes>"+nl;	
				}
				ret+="\t</sg>"+nl;
			}
			//Plural nominative:
			for(int i=0; i<Math.Max(pp.pl.Count, pp.plArt.Count); i++) {
				ret+="\t<pl>"+nl;
				if(pp.pl.Count>i) ret+="\t\t<articleNo>"+clean4xml(pp.pl[i].value)+"</articleNo>"+nl;
				if(pp.plArt.Count>i) ret+="\t\t<articleYes>"+clean4xml(pp.plArt[i].value)+"</articleYes>"+nl;
				ret+="\t</pl>"+nl;
			}
			ret+="</prepositionalPhrase>"+nl;
			ret+="</Lemma>";
			return ret;
		}
		public string printPrepositionXml(Preposition p)
		{
			string nl=Environment.NewLine;

			string ret="";
			if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
			ret+="<Lemma lemma='"+clean4xml(p.lemma)+"' uid='"+clean4xml(p.getNickname())+"'>"+nl;
			ret+="<preposition>"+nl;
			//Inflected forms:
			for(int i=0; i<p.sg1.Count; i++) ret+="\t<persSg1>"+p.sg1[i].value+"</persSg1>"+nl;
			for(int i=0; i<p.sg2.Count; i++) ret+="\t<persSg2>"+p.sg2[i].value+"</persSg2>"+nl;
			for(int i=0; i<p.sg3Masc.Count; i++) ret+="\t<persSg3Masc>"+p.sg3Masc[i].value+"</persSg3Masc>"+nl;
			for(int i=0; i<p.sg3Fem.Count; i++) ret+="\t<persSg3Fem>"+p.sg3Fem[i].value+"</persSg3Fem>"+nl;
			for(int i=0; i<p.pl1.Count; i++) ret+="\t<persPl1>"+p.pl1[i].value+"</persPl1>"+nl;
			for(int i=0; i<p.pl2.Count; i++) ret+="\t<persPl2>"+p.pl2[i].value+"</persPl2>"+nl;
			for(int i=0; i<p.pl3.Count; i++) ret+="\t<persPl3>"+p.pl3[i].value+"</persPl3>"+nl;
			ret+="</preposition>"+nl;
			ret+="</Lemma>";
			return ret;
		}
		public string printVerbXml(Verb v)
		{
			string nl=Environment.NewLine;

			string ret="";
			if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
			ret+="<Lemma lemma='"+clean4xml(v.getLemma())+"' uid='"+clean4xml(v.getNickname())+"'>"+nl;
			ret+="<verb>"+nl;
			for(int i=0; i<v.verbalNoun.Count; i++) ret+="\t<vn>"+clean4xml(v.verbalNoun[i].value)+"</vn>"+nl;
			for(int i=0; i<v.verbalAdjective.Count; i++) ret+="\t<va>"+clean4xml(v.verbalAdjective[i].value)+"</va>"+nl;

			Dictionary<string, VPTense> mapTense=new Dictionary<string, VPTense>();
			mapTense.Add("past", VPTense.Past);
			if(v.getLemma()=="bí") {
				mapTense.Add("present", VPTense.Pres);
				mapTense.Add("presentConti", VPTense.PresCont);
			} else {
				mapTense.Add("present", VPTense.PresCont);
			}
			mapTense.Add("future", VPTense.Fut);
			mapTense.Add("condi", VPTense.Cond);
			mapTense.Add("pastConti", VPTense.PastCont);
			
			Dictionary<string, VPPerson> mapPerson=new Dictionary<string, VPPerson>();
			mapPerson.Add("sg1", VPPerson.Sg1);
			mapPerson.Add("sg2", VPPerson.Sg2);
			mapPerson.Add("sg3Masc", VPPerson.Sg3Masc);
			mapPerson.Add("sg3Fem", VPPerson.Sg3Fem);
			mapPerson.Add("pl1", VPPerson.Pl1);
			mapPerson.Add("pl2", VPPerson.Pl2);
			mapPerson.Add("pl3", VPPerson.Pl3);
			mapPerson.Add("auto", VPPerson.Auto);

			VP vp=new VP(v);

			foreach(string tense in mapTense.Keys) {
				ret+="\t<"+tense+">"+nl;
				foreach(string pers in mapPerson.Keys) {
					ret+="\t\t<"+pers+">"+nl;
					foreach(Form form in vp.tenses[mapTense[tense]][VPShape.Declar][mapPerson[pers]][VPPolarity.Pos]) {
						ret+="\t\t\t<pos>"+clean4xml(form.value)+"</pos>"+nl;
					}
					foreach(Form form in vp.tenses[mapTense[tense]][VPShape.Interrog][mapPerson[pers]][VPPolarity.Pos]) {
						ret+="\t\t\t<quest>"+clean4xml(form.value)+"?</quest>"+nl;
					}
					foreach(Form form in vp.tenses[mapTense[tense]][VPShape.Declar][mapPerson[pers]][VPPolarity.Neg]) {
						ret+="\t\t\t<neg>"+clean4xml(form.value)+"</neg>"+nl;
					}
					ret+="\t\t</"+pers+">"+nl;
				}
				ret+="\t</"+tense+">"+nl;
			}

			Dictionary<string, VPMood> mapMood=new Dictionary<string, VPMood>();
			mapMood.Add("imper", VPMood.Imper);
			mapMood.Add("subj", VPMood.Subj);

			foreach(string mood in mapMood.Keys) {
				ret+="\t<"+mood+">"+nl;
				foreach(string pers in mapPerson.Keys) {
					ret+="\t\t<"+pers+">"+nl;
					foreach(Form form in vp.moods[mapMood[mood]][mapPerson[pers]][VPPolarity.Pos]) {
						ret+="\t\t\t<pos>"+clean4xml(form.value)+(mapMood[mood]==VPMood.Imper ? "!" : "")+"</pos>"+nl;
					}
					foreach(Form form in vp.moods[mapMood[mood]][mapPerson[pers]][VPPolarity.Neg]) {
						ret+="\t\t\t<neg>"+clean4xml(form.value)+(mapMood[mood]==VPMood.Imper ? "!" : "")+"</neg>"+nl;
					}
					ret+="\t\t</"+pers+">"+nl;
				}
				ret+="\t</"+mood+">"+nl;
			}

			ret+="</verb>"+nl;
			ret+="</Lemma>";
			return ret;
		}
		public string printPossessiveXml(Possessive p)
		{
			string nl=Environment.NewLine;

			string ret="";
			if(this.withXmlDeclarations) ret+="<?xml version='1.0' encoding='utf-8'?>"+nl;
			if(this.withXmlDeclarations) ret+="<?xml-stylesheet type='text/xsl' href='!gram.xsl'?>"+nl;
			ret+="<Lemma lemma='"+clean4xml(p.getLemma())+"' uid='"+clean4xml(p.getNickname())+"'>"+nl;
			ret+="<possessive disambig='"+p.disambig+"'>"+nl;
			//Forms:
			for(int i=0; i<p.full.Count; i++) ret+="\t<full>"+p.full[i].value+"</full>"+nl;
			for(int i=0; i<p.apos.Count; i++) ret+="\t<apos>"+p.apos[i].value+"</apos>"+nl;
			ret+="</possessive>"+nl;
			ret+="</Lemma>";
			return ret;
		}

        private string clean4xml(string text)
        {
            string ret = text;
            ret = ret.Replace("&", "&amp;");
            ret = ret.Replace("\"", "&quot;");
            ret = ret.Replace("'", "&apos;");
            ret = ret.Replace("<", "&lt;");
            ret = ret.Replace(">", "&gt;");
            return ret;
        }
    }
}
