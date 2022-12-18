using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumDocs.Hello
{
    public class HelloSelenium
    {
        public static void Main()
        {
            Console.WriteLine("Enter search prompt");
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

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(videoLinks[i]);
                Console.WriteLine(videoTitles[i]);
                int even = i * (i + 1);
                Console.WriteLine(videoViews[even]);
                Console.WriteLine(videoUploaders[i * 2]);
                Console.WriteLine("-----");
            }

            //foreach(var item in videoUploaders)
            //{
            //    Console.WriteLine("---");
            //    Console.WriteLine(item);
            //    Console.WriteLine("---");

            //}

            driver.Close();
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
