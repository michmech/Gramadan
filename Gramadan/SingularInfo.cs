using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gramadan
{
	//A class that encapsulates the singular forms of a noun or adjective:
	public class SingularInfo
	{
		public Gender gender;
		public List<Form> nominative=new List<Form>();
		public List<Form> genitive=new List<Form>();
		public List<Form> vocative=new List<Form>();
		public List<Form> dative=new List<Form>();

		public string print()
		{
			string ret="";
			ret+="NOM: "; foreach(Form f in this.nominative) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="GEN: "; foreach(Form f in this.genitive) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="VOC: "; foreach(Form f in this.vocative) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="DAT: "; foreach(Form f in this.dative) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			return ret;
		}
	}

	//Singular class O: all cases are identical.
	public class SingularInfoO : SingularInfo
	{
		public SingularInfoO(string lemma, Gender gender)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.genitive.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));
		}
	}

	//Singular class C: genitive and vocative formed by slenderization.
	public class SingularInfoC : SingularInfo
	{
		public SingularInfoC(string lemma, Gender gender) : this(lemma, gender, "") {}
		public SingularInfoC(string lemma, Gender gender, string slenderizationTarget)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive and assign the vocative:
			string form=lemma;
			form=Regex.Replace(form, "ch$", "gh"); //eg. bacach > bacaigh
			form=Opers.Slenderize(form, slenderizationTarget);
			if(gender==Gender.Fem) this.vocative.Add(new Form(lemma)); else this.vocative.Add(new Form(form));
			
			//derive and assign the genitive:
			if(gender==Gender.Fem) form=Regex.Replace(form, "igh$", "í"); //eg. cailleach > cailleaí
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class L: genitive formed by broadening.
	public class SingularInfoL : SingularInfo
	{
		public SingularInfoL(string lemma, Gender gender) : this(lemma, gender, "") { }
		public SingularInfoL(string lemma, Gender gender, string broadeningTarget)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive the genitive:
			string form=lemma;
			form=Opers.Broaden(form, broadeningTarget);
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class E: genitive formed by suffix "-e".
	public class SingularInfoE : SingularInfo
	{
		public SingularInfoE(string lemma, Gender gender, bool syncope, bool doubleDative) : this(lemma, gender, syncope, doubleDative, "") { }
		public SingularInfoE(string lemma, Gender gender, bool syncope, bool doubleDative, string slenderizationTarget)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));

			//derive the dative:
			string form=lemma;
			if(syncope) form=Opers.Syncope(form);
			form=Opers.Slenderize(form, slenderizationTarget);
			if(!doubleDative) {
				this.dative.Add(new Form(lemma));
			} else {
				this.dative.Add(new Form(lemma));
				this.dative.Add(new Form(form));
			}

			//continue deriving the genitive:
			form=Regex.Replace(form, "(["+Opers.VowelsSlender+"])ngt$", "$1ngth"); //eg. tarraingt > tarraingthe
			form=Regex.Replace(form, "ú$", "aith"); //eg. scrúdú > scrúdaithe
			form=form+"e";
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class A: genitive formed by suffix "-a".
	public class SingularInfoA : SingularInfo
	{
		public SingularInfoA(string lemma, Gender gender, bool syncope) : this(lemma, gender, syncope, "") { }
		public SingularInfoA(string lemma, Gender gender, bool syncope, string broadeningTarget)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive the genitive:
			string form=lemma;
			form=Regex.Replace(form, "(["+Opers.VowelsSlender+"])rt$", "$1rth"); //eg. bagairt > bagartha
			form=Regex.Replace(form, "(["+Opers.VowelsSlender+"])nnt$", "$1nn"); //eg. cionnroinnt > cionnranna
			form=Regex.Replace(form, "(["+Opers.VowelsSlender+"])nt$", "$1n"); //eg. canúint > canúna
			if(syncope) form=Opers.Syncope(form);
			form=Opers.Broaden(form, broadeningTarget);
			form=form+"a";
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class D: genitive ends in "-d".
	public class SingularInfoD : SingularInfo
	{
		public SingularInfoD(string lemma, Gender gender)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive the genitive:
			string form=lemma;
			form=Regex.Replace(form, "(["+Opers.VowelsBroad+"])$", "$1d"); //eg. cara > carad
			form=Regex.Replace(form, "(["+Opers.VowelsSlender+"])$", "$1ad"); //eg. fiche > fichead
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class N: genitive ends in "-n".
	public class SingularInfoN : SingularInfo
	{
		public SingularInfoN(string lemma, Gender gender)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive the genitive:
			string form=lemma;
			form=Regex.Replace(form, "(["+Opers.VowelsBroad+"])$", "$1n");
			form=Regex.Replace(form, "(["+Opers.VowelsSlender+"])$", "$1an");
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class EAX: genitive formed by suffix "-each".
	public class SingularInfoEAX : SingularInfo
	{
		public SingularInfoEAX(string lemma, Gender gender, bool syncope) : this(lemma, gender, syncope, "") { }
		public SingularInfoEAX(string lemma, Gender gender, bool syncope, string slenderizationTarget)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive the genitive:
			string form=lemma;
			if(syncope) form=Opers.Syncope(form);
			form=Opers.Slenderize(form, slenderizationTarget);
			form=form+"each";
			this.genitive.Add(new Form(form));
		}
	}

	//Singular class AX: genitive formed by suffix "-ach".
	public class SingularInfoAX : SingularInfo
	{
		public SingularInfoAX(string lemma, Gender gender, bool syncope) : this(lemma, gender, syncope, "") { }
		public SingularInfoAX(string lemma, Gender gender, bool syncope, string broadeningTarget)
		{
			this.gender=gender;
			this.nominative.Add(new Form(lemma));
			this.vocative.Add(new Form(lemma));
			this.dative.Add(new Form(lemma));

			//derive the genitive:
			string form=lemma;
			if(syncope) form=Opers.Syncope(form);
			form=Opers.Broaden(form, broadeningTarget);
			form=form+"ach";
			this.genitive.Add(new Form(form));
		}
	}

}
