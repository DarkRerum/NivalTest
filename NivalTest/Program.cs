using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NivalTest
{
    class Program
    {
        static void MyTask(string filename)
        {
            try
            {
                var p = new CalculationDeserializer();
                var operations = p.Deserialize(filename);
                var calc = new Calculator();
                var result = calc.Calculate(operations);
                Console.WriteLine($"{filename} {result}");
            }
            catch (XmlException xe)
            {
                Console.WriteLine($"{filename}: XmlError: {xe.Message}");                
            }
            catch (DivideByZeroException ze)
            {
                Console.WriteLine($"{filename}: Division By Zero");
            }
        }

        public static void Main(String[] args)
        {
            string pathToDir = args[0];

            if (!Directory.Exists(pathToDir))
            {
                Console.WriteLine("Invalid path to directory");
                Environment.Exit(1);
            }

            var files = Directory.EnumerateFiles(pathToDir, "*.xml");

            List<Task> tasks = new List<Task>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => MyTask(file)));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine("All tasks completed");

            Console.ReadKey();
        }        
    }
}
