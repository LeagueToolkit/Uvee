using System;
using Fantome.Libraries.League.Helpers;
using System.IO;
using Fantome.Libraries.League.IO.SimpleSkin;

namespace Uvee
{
#warning Build: dotnet publish -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true /p:TrimUnusedDependencies=true
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (File.Exists(args[0]))
                {
                    LeagueFileType meshType = Utilities.GetExtensionType(Path.GetExtension(args[0]));

                    if(meshType == LeagueFileType.SKN)
                    {
                        Processor.Process(args[0], new SKNFile(args[0]));
                    }
                    else
                    {
                        WriteError("File type must be SKN");
                    }

                }
                else
                {
                    WriteError("File does not exist");
                }
            }
            else
            {
                WriteError("Please input SKN file");
            }
        }

        private static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
