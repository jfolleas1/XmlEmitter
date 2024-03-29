﻿using System;
using CompCorpus.RunTime.Bricks;
using CompCorpus.RunTime;
using System.Collections;

namespace XmlForPluginEmitter
{
    class ConditionXmlWriter
    {
        static public void WriteXmlForCondition(Condition cond)
        {
            XmlEmitter.xmlWriter.WriteStartElement("Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("id", (XmlEmitter.nextId++).ToString());
            XmlEmitter.xmlWriter.WriteAttributeString("type", "Parcours");
            XmlEmitter.xmlWriter.WriteAttributeString("n", "Condition");
            XmlEmitter.xmlWriter.WriteAttributeString("d", writeXmlExpression(cond.conditionExpression));
            insertVarIdOfExpression(cond.conditionExpression);
            foreach (Brick bk in cond.brickList)
            {
                XmlEmitter.WriteXmlForBrick(bk);
            }
            XmlEmitter.xmlWriter.WriteEndElement();
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
                    return slugify(varId.name);
                default:
                    throw new NotImplementedException("the type of expression " + expression.Write() + "is not " +
                        "is not already manage by the emitter");
            }
        }

        private static void DeclarVariable(VariableCall vc)
        {
            if (vc.expression != null && !XmlEmitter.DBVariableName.Contains(vc.name)) // we declar all the variable needed
            {
                insertVarIdOfExpression(vc.expression);
            }

            XmlEmitter.xmlWriter.WriteStartElement("var");
            if (vc.expression == null && (!FromIteration(vc.name) || FromIterationReprise(vc.name)))
            {
                XmlEmitter.xmlWriter.WriteAttributeString("r", "true");
            }
            XmlEmitter.xmlWriter.WriteAttributeString("n", slugify(vc.name));
            XmlEmitter.xmlWriter.WriteAttributeString("c", computeType(vc));
            if (FromIteration(vc.name) && !FromIterationNotFromDB(vc.name))
            {
                XmlEmitter.xmlWriter.WriteAttributeString("r", "true");
            }
            if (vc.expression != null)
            {
                XmlEmitter.xmlWriter.WriteAttributeString("e", writeXmlExpression(vc.expression));
            }
            XmlEmitter.xmlWriter.WriteAttributeString("dep", "true");
            XmlEmitter.xmlWriter.WriteEndElement(); // var
        }

        private static void insertVarIdOfExpression(AbstractExpression expression)
        {
            ArrayList listOfVarId = listOfVarIdInExpression(expression);
            foreach (VariableId var in listOfVarId)
            {
                if (!XmlEmitter.DBDeclarationName.Contains(var.name))
                {
                    DeclarVariable(new VariableCall(var.name, var.local, var.dataType.ToString()));
                }
            }
        }

        private static bool FromIteration(string name)
        {
            foreach (string iterator in IterationXmlWriter.pileOfIterator)
            {
                if (name.StartsWith(iterator))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool FromIterationNotFromDB(string name)
        {
            foreach (string iterator in IterationXmlWriter.pileOfIteratorNotFromDB)
            {
                if (name.StartsWith(iterator))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool FromIterationReprise(string name)
        {
            foreach (string iterator in IterationXmlWriter.pileOfIteratorInReprise)
            {
                if (name.StartsWith(iterator))
                {
                    return true;
                }
            }
            return false;
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

        internal static string computeType(VariableCall vc)
        {
            string typeString = vc.typeString;
            if (vc.typeString[0] == 'L')
            {
                typeString = vc.typeString.Substring(1);
            }

            if (typeString == ExpressionType.NOMBRE.ToString())
            {
                return "N";
            }
            else if (typeString == ExpressionType.TEXTE.ToString())
            {
                return "S";
            }
            else if (typeString == ExpressionType.BOOL.ToString())
            {
                return "B";
            }
            else
            {
                Console.WriteLine(vc.typeString + "This type of var is not already supported by the emiteur");
                throw new Exception(vc.typeString + "This type of var is not already supported by the emiteur");
            }
        }

        private static string slugify(string varname)
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
