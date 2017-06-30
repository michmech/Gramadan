using System;
using System.Collections.Generic;

namespace Gramadan
{
	//A verbal phrase:
	public class VP
	{
		//Forms of the verbal phrase:
		public Dictionary<VPTense, Dictionary<VPShape, Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>>> tenses=new Dictionary<VPTense, Dictionary<VPShape, Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>>>();
		public Dictionary<VPMood, Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>> moods=new Dictionary<VPMood, Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>>();

		//Constructs a verbal phrase from a verb:
		public VP(Verb v)
		{
			#region prepare-structure
			VPTense[] ts=new VPTense[] { VPTense.Past, VPTense.PastCont, VPTense.Pres, VPTense.PresCont, VPTense.Fut, VPTense.Cond };
			VPMood[] ms=new VPMood[] { VPMood.Imper, VPMood.Subj };
			VPShape[] ss=new VPShape[] { VPShape.Declar, VPShape.Interrog /*, VPShape.RelDep, VPShape.RelIndep, VPShape.Report*/ };
			VPPerson[] pers=new VPPerson[] { VPPerson.Sg1, VPPerson.Sg2, VPPerson.Sg3Masc, VPPerson.Sg3Fem, VPPerson.Pl1, VPPerson.Pl2, VPPerson.Pl3, VPPerson.NoSubject, VPPerson.Auto };
			VPPolarity[] pols=new VPPolarity[] { VPPolarity.Pos, VPPolarity.Neg };
			foreach(VPTense t in ts) {
				this.tenses.Add(t, new Dictionary<VPShape, Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>>());
				foreach(VPShape s in ss) {
					this.tenses[t].Add(s, new Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>());
					foreach(VPPerson per in pers) {
						this.tenses[t][s].Add(per, new Dictionary<VPPolarity, List<Form>>());
						foreach(VPPolarity pol in pols) {
							this.tenses[t][s][per].Add(pol, new List<Form>());
						}
					}
				}
			}
			foreach(VPMood m in ms) {
				this.moods.Add(m, new Dictionary<VPPerson, Dictionary<VPPolarity, List<Form>>>());
				foreach(VPPerson per in pers) {
					this.moods[m].Add(per, new Dictionary<VPPolarity, List<Form>>());
					foreach(VPPolarity pol in pols) {
						this.moods[m][per].Add(pol, new List<Form>());
					}
				}
			}
			#endregion

			//Apply rules to build tensed forms:
			foreach(VPTense t in v.tenseRules.Keys) {
				foreach(VPPerson p in v.tenseRules[t].Keys) {
					foreach(VPShape s in v.tenseRules[t][p].Keys) {
						foreach(VPPolarity l in v.tenseRules[t][p][s].Keys) {
							foreach(VerbTenseRule rule in v.tenseRules[t][p][s][l]) {
								//For each verb form, use the rule to build a verbal phrase form:
								foreach(Form vForm in v.tenses[rule.verbTense][rule.verbDependency][rule.verbPerson]) {
									Form vpForm=new Form("");
									if(rule.particle!="") vpForm.value+=rule.particle+" ";
									vpForm.value+=Opers.Mutate(rule.mutation, vForm.value);
									if(rule.pronoun!="") vpForm.value+=" "+rule.pronoun;
									if(v.getLemma()=="bí" && t==VPTense.Pres && s==VPShape.Declar && l==VPPolarity.Neg && vpForm.value.StartsWith("ní fhuil")) {
										vpForm.value="níl"+vpForm.value.Substring(8); //ní fhuil --> níl
									}
									this.tenses[t][s][p][l].Add(vpForm);
								}
							}
						}
					}
				}
			}

			#region mappings
			Dictionary<VPPerson, VerbPerson> mapPerson=new Dictionary<VPPerson, VerbPerson>();
			mapPerson.Add(VPPerson.Sg1, VerbPerson.Sg1);
			mapPerson.Add(VPPerson.Sg2, VerbPerson.Sg2);
			mapPerson.Add(VPPerson.Sg3Masc, VerbPerson.Sg3);
			mapPerson.Add(VPPerson.Sg3Fem, VerbPerson.Sg3);
			mapPerson.Add(VPPerson.Pl1, VerbPerson.Pl1);
			mapPerson.Add(VPPerson.Pl2, VerbPerson.Pl2);
			mapPerson.Add(VPPerson.Pl3, VerbPerson.Pl3);
			mapPerson.Add(VPPerson.NoSubject, VerbPerson.Base);
			mapPerson.Add(VPPerson.Auto, VerbPerson.Auto);
			Dictionary<VPPerson, string> mapPronoun=new Dictionary<VPPerson, string>();
			mapPronoun.Add(VPPerson.Sg1, " mé");
			mapPronoun.Add(VPPerson.Sg2, " tú");
			mapPronoun.Add(VPPerson.Sg3Masc, " sé");
			mapPronoun.Add(VPPerson.Sg3Fem, " sí");
			mapPronoun.Add(VPPerson.Pl1, " muid");
			mapPronoun.Add(VPPerson.Pl2, " sibh");
			mapPronoun.Add(VPPerson.Pl3, " siad");
			mapPronoun.Add(VPPerson.NoSubject, "");
			mapPronoun.Add(VPPerson.Auto, "");
			#endregion
			#region create-mood-imperative
			foreach(VPPerson vpPerson in pers) {
				bool hasSyntheticForms=false;
				//Synthetic forms:
				foreach(Form vForm in v.moods[VerbMood.Imper][mapPerson[vpPerson]]) {
					string pos=vForm.value;
					string neg="ná "+Opers.Mutate(Mutation.PrefH, vForm.value);
					this.moods[VPMood.Imper][vpPerson][VPPolarity.Pos].Add(new Form(pos));
					this.moods[VPMood.Imper][vpPerson][VPPolarity.Neg].Add(new Form(neg));
					hasSyntheticForms=true;
				}
				//Analytic forms:
				if(!hasSyntheticForms || vpPerson==VPPerson.Pl1 || vpPerson==VPPerson.Pl3) {
					foreach(Form vForm in v.moods[VerbMood.Imper][VerbPerson.Base]) {
						string pos=vForm.value+mapPronoun[vpPerson];
						string neg="ná "+Opers.Mutate(Mutation.PrefH, vForm.value)+mapPronoun[vpPerson];
						this.moods[VPMood.Imper][vpPerson][VPPolarity.Pos].Add(new Form(pos));
						this.moods[VPMood.Imper][vpPerson][VPPolarity.Neg].Add(new Form(neg));
						hasSyntheticForms=true;
					}
				}
			}
			#endregion
			#region create-mood-subjunctive
			foreach(VPPerson vpPerson in pers) {
				Mutation posMut=Mutation.Ecl1;
				Mutation negMut=Mutation.Len1;
				string negParticle="nár ";

				//Exceptions for irregular verbs:
				if(v.getLemma()=="abair") { negMut=Mutation.None; }
				if(v.getLemma()=="bí") { negParticle="ná "; }

				bool hasSyntheticForms=false;
				//Synthetic forms:
				foreach(Form vForm in v.moods[VerbMood.Subj][mapPerson[vpPerson]]) {
					string pos="go "+Opers.Mutate(posMut, vForm.value);
					string neg=negParticle+Opers.Mutate(negMut, vForm.value);
					this.moods[VPMood.Subj][vpPerson][VPPolarity.Pos].Add(new Form(pos));
					this.moods[VPMood.Subj][vpPerson][VPPolarity.Neg].Add(new Form(neg));
					hasSyntheticForms=true;
				}
				//Analytic forms:
				if(!hasSyntheticForms || vpPerson==VPPerson.Pl1) {
					foreach(Form vForm in v.moods[VerbMood.Subj][VerbPerson.Base]) {
						string pos="go "+Opers.Mutate(posMut, vForm.value)+mapPronoun[vpPerson];
						string neg=negParticle+Opers.Mutate(negMut, vForm.value)+mapPronoun[vpPerson];
						this.moods[VPMood.Subj][vpPerson][VPPolarity.Pos].Add(new Form(pos));
						this.moods[VPMood.Subj][vpPerson][VPPolarity.Neg].Add(new Form(neg));
						hasSyntheticForms=true;
					}
				}
			}
			#endregion

		}

