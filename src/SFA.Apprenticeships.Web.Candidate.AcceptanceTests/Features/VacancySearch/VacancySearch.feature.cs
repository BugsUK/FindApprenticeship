﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34014
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Features.VacancySearch
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Vacancy Search")]
    [NUnit.Framework.CategoryAttribute("US496")]
    [NUnit.Framework.CategoryAttribute("US449")]
    public partial class VacancySearchFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "VacancySearch.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Vacancy Search", "In order to find a vacancy apprenticeship quickly\r\nAs a candidate\r\nI want to find" +
                    " a vacancy apprenticeship by location or keywords", ProgrammingLanguage.CSharp, new string[] {
                        "US496",
                        "US449"});
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
#line 7
#line 8
 testRunner.Given("I navigated to the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.And("I am logged out", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("I navigated to the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.Then("I am on the HomePage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find apprenticeships and test ordering without keywords")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindApprenticeshipsAndTestOrderingWithoutKeywords()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find apprenticeships and test ordering without keywords", new string[] {
                        "SmokeTests"});
#line 14
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 15
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Location",
                        "Coventry"});
            table1.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
#line 16
 testRunner.When("I enter data", ((string)(null)), table1, "When ");
#line 20
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table2.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
            table2.AddRow(new string[] {
                        "ResultsAreInDistanceOrder",
                        "Equals",
                        "True"});
#line 22
 testRunner.Then("I see", ((string)(null)), table2, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table3.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Closing Date"});
#line 29
 testRunner.And("I enter data", ((string)(null)), table3, "And ");
#line 32
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table4.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
            table4.AddRow(new string[] {
                        "ResultsAreInClosingDateOrder",
                        "Equals",
                        "True"});
#line 33
 testRunner.And("I see", ((string)(null)), table4, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find apprenticeships and test ordering with keywords")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindApprenticeshipsAndTestOrderingWithKeywords()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find apprenticeships and test ordering with keywords", new string[] {
                        "SmokeTests"});
#line 42
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 43
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table5.AddRow(new string[] {
                        "Keywords",
                        "Admin"});
            table5.AddRow(new string[] {
                        "Location",
                        "Coventry"});
            table5.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
#line 44
 testRunner.When("I enter data", ((string)(null)), table5, "When ");
#line 49
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table6.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
#line 51
 testRunner.Then("I see", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find apprenticeships and test paging")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void FindApprenticeshipsAndTestPaging()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Find apprenticeships and test paging", new string[] {
                        "SmokeTests"});
#line 59
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 60
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table7.AddRow(new string[] {
                        "Location",
                        "Coventry"});
            table7.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
#line 61
 testRunner.When("I enter data", ((string)(null)), table7, "When ");
#line 65
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
 testRunner.And("I wait to not see PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
 testRunner.And("I choose NextPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 70
 testRunner.And("I wait to see PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table8.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "3 of"});
            table8.AddRow(new string[] {
                        "PreviousPage",
                        "Contains",
                        "1 of"});
#line 71
 testRunner.Then("I see", ((string)(null)), table8, "Then ");
#line 75
 testRunner.When("I choose NextPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 76
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table9.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "4 of"});
            table9.AddRow(new string[] {
                        "PreviousPage",
                        "Contains",
                        "2 of"});
#line 77
 testRunner.Then("I see", ((string)(null)), table9, "Then ");
#line 81
 testRunner.When("I choose PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 82
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table10.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "3 of"});
            table10.AddRow(new string[] {
                        "PreviousPage",
                        "Contains",
                        "1 of"});
#line 83
 testRunner.Then("I see", ((string)(null)), table10, "Then ");
#line 87
 testRunner.When("I choose PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 88
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
 testRunner.And("I wait to not see PreviousPage", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table11.AddRow(new string[] {
                        "NextPage",
                        "Contains",
                        "2 of"});
#line 90
 testRunner.Then("I see", ((string)(null)), table11, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search when no results are returned for location")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void SearchWhenNoResultsAreReturnedForLocation()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search when no results are returned for location", new string[] {
                        "SmokeTests"});
#line 95
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 96
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table12.AddRow(new string[] {
                        "Keywords",
                        "Chef"});
            table12.AddRow(new string[] {
                        "Location",
                        "Dundee"});
#line 97
 testRunner.When("I enter data", ((string)(null)), table12, "When ");
#line 101
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table13.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Does Not Exist",
                        ""});
            table13.AddRow(new string[] {
                        "NoResultsTitle",
                        "Exists",
                        ""});
#line 103
 testRunner.Then("I see", ((string)(null)), table13, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search doesn\'t error when location doesn\'t exist")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void SearchDoesnTErrorWhenLocationDoesnTExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search doesn\'t error when location doesn\'t exist", new string[] {
                        "SmokeTests"});
#line 109
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 110
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table14.AddRow(new string[] {
                        "Location",
                        "KJHNSAKDFJHA"});
#line 111
 testRunner.When("I enter data", ((string)(null)), table14, "When ");
#line 114
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table15.AddRow(new string[] {
                        "SortOrderingDropDown",
                        "Does Not Exist",
                        ""});
            table15.AddRow(new string[] {
                        "NoResultsTitle",
                        "Exists",
                        ""});
#line 116
 testRunner.Then("I see", ((string)(null)), table15, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Search location autocomplete appears on both initial search page and search resul" +
            "ts page")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void SearchLocationAutocompleteAppearsOnBothInitialSearchPageAndSearchResultsPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search location autocomplete appears on both initial search page and search resul" +
                    "ts page", new string[] {
                        "SmokeTests"});
