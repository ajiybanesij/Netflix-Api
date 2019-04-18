using Netflix_Api.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Netflix_Api.Controllers
{
    [RoutePrefix("api")]
    public class MainController : ApiController
    {
        String URL = "https://www.netflix.com/tr/login";


        [Route("GetHistory/{email}/{password}")]
        [HttpGet,HttpPost]
        public async Task<IHttpActionResult> GetHistory(string email,string password)
        {
         
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>() { "headless" });

            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);
            driver.Navigate().GoToUrl(URL);
            driver.FindElement(By.XPath("//*[@id='id_userLoginId']")).SendKeys(email);
            driver.FindElement(By.XPath("//*[@id='id_password']")).SendKeys(password);
            var loginButton = driver.FindElement(By.XPath("//*[@id='appMountPoint']/div/div[3]/div/div/div[1]/form/button"));
            loginButton.Click();


            IList<IWebElement> all = driver.FindElementsByClassName("profile-name");
            int sayac = 0;
            int index = 0;
            foreach (var item in all)
            {
                if (item.Text.ToString().Equals("Nesij"))
                {
                    index = sayac;
                    Console.WriteLine(item.Text.ToString());
                }
                sayac++;
            }
            var proflieLink = driver.FindElement(By.CssSelector("#appMountPoint > div > div > div > div > div.profiles-gate-container > div > div > ul > li:nth-child(" + index + 1 + ") > div > a")).GetAttribute("href");
            driver.Navigate().GoToUrl(proflieLink);

            List<RootObject> allList = new List<RootObject>();
            int indexPage = 0;
            while (true)
            {

                driver.Navigate().GoToUrl("https://www.netflix.com/api/shakti/v6c995d00/viewingactivity?pg=" + indexPage);
                var source = driver.FindElementByTagName("body").Text;
                RootObject viewedItemModel = JsonConvert.DeserializeObject<RootObject>(source);
                if (viewedItemModel.ViewedItems.Count == 0)
                {
                    break;
                }
                allList.Add(viewedItemModel);
                indexPage++;
            }

            List<ViewedItem> list = new List<ViewedItem>();
            foreach (var item in allList)
            {
                foreach (var item2 in item.ViewedItems)
                {
                    ViewedItem model = new ViewedItem();
                    model.Title = item2.Title;
                    model.VideoTitle = item2.VideoTitle;
                    model.MovieID = item2.MovieID;
                    model.Country = item2.Country;
                    model.Bookmark = item2.Bookmark;
                    model.Duration = item2.Duration;
                    model.Date = item2.Date;
                    model.DeviceType = item2.DeviceType;
                    model.DateStr = item2.DateStr;
                    model.Index = item2.Index;
                    model.TopNodeId = item2.TopNodeId;
                    model.Series = item2.Series;
                    model.SeriesTitle = item2.SeriesTitle;
                    model.SeasonDescriptor = item2.SeasonDescriptor;
                    model.EpisodeTitle = item2.EpisodeTitle;
                    model.EstRating = item2.EstRating;
                    list.Add(model);
                }
            }

            return Ok(allList);

        }
    }
}
