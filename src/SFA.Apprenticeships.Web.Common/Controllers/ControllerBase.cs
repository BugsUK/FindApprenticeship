﻿using SFA.Apprenticeships.Web.Common.Constants;

namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Web.Mvc;
    using Attributes;
    using Mediators;
    using Providers;
    using Services;
    using Configuration;
    using StructureMap.Attributes;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Logging;
    using NLog.Contrib; // TODO: Inject logging context setter implementation rather than using directly (but use separate interface from ILogService)
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    [AuthenticateUser]
    public abstract class ControllerBase<TContextType> : ControllerBase, IUserController<TContextType> where TContextType : UserContext
    {
        public TContextType UserContext { get; protected set; }

        protected ControllerBase(IConfigurationService configurationService, ILogService logService) : base(configurationService, logService)
        {
        }
    }

    public abstract class ControllerBase : Controller
    {
        [SetterProperty]
        public IUserDataProvider UserData { get; set; }

        [SetterProperty]
        public IAuthenticationTicketService AuthenticationTicketService { get; set; }

        protected readonly ILogService _logService;
        protected readonly IConfigurationService _configurationService;

        protected ControllerBase(IConfigurationService configurationService, ILogService loggingService)
        {
            _configurationService = configurationService;
            _logService = loggingService;
        }

        protected void SetAbout()
        {
            var webConfiguration = _configurationService.Get<CommonWebConfiguration>();
            ViewBag.ShowAbout = webConfiguration.ShowAbout;
            ViewBag.Version = VersionLogging.GetVersion();
            ViewBag.Environment = webConfiguration.Environment;
        }

        protected void SetUserMessage(MediatorResponseMessage message)
        {
            if (message != null)
            {
                SetUserMessage(message.Text, message.Level);
            }
        }

        protected void SetUserMessage(string message, UserMessageLevel level = UserMessageLevel.Success)
        {
            switch (level)
            {
                case UserMessageLevel.Info:
                    UserData.Push(UserMessageConstants.InfoMessage, message);
                    break;
                case UserMessageLevel.Success:
                    UserData.Push(UserMessageConstants.SuccessMessage, message);
                    break;
                case UserMessageLevel.Warning:
                    UserData.Push(UserMessageConstants.WarningMessage, message);
                    break;
                case UserMessageLevel.Error:
                    UserData.Push(UserMessageConstants.ErrorMessage, message);
                    break;
            }
        }

        protected void SetPersistentLoggingInfo()
        {
            var sessionId = UserData.Get(UserDataItemNames.LoggingSessionId);
            if (sessionId == null)
            {
                sessionId = Guid.NewGuid().ToString("N");
                UserData.Push(UserDataItemNames.LoggingSessionId, sessionId);
            }
            MappedDiagnosticsLogicalContext.Set("sessionId", sessionId);

            // requestGuid is for tying together togging within an individual http request ONLY
            // Particularly useful for tying errors together with the original request details
            MappedDiagnosticsLogicalContext.Set("requestGuid", Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Log that "OnActionExecuting" has been called.
        /// Should be called early on in "OnActionExecuted" AFTER application-specific logging has been set up
        /// </summary>
        /// <param name="filterContext"></param>
        protected void LogOnActionExecuting(ActionExecutingContext filterContext)
        {
            SetOneOffLoggingInfo();

            // Kibana truncates the message even where there is plenty of space, so try and get as much useful info in at the beginning
            // Full URL is logged in the headers field.
            _logService.Info("{0} {1}", filterContext.HttpContext.Request.HttpMethod, Abbreviate(filterContext.HttpContext.Request.Url));

            ClearOneOffLoggingInfo();
        }

        private void SetOneOffLoggingInfo()
        {
            SetLoggingInfo("UserLanguages", () => Request.UserLanguages == null ? "<unknown>" : string.Join(",", Request.UserLanguages));
            SetLoggingInfo("CurrentCulture", () => CultureInfo.CurrentCulture.ToString());
            SetLoggingInfo("CurrentUICulture", () => CultureInfo.CurrentUICulture.ToString());

            SetLoggingInfo("Headers", () =>
            {
                var lines = new List<string>();
                lines.Add(Request.HttpMethod + " " + Request.RawUrl);
                lines.Add("Remote Address: " + Request.UserHostAddress);
                lines.Add("");

                return string.Join("\n", lines.Concat(Request.Headers.AllKeys.Select(key => string.Format("{0}: {1}", key, Request.Headers[key]))));
            });
        }

        private void ClearOneOffLoggingInfo()
        {
            // Because the space taken up by logs is critical, there should be a Grok script to remove
            // UserLanguages, CurrentCulture, CurrentUICulture and Headers from INFO, DEBUG and TRACE
            // logs when it sees this log value. These items can be still seen by looking at the first
            // INFO message for the requestGuid.
            SetLoggingInfo("headersLogged", () => "1");

            // If the above cannot be done then the following should be considered. The only bad effect
            // is that another step is required to see headers (etc) for ERROR or FATAL level logs.
            /*
            RemoveLoggingInfo("UserLanguages");
            RemoveLoggingInfo("CurrentCulture");
            RemoveLoggingInfo("CurrentUICulture");
            RemoveLoggingInfo("Headers");
            */
        }

        /// <summary>
        /// Safely set logging info with no fear of an exception.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getValue"></param>
        /// <param name="reportError"></param>
        protected void SetLoggingInfo(string key, Func<string> getValue, bool reportError = true)
        {
            try
            {
                MappedDiagnosticsLogicalContext.Set(key, getValue());
            }
            catch (Exception ex)
            {
                if (reportError)
                {
                    try
                    {
                        _logService.Warn(string.Format("Error setting {0} logging info", key), ex);
                    }
                    catch (Exception)
                    {
                        // Intentionally taking no action
                    }

                    try
                    {
                        MappedDiagnosticsLogicalContext.Set(key, ex.Message);
                    }
                    catch (Exception)
                    {
                        // Intentionally taking no action
                    }
                }
                else
                {
                    // Intentionally taking no action
                    // TODO: _logService.Trace("Error setting {key} logging info");
                }
            }
        }

        /// <summary>
        /// Safely remove logging info with no fear of an exception
        /// </summary>
        /// <param name="key"></param>
        protected void RemoveLoggingInfo(string key)
        {
            try
            {
                // The behavior when attempting to remove a non-existent value is not documented and therefore is undefined / may change
                MappedDiagnosticsLogicalContext.Remove(key);
            }
            catch (Exception)
            {
                // Intentionally taking no action
            }
        }

        private string Abbreviate(Uri uri)
        {
            if (uri == null)
                return null;

            return uri.PathAndQuery;
        }

    }
}
