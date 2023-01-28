// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Security.Policy;
using System.Xml.Linq;

namespace SeleniumTest
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            SeleniumTest Prosit = new SeleniumTest();

            string? opc = "";
            while(opc != "3")
            {
                Console.WriteLine("1. SetSpectralLibrary 2.GetSpectralLibrary 3.Exit");
                opc = Console.ReadLine();
            
                switch (opc)
                {
                    case "1":
                        //Choose the csv document verification
                        string documentPath = "";
                        while (documentPath == "")
                        {
                            documentPath = ShowDialog(documentPath);
                        }

                        if (documentPath != "menu")
                        {
                            documentPath = ChangeLocation(Prosit , documentPath);
                            Prosit.SetSpectralLibrary(documentPath);
                        }
                        else
                        {
                            
                        }
                        break;
                    case "2":
                        ChooseUrl(Prosit);
                        Prosit.GetSpectralLibrary();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
                if (opc == "3")
                    break;
            }

            static string ShowDialog(string documentPath)
            {
                //Create Dialog Window
                var dialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Title = "Open Excel Document",
                    Filter = "Csv Document|*.csv",
                };
                using (dialog)
                {
                    var button = dialog.ShowDialog();
                    if (button == DialogResult.OK)
                    {
                        documentPath = dialog.FileName;
                        Console.WriteLine(documentPath);
                    }
                    else if(button == DialogResult.Cancel)
                    {
                        Console.Clear();
                        return "menu";
                    }
                }
                return documentPath;
            }

            static string ChangeLocation(SeleniumTest prosit , string documentPath)
            {
                //Change the location of the selected document to the new folder in the data folder and create a folder with the name
                VisualStudioProvider.CreateDirectory(VisualStudioProvider.TryGetSolutionDirectoryInfo() + @"\data");
                prosit.name = documentPath.Split('\\').Last().Split('.')[0];
                Console.WriteLine($"File Moved To Data Folder divition {prosit.name}");
                while (File.Exists(prosit.path + prosit.name + ".csv"))
                {
                    prosit.name += "1";
                }
                VisualStudioProvider.CreateDirectory(VisualStudioProvider.TryGetSolutionDirectoryInfo() + $@"\data\{prosit.name}");
                prosit.path = VisualStudioProvider.TryGetSolutionDirectoryInfo() + $@"\data\{prosit.name}\";

                File.Move(documentPath, prosit.path + prosit.name + ".csv");

                string newpath = prosit.path + documentPath.Split('\\').Last();

                return newpath;
            }

            static void ChooseUrl(SeleniumTest Prosit)
            {
                //URL
                //"https://www.proteomicsdb.org/prosit/task/B3C2A1315FB848565E99E2DFD7BDCDC6" //Fail
                //"https://www.proteomicsdb.org/prosit/task/DF5057395121C4D7BAD8E53A2040BD0D" //Successed

                int counter = 0;
                string tokenPath = $@"{VisualStudioProvider.TryGetSolutionDirectoryInfo()}{@"\token\token.txt"}";
                foreach (string line in File.ReadLines(tokenPath))
                {
                    counter++;
                    System.Console.WriteLine($"{counter}: {line}");
                }

                Console.WriteLine("What Token do you wanna check out?");
                int opc = Convert.ToInt32(Console.ReadLine());
                counter = 0;
                foreach (string line in File.ReadLines(tokenPath))
                {
                    counter++;
                    if (counter == opc)
                        Prosit.url = line;
                }
                Prosit.name = Prosit.url.Split(" | ").Last();
                Prosit.url = Prosit.url.Split(" | ")[0];
                Console.WriteLine(Prosit.name);
                Prosit.path = VisualStudioProvider.TryGetSolutionDirectoryInfo() + $@"\data\{Prosit.name}\";

            }
        }

    }
    public static class VisualStudioProvider
    {
        public static DirectoryInfo TryGetSolutionDirectoryInfo()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }

            if(directory != null)
            {
               return directory;
            }
            else
            {
                return new DirectoryInfo(Directory.GetCurrentDirectory());
            }
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch
            {

            }
        }
    }
}