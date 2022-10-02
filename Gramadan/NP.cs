using System;
using System.Collections.Generic;
using System.Xml;

namespace Gramadan
{
	//A class for a noun phrase:
	public class NP
	{
		public string disambig="";
		public string getNickname()
		{
			string ret=getLemma()+" NP";
			if(this.disambig!="") ret+=" "+this.disambig;
			ret=ret.Replace(" ", "_");
			return ret;
		}
		
		//Noun phrase forms in the singular, without article:
        public List<FormSg> sgNom=new List<FormSg>();
        public List<FormSg> sgGen=new List<FormSg>();
		public List<FormSg> sgDat=new List<FormSg>(); //head noun left unmutated

        //Noun phrase forms in the singular, with article:
        public List<FormSg> sgNomArt=new List<FormSg>();
        public List<FormSg> sgGenArt=new List<FormSg>();
		public List<FormSg> sgDatArtN=new List<FormSg>(); //northern system, as if with article but the article is *not* included, head noun unmutated
		public List<FormSg> sgDatArtS=new List<FormSg>(); //southern system, as if with article but the article is *not* included, head noun unmutated

        //Noun phrase forms in the plural, without article:
        public List<Form> plNom=new List<Form>();
        public List<Form> plGen=new List<Form>();
		public List<Form> plDat=new List<Form>(); //head noun left unmutated

        //Noun phrase forms in the plural, with article:
        public List<Form> plNomArt=new List<Form>();
        public List<Form> plGenArt=new List<Form>();
		public List<Form> plDatArt=new List<Form>(); //as if with article but the article is *not* included, head noun unmutated

        //Whether this noun phrase is definite:
        public bool isDefinite=false; //If true, the articleless forms are definite and there are no articled forms.
                                      //If false, the articleless forms are indefinite and the articled forms are definite.

		//Whether this noun phrase is determined by a possessive pronoun:
		public bool isPossessed=false; //if true, only sgNom, sgDat, sgGen, plNom, plDat, plGen exist, the others are empty.

		//Whether this NP's head noun cannot be mutated by prepositions:
		public bool isImmutable=false; //Eg. "blitz", dative "leis an blitz mhór"

		//Should the unarticled nominative be used in place of the unarticled genitive?
		public bool forceNominative=false;

        //Returns the noun phrase's lemma:
        public string getLemma()
        {
            string ret="";
			if(this.sgNom.Count!=0) ret=this.sgNom[0].value;
			if(ret=="" && this.sgNomArt.Count!=0) ret=this.sgNomArt[0].value;
			if(ret=="" && this.plNom.Count!=0) ret=this.plNom[0].value;
			if(ret=="" && this.plNomArt.Count!=0) ret=this.plNomArt[0].value;
			return ret;
        }

		//Returns the noun phrase's gender:
		public Gender getGender()
		{
			Gender ret=Gender.Masc;
			if(this.sgNom.Count!=0) ret=this.sgNom[0].gender;
			else if(this.sgNomArt.Count!=0) ret=this.sgNomArt[0].gender;
			return ret;
		}
		public bool hasGender() {
			bool ret=false;
			if(this.sgNom.Count!=0 || this.sgNomArt.Count!=0) ret=true;
			return ret;
		}

        //Prints a user-friendly summary of the noun phrase's forms:
        public string print()
        {
            string ret="";
            ret+="sgNom: "; foreach(Form f in this.sgNom) { ret+="["+f.value +"] "; } ret+=Environment.NewLine;
            ret+="sgGen: "; foreach(Form f in this.sgGen) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
            ret+="plNom: "; foreach(Form f in this.plNom) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
            ret+="plGen: "; foreach(Form f in this.plGen) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
            ret+="sgNomArt: "; foreach(Form f in this.sgNomArt) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
            ret+="sgGenArt: "; foreach(Form f in this.sgGenArt) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plNomArt: "; foreach(Form f in this.plNomArt) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plGenArt: "; foreach(Form f in this.plGenArt) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+=Environment.NewLine;
			ret+="sgDat: "; foreach(Form f in this.sgDat) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="sgDatArtN: "; foreach(Form f in this.sgDatArtN) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="sgDatArtS: "; foreach(Form f in this.sgDatArtS) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plDat: "; foreach(Form f in this.plDat) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
			ret+="plDatArt: "; foreach(Form f in this.plDatArt) { ret+="["+f.value+"] "; } ret+=Environment.NewLine;
            return ret;
        }

