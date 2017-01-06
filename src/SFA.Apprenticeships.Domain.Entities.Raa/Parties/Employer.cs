namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;
    using Locations;

    public class Employer
    {
        public int EmployerId { get; set; }
        public Guid EmployerGuid { get; set; }
        public string EdsUrn { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
        public int PrimaryContact { get; set; }
        public PostalAddress Address { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public EmployerTrainingProviderStatuses EmployerStatus { get; set; }

        protected bool Equals(Employer other)
        {
            return EmployerId == other.EmployerId && EmployerGuid.Equals(other.EmployerGuid) && string.Equals(EdsUrn, other.EdsUrn) && string.Equals(FullName, other.FullName) && string.Equals(TradingName, other.TradingName) && PrimaryContact == other.PrimaryContact && Equals(Address, other.Address) && IsPositiveAboutDisability == other.IsPositiveAboutDisability && EmployerStatus == other.EmployerStatus;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Employer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EmployerId;
                hashCode = (hashCode*397) ^ EmployerGuid.GetHashCode();
                hashCode = (hashCode*397) ^ (EdsUrn != null ? EdsUrn.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TradingName != null ? TradingName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ PrimaryContact;
                hashCode = (hashCode*397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsPositiveAboutDisability.GetHashCode();
                hashCode = (hashCode*397) ^ (int) EmployerStatus;
                return hashCode;
            }
        }
    }
}
