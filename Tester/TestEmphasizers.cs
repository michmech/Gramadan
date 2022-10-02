using System;
using System.Collections.Generic;
using Gramadan;
using System.IO;
using System.Xml;

namespace Tester
{
    class TestEmphasizers
    {
        public static void PossN() {
            List<Noun> nouns=new List<Noun>();
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\cóta_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\aturnae_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\turas_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\uirlis_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\óstán_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\veidhlín_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\tír_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\glór_masc1.xml"));

            List<Possessive> possessives=new List<Possessive>();
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\mo_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\do_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_masc.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_fem.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\ár_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\bhur_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_pl.xml"));

            StreamWriter writer=new StreamWriter(@"C:\MBM\Deleteme\test.txt");
            foreach(Noun n in nouns) {
                foreach(Possessive poss in possessives) {
                    NP np = new NP(n, poss, true);
                    writer.WriteLine(poss.getFriendlyNickname()+" + "+n.getLemma()+"\t"+np.sgNom[0].value+"\t"+np.sgGen[0].value+"\t"+np.plNom[0].value+"\t"+np.plGen[0].value);
                    Console.WriteLine(np.print());
                }
                writer.WriteLine();
            }
            writer.Close();
        }

        public static void PrepPossN() {
            List<Noun> nouns=new List<Noun>();
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\cóta_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\aturnae_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\turas_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\uirlis_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\óstán_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\veidhlín_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\tír_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\glór_masc1.xml"));

            List<Possessive> possessives=new List<Possessive>();
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\mo_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\do_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_masc.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_fem.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\ár_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\bhur_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_pl.xml"));

            List<Preposition> prepositions=new List<Preposition>();
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\ag_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\ar_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\as_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\chuig_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\de_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\do_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\faoi_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\i_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\le_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\ó_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\roimh_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\thar_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\trí_prep.xml"));
            prepositions.Add(new Preposition(@"C:\MBM\michmech\BuNaMo\preposition\um_prep.xml"));

            StreamWriter writer=new StreamWriter(@"C:\MBM\Deleteme\test.txt");
            foreach(Preposition prep in prepositions) {
                foreach(Noun n in nouns) {
                    foreach(Possessive poss in possessives) {
                        NP np = new NP(n, poss, true);
                        PP pp=new PP(prep, np);
                        writer.WriteLine(prep.getLemma()+" + "+poss.getFriendlyNickname()+" + "+n.getLemma()+"\t"+pp.sg[0].value+"\t"+pp.pl[0].value);
                        Console.WriteLine(pp.print());
                    }
                    writer.WriteLine();
                }
            }
            writer.Close();
        }
    }
}
