using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDocs.Hello
{
    public class HelloSelenium
    {
        public static void Main()
        {
            Console.WriteLine("Enter the job function you want to search for in available jobs");
            string prompt = Console.ReadLine();

            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            string searchurl = "https://www.ictjob.be/nl/it-vacatures-zoeken?keywords=" + prompt;
            driver.Navigate().GoToUrl(searchurl);
            SortByDate(driver);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            List<string> jsTitles = Titles(driver);
            List<string> jsCompanies = Companies(driver);
            List<string> jsLocations = Locations(driver);
            List<string> jsKeywords = Keywords(driver);
            List<string> jsDetailPages = DetailPages(driver);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(jsTitles[i]);
                Console.WriteLine(jsCompanies[i]);
                Console.WriteLine(jsLocations[i]);
                Console.WriteLine(jsKeywords[i]);
                Console.WriteLine(jsDetailPages[i]);
                Console.WriteLine("-----");
            }

            driver.Close();
        }

        //private static void AllowCookies(ChromeDriver driver)
        //{
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        //    var buttonAccept = driver.FindElement(By.XPath("//*[@id=\'content\']/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button"));
        //    buttonAccept.Click();
        //}

        private static void SortByDate(ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var buttonDate = driver.FindElement(By.Id("sort-by-date"));
            buttonDate.Click();
        }

        private static List<string> Titles(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var titles = driver.FindElements(By.ClassName("job-title"));
            foreach (IWebElement title in titles)
            {
                list.Add(title.Text);
            }
            return list;
        }

        private static List<string> Companies(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var companies = driver.FindElements(By.ClassName("job-company"));
            foreach (IWebElement company in companies)
            {
                list.Add(company.Text);
            }
            return list;
        }

        private static List<string> Locations(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var locations = driver.FindElements(By.ClassName("job-location"));
            foreach (IWebElement location in locations)
            {
                list.Add(location.Text);
            }
            return list;
        }

        private static List<string> Keywords(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var keywords = driver.FindElements(By.ClassName("job-keywords"));
            foreach (IWebElement keyword in keywords)
            {
                list.Add(keyword.Text);
            }
            return list;
        }

        private static List<string> DetailPages(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var detailPages = driver.FindElements(By.ClassName("search-item-link"));
            foreach (IWebElement detailPage in detailPages)
            {
                string link = detailPage.GetAttribute("href");
                list.Add(link);
            }
            return list;
        }
    }
}

