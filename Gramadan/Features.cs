using System;
using System.Collections.Generic;

namespace Gramadan
{
	//Enumerations for various grammatical features:
	public enum Mutation { None, Len1, Len2, Len3, Ecl1, Ecl1x, Ecl2, Ecl3, PrefT, PrefH, Len1D, Len2D, Len3D }
	public enum Strength { Strong, Weak }
	public enum Number { Sg, Pl }
	public enum Gender { Masc, Fem }
    public enum Emphasizer { SaSe, SanSean, NaNe }

	//Encapsulates a word form, a phrase form or a clause form:
	public class Form
	{
		public string value;
		public Form(string value) { this.value=value; }
	}

    //Class for noun and noun phrase forms in the singular:
    public class FormSg : Form
    {
        public Gender gender; //in the singular, a noun form has gender
        public FormSg(string value, Gender gender) : base(value) { this.gender=gender; }
    }

    //Class for noun forms in the plural genitive:
    public class FormPlGen : Form
    {
        public Strength strength; //in the plural genitive, a noun form has strength.
        public FormPlGen(string value, Strength strength) : base(value) { this.strength=strength; }
    }
}
