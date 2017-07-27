using CompCorpus.RunTime.Bricks;
using System.Text.RegularExpressions;

namespace XmlForPluginEmitter
{
    internal static class DeadTextXmlWriter
    { 
        internal static void WriteXmlForDeadText(DeadText dt)
        {
            string[] listeOfClauseWithoutNewLine = Regex.Split(dt.Write(), @"<br/>");
            string last = listeOfClauseWithoutNewLine[listeOfClauseWithoutNewLine.Length - 1];
            foreach (string text in listeOfClauseWithoutNewLine)
            {
                XmlEmitter.xmlWriter.WriteStartElement("Clause");
                XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
                XmlEmitter.xmlWriter.WriteAttributeString("type", "Clause");
                XmlEmitter.xmlWriter.WriteStartElement("html");
                XmlEmitter.xmlWriter.WriteString(" " + text + " ");
                XmlEmitter.xmlWriter.WriteEndElement();
                XmlEmitter.xmlWriter.WriteEndElement();

                if (text != last)
                {
                    XmlEmitter.xmlWriter.WriteStartElement("Clause");
                    XmlEmitter.xmlWriter.WriteAttributeString("id", "2664");
                    XmlEmitter.xmlWriter.WriteAttributeString("type", "Clause");
                    XmlEmitter.xmlWriter.WriteStartElement("html");
                    XmlEmitter.xmlWriter.WriteString("<br/>\n&nbsp;");
                    XmlEmitter.xmlWriter.WriteEndElement();
                    XmlEmitter.xmlWriter.WriteEndElement();
                }
            }
           
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
