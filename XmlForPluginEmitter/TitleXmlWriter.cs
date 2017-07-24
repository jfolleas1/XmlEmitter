using CompCorpus.RunTime.Bricks;

namespace XmlForPluginEmitter
{
    internal class TitleXmlWriter
    {
        internal static void WriteXmlForTitle(Title tt)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("t", tt.text);
            XmlEmitter.xmlWriter.WriteEndElement();
        }
    }
}
