using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using MouseEvents;

namespace SpeechReco
{
    public static class Lms
    {
        public static System.Globalization.CultureInfo ci_lms = new System.Globalization.CultureInfo("ru-ru"); //язык
        public static SpeechRecognitionEngine sre_lms { get; set; } //распознавалка
        public static Choices commands_lms { get; set; }
        public static GrammarBuilder gb_lms { get; set; }
        public static Grammar g_lms { get; set; }


        static public IWebDriver Browser { get; set; }
        public static void lmsOn()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArgument("--start-maximized");
            Browser = new FirefoxDriver(options);
            Browser.Manage().Window.Maximize();
          
            Browser.Navigate().GoToUrl("https://lms.hse.ru/student.php?ctg=personal");
            IWebElement element = Browser.FindElement(By.Id("user_login-0"));
            element.SendKeys("login"); //your login here
            element = Browser.FindElement(By.Id("user_password-0"));
            element.SendKeys("passowrd" + Keys.Enter); //your pass here
            RecognizelmsCommands();
        }
        public static void RecognizelmsCommands()
        {
            sre_lms = new SpeechRecognitionEngine(ci_lms);
            sre_lms.SetInputToDefaultAudioDevice();
            commands_lms = new Choices();
            gb_lms = new GrammarBuilder();

            commands_lms.Add(new string[]
            {
                "Проекты",
                "Расписание",
                "Закрой элэмэс"

            });
            gb_lms.Append(commands_lms);
            g_lms = new Grammar(gb_lms);
            sre_lms.LoadGrammar(g_lms);

            sre_lms.RecognizeAsync(RecognizeMode.Multiple);
            sre_lms.SpeechRecognized += CommandRecognized;
        }

        private static void CommandRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if(e.Result.Text.ToString()=="Проекты" && e.Result.Confidence>0.7)
            {
                Console.WriteLine("Открываю проекты");              
                Browser.Url = "https://lms.hse.ru/student.php?ctg=personal";
            }
            if (e.Result.Text.ToString() == "Расписание" && e.Result.Confidence > 0.7)
            {
                Console.WriteLine("Открываю расписание");
                   Browser.FindElement(By.CssSelector("span[class='fa fa-2x fa-calendar']")).Click();
            }
            if (e.Result.Text.ToString() == "Закрой элэмэс" && e.Result.Confidence > 0.7)
            {
                Console.WriteLine("Закрываю браузер");
                sre_lms.RecognizeAsyncStop();
                sre_lms.RecognizeAsyncCancel();
                sre_lms = null;
                Browser.Close();
                Browser.Quit();
                commands_lms = null;
                gb_lms = null;
                g_lms = null;
            }
        }
    }
}