#line 122
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 123
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table16.AddRow(new string[] {
                        "Location",
                        "Coventry"});
#line 124
 testRunner.When("I enter data", ((string)(null)), table16, "When ");
#line 127
 testRunner.Then("I wait for 5 seconds to see LocationAutoComplete", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table17.AddRow(new string[] {
                        "Text",
                        "Equals",
                        "Coventry (West Midlands)"});
#line 128
 testRunner.When("I am on LocationAutoCompletItems list item matching criteria", ((string)(null)), table17, "When ");
#line 131
 testRunner.And("I choose WrappedElement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 132
 testRunner.And("I am on the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
 testRunner.And("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table18.AddRow(new string[] {
                        "ClearLocation",
                        "Equals",
                        "True"});
#line 135
 testRunner.Then("I see", ((string)(null)), table18, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table19.AddRow(new string[] {
                        "Location",
                        "London"});
#line 138
 testRunner.When("I enter data", ((string)(null)), table19, "When ");
#line 141
 testRunner.Then("I wait for 5 seconds to see LocationAutoComplete", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Different results per page")]
        [NUnit.Framework.CategoryAttribute("US517")]
        [NUnit.Framework.CategoryAttribute("SmokeTests")]
        public virtual void DifferentResultsPerPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Different results per page", new string[] {
                        "US517",
                        "SmokeTests"});
#line 144
this.ScenarioSetup(scenarioInfo);
#line 7
this.FeatureBackground();
#line 145
 testRunner.Given("I navigated to the VacancySearchPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table20.AddRow(new string[] {
                        "Location",
                        "London"});
            table20.AddRow(new string[] {
                        "WithInDistance",
                        "40 miles"});
#line 146
 testRunner.When("I enter data", ((string)(null)), table20, "When ");
#line 150
 testRunner.And("I choose Search", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table21.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "5"});
#line 152
 testRunner.And("I see", ((string)(null)), table21, "And ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table22.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "10 per page"});
#line 155
 testRunner.When("I enter data", ((string)(null)), table22, "When ");
#line 158
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table23.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "10"});
#line 159
 testRunner.And("I see", ((string)(null)), table23, "And ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table24.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "25 per page"});
#line 162
 testRunner.When("I enter data", ((string)(null)), table24, "When ");
#line 165
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table25 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table25.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "25"});
#line 166
 testRunner.And("I see", ((string)(null)), table25, "And ");
#line hidden
            TechTalk.SpecFlow.Table table26 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table26.AddRow(new string[] {
                        "ResultsPerPageDropDown",
                        "50 per page"});
#line 169
 testRunner.When("I enter data", ((string)(null)), table26, "When ");
#line 172
 testRunner.Then("I am on the VacancySearchResultPage page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table27 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Rule",
                        "Value"});
            table27.AddRow(new string[] {
                        "SearchResultItemsCount",
                        "Equals",
                        "50"});
#line 173
 testRunner.And("I see", ((string)(null)), table27, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
