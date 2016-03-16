Feature: Apply for an apprenticeship vacancy
	As a candidate
	I want to submit apprenticeship applications 
	so that it can be reviewed by a Vacancy Manager

Background: 
	Given I navigated to the ApprenticeshipSearchPage page
	And I am logged out
	And I navigated to the ApprenticeshipSearchPage page
	Then I am on the ApprenticeshipSearchPage page

@US461 @US154 @US458 @US464
Scenario: As a candidate I want to save my apprenticeship application as a draft and be able to resume
	Given I have registered a new candidate
	When I select the "first" apprenticeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page
	When I enter data
		| Field                 | Value                 |
		| EducationNameOfSchool | SchoolName            |
		| EducationFromYear     | 2010                  |
		| EducationToYear       | 2012                  |
		| WhatAreYourStrengths  | My strengths          |
		| WhatCanYouImprove     | What can I improve    |
		| HobbiesAndInterests   | Hobbies and interests |
	And I choose SaveButton
	Then I wait to see ApplicationSavedMessage
	And I see
		| Field                   | Rule      | Value           |
		| ApplicationSavedMessage | Ends With | my applications |
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                  | Rule   | Value |
		| DraftApplicationsCount | Equals | 1     |
	When I choose ResumeLink
	Then I am on the ApprenticeshipApplicationPage page
	And I see
		| Field                   | Rule      | Value                 |
		| ApplicationSavedMessage | Ends With | my applications       |
		| EducationNameOfSchool   | Equals    | SchoolName            |
		| EducationFromYear       | Equals    | 2010                  |
		| EducationToYear         | Equals    | 2012                  |
		| WhatAreYourStrengths    | Equals    | My strengths          |
		| WhatCanYouImprove       | Equals    | What can I improve    |
		| HobbiesAndInterests     | Equals    | Hobbies and interests |
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page

