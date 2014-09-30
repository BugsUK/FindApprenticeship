﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18444
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.Registration
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Forgotten Password")]
    [NUnit.Framework.CategoryAttribute("US276")]
    public partial class ForgottenPasswordFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ForgottenPassword.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Forgotten Password", "As a candidate who has forgotten my password\r\nI want to request to reset my passw" +
                    "ord\r\nso that I can sign in to my account", ProgrammingLanguage.CSharp, new string[] {
                        "US276"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 8
#line 9
 testRunner.Given("I navigated to the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
 testRunner.When("I am on the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Reset password successful")]
        public virtual void ResetPasswordSuccessful()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Reset password successful", ((string[])(null)));
#line 12
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 13
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
 testRunner.When("I navigate to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 15
 testRunner.Then("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
#line 16
 testRunner.When("I enter data", ((string)(null)), table1, "When ");
#line 19
 testRunner.And("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 21
 testRunner.When("I get the token to reset the password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
 testRunner.And("I navigate to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
 testRunner.When("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
#line 24
 testRunner.And("I enter data", ((string)(null)), table2, "And ");
#line 27
 testRunner.And("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 29
 testRunner.And("I get the same token to reset the password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "PasswordResetCode",
                        "{PasswordResetCodeToken}"});
            table3.AddRow(new string[] {
                        "Password",
                        "{NewPasswordToken}"});
            table3.AddRow(new string[] {
                        "ConfirmPassword",
                        "{NewPasswordToken}"});
#line 30
 testRunner.When("I enter data", ((string)(null)), table3, "When ");
#line 35
 testRunner.And("I choose ResetPasswordButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.Then("I am on the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table4.AddRow(new string[] {
                        "SuccessMessageText",
                        "Equals",
                        "You\'ve successfully reset your password"});
#line 37
 testRunner.And("I see", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Reset password with an invalid email")]
        public virtual void ResetPasswordWithAnInvalidEmail()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Reset password with an invalid email", ((string[])(null)));
#line 41
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 42
 testRunner.Given("I registered an account and activated it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 43
 testRunner.And("I navigated to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
 testRunner.When("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "EmailAddress",
                        "invalid.email.address@invalid.com"});
#line 45
 testRunner.And("I enter data", ((string)(null)), table5, "And ");
#line 48
 testRunner.And("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 50
 testRunner.And("I don\'t receive an email with the token to reset the password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Reset password with a mismatching password")]
        public virtual void ResetPasswordWithAMismatchingPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Reset password with a mismatching password", ((string[])(null)));
#line 52
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 53
 testRunner.Given("I have registered a new candidate", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 54
 testRunner.When("I navigate to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 55
 testRunner.Then("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table6.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
#line 56
 testRunner.When("I enter data", ((string)(null)), table6, "When ");
#line 59
 testRunner.And("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 62
 testRunner.When("I get the token to reset the password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
 testRunner.And("I navigate to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.When("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
#line 65
 testRunner.And("I enter data", ((string)(null)), table7, "And ");
#line 68
 testRunner.And("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 71
 testRunner.And("I get the same token to reset the password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table8.AddRow(new string[] {
                        "PasswordResetCode",
                        "{PasswordResetCodeToken}"});
            table8.AddRow(new string[] {
                        "Password",
                        "{NewPasswordToken}"});
            table8.AddRow(new string[] {
                        "ConfirmPassword",
                        "!CannotPossiblyM4tch!"});
#line 72
 testRunner.When("I enter data", ((string)(null)), table8, "When ");
#line 77
 testRunner.And("I choose ResetPasswordButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table9.AddRow(new string[] {
                        "ValidationSummaryCount",
                        "Equals",
                        "1"});
#line 80
 testRunner.And("I see", ((string)(null)), table9, "And ");
#line 84
 testRunner.And("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table10.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Sorry, your passwords don’t match"});
            table10.AddRow(new string[] {
                        "Href",
                        "Equals",
                        "#Password"});
#line 85
 testRunner.And("I am on ValidationSummaryItems list item matching criteria", ((string)(null)), table10, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Reset password in an unactivated account")]
        public virtual void ResetPasswordInAnUnactivatedAccount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Reset password in an unactivated account", ((string[])(null)));
#line 90
this.ScenarioSetup(scenarioInfo);
#line 8
this.FeatureBackground();
#line 91
 testRunner.Given("I navigated to the RegisterCandidatePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 92
 testRunner.When("I have created a new email address", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table11.AddRow(new string[] {
                        "Firstname",
                        "FirstnameTest"});
            table11.AddRow(new string[] {
                        "Lastname",
                        "LastnameTest"});
            table11.AddRow(new string[] {
                        "Phonenumber",
                        "07970523193"});
            table11.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
            table11.AddRow(new string[] {
                        "PostcodeSearch",
                        "N7 8LS"});
            table11.AddRow(new string[] {
                        "Day",
                        "01"});
            table11.AddRow(new string[] {
                        "Month",
                        "01"});
            table11.AddRow(new string[] {
                        "Year",
                        "2000"});
            table11.AddRow(new string[] {
                        "Password",
                        "?Password01!"});
            table11.AddRow(new string[] {
                        "ConfirmPassword",
                        "?Password01!"});
#line 93
 testRunner.And("I enter data", ((string)(null)), table11, "And ");
#line 105
 testRunner.And("I choose HasAcceptedTermsAndConditions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
 testRunner.And("I choose FindAddresses", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table12.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Flat A, 6 Furlong Road"});
#line 107
 testRunner.And("I am on AddressDropdown list item matching criteria", ((string)(null)), table12, "And ");
#line 110
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
 testRunner.And("I am on the RegisterCandidatePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
 testRunner.And("I choose CreateAccountButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
 testRunner.Then("I wait 120 second for the ActivationPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 114
 testRunner.When("I navigate to the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 115
 testRunner.And("I am on the ForgottenPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table13.AddRow(new string[] {
                        "EmailAddress",
                        "{EmailToken}"});
#line 116
 testRunner.And("I enter data", ((string)(null)), table13, "And ");
#line 119
 testRunner.And("I choose SendCodeButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
 testRunner.Then("I am on the ResetPasswordPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 121
 testRunner.And("I get the token to reset the password", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table14.AddRow(new string[] {
                        "PasswordResetCode",
                        "{PasswordResetCodeToken}"});
            table14.AddRow(new string[] {
                        "Password",
                        "{NewPasswordToken}"});
            table14.AddRow(new string[] {
                        "ConfirmPassword",
                        "{NewPasswordToken}"});
#line 122
 testRunner.When("I enter data", ((string)(null)), table14, "When ");
#line 127
 testRunner.And("I choose ResetPasswordButton", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 128
 testRunner.Then("I am on the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table15.AddRow(new string[] {
                        "SuccessMessageText",
                        "Equals",
                        "You\'ve successfully reset your password"});
#line 129
 testRunner.And("I see", ((string)(null)), table15, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
