using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Gramadan;

namespace Tester
{
	class QualityControl
	{
		public static void AinmfhocailBearnai()
		{
			StreamWriter writer=new StreamWriter(@"E:\deleteme\ainmfhocail-bearnaí.txt");
			writer.Write("fillteán"+"\t"+"leama"+"\t"+"inscne"+"\t"+"díochlaonadh");
			writer.Write("\t"+"fadhb");
			writer.Write("\t"+"ginideach uatha");
			writer.Write("\t"+"ainmneach iolra");
			writer.Write("\t"+"ginideach iolra");
			writer.WriteLine();

			string[] folders= { "noun", "nounNew", "nounUnsafe" };
			foreach(string folder in folders) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folder, "*.xml")) {
					Console.WriteLine(file);
					string problem="";

					Noun n=new Noun(file);
					if(n.sgGen.Count==0 && n.plNom.Count==0 && n.plGen.Count==0) problem+=", gach foirm in easnamh";
					else {
						if(n.sgNom.Count==0 && n.sgGen.Count==0) problem+=", uatha in easnamh";
						if(n.sgNom.Count==0 && n.sgGen.Count!=0) problem+=", ainmneach uatha in easnamh";
						if(n.sgNom.Count!=0 && n.sgGen.Count==0) problem+=", ginideach uatha in easnamh";
						if(n.plNom.Count==0 && n.plGen.Count==0) problem+=", iolra in easnamh";
						if(n.plNom.Count==0 && n.plGen.Count!=0) problem+=", ainmneach iolra in easnamh";
						if(n.plNom.Count!=0 && n.plGen.Count==0) problem+=", ginideach iolra in easnamh";
					}

					if(problem.Length>1) problem=problem.Substring(2);
					if(problem!="") {
						writer.Write(folder+"\t"+n.getLemma()+"\t"+n.getGender().ToString().ToLower()+"\t"+n.declension);
						writer.Write("\t"+problem);
						writer.Write("\t"+PrintForms(n.sgGen));
						writer.Write("\t"+PrintForms(n.plNom));
						writer.Write("\t"+PrintForms(n.plGen));
						writer.WriteLine();
					}
					
				}
			}
			writer.Close();
		}
		public static void AidiachtaiBearnai()
		{
			StreamWriter writer=new StreamWriter(@"E:\deleteme\aidiachtaí-bearnaí.txt");
			writer.Write("fillteán"+"\t"+"leama"+"\t"+"díochlaonadh");
			writer.Write("\t"+"fadhb");
			writer.Write("\t"+"ginideach firinsneach");
			writer.Write("\t"+"ginideach baininscneach");
			writer.Write("\t"+"iolra");
			writer.Write("\t"+"foirm chéimithe");
			writer.WriteLine();

			string[] folders= { "adjective", "adjectiveUnsafe" };
			foreach(string folder in folders) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folder, "*.xml")) {
					Console.WriteLine(file);
					string problem="";

					Adjective a=new Adjective(file);
					if(a.sgGenMasc.Count==0 && a.sgGenFem.Count==0) problem+=", ginideach in easnamh";
					if(a.sgGenMasc.Count!=0 && a.sgGenFem.Count==0) problem+=", ginideach baininscneach in easnamh";
					if(a.sgGenMasc.Count==0 && a.sgGenFem.Count!=0) problem+=", ginideach firinscneach in easnamh";
					if(a.plNom.Count==0) problem+=", iolra in easnamh";
					if(a.graded.Count==0) problem+=", foirm chéimithe in easnamh";

					if(problem.Length>1) problem=problem.Substring(2);
					if(problem!="") {
						writer.Write(folder+"\t"+a.getLemma()+"\t"+a.declension);
						writer.Write("\t"+problem);
						writer.Write("\t"+PrintForms(a.sgGenMasc));
						writer.Write("\t"+PrintForms(a.sgGenFem));
						writer.Write("\t"+PrintForms(a.plNom));
						writer.Write("\t"+PrintForms(a.graded));
						writer.WriteLine();
					}

				}
			}
			writer.Close();
		}
		public static void BriathraInfinideachaBearnai()
		{
			StreamWriter writer=new StreamWriter(@"E:\deleteme\briathra-infinideacha-bearnaí.txt");
			writer.Write("fillteán"+"\t"+"leama");
			writer.Write("\t"+"fadhb");
			writer.Write("\t"+"ainm briathartha");
			writer.Write("\t"+"aidiacht bhriathartha");
			writer.WriteLine();

			string[] folders= { "verb", "verbUnsafe" };
			foreach(string folder in folders) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folder, "*.xml")) {
					Console.WriteLine(file);
					string problem="";

					Verb v=new Verb(file);
					if(v.verbalNoun.Count==0) problem+=", ainm briathartha in easnamh";
					if(v.verbalAdjective.Count==0) problem+=", aidiacht bhriathartha in easnamh";

					if(problem.Length>1) problem=problem.Substring(2);
					if(problem!="") {
						writer.Write(folder+"\t"+v.getLemma());
						writer.Write("\t"+problem);
						writer.Write("\t"+PrintForms(v.verbalNoun));
						writer.Write("\t"+PrintForms(v.verbalAdjective));
						writer.WriteLine();
					}
				}
			}
			writer.Close();
		}
		public static void BriathraFinideachaBearnai()
		{
			Dictionary<VerbTense, string> tenses=new Dictionary<VerbTense, string>();
			Dictionary<VerbMood, string> moods=new Dictionary<VerbMood, string>();
			tenses.Add(VerbTense.Past, "aimsir chaite");
			tenses.Add(VerbTense.PastCont, "aimsir ghnáthchaite");
			tenses.Add(VerbTense.PresCont, "aimsir ghnáthláithreach");
			tenses.Add(VerbTense.Fut, "aimsir fháistineach");
			tenses.Add(VerbTense.Cond, "modh coinníollach");
			moods.Add(VerbMood.Imper, "modh ordaitheach");
			moods.Add(VerbMood.Subj, "modh foshuiteach");

			StreamWriter writer=new StreamWriter(@"E:\deleteme\briathra-finideacha-bearnaí.txt");
			writer.Write("fillteán"+"\t"+"leama"+"\t"+"aimsir/modh");
			writer.Write("\t"+"fadhb");
			writer.Write("\t"+"bunfhoirm scartha");
			writer.Write("\t"+"uatha 1 táite");
			writer.Write("\t"+"uatha 2 táite");
			writer.Write("\t"+"uatha 3 táite");
			writer.Write("\t"+"iolra 1 táite");
			writer.Write("\t"+"iolra 2 táite");
			writer.Write("\t"+"iolra 3 táite");
			writer.Write("\t"+"saorbhriathar");
			writer.WriteLine();

			string[] folders= { "verb", "verbUnsafe" };
			foreach(string folder in folders) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folder, "*.xml")) {
					Console.WriteLine(file);
					Verb v=new Verb(file);

					foreach(VerbTense t in tenses.Keys) {
						string problem="";
						if(v.tenses[t][VerbDependency.Indep][VerbPerson.Base].Count==0) problem+=", foirm scartha in easnamh";
						if(v.tenses[t][VerbDependency.Indep][VerbPerson.Auto].Count==0) problem+=", saorbhriathar in easnamh";
						if(v.tenses[t][VerbDependency.Indep][VerbPerson.Pl1].Count==0) problem+=", iolra 1 táite in easnamh";
						if(t==VerbTense.Past || t==VerbTense.PastCont || t==VerbTense.Cond) {
							if(v.tenses[t][VerbDependency.Indep][VerbPerson.Pl3].Count==0) problem+=", iolra 3 táite in easnamh";
						}

						if(problem.Length>1) problem=problem.Substring(2);
						if(problem!="") {
							writer.Write(folder+"\t"+v.getLemma()+"\t"+tenses[t]);
							writer.Write("\t"+problem);
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Base]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Sg1]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Sg2]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Sg3]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Pl1]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Pl2]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Pl3]));
							writer.Write("\t"+PrintForms(v.tenses[t][VerbDependency.Indep][VerbPerson.Auto]));
							writer.WriteLine();
						}
					}

					foreach(VerbMood m in moods.Keys) {
						string problem="";
						if(v.moods[m][VerbPerson.Base].Count==0) problem+=", foirm scartha in easnamh";
						if(v.moods[m][VerbPerson.Auto].Count==0) problem+=", saorbhriathar in easnamh";
						if(v.moods[m][VerbPerson.Pl1].Count==0) problem+=", iolra 1 táite in easnamh";
						if(m==VerbMood.Imper) {
							if(v.moods[m][VerbPerson.Pl3].Count==0) problem+=", iolra 3 táite in easnamh";
						}

						if(problem.Length>1) problem=problem.Substring(2);
						if(problem!="") {
							writer.Write(folder+"\t"+v.getLemma()+"\t"+moods[m]);
							writer.Write("\t"+problem);
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Base]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Sg1]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Sg2]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Sg3]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Pl1]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Pl2]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Pl3]));
							writer.Write("\t"+PrintForms(v.moods[m][VerbPerson.Auto]));
							writer.WriteLine();
						}
					}
				}
			}
			writer.Close();
		}

		public static void AinmfhocailRegex(string regex, string filenameEnding)
		{
			StreamWriter writer=new StreamWriter(@"E:\deleteme\ainmfhocail-"+filenameEnding+".txt");
			writer.Write("fillteán"+"\t"+"leama"+"\t"+"inscne"+"\t"+"díochlaonadh mar atá"+"\t"+"díochlaonadh nua");
			writer.WriteLine();

			string[] folders= { "noun", "nounNew", "nounUnsafe" };
			foreach(string folder in folders) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folder, "*.xml")) {
					Console.WriteLine(file);
					Noun n=new Noun(file);
					if(Regex.IsMatch(n.getLemma(), regex)) {
						writer.Write(folder+"\t"+n.getLemma()+"\t"+n.getGender().ToString().ToLower()+"\t"+n.declension);
						writer.WriteLine();
					}
				}
			}
			writer.Close();
		}
		public static void AidiachtaiRegex(string regex, string filenameEnding)
		{
			StreamWriter writer=new StreamWriter(@"E:\deleteme\aidiachtaí-"+filenameEnding+".txt");
			writer.Write("fillteán"+"\t"+"leama"+"\t"+"díochlaonadh"+"\t"+"díochlaonadh nua");
			writer.WriteLine();

			string[] folders= { "adjective", "adjectiveUnsafe" };
			foreach(string folder in folders) {
				foreach(string file in Directory.GetFiles(@"C:\MBM\Gramadan\BuNaMo\"+folder, "*.xml")) {
					Console.WriteLine(file);
					Adjective a=new Adjective(file);
					if(Regex.IsMatch(a.getLemma(), regex)) {
						writer.Write(folder+"\t"+a.getLemma()+"\t"+a.declension);
						writer.WriteLine();
					}
				}
			}
			writer.Close();
		}


		private static string PrintForms(List<Gramadan.FormSg> forms)
		{
			List<Form> fs=new List<Form>();
			foreach(FormSg f in forms) fs.Add(f);
			return PrintForms(fs);
		}
		private static string PrintForms(List<Gramadan.FormPlGen> forms)
		{
			List<Form> fs=new List<Form>();
			foreach(FormPlGen f in forms) fs.Add(f);
			return PrintForms(fs);
		}
		private static string PrintForms(List<Gramadan.Form> forms)
		{
			string ret="";
			foreach(Form f in forms) {
				if(ret!="") ret+=" | ";
				ret+=f.value;
			}
			return ret;
		}
	}
}
