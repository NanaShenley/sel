using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil;
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

        public string Comment
        {
            set
            {
                _commentTextBox.Click();
                _commentTextBox.SetText(value);
            }
            get { return _commentTextBox.GetValue(); }
        }

        #endregion

        #region Actions


        #endregion
    }
}
