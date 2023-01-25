// See https://aka.ms/new-console-template for more information
namespace SeleniumTest
{
    class Program
    {
        public static void Main(string[]argv)
        {
            SeleniumTest Prosit = new SeleniumTest();

            

            string opc = "";
            while(opc != "3")
            {
                Prosit.OpenDriver();
                Console.WriteLine("1. SetSpectralLibrary 2.GetSpectralLibrary 3.Exit");
                opc = Console.ReadLine();
                switch(opc)
                {
                    case "1":
                        Prosit.SetSpectralLibrary();
                        break;
                    case "2":
                        Prosit.GetSpectralLibrary("");
                        break;
                    default:
                        Console.Clear();
                        break;
                }
                if (opc == "3")
                    break;
                Prosit.CloseDriver();
            }

        }
    }
}