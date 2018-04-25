using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NivalTest
{
    class CalculationDeserializer
    {
        public List<Operation> Deserialize(string path)
        {            
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            var calculations = xmlDoc.DocumentElement;

            List<Operation> results = new List<Operation>();            

            for (int i = 0; i < calculations.ChildNodes.Count; i++)
            {
                try
                {
                    XmlNode calculation = xmlDoc.DocumentElement.ChildNodes[i];
                    if (calculation.ChildNodes.Count != 3)
                    {
                        throw new FormatException("Calculation must have 3 child nodes (uid, operand and mod)");
                    }
                    Guid uid = Guid.Empty;
                    Operand operand = Operand.Add;
                    int mod = 0;
                    foreach (XmlNode item in calculation.ChildNodes)
                    {
                        var value = item.Attributes["value"].Value;
                        switch (item.Attributes["name"].Value)
                        {
                            case "uid":
                                uid = Guid.Parse(value);
                                break;
                            case "operand":
                                operand = (Operand)Enum.Parse(typeof(Operand), value, true);
                                break;
                            case "mod":
                                mod = int.Parse(value);
                                break;
                            default:
                                throw new FormatException($"Unrecognized property: {item.Attributes["name"].Value}");
                        }
                    }

                    results.Add(new Operation(uid, operand, mod));
                }
                catch (FormatException fe)
                {
                    Console.WriteLine($"{path}: Error: {fe.Message}, ignoring calculation element");
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"{path}: Error: unrecognized operation");
                }
            }

            return results;
        }
    }
}
