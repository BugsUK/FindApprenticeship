using System;

namespace SFA.DAS.RAA.Api.Models
{
    public class VacancyIdentifier
    {
        public VacancyIdentifier(int id)
        {
            Id = id;
        }

        public VacancyIdentifier(string reference)
        {
            Reference = reference;
        }

        public VacancyIdentifier(Guid guid)
        {
            Guid = guid;
        }

        public int? Id { get; set; }
        public string Reference { get; set; }
        public Guid? Guid { get; set; }

        protected bool Equals(VacancyIdentifier other)
        {
            return Id == other.Id && string.Equals(Reference, other.Reference) && Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VacancyIdentifier) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Reference != null ? Reference.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Guid.GetHashCode();
                return hashCode;
            }
        }
    }
}