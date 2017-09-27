using NUnit.Framework;
using OpenQA.Selenium;
using Pupil.Components;
using Pupil.Components.Common;
using SharedComponents.Helpers;
using System.Linq;
using System;

namespace Pupil.ClassLog.Tests
{
    public class DisabledTests
    {
        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ClassLog.IndicatorTests, PupilTestGroups.Priority.Priority4 })]
        public void CanHideIndicators()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, false);

            var classLogNavigate = new ClassLogNavigation();

            classLogNavigate.NavigateToPupilClassLogFromQuickLink();

            var galleryList = SeleniumHelper.Get(By.CssSelector(".gallery-list"));

            Assert.IsFalse(galleryList.FindElements(By.ClassName("gallery-tile-actions-indicators")).First().Displayed);
        }

        //[WebDriverTest(Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { PupilTestGroups.ClassLog.IndicatorTests, PupilTestGroups.Priority.Priority4 })]
        public void CanShowIndicators()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, false);

            var classLogNavigate = new ClassLogNavigation();

            classLogNavigate.NavigateToPupilClassLogFromQuickLink();

            SeleniumHelper.FindAndClick(By.CssSelector(String.Format(ClassLogElements.Detail.IndicatorDropDownList)));
            SeleniumHelper.FindAndClick(By.CssSelector(String.Format(ClassLogElements.Detail.ShowIndicator)));

            var galleryList = SeleniumHelper.Get(By.CssSelector(".gallery-list"));

            Assert.IsTrue(galleryList.FindElements(By.ClassName("gallery-tile-actions-indicators")).First().Displayed);
        }
    }
}