		//Prints a user-friendly summary of the verbal phrase in one of its tenses, shapes and polarities:
		public string print(VPTense tense, VPShape shape, VPPolarity pol)
		{
			string ret="";
			VPPerson[] pers=new VPPerson[] { VPPerson.Sg1, VPPerson.Sg2, VPPerson.Sg3Masc, VPPerson.Sg3Fem, VPPerson.Pl1, VPPerson.Pl2, VPPerson.Pl3, VPPerson.NoSubject, VPPerson.Auto };
			foreach(VPPerson per in pers) {
				ret+=per.ToString()+": "; foreach(Form f in this.tenses[tense][shape][per][pol]) { ret+="["+f.value +"] "; } ret+=Environment.NewLine;
			}
			return ret;
		}

		//Prints a user-friendly summary of the verbal phrase in one of its moods and polarities:
		public string print(VPMood mood, VPPolarity pol)
		{
			string ret="";
			VPPerson[] pers=new VPPerson[] { VPPerson.Sg1, VPPerson.Sg2, VPPerson.Sg3Masc, VPPerson.Sg3Fem, VPPerson.Pl1, VPPerson.Pl2, VPPerson.Pl3, VPPerson.NoSubject, VPPerson.Auto };
			foreach(VPPerson per in pers) {
				ret+=per.ToString()+": "; foreach(Form f in this.moods[mood][per][pol]) { ret+="["+f.value +"] "; } ret+=Environment.NewLine;
			}
			return ret;
		}

	}

	//Enumerations used to access verb phrase forms:
	public enum VPTense { Any, Past, PastCont, Pres, PresCont, Fut, Cond }
	public enum VPMood { Imper, Subj }
	public enum VPShape { Any, Declar, Interrog /*, RelDep, RelIndep, Report*/ }
	public enum VPPerson { Any, Sg1, Sg2, Sg3Masc, Sg3Fem, Pl1, Pl2, Pl3, NoSubject, Auto }
	public enum VPPolarity { Any, Pos, Neg }
}
