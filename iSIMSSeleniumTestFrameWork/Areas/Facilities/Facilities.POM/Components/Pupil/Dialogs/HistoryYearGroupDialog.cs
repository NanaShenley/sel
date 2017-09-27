using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class HistoryYearGroupDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='LearnerYearGroupMemberships-grid-editor-dialog'][aria-hidden='false']"); }
        }

        #region Year Group Membership Grid

        public GridComponent<YearGroupMembership> YearGroups
        {
            get
            {
                GridComponent<YearGroupMembership> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<YearGroupMembership>(By.CssSelector("[data-maintenance-container='LearnerYearGroupMemberships']"), DialogIdentifier);

                });
                return returnValue;
            }
        }

        public class YearGroupMembership : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='YearGroup.dropdownImitator']")]
            private IWebElement _yearGroup;

            public string YearGroup
            {
                get
                {
                    return _yearGroup.GetValue();
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
