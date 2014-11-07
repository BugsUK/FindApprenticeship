﻿// <copyright file="DataSteps.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SpecBind.ActionPipeline;
    using SpecBind.Actions;
    using SpecBind.Helpers;
    using SpecBind.Pages;

    using TechTalk.SpecFlow;

    /// <summary>
    /// Step bindings related to entering or validating data.
    /// </summary>
    [Binding]
    public class DataSteps : PageStepBase
    {
        // Step regex values - in constants because they are shared.
        private const string EnterDataInFieldsStepRegex = @"I enter data";
        private const string ObserveDataStepRegex = @"I see";
        private const string ObserveListDataStepRegex = @"I see (.+) list ([A-Za-z ]+)";

        // The following Regex items are for the given "past tense" form
        private const string GivenEnterDataInFieldsStepRegex = @"I entered data";
        private const string GivenObserveDataStepRegex = @"I saw";
        private const string GivenObserveListDataStepRegex = @"I saw (.+) list ([A-Za-z ]+)";
        
        private readonly IActionPipelineService actionPipelineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSteps"/> class.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="actionPipelineService">The action pipeline service.</param>
        public DataSteps(IScenarioContextHelper scenarioContext, IActionPipelineService actionPipelineService)
            : base(scenarioContext)
        {
            this.actionPipelineService = actionPipelineService;
        }

        /// <summary>
        /// A When step for entering data into fields.
        /// </summary>
        /// <param name="data">The field data.</param>
        [Given(GivenEnterDataInFieldsStepRegex)]
        [When(EnterDataInFieldsStepRegex)]
        [Then(EnterDataInFieldsStepRegex)]
        public void WhenIEnterDataInFieldsStep(Table data)
        {
            string fieldHeader = null;
            string valueHeader = null;

            if (data != null)
            {
                fieldHeader = data.Header.FirstOrDefault(h => h.NormalizedEquals("Field"));
                valueHeader = data.Header.FirstOrDefault(h => h.NormalizedEquals("Value"));
            }

            if (fieldHeader == null || valueHeader == null)
            {
                throw new ElementExecuteException("A table must be specified for this step with the columns 'Field' and 'Value'");
            }

            var page = this.GetPageFromContext();

            var results = new List<ActionResult>(data.RowCount);
            results.AddRange(from tableRow in data.Rows
                             let fieldName = tableRow[fieldHeader]
                             let fieldValue = tableRow[valueHeader]
                             select new EnterDataAction.EnterDataContext(fieldName.ToLookupKey(), fieldValue) into context
                             select this.actionPipelineService.PerformAction<EnterDataAction>(page, context));

            if (results.Any(r => !r.Success))
            {
                var errors = string.Join("; ", results.Where(r => r.Exception != null).Select(r => r.Exception.Message));
                throw new ElementExecuteException("Errors occurred while entering data. Details: {0}", errors);
            }
        }

        /// <summary>
        /// A Then step
        /// </summary>
        /// <param name="data">The field data.</param>
        [Given(GivenObserveDataStepRegex)]
        [Then(ObserveDataStepRegex)]
        public void ThenISeeStep(Table data)
        {
            var validations = data.ToValidationTable();
            var page = this.GetPageFromContext();

            var context = new ValidateItemAction.ValidateItemContext(validations);
            this.actionPipelineService.PerformAction<ValidateItemAction>(page, context).CheckResult();
        }

        /// <summary>
        /// A Then step for validating items in a list.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="data">The field data.</param>
        /// <exception cref="ElementExecuteException">A table must be specified for this step with the columns 'Field', 'Rule' and 'Value'</exception>
        [Given(GivenObserveListDataStepRegex)]
        [Then(ObserveListDataStepRegex)]
        public void ThenISeeListStep(string fieldName, string rule, Table data)
        {
            if (data == null || data.RowCount == 0)
            {
                return;
            }

            ComparisonType comparisonType;
            switch (rule.ToLookupKey())
            {
                case "exists":
                case "contains":
                    comparisonType = ComparisonType.Contains;
                    break;

                case "doesnotexist":
                case "doesnotcontain":
                    comparisonType = ComparisonType.DoesNotContain;
                    break;

                case "startswith":
                    comparisonType = ComparisonType.StartsWith;
                    break;

                case "endswith":
                    comparisonType = ComparisonType.EndsWith;
                    break;
                case "equals":
                    comparisonType = ComparisonType.Equals;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Rule type '{0}' is not supported.", rule));
            }

            var page = this.GetPageFromContext();
            var validations = data.ToValidationTable();

            var context = new ValidateListAction.ValidateListContext(fieldName.ToLookupKey(), comparisonType, validations);
            this.actionPipelineService.PerformAction<ValidateListAction>(page, context).CheckResult();
        }
    }
}