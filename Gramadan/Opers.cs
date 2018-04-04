using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gramadan
{
	public class Opers
	{
		public static string Demutate(string text) {
			string pattern;
			pattern="^[bB][hH]([fF].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^([bcdfgmpstBCDFGMPST])[hH](.*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1$2");
			pattern="^[mM]([bB].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[gG]([cC].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[nN]([dD].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[nN]([gG].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[bB]([pP].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[tT]([sS].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[dD]([tT].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[dD]'([fF])[hH](.*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1$2");
			pattern="^[dD]'([aeiouaáéíóúAEIOUÁÉÍÓÚ].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[hH]([aeiouaáéíóúAEIOUÁÉÍÓÚ].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			pattern="^[nN]-([aeiouaáéíóúAEIOUÁÉÍÓÚ].*)$";
			if(Regex.IsMatch(text, pattern)) text=Regex.Replace(text, pattern, "$1");
			return text;
		}
		
		//Performs a mutation on the string:
		public static string Mutate(Mutation mutation, string text)
		{
			string ret="";
			string pattern;

			if(mutation==Mutation.Len1 || mutation==Mutation.Len1D) {
				//lenition 1
				if(ret=="") { pattern="^([pbmftdcgPBMFTDCG])[jJ]"; if(Regex.IsMatch(text, pattern)) ret=text; } //do not mutate exotic words with J in second position, like Djibouti
				if(ret=="") { pattern="^([pbmftdcgPBMFTDCG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "$1h$2"); }
				if(ret=="") { pattern="^([sS])([rnlRNLaeiouáéíóúAEIOUÁÉÍÓÚ].*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "$1h$2"); }
				if(ret=="") ret=text;
				if(mutation==Mutation.Len1D) { pattern="^([aeiouáéíóúAEIOUÁÉÍÓÚfF])(.*)$"; if(Regex.IsMatch(ret, pattern)) ret=Regex.Replace(ret, pattern, "d'$1$2"); }
			} else if(mutation==Mutation.Len2 || mutation==Mutation.Len2D) {
				//lenition 2: same as lenition 1 but leaves "d", "t" and "s" unmutated
				if(ret=="") { pattern="^([pbmftdcgPBMFTDCG])[jJ]"; if(Regex.IsMatch(text, pattern)) ret=text; } //do not mutate exotic words with J in second position, like Djibouti
				if(ret=="") { pattern="^([pbmfcgPBMFCG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "$1h$2"); }
				if(ret=="") ret=text;
				if(mutation==Mutation.Len2D) { pattern="^([aeiouáéíóúAEIOUÁÉÍÓÚfF])(.*)$"; if(Regex.IsMatch(ret, pattern)) ret=Regex.Replace(ret, pattern, "d'$1$2"); }
			} else if(mutation==Mutation.Len3 || mutation==Mutation.Len3D) {
				//lenition 3: same as lenition 2 but also changes "s" into "ts"
				if(ret=="") { pattern="^([pbmftdcgPBMFTDCG])[jJ]"; if(Regex.IsMatch(text, pattern)) ret=text; } //do not mutate exotic words with J in second position, like Djibouti
				if(ret=="") { pattern="^([pbmfcgPBMFCG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "$1h$2"); }
				if(ret=="") { pattern="^([sS])([rnlRNLaeiouáéíóúAEIOUÁÉÍÓÚ].*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "t$1$2"); }
				if(ret=="") ret=text;
				if(mutation==Mutation.Len3D) { pattern="^([aeiouáéíóúAEIOUÁÉÍÓÚfF])(.*)$"; if(Regex.IsMatch(ret, pattern)) ret=Regex.Replace(ret, pattern, "d'$1$2"); }
			} else if(mutation==Mutation.Ecl1) {
				//eclisis 1
				if(ret=="") { pattern="^([pP])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "b$1$2"); }
				if(ret=="") { pattern="^([bB])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "m$1$2"); }
				if(ret=="") { pattern="^([fF])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "bh$1$2"); }
				if(ret=="") { pattern="^([cC])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "g$1$2"); }
				if(ret=="") { pattern="^([gG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
				if(ret=="") { pattern="^([tT])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "d$1$2"); }
				if(ret=="") { pattern="^([dD])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
				if(ret=="") { pattern="^([aeiuoáéíúó])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n-$1$2"); }
				if(ret=="") { pattern="^([AEIUOÁÉÍÚÓ])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
			} else if(mutation==Mutation.Ecl1x) {
				//eclisis 1x: same as eclipsis 1 but leaves vowels unchanged
				if(ret=="") { pattern="^([pP])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "b$1$2"); }
				if(ret=="") { pattern="^([bB])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "m$1$2"); }
				if(ret=="") { pattern="^([fF])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "bh$1$2"); }
				if(ret=="") { pattern="^([cC])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "g$1$2"); }
				if(ret=="") { pattern="^([gG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
				if(ret=="") { pattern="^([tT])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "d$1$2"); }
				if(ret=="") { pattern="^([dD])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
			} else if(mutation==Mutation.Ecl2) {
				//eclipsis 2: same as eclipsis 1 but leaves "t", "d" and vowels unchanged
				if(ret=="") { pattern="^([pP])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "b$1$2"); }
				if(ret=="") { pattern="^([bB])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "m$1$2"); }
				if(ret=="") { pattern="^([fF])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "bh$1$2"); }
				if(ret=="") { pattern="^([cC])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "g$1$2"); }
				if(ret=="") { pattern="^([gG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
			} else if(mutation==Mutation.Ecl3) {
				//eclipsis 3: same as eclipsis 2 but also changes "s" to "ts"
				if(ret=="") { pattern="^([pP])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "b$1$2"); }
				if(ret=="") { pattern="^([bB])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "m$1$2"); }
				if(ret=="") { pattern="^([fF])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "bh$1$2"); }
				if(ret=="") { pattern="^([cC])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "g$1$2"); }
				if(ret=="") { pattern="^([gG])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "n$1$2"); }
				if(ret=="") { pattern="^([sS])([rnlRNLaeiouáéíóúAEIOUÁÉÍÓÚ].*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "t$1$2"); }
			} else if(mutation==Mutation.PrefT) {
				//t-prefixation
				if(ret=="") { pattern="^([aeiuoáéíúó])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "t-$1$2"); }
				if(ret=="") { pattern="^([AEIUOÁÉÍÚÓ])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "t$1$2"); }
			} else if(mutation==Mutation.PrefH) {
				//h-prefixation
				if(ret=="") { pattern="^([aeiuoáéíúóAEIUOÁÉÍÚÓ])(.*)$"; if(Regex.IsMatch(text, pattern)) ret=Regex.Replace(text, pattern, "h$1$2"); }
			}

			if(ret=="") ret=text;
			return ret;
		}

		//Tells you whether the string ends in a "dentals" cosonant:
		public static bool EndsDental(string txt)
		{
			return Regex.IsMatch(txt, "[dntsDNTS]$");
		}

		//Tells you whether the string ends in a slender consonant cluster:
		public static bool IsSlender(string txt)
		{
			return Regex.IsMatch(txt, "[eiéí][^aeiouáéíóú]+$");
		}

		//Tells you whether the string has a vowel or 'fh' (but not 'fhl' or 'fhr') at its start:
		public static bool StartsVowelFhx(string txt)
		{
			bool ret=false;
			if(Regex.IsMatch(txt, "^[aeiouáéíóúAEIOUÁÉÍÓÚ]")) ret=true;
			if(Regex.IsMatch(txt, "^fh[^lr]", RegexOptions.IgnoreCase)) ret=true;
			return ret;
		}

		//Tells you whether the string ends in a vowel:
		public static bool EndsVowel(string txt)
		{
			bool ret=false;
			if(Regex.IsMatch(txt, "[aeiouáéíóúAEIOUÁÉÍÓÚ]$")) ret=true;
			return ret;
		}

		//Tells you whether the string starts in a vowel:
		public static bool StartsVowel(string txt)
		{
			bool ret=false;
			if(Regex.IsMatch(txt, "^[aeiouáéíóúAEIOUÁÉÍÓÚ]")) ret=true;
			return ret;
		}

		//Tells you whether the string starts in b, m, p:
		public static bool StartsBilabial(string txt)
		{
			bool ret=false;
			if(Regex.IsMatch(txt, "^[bmpBMP]")) ret=true;
			return ret;
		}

		//Character types, for convenience when writing regular expressions:
		public static string Cosonants="bcdfghjklmnpqrstvwxz";
		public static string Vowels="aeiouáéíóú";
		public static string VowelsBroad="aouáóú";
		public static string VowelsSlender="eiéí";
		
		//Performs regular slenderization (attenuation): if the base ends in a consonant, and if the vowel cluster immediately before this consonant
		//ends in a broad vowel, then it changes this vowel cluster such that it ends in a slender vowel now.
		//Note: a base that's already slender passes through unchanged.
		public static string Slenderize(string bayse)
		{
			string ret=bayse;

			string[] sources=new string[] { "ea", "éa", "ia", "ío", "io", "iu", "ae"  };
			string[] targets=new string[] { "i",  "éi", "éi", "í",  "i",  "i",  "aei" };
			Match match;
			for(int i=0; i<sources.Length; i++) {
				match=Regex.Match(bayse, "^(.*["+Opers.Cosonants+"])?"+sources[i]+"(["+Opers.Cosonants+"]+)$");
				if(match.Success) { ret=match.Groups[1].Value+targets[i]+match.Groups[2].Value; return ret; }
			}

			//The generic case: insert "i" at the end of the vowel cluster:
			match=Regex.Match(bayse, "^(.*["+Opers.VowelsBroad+"])(["+Opers.Cosonants+"]+)$");
			if(match.Success) ret=match.Groups[1].Value+"i"+match.Groups[2].Value;

			return ret;
		}

		//Performs irregular slenderization (attenuation): if the base ends in a consonant, and if the vowel cluster immediately before this consonant
		//ends in a broad vowel, then it changes this vowel cluster into the target (the second argument).
		//Note: if the target does not end in a slender vowel, then regular slenderization is attempted instead.
		//Note: a base that's already attenuated passes through unchanged.
		public static string Slenderize(string bayse, string target)
		{
			string ret=bayse;
			if(!Regex.IsMatch(target, "["+Opers.VowelsSlender+"]$")) {
				ret=Opers.Slenderize(bayse); //attempt regular slenderization instead
			} else {
				Match match=Regex.Match(bayse, "^(.*?)["+Opers.Vowels+"]*["+Opers.VowelsBroad+"](["+Opers.Cosonants+"]+)$");
				if(match.Success) ret=match.Groups[1].Value+target+match.Groups[2].Value;
			}
			return ret;
		}

		//Performs regular broadening: if the base ends in a consonant, and if the vowel cluster immediately before this consonant
		//ends in a slender vowel, then it changes this vowel cluster such that it ends in a broad vowel now.
		//Note: a base that's already broad passes through unchanged.
		public static string Broaden(string bayse)
		{
			string ret=bayse;

			string[] sources=new string[] { "ói", "ei", "éi", "i",  "aí",  "í",  "ui", "io" };
			string[] targets=new string[] { "ó",  "ea", "éa", "ea", "aío", "ío", "o",  "ea" };
			Match match;
			for(int i=0; i<sources.Length; i++) {
				match=Regex.Match(bayse, "^(.*["+Opers.Cosonants+"])?"+sources[i]+"(["+Opers.Cosonants+"]+)$");
				if(match.Success) { ret=match.Groups[1].Value+targets[i]+match.Groups[2].Value; return ret; }
			}

			//The generic case: remove "i" from the end of the vowel cluster:
			match=Regex.Match(bayse, "^(.*)i(["+Opers.Cosonants+"]+)$");
			if(match.Success) ret=match.Groups[1].Value+match.Groups[2].Value;

			return ret;
		}

		//Performs irregular broadening: if the base ends in a consonant, and if the vowel cluster immediately before this consonant
		//ends in a slender vowel, then it changes this vowel cluster into the target (the second argument).
		//Note: if the target does not end in a broad vowel, then regular broadening is attempted instead.
		//Note: a base that's already broad passes through unchanged.
		public static string Broaden(string bayse, string target)
		{
			string ret=bayse;
			if(!Regex.IsMatch(target, "["+Opers.VowelsBroad+"]$")) {
				ret=Opers.Broaden(bayse); //attempt regular broadening instead
			} else {
				Match match=Regex.Match(bayse, "^(.*?)["+Opers.Vowels+"]*["+Opers.VowelsSlender+"](["+Opers.Cosonants+"]+)$");
				if(match.Success) ret=match.Groups[1].Value+target+match.Groups[2].Value;
			}
			return ret;
		}

		//If the final consonant cluster consists of two consonants that differ in voicing,
		//and if neither one of them is "l", "n" or "r", then devoices the second one.
		public static string Devoice(string bayse)
		{
			string ret=bayse;
			Match match=Regex.Match(bayse, "^(.*)sd$"); if(match.Success) { ret=match.Groups[1].Value+"st"; return ret; }
			//May need elaboration.
			return ret;
		}

		//Reduces any duplicated consonants at the end into a single consonant.
		public static string Unduplicate(string bayse)
		{
			string ret=bayse;
			
			Match match=Regex.Match(bayse, "^.*["+Opers.Cosonants+"]["+Opers.Cosonants+"]$");
			if(match.Success && bayse[bayse.Length-1]==bayse[bayse.Length-2]) ret=bayse.Substring(0, bayse.Length-1);

			return ret;
		}

		//Performs syncope by removing the final vowel cluster,
		//then unduplicates and devoices the consonant cluster at the end.
		public static string Syncope(string bayse)
		{
			string ret=bayse;

			Match match=Regex.Match(bayse, "^(.*["+Opers.Cosonants+"])?["+Opers.Vowels+"]+(["+Opers.Cosonants+"]+)$");
			if(match.Success) ret=Opers.Devoice(Opers.Unduplicate(match.Groups[1].Value+match.Groups[2].Value));

			return ret;
		}

		public static string HighlightMutations(string text)
		{
			return HighlightMutations(text, "");
		}
		public static string HighlightMutations(string text, string bayse)
		{
			text=Regex.Replace(text, "(^| )([cdfgmpst])(h)", "$1$2<u class='lenition'>$3</u>", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(b)(h)([^f])", "$1$2<u class='lenition'>$3</u>$4", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(t)(s)", "$1<u class='lenition'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(m)(b)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(g)(c)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(n)(d)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(bh)(f)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(n)(g)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(b)(p)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(d)(t)", "$1<u class='eclipsis'>$2</u>$3", RegexOptions.IgnoreCase);
			text=Regex.Replace(text, "(^| )(n-)([aeiouáéíóú])", "$1<u class='eclipsis'>$2</u>$3");
			if(!bayse.StartsWith("n")) text=Regex.Replace(text, "(^| )(n)([AEIOUÁÉÍÓÚ])", "$1<u class='eclipsis'>$2</u>$3");
			if(!bayse.StartsWith("t-")) text=Regex.Replace(text, "(^| )(t-)([aeiouáéíóú])", "$1<u class='lenition'>$2</u>$3");
			if(!bayse.StartsWith("t")) text=Regex.Replace(text, "(^| )(t)([AEIOUÁÉÍÓÚ])", "$1<u class='lenition'>$2</u>$3");
			if(!bayse.StartsWith("h")) text=Regex.Replace(text, "(^| )(h)([aeiouáéíóú])", "$1<u class='lenition'>$2</u>$3", RegexOptions.IgnoreCase);
			return text;
		}

		public static string Prefix(string prefix, string body)
		{
			Mutation m=Mutation.Len1; if(Opers.EndsDental(prefix)) m=Mutation.Len2; //pick the right mutation
			if(prefix.Substring(prefix.Length-1)==body.Substring(0)) prefix+="-"; //eg. "sean-nós"
			if(EndsVowel(prefix) && StartsVowel(body)) prefix+="-"; //eg. "ró-éasca"
			if(body.Substring(0, 1)==body.Substring(0, 1).ToUpper()) { //eg. "seanÉireannach" > "Sean-Éireannach"
				prefix=prefix.Substring(0, 1).ToUpper()+prefix.Substring(1);
				if(!prefix.EndsWith("-")) prefix+="-";
			}
			string ret=prefix+Mutate(m, body);
			return ret;
		}
	}
}
