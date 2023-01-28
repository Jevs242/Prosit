using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Security.Policy;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace SeleniumTest
{
    class SeleniumTest
    {
        IWebDriver? driver { get; set; } 
        public string? path { get; set; }
        public string? name { get; set; }
        public string? url { get; set; }
        

        public void OpenDriver()
        {
            //Services
            EdgeDriverService service = EdgeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            //Options
            EdgeOptions option = new EdgeOptions();
            option.AddArgument("--window-position=-32000,-32000");
            option.AddUserProfilePreference("download.default_directory", path);
            option.AddUserProfilePreference("download.prompt_for_download", false);
            option.AddUserProfilePreference("disable-popup-blocking", "true");
            //Open Driver
            driver = new EdgeDriver(service, option);
        }

        public void CloseDriver()
        {
            try
            {
                if(driver != null)
                {
                    driver.Quit();
                }
            }
            catch
            {

            }
        }

    

        public void SetSpectralLibrary(string documentPath )
        {
            //Open Edge
            OpenDriver();
            //Close the Edge just in case it closes incorrectly
            AppDomain.CurrentDomain.ProcessExit += (s, e) => CloseDriver();
            //Get Name by the Path
            string name = documentPath.Split('\\').Last().Split('.')[0];
            Console.WriteLine("Set Spectral Library");

            if (driver == null)
                return;

            //Go to the Website
            driver.Url = "https://www.proteomicsdb.org/prosit/";
            //Section : the Spectral Library
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
            SendKeys.SendWait($@"{documentPath}" + "{ENTER}");

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
            Thread.Sleep(1000);

            //Section : Isobaric Label
            
            //Button : NIST .MSP Text Format of individual spectra (Skyline and MSPepSearch compatible)
            driver.FindElement(By.XPath($"//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[11]/div/div/div[2]/div/div[1]/div/div[{1}]/div")).Click();
            //Button : Next
            driver.FindElement(By.XPath("//*[@id='app']//div[15]/div[1]/div/div[4]/div/div/div/div[2]/div/div[2]/div/div[11]/div/div/div[3]/button[2]")).Click();
            Thread.Sleep(2000);
            Console.WriteLine(driver.Url);
            string url = driver.Url;

            //Close Edge
            CloseDriver();
            //Change The location of data 
            VisualStudioProvider.CreateDirectory(VisualStudioProvider.TryGetSolutionDirectoryInfo() + @"\data");

            //Create a File to save the tokens
            VisualStudioProvider.CreateDirectory(VisualStudioProvider.TryGetSolutionDirectoryInfo() + @"\token");
            DateTime localDate = DateTime.Now;
            string path = $@"{VisualStudioProvider.TryGetSolutionDirectoryInfo()}\token\token.txt";
            if (!File.Exists(path))
            {
                Thread.Sleep(1000);
                File.Create(path);
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine($"{url}" + " | " + localDate + " | " + name);
                    sw.Close();
                }
            }
            else if (File.Exists(path))
            {
                Thread.Sleep(1000);
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine($"{url}" + " | " + localDate + " | " + name);
                    sw.Close();
                }
            }

            this.url = url;
            GetSpectralLibrary();
        }

        public void GetSpectralLibrary()
        {
            Console.WriteLine("Get Spectral Library");

            //Open Edge
            OpenDriver();
            //Close the Edge just in case it closes incorrectly
            AppDomain.CurrentDomain.ProcessExit += (s, e) => CloseDriver();

            if (driver == null)
                return;

            //Go to the website
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
                    string? download = "";
                    Console.WriteLine("It is Ready, Do you want to download? Y/N");
                    download = Console.ReadLine();

                    if (download == null)
                        download = "";

                    if (download.ToLower() == "y")
                    {
                        downloadButton.Click();
                        Console.WriteLine("Downloading in " + path);
                    }


                    while (!File.Exists(path + @"\download.zip"))
                    {
                    }
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

            //Close Edge
            CloseDriver();

            //Refresh
            if(!exist)
            {
                Console.WriteLine("Refresh Y / N Exit");
                string? refresh = "";
                refresh = Console.ReadLine();
                Console.Clear();

                if (refresh == null)
                    refresh = "";

                if(refresh.ToLower() == "y")
                {
                    GetSpectralLibrary();
                }
            }

        }
    }
}