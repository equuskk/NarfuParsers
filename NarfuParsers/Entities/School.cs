using System;

namespace NarfuParsers.Entities
{
    public class School : IEquatable<School>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public bool Equals(School other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id;
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

            return Equals((School)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}