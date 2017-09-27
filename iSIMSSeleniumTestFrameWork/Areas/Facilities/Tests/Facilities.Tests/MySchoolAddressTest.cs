using Facilities.Components.Common;
using Facilities.Components.Facilities_Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using Selene.Support.Attributes;
using SharedComponents.Helpers;
using System;
using TestSettings;
using WebDriverRunner.internals;

namespace Facilities.Tests
{
  
    public class MySchoolAddressTest
    {
        string addressDisplaySmall=string.Empty;
        string addressDisplayLarge=string.Empty;
        string PAONRange = "22";
        string Street = "SUDELEY WALK";
        string Town = "BEDFORD";
        string PostCode = "MK41 8HS";
        string Country = "United Kingdom";
        string NewStreet = "SUDELEY NEW WALK";
        const string _space = " ";
        const string _seperator = ", ";
        const string _lineSeperator = "\r\n";

        [WebDriverTest(TimeoutSeconds = 2400, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "Add", "School_Address" , "P2"})]
        [Variant(Variant.EnglishStatePrimary | Variant.EnglishStateSecondary | Variant.EnglishStateMultiphase |
   Variant.WelshStatePrimary | Variant.WelshStateSecondary | Variant.WelshStateMultiphase)]
        public void Add_Update_Delete_Address()
        {
            MySchoolDetailsPage schoolPage = FacilitiesNavigation.NavigatetoMySchoolDetailPageWithFeatureAddresses();
            schoolPage.ExpandSchoolAddress();
            if (schoolPage.IsAddSchoolAddress())
            {
                AddSchoolAddress(schoolPage);
                EditSchoolAddress(schoolPage);
                DeleteSchoolAddress(schoolPage);
            }
            else
            {
                DeleteSchoolAddress(schoolPage);
                AddSchoolAddress(schoolPage);
                EditSchoolAddress(schoolPage);
            }
        }

        private void AddSchoolAddress(MySchoolDetailsPage schoolPage)
        {
            //Add Address
            addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);
            var schoolName = TestDefaults.Default.SchoolName;
            addressDisplayLarge = string.Concat(
                schoolName, _lineSeperator,
                PAONRange, _space,
                Street, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);
            schoolPage.ClickAddAddrss();
            var addressDialog = new AddAddressDialog();
            addressDialog.PAONRangeSearch = PAONRange;
            addressDialog.PostCodeSearch = PostCode;
            addressDialog.ClickSearch();
            addressDialog.Addresses = addressDialog.Addresses;
            addressDialog.Street = Street;
            addressDialog.District = Town;
            addressDialog.Town = Town;
            addressDialog.PostCode = PostCode;
            addressDialog.ClickOk();
            schoolPage.Save();
            schoolPage.WaitLoading();
             Assert.AreEqual(addressDisplayLarge, schoolPage.MedicalPracticeAddresss);
        }

        private void EditSchoolAddress(MySchoolDetailsPage schoolPage)
        {
            //Edit Address
            addressDisplaySmall = string.Concat(
                PAONRange, _seperator, Street, _seperator, Town);
            var schoolName = TestDefaults.Default.SchoolName;
            addressDisplayLarge = string.Concat(
                schoolName, _lineSeperator,
                PAONRange, _space,
                NewStreet, _lineSeperator,
                Town, _lineSeperator,
                Town, _lineSeperator,
                PostCode, _lineSeperator,
                Country);
            schoolPage.ClickEditAddrss();
            var editAddressDialog = new AddAddressDialog();
            editAddressDialog.PAONRangeSearch = PAONRange;
            editAddressDialog.PostCodeSearch = PostCode;
            editAddressDialog.ClickSearch();
            editAddressDialog.Street = NewStreet;
            editAddressDialog.ClickOk();
            schoolPage.Save();
            schoolPage.WaitLoading();
            Assert.AreEqual(addressDisplayLarge, schoolPage.MedicalPracticeAddresss);
        }

        private void DeleteSchoolAddress(MySchoolDetailsPage schoolPage)
        {
            //Delete Address
            const string emptyAddress = "Address Not Defined";
            schoolPage.ClickDeleteAddrss();
            schoolPage.WaitLoading();
            Assert.AreEqual(emptyAddress, schoolPage.MedicalPracticeAddresss);
        }

    }
}

