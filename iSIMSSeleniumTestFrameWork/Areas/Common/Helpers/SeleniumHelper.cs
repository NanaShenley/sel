using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using POM.Helper;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;

namespace SharedComponents.Helpers
{
    public static class SimsBy
    {
        public static By AutomationId(string id)
        {
            return By.CssSelector(SeleniumHelper.AutomationId(id));
        }
    }

    /// <summary>
    /// Provides static helpers in a sequential and fluent syntaxes for rapidly constructing Selenium tests.
    /// </summary>
    public static class SeleniumHelper
    {
        public const string AutomationIdAttributeFormat = "[data-automation-id='{0}']";

        public static string AutomationId(string value)
        {
            return String.Format(AutomationIdAttributeFormat, value);
        }

        public static By SelectByDataAutomationID(string idToFind)
        {
            return By.CssSelector(AutomationId(idToFind));
        }

        public static IWebElement ScrollToElement(this IWebElement element, By elementToScrollTo, string containerWhichNeedsToScrollCss)
        {
            var scrollTo = element.FindElement(elementToScrollTo);
            var h = scrollTo.Location.Y + scrollTo.Size.Height;

            WebContext.WebDriver.Url = string.Format("javascript:$('{0}').scrollTop({1},0);", containerWhichNeedsToScrollCss, h);
            Thread.Sleep(500);
            return element;
        }

        public static IWebElement GoToAccordionPanel(this IWebElement element, string title)
        {
            Thread.Sleep(1000);
            var accordion = element.FindElementsSafe(By.CssSelector(".accordion-default [data-accordian-panel]"));

            foreach (var accordionSection in accordion)
            {
                var header = accordionSection.FindChild(By.CssSelector(".panel-heading"));
                if (header.Text.Trim().Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    var h = header.Location.Y + header.Size.Height;
                    WebContext.WebDriver.Url = "javascript:$('#screen > div > div.layout-col.main.pane > div > div > div > div.form-body > div > div > div > div.layout-col.form.pane > div > div.form-body > div').scrollTop(" + h + ",0);";

                    header.Click();

                    break;
                }
            }

            return element;
        }

        public static IWebElement ValueChangeTriggerAction(this IWebElement element, Func<IWebElement, IWebElement> action, string updateSectionId)
        {
            //SJootle - all comments here in this method should not be removed. It was an approach which no longer seems to work,
            //for now im getting this workaround in. I'll look to find a better solution soon.

            var updateSection = element.FindChild(By.CssSelector("[data-section-id=\"" + updateSectionId + "\"]"));

            //var sender = GetWaiterForAriaBusyState(updateSection, updateSectionId, "false");
            //var waiter = GetWaiterForAriaBusyState(updateSection, updateSectionId, "true");

            //sender.Start();
            //Debug.WriteLine(string.Format("ValueChange for \"{0}\" started.", updateSectionId));
            action.Invoke(element);
            Thread.Sleep(5000);
            //sender.Join(BrowserDefaults.TimeOut);
            //waiter.Start();
            //Debug.WriteLine(string.Format("ValueChange for \"{0}\" in progress.", updateSectionId));
            //waiter.Join(BrowserDefaults.TimeOut);
            //Debug.WriteLine(string.Format("ValueChange for \"{0}\" complete.", updateSectionId));

            var topLevelSelector = element.GetTopLevelSelector();
            var topLevelElement = ElementRetriever.GetOnceLoaded(topLevelSelector);
            element = topLevelElement;
            element.SetTopLevelSelector(topLevelSelector);

            return element;
        }

        private static Thread GetWaiterForAriaBusyState(IWebElement updateSection, string updateSectionId, string busyStateToWaitFor)
        {
            return new Thread(() =>
            {
                {
                    var busy = updateSection.GetAttribute("aria-busy");
                    Debug.WriteLine("\"{0}\" aria-busy=\"{1}\".", updateSectionId, busyStateToWaitFor);
                    while (busy == busyStateToWaitFor)
                    {
                        busy = updateSection.GetAttribute("aria-busy");
                        Debug.WriteLine("\"{0}\" aria-busy=\"{1}\".", updateSectionId, busyStateToWaitFor);
                        Thread.Sleep(1);
                    }
                }
            });
        }

        private static readonly ConditionalWeakTable<IWebElement, By> TopLevelSelectors = new ConditionalWeakTable<IWebElement, By>();

        private static By GetTopLevelSelector(this IWebElement element)
        {
            By topLevelSelector;
            if (!TopLevelSelectors.TryGetValue(element, out topLevelSelector)) throw new Exception("Cannot identify top level element.");
            return topLevelSelector;
        }

