using CompCorpus.RunTime.Bricks;

namespace XmlForPluginEmitter
{
    internal class TitleXmlWriter
    {
        internal static void WriteXmlForTitle(Title tt)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteEndElement();
            XmlEmitter.xmlWriter.WriteAttributeString("t", tt.text);
        }
    }
}
