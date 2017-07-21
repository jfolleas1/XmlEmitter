using CompCorpus.RunTime.Bricks;

namespace XmlForPluginEmitter
{
    static class OptionXmlWriter
    {
        static public void WriteXmlForOption(Option opt)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("n", opt.textOfOption.Substring(
                1, opt.textOfOption.Length - 2));
            XmlEmitter.xmlWriter.WriteAttributeString("o", "true");
            foreach (Brick bk in opt.brickList)
            {
                XmlEmitter.WriteXmlForBrick(bk);
            }
            XmlEmitter.xmlWriter.WriteEndElement();
        }
    }
}
