using System;
using System.Collections.Generic;
using System.Xml;

using M=Gramadan.Mutation;
using VT=Gramadan.VerbTense;
using VD=Gramadan.VerbDependency;
using VPN=Gramadan.VerbPerson;

namespace Gramadan
{
	//A verb:
	public class Verb
	{
		public string disambig="";
		public string getNickname()
		{
			string ret=getLemma()+" verb";
			if(this.disambig!="") ret+=" "+this.disambig;
			ret=ret.Replace(" ", "_");
			return ret;
		}

		//Forms of the verb:
		public List<Form> verbalNoun=new List<Form>();
		public List<Form> verbalAdjective=new List<Form>();
		public Dictionary<VerbTense, Dictionary<VerbDependency, Dictionary<VerbPerson, List<Form>>>> tenses=new Dictionary<VerbTense,Dictionary<VerbDependency,Dictionary<VerbPerson,List<Form>>>>();
		public Dictionary<VerbMood, Dictionary<VerbPerson, List<Form>>> moods=new Dictionary<VerbMood, Dictionary<VerbPerson, List<Form>>>();

		//Rules for building verbal phrases:
		public Dictionary<VPTense, Dictionary<VPPerson, Dictionary<VPShape, Dictionary<VPPolarity, List<VerbTenseRule>>>>> tenseRules=new Dictionary<VPTense, Dictionary<VPPerson, Dictionary<VPShape, Dictionary<VPPolarity, List<VerbTenseRule>>>>>();

		//Returns tense rules that match the parameters. In each paramer, '.Any' means 'any'.
		public List<VerbTenseRule> getTenseRules(VPTense tense, VPPerson person, VPShape shape, VPPolarity polarity)
		{
			List<VerbTenseRule> ret=new List<VerbTenseRule>();
			VPTense[] ts=new VPTense[] { VPTense.Past, VPTense.PastCont, VPTense.Pres, VPTense.PresCont, VPTense.Fut, VPTense.Cond };
			VPShape[] ss=new VPShape[] { VPShape.Declar, VPShape.Interrog /*, VPShape.RelDep, VPShape.RelIndep, VPShape.Report*/ };
			VPPerson[] pers=new VPPerson[] { VPPerson.Sg1, VPPerson.Sg2, VPPerson.Sg3Masc, VPPerson.Sg3Fem, VPPerson.Pl1, VPPerson.Pl2, VPPerson.Pl3, VPPerson.NoSubject, VPPerson.Auto };
			VPPolarity[] pols=new VPPolarity[] { VPPolarity.Pos, VPPolarity.Neg };
			foreach(VPTense t in ts) {
				foreach(VPPerson per in pers) {
					foreach(VPShape s in ss) {
						foreach(VPPolarity pol in pols) {
							if((tense==VPTense.Any || t==tense) && (person==VPPerson.Any || per==person) && (shape==VPShape.Any || s==shape) && (polarity==VPPolarity.Any || pol==polarity)) {
								foreach(VerbTenseRule rule in this.tenseRules[t][per][s][pol]) {
									ret.Add(rule);
								}
							}
						}
					}
				}
			}
			return ret;
		}

		//Constructor:
		public Verb()
		{
			#region prepare-structure-for-rules
			{
				VPTense[] ts=new VPTense[] { VPTense.Past, VPTense.PastCont, VPTense.Pres, VPTense.PresCont, VPTense.Fut, VPTense.Cond };
				VPShape[] ss=new VPShape[] { VPShape.Declar, VPShape.Interrog /*, VPShape.RelDep, VPShape.RelIndep, VPShape.Report*/ };
				VPPerson[] pers=new VPPerson[] { VPPerson.Sg1, VPPerson.Sg2, VPPerson.Sg3Masc, VPPerson.Sg3Fem, VPPerson.Pl1, VPPerson.Pl2, VPPerson.Pl3, VPPerson.NoSubject, VPPerson.Auto };
				VPPolarity[] pols=new VPPolarity[] { VPPolarity.Pos, VPPolarity.Neg };
				foreach(VPTense t in ts) {
					this.tenseRules.Add(t, new Dictionary<VPPerson, Dictionary<VPShape, Dictionary<VPPolarity, List<VerbTenseRule>>>>());
					foreach(VPPerson per in pers) {
						this.tenseRules[t].Add(per, new Dictionary<VPShape, Dictionary<VPPolarity, List<VerbTenseRule>>>());
						foreach(VPShape s in ss) {
							this.tenseRules[t][per].Add(s, new Dictionary<VPPolarity, List<VerbTenseRule>>());
							foreach(VPPolarity pol in pols) {
								this.tenseRules[t][per][s].Add(pol, new List<VerbTenseRule>());
							}
						}
					}
				}
			}
			#endregion
			#region default-rules
			{
				VPTense t; VPPerson p; string pron;
				VPShape dec=VPShape.Declar; VPShape rog=VPShape.Interrog;
				VPPolarity pos=VPPolarity.Pos; VPPolarity neg=VPPolarity.Neg;
				#region past
				t=VPTense.Past;
				p=VPPerson.NoSubject; //cheap, d'oscail
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, ""));