        private static void SetTopLevelSelector(this IWebElement element, By topLevelSelector)
        {
            TopLevelSelectors.Add(element, topLevelSelector);
        }

        #region Fluent

        /// <summary>
        /// Gets an element from the DOM.
        /// </summary>
        /// <param name="selector">A selector describing which element to retrieve. Scoped to the document root.</param>
        /// <returns></returns>
        public static IWebElement Get(By selector)
        {
            IWebElement element = ElementRetriever.GetOnceLoaded(selector);
            element.SetTopLevelSelector(selector);
            return element;
        }

        /// <summary>
        /// Gets an element from unique element automation id
        /// </summary>
        /// <param name="id">unique element automation id</param>
        /// <returns></returns>
        public static IWebElement Get(string id)
        {
            var cssFormat = AutomationId(id);
            return Get(By.CssSelector(cssFormat));
        }

        /// <summary>
        /// An element (such as a div) is considered available and displayed even with a zero height or width. This method corrects that.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IWebElement GetVisible(By selector)
        {
            var elementToGet = ElementRetriever.GetOnceLoaded(selector);
            if (elementToGet.Displayed
                && elementToGet.Size.Height > 0
                && elementToGet.Size.Width > 0)
            {
                return elementToGet;
            }

            // Element not truly visible
            return null;
        }

        /// <summary>
        /// Clicks an element.
        /// </summary>
        /// <param name="element">The element containing the element to click.</param>
        /// <param name="selector">A selector describing which element to click. Scoped by the element this method is called upon.</param>
        /// <returns></returns>
        public static IWebElement Click(this IWebElement element, By selector)
        {
            var elementToClick = element.FindChild(selector);
            elementToClick.WaitUntilState(ElementState.Displayed);

            elementToClick.Click();
            return element;
        }

        /// <summary>
        /// Gets an element contained within another.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing which element to retrieve. Scoped by the element this method is called upon.</param>
        /// <returns></returns>
        public static IWebElement FindChild(this IWebElement element, By selector)
        {
            element.WaitUntilState(ElementState.Displayed);
            return ElementRetriever.GetOnceLoaded(element, selector);
        }

        /// <summary>
        /// Clicks on one element, waits for another to load. Typically used when areas of the DOM are being replaced. For example, saving and reloading a record.
        /// </summary>
        /// <param name="elementToClick">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="elementToWaitFor">A selector describing which element to wait until it's loaded. Scoped to the document root.</param>
        /// <returns>The element you've been waiting for.</returns>
        public static IWebElement ClickAndWaitFor(By elementToClick, By elementToWaitFor)
        {
            var toClick = Get(elementToClick);
            Retry.Do(toClick.Click);

            var toWaitUpon = Get(elementToWaitFor);
            toWaitUpon.WaitUntilState(ElementState.Displayed);

            return ElementRetriever.GetOnceLoaded(elementToWaitFor);
        }

        public static IWebElement WaitForUpdateSection(this IWebElement element, By elementInUpdateSection)
        {
            var topLevelSelector = element.GetTopLevelSelector();
            var topLevelElement = ElementRetriever.GetOnceLoaded(topLevelSelector);
            var updatedElement = topLevelElement.FindElementSafe(elementInUpdateSection);
            updatedElement.WaitUntilState(ElementState.Displayed);

            element = topLevelElement;
            element.SetTopLevelSelector(topLevelSelector);
            return element;
        }

        public static IWebElement WaitForUpdateSection(this IWebElement element, string updateSectionId)
        {
            //var topLevelSelector = element.GetTopLevelSelector();
            //var topLevelElement = ElementRetriever.GetOnceLoaded(topLevelSelector);

            var updateSection = element.FindChild(By.CssSelector("[data-section-id=\"" + updateSectionId + "\"]"));
            for (int i = 0; i < 1000; i++)
            {
                var busy = updateSection.GetAttribute("aria-busy");

                Debug.WriteLine("aria-busy: {0} {1}", updateSectionId, busy);
                Thread.Sleep(1);
            }
            //var updatedElement = topLevelElement.FindElementSafe(elementInUpdateSection);
            //updatedElement.WaitUntilState(ElementState.Displayed);

            //element = topLevelElement;
            //element.SetTopLevelSelector(topLevelSelector);
            return element;
        }

        public static IWebElement ClickAndWaitFor(this IWebElement elementToClick, By elementToWaitFor)
        {
            elementToClick.Click();

            var toWaitUpon = Get(elementToWaitFor);
            toWaitUpon.WaitUntilState(ElementState.Displayed);

            return ElementRetriever.GetOnceLoaded(elementToWaitFor);
        }

