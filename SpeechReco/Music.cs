using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Microsoft.Speech.Recognition;
using Microsoft.CognitiveServices.Speech;

namespace SpeechReco
{
    public static class Music
    {
        public static System.Globalization.CultureInfo ci_music = new System.Globalization.CultureInfo("ru-ru"); //язык
        public static SpeechRecognitionEngine sre_music { get; set; } //распознавалка
        public static Choices commands_music { get; set; }
        public static GrammarBuilder gb_music { get; set; }
        public static Grammar g_music { get; set; }


        static public  IWebDriver Browser { get; set; }
        public static void MusicOn()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments("--headless");  //запуск в фоновом режиме
            Browser = new FirefoxDriver(options);
            // Browser.Manage().Window.Minimize(); свернуть в трей
            Console.WriteLine("Включаю");
            Browser.Navigate().GoToUrl("https://vk.com/audios59553956");         
            IWebElement SearchInput = Browser.FindElement(By.Id("email"));
            SearchInput.SendKeys("login"); //login here
            SearchInput = Browser.FindElement(By.Id("pass"));
            SearchInput.SendKeys("password" + OpenQA.Selenium.Keys.Enter); //pass here
            Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); //Ожидать загрузки страницы
            SearchInput = Browser.FindElement(By.CssSelector("button[class='audio_page__shuffle_all_button']"));
            SearchInput.Click();
       
            RecognizeMusicCommands();
        }
        public static void RecognizeMusicCommands()
        {
                   sre_music = new SpeechRecognitionEngine(ci_music);
                   sre_music.SetInputToDefaultAudioDevice();
                  commands_music = new Choices();
                 gb_music = new GrammarBuilder();

                commands_music.Add(new string[]
                {
                "Следущая",
                "Пауза",
                "Играть дальше",
                "Громче",
                "Тише",
                "Закрой музыку"
                

                });
                gb_music.Append(commands_music);
                g_music = new Grammar(gb_music);
                sre_music.LoadGrammar(g_music);
            
                sre_music.RecognizeAsync(RecognizeMode.Multiple);
                sre_music.SpeechRecognized += CommandRecognized;             
        }

        private static void CommandRecognized(object sender, SpeechRecognizedEventArgs e)
        {          
                if (e.Result.Text.ToString() == "Следущая" && e.Result.Confidence > 0.7)
                {
                    Console.WriteLine("Включаю следующую");
                    IWebElement SearchInput = Browser.FindElement(By.CssSelector("button[class='audio_page_player_ctrl audio_page_player_next']"));
                    SearchInput.Click();
                }
                if (e.Result.Text.ToString() == "Пауза" && e.Result.Confidence > 0.7)
                {
                    Console.WriteLine("Ппроигрывание приостановлено");
                    IWebElement SearchInput = Browser.FindElement(By.CssSelector("button[class='audio_page_player_ctrl audio_page_player_play _audio_page_player_play audio_playing']"));
                    SearchInput.Click();
                }
                if (e.Result.Text.ToString() == "Играть дальше" && e.Result.Confidence > 0.7)
                {
                    Console.WriteLine("Проигрывание возобновлено");
                    IWebElement SearchInput = Browser.FindElement(By.CssSelector("button[class='audio_page_player_ctrl audio_page_player_play _audio_page_player_play']"));
                    SearchInput.Click();
                }
               if (e.Result.Text.ToString() == "Громче" && e.Result.Confidence > 0.7)
               {
                   Console.WriteLine("Делаю громче");
                   IWebElement slider = Browser.FindElement(By.CssSelector("div[class='slider_handler']"));
                   Actions move = new Actions(Browser);
                   move.DragAndDropToOffset(slider, 8, 0).Build().Perform();
                   slider.Click();
               }
               if (e.Result.Text.ToString() == "Тише" && e.Result.Confidence > 0.7)
               {
                   Console.WriteLine("Делаю тише");
                   IWebElement slider = Browser.FindElement(By.CssSelector("div[class='slider_handler']"));
                   Actions move = new Actions(Browser);
                   move.DragAndDropToOffset(slider, -8, 0).Build().Perform();
                   slider.Click();
               }
               if (e.Result.Text.ToString() == "Закрой музыку" && e.Result.Confidence > 0.7)
               {
                    Console.WriteLine("Закрываю браузер");
                    sre_music.RecognizeAsyncStop();
                    sre_music.RecognizeAsyncCancel();
                    sre_music = null;
                    Browser.Close();
                    commands_music = null;
                    gb_music = null;
                    g_music = null;
               }
              
        }
    }
}
