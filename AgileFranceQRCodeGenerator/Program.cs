using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace AgileFranceQRCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Schedule schedule = LoadJson("schedule.json");

            WebClient webClient = new WebClient();
            foreach (var day in schedule.days)
            {
                foreach (var conference in day.events)
                {
                    var title = conference.feedbackUrl.Split('-')[1]; // Retrieve id of talk from feedback url (eg. 'https://roti.express/r/AF2019-41' should take only '41')
                    var fileName = $"{ title }.png";

                    var fileUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=600x600&margin=1&data={ conference.feedbackUrl }";

                    webClient.DownloadFile(fileUrl, fileName);

                    Console.WriteLine($"Downloading { fileName } (from { fileUrl })");
                }
            }

            Console.WriteLine($"Downloading finished ! Press any key to exit ...");
            Console.ReadKey();
        }

        private static Schedule LoadJson(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string jsonString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Schedule>(jsonString);
            }
        }

        public class Schedule
        {
            public ScheduleDay[] days;
        }

        public class ScheduleDay
        {
            public string day;
            public string dayName;
            public Conference[] events;
        }

        public class Conference
        {
            public int id;
            public string day;
            public string dayName;
            public string halfdayName;
            public string startTime;
            public string endTime;
            public string title;
            public string description;
            public string room;
            public string level;
            public string objectif;
            public string[] speakers;
            public string feedbackUrl;
        }
    }
}