        //Creates a noun phrase from an explicit listing of all the basic forms:
        public NP(Gender gender, string sgNom, string sgGen, string plNom, string plGen, string sgDatArtN)
        {
            #region singular-nominative
            { //without article:
                this.sgNom.Add(new FormSg(sgNom, gender));
            }
            { //with article:
				Mutation mut=(gender==Gender.Masc ? Mutation.PrefT : Mutation.Len3);
				string value="an "+Opers.Mutate(mut, sgNom);
				this.sgNomArt.Add(new FormSg(value, gender));
            }
            #endregion
            #region singular-genitive
            { //without article:
                string value=sgNom;
                this.sgGen.Add(new FormSg(value, gender));
            }
            { //with article:
                Mutation mut=(gender==Gender.Masc ? Mutation.Len3 : Mutation.PrefH);
                string article=(gender==Gender.Masc ? "an" : "na");
                string value=article+" "+Opers.Mutate(mut, sgGen);
                this.sgGenArt.Add(new FormSg(value, gender));
            }
            #endregion
            #region plural-nominative
            { //without article:
                this.plNom.Add(new Form(plNom));
            }
            { //with article:
                string value="na "+Opers.Mutate(Mutation.PrefH, plNom);
                this.plNomArt.Add(new Form(value));
            }
            #endregion
            #region plural-genitive
            { //without article:
                this.plGen.Add(new Form(plNom));
            }
            { //with article:
                string value="na "+Opers.Mutate(Mutation.Ecl1, plGen);
                this.plGenArt.Add(new Form(value));
            }
            #endregion
			#region singular-dative
			{ //without article:
				this.sgDat.Add(new FormSg(sgNom, gender));
			}
			{ //with article:
				this.sgDatArtN.Add(new FormSg(sgDatArtN, gender));
				this.sgDatArtS.Add(new FormSg(sgNom, gender));

				Mutation mut=(gender==Gender.Masc ? Mutation.PrefT : Mutation.Len3);
				string value="an "+Opers.Mutate(mut, sgNom);
				this.sgNomArt.Add(new FormSg(value, gender));
			}
			#endregion
			#region plural-dative
			{ //without article:
				this.plDat.Add(new Form(plNom));
			}
			{ //with article:
				this.plDatArt.Add(new Form(plNom));
			}
			#endregion
		}
        
		//Creates a noun phrase from a noun:
        public NP(Noun head)
        {
            this.isDefinite=head.isDefinite;
			this.isImmutable=head.isImmutable;
            #region singular-nominative
            foreach(FormSg headForm in head.sgNom) {
                { //without article:
                    this.sgNom.Add(new FormSg(headForm.value, headForm.gender));
                }
                if(!head.isDefinite) { //with article:
                    Mutation mut=(headForm.gender==Gender.Masc ? Mutation.PrefT : Mutation.Len3);
					if(head.isImmutable) mut=Mutation.None;
                    string value="an "+Opers.Mutate(mut, headForm.value);
                    this.sgNomArt.Add(new FormSg(value, headForm.gender));
                }
            }
            #endregion
            #region singular-genitive
            foreach(FormSg headForm in head.sgGen) {
                { //without article:
                    Mutation mut=(head.isProper ? Mutation.Len1 : Mutation.None); //proper nouns are always lenited in the genitive
					if(head.isImmutable) mut=Mutation.None;
					string value=Opers.Mutate(mut, headForm.value);
                    this.sgGen.Add(new FormSg(value, headForm.gender));
                }
                //with article:
                if(!head.isDefinite || head.allowArticledGenitive) {
                    Mutation mut=(headForm.gender==Gender.Masc ? Mutation.Len3 : Mutation.PrefH);
					if(head.isImmutable) mut=Mutation.None;
                    string article=(headForm.gender==Gender.Masc ? "an" : "na");
                    string value=article+" "+Opers.Mutate(mut, headForm.value);
                    this.sgGenArt.Add(new FormSg(value, headForm.gender));
                }
            }
            #endregion
            #region plural-nominative
            foreach(Form headForm in head.plNom) {
                { //without article:
                    this.plNom.Add(new Form(headForm.value));
                }
                if(!head.isDefinite) { //with article:
					Mutation mut=Mutation.PrefH;
					if(head.isImmutable) mut=Mutation.None;
                    string value="na "+Opers.Mutate(mut, headForm.value);
                    this.plNomArt.Add(new Form(value));
                }
            }
            #endregion
            #region plural-genitive
            foreach(Form headForm in head.plGen) {
                { //without article:
                    Mutation mut=(head.isProper ? Mutation.Len1 : Mutation.None); //proper nouns are always lenited in the articleless genitive
					if(head.isImmutable) mut=Mutation.None;
					string value=Opers.Mutate(mut, headForm.value);
                    this.plGen.Add(new Form(value));
                }
                if(!head.isDefinite || head.allowArticledGenitive) { //with article:
					Mutation mut=Mutation.Ecl1;
					if(head.isImmutable) mut=Mutation.None;
					string value="na "+Opers.Mutate(mut, headForm.value);
                    this.plGenArt.Add(new Form(value));
                }
            }
            #endregion
			#region singular-dative
			foreach(FormSg headForm in head.sgDat) {
				{ //without article:
					this.sgDat.Add(new FormSg(headForm.value, headForm.gender));
				}
				if(!head.isDefinite) { //with article:
					this.sgDatArtN.Add(new FormSg(headForm.value, headForm.gender));
					this.sgDatArtS.Add(new FormSg(headForm.value, headForm.gender));
				}
			}
			#endregion
			#region plural-dative
			foreach(Form headForm in head.plNom) {
				{ //without article:
					this.plDat.Add(new Form(headForm.value));
				}
				if(!head.isDefinite) { //with article:
					this.plDatArt.Add(new Form(headForm.value));
				}
			}
			#endregion
		}