        /// <summary>
        /// Gets the specified grid row from a grid.
        /// </summary>
        /// <param name="element">The element containing the grid.</param>
        /// <param name="gridSelector">A selector describing which grid the row to retrieve belongs to. Scoped by the element this method is called upon.</param>
        /// <param name="rowIndex">The zero-based index of the row to retrieve.</param>
        /// <returns></returns>
        public static IWebElement GetGridRow(this IWebElement element, By gridSelector, int rowIndex)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            element.WaitUntilState(ElementState.Displayed);
            var wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> els = wait.Until(x => x
                .FindElement(gridSelector)
                .WaitUntilState(ElementState.Displayed)
                .FindElements(By.CssSelector(gridRowCss)));

            return els[rowIndex];
        }

        /// <summary>
        /// Gets the specified grid row from a grid.
        /// </summary>
        /// <param name="element">The element containing the grid.</param>
        /// <param name="gridSelector">A selector describing which grid the row to retrieve belongs to. Scoped by the element this method is called upon.</param>
        /// <param name="rowIndex">The zero-based index of the row to retrieve.</param>
        /// <returns></returns>
        public static IWebElement GetGridRow(this IWebElement element, int rowIndex)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            element.WaitUntilState(ElementState.Displayed);
            var wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> els = element.WaitUntilState(ElementState.Displayed).FindElements(By.CssSelector(gridRowCss));

