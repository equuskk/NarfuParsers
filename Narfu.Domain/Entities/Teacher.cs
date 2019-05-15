using System;

namespace Narfu.Domain.Entities
{
    public class Teacher : IEquatable<Teacher>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Depart { get; set; }

        public bool Equals(Teacher other)
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

            if(obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Teacher)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}