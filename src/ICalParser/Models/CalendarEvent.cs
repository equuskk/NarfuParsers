using System;

namespace ICalParser.Models
{
    public class CalendarEvent : IEquatable<CalendarEvent>
    {
        public string Uid { get; }
        public DateTime DtStart { get; }
        public DateTime DtEnd { get; }

        public string Description { get; }
        public string Location { get; }

        public string Summary { get; }

        public CalendarEvent(string uid, DateTime start, DateTime end, string description, string location, string summary)
        {
            Uid = uid;
            DtStart = start;
            DtEnd = end;
            Description = description;
            Location = location;
            Summary = summary;
        }

        public bool Equals(CalendarEvent other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return DtStart.Equals(other.DtStart) && Description == other.Description && Location == other.Location &&
                   Summary == other.Summary;
        }

        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }
    }
}