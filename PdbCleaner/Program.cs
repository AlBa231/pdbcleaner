using System;
using System.Collections.Specialized;
using System.IO;

namespace PdbCleaner
{
    class Program
    {
        private static readonly StringCollection _types = new StringCollection();

        static void Main(string[] args)
        {
            string[] fileTypes = {
                ".pdb",
                ".ncb",
                ".obj",
                ".pch",
                ".idb",
                ".ilk",
                ".aps",
                ".tds",
                ".plg",
                ".opt",
                ".suo",
                ".lib",
                ".exp",
                ".cache",
                ".suo",
                ".vshost",
                "BuildLog.htm"
            };

            _types.AddRange(fileTypes);
            _types.AddRange(args);

            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            RemoveDirectoryFiles(directory);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Removing Compleate succesfully");
            Console.WriteLine("Press key to continue...");
            Console.ReadKey();
        }

        private static void RemoveFileAttributes(DirectoryInfo directory)
        {
            directory.Attributes = FileAttributes.Normal;
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                RemoveFileAttributes(dir);
            }
        }

        private static void RemoveDirectoryFiles(DirectoryInfo directory)
        {
            try
            {
                foreach (DirectoryInfo dir in directory.GetDirectories())
                {
                    RemoveDirectoryFiles(dir);
                }
                if (directory.Name == "obj" ||
                    directory.Name.StartsWith("_ReSharper."))
                {
                    FullRemoveDirectory(directory);
                    return;
                }
                RemoveDebugFilesFromDir(directory);
            }
            catch (DirectoryNotFoundException)
            {
                //skip invalid dir
            }
        }

        private static void RemoveDebugFilesFromDir(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                if (file.FullName.Contains(".vshost.exe") || _types.IndexOf(file.Extension) != -1 || _types.IndexOf(file.Name) != -1)
                {
                    try
                    {
                        Console.WriteLine("Deleting {0}...", file.FullName);
                        file.Delete();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error to delete the file {0}...\r\n{1}", file.FullName,
                            ex.Message);
                    }
                }
            }
        }

        private static void FullRemoveDirectory(DirectoryInfo directory)
        {
            try
            {
                Console.WriteLine("Deleting directory {0}...", directory.FullName);
                RemoveFileAttributes(directory);
                directory.Delete(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error to delete the directory {0}...\r\n{1}", directory.FullName,
                                  ex.Message);
            }
        }
    }
}
