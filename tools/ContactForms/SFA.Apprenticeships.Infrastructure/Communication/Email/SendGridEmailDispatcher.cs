
using SFA.Apprenticeships.Common;
using SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters;

namespace SFA.Apprenticeships.Infrastructure.Communication.Email
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Domain.Exceptions;
    using SendGrid;
    using ErrorCodes = Application.Interfaces.Communications.ErrorCodes;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private readonly ILogService _logger;

        private readonly string _password;
        private readonly SendGridTemplateConfiguration[] _templates;
        private readonly string _userName;
        private readonly IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> _messageFormatters;

        public SendGridEmailDispatcher(SendGridConfiguration configuration, IEnumerable<KeyValuePair<MessageTypes,
            EmailMessageFormatter>> messageFormatters,
            ILogService logger)
        {
            _messageFormatters = messageFormatters;
            _logger = logger;
            _userName = configuration.UserName;
            _password = configuration.Password;
            _templates = configuration.Templates.ToArray();
        }

        public void SendEmail(EmailRequest request)
        {
            try
            {
                _logger.Debug("Dispatching email To:{0}, Template:{1}", request.ToEmail, request.MessageType);

                var message = ComposeMessage(request);
                DispatchMessage(message);
            }
            catch (Exception ex)
            {
                _logger.Error("Error sending email", ex);
                throw;
            }
        }

        private SendGridMessage ComposeMessage(EmailRequest request)
        {
            var message = CreateMessage(request);

            AttachTemplate(request, message);
            PopulateTemplate(request, message);

            return message;
        }

        private static SendGridMessage CreateMessage(EmailRequest request)
        {
            const string emptyHtml = "<span></span>";
            const string emptyText = "";
            var subject = request.Subject ?? " ";

            //to support multiple email ids in to field
            MailAddress[] toEmailAddresses = ParseEmailList(request.ToEmail);

            // NOTE: https://github.com/sendgrid/sendgrid-csharp.
            var message = new SendGridMessage
            {
                Subject = subject,
                To = toEmailAddresses,
                Text = emptyText,
                Html = emptyHtml,

            };

            //Append the attachment if any
            if (!string.IsNullOrEmpty(request.StreamedAttachmentName))
            {
                message.AddAttachment(request.StreamedAttachmentName);
            }

            return message;
        }

        private static MailAddress[] ParseEmailList(string toEmail)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                throw new CustomException("ToEmailAddress key is blank");
            }

            string[] toEmailArray = toEmail.Split(Constants.EmailSeparator);
            MailAddress[] toEmailAddresses = new MailAddress[toEmailArray.Length];

            for (var i = 0; i < toEmailArray.Length; i++)
            {
                toEmailAddresses[i] = new MailAddress(toEmailArray[i]);
            }
            return toEmailAddresses;
        }

        private void PopulateTemplate(EmailRequest request, SendGridMessage message)
        {
            // NOTE: https://sendgrid.com/docs/API_Reference/SMTP_API/substitution_tags.html.
            if (!_messageFormatters.Any(mf => mf.Key == request.MessageType))
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
            var fromEmail = template.FromEmail;

            message.From = new MailAddress(fromEmail);
            message.EnableTemplateEngine(template.Id);
        }

        private string GetTemplateName(Enum messageType)
        {
            var enumType = messageType.GetType();
            var templateName = string.Format("{0}.{1}", enumType.Name, Enum.GetName(enumType, messageType));
            _logger.Debug("Determined email template: EnumType={0} Name={1} TemplateName={2} MessageType={3}", enumType,
                enumType.Name, templateName, messageType);
            return templateName;
        }

        private SendGridTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            var template = _templates.FirstOrDefault(each => each.Name == templateName);

            if (template != null)
            {
                return template;
            }

            var errorMessage = string.Format("GetTemplateConfiguration : Invalid email template name: {0}",
                templateName);
            _logger.Error(errorMessage);

            throw new ConfigurationErrorsException(errorMessage);
        }

        private void DispatchMessage(SendGridMessage message)
        {
            try
            {
                var credentials = new NetworkCredential(_userName, _password);
                var web = new Web(credentials);

                _logger.Debug("Dispatching email: {0}", LogSendGridMessage(message));
                web.Deliver(message);
                _logger.Info("Dispatched email: {0} to {1}", message.Subject,
                    string.Join(", ", message.To.Select(a => a.Address)));
            }
            catch (Exception e)
            {
                _logger.Error("Failed to dispatch email", e);
                throw new CustomException("Failed to dispatch email", e, ErrorCodes.EmailError);
            }
        }

        private static string LogSendGridMessage(SendGridMessage message)
        {
            var messageLog = string.Format("Subject: {0}", message.Subject);
            messageLog += "To: ";
            message.To.ToList().ForEach(t => messageLog += string.Format("{0}, ", t.Address));
            messageLog += string.Format("From: {0}, ", message.From.Address);

            return messageLog;
        }
    }
}