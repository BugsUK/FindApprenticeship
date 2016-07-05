namespace SFA.Apprenticeships.Web.Common.ViewModels.Locations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Serializable]
    [Validator(typeof (AddressViewModelValidator))]
    public class AddressViewModel
    {
        private string _postcode;
        //private string _addressLine4;

        [Display(Name = AddressViewModelMessages.AddressLine1.LabelText)]
        public string AddressLine1 { get; set; }

        public string AddressLineEllipsis
        {
            get
            {
                var addressToShow = AddressLine2 ?? AddressLine1;
                if (string.IsNullOrWhiteSpace(addressToShow)
                    || addressToShow.Length <= 20)
                    return addressToShow;

                return $"{addressToShow.Substring(0, 20)}...";
            }
        }

        [Display(Name = AddressViewModelMessages.AddressLine2.LabelText)]
        public string AddressLine2 { get; set; }

        [Display(Name = AddressViewModelMessages.AddressLine3.LabelText)]
        public string AddressLine3 { get; set; }

        [Display(Name = AddressViewModelMessages.AddressLine4.LabelText)]
        public string AddressLine4{ get;set; }

        [Display(Name = AddressViewModelMessages.AddressLine5.LabelText)]
        public string AddressLine5 { get; set; }

        [Display(Name = AddressViewModelMessages.Town.LabelText)]
        public string Town { get; set; }

        [Display(Name = AddressViewModelMessages.Postcode.LabelText)]
        public string Postcode
        {
            get { return string.IsNullOrWhiteSpace(_postcode) ? _postcode : _postcode.ToUpperInvariant(); }
            set { _postcode = !string.IsNullOrWhiteSpace(value) ? value.ToUpperInvariant() : value; }
        }

        public string Uprn { get; set; }

        public string County { get; set; }

        public GeoPointViewModel GeoPoint { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AddressViewModel);
        }

        protected bool Equals(AddressViewModel other)
        {
            if (other == null) return false;

            return string.Equals(_postcode, other._postcode) && string.Equals(AddressLine1, other.AddressLine1) &&
                   string.Equals(AddressLine2, other.AddressLine2) && string.Equals(AddressLine3, other.AddressLine3) &&
                   string.Equals(AddressLine4, other.AddressLine4) && string.Equals(Uprn, other.Uprn) &&
                   Equals(GeoPoint, other.GeoPoint);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_postcode != null ? _postcode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine1 != null ? AddressLine1.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine2 != null ? AddressLine2.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine3 != null ? AddressLine3.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AddressLine4 != null ? AddressLine4.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Uprn != null ? Uprn.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (GeoPoint != null ? GeoPoint.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}