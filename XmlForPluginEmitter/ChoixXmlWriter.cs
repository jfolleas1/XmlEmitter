using System;
using CompCorpus.RunTime.Bricks;

namespace XmlForPluginEmitter
{
    static class ChoixXmlWriter
    {
        static public void WriteXmlForChoix(Choice chx)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Choix");
            XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Choix");
            XmlEmitter.xmlWriter.WriteAttributeString("n", chx.textOfChoice.Substring(
                1, chx.textOfChoice.Length - 2));
            foreach (Proposition ppstion in chx.propositionList)
            {
                WriteXmlForProposition(ppstion);
            }
            XmlEmitter.xmlWriter.WriteEndElement();
        }

        private static void WriteXmlForProposition(Proposition ppstion)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("n", ppstion.textOfChoice.Substring(
                0, ppstion.textOfChoice.Length - 1));
            foreach (Brick bk in ppstion.brickList)
            {
                XmlEmitter.WriteXmlForBrick(bk);
            }
            XmlEmitter.xmlWriter.WriteEndElement();
        }
    }
}
