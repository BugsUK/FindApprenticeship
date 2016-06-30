namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators
{
    using FluentAssertions;
    using NUnit.Framework;
    using Raa.Common.Validators;

    [TestFixture]
    public class ValidFreeTextTest
    {        
        [TestCase("")]       
        public void BeAValidFreeText_EmptyString_Valid(string emptyInput)
        {
            //Act
            bool isValid = Common.BeAValidFreeText(emptyInput);

            //Assert
            isValid.Should().BeTrue("Empty string is a valid string");
        }

        [TestCase("<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, " +
                  "friendly nursery.  You will be caring for and meeting <br>the needs of <br><br>children aged between " +
                  "4 months and 5 years, creating a safe environment and providing stimulating </h1>\r\n<br></br> <br></br>" +
                  "<br></br><script>alert('Hello World')</script>")]
        [TestCase("<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, " +
                  "friendly nursery.  You will be caring for and meeting <br>the needs of <br><br>children aged between " +
                  "4 months and 5 years, creating a safe environment and providing stimulating </h1>\r\n<br></br> <br></br>" +
                  "<br></br><input type='button'></input>")]
        [TestCase("<h1>Excellent op<h2>portunities</h2> for Pre School Apprentice&#39;s are available in this established, " +
                  "friendly nursery.  You will be caring for and meeting <br>the needs of <br><br>children aged between " +
                  "4 months and 5 years, creating a safe environment and providing stimulating </h1>\r\n<br></br> <br></br>" +
                  "<br></br><object id='object1'></object>")]
        public void BeAValidFreeText_InvalidString_ReturnFalse(string invalidString)
        {
            //Act
            bool isValid = Common.BeAValidFreeText(invalidString);

            //Assert
            isValid.Should().BeFalse("Return false as the string is an invalid!!");
        }
    }
}
