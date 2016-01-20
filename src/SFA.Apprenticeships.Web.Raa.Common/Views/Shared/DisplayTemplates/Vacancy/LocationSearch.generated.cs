﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates.Vacancy
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    
    #line 2 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Raa.Common;
    
    #line 3 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Vacancy/LocationSearch.cshtml")]
    public partial class LocationSearch : System.Web.Mvc.WebViewPage<LocationSearchViewModel>
    {
        public LocationSearch()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 6 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Add locations";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n    Vacancy location(s)\r\n</h1>\r\n\r\n");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.Ukprn));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 15 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.Ern));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 16 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.ProviderSiteErn));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.VacancyGuid));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 18 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.ComeFromPreview));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 19 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 20 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.IsEmployerLocationMainApprenticeshipLocation));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 21 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.HiddenFor(m => m.CurrentPage));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 23 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<noscript>\r\n    <div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

            
            #line 28 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
            
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
              
                var className = "form-group";
                var addAnotherLocationStatus = "";
                if (ViewData.ModelState.Keys.Contains("PostcodeSearch"))
                {
                    className += " input-validation-error";
                    addAnotherLocationStatus = "open";
                }
            
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
            
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
             if (Model.Addresses == null || !Model.Addresses.Any())
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1236), Tuple.Create("\"", 1254)
            
            #line 40 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
, Tuple.Create(Tuple.Create("", 1244), Tuple.Create<System.Object, System.Int32>(className
            
            #line default
            #line hidden
, 1244), false)
);

WriteLiteral(">\r\n                    <a");

WriteLiteral(" name=\"postcodesearch\"");

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"bold-small form-label\"");

WriteLiteral(" for=\"PostcodeSearch\"");

WriteLiteral(">Enter the vacancy postcode</label>\r\n                    <input");

WriteLiteral(" class=\"form-control-medium form-control\"");

WriteLiteral(" id=\"PostcodeSearch\"");

WriteLiteral(" name=\"PostcodeSearch\"");

WriteLiteral(" size=\"40\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" value=\"\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 44 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
               Write(Html.ValidationMessage("PostcodeSearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

WriteLiteral("                <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"AddLocations\"");

WriteLiteral(" value=\"SearchLocations\"");

WriteLiteral(">Find address</button>\r\n");

            
            #line 47 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
            }
            else
            {

            
            #line default
            #line hidden
WriteLiteral("                <details ");

            
            #line 50 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                    Write(addAnotherLocationStatus);

            
            #line default
            #line hidden
WriteLiteral(">\r\n                    <summary>Add another location</summary>\r\n                 " +
"   <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1959), Tuple.Create("\"", 1977)
            
            #line 52 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
, Tuple.Create(Tuple.Create("", 1967), Tuple.Create<System.Object, System.Int32>(className
            
            #line default
            #line hidden
, 1967), false)
);

WriteLiteral(">\r\n                        <label");

WriteLiteral(" class=\"bold-small form-label\"");

WriteLiteral(" for=\"PostcodeSearch\"");

WriteLiteral(">Enter the vacancy postcode</label>\r\n                        <input");

WriteLiteral(" class=\"form-control-medium form-control\"");

WriteLiteral(" id=\"PostcodeSearch\"");

WriteLiteral(" name=\"PostcodeSearch\"");

WriteLiteral(" size=\"40\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" value=\"\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 55 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                   Write(Html.ValidationMessage("PostcodeSearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"AddLocations\"");

WriteLiteral(" value=\"SearchLocations\"");

WriteLiteral(">Find address</button>\r\n                </details>\r\n");

            
            #line 59 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n\r\n");

            
            #line 62 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
        
            
            #line default
            #line hidden
            
            #line 62 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
         if (Model.SearchResultAddresses != null && Model.SearchResultAddresses.Page != null && Model.SearchResultAddresses.Page.Any())
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"form-group grid grid-1-2 scrolling-panel max-height-15\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"tabbed-element tab1 active width-all-1-1\"");

WriteLiteral(">\r\n                    <table");

WriteLiteral(" class=\"no-btm-margin\"");

WriteLiteral(">\r\n                        <colgroup>\r\n                            <col");

WriteLiteral(" class=\"t70\"");

WriteLiteral(">\r\n                            <col");

WriteLiteral(" class=\"t30\"");

WriteLiteral(">\r\n                        </colgroup>\r\n                        <tbody>\r\n");

            
            #line 72 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 72 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                          
                            var index = 0;
                        
            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 75 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 75 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                         foreach (var address in Model.SearchResultAddresses.Page)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <tr>\r\n                                <td");

WriteLiteral(" class=\"location-search-results\"");

WriteLiteral(">\r\n");

WriteLiteral("                                    ");

            
            #line 79 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(address.Address.AddressLine1);

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 80 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                     if (!string.IsNullOrWhiteSpace(@address.Address.AddressLine2))
                                    {
                                        string.Format(", {0}", @address.Address.AddressLine2);
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                    ");

            
            #line 84 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(string.Format(", {0} {1}", address.Address.AddressLine4, address.Address.Postcode));

            
            #line default
            #line hidden
WriteLiteral("\r\n                                </td>\r\n                                <td");

WriteLiteral(" class=\"location-search-results\"");

WriteLiteral(">\r\n");

            
            #line 87 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 87 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                      
                                        var buttonName = "UseLocation-" + @index + "-" + @Model.PostcodeSearch;
                                    
            
            #line default
            #line hidden
WriteLiteral("\r\n                                    <button");

WriteLiteral(" id=\"useLocation\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link use-location-button-link add-location-link\"");

WriteLiteral(" name=\"AddLocations\"");

WriteAttribute("value", Tuple.Create(" value=\"", 4314), Tuple.Create("\"", 4333)
            
            #line 90 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                                                                     , Tuple.Create(Tuple.Create("", 4322), Tuple.Create<System.Object, System.Int32>(buttonName
            
            #line default
            #line hidden
, 4322), false)
);

