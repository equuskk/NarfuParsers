using System;
using NarfuParsers.Schedule;

namespace NarfuParsers
{
    public class ScheduleService
    {
        public StudentsSchedule Students { get; }
        public TeachersSchedule Teachers { get; }

        public ScheduleService(TimeSpan timeout)
        {
            Students = new StudentsSchedule(timeout);
            Teachers = new TeachersSchedule(timeout);
        }
    }
}