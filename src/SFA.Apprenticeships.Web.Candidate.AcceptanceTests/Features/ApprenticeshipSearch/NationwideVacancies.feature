@US500
Feature: Nationwide apprenticeships
	As a candidate
	I want to be able to see apprenticeships that exist nationwide
	so that I can see opportunities that may be of interest to me irrespective of my location

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@SmokeTests
Scenario: After clicking on nationwide apprenticeships I see them
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |
        | SearchResultItemsCount     | Greater Than   | 0     |

Scenario: Remebering last location search does not break nationwide results
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field    | Value |
		 | Location | Hull  |
	And I wait for 5 seconds to see LocationAutoComplete
	And I choose SearchHeader
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |
        | SearchResultItemsCount     | Greater Than   | 0     |
	When I choose HeaderLinkFaa
	Then I am on the ApprenticeshipSearchPage page
	Then I see
	    | Field         | Rule   | Value                           |
	    | Location      | Equals | Hull (East Riding of Yorkshire) |
	    | ClearLocation | Equals | Cleared                         |
	When I enter data
		 | Field    | Value      |
		 | Location | Birmingham |
	And I wait for 5 seconds to see LocationAutoComplete
	And I choose SearchHeader
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |
        | SearchResultItemsCount     | Greater Than   | 0     |

@SmokeTests
#TODO: replace it with view unit tests?
Scenario: Nationwide apprenticeships do not show distance
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	Then I see SearchResults list contains
        | Field                | Rule   | Value |
        | DistanceDisplayed    | Equals | False |
        | ClosingDateDisplayed | Equals | True  |
        | NationwideDisplayed  | Equals | True  |

@SmokeTests
#TODO: replace it with integration tests?
Scenario: Nationwide apprenticeships are in closing date order
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                        | Rule   | Value |
        | ResultsAreInClosingDateOrder | Equals | True  |

@SmokeTests
Scenario: Nationwide apprenticeships found by keyword can be ordered
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | it         |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	And I am on the ApprenticeshipSearchResultPage page
	Then I see 
        | Field                      | Rule         | Value        |
        | SearchResultItemsCount     | Greater Than | 0            |
        | SortOrderingDropDown       | Equals       | Best match   |
        | NationwideLocationTypeLink | Exists       |              |
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                          | Rule           | Value                                  |
        | LocalLocationTypeLink          | Exists         |                                        |
        | NationwideLocationTypeLink     | Does Not Exist |                                        |
        | SortOrderingDropDownItemsCount | Equals         | 3                                      |
        | SortOrderingDropDownItemsText  | Equals         | Best match,Closing date,Recently added |
        | SortOrderingDropDown           | Equals         | Best match                             |

@SmokeTests
Scenario: When I'm seeing nationwide apprenticeships and I change the results per page I remain there
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	When I enter data
		| Field                  | Value       |
		| ResultsPerPageDropDown | 25 per page |
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |

@SmokeTests
Scenario: When I'm seeing nationwide apprenticeships and I change the sort order I remain there
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
		 | Field               | Value      |
		 | Keywords            | it         |
		 | Location            | London     |
		 | WithInDistance      | 40 miles   |
		 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	When I choose NationwideLocationTypeLink
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	When I enter data
		| Field                | Value        |
		| SortOrderingDropDown | Closing date |
	Then I am on the ApprenticeshipSearchResultPage page
	And I wait 3 seconds
	And I see
        | Field                      | Rule           | Value |
        | LocalLocationTypeLink      | Exists         |       |
        | NationwideLocationTypeLink | Does Not Exist |       |