using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    /// <summary>
    /// Entry point for a performance monitoring program (by: Mike F), coded along a youtube-video by Barnacules(Codegasm).
    /// </summary>
    /// 
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        //
        // WHERE ALL THE MAGIC HAPPENS!!!
        //
        static void Main(string[] args)
        {
            
            // List of messages that will be selected at random when the CPU is hammered!
            List<string> cpuMaxedOutMessages = new List<string>();
            cpuMaxedOutMessages.Add("WARNING: Holy crap your CPU is about to catch on fire!");
            cpuMaxedOutMessages.Add("WARNING: Oh my god you should not run your CPU that hard!");
            cpuMaxedOutMessages.Add("WARNING: Stop downloading the porn it's maxing me out!");
            cpuMaxedOutMessages.Add("WARNING: Your CPU is officially chasing squirrels!");
            cpuMaxedOutMessages.Add("RED ALERT! RED ALERT! RED ALERT! RED ALERT! I FARTED!");


            // The dice! Like DnD
            Random rand = new Random();


            // This will greet the user in the default voice.
            Console.WriteLine("Welcome to Jarvis version 1.0");
            synth.Speak("Welcome to Jarvis version one point O!");


            #region Performance Values
            // This will pull the current CPU load in percentage.
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            // This will pull the current available memory in Megabytes.
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCount.NextValue();

            // This will get us the system uptime (in seconds).
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            perfUptimeCount.NextValue();
            #endregion


            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUptimeCount.NextValue());
            string systemUptimeMessage = string.Format("The current system up time is {0} days {1} hours {2} minutes {3} seconds",
                (int)uptimeSpan.TotalDays,
                (int)uptimeSpan.Hours,
                (int)uptimeSpan.Minutes,
                (int)uptimeSpan.Seconds
                );

            // Tell the user what the current system uptime is.
            Console.WriteLine(systemUptimeMessage);
            MikeSpeak(systemUptimeMessage, VoiceGender.Female, 2);


            int speechSpeed = 1;
            bool isChromeOpenedAlready = false;


            // Infinite While Loop.
            while (true)
            {



                // Get the current performance counter values.
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();

                // Every 1 second print the CPU load in percentage and the available memory in MBs to the screen.
                Console.WriteLine("\nCPU Load: {0}%", currentCpuPercentage);
                Console.WriteLine("Available Memory: {0} MB", currentAvailableMemory);

                
                // Only tell the user the CPU load when it is above 80%.
                #region Logic
                if (currentCpuPercentage > 80)
                {
                    // Warning when the CPU is maxed out!
                    if (currentCpuPercentage == 100)
                    {

                        // This is designed to prevent the speech speed from exceeding 7x normal.
                        if(speechSpeed < 7)
                        {
                            speechSpeed++;
                        }
                        string cpuLoadVocalMessage = cpuMaxedOutMessages[rand.Next(5)];

                        if (isChromeOpenedAlready == false)
                        {
                            OpenWebsite("https://www.youtube.com/watch?v=kxopViU98Xo"); // 10 hours of "Saxophone Guy"... 
                            isChromeOpenedAlready = true;
                        }

                        MikeSpeak(cpuLoadVocalMessage, VoiceGender.Male, speechSpeed);

                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} %", currentCpuPercentage);
                        MikeSpeak(cpuLoadVocalMessage, VoiceGender.Female, 5);
                    }
                }
                #endregion

                // Only tell the user the state of the memory when it is below 1GB.
                if (currentAvailableMemory < 1024)
                {
                    string memAvailableVocalMessage = String.Format("You currently have {0} megabytes of memory available", currentAvailableMemory);
                    MikeSpeak(memAvailableVocalMessage, VoiceGender.Female, 10);

                }


                Thread.Sleep(1000);

            } // end of loop

        }      
        /// <summary>
        /// Speaks with a selected voice. Which on my current system (Swedish Windows 10 Home) is a female voice only.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        public static void MikeSpeak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }


        /// <summary>
        /// Speaks with a selected voice at a selected speed. Which, again, on my current system (Swedish Windows 10 Home) is a female voice only.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGender"></param>
        /// <param name="rate"></param>
        public static void MikeSpeak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            MikeSpeak(message, voiceGender);
        }
        // Open a website.
        public static void OpenWebsite(string URL)
        {

            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
        }
        
    }
}
                   