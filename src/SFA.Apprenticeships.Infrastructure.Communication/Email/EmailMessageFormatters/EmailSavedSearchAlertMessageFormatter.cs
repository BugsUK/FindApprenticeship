namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Xsl;
    using Application.Interfaces.Communications;
    using Domain.Entities.Communication;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Interfaces.Configuration;
    using Newtonsoft.Json;
    using SendGrid;

    public class EmailSavedSearchAlertMessageFormatter : EmailMessageFormatter
    {
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

            // TODO: AG: US638: remove this hack.
            html = html.Replace("&amp;", "&");

            // TODO: AG: US638: remove debugging aid.
            Console.WriteLine(html);
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
                    url = FormatSearchUrl(savedSearchAlert),
                    searchMode = GetSearchModeName(savedSearchAlert.Parameters.SearchMode),
                    keywords = savedSearchAlert.Parameters.Keywords,
                    category = savedSearchAlert.Parameters.Category,
                    subCategories = FormatSubCategories(savedSearchAlert),
                    withinDistance = savedSearchAlert.Parameters.WithinDistance,
                    location = savedSearchAlert.Parameters.Location,
                    apprenticeshipLevel = savedSearchAlert.Parameters.ApprenticeshipLevel,
                },
                results = savedSearchAlert.Results.Select(result => new
                {
                    url = FormatVacancyDetailsUrl(result),
                    title = result.Title,
                    employerName = result.EmployerName,
                    description = result.Description,
                    closingDate = FormatDate(result.ClosingDate)
                })
            });
        }

        private string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("d MMM yyyy");
        }

        private string FormatSubCategories(SavedSearchAlert savedSearchAlert)
        {
            if (savedSearchAlert.Parameters.SubCategories == null ||
                savedSearchAlert.Parameters.SubCategories.Length == 0)
            {
                return string.Empty;
            }

            return string.Join(", ", savedSearchAlert.Parameters.SubCategories);
        }

        private string GetSearchModeName(ApprenticeshipSearchMode searchMode)
        {
            return searchMode == ApprenticeshipSearchMode.Keyword ? "keyword" : "category";
        }

        private string FormatSearchUrl(SavedSearchAlert savedSearchAlert)
        {

            switch (savedSearchAlert.Parameters.SearchMode)
            {
                case ApprenticeshipSearchMode.Category:
                    return FormatCategorySearchUrl(savedSearchAlert);

                case ApprenticeshipSearchMode.Keyword:
                    return FormatKeywordSearchUrl(savedSearchAlert);

                default:
                    return _siteDomainName;
            }
        }

        private string FormatKeywordSearchUrl(SavedSearchAlert savedSearchAlert)
        {
            const string locationType = "NonNational";
            const string searchField = "All";
            const string searchMode = "Search";
            const string sortType = "RecentlyAdded";
            const string searchAction = "Search";

            const int hash = 0; // TODO: AG: US638: compute / get location hash
            const int pageNumber = 1;
            const int resultsPerPage = 5;

            var format = new[]
            {
                "https://{0}/apprenticeships?",
                "Keywords={1}&",
                "Location={2}&",
                "LocationType={3}&",
                "ApprenticeshipLevel={4}&",
                "SearchField={5}&",
                "SearchMode={6}&",
                "Hash={7}&",
                "WithinDistance={8}&",
                "SortType={9}&",
                "PageNumber={10}&",
                "SearchAction={11}&",
                "ResultsPerPage={12}"  
            };

            return string.Format(string.Join(string.Empty, format),
                _siteDomainName,
                savedSearchAlert.Parameters.Keywords,
                savedSearchAlert.Parameters.Location,
                locationType,
                savedSearchAlert.Parameters.ApprenticeshipLevel,
                searchField,
                searchMode,
                hash,
                savedSearchAlert.Parameters.WithinDistance,
                sortType,
                pageNumber,
                searchAction,
                resultsPerPage);
        }

        private string FormatCategorySearchUrl(SavedSearchAlert savedSearchAlert)
        {
            const string searchAction = "Search";
            const string searchMode = "Category";
            const string locationType = "NonNational";
            const string sortType = "RecentlyAdded";

            const int hash = 0; // TODO: AG: US638: compute / get location hash
            const int pageNumber = 1;
            const int resultsPerPage = 5;

            var format = new[]
            {
                "https://{0}/apprenticeships?",
                "Category={1}&",
                "{2}&",
                "Location={3}&",
                "WithinDistance={4}&",
                "ApprenticeshipLevel={5}&",
                "Hash={6}&",
                "SearchMode={7}&",
                "LocationType={8}&",
                "sortType={9}&",
                "SearchAction={10}&",
                "PageNumber={11}&",
                "resultsPerPage={12}"
            };

            return string.Format(string.Join(string.Empty, format),
                _siteDomainName,
                savedSearchAlert.Parameters.Category,
                FormatCategorySearchUrlSubCategories(savedSearchAlert),
                savedSearchAlert.Parameters.Location,
                savedSearchAlert.Parameters.WithinDistance,
                savedSearchAlert.Parameters.ApprenticeshipLevel,
                hash,
                searchMode,
                locationType,
                sortType,
                searchAction,
                pageNumber,
                resultsPerPage);
        }

        private string FormatCategorySearchUrlSubCategories(SavedSearchAlert savedSearchAlert)
        {
            if (savedSearchAlert.Parameters.SubCategories == null ||
                savedSearchAlert.Parameters.SubCategories.Length == 0)
            {
                return string.Empty;
            }

            const string format = "SubCategories={0}&";
            var sb = new StringBuilder();

            foreach (var subCategory in savedSearchAlert.Parameters.SubCategories)
            {
                sb.AppendFormat(format, subCategory);
            }

            return sb.ToString();
        }

        private string FormatVacancyDetailsUrl(ApprenticeshipSummary apprenticeshipSummary)
        {
            const string format = "https://{0}/apprenticeship/{1}";

            return string.Format(format, _siteDomainName, apprenticeshipSummary.Id);
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
    }
}