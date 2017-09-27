using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using NUnit.Framework;
using POM.Helper;
using POM.Base;
using POM.Components.Common;



namespace POM.Components.Communication
{
    public class MedicalPracticeTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public MedicalPracticeTriplet()
        {
            _searchCriteria = new MedicalPracticeSearchPage(this);
        }

        #region Search

        private readonly MedicalPracticeSearchPage _searchCriteria;
        public MedicalPracticeSearchPage SearchCriteria { get { return _searchCriteria; } }

        public class MedicalPracticeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;
                
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

       
        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public MedicalPracticePage Create()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("add_button")).ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            return new MedicalPracticePage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: click Delete button to delete an existing scheme
        /// </summary>
      
        public void SelectSearchTile(MedicalPracticeSearchResultTile medicalPracticeTile)
        {
            if (medicalPracticeTile != null)
            {
                medicalPracticeTile.Click();
            }
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                var confirmDialog = new DeleteConfirmationDialog();
                confirmDialog.ConfirmDelete();
            }
        }

        public MedicalPracticePage CancelDeleteSchoolIntake()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationDialog();
                confirmDialog.CancelDelete();
            }
            return new MedicalPracticePage();
        }

       #endregion
    }

    public class MedicalPracticeSearchPage : SearchCriteriaComponent<MedicalPracticeTriplet.MedicalPracticeSearchResultTile>
    {
        public MedicalPracticeSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _searchByName;

        public string SearchByName
        {
            get { return _searchByName.GetValue(); }
            set { _searchByName.SetText(value); }
        }
     
        #endregion
    }
}
