using System;
using CompCorpus.RunTime;
using CompCorpus;
using CompCorpus.RunTime.Bricks;
using CompCorpus.RunTime.declaration;
using System.Xml;
using System.Text;
using System.Collections.Generic;


namespace XmlForPluginEmitter
{
    public class XmlEmitter
    {
        internal static XmlWriter xmlWriter = null;
        internal static List<String> DBDeclarationName = new List<string>();
        internal static List<String> DBVariableName = new List<string>();
        internal static int nextId = 1;

        static void Main(string[] args)
        {
            buildXMLStringFromMontage(Program.CompileMain(@"C:\Users\j.folleas\Desktop\FichierTCcomp\source.txt", "", "", false));
            DocXLauncher.CreatSignatureDocxAndLaunchIt("adop.docx");
            Console.ReadLine();
        }


        public static void buildXMLStringFromMontage(Montage montage)
        {
            IterationXmlWriter.setknownVInameEmpty();
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                settings.OmitXmlDeclaration = false;
                xmlWriter = XmlWriter.Create(Constants.graphePath, settings);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Noeud");
                xmlWriter.WriteAttributeString("id", (nextId++).ToString());
                xmlWriter.WriteAttributeString("qualiacte", "0");
                xmlWriter.WriteAttributeString("thema", "");
                xmlWriter.WriteAttributeString("type", "Parcours");
                xmlWriter.WriteStartElement("Parcours");
                xmlWriter.WriteAttributeString("id", (nextId++).ToString());
                xmlWriter.WriteAttributeString("n", montage.nameOfTheMontage);
                xmlWriter.WriteAttributeString("t", montage.nameOfTheMontage);
                xmlWriter.WriteAttributeString("type", "Parcours");

                AddDeclarations(montage);
                AddBricks(montage);

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

        private static void AddDeclarations(Montage montage)
        {
            xmlWriter.WriteStartElement("Parcours");
            xmlWriter.WriteAttributeString("id", (nextId++).ToString());
            xmlWriter.WriteAttributeString("n", "Declarations");
            xmlWriter.WriteAttributeString("type", "Parcours");
            foreach (Declaration dec in montage.listOfDeclarations)
            {
                if (!dec.fromDataBase && !(dec.type == ExpressionType.LISTSTRUCT))
                {
                    xmlWriter.WriteStartElement("var");
                    xmlWriter.WriteAttributeString("n", dec.name);
                    xmlWriter.WriteAttributeString("c", VariableCallXmlWriter.computeType(new VariableCall(
                    dec.name, false, dec.type.ToString())));
                    xmlWriter.WriteEndElement(); // var }
                }
                if (dec.fromDataBase)
                {
                    AddDecFromDB(dec);
                }
            }
            foreach (Affectation aff in montage.mapOfCalculExpressions.Values)
            {
                if (aff.fromDataBase)
                {
                    DBVariableName.Add(aff.variableName.name);
                }
            }
            xmlWriter.WriteEndElement();
        }

        private static void AddDecFromDB(Declaration dec)
        {
           
            if (dec is DeclarationStruct)
            {
                foreach (Tuple<string, string> decItem in (dec as DeclarationStruct).GetSymboles())
                {
                    DBDeclarationName.Add(decItem.Item1);
                }
            }
            else
            {
                DBDeclarationName.Add(dec.name);
            }
        }

        private static void AddBricks(Montage montage)
        {
            foreach (Brick bk in montage.listOfBricks)
            {
                WriteXmlForBrick(bk);
            }
        }

        internal static void WriteXmlForBrick(Brick bk)
        {
           
            switch(bk)
            {
                case DeadText dt:
                    DeadTextXmlWriter.WriteXmlForDeadText(dt);
                    break;
                case Title tt:
                    TitleXmlWriter.WriteXmlForTitle(tt);
                        break;
                case VariableCall vc:
                    VariableCallXmlWriter.WriteXmlForVariableCall(vc);
                    break;
                case Option opt:
                    OptionXmlWriter.WriteXmlForOption(opt);
                    break;
                case Iteration itr:
                    IterationXmlWriter.WriteXmlForIteration(itr);
                    break;
                case Condition cond:
                    ConditionXmlWriter.WriteXmlForCondition(cond);
                    break;
                case Choice chx:
                    ChoixXmlWriter.WriteXmlForChoix(chx);
                    break;
                default:
                    throw new NotImplementedException();

            }
        }       
    }
}