WriteLiteral(">add location</button>\r\n                                </td>\r\n                  " +
"          </tr>\r\n");

            
            #line 93 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                            index = index+1;
                        }

            
            #line default
            #line hidden
WriteLiteral("                        </tbody>\r\n                    </table>\r\n                <" +
"/div>\r\n            </div>\r\n");

            
            #line 99 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 101 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
        
            
            #line default
            #line hidden
            
            #line 101 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
         if (Model.Addresses != null && Model.Addresses.Any())
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" name=\"locationaddresses\"");

WriteLiteral("></a>\r\n                <div");

WriteLiteral(" class=\"tabbed-element tab1 active width-all-1-1\"");

WriteLiteral(">\r\n                    <table");

WriteLiteral(" class=\"no-btm-margin grid-3-4\"");

WriteLiteral(">\r\n                        <colgroup>\r\n                            <col");

WriteLiteral(" class=\"t50\"");

WriteLiteral(">\r\n                            <col");

WriteLiteral(" class=\"t30\"");

WriteLiteral(">\r\n                            <col");

WriteLiteral(" class=\"t20\"");

WriteLiteral(@">
                        </colgroup>
                        <thead>
                        <tr>
                            <th>Address</th>
                            <th>Number of positions</th>
                            <th></th>
                        </tr>
                        </thead>
                        <tbody>
");

            
            #line 120 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 120 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                         for (var i = 0; i < Model.Addresses.Count; i++)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <tr>\r\n                                <td>\r\n");

WriteLiteral("                                    ");

            
            #line 124 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Model.Addresses.ToList()[i].Address.AddressLine1);

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 125 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 125 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                     if (!string.IsNullOrWhiteSpace(@Model.Addresses.ToList()[i].Address.AddressLine2))
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <br/>");

            
            #line 127 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                             
            
            #line default
            #line hidden
            
            #line 127 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                        Write(Model.Addresses.ToList()[i].Address.AddressLine2);

            
            #line default
            #line hidden
            
            #line 127 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                                                                              
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                    <br/>");

            
            #line 129 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                    Write(Model.Addresses.ToList()[i].Address.AddressLine4);

            
            #line default
            #line hidden
WriteLiteral(" ");

            
            #line 129 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                                                                      Write(Model.Addresses.ToList()[i].Address.Postcode);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("                                    ");

            
            #line 131 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.HiddenFor(m => Model.Addresses.ToList()[i].Address.Postcode, new {@id = "addresses_" + i + "address__Postcode", Name = "Addresses[" + i + "].Address.Postcode"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                    ");

            
            #line 132 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.HiddenFor(m => Model.Addresses.ToList()[i].Address.AddressLine1, new {@id = "addresses_" + i + "address__AddressLine1", Name = "Addresses[" + i + "].Address.AddressLine1"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                    ");

            
            #line 133 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.HiddenFor(m => Model.Addresses.ToList()[i].Address.AddressLine2, new {@id = "addresses_" + i + "address__AddressLine2", Name = "Addresses[" + i + "].Address.AddressLine2"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                    ");

            
            #line 134 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.HiddenFor(m => Model.Addresses.ToList()[i].Address.AddressLine3, new {@id = "addresses_" + i + "address__AddressLine3", Name = "Addresses[" + i + "].Address.AddressLine3"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                    ");

            
            #line 135 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.HiddenFor(m => Model.Addresses.ToList()[i].Address.AddressLine4, new {@id = "addresses_" + i + "address__AddressLine4", Name = "Addresses[" + i + "].Address.AddressLine4"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                    ");

            
            #line 136 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.HiddenFor(m => Model.Addresses.ToList()[i].Address.Uprn, new {@id = "addresses_" + i + "address__Uprn", Name = "Addresses[" + i + "].Address.Uprn"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                                </td>\r\n                                <td>\r\n");

WriteLiteral("                                    ");

            
            #line 139 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.TextBoxFor(m => Model.Addresses.ToList()[i].NumberOfPositions, new {@class = "form-control-small", @maxlength = "5", @id = "addresses_" + i + "__numberofpositions", Name = "Addresses[" + i + "].NumberOfPositions"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                    ");

            
            #line 140 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                               Write(Html.ValidationMessage("Addresses[" + i + "].NumberOfPositions"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                                </td>\r\n                                <td");

WriteLiteral(" class=\"ta-center\"");

WriteLiteral(">\r\n");

            
            #line 143 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 143 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                      
                                        var buttonName = "RemoveLocation-" + i;
                                    
            
            #line default
            #line hidden
WriteLiteral("\r\n                                    <button");

WriteLiteral(" id=\"removeLocation\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link remove-button-link\"");

WriteLiteral(" name=\"AddLocations\"");

WriteAttribute("value", Tuple.Create(" value=\"", 8223), Tuple.Create("\"", 8242)
            
            #line 146 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                                                , Tuple.Create(Tuple.Create("", 8231), Tuple.Create<System.Object, System.Int32>(buttonName
            
            #line default
            #line hidden
, 8231), false)
);

