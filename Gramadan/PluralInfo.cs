using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gramadan
{
	public class PluralInfo
	{
		public Strength strength;
		public List<Form> nominative=new List<Form>();
		public List<Form> genitive=new List<Form>();
		public List<Form> vocative=new List<Form>();

		public string print()
		{
			string ret="";
			ret+="NOM: "; foreach(Form f in this.nominative) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="GEN: "; foreach(Form f in this.genitive) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="VOC: "; foreach(Form f in this.vocative) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			return ret;
		}
	}

	//Plural class LgC: weak, plural formed by slenderization.
	public class PluralInfoLgC : PluralInfo
	{
		public PluralInfoLgC(string bayse) : this(bayse, "") {}
		public PluralInfoLgC(string bayse, string slenderizationTarget)
		{
			this.strength=Strength.Weak;

			//generate the genitive:
			string form=Opers.Broaden(bayse);
			this.genitive.Add(new Form(form));
			
			//generate the vocative:
			form=form+"a";
			this.vocative.Add(new Form(form));

			//generate the nominative:
			form=bayse;
			form=Regex.Replace(form, "ch$", "gh"); //eg. bacach > bacaigh
			form=Opers.Slenderize(form, slenderizationTarget);
			this.nominative.Add(new Form(form));
		}
	}

	//Plural class LgE: weak, plural formed by suffix "-e".
	public class PluralInfoLgE : PluralInfo
	{ 
		public PluralInfoLgE(string bayse) : this(bayse, "") {}
		public PluralInfoLgE(string bayse, string slenderizationTarget)
		{
			this.strength=Strength.Weak;
			
			string form=bayse;
			form=Opers.Slenderize(form, slenderizationTarget)+"e";

			this.nominative.Add(new Form(form));
			this.genitive.Add(new Form(Opers.Broaden(bayse)));
			this.vocative.Add(new Form(form));
		}
	}

	//Plural class LgA: weak, plural formed by suffix "-a".
	public class PluralInfoLgA : PluralInfo
	{
		public PluralInfoLgA(string bayse) : this(bayse, "") {}
		public PluralInfoLgA(string bayse, string broadeningTarget)
		{
			this.strength=Strength.Weak;
			
			string form=bayse;
			form=Opers.Broaden(form, broadeningTarget)+"a";

			this.nominative.Add(new Form(form));
			this.genitive.Add(new Form(Opers.Broaden(bayse)));
			this.vocative.Add(new Form(form));
		}
	}

	//Plural class Tr: strong.
	public class PluralInfoTr : PluralInfo
	{
		public PluralInfoTr(string bayse)
		{
			this.strength=Strength.Strong;
			this.nominative.Add(new Form(bayse));
			this.genitive.Add(new Form(bayse));
			this.vocative.Add(new Form(bayse));
		}
	}

}
