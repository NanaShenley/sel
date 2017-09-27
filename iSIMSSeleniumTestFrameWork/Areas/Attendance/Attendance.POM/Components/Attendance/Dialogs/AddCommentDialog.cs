using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;

namespace POM.Components.Attendance
{
    public class AddCommentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-detail-section-name='detail'] .modal-content"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "CommentsMultiLineTextBox")]
        private IWebElement _commentTextBox;
        [FindsBy(How = How.Name, Using = "MinutesLate")]
        private IWebElement _minuteLateTextBox;

        public string Comment
        {
            set
            {
                _commentTextBox.Click();
                _commentTextBox.SetText(value);
            }
            get { return _commentTextBox.GetValue(); }
        }

        public string MinuteLate
        {
            set
            {
                _minuteLateTextBox.Click();
                _minuteLateTextBox.SetText(value);
            }
            get { return _minuteLateTextBox.GetValue(); }
        }

        #endregion

        #region Actions


        #endregion
    }
}
