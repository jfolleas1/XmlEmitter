using CompCorpus.RunTime.Bricks;
using System;
using System.Collections.Generic;

namespace XmlForPluginEmitter
{
    static class IterationXmlWriter
    {
        static private List<string> knownVIname = new List<string>();
        static public List<string> pileOfIterator = new List<string>();
        static public List<string> pileOfIteratorInReprise = new List<string>();
        static public List<string> pileOfIteratorNotFromDB = new List<string>();

        public static void setknownVInameEmpty()
        {
            knownVIname = new List<string>();
        }

        static private bool VIAlreadyKnown(string VIname)
        {
            Console.WriteLine("VI : " + VIname);
            if (knownVIname.Contains(VIname))
            {
                Console.WriteLine("TRUE");
                return true;
            }
            else
            {
                Console.WriteLine("FALSE");
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
                XmlEmitter.xmlWriter.WriteAttributeString("n", Slugify(itr.listData.name)); // To sflugify
                iteratorName = itr.listData.name;
                pileOfIteratorNotFromDB.Add(itr.iteratorName);
                XmlEmitter.xmlWriter.WriteAttributeString("c", "VM");
                if (VIAlreadyKnown(iteratorName))
                {
                    XmlEmitter.xmlWriter.WriteAttributeString("r", "true");
                    pileOfIteratorInReprise.Add(itr.iteratorName);
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

            pileOfIterator.Add(itr.iteratorName);
            foreach (Brick bk in itr.brickList)
            {
                XmlEmitter.WriteXmlForBrick(bk);
            }
            if ((pileOfIteratorNotFromDB.LastIndexOf(itr.iteratorName) > -1) && (pileOfIteratorNotFromDB.LastIndexOf(itr.iteratorName) == (pileOfIteratorNotFromDB.Count-1)))
            {
                pileOfIteratorNotFromDB.RemoveAt(pileOfIteratorNotFromDB.Count - 1);
                if ((pileOfIteratorInReprise.LastIndexOf(itr.iteratorName) > -1 ) && (pileOfIteratorInReprise.LastIndexOf(itr.iteratorName) == (pileOfIteratorInReprise.Count-1)))
                {
                    pileOfIteratorInReprise.RemoveAt(pileOfIteratorInReprise.Count - 1);
                }
            }
            pileOfIterator.RemoveAt(pileOfIterator.Count - 1);
            XmlEmitter.xmlWriter.WriteEndElement();
        }


        internal static string Slugify(string varname)
        {
            string[] splitedVarName = varname.Split('.');
            string res = splitedVarName[0];
            for (int i = 1; i < splitedVarName.Length; i++)
            {
                res += splitedVarName[i].Substring(0, 1).ToUpper() + splitedVarName[i].Substring(1);
            }
            return res;
        }
    }
}
