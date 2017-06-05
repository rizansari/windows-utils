using System;
using System.IO;
using System.Text;

namespace tail
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length > 1)
            {
                Console.WriteLine("Incorrect arguments, usage tail <filename>");
                return;
            }

            string fileName = args[0];

            if (!File.Exists(fileName))
            {
                Console.WriteLine("File '" + fileName + "' does not exists");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    // get length of the file
                    long maxOffset = reader.BaseStream.Length;

                    long lastLinesOffset = maxOffset;

                    // display last few lines
                    if(maxOffset > 512)
                    {
                        lastLinesOffset = 512;
                    }
                    else
                    {
                        lastLinesOffset = maxOffset;
                    }

                    reader.BaseStream.Seek(-lastLinesOffset, SeekOrigin.End);
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        WriteLine(line);
                    }

                    while (true)
                    {
                        System.Threading.Thread.Sleep(100);

                        // if the file size is same then continue
                        if (reader.BaseStream.Length == maxOffset)
                            continue;

                        // seek to the last max offset
                        reader.BaseStream.Seek(maxOffset, SeekOrigin.Begin);

                        // read lines and print
                        while ((line = reader.ReadLine()) != null)
                            WriteLine(line);

                        // update the last max offset
                        maxOffset = reader.BaseStream.Position;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void WriteLine(string line)
        {
            if (line.Contains("ERROR") || line.Contains("FATAL"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (line.Contains("WARN"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (line.Contains("DEBUG"))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
            {
                //Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(line);
            //Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
        }
    }
}
