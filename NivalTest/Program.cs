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
        //thread-protected by longestFilename
        static int longest = 0;
        //thread-protected by longestFilename
        static string longestFilename = "";

        static void MyTask(string filename)
        {
            try
            {
                var p = new CalculationDeserializer();
                var operations = p.Deserialize(filename);
                var calc = new Calculator();
                var result = calc.Calculate(operations);
                var length = operations.Count;

                lock(longestFilename)
                {
                    if (length > longest)
                    {
                        longest = length;
                        longestFilename = filename;
                    }
                }

                Console.WriteLine($"{filename} {result}");
            }
            catch (XmlException xe)
            {
                Console.WriteLine($"{filename}: XmlError: {xe.Message}");                
            }
            catch (DivideByZeroException ze)
            {
                Console.WriteLine($"{filename}: Division By Zero.");
            }
        }        

        public static void Main(String[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            if (args.Length != 1)
            {
                Console.WriteLine("Error! Expected one argument: path to dir.");
                Console.WriteLine($"Program execution time: {watch.ElapsedMilliseconds} ms.");
                Environment.Exit(1);
            }

            string pathToDir = args[0];

            if (!Directory.Exists(pathToDir))
            {
                Console.WriteLine("Invalid path to directory.");
                Console.WriteLine($"Program execution time: {watch.ElapsedMilliseconds} ms.");
                Environment.Exit(1);
            }

            var files = Directory.EnumerateFiles(pathToDir, "*.xml");
            List<Task> tasks = new List<Task>();

            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => MyTask(file)));
            }

            Task.WaitAll(tasks.ToArray());

            lock (longestFilename)
            {
                Console.WriteLine($"Most successfully deserialized elements in {longestFilename}.");
            }

            watch.Stop();            

            Console.WriteLine($"Program execution time: {watch.ElapsedMilliseconds} ms.");            
        }        
    }
}
