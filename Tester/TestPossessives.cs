using System;
using System.Collections.Generic;
using Gramadan;
using System.IO;
using System.Xml;

namespace Tester
{
    class TestPossessives
    {
        public static void PossNP() {
            List<Noun> nouns=new List<Noun>();
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\árasán_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\bó_fem.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\comhlacht_masc3.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\dealbh_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\éiceachóras_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\francfurtar_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\fliúit_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\fadhb_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\fobhríste_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\garáiste_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\haematóma_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\iasacht_fem3.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\jab_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\leabharlann_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\máthair_fem5.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\nóta_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\ócáid_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\pacáiste_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\rás_masc3.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\sobaldráma_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\sábh_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\stábla_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\sráid_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\tábhairne_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\ubh_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\x-gha_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\zombaí_masc4.xml"));

            List<Possessive> possessives=new List<Possessive>();
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\mo_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\do_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_masc.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_fem.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\ár_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\bhur_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_pl.xml"));

            Adjective adj=new Adjective(@"C:\MBM\michmech\BuNaMo\adjective\mór_adj1.xml");

            StreamWriter writer=new StreamWriter(@"C:\MBM\Deleteme\test.txt");
            foreach(Noun n in nouns) {
                foreach(Possessive poss in possessives) {
                    NP np = new NP(n, adj, poss);
                    writer.WriteLine(poss.getFriendlyNickname()+"\t"+np.sgNom[0].value+"\t"+np.sgGen[0].value+"\t"+np.plNom[0].value+"\t"+np.plGen[0].value);
                    Console.WriteLine(np.print());
                }
                writer.WriteLine();
            }
            writer.Close();
        }

        public static void PrepPossNP() {
            List<Noun> nouns=new List<Noun>();
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\árasán_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\bó_fem.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\comhlacht_masc3.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\dealbh_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\éiceachóras_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\francfurtar_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\fliúit_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\fadhb_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\fobhríste_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\garáiste_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\haematóma_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\iasacht_fem3.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\jab_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\leabharlann_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\máthair_fem5.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\nóta_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\ócáid_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\pacáiste_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\rás_masc3.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\sobaldráma_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\sábh_masc1.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\stábla_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\sráid_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\tábhairne_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\ubh_fem2.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\x-gha_masc4.xml"));
            nouns.Add(new Noun(@"C:\MBM\michmech\BuNaMo\noun\zombaí_masc4.xml"));

            List<Possessive> possessives=new List<Possessive>();
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\mo_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\do_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_masc.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_fem.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\ár_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\bhur_poss.xml"));
            possessives.Add(new Possessive(@"C:\MBM\michmech\BuNaMo\possessive\a_poss_pl.xml"));

            Adjective adj=new Adjective(@"C:\MBM\michmech\BuNaMo\adjective\mór_adj1.xml");

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
                        NP np = new NP(n, adj, poss);
                        PP pp=new PP(prep, np);
                        writer.WriteLine("["+prep.getLemma()+" + "+poss.getFriendlyNickname()+"]\t"+pp.sg[0].value+"\t"+pp.pl[0].value);
                        Console.WriteLine(pp.print());
                    }
                    writer.WriteLine();
                }
            }
            writer.Close();
        }
    }
}
