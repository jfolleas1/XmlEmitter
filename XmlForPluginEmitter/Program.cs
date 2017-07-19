using System;
using CompCorpus.RunTime;
using CompCorpus;
using System.Collections;
using CompCorpus.RunTime.Bricks;
using System.Xml;
using System.Text;

namespace XmlForPluginEmitter
{
    public class XmlEmitter
    {
        private static XmlWriter xmlWriter = null;
        private static ArrayList variableNameListIndexByWid = new ArrayList();
        static void Main(string[] args)
        {
            buildXMLStringFromMontage(Program.CompileMain(@"C:\Users\j.folleas\Desktop\FichierTCcomp\source.txt", "", "", false));
            DocXLauncher.CreatSignatureDocxAndLaunchIt("adop.docx");
        }


        public static void buildXMLStringFromMontage(Montage montage)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = false;
                xmlWriter = XmlWriter.Create(Constants.graphePath, settings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Noeud");
                xmlWriter.WriteAttributeString("id", "123456");
                xmlWriter.WriteAttributeString("n", montage.nameOfTheMontage);
                xmlWriter.WriteAttributeString("qualiacte", "0");
                xmlWriter.WriteAttributeString("t", montage.nameOfTheMontage);
                xmlWriter.WriteAttributeString("thema", "");
                xmlWriter.WriteAttributeString("type", "Parcours");
                xmlWriter.WriteStartElement("Parcours");
                xmlWriter.WriteAttributeString("n", "root");
                xmlWriter.WriteAttributeString("type", "Parcours");
                foreach (Brick bk in montage.listOfBricks)
                {
                    WriteXmlForBrick(bk);
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                
                xmlWriter.Flush();
                xmlWriter.Close();
            }

            finally {
                if (xmlWriter != null)
                    xmlWriter.Close();
            }

        }

        private static void WriteXmlForBrick(Brick bk)
        {
           
            switch(bk)
            {
                case DeadText dt:
                    WriteXmlForDeadText(dt);
                    break;
                case Title tt:
                    WriteXmlForTitle(tt);
                        break;
                case VariableCall vc:
                    WriteXmlForVariableCall(vc);
                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        private static void WriteXmlForVariableCall(VariableCall vc)
        {
            xmlWriter.WriteStartElement("Noeud");
            xmlWriter.WriteAttributeString("id", "233");
            xmlWriter.WriteAttributeString("type", "Clause");

            xmlWriter.WriteStartElement("var");
            xmlWriter.WriteAttributeString("wId", "V__" + computeWidInString(vc.text));
            xmlWriter.WriteAttributeString("n", vc.text);
            xmlWriter.WriteAttributeString("c", computeType(vc));
            //xmlWriter.WriteAttributeString("r", "true");
            xmlWriter.WriteEndElement(); // var

            xmlWriter.WriteStartElement("html");
            xmlWriter.WriteString("<var id=\"" + vc.text + "\">" + vc.text + "</var>");
            xmlWriter.WriteEndElement(); // html

            xmlWriter.WriteEndElement(); // Clause
        }

        private static string computeType(VariableCall vc)
        {
            if (vc.typeString == ExpressionType.NOMBRE.ToString())
            {
                return "N";
            }else if (vc.typeString == ExpressionType.TEXTE.ToString())
            {
                return "S";
            }
            else
            { 
               throw new Exception(vc.GetType().ToString() + "This type of var is not already supported by the emiteur");
            }
        }

        private static string computeWidInString(string variableName)
        {
            int potentialWid = variableNameListIndexByWid.IndexOf(variableName);
            if (potentialWid >= 0)
            {
                return (potentialWid + 101).ToString();
            }
            else
            {
                variableNameListIndexByWid.Add(variableName);
                return (variableNameListIndexByWid.Count + 100).ToString();
            }
        }


        private static void WriteXmlForTitle(Title tt)
        {
            xmlWriter.WriteStartElement("Noeud");
            xmlWriter.WriteAttributeString("type", "Parcours");
            xmlWriter.WriteAttributeString("t", tt.text);
            xmlWriter.WriteEndElement();
        }

        

        private static void WriteXmlForDeadText(DeadText dt)
        {
            xmlWriter.WriteStartElement("Noeud");
            xmlWriter.WriteAttributeString("id", "123");
            xmlWriter.WriteAttributeString("type", "Clause");
            xmlWriter.WriteStartElement("html");
            xmlWriter.WriteString(dt.Write()+" ");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

       
    }
}
