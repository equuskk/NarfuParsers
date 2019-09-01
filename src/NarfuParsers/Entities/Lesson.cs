using System;

namespace NarfuParsers.Entities
{
    public class Lesson : IEquatable<Lesson>
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Number { get; set; }
        public string Address { get; set; }
        public string Auditory { get; set; }
        public string Teacher { get; set; }
        public string Groups { get; set; }

        public bool Equals(Lesson other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name) && StartTime.Equals(other.StartTime);
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj))
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            if(obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Lesson)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ StartTime.GetHashCode();
        }
    }
}