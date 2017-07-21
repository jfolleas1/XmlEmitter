using System;
using CompCorpus.RunTime;
using CompCorpus;
using System.Collections;
using CompCorpus.RunTime.Bricks;
using CompCorpus.RunTime.declaration;
using System.Xml;
using System.Text;
using System.Collections.Generic;


namespace XmlForPluginEmitter
{
    public class XmlEmitter
    {
        private static XmlWriter xmlWriter = null;
        static void Main(string[] args)
        {
            buildXMLStringFromMontage(Program.CompileMain(@"C:\Users\j.folleas\Desktop\FichierTCcomp\source.txt", "", "", false));
            DocXLauncher.CreatSignatureDocxAndLaunchIt("adop.docx");
            Console.ReadLine();
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
                xmlWriter.WriteAttributeString("qualiacte", "0");
                xmlWriter.WriteAttributeString("thema", "");
                xmlWriter.WriteAttributeString("type", "Parcours");
                xmlWriter.WriteStartElement("Parcours");
                xmlWriter.WriteAttributeString("n", montage.nameOfTheMontage);
                xmlWriter.WriteAttributeString("t", montage.nameOfTheMontage);
                xmlWriter.WriteAttributeString("type", "Parcours");

                AddDeclarations(montage);

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

        private static void AddDeclarations(Montage montage)
        {
            xmlWriter.WriteStartElement("Parcours");
            xmlWriter.WriteAttributeString("n", "Declarations");
            xmlWriter.WriteAttributeString("type", "Parcours");
            foreach (Declaration dec in montage.listOfDeclarations)
            {
                if (!dec.fromDataBase)
                {
                    xmlWriter.WriteStartElement("var");
                    xmlWriter.WriteAttributeString("n", dec.name);
                    xmlWriter.WriteAttributeString("c", computeType(new VariableCall(
                    dec.name, false, dec.type.ToString())));
                    xmlWriter.WriteEndElement(); // var }
                }
            }
            xmlWriter.WriteEndElement();
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
            xmlWriter.WriteStartElement("Parcours");
            xmlWriter.WriteAttributeString("id", "233");
            xmlWriter.WriteAttributeString("type", "Clause");

            if (vc.expression != null) // we declar all the variable needed
            {
                insertVarIdOfExpression(vc.expression);
            }

            xmlWriter.WriteStartElement("var");
            if (vc.expression == null)
            {
                xmlWriter.WriteAttributeString("r", "true");
            }
            xmlWriter.WriteAttributeString("n", vc.name);
            xmlWriter.WriteAttributeString("c", computeType(vc));
            if (vc.expression != null)
            {
                xmlWriter.WriteAttributeString("e", writeXmlExpression(vc.expression));
            }
            xmlWriter.WriteEndElement(); // var

            xmlWriter.WriteStartElement("html");
            xmlWriter.WriteString("<var id=\"" + vc.name + "\">" + vc.name + "</var>");
            xmlWriter.WriteEndElement(); // html

            xmlWriter.WriteEndElement(); // Clause
        }

        private static void insertVarIdOfExpression(AbstractExpression expression)
        {
            ArrayList listOfVarId = listOfVarIdInExpression(expression);
            foreach (VariableId var in listOfVarId)
            {
                xmlWriter.WriteStartElement("var");
                xmlWriter.WriteAttributeString("r", "true");
                xmlWriter.WriteAttributeString("n", var.name);
                xmlWriter.WriteAttributeString("c", computeType(new VariableCall(
                    var.name, false, var.dataType.ToString())));
                xmlWriter.WriteEndElement(); // var
            }
        }

        private static ArrayList listOfVarIdInExpression(AbstractExpression expression)
        {
           
            switch (expression)
            {  
                case Expression exp:
                    ArrayList resExp = new ArrayList();
                    resExp.AddRange(listOfVarIdInExpression(exp.expression1));

                    if (exp.expression2 != null) // Binarie expression
                    {
                        foreach (VariableId vc in listOfVarIdInExpression(exp.expression2))
                        {
                            if (!ContainedInRes(vc, resExp))
                            {
                                resExp.Add(vc);
                            }
                        }
                    }
                    return resExp;
                case VariableId varId:
                    ArrayList resVarId = new ArrayList();
                    resVarId.Add(varId);
                    return resVarId;
                default:
                    return new ArrayList();
            }
        }

        private static bool ContainedInRes(VariableId vc, ArrayList resExp)
        {
            foreach (VariableId item in resExp)
            {
                if (item.name == vc.name)
                    return true;
            }
            return false;
        }

        private static string writeXmlExpression(AbstractExpression expression)
        {
            switch (expression)
            {
                case Expression exp:
                    if (exp.expression2 != null) // Binarie expression
                    {
                        return writeXmlExpression(exp.expression1) + exp.SymboleToString() + writeXmlExpression(exp.expression2);
                    }
                    else  //Unaire expression
                    {
                        if (exp.symbole == ExpressionSymbole.NOT)
                        {
                            return exp.SymboleToString() + writeXmlExpression(exp.expression1);
                        }
                        else // parenteses
                        {
                            return "(" + writeXmlExpression(exp.expression1) + ")";
                        }
                    }
                case VariableInteger varInt:
                    return varInt.Write();
                case VariableFloat varFloat:
                    return varFloat.Write();
                case VariableString varStr:
                    return varStr.value;
                case VariableId varId:
                    return varId.name;
                default:
                    throw new NotImplementedException("the type of expression " + expression.Write() + "is not " +
                        "is not already manage by the emitter");
            }
        }

        private static string computeType(VariableCall vc)
        {
            string typeString = vc.typeString;
            if (vc.typeString[0] == 'L')
            {
                typeString = vc.typeString.Substring(1);
            }

            if (typeString == ExpressionType.NOMBRE.ToString())
            {
                return "N";
            }else if (typeString == ExpressionType.TEXTE.ToString())
            {
                return "S";
            } else if (typeString == ExpressionType.BOOL.ToString())
            {
                return "B";
            }
            else
            { 
                Console.WriteLine(vc.typeString + "This type of var is not already supported by the emiteur");
                throw new Exception(vc.typeString + "This type of var is not already supported by the emiteur");
            }
        }


        private static void WriteXmlForTitle(Title tt)
        {
            xmlWriter.WriteStartElement("Parcours");
            xmlWriter.WriteAttributeString("type", "Parcours");
            xmlWriter.WriteAttributeString("t", tt.text);
            xmlWriter.WriteEndElement();
        }

        

        private static void WriteXmlForDeadText(DeadText dt)
        {
            xmlWriter.WriteStartElement("Clause");
            xmlWriter.WriteAttributeString("id", "123");
            xmlWriter.WriteAttributeString("type", "Clause");
            xmlWriter.WriteStartElement("html");
            xmlWriter.WriteString(dt.Write()+" ");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }

       
    }
}
