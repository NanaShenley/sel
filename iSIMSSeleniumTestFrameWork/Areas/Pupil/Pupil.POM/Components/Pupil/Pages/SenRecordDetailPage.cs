using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SenRecordDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("sen_record_detail"); }
        }

        #region Page Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.Name, Using = "LearningStrategies")]
        private IWebElement _learningStrategyTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_sen_review_button']")]
        private IWebElement _addSenReviewButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_sen_statement_button']")]
        private IWebElement _addSenStatemenButton;


        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_education_and_health_care_plan_button']")]
        private IWebElement _addEhcpButton;

        public string LearningStrategy
        {
            set { _learningStrategyTextBox.SetText(value); }
            get { return _learningStrategyTextBox.GetText(); }
        }

        public class SENStage
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='SENStatus.dropdownImitator']")]
            private IWebElement _stage;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDay;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDay;

            public string Stage
            {
                set
                {
                    _stage.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _stage.GetAttribute("value"); }
            }

            public string StartDay
            {
                set
                {
                    _startDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _startDay.GetDateTime(); }
            }

            public string EndDay
            {
                set
                {
                    _endDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _endDay.GetDateTime(); }
            }
        }

        public GridComponent<SENStage> SenStages
        {
            get
            {
                GridComponent<SENStage> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SENStage>(By.CssSelector("[data-maintenance-container='LearnerSENStatuses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SENNeed
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='NeedType.dropdownImitator']")]
            private IWebElement _needType;

            [FindsBy(How = How.CssSelector, Using = "[name$='Rank']")]
            private IWebElement _rank;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDay;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDay;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id*='documents']")]
            private IWebElement _documents;

            public string NeedType
            {
                set
                {
                    _needType.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _needType.GetAttribute("value"); }
            }

            public string Rank
            {
                set
                {
                    if (_rank.GetAttribute("value").Equals(value))
                    {
                        return;
                    }
                    Retry.Do(_rank.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    _rank.SetText(value);
                }
                get { return _rank.GetAttribute("value"); }
            }

            public string Description
            {
                set
                {
                    _description.SetText(value);
                    _description.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _description.GetAttribute("value"); }
            }

            public string StartDay
            {
                set
                {
                    _startDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _startDay.GetDateTime(); }
            }

            public string EndDay
            {
                set
                {
                    _endDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _endDay.GetDateTime(); }
            }

            public void AddDocument()
            {
                _documents.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
        }

        public GridComponent<SENNeed> SenNeeds
        {
            get
            {
                GridComponent<SENNeed> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SENNeed>(By.CssSelector("[data-maintenance-container='LearnerSENNeedTypes']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SENProvsion
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='SENProvisionType.dropdownImitator']")]
            private IWebElement _provisionType;

            [FindsBy(How = How.CssSelector, Using = "[name$='Rank']")]
            private IWebElement _rank;

            [FindsBy(How = How.CssSelector, Using = "[data-popup-button]")]
            private IWebElement _commentButton;

            [FindsBy(How = How.CssSelector, Using = "[name$='Comment']")]
            private IWebElement _comment;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDay;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDay;

            public string ProvisionType
            {
                set
                {
                    _provisionType.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _provisionType.GetAttribute("value"); }
            }

            public string StartDay
            {
                set
                {
                    _startDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _startDay.GetDateTime(); }
            }

            public string EndDay
            {
                set
                {
                    _endDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _endDay.GetDateTime(); }
            }

            public string Comment
            {
                set
                {
                    _comment.Click();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    _comment.SetText(value);
                }
                get { return _comment.GetAttribute("value"); }
            }
        }

        public GridComponent<SENProvsion> SenProvisions
        {
            get
            {
                GridComponent<SENProvsion> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SENProvsion>(By.CssSelector("[data-maintenance-container='LearnerSENProvisionTypes']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SENReview : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='ReviewType.dropdownImitator']")]
            private IWebElement _reviewType;

            [FindsBy(How = How.CssSelector, Using = "[name$='ReviewStatus.dropdownImitator']")]
            private IWebElement _reviewStatus;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDay;

            public string StartDate
            {
                set
                {
                    _startDay.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _startDay.GetDateTime(); }
            }

            public string ReviewType
            {
                set
                {
                    _reviewType.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _reviewType.GetAttribute("value"); }
            }

            public string ReviewStatus
            {
                set
                {
                    _reviewStatus.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _reviewStatus.GetAttribute("value"); }
            }
        }

        public GridComponent<SENReview> SenReviews
        {
            get
            {
                GridComponent<SENReview> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SENReview>(By.CssSelector("[data-maintenance-container='LearnerSENReviews']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class SENStatement : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='DateRequestedforAssessment']")]
            private IWebElement _dateRequest;

            [FindsBy(How = How.CssSelector, Using = "[name$='SENStatutoryStatement.dropdownImitator']")]
            private IWebElement _statementOutcome;

            [FindsBy(How = How.CssSelector, Using = "[name$='StatementDateFinalised']")]
            private IWebElement _dateFinalised;

            [FindsBy(How = How.CssSelector, Using = "[name$='StatementDateCeased']")]
            private IWebElement _dateCeased;

            [FindsBy(How = How.CssSelector, Using = "[name$='SENStatutoryAssessment.dropdownImitator']")]
            private IWebElement _elbResponse;

            public string DateRequested
            {
                set
                {
                    _dateRequest.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _dateRequest.GetDateTime(); }
            }

            public string ELBResponse
            {
                set
                {
                    _elbResponse.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _elbResponse.GetAttribute("value"); }
            }

            public string StatementOutcome
            {
                set
                {
                    _statementOutcome.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _statementOutcome.GetAttribute("value"); }
            }

            public string DateFinalised
            {
                set
                {
                    _dateFinalised.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _dateFinalised.GetDateTime(); }
            }

            public string DateCeased
            {
                set
                {
                    _dateCeased.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _dateCeased.GetDateTime(); }
            }
        }

        public GridComponent<SENStatement> SenStatements
        {
            get
            {
                GridComponent<SENStatement> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<SENStatement>(By.CssSelector("[data-maintenance-container='SENStatement']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class Ehcp : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='DateRequestedforAssessment']")]
            private IWebElement _dateRequest;

            [FindsBy(How = How.CssSelector, Using = "[name$='EHCPAssessmentRequestStatus.dropdownImitator']")]
            private IWebElement _ehcpOutcome;

            [FindsBy(How = How.CssSelector, Using = "[name$='DateFinalised']")]
            private IWebElement _dateFinalised;

            [FindsBy(How = How.CssSelector, Using = "[name$='DateCeased']")]
            private IWebElement _dateCeased;

            [FindsBy(How = How.CssSelector, Using = "[name$='EHCPAssessmentOutcome.dropdownImitator']")]
            private IWebElement _elbAssessmentOutcome;

            public string DateRequested
            {
                set
                {
                    _dateRequest.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _dateRequest.GetDateTime(); }
            }

            public string AssessmentOutcome
            {
                set
                {
                    _elbAssessmentOutcome.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _elbAssessmentOutcome.GetAttribute("value"); }
            }

            public string EHCPOutcome
            {
                set
                {
                    _ehcpOutcome.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _ehcpOutcome.GetAttribute("value"); }
            }

            public string DateFinalised
            {
                set
                {
                    _dateFinalised.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _dateFinalised.GetDateTime(); }
            }

            public string DateCeased
            {
                set
                {
                    _dateCeased.SetDateTimeByJS(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _dateCeased.GetDateTime(); }
            }
        }

        public GridComponent<Ehcp> Ehcps
        {
            get
            {
                GridComponent<Ehcp> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Ehcp>(By.CssSelector("[data-maintenance-container='EHCPs']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }




        #endregion

        #region Page Actions
        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsMessageSuccessAppear()
        {
            return _messageSuccess.IsElementExists();
        }

        public AddSenReviewDialog ClickAddSenReview()
        {
            _addSenReviewButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSenReviewDialog();
        }

        public AddSenStatementDialog ClickAddSenStatement()
        {
            _addSenStatemenButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSenStatementDialog();
        }

        public AddEhcpDialog ClickAddEhcp()
        {
            _addEhcpButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddEhcpDialog();
        }


        /// <summary>
        /// Au : Hieu Pham
        /// Check SEN record display for pupil name.
        /// </summary>
        /// <param name="pupilName"></param>
        /// <returns></returns>
        public bool IsSenRecordForPupilName(string pupilName)
        {
            try
            {
                IWebElement titleElement = SeleniumHelper.FindElement(SimsBy.AutomationId("sen_record_header_display_name"));
                return titleElement.GetText().Equals(pupilName);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        #endregion
    }
}
