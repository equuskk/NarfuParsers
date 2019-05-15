using System;
using Narfu.Schedule;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var teacherSchedule = new TeachersSchedule();
            var teacherLessons = teacherSchedule.GetLessons(22914).GetAwaiter().GetResult();

            var studentsSchedule = new StudentsSchedule();
            var studentsLessons = studentsSchedule.GetLessons(9092).GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
    }
}
