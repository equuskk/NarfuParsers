using System;
using Narfu.Schedule;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var schedule = new TeachersSchedule();
            var lessons = schedule.GetLessons(22914).GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
    }
}
