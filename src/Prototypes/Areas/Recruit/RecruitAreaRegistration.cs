using System.Web.Mvc;

namespace Prototypes.Areas.Recruit
{
    public class RecruitAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Recruit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Recruit_default",
                "Recruit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "SFA.Apprenticeships.Web.Recruit.Controllers" }
            );
        }
    }
}