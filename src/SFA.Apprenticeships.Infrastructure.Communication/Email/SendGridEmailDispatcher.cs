namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using EmailFromResolvers;
    using Exceptions;
    using SendGrid;
    using ErrorCodes = Application.Interfaces.Communications.ErrorCodes;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private readonly ILogService _logger;

        private readonly string _userName;
        private readonly string _password;
        private readonly EmailTemplate[] _templates;
        private readonly IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> _messageFormatters;
        private readonly IEnumerable<IEmailFromResolver> _emailFromResolvers;

        public SendGridEmailDispatcher(
            IConfigurationService configuration, 
            IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> messageFormatters, 
            ILogService logger,
            IEnumerable<IEmailFromResolver> emailFromResolvers)
        {
            _messageFormatters = messageFormatters;
            _logger = logger;
            _emailFromResolvers = emailFromResolvers;

            var config = configuration.Get<EmailConfiguration>();

            _userName = config.Username;
            _password = config.Password;
            _templates = config.Templates.ToArray();
        }

        public void SendEmail(EmailRequest request)
        {
            _logger.Debug("Dispatching email To:{0}, Template:{1}", request.ToEmail, request.MessageType);

            var message = ComposeMessage(request);
            DispatchMessage(request, message);
        }

        private SendGridMessage ComposeMessage(EmailRequest request)
        {
            var message = CreateMessage(request);

            AttachTemplate(request, message);
            PopulateTemplate(request, message);

            return message;
        }

        private static SendGridMessage  CreateMessage(EmailRequest request)
        {
            const string emptyHtml = "<span></span>";
            const string emptyText = "";
            const string subject = " ";

            // NOTE: https://github.com/sendgrid/sendgrid-csharp.
            var message = new SendGridMessage
            {
                Subject = subject,
                To = new[]
                {
                    //TODO: Support multiple to addresses
                    new MailAddress(request.ToEmail)
                },
                Text = emptyText,
                Html = emptyHtml
            };

            return message;
        }

        private void PopulateTemplate(EmailRequest request, SendGridMessage message)
        {
            // NOTE: https://sendgrid.com/docs/API_Reference/SMTP_API/substitution_tags.html.
            if (_messageFormatters.All(mf => mf.Key != request.MessageType))
            {
                var errorMessage = string.Format("Populate template: No message formatter exists for MessageType name: {0}", request.MessageType);
                _logger.Error(errorMessage);

                throw new ConfigurationErrorsException(errorMessage);
            }
            _messageFormatters.First(mf => mf.Key == request.MessageType).Value.PopulateMessage(request, message);
        }

        private void AttachTemplate(EmailRequest request, SendGridMessage message)
        {
            var templateName = GetTemplateName(request.MessageType);
            var template = GetTemplateConfiguration(templateName);
            var fromEmail = _emailFromResolvers.First(er => er.CanResolve(request.MessageType)).Resolve(request, template.FromEmail);

            message.From = new MailAddress(fromEmail);
            message.EnableTemplateEngine(template.Id.ToString());
        }

        private string GetTemplateName(Enum messageType)
        {
            var enumType = messageType.GetType();
            var templateName = string.Format("{0}.{1}", enumType.Name, Enum.GetName(enumType, messageType));

            _logger.Debug("Determined email template: EnumType={0} Name={1} TemplateName={2} MessageType={3}", enumType,
                enumType.Name, templateName, messageType);

            return templateName;
        }

        private EmailTemplate GetTemplateConfiguration(string templateName)
        {
            var template = _templates.FirstOrDefault(each => each.Name == templateName);

            if (template != null)
            {
                return template;
            }

            var errorMessage = string.Format("GetTemplateConfiguration : Invalid email template name: {0}", templateName);

            _logger.Error(errorMessage);

            throw new ConfigurationErrorsException(errorMessage);
        }

        private void DispatchMessage(EmailRequest request, SendGridMessage message)
        {
            var logMessage = GetEmailLogMessage(request, message);

            try
            {
                _logger.Debug("Dispatching email: {0}", logMessage);

                var credentials = new NetworkCredential(_userName, _password);
                var web = new Web(credentials);

                web.Deliver(message);

                _logger.Info("Dispatched email: {0}", logMessage);
            }
            catch (InvalidApiRequestException e)
            {
                var errorMessage = string.Format("Failed to dispatch email: {0}. Errors: {1}", logMessage, string.Join(", ", e.Errors));

                _logger.Error(errorMessage, e, logMessage);
                throw new CustomException(errorMessage, e, ErrorCodes.EmailApiError);
            }
            catch (Exception e)
            {
                var errorMessage = string.Format("Failed to dispatch email: {0}", logMessage);

                _logger.Error(errorMessage, e, logMessage);
                throw new CustomException(errorMessage, e, ErrorCodes.EmailError);
            }
        }

        private static string GetEmailLogMessage(EmailRequest request, SendGridMessage message)
        {
            var subject = string.IsNullOrWhiteSpace(message.Subject) ? "<from template>" : message.Subject;
            var recipients = string.Join(", ", message.To.Select(address => address.Address));
            var tokens = string.Join(", ", request.Tokens.Select(token => string.Format("'{0}'='{1}'", token.Key, token.Value)));

            return string.Format("type='{0}', subject='{1}', from='{2}', to='{3}', tokens='{4}'",
                request.MessageType, subject, message.From.Address, recipients, tokens);
        }
    }
}