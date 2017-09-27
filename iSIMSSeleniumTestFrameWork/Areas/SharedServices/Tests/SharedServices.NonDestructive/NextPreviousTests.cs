using Microsoft.VisualStudio.TestTools.UnitTesting;
using Selene.Support.Attributes;
using SharedServices.Components;
using SharedServices.Components.Common;
using TestSettings;

namespace SharedServices.NonDestructive
{
    public class NextPreviousTests
    {
        [WebDriverTest(Groups = new[] { TestGroups.NextPreviousFeature, TestGroups.P2 }, Browsers = new[] { BrowserDefaults.Ie, BrowserDefaults.Chrome })]
        public void CanNavigateBetweenRecords()
        {
            var nextPrevious = new NextPrevious();
            nextPrevious.SearchPupilAction();
            nextPrevious.NavigateToFirstPupil();
            nextPrevious.NavigateToNextPupils();

            Assert.AreEqual("search-result-tile-detail loaded", nextPrevious.GetCurrentSelectedPupilRecordsClassValue());
        }
    }
}