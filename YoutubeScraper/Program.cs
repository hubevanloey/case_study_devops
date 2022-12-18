// imports
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;

namespace SeleniumDocs.Hello
{
    public class HelloSelenium
    {
        public static void Main()
        {
            Console.WriteLine("Please enter search below"); //request user input via console
            string prompt = Console.ReadLine(); //read user input from console
            string searchurl = "https://www.youtube.com/results?search_query=" + prompt + "&sp=CAI%253D"; //construct url based on user input | &sp=CAI%253D filters on recentness

            var driver = new ChromeDriver(); //initialize chrome driver
            driver.Manage().Window.Maximize(); //maximize window
            driver.Navigate().GoToUrl(searchurl); //navigate to url
            AllowCookies(driver); //execute function to handle cookie window
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30); //wait until loaded
            Boolean ad = CheckAds(driver); //check if ad is on page

            //save gathered data to local variables
            List<string> videoLinks = Links(driver);
            List<string> videoTitles = Titles(driver);
            List<string> videoViews = Views(driver);
            List<string> videoUploaders = Uploaders(driver);
            videoUploaders.RemoveAt(0); //this index is always empty

            if (ad) //if ad is on page -> rearrange lists to delete saved ad values
            {
                videoLinks.RemoveAt(0);
                videoTitles.RemoveAt(0);
                videoUploaders.RemoveRange(0, 1);
            }

            List<ToJson> toJsons = new List<ToJson>(); //list for json object
            var csv = new StringBuilder(); //initialize csv

            Console.WriteLine("------- Top 5 most recent videos from search: " + prompt + " -------");
            for (int i = 0; i < 5; i++) //for top 5 recent videos
            {
                var videoNr = i + 1;
                Console.WriteLine("video " + videoNr + "\n");
                Console.WriteLine("title: " + videoTitles[i]);
                Console.WriteLine("url: " + videoLinks[i]);
                int even = i * (i + 1);
                Console.WriteLine("views: " + videoViews[even]);
                Console.WriteLine("channel: " + videoUploaders[i * 2] + "\n");
                Console.WriteLine("-------");

                ToJson a = new ToJson() //create json object
                {
                    title = videoTitles[i],
                    url = videoLinks[i],
                    views = videoViews[even],
                    channel = videoUploaders[i*2],
                };
                toJsons.Add(a); //add json object to list

                var data = string.Format("{0},{1},{2},{3}", videoTitles[i], videoLinks[i], videoViews[i], videoUploaders[i]);
                csv.AppendLine(data); //add data to csv
            }

            // save json objects in .txt file
            using (StreamWriter file = File.CreateText(@"C:\Users\Hube\Desktop\Hube\Thomas More\BAC2\DevOps\case_study_devops\youtube_top5.txt"))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                //serialize objects directly into file stream
                serializer.Serialize(file, toJsons[0]);
                serializer.Serialize(file, toJsons[1]);
                serializer.Serialize(file, toJsons[2]);
                serializer.Serialize(file, toJsons[3]);
            }

            //create & save csv file
            var filePath = "C:/Users/Hube/Desktop/Hube/Thomas More/BAC2/DevOps/case_study_devops/youtube_top5.csv";
            File.WriteAllText(filePath, csv.ToString());

            driver.Close(); //close driver
        }

        //json object
        class ToJson
        {
            public string title;
            public string url;
            public string views;
            public string channel;
        }

        //allow cookies
        private static void AllowCookies(ChromeDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var buttonAccept = driver.FindElement(By.XPath("//*[@id=\'content\']/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button"));
            buttonAccept.Click();
        }

        //check if ad is loaded in on page
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

        //get url links to video
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

        //get title of video
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

        //get number of views
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

        //get name of the uploader
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
