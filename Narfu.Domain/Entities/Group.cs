using System;

namespace Narfu.Domain.Entities
{
    public class Group : IEquatable<Group>
    {
        public int RealId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }

        public bool Equals(Group other)
        {
            if(ReferenceEquals(null, other))
            {
                return false;
            }

            if(ReferenceEquals(this, other))
            {
                return true;
            }

            return RealId == other.RealId && SiteId == other.SiteId;
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

            return Equals((Group)obj);
        }

        public override int GetHashCode()
        {
            return RealId ^ SiteId;
        }
    }
}