namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using Common.Constants;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class UserData
    {
        private IDictionary<string, Stack<string>> _data = new Dictionary<string, Stack<string>>();

        public void Push(string key, string value)
        {
            Stack<string> stack;
            if (!_data.TryGetValue(key, out stack))
            {
                stack = new Stack<string>();
                _data[key] = stack;
            }
            stack.Push(value);
        }

        public string Get(string key)
        {
            Stack<string> stack;
            if (_data.TryGetValue(key, out stack) && stack.Any())
                return stack.Peek();
            else
                return null;
        }

        public string Pop(string key)
        {
            Stack<string> stack;
            if (_data.TryGetValue(key, out stack) && stack.Any())
                return stack.Pop();
            else
                return null;
        }
    }
    //[OutputCache(CacheProfile = "None")]
    public abstract class ControllerBase : Controller
    {
        public UserData UserData = new UserData();

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
    }
}
