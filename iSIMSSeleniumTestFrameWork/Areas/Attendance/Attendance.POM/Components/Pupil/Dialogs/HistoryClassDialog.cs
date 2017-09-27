using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class HistoryClassDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='LearnerPrimaryClassMemberships-grid-editor-dialog'][aria-hidden='false']"); }
        }

        #region Class Membership Grid

        public GridComponent<ClassMembership> Classes
        {
            get
            {
                GridComponent<ClassMembership> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ClassMembership>(By.CssSelector("[data-maintenance-container='LearnerPrimaryClassMemberships']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class ClassMembership : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='PrimaryClass.dropdownImitator']")]
            private IWebElement _className;

            public string Class
            {
                get
                {
                    return _className.GetValue();
                }
            }
        }

        #endregion

        #region Page actions
        public PupilRecordPage ClickOK()
        {
            IWebElement okButton = SeleniumHelper.FindElement(SimsBy.CssSelector("[aria-hidden='false'] [data-automation-id='ok_button']"));
            okButton.ClickByJS();
            return new PupilRecordPage();
        }
        #endregion
    }
}
