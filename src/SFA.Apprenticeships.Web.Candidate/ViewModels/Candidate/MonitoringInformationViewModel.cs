namespace SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;

    public class MonitoringInformationViewModel
    {
        public MonitoringInformationViewOptions Options { get; set; }

        public int Ethnicity { get; set; }

        public SelectList EthnicitiesSelectList
        {
            get
            {
                const string a = "White";
                const string b = "Mixed/Multiple ethnic groups";
                const string c = "Asian/Asian British";
                const string d = "Black/African/Caribbean/Black British";
                const string e = "Other ethnic group";

                var list = new List<EthnicityViewModel>
                {
                    // new EthnicityViewModel {Name = "-- Please select --", Value = 0, Group = string.Empty},
                    // new EthnicityViewModel {Name = "Prefer not to say", Value = -1, Group = string.Empty},

                    new EthnicityViewModel {Name = "English/Welsh/Scottish/Northern Irish/British", Value = 31, Group = a},
                    new EthnicityViewModel {Name = "Irish", Value = 32, Group = a},
                    new EthnicityViewModel {Name = "Gypsy or Irish Traveller", Value = 33, Group = a},
                    new EthnicityViewModel {Name = "Any Other White background", Value = 34, Group = a},
                                            
                    new EthnicityViewModel {Name = "White and Black Caribbean", Value = 35, Group = b},
                    new EthnicityViewModel {Name = "White and Black African", Value = 36, Group = b},
                    new EthnicityViewModel {Name = "White and Asian", Value = 37, Group = b},
                    new EthnicityViewModel {Name = "Other mixed/multiple ethnic background", Value = 28, Group = b},
                                            
                    new EthnicityViewModel {Name = "Bangladeshi", Value = 41, Group = c},
                    new EthnicityViewModel {Name = "Indian", Value = 39, Group = c},
                    new EthnicityViewModel {Name = "Pakistani", Value = 40, Group = c},
                    new EthnicityViewModel {Name = "Any other Asian background", Value = 43, Group = c},
                                            
                    new EthnicityViewModel {Name = "African", Value = 44, Group = d},
                    new EthnicityViewModel {Name = "Caribbean", Value = 45, Group = d},
                    new EthnicityViewModel {Name = "Any other Black / African / Caribbean background", Value = 46, Group = d},
                                            
                    new EthnicityViewModel {Name = "African", Value = 44, Group = d},
                    new EthnicityViewModel {Name = "Caribbean", Value = 45, Group = d},
                    new EthnicityViewModel {Name = "Any other Black / African / Caribbean background", Value = 46, Group = d},
                                            
                    new EthnicityViewModel {Name = "Arab", Value = 47, Group = e},
                    new EthnicityViewModel {Name = "Any other ethnic group", Value = 98, Group = e},
                };

                return new SelectList(list, "Value", "Name", "Group");
            }
        }

        public bool RequiresSupportForInterview { get; set; }

        [Display(Name = MonitoringInformationViewModelMessages.AnythingWeCanDoToSupportYourInterviewMessages.HintText, Description = "")]
        public string AnythingWeCanDoToSupportYourInterview { get; set; }
    }
}