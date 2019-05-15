using System;
using System.Threading.Tasks;
using Narfu.Parsers;
using Narfu.Schedule;

namespace ExampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var teacherSchedule = new TeachersSchedule();
            var teacherLessons = await teacherSchedule.GetLessons(22914);

            var studentsSchedule = new StudentsSchedule();
            var studentsLessons = await studentsSchedule.GetLessons(9092);

            var schoolsParser = new SchoolsParser();
            var schools = await schoolsParser.GetSchools();

            var groupsParser = new GroupsParser();
            var groups = await groupsParser.GetGroupsFromSchool(15);
            Console.WriteLine("Hello World!");
        }
    }
}
