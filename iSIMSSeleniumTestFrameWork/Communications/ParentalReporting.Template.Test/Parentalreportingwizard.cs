using Communications.POM.Components.Communication.Pages;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;

namespace ParentalReporting.Template.Test
{
    class Parentalreportingwizard
    {
        [WebDriverTest(TimeoutSeconds = 2000, Enabled = true, Groups = new[] { "All", "P1" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Setupmarksheet()
        {
            // Login
            String[] featureList = { "ManageParentalReports" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.AssessmentCoordinator, featureList);
            AutomationSugar.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            Wait.WaitForDocumentReady();

            SetupMarksheetandParentalreportTriplet templateObject = new SetupMarksheetandParentalreportTriplet();
            templateObject.SetupParentalReportButton();
            templateObject.CreateNewParentalReportingTemplate();
            var parentalTemplate = new ParentalReportingTypeSelectionPage();
            string TemplateName = SeleniumHelper.GenerateRandomString(10);
            parentalTemplate.FillTemplateDetails(TemplateName);
            parentalTemplate._clickNextButton();
            NewParentalReportingTemplatePage newparentalTemplate = new NewParentalReportingTemplatePage();
            newparentalTemplate._ClickAssessmentButton();
            AttendenceConductReviewPage attendencepage = new AttendenceConductReviewPage();
            attendencepage.IsActive = true;
            attendencepage.Save();
            attendencepage.saveTemplate();
        }
    }
 }

