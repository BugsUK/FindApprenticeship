﻿namespace SFA.Apprenticeships.Infrastructure.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using SendGrid;
    using Application.Interfaces.Messaging;
    using Configuration;

    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private readonly string _userName;
        private readonly string _password;

        private readonly SendGridTemplateConfiguration[] _templates;

        public SendGridEmailDispatcher(SendGridConfiguration configuration)
        {
            _userName = configuration.UserName;
            _password = configuration.Password;
            _templates = configuration.Templates.ToArray();
        }

        public void SendEmail(EmailRequest request)
        {
            var message = ComposeMessage(request);

            DispatchMessage(message);
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

            // NOTE: https://github.com/sendgrid/sendgrid-csharp.
            var message = new SendGridMessage
            {
                Subject = request.Subject,
                From = new MailAddress(request.FromEmail),
                To = new[]
                {
                    new MailAddress(request.ToEmail)
                },
                Text = emptyText,
                Html = emptyHtml
            };

            return message;
        }

        private static void PopulateTemplate(EmailRequest request, SendGridMessage message)
        {
            // NOTE: https://sendgrid.com/docs/API_Reference/SMTP_API/substitution_tags.html.
            foreach (var token in request.Tokens)
            {
                message.AddSubstitution(
                    DelimitToken(token.Key),
                    new List<string>
                    {
                        token.Value
                    });
            }
        }

        private static string DelimitToken(string key)
        {
            const string templateTokenDelimiter = "-";

            return string.Format("{0}{1}{0}", templateTokenDelimiter, key);
        }

        private void AttachTemplate(EmailRequest request, SendGridMessage message)
        {
            var template = GetTemplateConfiguration(request.TemplateName);

            message.EnableTemplateEngine(template.Id);
        }

        private SendGridTemplateConfiguration GetTemplateConfiguration(string templateName)
        {
            var template = _templates
                .FirstOrDefault(each => each.Name == templateName);

            if (template == null)
            {
                var message = string.Format("Invalid email template name: \"{0}\".", templateName);

                // TODO: AG: template is invalid, log / throw domain exception.
                throw new Exception(message);
            }

            return template;
        }

        private void DispatchMessage(SendGridMessage message)
        {
            try
            {
                var credentials = new NetworkCredential(_userName, _password);
                var web = new Web(credentials);

                web.Deliver(message);
            }
            catch (Exception e)
            {
                // TODO: AG: failed to send, log / throw domain exception.
                throw new Exception("Failed to dispatch email.", e);
            }
        }
    }
}