WriteLiteral(">remove location</button>\r\n                                </td>\r\n               " +
"             </tr>\r\n");

            
            #line 149 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        </tbody>\r\n                    </table>\r\n                <" +
"/div>\r\n            </div>\r\n");

            
            #line 154 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"

            
            
            #line default
            #line hidden
            
            #line 155 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
       Write(Html.EditorFor(m => m.LocationAddressesComment, "Comment", Html.GetLabelFor(m => m.LocationAddressesComment)));

            
            #line default
            #line hidden
            
            #line 155 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
                                                                                                                          


            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 158 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
           Write(Html.FormTextAreaFor(m => m.AdditionalLocationInformation, controlHtmlAttributes: new {@class = "width-all-1-1", type = "text", size = 12, style = "height: 200px;"}, labelHtmlAttributes: new {@class = "bold-small"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 159 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
           Write(Html.EditorFor(m => m.AdditionalLocationInformationComment, "Comment", Html.GetLabelFor(m => m.AdditionalLocationInformationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n");

            
            #line 161 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</noscript>\r\n\r\n<div");

WriteLiteral(" class=\"hide-nojs\"");

WriteLiteral(" id=\"locationAddressesTable\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" id=\"address-lookup\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(" id=\"addressInputWrapper\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"postcode-search\"");

WriteLiteral(" class=\"form-label bold-small\"");

WriteLiteral(">Enter the vacancy location or postcode</label>\r\n            <input");

WriteLiteral(" id=\"postcode-search\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control form-control-1-3\"");

WriteLiteral(" spellcheck=\"false\"");

WriteLiteral(" autocorrect=\"off\"");

WriteLiteral(" data-bind=\"style: {\'display\': addModeOn() }\"");

WriteLiteral("/>\r\n            <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" id=\"add-new-location\"");

WriteLiteral(" data-bind=\"style: {\'display\': addModeOff() }, click: addNewLocation\"");

WriteLiteral(">Add another location </a>\r\n            <span");

WriteLiteral(" class=\"visuallyhidden\"");

WriteLiteral(" id=\"ariaAddressEntered\"");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral("></span>\r\n        </div>\r\n        <div");

WriteLiteral(" id=\"address-manual\"");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"copy-19\"");

WriteLiteral(" id=\"enterAddressManually\"");

WriteLiteral("></a><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"copy-19\"");

WriteLiteral(" id=\"enterAddressManually\"");

WriteLiteral("></a>\r\n            <span");

WriteLiteral(" class=\"toggle-content hide-nojs loading-text\"");

WriteLiteral(" id=\"addressLoading\"");

WriteLiteral(">Loading address...</span>\r\n            <span");

WriteLiteral(" class=\"toggle-content hide-nojs loading-text\"");

WriteLiteral(" id=\"noResults\"");

WriteLiteral(">No results match your search. You must enter a valid postcode</span>\r\n          " +
"  <span");

WriteLiteral(" style=\"color: red\"");

WriteLiteral(" id=\"postcodeServiceUnavailable\"");

WriteLiteral(" class=\"copy-19 toggle-content hide-nojs text\"");

WriteLiteral(">Service is currently unavailable, enter the full postcode</span>\r\n        </div>" +
"\r\n    </div>\r\n    <div>\r\n        <div");

WriteLiteral(" data-bind=\"style: {\'display\': locationAddressesStatus() }\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" name=\"locationaddresses\"");

WriteLiteral("></a>\r\n            <table");

WriteLiteral(" class=\"grid-3-4\"");

WriteLiteral(">\r\n                <colgroup>\r\n                    <col");

WriteLiteral(" class=\"t40\"");

WriteLiteral(">\r\n                    <col");

WriteLiteral(" class=\"t25\"");

WriteLiteral(">\r\n                    <col");

WriteLiteral(" class=\"t20\"");

WriteLiteral(">\r\n                    <col>\r\n                </colgroup>\r\n\r\n                <the" +
"ad>\r\n                <tr>\r\n                    <th>\r\n                        <sp" +
"an");

WriteLiteral(" class=\"heading-span\"");

WriteLiteral(">Location</span>\r\n                    </th>\r\n                    <th>\r\n          " +
"              <span");

WriteLiteral(" class=\"heading-span\"");

WriteLiteral(">Number of positions</span>\r\n                    </th>\r\n                    <th>\r" +
"\n                    </th>\r\n                </tr>\r\n                </thead>\r\n   " +
"             <tbody");

WriteLiteral(" id=\"location-addresses\"");

WriteLiteral(" data-bind=\"foreach: locationAddresses\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td>\r\n                        <span");

WriteLiteral(" data-bind=\"html: itemFriendlyAddress()\"");

WriteLiteral(" maxlength=\"50\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td>\r\n                       " +
" <span");

WriteLiteral(" style=\"width: 20%; display: inline-block\"");

WriteLiteral("></span>\r\n                        <input");

WriteLiteral(" class=\"form-control-small qual-input-edit qual-year\"");

WriteLiteral(" maxlength=\"5\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: itemNumberOfPositions, attr:{\'name\':\'Addresses[\' + $index() +\'" +
"].NumberOfPositions\', \'id\':\'addresses_\'+ $index() + \'__numberofpositions\'}\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" class=\"field-validation-valid\"");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(" data-valmsg-replace=\"true\"");

WriteLiteral(" data-bind=\"attr:{\'id\': \'Addresses[\' + $index() +\'].NumberOfPositions_Error\', \'da" +
"ta-valmsg-for\': \'Addresses[\' + $index() +\'].NumberOfPositions\'}\"");

WriteLiteral("></span>\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-bind=\"value: itemAddressLine1, attr:{\'name\':\'Addresses[\' + $index() +\'].Add" +
"ress.AddressLine1\', \'id\':\'addresses_\'+ $index() + \'address__AddressLine1\' }\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-bind=\"value: itemAddressLine2, attr:{\'name\':\'Addresses[\' + $index() +\'].Add" +
"ress.AddressLine2\', \'id\':\'addresses_\'+ $index() + \'address__AddressLine2\' }\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-bind=\"value: itemAddressLine3, attr:{\'name\':\'Addresses[\' + $index() +\'].Add" +
"ress.AddressLine3\', \'id\':\'addresses_\'+ $index() + \'address__AddressLine3\' }\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-bind=\"value: itemAddressLine4, attr:{\'name\':\'Addresses[\' + $index() +\'].Add" +
"ress.AddressLine4\', \'id\':\'addresses_\'+ $index() + \'address__AddressLine4\' }\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-bind=\"value: itemPostcode, attr:{\'name\':\'Addresses[\' + $index() +\'].Address" +
".Postcode\', \'id\':\'addresses_\'+ $index() + \'address__Postcode\' }\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-bind=\"value: itemUprn, attr:{\'name\':\'Addresses[\' + $index() +\'].Address.Upr" +
"n\', \'id\':\'addresses_\'+ $index() + \'address__Uprn\' }\"");

WriteLiteral(">\r\n                    </td>\r\n                    <td");

WriteLiteral(" class=\"ta-center\"");

WriteLiteral(">\r\n                        <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click: $root.removeLocationAddress\"");

WriteLiteral(">remove location</a>\r\n                    </td>\r\n                </tr>\r\n         " +
"       </tbody>\r\n            </table>\r\n");

WriteLiteral("            ");

            
            #line 225 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
       Write(Html.EditorFor(m => m.LocationAddressesComment, "Comment", Html.GetLabelFor(m => m.LocationAddressesComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"style: {\'display\': locationAddressesStatus() }\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 229 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
       Write(Html.FormTextAreaFor(m => m.AdditionalLocationInformation, controlHtmlAttributes: new { @class = "width-all-1-1", type = "text", size = 12, style = "height: 200px;" }, labelHtmlAttributes: new { @class = "bold-small" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 230 "..\..\Views\Shared\DisplayTemplates\Vacancy\LocationSearch.cshtml"
       Write(Html.EditorFor(m => m.AdditionalLocationInformationComment, "Comment", Html.GetLabelFor(m => m.AdditionalLocationInformationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
