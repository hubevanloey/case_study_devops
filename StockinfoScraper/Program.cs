using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;

namespace SeleniumDocs.Hello
{
    public class HelloSelenium
    {
        public static void Main()
        {
            Console.WriteLine("Enter stock name or ticker below!"); //request user input via console
            string prompt = Console.ReadLine(); //read user input from console

            var driver = new ChromeDriver(); //initialize chrome driver
            driver.Manage().Window.Maximize(); //maximize window
            driver.Navigate().GoToUrl("https://www.google.com/finance/"); //navigate to url
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30); //wait until loaded
            AllowCookies(driver); //accept cookies
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30); //wait until loaded
            searchStock(driver, prompt); //search stock that user asked for

            List<string> stockData = StockInfo(driver); //save gathered data to local variable
            Console.WriteLine("------- $$$ ------- \n"); 
            Console.WriteLine("Name: " + stockData[0]);
            Console.WriteLine("Primary market: " + stockData[1]);
            Console.WriteLine("Current price: " + stockData[2]);
            Console.WriteLine("$ change: " + stockData[3] +"\n");
            Console.WriteLine("More info: " + driver.Url);
            Console.WriteLine("------- $$$ ------- \n");

            //create json object
            ToJson a = new ToJson()
            {
                name = stockData[0],
                primaryMarket = stockData[1],
                currentPrice = stockData[2],
                dollarChange = stockData[3],
                moreInfo = driver.Url,
            };

            //save json object in .txt file
            using (StreamWriter file = File.CreateText(@"C:\Users\Hube\Desktop\Hube\Thomas More\BAC2\DevOps\case_study_devops\stockinfo.txt"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, a);
            }

            //create & save csv file
            var csv = new StringBuilder();
            var data = string.Format("{0},{1},{2},{3},{4}", stockData[0], stockData[1], stockData[2], stockData[3], driver.Url);
            csv.AppendLine(data);
            var filePath = "C:/Users/Hube/Desktop/Hube/Thomas More/BAC2/DevOps/case_study_devops/stockinfo.csv";
            File.WriteAllText(filePath, csv.ToString()); 

            driver.Close(); //close driver
        }

        //json object
        class ToJson
        {
            public string name;
            public string primaryMarket;
            public string currentPrice;
            public string dollarChange;
            public string moreInfo;
        }

        //accept cookies when window pops up
        private static void AllowCookies(ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var buttonAccept = driver.FindElement(By.XPath("/html/body/c-wiz/div/div/div/div[2]/div[1]/div[3]/div[1]/div[1]/form[2]/div/div/button"));
            buttonAccept.Click();
        }

        //enter the user input into searchbar on Google Finance site
        private static void searchStock(ChromeDriver driver, string search)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var searchbar = driver.FindElement(By.XPath("/html/body/c-wiz/div/div[3]/div[3]/div/div/div/div[1]/input[2]"));
            searchbar.SendKeys(search);
            searchbar.SendKeys(Keys.Enter);
        }

        //Get all data from specific stock
        public static List<string> StockInfo(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            var stockName = driver.FindElement(By.ClassName("zzDege"));
            var stockPrice = driver.FindElement(By.ClassName("fxKbKc"));
            var procentDiv = driver.FindElement(By.ClassName("ZYVHBb"));
            var stockMarket = driver.FindElement(By.XPath("//*[@id=\"yDmH0d\"]/c-wiz[2]/div/div[4]/div/main/div[2]/div[2]/div/div[1]/div[9]/div"));
            list.Add(stockName.Text);
            list.Add(stockMarket.Text);
            list.Add(stockPrice.Text);
            list.Add(procentDiv.Text);
            return list;
        }
    }
}