            return els[rowIndex];
        }

        public static ReadOnlyCollection<IWebElement> GetGridRows(By gridSelector)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            var wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> rowCollection = wait.Until(x => x
                .FindElement(gridSelector)
                .WaitUntilState(ElementState.Displayed)
                .FindElements(By.CssSelector(gridRowCss)));

            return rowCollection;
        }

        /// <summary>
        /// Selects an option from a "Selector" control without holding an element reference to avoid stale element reference exceptions, waits until
        /// AJAX request has completed to avoid arbitrary Thread.Sleeps guessing when selector has finished
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="option"></param>
        public static void ChooseSelectorOption(By selector, string option)
        {
            var selectorInputQuery = SeSugar.Automation.SimsBy.CssSelector("[id*=s2id_autogen][id$=_search]");

            //http://stackoverflow.com/questions/11908249/debugging-element-is-not-clickable-at-point-error
            // Peter Bernier, Jul 8 15 highlighted workaround to fragility using Chrome web driver and trying to click
            // screen elements, send an "ArrowDown" instead (don't use "Enter" because this submits the search form)
            ElementRetriever.FindElementSafe(WebContext.WebDriver, selector).Clear();
            ElementRetriever.FindElementSafe(WebContext.WebDriver, selector).SendKeys(Keys.ArrowDown);
            
            ElementRetriever.FindElementSafe(WebContext.WebDriver, selectorInputQuery).Clear();
            ElementRetriever.FindElementSafe(WebContext.WebDriver, selectorInputQuery).SendKeys(option);
            ElementRetriever.FindElementSafe(WebContext.WebDriver, selectorInputQuery).SendKeys(Keys.Tab);

            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        /// <summary>
        /// Selects an option from a "Selector" control.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the top-level element of the Selector control. Scoped by the element this method is called upon.</param>
        /// <param name="option">The literal value of the option to select (case sensitive).</param>
        /// <returns></returns>
        public static IWebElement ChooseSelectorOption(this IWebElement element, By selector, string option)
        {
            var selectorElement = element.FindChild(selector);
            selectorElement.ChooseSelectorOption(option);
            return element;
        }

        /// <summary>
        /// Selects an option from a "Selector" control.
        /// </summary>
        /// <param name="element">A selector describing the top-level element of the Selector control.</param>
        /// <param name="option">The literal value of the option to select (case sensitive).</param>
        /// <returns></returns>
        public static IWebElement ChooseSelectorOption(this IWebElement element, string option)
        {
            var selectorSearchElement = OpenSelector(element);
            selectorSearchElement.Click();
            selectorSearchElement.Clear();
            selectorSearchElement.SendKeys(option);
            selectorSearchElement.SendKeys(Keys.Tab);
            return element;
        }

        /// <summary>
        /// Opens a "Selector" control.
        /// </summary>
        /// <param name="element">A selector describing the top-level element of the Selector control.</param>
        /// <returns></returns>
        public static IWebElement OpenSelector(this IWebElement element)
        {
            element.WaitUntilState(ElementState.Displayed);
            Retry.Do(element.Click);

            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";
            var selectorSearchElement = ElementRetriever.GetOnceLoaded(By.CssSelector(selectorSearchBox));
            selectorSearchElement.WaitUntilState(ElementState.Displayed);

            return selectorSearchElement;
        }

        /// <summary>
        /// Selects an option from a "MultiSelector" control.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of all checkboxes within a MultiSelector. Typically the "Name" attribute. Scoped by the element this method is called upon.</param>
        /// <param name="option">The literal value of the option to select (case sensitive).</param>
        /// <returns></returns>
        public static IWebElement ChooseMultiSelectorOption(this IWebElement element, By selector, string option)
        {
            element.WaitUntilState(ElementState.Displayed);

            foreach (var e in element.FindElementsSafe(selector))
            {
                var id = e.GetAttribute("id");
                var label = element.FindElementSafe(By.CssSelector("label[for=\"" + id + "\""));

                if (label.Text == option)
                {
                    label.Click();
                    break;
                }
            }
            return element;
        }

        /// <summary>
        /// Waits for element to be clickable and then clicks on it
        /// </summary>
        /// <param name="selector">element selector</param>
        public static void WaitForElementClickableThenClick(By selector)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementToBeClickable(selector));
            FindAndClick(selector);
        }

        /// <summary>
        /// Sets the value of a Checkbox.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of the Checkbox. Scoped by the element this method is called upon.</param>
        /// <param name="checked">Checked or not.</param>
        /// <returns></returns>
        public static IWebElement SetCheckBox(this IWebElement element, By selector, bool @checked)
        {
            var checkBoxElement = element.FindChild(selector);
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                    checkBoxElement.Click();
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                    checkBoxElement.Click();
            }

            return element;
        }

        public static IWebElement SetCheckBox(this IWebElement checkBoxElement, bool @checked)
        {
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                    checkBoxElement.Click();
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                    checkBoxElement.Click();
            }

            return checkBoxElement;
        }

        //tristate click
        public static IWebElement SetTriStateCheckBox(this IWebElement element, By selector, bool @checked)
        {
            var checkBoxElement = element.FindChild(selector);
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                {
                    checkBoxElement.Click();
                }
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                {
                    checkBoxElement.Click();
                }
            }

            return element;
        }

        /// <summary>
        /// Sets the value of a Text control.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of the Text control. Scoped by the element this method is called upon.</param>
        /// <param name="text">The value to enter into the Text control.</param>
        /// <returns></returns>
        public static IWebElement SetText(this IWebElement element, By selector, string text)
        {
            return SetText(element.FindChild(selector), text);
        }

        public static IWebElement SetText(this IWebElement element, By selector, string text, bool validate = true)
        {
            return SetText(element.FindChild(selector), text, validate);
        }

        public static IWebElement SetText(this IWebElement element, string text, bool validate = true)
        {
            element.WaitUntilState(ElementState.Displayed);
            element.Clear();
            element.SendKeys(text);
            if (validate)
                element.WaitUntilCondition(x => (x.GetAttribute("value") ?? String.Empty) == text);
            element.SendKeys(Keys.Tab);

            return element;
        }

        public static IWebElement DeStaler(this IWebElement element, By selector)
        {
            try
            {
                var displayed = element.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                element = WebContext.WebDriver.FindElement(selector);
            }
            return element;
        }

        public static ReadOnlyCollection<IWebElement> DeStaler(this ReadOnlyCollection<IWebElement> elements, By selector)
        {
            try
            {
                var displayed = elements.Any() && elements.FirstOrDefault().Displayed;
            }
            catch (StaleElementReferenceException)
            {
                elements = WebContext.WebDriver.FindElements(selector);
            }
            return elements;
        }

        /// <summary>
        /// Sets the Date and Time in a DateTime control.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of the DateTime control. Scoped by the element this method is called upon.</param>
        /// <param name="dateTime">The value to enter into the DateTime control.</param>
        /// <returns></returns>
        public static IWebElement SetDateTime(this IWebElement element, By selector, string dateTime)
        {
            //TODO use the date picker component.
            element.SetText(selector, dateTime);
            return element;
        }

        /// <summary>
        /// Checks the value of the checkbox and returns TRUE if checkbox is checked else returns FALSE.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of the Checkbox. Scoped by the element this method is called upon.</param>
        /// <returns>TRUE if checkbox is checked else returns FALSE.</returns>
        public static bool IsCheckboxChecked(this IWebElement element, By selector)
        {
            var checkBoxElement = element.FindChild(selector);
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            //Assuming !NULL means checked.
            return checkedValue != null ? true : false;
        }

        public static bool IsCheckboxChecked(this IWebElement checkBoxElement)
        {
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            //Assuming !NULL means checked.
            return checkedValue != null ? true : false;
        }

        /// <summary>
        /// Returns the value of the element
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <returns></returns>
        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value") ?? String.Empty;
        }

        #endregion Fluent

        #region General

        /// <summary>
        /// Creates an instance of a Selenium Component
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public static TComponent InitialiseComponent<TComponent>() where TComponent : new()
        {
            //do some other stuff if you want

            return new TComponent();
        }

        #endregion General

        #region Navigation

        /// <summary>
        /// Retrieves the text of a menu item
        /// </summary>
        /// <param name="itemId">Menu item automation id</param>
        public static string GetMenuText(string itemId)
        {
            const string statement = "return $('#task-menu').find('[data-automation-id={0}]').text().trim()";
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;

            var label = (string)jsExecutor.ExecuteScript(string.Format(statement, itemId));

            return label;
        }

        /// <summary>
        /// Navigates to the page via the side menu
        /// </summary>
        /// <param name="itemId">Menu item automation id</param>
        public static void NavigateMenu(string itemId)
        {
            const string menuUrlJsFormat = "$('#task-menu').find('[data-automation-id={0}]').data('ajaxUrl')";
            const string navigationJsFormat = "sims_commander.OpenPage('#task-menu',{0}, '')";
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;

            SeSugar.Automation.Retry.Do((Action)(() => jsExecutor.ExecuteScript(string.Format(navigationJsFormat, string.Format(menuUrlJsFormat, itemId)))));
            AutomationSugar.WaitForAjaxCompletion();
        }

        /// <summary>
        /// Navigates to the page via the side menu
        /// </summary>
        /// <param name="topLevel">e.g.: Tasks or Lookups</param>
        /// <param name="category">e.g.: Staff</param>
        /// <param name="item">e.g: Service Terms</param>
        public static void NavigateMenu(string topLevel, string category, string item)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            Thread.Sleep(2000);
            WaitForElementClickableThenClick(By.CssSelector("[title=\"Task Menu\"]"));
            var taskMenu = Get(By.CssSelector("#task-menu"));
            var level1 = taskMenu.FindElementsSafe(By.CssSelector(".shell-task-menu-section"));
            foreach (var l1Item in level1)
            {
                var l1Header = l1Item.FindElementSafe(By.CssSelector(".shell-task-menu-section-header"));
                if (l1Header.Text.Trim().Equals(topLevel, StringComparison.OrdinalIgnoreCase))
                {
                    var level2 = l1Item.FindElementsSafe(By.CssSelector(".panel-group.accordion-sidemenu .panel-default.panel"));
                    foreach (var l2Item in level2)
                    {
                        var l2Header = l2Item.FindElementSafe(By.CssSelector(".panel-heading"));
                        if (l2Header.Text.Trim().Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            waiter.Until(ExpectedConditions.ElementToBeClickable(l2Header));
                            Thread.Sleep(2000);
                            l2Header.Click();
                            Thread.Sleep(2000);
                            var level3 = l2Item.FindElementsSafe(By.CssSelector(".shell-task-menu-item-title")); //changed based on the menu item Title
                            foreach (var l3Item in level3)
                            {
                                if (l3Item.Text.Trim().Equals(item))
                                {
                                    waiter.Until(ExpectedConditions.ElementToBeClickable(l3Item));
                                    Thread.Sleep(2000);
                                    l3Item.Click();
                                    Thread.Sleep(2000);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        #endregion Navigation

        #region Navigation

        /// <summary>
        /// Navigates back to the page via the side menu and checks whether the sub menu is already expanded
        /// </summary>
        /// <param name="topLevel">e.g.: Tasks or Lookups</param>
        /// <param name="category">e.g.: Staff</param>
        /// <param name="item">e.g: Service Terms</param>
        public static void NavigatebackToMenu(string topLevel, string category, string item)
        {
            FindAndClick(By.CssSelector("[title=\"Task Menu\"]"));
            var taskMenu = Get(By.CssSelector("#task-menu"));
            var level1 = taskMenu.FindElementsSafe(By.CssSelector(".shell-task-menu-section"));
            foreach (var l1Item in level1)
            {
                var l1Header = l1Item.FindElementSafe(By.CssSelector(".shell-task-menu-section-header"));
                if (l1Header.Text.Trim().Equals(topLevel, StringComparison.OrdinalIgnoreCase))
                {
                    var level2 = l1Item.FindElementsSafe(By.CssSelector(".panel-group.accordion-sidemenu .panel-default.panel"));
                    foreach (var l2Item in level2)
                    {
                        var l2Header = l2Item.FindElementSafe(By.CssSelector(".panel-heading"));
                        if (l2Header.Text.Trim().Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            if (l2Item.GetAttribute("aria-expanded") == "false")
                            {
                                l2Header.Click();
                                Thread.Sleep(2000);
                            }
                            var level3 = l2Item.FindElementsSafe(By.CssSelector(".shell-task-menu-item-title")); //changed based on the menu item Title
                            foreach (var l3Item in level3)
                            {
                                if (l3Item.Text.Trim().Equals(item))
                                {
                                    l3Item.Click();
                                    Thread.Sleep(2000);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
        }

        #endregion Navigation

        #region Login

        /// <summary>
        /// The types of iSIMS user.
        /// </summary>
        public enum iSIMSUserType
        {
            TestUser,
            PersonnelOfficer,
            SchoolAdministrator,
            SENCoordinator,
            AdmissionsOfficer,
            HeadTeacher,
            SystemManger,
            AssessmentCoordinator,
            CurricularManager,
            SeniorManagementTeam,
            ClassTeacher,
            ReturnsManager
        }

        /// <summary>
        /// Log in as a given user. TestUser as default.
        /// </summary>
        /// <param name="userType">The user type to login as.</param>
        /// <param name="schoolSelection">show  school selection?</param>
        /// <param name="enabledFeatures"></param>
        public static void Login(SeleniumHelper.iSIMSUserType userType = iSIMSUserType.TestUser, bool schoolSelection = true, params string[] enabledFeatures)
        {
            if (Configuration.ForceTestUserLogin) userType = iSIMSUserType.TestUser;

            switch (userType)
            {
                case iSIMSUserType.TestUser:
                    Login(
                        TestDefaults.Default.TestUser,
                        TestDefaults.Default.TestUserPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        schoolSelection,
                        enabledFeatures);
                    break;

                case iSIMSUserType.SchoolAdministrator:
                    Login(
                        TestDefaults.Default.SchoolAdministratorUser,
                        TestDefaults.Default.SchoolAdministratorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.SENCoordinator:
                    Login(
                        TestDefaults.Default.SENCoordinator,
                        TestDefaults.Default.SENCoordinatorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.AdmissionsOfficer:
                    Login(
                        TestDefaults.Default.AdmissionsOfficer,
                        TestDefaults.Default.AdmissionsOfficerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.PersonnelOfficer:
                    Login(
                        TestDefaults.Default.PersonnelOfficer,
                        TestDefaults.Default.PersonnelOfficerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.HeadTeacher:
                    Login(
                        TestDefaults.Default.HeadTeacher,
                        TestDefaults.Default.HeadTeacherPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.SystemManger:
                    Login(
                        TestDefaults.Default.SystemManger,
                        TestDefaults.Default.SystemMangerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.AssessmentCoordinator:
                    Login(
                        TestDefaults.Default.AssessmentCoordinator,
                        TestDefaults.Default.AssessmentCoordinatorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.CurricularManager:
                    Login(
                        TestDefaults.Default.CurricularManager,
                        TestDefaults.Default.CurricularManagerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.SeniorManagementTeam:
                    Login(
                        TestDefaults.Default.SeniorManagementTeam,
                        TestDefaults.Default.SeniorManagementTeamPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.ClassTeacher:
                    Login(
                        TestDefaults.Default.ClassTeacher,
                        TestDefaults.Default.ClassTeacherPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                case iSIMSUserType.ReturnsManager:
                    Login(
                        TestDefaults.Default.ReturnsManager,
                        TestDefaults.Default.ReturnsManagerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("userType");
            }
        }

        /// <summary>
        /// Log in to iSIMS Shell.
        /// </summary>
        public static void Login(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection = true, params string[] enabledFeatures)
        {
            NavigateToHomePage(user, password, tenantId, schoolName, url, schoolSelection, false, enabledFeatures);
        }

        /// <summary>
        /// Log in to iSIMS Enterprise Shell.
        /// </summary>
        public static void EnterpriseLogin(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection = true, params string[] enabledFeatures)
        {
            NavigateToHomePage(user, password, tenantId, schoolName, url, schoolSelection, true, enabledFeatures);
        }

        /// <summary>
        /// Navigate to School or Enterprise Shell
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="tenantId"></param>
        /// <param name="schoolName"></param>
        /// <param name="url"></param>
        /// <param name="schoolSelection"></param>
        /// <param name="IsEnterprise"></param>
        /// <param name="enabledFeatures"></param>
        public static void NavigateToHomePage(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection = true, bool IsEnterprise = false, params string[] enabledFeatures)
        {
            HomePage homePage = HomePage.NavigateTo(user, password, tenantId, schoolName, url, schoolSelection, enabledFeatures);
            if (schoolSelection && !IsEnterprise)
                homePage.MenuBar().WaitFor();
            Thread.Sleep(2000);
        }

        #endregion Login

        #region Seacrh Criteria

        /// <summary>
        /// Function to click on a checkbox within a checkbox group in a Search Criteria
        /// eg in form[data-section-id='searchCriteria'
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="checkBoxGroupText"></param>
        /// <param name="checkBoxText"></param>
        public static void FindCheckBoxAndClick(this IWebElement searchCriteria, string checkBoxGroupText, List<string> checkBoxText)
        {
            IReadOnlyCollection<IWebElement> getCheckBoxElements = searchCriteria.FindElements(By.CssSelector(".checkbox-tree.wrap"));
            foreach (var checkBoxElement in getCheckBoxElements)
            {
                var groupHeader = checkBoxElement.FindElement(By.CssSelector("a[data-parent]"));
                if (groupHeader.Text == checkBoxGroupText)
                {
                    var checkBoxGroupContainer = checkBoxElement.FindElement(By.Id(groupHeader.GetAttribute("data-parent").TrimStart('#')));
                    var checkBoxGroup = checkBoxGroupContainer.FindElement(By.CssSelector("div[data-checkbox-tree-list]"));
                    var groupId = checkBoxGroup.GetAttribute("id");
                    
                    var checkboxContainer = checkBoxGroup.FindElements(By.CssSelector("label[data-checkbox-container='#" + groupId + "']"));
                    foreach (var checkbox in checkboxContainer.Where(cb => checkBoxText.Any(cb.Text.Contains)))
                    {
                        checkbox.Click();
                    }
                }
            }
        }

        #endregion Seacrh Criteria

        #region Find and Click

        /// <summary>
        /// Find an element and click it.
        /// </summary>
        /// <param name="elementAccessor">A selector describing which element to click. Scoped to the document root.</param>
        public static void FindAndClick(By elementAccessor)
        {
            //TODO Use ElementRetrieval helper methods.
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(15));
            BaseSeleniumComponents.WaitForAndClick(BrowserDefaults.TimeOut, elementAccessor);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Helper method that bypasses Selenium API methods and uses raw javascript to find a checkbox
        /// in the DOM and toggle its selected state. This avoids problems where the Chrome web driver throws
        /// exceptions where an AJAX spinner overlay or a modal dialog div has a higher Z order
        /// </summary>
        /// <param name="elementName"></param>
        public static void ToggleCheckbox(string elementName)
        {
            var script = "try { " +
                    "var checkboxes = document.getElementsByName('" + elementName + "');" +
                    "console.log('ToggleCheckbox: checkboxes.length:' + checkboxes.length);" +
                    "for(x = 0; x < checkboxes.length; x++)" +
                    "{" +
                        "checkboxes[x].checked = !checkboxes[x].checked; " +
                        "console.log('ToggleCheckbox: checkbox toggled - ' +  checkboxes[x].name); " +
                    "} " +
                "} catch(ex) { " +
                    "console.log('ToggleCheckbox: failed with - ' + ex); " +
                "} ";

            ((IJavaScriptExecutor) WebContext.WebDriver).ExecuteScript(script);
        }

        /// <summary>
        /// Helper method that bypasses Selenium API methods and uses raw javascript to find a checkbox
        /// in the DOM and toggle its selected state. This avoids problems where the Chrome web driver throws
        /// exceptions where an AJAX spinner overlay or a modal dialog div has a higher Z order
        /// </summary>
        /// <param name="automationId">The data automation id of the parent container</param>
        /// <param name="labelText">The label text for checkbox</param
        public static void ToggleCheckboxForLabel(string automationId, string labelText)
        {
            var checkBoxXpath = String.Format(
                "\".//*[@data-automation-id='{0}']/../../following-sibling::div//div[@data-checkbox-tree-list='']//label[text()='{1}']//preceding-sibling::input\"",
                automationId,
                labelText);

            var script = "try { " +
                    "var checkbox = document.evaluate(" + checkBoxXpath + ", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; " +
                    "checkbox.checked = !checkbox.checked " +
                "} catch(ex) { " +
                    "console.log('ToggleCheckboxForLabel: failed with - ' + ex); " +
                "} ";

            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(script);
        }

        /// <summary>
        /// Find an element and click it.
        /// </summary>
        /// <param name="elementAccessor">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="timeout">The longest time to wait for the action to be completed.</param>
        public static void FindAndClick(By elementAccessor, TimeSpan timeout)
        {
            //TODO Use ElementRetrieval helper methods.
            BaseSeleniumComponents.WaitForAndClick(timeout, elementAccessor);
        }

        /// <summary>
        /// Find and element with a specific part-name concatenated to main selector
        /// </summary>
        /// <param name="elementAccessor"></param>
        /// <param name="cssElements"></param>
        public static void FindAndClick(string elementAccessor, params object[] cssElements)
        {
            var link = By.CssSelector(string.Format(elementAccessor, cssElements));
            FindAndClick(link);
        }

        #endregion Find and Click

        /// <summary>
        /// Switch on the implicit timeouts to default value
        /// </summary>
        public static void SwitchOnImplicitTimeout()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(TestDefaults.Default.TimeOut.Seconds));
        }

        /// <summary>
        /// Switch off the implicit timeout
        /// </summary>
	    public static void SwitchOffImplicitTimeout()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Enables a feature within Feature Bee
        /// </summary>
        /// <param name="feature">The feature to be enabled as specified in the data-toggle-target attribute (case sensitive)</param>
        public static void EnableFeature(string feature)
        {
            ToggleFeature(feature, true);
        }

        /// <summary>
        /// Disables a feature within Feature Bee
        /// </summary>
        /// <param name="feature">The feature to be disabled as specified in the data-toggle-target attribute (case sensitive)</param>
        public static void DisableFeature(string feature)
        {
            ToggleFeature(feature, false);
        }

        public static string ExecuteJavascript(string commandScript)
        {
            try
            {
                IWebDriver webDriver = WebContext.WebDriver;
                IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

                var result = js.ExecuteScript(commandScript);
                if (result != null)
                    return result.ToString();
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Toggles a feature within Feature Bee
        /// </summary>
        /// <param name="feature">The feature to be enabled/disabled as specified in the data-toggle-target attribute (case sensitive)</param>
        /// <param name="enable">Whether the feature should be enabled or disabled</param>
        private static void ToggleFeature(string feature, bool enable)
        {
            bool featureBeeEnabled = false;
            By featureBeeCss = By.CssSelector(".feature-bee-show");

            try
            {
                WebContext.WebDriver.FindElement(featureBeeCss);

                featureBeeEnabled = true;
            }
            catch (NoSuchElementException)
            {
            }

            if (!featureBeeEnabled)
            {
                WebContext.WebDriver.Url = string.Format("{0}{1}/Home?featuretray=show", Configuration.GetSutUrl(), TestDefaults.Default.Path);
                WebContext.WebDriver.SwitchTo().Alert().Accept();
                Thread.Sleep(1000);
            }

            var featureBeePanelCss = By.CssSelector(".feature-bee-show");
            Wait.WaitForElementReady(By.CssSelector(".feature-bee-show"));
            Wait.WaitForElementDisplayed(By.CssSelector(".feature-bee-show"));
            var featureBeePanel = WebContext.WebDriver.FindElement(featureBeePanelCss);

            if (featureBeePanel.Displayed)
            {
                Wait.WaitLoading();
                IWebElement btn = WebContext.WebDriver.FindElement(featureBeeCss).WaitUntilState(ElementState.Enabled);
                btn.ClickUntilAppearElement(By.CssSelector(".feature-bee-panel"));
                Thread.Sleep(1000);
            }

            var featureBeeOptionCss = By.CssSelector(string.Format(".feature-bee-scroll-content-item button[title='{0}']", feature));
            IWebElement featureBeeOption = WebContext.WebDriver.FindElement(featureBeeOptionCss);
            if (!featureBeeOption.Displayed)
            {
                ExecuteJavascript("$$(document.getElementsByClassName(\"feature-bee-scroll-pane\")[0].scrollLeft += 2000)");
            }
            if (!featureBeeOption.Displayed)
            {
                ExecuteJavascript("$$(document.getElementsByClassName(\"feature-bee-scroll-pane\")[0].scrollLeft += 4000)");
            }

            if ((enable && featureBeeOption.Text == "Off") ||
                !enable && featureBeeOption.Text == "On")
            {
                Wait.WaitLoading();
                SeleniumHelper.FindAndClick(featureBeeOptionCss);
                Thread.Sleep(1000);
                WebContext.WebDriver.Url = string.Format("{0}{1}/Home", Configuration.GetSutUrl(), TestDefaults.Default.Path);
                Thread.Sleep(1000);
                WebContext.WebDriver.SwitchTo().Alert().Accept();
                //WebContext.WebDriver.Navigate().Refresh();
                //WebContext.WebDriver.SwitchTo().Alert().Accept();
                Wait.WaitLoading();
            }
        }

        /// <summary>
        /// Des: Scroll to element use action
        /// </summary>
        /// <param name="element"></param>
        public static void ScrollToByAction(this IWebElement element)
        {
            Actions actions = new Actions(WebContext.WebDriver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        public static void FindAndClickSingleGroupInTreeList(string autId, string groupName)
        {
            string locator = @".//*[@data-automation-id='{0}']/../../following-sibling::div//div[@data-checkbox-tree-list='']//label[text()='{1}']//preceding-sibling::input";
            WebContext.WebDriver.FindElement(By.XPath(string.Format(locator, autId, groupName))).Click();
        }
    }
}