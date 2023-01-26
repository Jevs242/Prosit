using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Security.Policy;
using static System.Windows.Forms.Design.AxImporter;

namespace SeleniumTest
{
    class SeleniumTest
    {
        IWebDriver driver;
        EdgeDriverService service;
        EdgeOptions option = new EdgeOptions();
        string path;

        public void OpenDriver()
        {
            service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            //Hide Edge
            option.AddArgument("--window-position=-32000,-32000");
            path = System.AppDomain.CurrentDomain.BaseDirectory + "data\\";
            Directory.CreateDirectory(path);
            option.AddUserProfilePreference("download.default_directory", path);
            option.AddUserProfilePreference("download.prompt_for_download", false);
            option.AddUserProfilePreference("disable-popup-blocking", "true");
            driver = new EdgeDriver(service, option);
            
            
        }

        public void CloseDriver()
        {
            try
            {
                driver.Close();
            }
            catch
            {

            }
        }

        public void SetSpectralLibrary()
        {
            OpenDriver();
            //Close Edge
            AppDomain.CurrentDomain.ProcessExit += (s, e) => CloseDriver();
            Console.WriteLine("Set Spectral Library");

            //URL
            driver.Url = "https://www.proteomicsdb.org/prosit/";

            //Putting the Spectral Library Section
            driver.FindElement(By.XPath(".//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[1]/div/div/div[3]/a")).Click();

            Thread.Sleep(1000);
            //Section: Setting 

            //Button : Next
            driver.FindElement(By.XPath(".//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[3]/div/div/div[4]/button")).Click();

            Thread.Sleep(1000);
            //Section: Upload Files 
            // Button : Upload
            var uploadButton = driver.FindElement(By.XPath(".//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[5]/div/div[3]/div/div[2]/div[1]/div[1]/input"));
        
            // Button :Next
            IWebElement link = driver.FindElement(By.XPath(".//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[5]/div/div[4]/div/button[2]"));
            uploadButton.Click();
            Thread.Sleep(1000);

            //Automatic Input
            //SendKeys.SendWait(@"C:\Users\Jose\Downloads\mini.csv" + "{ENTER}");

            //Check if already upload the file
            while (link.GetAttribute("disabled") != null)
            {
                Thread.Sleep(3000);
            }

            if (link.GetAttribute("disabled") == null)
            {
                link.Click();
            }
            Thread.Sleep(1000);
            //Section : Model

            //Button : Prosit_2020_intensity_hcd
            driver.FindElement(By.XPath($"//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[7]/div/div/div[1]/div/div/div[1]/div/div[{3}]/div")).Click();
            //Button : Prosit_2019_irt
            driver.FindElement(By.XPath($"//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[7]/div/div/div[2]/div/div/div[1]/div/div[{1}]/div")).Click();
            //Button : Next
            driver.FindElement(By.XPath("//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[7]/div/div/div[3]/button[2]")).Click();

            //Section : Isobaric Label
            Thread.Sleep(1000);
            //Button : NIST .MSP Text Format of individual spectra (Skyline and MSPepSearch compatible)
            driver.FindElement(By.XPath($"//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[11]/div/div/div[2]/div/div[1]/div/div[{1}]/div")).Click();
            //Button : Next
            driver.FindElement(By.XPath("//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[11]/div/div/div[3]/button[2]")).Click();

            Thread.Sleep(2000);
            Console.WriteLine(driver.Url);
            DateTime localDate = DateTime.Now;
            string path = @"token.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine($"{driver.Url}" + " | " + localDate);
                tw.Close();
            }
            else if (File.Exists(path))
            {
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine($"{driver.Url}" + " | " + localDate);
                }
            }
            string url = driver.Url;
            driver.Close();

            GetSpectralLibrary(url);
        }

        public void GetSpectralLibrary(string Lurl)
        {
            OpenDriver();
            //Close Edge
            AppDomain.CurrentDomain.ProcessExit += (s, e) => CloseDriver();
            Console.WriteLine("Get Spectral Library");

            //URL
            //"https://www.proteomicsdb.org/prosit/task/B3C2A1315FB848565E99E2DFD7BDCDC6" //Fail
            //"https://www.proteomicsdb.org/prosit/task/DF5057395121C4D7BAD8E53A2040BD0D" //Successed

            string url = "";

            if(Lurl != "None")
            {
                url = Lurl;
            }
            else if(Lurl == "None")
            {
                int counter = 0;
                foreach (string line in File.ReadLines(@"token.txt"))
                {
                    counter++;
                    System.Console.WriteLine($"{counter}: {line}");
                }
                Console.WriteLine("What Token do you wanna check out?");
                int opc = Convert.ToInt32(Console.ReadLine());
                counter= 0;
                foreach (string line in File.ReadLines(@"token.txt"))
                {
                    counter++;
                    if(counter == opc)
                        url = line;
                }
                url = url.Split(" | ")[0];
            }

            driver.Url = url;

            Thread.Sleep(1000);
            //Title
            Console.WriteLine(driver.FindElement(By.XPath("//html/body/div/div[2]/div[1]/div/div[4]/div/div/div[1]")).Text);

            //Success Case
            var downloadButton = driver.FindElement(By.XPath("//*[@id='app']//div[2]/div[1]/div/div[4]/div/div/div[2]/div/a"));
            bool exist = false;
            try
            {
                if(driver.FindElement(By.XPath("//html/body/div/div[2]/div[1]/div/div[4]/div/div/div[1]")).Text == "Your files are ready.")
                {
                    exist = true;
                    string download = "";
                    Console.WriteLine("It is Ready, Do you want to download? Y/N");
                    download = Console.ReadLine();
                    if(download.ToLower() == "y")
                    {
                        downloadButton.Click();
                        Console.WriteLine("Downloading in " + path);
                    }
                    Console.Clear();
                }
            }
            catch { }
            
            //Error Case
            var debugLogButton = driver.FindElement(By.XPath("//html/body/div/div[2]/div[1]/div/div[4]/div/div/div[1]/div/div/ul/li"));
            try
            {
                if (driver.FindElement(By.XPath("//html/body/div/div[2]/div[1]/div/div[4]/div/div/div[1]")).Text.Contains("error"))
                {
                    exist = true;
                    debugLogButton.Click();
                    Thread.Sleep(1000);
                    Console.WriteLine(driver.FindElement(By.XPath("//html/body/div/div[2]/div[1]/div/div[4]/div/div/div[1]/div/div/ul/li/div[2]/div/div")).Text);
                    Console.WriteLine();
                }
            }
            catch { }

            driver.Close();

            //Refresh
            if(!exist)
            {
                Console.WriteLine("Refresh Y / N Exit");
                string refresh = "";
                refresh = Console.ReadLine();
                Console.Clear();
                if(refresh.ToLower() == "y")
                {
                    GetSpectralLibrary(url);
                }
            }
        }
    }
}