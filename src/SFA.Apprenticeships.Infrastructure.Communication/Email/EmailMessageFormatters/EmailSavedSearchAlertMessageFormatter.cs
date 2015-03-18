namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Xsl;
    using Application.Interfaces.Communications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using Newtonsoft.Json;
    using SendGrid;

    public class EmailSavedSearchAlertMessageFormatter : EmailMessageFormatter
    {
        private const string HttpsScheme = "https://";

        private readonly string _siteDomainName;

        public EmailSavedSearchAlertMessageFormatter(IConfigurationManager configurationManager)
        {
            _siteDomainName = configurationManager.GetAppSetting<string>("SiteDomainName");
        }

        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            PopulateCandidateName(request, message);
            PopulateSavedSearchAlerts(request, message);
        }

        private void PopulateSavedSearchAlerts(EmailRequest request, ISendGrid message)
        {
            var token = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.SavedSearchAlerts);
            var value = request.Tokens.First(t => t.Key == CommunicationTokens.SavedSearchAlerts).Value;

            var json = TransformTokenValueToJson(value);
            var html = TransformJsonToHtml(json);

            AddSubstitutionTo(message, token, html);
        }

        private string TransformJsonToHtml(dynamic json)
        {
            var wrappedJson = string.Format("{{ \"savedSearchAlert\": {0} }}", JsonConvert.SerializeObject(json));
            var xml = JsonConvert.DeserializeXmlNode(wrappedJson, "savedSearchAlerts");
            var transform = new XslCompiledTransform();

            var settings = new XmlWriterSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                Indent = true,
                DoNotEscapeUriAttributes = true
            };

            using (var sw = new StringWriter())
            using (var xw = XmlWriter.Create(sw, settings))
            {
                transform.Load(GetStylesheetUri());
                transform.Transform(xml, xw);

                return sw.ToString();
            }
        }

        private static string GetStylesheetUri()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            // ReSharper disable once AssignNullToNotNullAttribute
            return Path.Combine(path, "Email", "EmailTemplates", "SavedSearchAlertsEmail.xslt");
        }

        private dynamic TransformTokenValueToJson(string value)
        {
            var savedSearchAlerts = JsonConvert.DeserializeObject<List<SavedSearchAlert>>(value);

            return savedSearchAlerts.Select(savedSearchAlert => new
            {
                resultsCount = savedSearchAlert.Results.Count(),
                parameters = new
                {
                    name = savedSearchAlert.Parameters.Name(),
                    url = FormatSearchUrl(savedSearchAlert),
                    searchMode = GetSearchModeName(savedSearchAlert.Parameters.SearchMode),
                    keywords = savedSearchAlert.Parameters.Keywords,
                    category = savedSearchAlert.Parameters.CategoryFullName,
                    subCategories = savedSearchAlert.Parameters.SubCategoriesFullName,
                    location = savedSearchAlert.Parameters.Location,
                    apprenticeshipLevel = savedSearchAlert.Parameters.ApprenticeshipLevel,
                },
                results = savedSearchAlert.Results.Select(result => new
                {
                    url = FormatVacancyDetailsUrl(result),
                    title = result.Title,
                    employerName = result.EmployerName,
                    description = result.Description,
                    closingDate = FormatDate(result.ClosingDate),
                    distance = FormatDistance(result.Distance)
                })
            });
        }

        #region Helpers

        // TODO: US638: most of these functions are all candidates to be moved elsewhere with supporting tests.

        private string FormatDistance(double distance)
        {
            return Math.Round(distance, 1, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
        }

        private string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("d MMM yyyy");
        }

        private string GetSearchModeName(ApprenticeshipSearchMode searchMode)
        {
            return searchMode == ApprenticeshipSearchMode.Keyword ? "keyword" : "category";
        }

        private string FormatSearchUrl(SavedSearchAlert savedSearchAlert)
        {
            return string.Format("{0}{1}/{2}", HttpsScheme, _siteDomainName, savedSearchAlert.Parameters.SearchUrl().Value);
        }

        private string FormatVacancyDetailsUrl(ApprenticeshipSearchResponse apprenticeshipSearchResponse)
        {
            return string.Format("{0}{1}/apprenticeship/{2}", HttpsScheme, _siteDomainName, apprenticeshipSearchResponse.Id);
        }

        private void PopulateCandidateName(EmailRequest request, ISendGrid message)
        {
            var token = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.CandidateFirstName);
            var value = request.Tokens.First(t => t.Key == CommunicationTokens.CandidateFirstName).Value;

            AddSubstitutionTo(message, token, value);
        }

        private void AddSubstitutionTo(ISendGrid message, string replacementTag, string value)
        {
            message.AddSubstitution(replacementTag, new List<string> { value });
        }

        #endregion
    }
}