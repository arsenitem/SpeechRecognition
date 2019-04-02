using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA;
using Microsoft.Speech.Recognition;

namespace SpeechReco
{
    class Program
    {
        static System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
        static SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);

        static Choices commands = new Choices();
        static GrammarBuilder gb = new GrammarBuilder();
        static Grammar g;

        static void Main(string[] args)
        {
            sre.SetInputToDefaultAudioDevice();
            commands.Add(new string[]
            {
                "Тигра,включи музыку",
                "Тигра,открой элэмэс",
                "Тигра,найди в гугле",
                "Напиши дэну"

            });
            gb.Append(commands);
            g = new Grammar(gb);
            sre.LoadGrammar(g);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += Sre_SpeechRecognized;
            while (true)
            {
                Console.ReadKey();
            }
        }

        private static void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.ToString() == "Тигра,включи музыку" && e.Result.Confidence > 0.8)
            {               
                Music.MusicOn();
            }
            if (e.Result.Text.ToString() == "Тигра,открой элэмэс" && e.Result.Confidence > 0.8)
            {
                Lms.lmsOn();
            }
            if (e.Result.Text.ToString() == "Тигра,найди в гугле" && e.Result.Confidence > 0.8)
            {
                Google.GoogleON();
            }
            if (e.Result.Text.ToString() == "Напиши дэну" && e.Result.Confidence > 0.8)
            {
                Testing.WriteMessage();
            }
        }

       
    }
   
}
  

