using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;

namespace SeleniumDocs.Hello
{
    public class HelloSelenium
    {
        public static void Main()
        {
            Console.WriteLine("Please enter search below");
            string prompt = Console.ReadLine();

            var driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            string searchurl = "https://www.youtube.com/results?search_query=" + prompt + "&sp=CAI%253D";
            driver.Navigate().GoToUrl(searchurl);
            AllowCookies(driver);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            Boolean ad = CheckAds(driver);
            List<string> videoLinks = Links(driver);
            List<string> videoTitles = Titles(driver);
            List<string> videoViews = Views(driver);
            List<string> videoUploaders = Uploaders(driver);
            videoUploaders.RemoveAt(0);

            if (ad)
            {
                videoLinks.RemoveAt(0);
                videoTitles.RemoveAt(0);
                videoUploaders.RemoveRange(0, 1);
            }

            List<ToJson> toJsons = new List<ToJson>();
            var csv = new StringBuilder();

            Console.WriteLine("------- Top 5 most recent videos from search: " + prompt + " -------");
            for (int i = 0; i < 5; i++)
            {
                var videoNr = i + 1;
                Console.WriteLine("video " + videoNr + "\n");
                Console.WriteLine("title: " + videoTitles[i]);
                Console.WriteLine("url: " + videoLinks[i]);
                int even = i * (i + 1);
                Console.WriteLine("views: " + videoViews[even]);
                Console.WriteLine("channel: " + videoUploaders[i * 2] + "\n");
                Console.WriteLine("-------");

                ToJson a = new ToJson()
                {
                    title = videoTitles[i],
                    url = videoLinks[i],
                    views = videoViews[even],
                    channel = videoUploaders[i*2],
                };
                toJsons.Add(a);

                var data = string.Format("{0},{1},{2},{3}", videoTitles[i], videoLinks[i], videoViews[i], videoUploaders[i]);
                csv.AppendLine(data);
            }

            // save json objects in .txt file
            using (StreamWriter file = File.CreateText(@"C:\Users\Hube\Desktop\Hube\Thomas More\BAC2\DevOps\case_study_devops\youtube_top5.txt"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, toJsons[0]);
                serializer.Serialize(file, toJsons[1]);
                serializer.Serialize(file, toJsons[2]);
                serializer.Serialize(file, toJsons[3]);
            }

            //create & save csv file
            var filePath = "C:/Users/Hube/Desktop/Hube/Thomas More/BAC2/DevOps/case_study_devops/youtube_top5.csv";
            File.WriteAllText(filePath, csv.ToString());

            driver.Close();
        }

        class ToJson
        {
            public string title;
            public string url;
            public string views;
            public string channel;
        }

        private static void AllowCookies(ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var buttonAccept = driver.FindElement(By.XPath("//*[@id=\'content\']/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button"));
            buttonAccept.Click();
        }

        private static Boolean CheckAds(ChromeDriver driver)
        {
            List<IWebElement> e = new List<IWebElement>();
            var adds = driver.FindElements(By.TagName("ytd-search-pyv-renderer"));
            foreach (IWebElement ad in adds)
            {
                e.Add(ad);
            }
            if (e.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static List<string> Links(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var videos = driver.FindElements(By.Id("video-title"));
            foreach (IWebElement video in videos)
            {
                string link = video.GetAttribute("href");
                list.Add(link);
            }
            return list;
        }

        private static List<string> Titles(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var videos = driver.FindElements(By.Id("video-title"));
            foreach (IWebElement video in videos)
            {
                string title = video.GetAttribute("title");
                list.Add(title);
            }
            return list;
        }

        private static List<string> Views(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var views = driver.FindElements(By.ClassName("inline-metadata-item"));
            foreach (IWebElement view in views)
            {
                string nrOfViews = view.Text;
                list.Add(nrOfViews);
            }
            return list;
        }

        private static List<string> Uploaders(ChromeDriver driver)
        {
            List<string> list = new List<string>();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var uploaders = driver.FindElements(By.TagName("ytd-channel-name"));
            foreach (IWebElement uploader in uploaders)
            {
                string uploaderName = uploader.Text;
                list.Add(uploaderName);
            }
            return list;
        }
    }
}
