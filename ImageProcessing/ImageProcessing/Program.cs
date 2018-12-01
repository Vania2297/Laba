using System;
using System.IO;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            string param;
            int result;
            do
            {
                Console.WriteLine(@"If you want to rename image - enter 1.
If you want to add time stamp - enter 2");
                param = Console.ReadLine();
            } while (!int.TryParse(param, out result));

            var folderFrom = ReadUserDirectory();

            var dirInfo = new DirectoryInfo(folderFrom);
            string newFolder;
            string folderTo;
            CopyService copyService;
            switch (result)
            {
                case 1:
                    newFolder = $"{dirInfo.Name}_rename";
                    folderTo = Path.Combine(dirInfo.Parent.FullName, newFolder);

                    copyService = new CopyService();
                    copyService.Copy(folderFrom, folderTo);

                    var renamingService = new RenamingService();
                    renamingService.Raname(folderTo);
                    break;

                case 2:
                    newFolder = $"{dirInfo.Name}_add stamp";
                    folderTo = Path.Combine(dirInfo.Parent.FullName, newFolder);

                    copyService = new CopyService();
                    copyService.Copy(folderFrom, folderTo);

                    var stampService = new StampService();
                    stampService.AddStamp(folderTo);
                    break;
            }

            Console.WriteLine("Success!!!");
            Console.ReadKey();
        }

        private static string ReadUserDirectory()
        {
            Console.WriteLine("Please enter path name");

            var directoryPath = Console.ReadLine();
            while (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
            {
                Console.WriteLine("Path is not found. Please enter path name");
                directoryPath = Console.ReadLine();
            }

            return directoryPath;
        }
    }
}
