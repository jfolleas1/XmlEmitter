using CompCorpus.RunTime.Bricks;
using System.Collections.Generic;

namespace XmlForPluginEmitter
{
    static class IterationXmlWriter
    {
        static private List<string> knownVIname = new List<string>();

        static private bool VIAlreadyKnown(string VIname)
        {
            if (knownVIname.Contains(VIname))
            {
                return true;
            }
            else
            {
                knownVIname.Add(VIname);
                return false;
            }
        }

        static public void WriteXmlForIteration(Iteration itr)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("a", "true");
            XmlEmitter.xmlWriter.WriteAttributeString("n", "iteration sur " + itr.listData.name);
            XmlEmitter.xmlWriter.WriteAttributeString("f", "false");

            XmlEmitter.xmlWriter.WriteStartElement("vi");
            XmlEmitter.xmlWriter.WriteAttributeString("n", itr.listData.name);
            XmlEmitter.xmlWriter.WriteAttributeString("c", "VM");
            if (VIAlreadyKnown(itr.iteratorName))
            {
                XmlEmitter.xmlWriter.WriteAttributeString("r", "true");
            }
            XmlEmitter.xmlWriter.WriteEndElement();

            foreach (Brick bk in itr.brickList)
            {
                XmlEmitter.WriteXmlForBrick(bk);
            }

            XmlEmitter.xmlWriter.WriteEndElement();
        }
    }
}
