using CompCorpus.RunTime.Bricks;
using System.Collections.Generic;

namespace XmlForPluginEmitter
{
    static class IterationXmlWriter
    {
        static private List<string> knownVIname = new List<string>();
        static public List<string> pileOfIterator = new List<string>();



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
            XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("a", "true");
            string name = itr.listData.name;
            if (name[0] == 'B' && name[1] == 'D' && name[2] == '.')
            {
                name = name.Substring(3);
            }
            XmlEmitter.xmlWriter.WriteAttributeString("n", "iteration sur " + itr.listData.name);
            XmlEmitter.xmlWriter.WriteAttributeString("f", "false");

            string iteratorName = "";
            if (itr.listData.name[0] != 'B' || itr.listData.name[1] != 'D' || itr.listData.name[2] != '.')
            {
                XmlEmitter.xmlWriter.WriteStartElement("vi");
                XmlEmitter.xmlWriter.WriteAttributeString("n", itr.listData.name);
                iteratorName = itr.listData.name;
                XmlEmitter.xmlWriter.WriteAttributeString("c", "VM");
                if (VIAlreadyKnown(itr.iteratorName))
                {
                    XmlEmitter.xmlWriter.WriteAttributeString("r", "true");
                }
                XmlEmitter.xmlWriter.WriteEndElement();
            }
            else
            {
                XmlEmitter.xmlWriter.WriteStartElement("vi");
                XmlEmitter.xmlWriter.WriteAttributeString("n", itr.iteratorName);
                iteratorName = itr.iteratorName;
                XmlEmitter.xmlWriter.WriteAttributeString("c", "VM");
                XmlEmitter.xmlWriter.WriteAttributeString("e", itr.listData.name.Substring(3));
                XmlEmitter.xmlWriter.WriteEndElement();
            }

            pileOfIterator.Add(iteratorName);
            foreach (Brick bk in itr.brickList)
            {
                XmlEmitter.WriteXmlForBrick(bk);
            }
            pileOfIterator.RemoveAt(pileOfIterator.Count - 1);
            XmlEmitter.xmlWriter.WriteEndElement();
        }
    }
}
