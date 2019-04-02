using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Firefox;
using System.Speech.Synthesis;
using OpenQA.Selenium;
using Microsoft.Speech.Recognition;
using Microsoft.CognitiveServices.Speech;

namespace SpeechReco
{
    static public class Google
    {
        public static string res_to_search;
        static public void GoogleON()
        {
            Console.WriteLine("Готова искать");
            System.Threading.Thread recognition = new System.Threading.Thread(WhatToSearch);
            recognition.Start();
            IWebDriver Browser = new FirefoxDriver();
            Browser.Navigate().GoToUrl("https://www.google.com/");
            IWebElement element = Browser.FindElement(By.CssSelector("input[class='gLFyf gsfi']"));
            element.SendKeys(res_to_search + Keys.Enter);
            recognition.Join();
        }

        static public void WhatToSearch()
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.Speak("Что найти?");
            }
            var config = SpeechConfig.FromSubscription("24670b42504d4966ad11255e07fa73cd", "westus");
            config.SpeechRecognitionLanguage = "ru-RU";
            SpeechRecognizer sre = new SpeechRecognizer(config);
            sre.StartContinuousRecognitionAsync();
            
            Console.WriteLine("Что найти?");
            sre.Recognized += (s, e) =>
            {
                Console.WriteLine("ищу " + e.Result.Text);
                res_to_search = e.Result.Text;
                sre.StopContinuousRecognitionAsync();
            };
          
        }
    }
}