				p=VPPerson.Sg1; pron="mé"; //cheap mé, d'oscail mé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg2; pron="tú"; //cheap tú, d'oscail tú
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Masc; pron="sé"; //cheap sé, d'oscail sé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Fem; pron="sí"; //cheap sí, d'oscail sí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl1; //cheapamar, d'osclaíomar
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Pl1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Pl1, ""));

				p=VPPerson.Pl1; pron="muid"; //cheap muid, d'oscail muid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				
				p=VPPerson.Pl2; pron="sibh"; //cheap sibh, d'oscail sibh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; pron="siad"; //cheap siad, d'oscail siad
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; //cheapadar, d'osclaíodar
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Past, VD.Indep, VPN.Pl3, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.Len1, VT.Past, VD.Dep, VPN.Pl3, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.Len1, VT.Past, VD.Dep, VPN.Pl3, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.Len1, VT.Past, VD.Dep, VPN.Pl3, ""));

				p=VPPerson.Auto; //ceapadh, osclaíodh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Past, VD.Indep, VPN.Auto, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("níor", M.None, VT.Past, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("ar", M.None, VT.Past, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nár", M.None, VT.Past, VD.Dep, VPN.Auto, ""));
				#endregion
				#region pres
				t=VPTense.Pres; //Only 'bí' has forms in this tense.
				p=VPPerson.NoSubject; //tá
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, ""));

				p=VPPerson.Sg1; //táim
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Sg1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Sg1, ""));

				p=VPPerson.Sg1; pron="mé"; //tá mé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg2; pron="tú"; //tá tú
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Masc; pron="sé"; //tá sé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Fem; pron="sí"; //tá sí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl1; //táimid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Pl1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Pl1, ""));

				p=VPPerson.Pl1; pron="muid"; //tá muid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl2; pron="sibh"; //tá sibh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; pron="siad"; //tá siad
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Base, pron));

				p=VPPerson.Auto; //táthar
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Pres, VD.Indep, VPN.Auto, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Pres, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Pres, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Pres, VD.Dep, VPN.Auto, ""));
				#endregion
				#region presCont
				t=VPTense.PresCont;
				p=VPPerson.NoSubject; //ceapann, osclaíonn
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, ""));

				p=VPPerson.Sg1; //ceapaim, osclaím
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Sg1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Sg1, ""));

				p=VPPerson.Sg2; pron="tú"; //ceapann tú, osclaíonn tú
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Masc; pron="sé"; //ceapann sé, osclaíonn sé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Fem; pron="sí"; //ceapann sí, osclaíonn sí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl1; //ceapaimid, osclaímid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Pl1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Pl1, ""));

				p=VPPerson.Pl1; pron="muid"; //ceapann muid, osclaíonn muid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl2; pron="sibh"; //ceapann sibh, osclaíonn sibh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; pron="siad"; //ceapann siad, osclaíonn siad
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Auto; //ceaptar, osclaítear
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.PresCont, VD.Indep, VPN.Auto, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PresCont, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PresCont, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PresCont, VD.Dep, VPN.Auto, ""));
				#endregion
				#region fut
				t=VPTense.Fut;
				p=VPPerson.NoSubject; //ceapfaidh, osclóidh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, ""));

				p=VPPerson.Sg1; pron="mé"; //ceapfaidh mé, osclóidh mé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg2; pron="tú"; //ceapfaidh tú, osclóidh tú
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Masc; pron="sé"; //ceapfaidh sé, osclóidh sé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Fem; pron="sí"; //ceapfaidh sí, osclóidh sí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl1; //ceapfaimid, osclóimid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Pl1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Pl1, ""));

				p=VPPerson.Pl1; pron="muid"; //ceapfaidh muid, osclóidh muid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl2; pron="sibh"; //ceapfaidh sibh, osclóidh sibh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; pron="siad"; //ceapfaidh siad, osclóidh siad
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Base, pron));

				p=VPPerson.Auto; //ceapfar, osclófar
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.None, VT.Fut, VD.Indep, VPN.Auto, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Fut, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Fut, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Fut, VD.Dep, VPN.Auto, ""));
				#endregion
				#region cond
				t=VPTense.Cond;
				p=VPPerson.NoSubject; //cheapfadh, d'osclódh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Base, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Base, ""));

				p=VPPerson.Sg1; //cheapfainn, d'osclóinn
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Sg1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Sg1, ""));

				p=VPPerson.Sg2; //cheapfá, d'osclófá
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Sg2, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Sg2, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Sg2, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Sg2, ""));

				p=VPPerson.Sg3Masc; pron="sé"; //cheapfadh sé, d'osclódh sé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Fem; pron="sí"; //cheapfadh sí, d'osclódh sí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl1; //cheapfaimis, d'osclóimis
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Pl1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Pl1, ""));

				p=VPPerson.Pl1; pron="muid"; //cheapfadh muid, d'osclódh muid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl2; pron="sibh"; //cheapfadh sibh, d'osclódh sibh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; //cheapfaidís, d'osclóidís
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Pl3, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Pl3, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Pl3, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Pl3, ""));

				p=VPPerson.Pl3; pron="siad"; //cheapfadh siad, d'osclódh siad
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Base, pron));

				p=VPPerson.Auto; //cheapfaí, d'osclófaí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.Cond, VD.Indep, VPN.Auto, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.Cond, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.Cond, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.Cond, VD.Dep, VPN.Auto, ""));
				#endregion
				#region pastCont
				t=VPTense.PastCont;
				p=VPPerson.NoSubject; //cheapadh, d'osclaíodh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Base, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Base, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Base, ""));

				p=VPPerson.Sg1; //cheapainn, d'osclaínn
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Sg1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Sg1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Sg1, ""));

				p=VPPerson.Sg2; //cheaptá, d'osclaíteá
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Sg2, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Sg2, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Sg2, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Sg2, ""));

				p=VPPerson.Sg3Masc; pron="sé"; //cheapadh sé, d'osclaíodh sé
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Sg3Fem; pron="sí"; //cheapadh sí, d'osclaíodh sí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl1; //cheapaimis, d'osclaímis
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Pl1, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Pl1, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Pl1, ""));

				p=VPPerson.Pl1; pron="muid"; //cheapadh muid, d'osclaíodh muid
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl2; pron="sibh"; //cheapadh sibh, d'osclaíodh sibh
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Pl3; //cheapaidís, d'osclaídís
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Pl3, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Pl3, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Pl3, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Pl3, ""));

				p=VPPerson.Pl3; pron="siad"; //cheapadh siad, d'osclaíodh siad
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Base, pron));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Base, pron));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Base, pron));

				p=VPPerson.Auto; //cheaptaí, d'osclaítí
				this.tenseRules[t][p][dec][pos].Add(new VerbTenseRule("", M.Len1D, VT.PastCont, VD.Indep, VPN.Auto, ""));
				this.tenseRules[t][p][dec][neg].Add(new VerbTenseRule("ní", M.Len1, VT.PastCont, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][pos].Add(new VerbTenseRule("an", M.Ecl1x, VT.PastCont, VD.Dep, VPN.Auto, ""));
				this.tenseRules[t][p][rog][neg].Add(new VerbTenseRule("nach", M.Ecl1, VT.PastCont, VD.Dep, VPN.Auto, ""));
				#endregion
			}
			#endregion
			#region prepare-structure-for-data
			{
				VerbTense[] ts=new VerbTense[] { VerbTense.Past, VerbTense.PastCont, VerbTense.Pres, VerbTense.PresCont, VerbTense.Fut, VerbTense.Cond };
				VerbMood[] ms=new VerbMood[] { VerbMood.Imper, VerbMood.Subj };
				VerbDependency[] ds=new VerbDependency[] { VerbDependency.Indep, VerbDependency.Dep };
				VerbPerson[] ps=new VerbPerson[] { VerbPerson.Base, VerbPerson.Sg1, VerbPerson.Sg2, VerbPerson.Sg3, VerbPerson.Pl1, VerbPerson.Pl2, VerbPerson.Pl3, VerbPerson.Auto };
				foreach(VerbTense t in ts) {
					this.tenses.Add(t, new Dictionary<VerbDependency, Dictionary<VerbPerson, List<Form>>>());
					foreach(VerbDependency d in ds) {
						this.tenses[t].Add(d, new Dictionary<VerbPerson, List<Form>>());
						foreach(VerbPerson p in ps) {
							this.tenses[t][d].Add(p, new List<Form>());
						}
					}
				}
				foreach(VerbMood m in ms) {
					this.moods.Add(m, new Dictionary<VerbPerson, List<Form>>());
					foreach(VerbPerson p in ps) {
						this.moods[m].Add(p, new List<Form>());
					}

				}
			}
			#endregion
		}
		public Verb(string fileName) : this(Utils.LoadXml(fileName)) { }
		public Verb(XmlDocument doc):this()
		{
			this.disambig=doc.DocumentElement.GetAttribute("disambig");
			foreach(XmlElement el in doc.SelectNodes("/*/verbalNoun")) {
				this.verbalNoun.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/verbalAdjective")) {
				this.verbalAdjective.Add(new Form(el.GetAttribute("default")));
			}
			foreach(XmlElement el in doc.SelectNodes("/*/tenseForm")) {
				string value=el.GetAttribute("default");
				VerbTense tense=(VerbTense)Enum.Parse(typeof(VerbTense), el.GetAttribute("tense"));
				VerbDependency dependency=(VerbDependency)Enum.Parse(typeof(VerbDependency), el.GetAttribute("dependency"));
				VerbPerson person=(VerbPerson)Enum.Parse(typeof(VerbPerson), el.GetAttribute("person"));
				this.addTense(tense, dependency, person, value);
			}
			foreach(XmlElement el in doc.SelectNodes("/*/moodForm")) {
				string value=el.GetAttribute("default");
				VerbMood mood=(VerbMood)Enum.Parse(typeof(VerbMood), el.GetAttribute("mood"));
				VerbPerson person=(VerbPerson)Enum.Parse(typeof(VerbPerson), el.GetAttribute("person"));
				this.addMood(mood, person, value);
			}
			#region change-rules-for-irregular-bí
			if(this.getLemma()=="bí") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Pos)) {
					rule.mutation=Mutation.Len1;
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.None;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.None;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.None;
					rule.particle="nach";
				}
			}
			#endregion
			#region change-rules-for-irregular-abair
			if(this.getLemma()=="abair") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Any, VPPerson.Any, VPShape.Declar, VPPolarity.Pos)) {
					rule.mutation=Mutation.None;
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Any, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.None;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Any, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.Ecl1x;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Any, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="nach";
				}
			}
			#endregion
			#region change-rules-for-irregular-déan
			if(this.getLemma()=="déan") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.Len1;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.Ecl1x;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="nach";
				}
			}
			#endregion
			#region change-rules-for-irregular-faigh
			if(this.getLemma()=="faigh") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Pos)) {
					rule.mutation=Mutation.None;
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.Ecl1x;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="nach";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Fut, VPPerson.Any, VPShape.Declar, VPPolarity.Pos)) {
					rule.mutation=Mutation.Len1;
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Fut, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Fut, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.Ecl1x;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Fut, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="nach";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Cond, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="ní";
				}
			}
			#endregion
			#region change-rules-for-irregular-feic
			if(this.getLemma()=="feic") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Pos)) {
					rule.mutation=Mutation.Len1;
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.Len1;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.Ecl1x;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="nach";
				}
			}
			#endregion
			#region change-rules-for-irregular-téigh
			if(this.getLemma()=="téigh") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Pos)) {
					rule.mutation=Mutation.Len1;
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Declar, VPPolarity.Neg)) {
					rule.mutation=Mutation.Len1;
					rule.particle="ní";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Pos)) {
					rule.mutation=Mutation.Ecl1x;
					rule.particle="an";
				}
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Any, VPShape.Interrog, VPPolarity.Neg)) {
					rule.mutation=Mutation.Ecl1;
					rule.particle="nach";
				}
			}
			#endregion
			#region change-rules-for-irregular-tar
			if(this.getLemma()=="tar") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Auto, VPShape.Any, VPPolarity.Any)) {
					rule.mutation=Mutation.Len1;
				}
			}
			#endregion
			#region change-rules-for-irregular-clois
			if(this.getLemma()=="clois") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Auto, VPShape.Any, VPPolarity.Any)) {
					rule.mutation=Mutation.Len1;
				}
			}
			#endregion
			#region change-rules-for-irregular-cluin
			if(this.getLemma()=="cluin") {
				foreach(VerbTenseRule rule in this.getTenseRules(VPTense.Past, VPPerson.Auto, VPShape.Any, VPPolarity.Any)) {
					rule.mutation=Mutation.Len1;
				}
			}
			#endregion
		}

		//Extracts the verb's lemma:
		public string getLemma()
		{
			string ret="";
			//the imperative second-person singular is the lemma:
			if(this.moods[VerbMood.Imper][VerbPerson.Sg2].Count>0) ret=this.moods[VerbMood.Imper][VerbPerson.Sg2][0].value;
			if(ret=="") {
				//if not available, then the past tense base is the lemma:
				if(this.tenses[VerbTense.Past][VerbDependency.Indep][VerbPerson.Base].Count>0) ret=this.tenses[VerbTense.Past][VerbDependency.Indep][VerbPerson.Base][0].value;
			}
			return ret;
		}

		//Helper methods to add forms quickly:
		public void addTense(VerbTense t, VerbDependency d, VerbPerson p, string form)
		{
			this.tenses[t][d][p].Add(new Form(form));
		}
		public void addTense(VerbTense t, VerbPerson p, string form)
		{
			this.addTense(t, VerbDependency.Indep, p, form);
			this.addTense(t, VerbDependency.Dep, p, form);
		}
		public void addMood(VerbMood m, VerbPerson p, string form)
		{ 
			this.moods[m][p].Add(new Form(form));
		}

		//Prints the verb in BuNaMo format:
		public XmlDocument printXml()
		{
			XmlDocument doc=new XmlDocument(); doc.LoadXml("<verb/>");
			doc.DocumentElement.SetAttribute("default", this.getLemma());
			doc.DocumentElement.SetAttribute("disambig", this.disambig);
			foreach(Form f in this.verbalNoun) {
				XmlElement el=doc.CreateElement("verbalNoun");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(Form f in this.verbalAdjective) {
				XmlElement el=doc.CreateElement("verbalAdjective");
				el.SetAttribute("default", f.value);
				doc.DocumentElement.AppendChild(el);
			}
			foreach(VerbTense tense in this.tenses.Keys) {
				foreach(VerbDependency dependency in this.tenses[tense].Keys) {
					foreach(VerbPerson person in this.tenses[tense][dependency].Keys) {
						foreach(Form f in this.tenses[tense][dependency][person]) {
							XmlElement el=doc.CreateElement("tenseForm");
							el.SetAttribute("default", f.value);
							el.SetAttribute("tense", tense.ToString());
							el.SetAttribute("dependency", dependency.ToString());
							el.SetAttribute("person", person.ToString());
							doc.DocumentElement.AppendChild(el);
						}
					}
				}
			}
			foreach(VerbMood mood in this.moods.Keys) {
				foreach(VerbPerson person in this.moods[mood].Keys) {
					foreach(Form f in this.moods[mood][person]) {
						XmlElement el=doc.CreateElement("moodForm");
						el.SetAttribute("default", f.value);
						el.SetAttribute("mood", mood.ToString());
						el.SetAttribute("person", person.ToString());
						doc.DocumentElement.AppendChild(el);
					}
				}
			}
			return doc;
		}
	}

	//Enumerations used to access verb forms:
	public enum VerbTense { Past, PastCont, Pres, PresCont, Fut, Cond }
	public enum VerbMood { Imper, Subj }
	public enum VerbDependency { Indep, Dep }
	public enum VerbPerson { Base, Sg1, Sg2, Sg3, Pl1, Pl2, Pl3, Auto }

	//A rule for building a tensed form of a verbal phrase from a verb:
	public class VerbTenseRule
	{
		//Which particle to put in front of the verb form (empty string if none):
		public string particle="";

		//Which mutation to cause on the verb form:
		public Mutation mutation=Mutation.None;

		//Which verb form to use:
		public VerbTense verbTense;
		public VerbDependency verbDependency;
		public VerbPerson verbPerson;

		//Which pronoun to put after the verb form (empty string if none):
		public string pronoun="";

		//Constructor:
		public VerbTenseRule(string particle, Mutation mutation, VerbTense verbTense, VerbDependency verbDependency, VerbPerson verbPerson, string pronoun)
		{
			this.particle=particle;
			this.mutation=mutation;
			this.verbTense=verbTense;
			this.verbDependency=verbDependency;
			this.verbPerson=verbPerson;
			this.pronoun=pronoun;
		}
	}

}
