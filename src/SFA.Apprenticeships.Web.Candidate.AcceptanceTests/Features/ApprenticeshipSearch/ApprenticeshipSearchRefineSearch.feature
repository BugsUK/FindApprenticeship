Feature: ApprenticeshipSearchRefineSearch
	In order to filter search results by specific fields
	As a candidate apprentice
	I want be able to select which fields to filter by

Scenario: The refine search options should display correctly
	Given I navigated to the ApprenticeshipSearchPage page
	Then I see
        | Field            | Rule           | Value |
        | RefineSearchLink | Exists         |       |
        | RefineControls   | Does Not Exist |       |
	When I choose RefineSearchLink
	Then I see
        | Field            | Rule   | Value |
        | RefineSearchLink | Exists |       |
        | RefineControls   | Exists |       |
	When I choose RefineSearchLink
	Then I see
        | Field            | Rule           | Value |
        | RefineSearchLink | Exists         |       |
        | RefineControls   | Does Not Exist |       |
	When I choose RefineSearchLink
	Then I see
        | Field            | Rule   | Value |
        | RefineSearchLink | Exists |       |
        | RefineControls   | Exists |       |

Scenario: The refine search options should display on results page if selected
	Given I navigated to the ApprenticeshipSearchPage page
	When I choose RefineSearchLink
	And I choose RefineControlJobTitle
	When I enter data
			 | Field               | Value      |
			 | Keywords            | admin      |
			 | Location            | London     |
			 | WithInDistance      | 40 miles   |
			 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field                        | Rule   | Value |
        | RefineSearchLink             | Exists |       |
        | RefineControls               | Exists |       |
        | RefineControlJobTitleChecked | Equals | True  |
