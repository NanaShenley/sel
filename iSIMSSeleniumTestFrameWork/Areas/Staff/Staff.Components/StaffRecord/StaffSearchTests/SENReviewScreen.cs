using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using WebDriverRunner.webdriver;
using SharedComponents.Helpers;
using System.IO;
using Staff.Components.StaffRecord.Enumerations;

namespace Staff.Components.StaffRecord
{
    public class SENReviewScreen : StaffSearchBase
    {
        #region Const
        private const string _pupilSearchButton = "[data-automation-id=\"search_criteria_submit\"]";
        private const string _pupil = "[data-automation-id=\"resultTile\"]";
        private const string _addSenReviewButton = "[data-automation-id=\"add_sen_review_button\"]";
        private const string _addPeopleButton = "[data-automation-id=\"add_button\"]";
        #endregion

        public static readonly By AddPeopleInvolvedButton = By.CssSelector("[data-automation-id=\"add_button\"]");
        public static readonly By SelectPeopleButton = By.CssSelector("[data-automation-id=\"select_button\"]");
        
        #region Actions      

        public void ClickPupilSearchButton()
        {
            By loc = By.CssSelector(_pupilSearchButton);
            WaitForAndClick(new TimeSpan(0, 0, 10), loc);
        }

        public void ClickAddSenReviewButton()
        {
            By loc = By.CssSelector(_addSenReviewButton);
            IWebElement pupilMaintenance = WaitForAndGet(new TimeSpan(0, 0, 10), loc);
            By addPeopleButton = By.CssSelector(_addPeopleButton);
            pupilMaintenance.ClickAndWaitFor(addPeopleButton);
        }

        public void GetAndClickFirstPupil()
        {
            By loc = By.CssSelector(_pupil);
            WaitForAndClick(new TimeSpan(0, 0, 10), loc);

        }
        
        #endregion
    }
}
