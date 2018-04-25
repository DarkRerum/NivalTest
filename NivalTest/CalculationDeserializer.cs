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
                        throw new FormatException("Calculation must have 3 child nodes (uid, operand and mod).");
                    }
                    Guid uid = Guid.Empty;
                    Operand operand = Operand.Add;
                    int mod = 0;
                    bool uidInit = false;
                    bool opInit = false;
                    bool modInit = false;
                    foreach (XmlNode item in calculation.ChildNodes)
                    {
                        var value = item.Attributes["value"].Value;
                        switch (item.Attributes["name"].Value)
                        {
                            case "uid":
                                if (!uidInit)
                                {
                                    uid = Guid.Parse(value);
                                    uidInit = true;
                                }
                                else
                                {
                                    throw new FormatException($"Uid provided more than once.");
                                }
                                break;
                            case "operand":
                                if (!opInit)
                                {
                                    operand = (Operand)Enum.Parse(typeof(Operand), value, true);
                                    opInit = true;
                                }
                                else
                                {
                                    throw new FormatException($"Operand provided more than once.");
                                }
                                
                                break;
                            case "mod":
                                if (!modInit)
                                {
                                    mod = int.Parse(value);
                                    modInit = true;
                                }
                                else
                                {
                                    throw new FormatException($"Mod provided more than once.");
                                }                                
                                break;
                            default:
                                throw new FormatException($"Unrecognized property: {item.Attributes["name"].Value}.");
                        }
                    }

                    if (!uidInit)
                    {
                        throw new FormatException($"Uid not provided.");
                    }
                    if (!opInit)
                    {
                        throw new FormatException($"Operand not provided.");
                    }
                    if (!modInit)
                    {
                        throw new FormatException($"Mod not provided.");
                    }

                    results.Add(new Operation(uid, operand, mod));
                }
                catch (FormatException fe)
                {
                    Console.WriteLine($"{path}: Error: {fe.Message} Ignoring calculation element.");
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"{path}: Error: unrecognized operation. {ae.Message} Ignoring calculation element.");
                }
            }

            return results;
        }
    }
}
