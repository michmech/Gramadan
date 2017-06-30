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
	}
}