@US461 @US362 @US365 @US154 @US463 @US352 @US354 @PrimaryTransaction
Scenario: As a candidate I want to enter my qualifications and work experience in an apprenticeship application
	Given I have registered a new candidate
	When I select the "first" apprenticeship vacancy in location "Coventry" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page

	# Qualifications
	When I choose QualificationsYes
	And I choose SaveQualification
	Then I see
		| Field                               | Rule   | Value |
		| QualificationsValidationErrorsCount | Equals | 4     |
	When I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the ApprenticeshipApplicationPage page
	When I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I am on the ApprenticeshipApplicationPage page
	And I choose SaveQualification
	#Should be removed when it works properly
	And I choose SaveQualification
	And I wait for 30 seconds to see QualificationsSummary
	Then I see
        | Field                      | Rule   | Value |
        | QualificationsSummaryCount | Equals | 1     |
	And I am on QualificationsSummaryItems list item matching criteria
		| Field   | Rule   | Value        |
		| Subject | Equals | SubjectName  |
		| Year    | Equals | 2012         |
		| Grade   | Equals | SubjectGrade |
	When I choose RemoveQualificationLink
	And I am on the ApprenticeshipApplicationPage page
	Then I see
        | Field                 | Rule           | Value |
        | QualificationsSummary | Does Not Exist |       |

	# Work Experience
	When I choose WorkExperienceYes
	And I choose SaveWorkExperience
	Then I see
		| Field                               | Rule   | Value |
		| WorkExperienceValidationErrorsCount | Equals | 5     |
	When I enter data
		| Field        | Value        |
		| WorkEmployer | WorkEmployer |
		| WorkTitle    | WorkTitle    |
		| WorkRole     | WorkRole     |
		| WorkFromYear | 2011         |
		| WorkToYear   | 2012         |
	And I choose SaveWorkExperience
	#Should be removed when it works properly
	And I choose SaveWorkExperience
	Then I wait for 30 seconds to see WorkExperienceSummary
	Then I see
        | Field                | Rule   | Value |
        | WorkExperiencesCount | Equals | 1     |
	And I am on WorkExperienceSummaryItems list item matching criteria
		| Field      | Rule   | Value        |
		| Employer   | Equals | WorkEmployer |
		| JobTitle   | Equals | WorkTitle    |
		| MainDuties | Equals | WorkRole     |
	When I choose RemoveWorkExperienceLink
	And I am on the ApprenticeshipApplicationPage page
	Then I see
        | Field                 | Rule           | Value |
        | WorkExperienceSummary | Does Not Exist |       |

	# Training Courses
	When I choose TrainingCoursesYes
	And I choose SaveTrainingCourseButton
	Then I see
		| Field                                | Rule   | Value |
		| TrainingCourseValidationErrorsCount | Equals | 4     |
	When I enter data
		| Field                  | Value                  |
		| TrainingCourseProvider | TrainingCourseProvider |
		| TrainingCourseTitle    | TrainingCourseTitle    |
		| TrainingCourseFromYear | 2011                   |
		| TrainingCourseToYear   | 2012                   |
	And I choose SaveTrainingCourseButton
	#Should be removed when it works properly
	And I choose SaveTrainingCourseButton
	Then I wait for 30 seconds to see TrainingCourseSummary
	Then I see
        | Field               | Rule   | Value |
        | TrainingCourseCount | Equals | 1     |
	And I am on TrainingCourseSummaryItems list item matching criteria
		| Field       | Rule   | Value                  |
		| Provider    | Equals | TrainingCourseProvider |
		| CourseTitle | Equals | TrainingCourseTitle    |
	When I choose RemoveTrainingCourseLink
	And I am on the ApprenticeshipApplicationPage page
	Then I see
        | Field                 | Rule           | Value |
        | TrainingCourseSummary | Does Not Exist |       |

	#Enter data to save
	When I enter data
		| Field                   | Value                         |
		| EducationNameOfSchool   | SchoolName                    |
		| EducationFromYear       | 2010                          |
		| EducationToYear         | 2012                          |
		| WhatAreYourStrengths    | My strengths                  |
		| WhatCanYouImprove       | What can I improve            |
		| HobbiesAndInterests     | Hobbies and interests         |
	And I enter employer question data if present
		| Field                                              | Value |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer1 | Emp 1 |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer2 | Emp 2 |
	And I choose QualificationsYes
	And I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the ApprenticeshipApplicationPage page
	And I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I choose SaveQualification
	When I choose WorkExperienceYes
	And I enter data
		| Field        | Value        |
		| WorkEmployer | WorkEmployer |
		| WorkTitle    | WorkTitle    |
		| WorkRole     | WorkRole     |
		| WorkFromYear | 2011         |
		| WorkToYear   | 2012         |
	And I choose SaveWorkExperience

	When I choose TrainingCoursesYes
	And I enter data
		| Field                  | Value               |
		| TrainingCourseProvider | TrainingProvider    |
		| TrainingCourseTitle    | TrainingCourseTitle |
		| TrainingCourseFromYear | 2011                |
		| TrainingCourseToYear   | 2012                |
	And I choose SaveTrainingCourseButton

	And I choose SaveButton
	Then I wait to see ApplicationSavedMessage
	And I see
		| Field                   | Rule      | Value           |
		| ApplicationSavedMessage | Ends With | my applications |
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                  | Rule   | Value |
		| DraftApplicationsCount | Equals | 1     |
	When I choose ResumeLink
	Then I wait 120 second for the ApprenticeshipApplicationPage page
	When I am on the ApprenticeshipApplicationPage page
	Then I see
        | Field                      | Rule   | Value |
        | QualificationsSummaryCount | Equals | 1     |
	And I am on QualificationsSummaryItems list item matching criteria
		| Field   | Rule   | Value        |
		| Subject | Equals | SubjectName  |
		| Year    | Equals | 2012         |
		| Grade   | Equals | SubjectGrade |
	And I am on the ApprenticeshipApplicationPage page
	And I see
        | Field                | Rule   | Value |
        | WorkExperiencesCount | Equals | 1     |
	And I am on WorkExperienceSummaryItems list item matching criteria
		| Field      | Rule   | Value        |
		| Employer   | Equals | WorkEmployer |
		| JobTitle   | Equals | WorkTitle    |
		| MainDuties | Equals | WorkRole     |
	When I am on the ApprenticeshipApplicationPage page
	And I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPreviewPage page
	And I see
		| Field                         | Rule           | Value                 |
		| ApplicationSavedTopMessage    | Ends With      | my applications       |
		| ApplicationSavedBottomMessage | Ends With      | my applications       |
		| EducationNameOfSchool         | Equals         | SchoolName            |
		| EducationFromYear             | Equals         | 2010                  |
		| EducationToYear               | Equals         | 2012                  |
		| NoQualificationsMessage       | Does Not Exist |                       |
		| NoWorkExperienceMessage       | Does Not Exist |                       |
		| NoTrainingCoursesMessage      | Does Not Exist |                       |
		| WhatAreYourStrengths          | Equals         | My strengths          |
		| WhatCanYouImprove             | Equals         | What can I improve    |
		| HobbiesAndInterests           | Equals         | Hobbies and interests |
	When I choose AcceptSubmit
	Then I am on the ApprenticeshipApplicationPreviewPage page
	When I choose SubmitApplication
	Then I am on the ApprenticeshipApplicationCompletePage page
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                      | Rule   | Value |
		| SubmittedApplicationsCount | Equals | 1     |

@US154
Scenario: As a candidate I would like to see my apprenticeship application as submitted
	Given I have registered a new candidate
	When I select the "first" apprenticeship vacancy in location "London" that can apply by this website
	Then I am on the ApprenticeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPage page
	When I choose SupportMeYes
	And I enter data
		| Field                   | Value                         |
		| EducationNameOfSchool   | InProgress                    |
		| EducationFromYear       | 2010                          |
		| EducationToYear         | 2012                          |
		| WhatAreYourStrengths    | My strengths                  |
		| WhatCanYouImprove       | What can I improve            |
		| HobbiesAndInterests     | Hobbies and interests         |
		| WhatCanWeDoToSupportYou | What can we do to support you |
	And I enter employer question data if present
		| Field                                              | Value |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer1 | Emp 1 |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer2 | Emp 2 |
	And I choose ApplyButton
	Then I am on the ApprenticeshipApplicationPreviewPage page
	When I choose AcceptSubmit
	Then I am on the ApprenticeshipApplicationPreviewPage page
	When I choose SubmitApplication
	Then I am on the ApprenticeshipApplicationCompletePage page
	When I wait 5 seconds
	When I choose MyApplicationsLink
	Then I am on the MyApplicationsPage page
	And I see
		| Field                      | Rule   | Value |
		| SubmittedApplicationsCount | Equals | 1     |
	And I navigate to the ApprenticeshipDetailsPage page with parameters
		| VacancyId   |
		| {VacancyId} |
	Then I see
		| Field                      | Rule   | Value |
		| TrackApplicationStatusLink | Exists |       |
	When I choose TrackApplicationStatusLink
	Then I am on the MyApplicationsPage page
