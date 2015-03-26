namespace SFA.Apprenticeships.Web.ContactForms.Tests.Builders
{
    using ContactForms.ViewModels;

    public class AccessRequestViewModelBuilder
    {
        private string _title;
        private string _firstname = "First";
        private string _lastname = "Last";
        private string _email;
        private string _confirmEmail;
        private string _phoneNumber;
        private string _mobileNumber;
        private string _position;
        private string _companyName;
        private string _userType;
        private AddressViewModel _addressViewModel;

        public AccessRequestViewModelBuilder ConfirmEmail(string confirmEmaiil)
        {
            _confirmEmail = confirmEmaiil;
            return this;
        }

        public AccessRequestViewModelBuilder UserType(string usertype)
        {
            _userType = usertype;
            return this;
        }
        public AccessRequestViewModelBuilder MobileNumber(string mobile)
        {
            _mobileNumber = mobile;
            return this;
        }
        public AccessRequestViewModelBuilder Position(string postion)
        {
            _position = postion;
            return this;
        }
        public AccessRequestViewModelBuilder Companyname(string companyname)
        {
            _companyName = companyname;
            return this;
        }
        public AccessRequestViewModelBuilder Title(string title)
        {
            _title = title;
            return this;
        }
        
        public AccessRequestViewModelBuilder Email(string email)
        {
            _email = email;
            return this;
        }

        public AccessRequestViewModelBuilder Address(AddressViewModel addressViewModel)
        {
            _addressViewModel= addressViewModel;
            return this;
        }

        public AccessRequestViewModelBuilder Firstname(string firstname)
        {
            _firstname = firstname;
            return this;
        }

        public AccessRequestViewModelBuilder Lastname(string lastname)
        {
            _lastname = lastname;
            return this;
        }
        public AccessRequestViewModelBuilder PhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        public AccessRequestViewModel Build()
        {
            var model = new AccessRequestViewModel()
            {
                Firstname = _firstname,
                Lastname = _lastname,
                WorkPhoneNumber = _phoneNumber,
                MobileNumber = _mobileNumber,
                Position = _position,
                Companyname = _companyName,
                Title = _title,
                Email = _email,
                ConfirmEmail = _confirmEmail,
                UserType = _userType,
                Address = _addressViewModel
            };

            return model;
        }
    }
}