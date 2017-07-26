using CompCorpus.RunTime.Bricks;
using System.Text.RegularExpressions;

namespace XmlForPluginEmitter
{
    internal static class DeadTextXmlWriter
    { 
        internal static void WriteXmlForDeadText(DeadText dt)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Clause");
            XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Clause");
            XmlEmitter.xmlWriter.WriteStartElement("html");
            XmlEmitter.xmlWriter.WriteString(" "+ InsertNewLine(dt.Write()) + " ");
            XmlEmitter.xmlWriter.WriteEndElement();
            XmlEmitter.xmlWriter.WriteEndElement();
        }


        private static string InsertNewLine(string input)
        {
            string output = "";
            string patern = @"<br\/>";
            Regex rgx = new Regex(patern);
            output = rgx.Replace(input, "<br/>&nbsp;");
            return output;
        }
    }
}
