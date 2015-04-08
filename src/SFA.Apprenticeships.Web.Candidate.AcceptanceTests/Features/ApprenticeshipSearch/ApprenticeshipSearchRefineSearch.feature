@US702
Feature: ApprenticeshipSearchRefineSearch
	In order to filter search results by specific fields
	As a candidate apprentice
	I want be able to select which fields to filter by

Scenario: The refine search options should display correctly
	Given I navigated to the ApprenticeshipSearchPage page
	Then I see
        | Field       | Rule   | Value |
        | SearchField | Exists |       |
        | SearchField | Equals | All   |

Scenario: The refine search option should be correct on results page if all was selected
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
			 | Field               | Value      |
			 | Location            | London     |
			 | WithInDistance      | 40 miles   |
			 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field       | Rule   | Value               |
        | SearchField | Exists |                     |
        | SearchField | Equals | -- Refine search -- |

Scenario: The refine search option should be Job title on results page if Job title was selected
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
			 | Field               | Value      |
			 | SearchField         | Job title  |
			 | Location            | London     |
			 | WithInDistance      | 40 miles   |
			 | ApprenticeshipLevel | All levels |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field       | Rule   | Value     |
        | SearchField | Exists |           |
        | SearchField | Equals | Job title |

Scenario: The refine search option should be Ref number on results page if Ref number was selected
	Given I navigated to the ApprenticeshipSearchPage page
	When I enter data
			 | Field               | Value        |
			 | SearchField         | Ref. number  |
			 | Keywords            | VAC000123456 |
			 | Location            | London       |
			 | WithInDistance      | 40 miles     |
			 | ApprenticeshipLevel | All levels   |
	And I choose Search
	Then I am on the ApprenticeshipSearchResultPage page
	And I see
        | Field       | Rule   | Value       |
        | SearchField | Exists |             |
        | SearchField | Equals | Ref. number |
