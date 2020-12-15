using System;
using System.Collections.Generic;
using System.Xml;

namespace Gramadan
{
	public class Utils
	{
		public static XmlDocument LoadXml(string fileName)
		{
			XmlDocument doc=new XmlDocument();
			doc.Load(fileName);
			return doc;
		}

		public static string LowerInit(string s) {
			if(s.Length > 1) {
				s=s.Substring(0, 1).ToLower()+s.Substring(1);
			}
			return s;
		}
		public static string UpperInit(string s) {
			if(s.Length > 1) {
				s=s.Substring(0, 1).ToUpper()+s.Substring(1);
			}
			return s;
		}
	}
}
