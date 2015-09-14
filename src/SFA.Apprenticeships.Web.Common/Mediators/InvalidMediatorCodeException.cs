using System;

namespace SFA.Apprenticeships.Web.Common.Mediators
{
    public class InvalidMediatorCodeException : Exception
    {
        public string Code { get; set; }

        public InvalidMediatorCodeException(string code)
        {
            Code = code;
        }

        public override string Message
        {
            get { return string.Format("Mediator returned unrecognised code: {0}", Code); }
        }
    }
}