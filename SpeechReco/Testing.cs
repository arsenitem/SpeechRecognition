using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using Microsoft.CognitiveServices.Speech;

namespace SpeechReco
{
    class Testing
    {
        public static string message;
        public static void WriteMessage()
        {
            Thread t = new Thread(Recofnition);
            t.Start();
            IWebDriver Browser = new FirefoxDriver();
            Browser.Navigate().GoToUrl("https://vk.com/im");
            IWebElement SearchInput = Browser.FindElement(By.Id("email"));
            SearchInput.SendKeys("temyen@yandex.ru");
            SearchInput = Browser.FindElement(By.Id("pass"));
            SearchInput.SendKeys("Temnik99" + OpenQA.Selenium.Keys.Enter);
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //Ожидать загрузки страницы
            Browser.FindElement(By.CssSelector("li[class='nim-dialog _im_dialog _im_dialog_34389212 nim-dialog_classic']")).Click();
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //Ожидать загрузки страницы
            SearchInput = Browser.FindElement(By.CssSelector("div[id='im_editable0']"));
            SearchInput.SendKeys(message + Keys.Enter);
            t.Join();
        }
        public static void Recofnition()
        {
            var config = SpeechConfig.FromSubscription("24670b42504d4966ad11255e07fa73cd", "westus");
            config.SpeechRecognitionLanguage = "ru-RU";
            SpeechRecognizer sre = new SpeechRecognizer(config);
            sre.StartContinuousRecognitionAsync();
            Console.WriteLine("Что написать");
            sre.Recognized += (s, e) =>
            {
                Console.WriteLine("распознала " + e.Result.Text);
                message = e.Result.Text;
                sre.StopContinuousRecognitionAsync();
            };

        }
    }
}