        //Creates a noun phrase from a noun modified by an adjective:
        public NP(Noun head, Adjective mod)
        {
			if(mod.isPre) {
				Noun prefixedHead=new Noun(head.printXml()); //create a copy of the head noun
				string prefix=mod.getLemma();
				foreach(Form f in prefixedHead.sgNom) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.sgGen) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.sgDat) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.sgVoc) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.plNom) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.plGen) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.plVoc) f.value=Opers.Prefix(prefix, f.value);
				foreach(Form f in prefixedHead.count) f.value=Opers.Prefix(prefix, f.value);
				NP np=new NP(prefixedHead);
				this.isDefinite=np.isDefinite;
				this.sgNom=np.sgNom; this.sgNomArt=np.sgNomArt;
				this.sgGen=np.sgGen; this.sgGenArt=np.sgGenArt;
				this.sgDat=np.sgDat; this.sgDatArtN=np.sgDatArtN; this.sgDatArtS=np.sgDatArtS;
				this.plNom=np.plNom; this.plNomArt=np.plNomArt;
				this.plGen=np.plGen; this.plGenArt=np.plGenArt;
				this.plDat=np.plDat; this.plDatArt=np.plDatArt;
			} else {
				this.isDefinite=head.isDefinite;
				this.isImmutable=head.isImmutable;
				this.forceNominative=true;
				#region singular-nominative
				foreach(FormSg headForm in head.sgNom) {
					{ //without article:
						foreach(Form modForm in mod.sgNom) {
							Mutation mutA=(headForm.gender==Gender.Masc ? Mutation.None : Mutation.Len1);
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.sgNom.Add(new FormSg(value, headForm.gender));
						}
					}
					if(!head.isDefinite) { //with article:
						foreach(Form modForm in mod.sgNom) {
							Mutation mutN=(headForm.gender==Gender.Masc ? Mutation.PrefT : Mutation.Len3);
							if(head.isImmutable) mutN=Mutation.None;
							Mutation mutA=(headForm.gender==Gender.Masc ? Mutation.None : Mutation.Len1);
							string value="an "+Opers.Mutate(mutN, headForm.value)+" "+Opers.Mutate(mutA, modForm.value);
							this.sgNomArt.Add(new FormSg(value, headForm.gender));
						}
					}
				}
				#endregion
				#region singular-genitive
				foreach(FormSg headForm in head.sgGen) {
					{ //without article:
						List<Form> modForms=(headForm.gender==Gender.Masc ? mod.sgGenMasc : mod.sgGenFem);
						foreach(Form modForm in modForms) {
							Mutation mutN=(head.isProper ? Mutation.Len1 : Mutation.None); //proper nouns are always lenited in the genitive
							if(head.isImmutable) mutN=Mutation.None;
							Mutation mutA=(headForm.gender==Gender.Masc ? Mutation.Len1 : Mutation.None);
							string value=Opers.Mutate(mutN, headForm.value)+" "+Opers.Mutate(mutA, modForm.value);
							this.sgGen.Add(new FormSg(value, headForm.gender));
						}
					}
				}
				foreach(FormSg headForm in head.sgGen) {
					//with article:
					if(!head.isDefinite || head.allowArticledGenitive) {
						List<Form> modForms=(headForm.gender==Gender.Masc ? mod.sgGenMasc : mod.sgGenFem);
						foreach(Form modForm in modForms) {
							Mutation mutN=(headForm.gender==Gender.Masc ? Mutation.Len3 : Mutation.PrefH);
							if(head.isImmutable) mutN=Mutation.None;
							Mutation mutA=(headForm.gender==Gender.Masc ? Mutation.Len1 : Mutation.None);
							string article=(headForm.gender==Gender.Masc ? "an" : "na");
							string value=article+" "+Opers.Mutate(mutN, headForm.value)+" "+Opers.Mutate(mutA, modForm.value);
							this.sgGenArt.Add(new FormSg(value, headForm.gender));
						}
					}
				}
				#endregion
				#region plural-nominative
				foreach(Form headForm in head.plNom) {
					{ //without article:
						foreach(Form modForm in mod.plNom) {
							Mutation mutA=(Opers.IsSlender(headForm.value) ? Mutation.Len1 : Mutation.None);
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.plNom.Add(new Form(value));
						}
					}
					if(!head.isDefinite) { //with article:
						foreach(Form modForm in mod.plNom) {
							Mutation mutN=Mutation.PrefH;
							if(head.isImmutable) mutN=Mutation.None;
							Mutation mutA=(Opers.IsSlender(headForm.value) ? Mutation.Len1 : Mutation.None);
							string value="na "+Opers.Mutate(mutN, headForm.value)+" "+Opers.Mutate(mutA, modForm.value);
							this.plNomArt.Add(new Form(value));
						}
					}
				}
				#endregion
				#region plural-genitive
				foreach(FormPlGen headForm in head.plGen) {
					{ //without article:
						List<Form> modForms=(headForm.strength==Strength.Strong ? mod.plNom : mod.sgNom);
						foreach(Form modForm in modForms) {
							Mutation mutA=(Opers.IsSlender(headForm.value) ? Mutation.Len1 : Mutation.None);
							if(headForm.strength==Strength.Weak) mutA=(Opers.IsSlenderI(headForm.value) ? Mutation.Len1 : Mutation.None); //"Gael", "captaen" are not slender
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.plGen.Add(new Form(value));
						}
					}
				}
				foreach(FormPlGen headForm in head.plGen) {
					//with article:
					if(!head.isDefinite || head.allowArticledGenitive) {
						List<Form> modForms=(headForm.strength==Strength.Strong ? mod.plNom : mod.sgNom);
						foreach(Form modForm in modForms) {
							Mutation mutN=Mutation.Ecl1;
							if(head.isImmutable) mutN=Mutation.None;
							Mutation mutA=(Opers.IsSlender(headForm.value) ? Mutation.Len1 : Mutation.None);
							if(headForm.strength==Strength.Weak) mutA=(Opers.IsSlenderI(headForm.value) ? Mutation.Len1 : Mutation.None); //"Gael", "captaen" are not slender
							string value="na "+Opers.Mutate(mutN, headForm.value)+" "+Opers.Mutate(mutA, modForm.value);
							this.plGenArt.Add(new Form(value));
						}
					}
				}
				#endregion
				#region singular-dative
				foreach(FormSg headForm in head.sgDat) {
					{ //without article:
						foreach(Form modForm in mod.sgNom) {
							Mutation mutA=(headForm.gender==Gender.Masc ? Mutation.None : Mutation.Len1);
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.sgDat.Add(new FormSg(value, headForm.gender));
						}
					}
					if(!head.isDefinite) { //with article:
						foreach(Form modForm in mod.sgNom) {
							Mutation mutA=(headForm.gender==Gender.Masc ? Mutation.None : Mutation.Len1);
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.sgDatArtS.Add(new FormSg(value, headForm.gender));
						}
						foreach(Form modForm in mod.sgNom) {
							string value=headForm.value+" "+Opers.Mutate(Mutation.Len1, modForm.value);
							this.sgDatArtN.Add(new FormSg(value, headForm.gender));
						}
					}
				}
				#endregion
				#region plural-dative
				foreach(Form headForm in head.plNom) {
					{ //without article:
						foreach(Form modForm in mod.plNom) {
							Mutation mutA=(Opers.IsSlender(headForm.value) ? Mutation.Len1 : Mutation.None);
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.plDat.Add(new Form(value));
						}
					}
					if(!head.isDefinite) { //with article:
						foreach(Form modForm in mod.plNom) {
							Mutation mutA=(Opers.IsSlender(headForm.value) ? Mutation.Len1 : Mutation.None);
							string value=headForm.value+" "+Opers.Mutate(mutA, modForm.value);
							this.plDatArt.Add(new Form(value));
						}
					}
				}
				#endregion
			}
		}

		//Constructor helper: Adds a possessive pronoun to sgNom, sgDat, sgGen, plNom, plDat, plGen of itself, empties all other forms:
		private void makePossessive(Possessive poss) {
			this.isDefinite=true;
			this.isPossessed=true;
			#region singular-nominative
			foreach(FormSg headForm in this.sgNom) {
				if(poss.apos.Count>0 & (Opers.StartsVowel(headForm.value) || Opers.StartsFVowel(headForm.value))) {
					foreach(Form possForm in poss.apos) {
						string value=possForm.value+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				} else {
					foreach(Form possForm in poss.full) {
						string value=possForm.value+" "+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				}
			}
            #endregion
			#region singular-dative
			foreach(FormSg headForm in this.sgDat) {
				if(poss.apos.Count>0 & (Opers.StartsVowel(headForm.value) || Opers.StartsFVowel(headForm.value))) {
					foreach(Form possForm in poss.apos) {
						string value=possForm.value+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				} else {
					foreach(Form possForm in poss.full) {
						string value=possForm.value+" "+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				}
			}
            #endregion
			#region singular-genitive
			foreach(FormSg headForm in this.sgGen) {
				if(poss.apos.Count>0 & (Opers.StartsVowel(headForm.value) || Opers.StartsFVowel(headForm.value))) {
					foreach(Form possForm in poss.apos) {
						string value=possForm.value+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				} else {
					foreach(Form possForm in poss.full) {
						string value=possForm.value+" "+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				}
			}
            #endregion
			#region plural-nominative
			foreach(Form headForm in this.plNom) {
				if(poss.apos.Count>0 & (Opers.StartsVowel(headForm.value) || Opers.StartsFVowel(headForm.value))) {
					foreach(Form possForm in poss.apos) {
						string value=possForm.value+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				} else {
					foreach(Form possForm in poss.full) {
						string value=possForm.value+" "+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				}
			}
            #endregion
			#region plural-dative
			foreach(Form headForm in this.plDat) {
				if(poss.apos.Count>0 & (Opers.StartsVowel(headForm.value) || Opers.StartsFVowel(headForm.value))) {
					foreach(Form possForm in poss.apos) {
						string value=possForm.value+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				} else {
					foreach(Form possForm in poss.full) {
						string value=possForm.value+" "+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				}
			}
            #endregion
			#region plural-genitive
			foreach(Form headForm in this.plGen) {
				if(poss.apos.Count>0 & (Opers.StartsVowel(headForm.value) || Opers.StartsFVowel(headForm.value))) {
					foreach(Form possForm in poss.apos) {
						string value=possForm.value+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				} else {
					foreach(Form possForm in poss.full) {
						string value=possForm.value+" "+Opers.Mutate(poss.mutation, headForm.value);
						headForm.value=value;
					}
				}
			}
            #endregion
            #region empty-all-others
			this.sgDatArtN.Clear();
			this.sgDatArtS.Clear();
			this.sgGenArt.Clear();
			this.sgNomArt.Clear();
			this.plDatArt.Clear();
			this.plGenArt.Clear();
			this.plNomArt.Clear();
            #endregion
        }

        //Creates a noun phrase from a noun determined by a possessive pronoun:
        public NP(Noun head, Possessive poss):this(head, poss, false) { }
        public NP(Noun head, Possessive poss, bool emphasize) : this(head) {
			this.makePossessive(poss);
			if (emphasize) {
				foreach (Form form in this.sgNom) form.value = Opers.Emphasize(form.value, poss.emphasizer);
				foreach (Form form in this.sgDat) form.value = Opers.Emphasize(form.value, poss.emphasizer);
				foreach (Form form in this.sgGen) form.value = Opers.Emphasize(form.value, poss.emphasizer);
				foreach (Form form in this.plNom) form.value = Opers.Emphasize(form.value, poss.emphasizer);
				foreach (Form form in this.plDat) form.value = Opers.Emphasize(form.value, poss.emphasizer);
				foreach (Form form in this.plGen) form.value = Opers.Emphasize(form.value, poss.emphasizer);
            }
        }

        //Creates a noun phrase from a noun modified by an adjective determined by a possessive pronoun:
        public NP(Noun head, Adjective mod, Possessive poss) : this(head, mod) {
			this.makePossessive(poss);
        }

        //Prints the noun phrase in BuNaMo format:
        public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<nounPhrase/>");
			doc.DocumentElement.SetAttribute("default", this.getLemma());
			doc.DocumentElement.SetAttribute("disambig", this.disambig);
			doc.DocumentElement.SetAttribute("isImmutable", (this.isImmutable ? "1" : "0"));
			doc.DocumentElement.SetAttribute("isDefinite", (this.isDefinite ? "1" : "0"));
			doc.DocumentElement.SetAttribute("isPossessed", (this.isPossessed ? "1" : "0"));
			doc.DocumentElement.SetAttribute("forceNominative", (this.forceNominative? "1" : "0"));
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
			foreach(FormSg f in this.sgNomArt) {
				XmlElement el=doc.CreateElement("sgNomArt");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgGenArt) {
				XmlElement el=doc.CreateElement("sgGenArt");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plNom) {
				XmlElement el=doc.CreateElement("plNom");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plGen) {
				XmlElement el=doc.CreateElement("plGen");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plNomArt) {
				XmlElement el=doc.CreateElement("plNomArt");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plGenArt) {
				XmlElement el=doc.CreateElement("plGenArt");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgDat) {
				XmlElement el=doc.CreateElement("sgDat");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgDatArtN) {
				XmlElement el=doc.CreateElement("sgDatArtN");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(FormSg f in this.sgDatArtS) {
				XmlElement el=doc.CreateElement("sgDatArtS");
				el.SetAttribute("default", f.value);
				el.SetAttribute("gender", (f.gender==Gender.Masc ? "masc" : "fem"));
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plDat) {
				XmlElement el=doc.CreateElement("plDat");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.plDatArt) {
				XmlElement el=doc.CreateElement("plDatArt");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			return doc;
		}

		//Constructs a noun phrase from an XML file in BuNaMo format:
		public NP(XmlDocument doc)
		{
			this.disambig=doc.DocumentElement.GetAttribute("disambig");
			this.isDefinite=(doc.DocumentElement.GetAttribute("isDefinite")=="1" ? true : false);
			this.isPossessed=(doc.DocumentElement.GetAttribute("isPossessed")=="1" ? true : false);
			this.isImmutable=(doc.DocumentElement.GetAttribute("isImmutable")=="1" ? true : false);
			this.forceNominative=(doc.DocumentElement.GetAttribute("forceNominative")=="1" ? true : false);
			foreach(XmlElement el in doc.SelectNodes("/*/sgNom")) {
				this.sgNom.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgGen")) {
				this.sgGen.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgNomArt")) {
				this.sgNomArt.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgGenArt")) {
				this.sgGenArt.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plNom")) {
				this.plNom.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plGen")) {
				this.plGen.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plNomArt")) {
				this.plNomArt.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plGenArt")) {
				this.plGenArt.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgDat")) {
				this.sgDat.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgDatArtN")) {
				this.sgDatArtN.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/sgDatArtS")) {
				this.sgDatArtS.Add(new FormSg(el.GetAttribute("default"), (el.GetAttribute("gender")=="fem" ? Gender.Fem : Gender.Masc)));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plDat")) {
				this.plDat.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/plDatArt")) {
				this.plDatArt.Add(new Form(el.GetAttribute("default")));
			}

		}
		public NP(string fileName) : this(Utils.LoadXml(fileName)) { }

    }
}
