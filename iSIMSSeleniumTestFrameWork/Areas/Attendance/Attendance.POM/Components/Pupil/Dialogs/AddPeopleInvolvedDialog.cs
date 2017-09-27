using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
	public class AddPeopleInvolvedDialog : BaseDialogComponent
	{
		public override By DialogIdentifier
		{
			get { return SimsBy.AutomationId("sen_review_people_involved_record_detail"); }
		}

		#region Page properties

		[FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_button']")]
		private IWebElement _selectButton;

		[FindsBy(How = How.Name, Using = "Name")]
		private IWebElement _nameTextBox;

		[FindsBy(How = How.Name, Using = "SenPeopleInvolvedRelationship.dropdownImitator")]
		private IWebElement _relationshipDropdown;

		[FindsBy(How = How.Name, Using = "Date")]
		private IWebElement _dateConsultedTextBox;

		[FindsBy(How = How.Name, Using = "Invited")]
		private IWebElement _invitedCheckBox;

		[FindsBy(How = How.Name, Using = "Accepted")]
		private IWebElement _acceptedCheckBox;

		[FindsBy(How = How.Name, Using = "Attended")]
		private IWebElement _attendedCheckBox;

		public string Relationship
		{
			set { _relationshipDropdown.EnterForDropDown(value); }
			get { return _relationshipDropdown.GetValue(); }
		}

		public string DateConsulted
		{
			set { _dateConsultedTextBox.SetDateTime(value); }
			get { return _dateConsultedTextBox.GetDateTime(); }
		}

		public string Name
		{
			get { return _nameTextBox.GetValue(); }
		}

		public bool IsInvited
		{
			set { _invitedCheckBox.Set(value); }
			get { return _invitedCheckBox.IsChecked(); }
		}

		public bool IsAccepted
		{
			set { _acceptedCheckBox.Set(value); }
			get { return _acceptedCheckBox.IsChecked(); }
		}

		public bool IsAttended
		{
			set { _attendedCheckBox.Set(value); }
			get { return _attendedCheckBox.IsChecked(); }
		}

		#endregion

		#region Public actions

		public SelectPeopleDialog ClickSelectPeople()
		{
			_selectButton.Click();
			Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
			return new SelectPeopleDialog();
		}

		#endregion
	}
}
