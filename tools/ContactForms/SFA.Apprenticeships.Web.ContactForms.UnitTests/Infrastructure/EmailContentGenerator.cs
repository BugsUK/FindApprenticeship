namespace SFA.Apprenticeships.Web.ContactForms.Tests.Infrastructure
{
    using System.Text;
    using Builders;
    using Common.Extensions;

    public static class EmailContentGenerator
    {
        public static string CreateContactForm()
        {
            string firstname = "First",
                lastname = "Last",
                companyname = "companyName",
                email = "valid@email.com",
                employeeCount = "100",
                enquiryDescription = "test query",
                enquirySource = "Telephone Call",
                title = "Mr",
                prevExp = "Yes",
                position = "Position",
                workSector = "retail",
                phoneNumber = "0987654321",
                mobile = "1234567890",
                address1 = "Line1",
                city = "City",
                postcode = "Postcode";
            var addressViewModelBuilder =
                new AddressViewModelBuilder().AddressLine1(address1).City(city).Postcode(postcode).Build();

            var enquiryData = new EmployerEnquiryViewModelBuilder().Firstname(firstname).Lastname(lastname).MobileNumber(mobile)
                .PhoneNumber(phoneNumber).Position(position).PrevExp(prevExp).Title(title)
                .WorkSector(workSector)
                .EnquirySource(enquirySource).EnquiryDescription(enquiryDescription).EmployeeCount(employeeCount)
                .Email(email).Companyname(companyname)
                .Address(addressViewModelBuilder)
                .Build();

            var builder = new StringBuilder();
            builder.AppendLine(string.Format("Full name : {0} {1} {2}",enquiryData.Title, enquiryData.Firstname.ToFirstCharToUpper(), enquiryData.Lastname.ToFirstCharToUpper()));
            builder.AppendLine(string.Format("Email : {0}", enquiryData.Email));
            builder.AppendLine(string.Format("Position at company : {0}", enquiryData.Position.ToFirstCharToUpper()));
            builder.AppendLine(string.Format("Phone number : {0}", enquiryData.WorkPhoneNumber));
            if (!string.IsNullOrEmpty(enquiryData.MobileNumber))
            {
                builder.AppendLine(string.Format("Mobile : {0}", enquiryData.MobileNumber));
            }

            builder.AppendLine(string.Format("Company name : {0}", enquiryData.Companyname.ToFirstCharToUpper()));
            builder.AppendLine(string.Format("Address: line 1 - {0}", enquiryData.Address.AddressLine1));

            if (!string.IsNullOrEmpty(enquiryData.Address.AddressLine2))
            {
                builder.AppendLine(string.Format("Address : line 2 - {0}", enquiryData.Address.AddressLine2));
            }
            if (!string.IsNullOrEmpty(enquiryData.Address.AddressLine3))
            {
                builder.AppendLine(string.Format("Address : line 3 - {0}", enquiryData.Address.AddressLine3));
            }
            if (!string.IsNullOrEmpty(enquiryData.Address.City))
            {
                builder.AppendLine(string.Format("Address : city - {0}", enquiryData.Address.City.ToFirstCharToUpper()));
            }

            builder.AppendLine(string.Format("Address : postcode - {0}", enquiryData.Address.Postcode));
            builder.AppendLine(string.Format("Total no of employees : {0}", enquiryData.EmployeesCount));
            builder.AppendLine(string.Format("Industry sector : {0}", enquiryData.WorkSector));
            builder.AppendLine(string.Format("Previous experience with Apprenticeships/Traineeships? : {0}", enquiryData.PreviousExperienceType));
            builder.AppendLine(string.Format("What prompted to make enquiry : {0}", enquiryData.EnquirySource));
            builder.AppendLine(string.Format("Nature of query : {0}", enquiryData.EnquiryDescription.ToFirstCharToUpper()));

            return builder.ToString();
        }
    }
}