using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class DidYouMeanDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='add_new_pupil_wizard']"); }
        }

        #region Properties

        public GridComponent<FormerPupil> FormerPupils
        {
            get
            {
                GridComponent<FormerPupil> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<FormerPupil>(By.CssSelector("[data-maintenance-grid-id='pupil_matches_former']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class FormerPupil
        {

            [FindsBy(How = How.CssSelector, Using = "[name$=PreferredListNameDisplay]")]
            private IWebElement _pupilName;

            [FindsBy(How = How.CssSelector, Using = "[name$=DateOfBirth]")]
            private IWebElement _dob;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'yes,_re-enrol_pupil_button']")]
            private IWebElement _reAdmit;

            public string PupilName
            {
                get { return _pupilName.GetValue(); }
            }

            public string DOB
            {
                get { return _dob.GetDateTime(); }
            }

            public RegistrationDetailDialog ClickReEnrolPupilLink()
            {
                _reAdmit.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                return new RegistrationDetailDialog();
            }
        }

        #endregion
    }
}
