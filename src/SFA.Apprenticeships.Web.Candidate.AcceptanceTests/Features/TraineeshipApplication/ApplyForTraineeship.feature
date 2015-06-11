﻿@US583
Feature: Apply for a traineeship vacancy
	As a candidate
	I want to submit traineeship applications 
	so that it can be reviewed by a Vacancy Manager

Background: 
	Given I navigated to the TraineeshipSearchPage page
	And I am logged out
	And I navigated to the TraineeshipSearchPage page
	Then I am on the TraineeshipSearchPage page

@US592
Scenario: As a candidate I am taken to login or create an account when viewing traineeship details
	Given I select the "first" traineeship vacancy in location "N7 8LS" that can apply by this website
	When I am on the TraineeshipDetailsPage page
	And I choose ApplyButton
	Then I am on the LoginPage page

@PrimaryTransaction
Scenario: As a candidate I want to enter my qualifications and work experience in a traineeship application
	Given I have registered a new candidate
	When I select the "first" traineeship vacancy in location "N7 8LS" that can apply by this website
	Then I am on the TraineeshipDetailsPage page
	When I choose ApplyButton
	Then I am on the TraineeshipApplicationPage page

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
	And I am on the TraineeshipApplicationPage page
	When I enter data
		| Field        | Value        |
		| SubjectYear  | 2012         |
		| SubjectName  | SubjectName  |
		| SubjectGrade | SubjectGrade |
	And I am on the TraineeshipApplicationPage page
	And I choose SaveQualification
	# Should be removed when it works properly
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
	And I am on the TraineeshipApplicationPage page
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
	# Should be removed when it works properly
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
	And I am on the TraineeshipApplicationPage page
	Then I see
        | Field                 | Rule           | Value |
        | WorkExperienceSummary | Does Not Exist |       |

	# Training Courses
	When I choose TrainingCoursesYes
	And I choose SaveTrainingCourseButton
	Then I see
		| Field                               | Rule   | Value |
		| TrainingCourseValidationErrorsCount | Equals | 4     |
	When I enter data
		| Field                  | Value                  |
		| TrainingCourseProvider | TrainingCourseProvider |
		| TrainingCourseTitle    | TrainingCourseTitle    |
		| TrainingCourseFromYear | 2011                   |
		| TrainingCourseToYear   | 2012                   |
	And I choose SaveTrainingCourseButton
	# Should be removed when it works properly
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
	And I am on the TraineeshipApplicationPage page
	Then I see
        | Field                 | Rule           | Value |
        | TrainingCourseSummary | Does Not Exist |       |

	# Enter data to save
	When I enter employer question data if present
		| Field                                              | Value |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer1 | Emp 1 |
		| Candidate_EmployerQuestionAnswers_CandidateAnswer2 | Emp 2 |
	And I choose QualificationsYes
	And I am on QualificationTypeDropdown list item matching criteria
		| Field | Rule   | Value |
		| Text  | Equals | GCSE  |
	And I choose WrappedElement
	And I am on the TraineeshipApplicationPage page
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

	#When I am on the TraineeshipApplicationPage page
	And I choose ApplyButton
	Then I am on the TraineeshipWhatsNextPage page