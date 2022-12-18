using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;

namespace SeleniumDocs.Hello
{
    public class HelloSelenium
    {
        public static void Main()
        {
            Console.WriteLine("Enter the job function you want to search for (press enter for random selection)");
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

            List<ToJson> toJsons = new List<ToJson>();
            var csv = new StringBuilder();

            Console.WriteLine("------- Here are the 5 most recent job ads for your search: " + prompt + " ------- \n");
            for (int i = 0; i < 5; i++)
            {
                //write to console
                Console.WriteLine("Job description: " + jsTitles[i]);
                Console.WriteLine("Company: " + jsCompanies[i]);
                Console.WriteLine("Location: " + jsLocations[i]);
                Console.WriteLine("Relevant keywords: " + jsKeywords[i]);
                Console.WriteLine("More information: " + jsDetailPages[i] + "\n");
                Console.WriteLine("-------");

                //create json object
                ToJson a = new ToJson()
                {
                    jobdescription = jsTitles[i],
                    company = jsCompanies[i],
                    location = jsLocations[i],
                    keywords = jsKeywords[i],
                    moreinfo = jsDetailPages[i],
                };
                toJsons.Add(a);

                var data = string.Format("{0},{1},{2},{3},{4}", jsTitles[i], jsCompanies[i], jsLocations[i], jsKeywords[i], jsDetailPages[i]);
                csv.AppendLine(data);
            }

            // save json objects in .txt file
            using (StreamWriter file = File.CreateText(@"C:\Users\Hube\Desktop\Hube\Thomas More\BAC2\DevOps\case_study_devops\jobsite_info.txt"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, toJsons[0]);
                serializer.Serialize(file, toJsons[1]);
                serializer.Serialize(file, toJsons[2]);
                serializer.Serialize(file, toJsons[3]);
                serializer.Serialize(file, toJsons[4]);
            }

            //create & save csv file
            var filePath = "C:/Users/Hube/Desktop/Hube/Thomas More/BAC2/DevOps/case_study_devops/jobsite_info.csv";
            File.WriteAllText(filePath, csv.ToString());

            driver.Close();
        }

        class ToJson
        {
            public string jobdescription;
            public string company;
            public string location;
            public string keywords;
            public string moreinfo;
        }

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

