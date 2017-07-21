using CompCorpus.RunTime.Bricks;

namespace XmlForPluginEmitter
{
    internal static class DeadTextXmlWriter
    { 
        internal static void WriteXmlForDeadText(DeadText dt)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Clause");
            XmlEmitter.xmlWriter.WriteAttributeString("id", "123");
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Clause");
            XmlEmitter.xmlWriter.WriteStartElement("html");
            XmlEmitter.xmlWriter.WriteString(dt.Write() + " ");
            XmlEmitter.xmlWriter.WriteEndElement();
            XmlEmitter.xmlWriter.WriteEndElement();
        }
    }
}
