using System;
using System.Threading.Tasks;
using NarfuParsers.Parsers;
using NarfuParsers.Schedule;

namespace ExampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var timeout = TimeSpan.FromSeconds(5);

            var teacherSchedule = new TeachersSchedule(timeout);
            var teacherLessons = await teacherSchedule.GetLessons(22914);

            var studentsSchedule = new StudentsSchedule(timeout);
            var studentsLessons = await studentsSchedule.GetLessons(9092);

            var schoolsParser = new SchoolsParser(timeout);
            var schools = await schoolsParser.GetSchools();

            var groupsParser = new GroupsParser(timeout);
            var groups = await groupsParser.GetGroupsFromSchool(15);

            var teachersParser = new TeachersParser(timeout);
            var teachers = await teachersParser.GetTeachersInRange(22913, 22917, 3, 1000);

            Console.ReadLine();
        }
    }
}
