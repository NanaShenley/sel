using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
//using SharedComponents.HomePages;
using WebDriverRunner.webdriver;
using System.Diagnostics;
using TestSettings;
//using POM.Components.HomePages;
using OpenQA.Selenium.Interactions;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using OpenQA.Selenium.Remote;
using System.Text.RegularExpressions;
using System.Globalization;
using Staff.POM.Components.HomePages;
using System.Text.RegularExpressions;
using System.Threading;
using TestSettings;
using Staff.POM.Base;
using SeSugar.Automation;
using SeSugar.Interfaces;



namespace Staff.POM.Helper
{
    public static class SeleniumHelper//SeleniumHelper
    {
        private static readonly ILogger _logger = SeSugar.Environment.Logger;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int command);

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
            return element;
        }

        public static IWebElement GoToAccordionPanel(this IWebElement element, string title)
        {
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
            var updateSection = element.FindChild(By.CssSelector("[data-section-id=\"" + updateSectionId + "\"]"));

            var sender = GetWaiterForAriaBusyState(updateSection, updateSectionId, "false");
            var waiter = GetWaiterForAriaBusyState(updateSection, updateSectionId, "true");

            sender.Start();
            Debug.WriteLine(string.Format("ValueChange for \"{0}\" started.", updateSectionId));
            action.Invoke(element);
            sender.Join(BrowserDefaults.TimeOut);
            waiter.Start();
            Debug.WriteLine(string.Format("ValueChange for \"{0}\" in progress.", updateSectionId));
            waiter.Join(BrowserDefaults.TimeOut);
            Debug.WriteLine(string.Format("ValueChange for \"{0}\" complete.", updateSectionId));

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

        /// <summary>
        /// Au: Logigear
        /// Des: Get value of selected item on a combobox
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static String GetSelectedComboboxItemText(this IWebElement element)
        {
            String text = "";
            SelectElement select = new SelectElement(element);
            text = select.SelectedOption.Text;
            return text;
        }

        /// <summary>
        /// Au: Luong.Mai
        /// Des: Scroll to element use action
        /// </summary>
        /// <param name="element"></param>
        public static void ScrollToByAction(this IWebElement element)
        {
            Actions actions = new Actions(WebContext.WebDriver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Select an option in a combobox control by text
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        public static void SelectByText(this IWebElement element, String text)
        {
            SelectElement select = new SelectElement(element);
            select.SelectByText(text);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set "checked" for an checkbox
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ischeck"></param>
        public static void Set(this IWebElement element, Boolean ischeck)
        {
            bool check = IsChecked(element);
            if ((ischeck && !check) || (!ischeck && check))
            {
                element.ClickByJS();
            }

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a checkbox is checked
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false</returns>
        public static Boolean IsChecked(this IWebElement element)
        {
            return element.Selected;

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Clicking on an element use JavaScript
        /// </summary>
        /// <param name="element"></param>
        public static void ClickByJS(this IWebElement element)
        {
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// Au: Huy Vo
        /// Des: GetText method uses to get text from element
        /// <param name="element">Element contais text</param>
        /// <returns>Return string value</returns>
        public static String GetText(this IWebElement element)
        {
            return element.Text;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check an element is displayed.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false</returns>
        public static Boolean IsElementDisplayed(this IWebElement element)
        {
            Boolean isDisplayed = false;
            if (IsElementExists(element))
            {
                try
                {
                    isDisplayed = element.Displayed;
                }
                catch (Exception e)
                {
                    _logger.LogLine("isElementDisplayed:" + e.Message);
                }
            }
            return isDisplayed;
        }

        public static Boolean IsExist(this IWebElement element)
        {
            try
            {
                if (element.Displayed)
                {
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Au: Logigear
        /// Des: Clear content of a TextBox element or customize controls use TextBox
        /// such as: Combobox, TextArea
        /// </summary>
        /// <param name="element"></param>
        public static void ClearText(this IWebElement element)
        {
            try
            {
                element.SendKeys(Keys.Control + "a");
                element.SendKeys(Keys.Delete);
            }
            catch (Exception e)
            {
                _logger.LogLine("Have an exception when executing 'Send Keys' :" + e.ToString());
            }
        }


        /// <summary>
        /// Checks if an element exists on a page.
        /// </summary>
        /// <param name="thingToFind"></param>
        /// <returns>Returns TRUE if element exists, FALSE otherwise.</returns>
        public static bool DoesElementExist(By thingToFind)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
            return WebContext.WebDriver.FindElements(thingToFind).Count > 0;
        }

        public static void SetText(this IWebElement element, string value, By thingToWaitFor = null)
        {
            if (thingToWaitFor != null)
            {
                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
                wait.Until(ExpectedConditions.ElementToBeClickable(thingToWaitFor));
            }

            //Clear out the field using JS
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("arguments[0].value='';", element);
            element.SendKeys(value);
            element.SendKeys(Keys.Tab);
            AutomationSugar.WaitForAjaxCompletion();
        }

        public static void SetTextInUpdatePanel(string value, By thingToWaitFor, bool addSpace = true)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
            wait.Until(ExpectedConditions.ElementToBeClickable(thingToWaitFor));
            var element = WebContext.WebDriver.FindElement(thingToWaitFor);

            element.Click();
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("arguments[0].value='" + value + "';", element);
            if (addSpace)
                element.SendKeys(" ");

            int tries = 0;
            const int maxTries = 100;
            Retry.Do(() =>
            {
                if (++tries > 0)
                    _logger.LogLine("Waiting for Update Panel. Retry attempt {0} of {1}.", tries, maxTries);

                wait.Until(ExpectedConditions.ElementIsVisible(thingToWaitFor));
                wait.Until(ExpectedConditions.ElementToBeClickable(thingToWaitFor));

                element = WebContext.WebDriver.FindElement(thingToWaitFor);
                element.SendKeys(Keys.Tab);
            }, 10, maxTries);

            AutomationSugar.WaitForAjaxCompletion();
        }

        public static void TriggerChange(string inputElementNameAttrobuteValue)
        {
            Thread.Sleep(250);
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("$('[name=" + inputElementNameAttrobuteValue + "]').change();");
            Thread.Sleep(250);
        }

        public static void SetDateIntoField(this IWebElement element, string value)
        {
            try
            {
                element.ClearText();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                element.SetDateTime(value);
            }
            catch (Exception ex)
            {
                _logger.LogLine("Can't sendkeys to element" + ex.ToString());
            }
        }

        /// <summary>
        /// Au: Luong.Mai
        /// Des: Get value of web element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Enter a string into TextBox, TextArea... controls
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void EnterForDropDown(this IWebElement element, string value)
        {
            try
            {
                element.SendKeys(Keys.Enter);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                IWebElement inputElement = FindElement(SimsBy.CssSelector("#select2-drop input.select2-input"));
                inputElement.SendKeys(value);
                //Thread.Sleep(2000);
                inputElement.SendKeys(Keys.Enter);
            }
            catch (Exception ex)
            {
                _logger.LogLine("Have an exception happens when enter date for drop down: " + ex.ToString());
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Get a attribute of an element by att's name
        /// </summary>
        /// <param name="element"></param>
        /// <param name="att"></param>
        /// <returns></returns>
        public static String GetAttribute(this IWebElement element, String att)
        {
            try
            {
                return element.GetAttribute(att);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void SetAttribute(this IWebElement element, string att, string value)
        {
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("arguments[0].setAttribute('" + att + "', '" + value + "');", element);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a element is exists with default time out.
        /// Return false if it isn't existed
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false/throws a exception</returns>
        public static Boolean IsElementExists(this IWebElement element)
        {
            try
            {
                if (element != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                _logger.LogLine("isElementExists: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a element is exists with default time out.
        /// Return false if it isn't existed
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false/throws a exception</returns>
        public static Boolean IsElementExists(By element)
        {
            try
            {
                if (FindElement(element) != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                _logger.LogLine("isElementExists: " + e.Message);
                return false;
            }
        }

        /// <summary>
        ///Des: Double Click method uses to click on an element
        ///<param name="element">an element</param>
        /// <returns>Void</returns>
        public static void DoubleClick(this IWebElement e)
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.DoubleClick(e);
            action.Perform();
        }

        public static void ClickByAction(this IWebElement e)
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.Click(e);
            action.Perform();
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check an element is collapsed
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false</returns>
        public static Boolean IsCollapsed(this IWebElement element)
        {
            return element.GetAttribute("class").Contains("collapsed");
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Get a collection web element with time out is inputed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElements(By element)
        {
            Wait.WaitForElement(element);
            return WebContext.WebDriver.FindElements(element);
        }

        /// <summary>
        /// Au: Logigear:
        /// Des: Finding an web element with time out is inputed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static IWebElement FindElement(By element)
        {
            Wait.WaitForControl(element);
            return WebContext.WebDriver.FindElement(element);
        }

        public static IWebElement FindElementWithOutTimeout(By element)
        {
            return WebContext.WebDriver.FindElement(element);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Sleep a thread in a moment time (second)
        /// </summary>
        /// <param name="second"></param>
        public static void Sleep(int second)
        {
            try
            {
                Thread.Sleep(second * 1000);
            }
            catch (ThreadInterruptedException e)
            {
                _logger.LogLine(e.Message);
            }

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Finding a text in a page
        /// </summary>
        /// <param name="text"></param>
        /// <returns>true/false</returns>
        public static bool FindTextInPage(String text)
        {
            bool isFound = false;
            if (WebContext.WebDriver.PageSource.Contains(text))
            {
                isFound = true;
            }

            return isFound;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Get the parent tab of opened browser
        /// </summary>
        /// <returns></returns>
        public static string GetParentTab()
        {
            return WebContext.WebDriver.CurrentWindowHandle;
        }

        /// <summary>
        /// Au: Huy Vo
        /// Des: SwitchToNewTab method uses to switch to new tab that is opening on Browsers
        /// </summary>
        /// <param name="parentTab">parentTab: Current tab that being focused</param>
        public static void SwitchToNewTab(string parentTab)
        {
            string newTab = string.Empty;
            ReadOnlyCollection<string> windowHandles = WebContext.WebDriver.WindowHandles;
            foreach (string handle in windowHandles)
            {
                if (handle != parentTab)
                {
                    newTab = handle;
                    break;
                }
            }
            WebContext.WebDriver.SwitchTo().Window(newTab);
        }

        /// <summary>
        /// Au: Huy Vo
        /// Des: CloseTab uses to close current tab on IE, Chrome, Firefox
        /// </summary>
        public static void CloseTab()
        {
            WebContext.WebDriver.Close();
        }

        /// <summary>
        /// Author: Huy Vo
        /// Description: Switch to parent tab such as root tab on Browsers
        /// </summary>
        /// <param name="parentTab">parentTab: The first tab is opened on Browser</param>
        public static void SwitchToRootTab(string parentTab)
        {
            WebContext.WebDriver.SwitchTo().Window(parentTab);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Comparing two lists string together
        /// </summary>
        /// <param name="listRowElement1"></param>
        /// <param name="listRowElement2"></param>
        /// <returns>true/false</returns>
        public static bool DoesListItemEqual(List<string> listRowElement1, List<string> listRowElement2)
        {
            if (listRowElement1.Count != listRowElement2.Count)
                return false;

            for (int i = 0; i < listRowElement1.Count; i++)
                if (!listRowElement1[i].Equals(listRowElement2[i]))
                {
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Convert a collect IWebElement to list string
        /// </summary>
        /// <param name="webElementCollection"></param>
        /// <param name="listTextWebElement"></param>
        public static void ConvertToListItem(List<IWebElement> webElementCollection, ref List<string> listTextWebElement, bool isGetAttribute = false, string attribute = "value")
        {
            listTextWebElement.Clear();
            foreach (var item in webElementCollection)
            {
                if (!isGetAttribute)
                {
                    listTextWebElement.Add(item.Text.Trim());
                }
                else
                {
                    listTextWebElement.Add(item.GetAttribute(attribute).Trim());
                }
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a web element existed
        /// </summary>
        /// <returns></returns>
        public static bool DoesWebElementExist(By element)
        {
            bool isExist = false;
            try
            {
                isExist = FindElement(element) != null;
            }
            catch (Exception)
            {
                isExist = false;
            }

            return isExist;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a web element existed
        /// </summary>
        /// <returns></returns>
        public static bool DoesWebElementExist(IWebElement element)
        {
            bool isExisted = true;
            try
            {
                bool isDisplayed = element.Displayed;
            }
            catch (NoSuchElementException)
            {
                isExisted = false;
            }

            return isExisted;
        }

        /// <summary>
        /// Au: Logigear
        /// Des : Switch to window with name of windows. Name of windows will be contain at Automation.WebContext.WebDriver.WindowHandles
        /// </summary>
        /// <param name="windowsHandle">Name of windows which want to be switch</param>
        /// <exception>Throws NoSuchWindowException</exception>
        public static void SwitchToWindow(string windowHandle)
        {
            try
            {
                WebContext.WebDriver.SwitchTo().Window(windowHandle);
            }
            catch (NoSuchWindowException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Au : Logigear
        /// Des : Switch to new window opened and save current window before switch
        /// </summary>
        /// <param name="currentWindows">Contain currentWindow name before switch</param>
        /// <exception>Throws NoSuchWindowException</exception>
        public static void SwitchToNewWindow(out string currentWindow)
        {
            try
            {
                //Store the current window handle
                currentWindow = WebContext.WebDriver.CurrentWindowHandle;

                //Switch to new window opened
                foreach (string winHandle in WebContext.WebDriver.WindowHandles)
                {
                    SwitchToWindow(winHandle);
                }

            }
            catch (NoSuchWindowException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Execute a specifice javascript command.
        /// </summary>
        /// <param name="commandScript"></param>
        /// <returns></returns>
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
        /// Au: Logigear
        /// Des: Execute a specifice javascript command with element
        /// </summary>
        /// <param name="commandScript"></param>
        /// <returns></returns>
        public static string ExecuteJavascript(string commandScript, By element)
        {
            try
            {
                IWebDriver webDriver = WebContext.WebDriver;
                IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

                var result = js.ExecuteScript(commandScript, Get(element));
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
        /// Au : Logigear
        /// Des : Switch to new window opened
        /// </summary>
        /// <exception>Throws NoSuchWindowException</exception>
        public static void SwitchToNewWindow()
        {
            try
            {
                //Switch to new window opened
                foreach (string winHandle in WebContext.WebDriver.WindowHandles)
                {
                    SwitchToWindow(winHandle);
                }
            }
            catch (NoSuchWindowException e)
            {
                throw e;
            }
        }

        public static string GetCurrentWindow()
        {
            try
            {
                return WebContext.WebDriver.CurrentWindowHandle;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void SelectDropDownListItem(By elementListItem, string value)
        {
            ReadOnlyCollection<IWebElement> listItme = FindElements(elementListItem);
            foreach (IWebElement item in listItme)
            {
                if (item.Text.Equals(value))
                {

                    item.Click();
                    return;
                }
            }

        }

        public static void SelectElemetAtIndex(By elementListItem, int index)
        {
            ReadOnlyCollection<IWebElement> listItem = FindElements(elementListItem);

            if (listItem.Any())
            {

                listItem[index].Click();
                return;
            }

        }

        public static void SendKeyToElement(By element, string p)
        {
            try
            {
                if (p.Equals("enter"))
                {
                    Get(element).SendKeys(Keys.Enter);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set Date Time for Date Time textbox
        /// </summary>
        /// <param name="element">Date Time textbox</param>
        /// <param name="value">valute</param>
        public static void SetDateTime(this IWebElement element, string value, bool formatTime = false, bool timeOnly = false)
        {
            string elementFormat = Regex.Replace(element.GetAttribute("data-date-validator-format"), "a|A", "tt");
            if (!formatTime)
            {
                element.SetText(DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString(elementFormat));
            }
            else
            {
                if (timeOnly)
                {
                    element.SetText(DateTime.ParseExact(value, "HH:mm", CultureInfo.InvariantCulture).ToString(elementFormat));
                }
                else
                {
                    element.SetText(DateTime.ParseExact(value, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture).ToString(elementFormat));
                }
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set Date Time for Date Time textbox
        /// </summary>
        /// <param name="element">Date Time textbox</param>
        /// <param name="value">valute</param>
        public static void SetDateTimeByJS(this IWebElement element, string value, bool formatTime = false)
        {
            string elementFormat = Regex.Replace(element.GetAttribute("data-date-validator-format"), "a|A", "tt");
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("arguments[0].value='';", element);
            if (!formatTime)
            {
                element.SetText(DateTime.ParseExact(value, "M/d/yyyy", CultureInfo.InvariantCulture).ToString(elementFormat));
            }
            else
            {
                element.SetText(DateTime.ParseExact(value, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString(elementFormat));
            }
            element.SendKeys(Keys.Tab);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set Date Time for Date Time textbox
        /// </summary>
        /// <param name="element">Date Time textbox</param>
        /// <param name="formatTime">contains time or not</param>
        public static string GetDateTime(this IWebElement element, bool formatTime = false, bool timeOnly = false)
        {
            string elementFormat = Regex.Replace(element.GetAttribute("data-date-validator-format"), "a|A", "tt");
            string value = element.GetAttribute("value");
            if (String.IsNullOrEmpty(value))
            {
                value = element.GetText();
            }
            return Format(value, elementFormat, formatTime, timeOnly);
        }

        public static void MinimizeAll()
        {
            System.Diagnostics.Process thisProcess =
               System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.Process[] processes =
               System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process process in processes)
            {
                if (process.MainWindowTitle.Contains("SIMS")) continue;
                System.IntPtr handle = process.MainWindowHandle;
                if (handle == System.IntPtr.Zero) continue;
                // Minimize all window
                ShowWindow(handle, 6);

            } //loop
        } //MinimizeAll

        public static string GenerateRandomString(int length)
        {
            Random _random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        public static string GenerateRandomNumberUsingDateTime(int length = 0)
        {
            int randomNumber = Math.Abs((int)DateTime.Now.Ticks);
            if (length == 0)
            {
                return randomNumber.ToString();
            }
            string time = randomNumber.ToString();
            string rand = time.Substring(time.Length - 1 - length, length);
            return rand;
        }

        public static int GenerateRandomNumber(int max)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(max);
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

        /// <summary>
        /// Clicks on one element, waits for another to load. Typically used when areas of the DOM are being replaced. For example, saving and reloading a record.
        /// Using Javascript
        /// </summary>
        /// <param name="elementToClick">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="elementToWaitFor">A selector describing which element to wait until it's loaded. Scoped to the document root.</param>
        /// <returns>The element you've been waiting for.</returns>
        public static IWebElement ClickAndWaitForByJS(By elementToClick, By elementToWaitFor)
        {
            var toClick = Get(elementToClick);
            toClick.ClickByJS();

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

        public static ReadOnlyCollection<IWebElement> GetGridRows(this IWebElement element, By gridSelector)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            element.WaitUntilState(ElementState.Displayed);
            var wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> rowCollection = wait.Until(x => x
                .FindElement(gridSelector)
                .WaitUntilState(ElementState.Displayed)
                .FindElements(By.CssSelector(gridRowCss)));

            return rowCollection;
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
            selectorElement.WaitUntilState(ElementState.Displayed);
            selectorElement.Click();

            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";

            var selectorSearchElement = ElementRetriever.GetOnceLoaded(By.CssSelector(selectorSearchBox));
            selectorSearchElement.WaitUntilState(ElementState.Displayed);
            selectorSearchElement.Click();
            selectorSearchElement.Clear();
            selectorSearchElement.SendKeys(option);
            selectorSearchElement.SendKeys(Keys.Tab);
            return element;
        }

        public static IWebElement ChooseSelectorOption(this IWebElement element, string option)
        {
            element.WaitUntilState(ElementState.Displayed);
            Retry.Do(element.Click);
            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";
            var selectorSearchElement = ElementRetriever.GetOnceLoaded(By.CssSelector(selectorSearchBox));
            selectorSearchElement.WaitUntilState(ElementState.Displayed);
            selectorSearchElement.Click();
            selectorSearchElement.Clear();
            selectorSearchElement.SendKeys(option);
            selectorSearchElement.SendKeys(Keys.Tab);
            return element;
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
        //public static IWebElement SetText(this IWebElement element, By selector, string text)
        //{
        //    return SetText(element.FindChild(selector), text);
        //}

        //public static IWebElement SetText(this IWebElement element, string text)
        //{
        //    element.WaitUntilState(ElementState.Displayed);
        //    element.Clear();
        //    element.SendKeys(text);
        //    element.WaitUntilCondition(x => (x.GetAttribute("value") ?? String.Empty) == text);
        //    element.SendKeys(Keys.Tab);

        //    return element;
        //}

        ///// <summary>
        ///// Sets the Date and Time in a DateTime control.
        ///// </summary>
        ///// <param name="element">The containing element.</param>
        ///// <param name="selector">A selector describing the retrieval of the DateTime control. Scoped by the element this method is called upon.</param>
        ///// <param name="dateTime">The value to enter into the DateTime control.</param>
        ///// <returns></returns>
        //public static IWebElement SetDateTime(this IWebElement element, By selector, string dateTime)
        //{
        //    //TODO use the date picker component.
        //    element.SetText(selector, dateTime);
        //    return element;
        //}

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

        ///// <summary>
        ///// Returns the value of the element
        ///// </summary>
        ///// <param name="element">The containing element.</param>
        ///// <returns></returns>
        //public static string GetValue(this IWebElement element)
        //{
        //    return element.GetAttribute("value") ?? String.Empty;
        //}

        public static void Type(this IWebElement element, string value)
        {
            try
            {
                foreach (char c in value)
                {
                    element.SendKeys(c.ToString());
                    Thread.Sleep(400);
                }
            }
            catch (Exception ex)
            {
                _logger.LogLine("Can't sendkeys to element" + ex.ToString());
            }
        }

        #endregion

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
        #endregion

        #region Navigation
        /// <summary>
        /// Navigates to the page via the side menu
        /// </summary>
        /// <param name="topLevel">e.g.: Tasks or Lookups</param>
        /// <param name="category">e.g.: Staff</param>
        /// <param name="item">e.g: Service Terms</param>
        public static void NavigateMenu(string topLevel, string category, string item)
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
                        var l2Header = l2Item.FindElementSafe(By.CssSelector(".accordion-toggle"));
                        if (l2Header.Text.Trim().Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            if (l2Header.GetAttribute("aria-expanded").Trim().ToLower().Equals("false"))
                            {
                                l2Header.ClickByJS();
                                Thread.Sleep(1000);
                            }
                            var level3 = l2Item.FindElementsSafe(By.CssSelector(".shell-task-menu-item-title")); //changed based on the menu item Title
                            foreach (var l3Item in level3)
                            {
                                if (l3Item.Text.Trim().Equals(item))
                                {
                                    l3Item.ClickByJS();
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }

        /// <summary>
        /// Navigate to menu item by enter menu item into search textbox
        /// </summary>
        /// <param name="itemMenu"></param>
        public static void NavigateBySearch(string itemMenu, bool wait = false)
        {
            FindAndClick(By.CssSelector("[title=\"Task Menu\"]"));
            var searchTextbox = SeleniumHelper.Get(SimsBy.Id("task-menu-search"));
            if (wait)
            {
                SeleniumHelper.Type(searchTextbox, itemMenu);
            }
            else
            {
                searchTextbox.Clear();
                searchTextbox.SendKeys(itemMenu);
            }
            IWebElement taskResult = null;
            IReadOnlyCollection<IWebElement> taskResults = FindElements(SimsBy.CssSelector(".task-menu-dropdown .task-menu-suggestion"));

            foreach (var item in taskResults)
            {
                IWebElement resultTitle = item.FindElement(SimsBy.CssSelector(".h1-result"));
                if (resultTitle.Text.Trim().ToLower().Equals(itemMenu.ToLower()))
                {
                    taskResult = item.FindElement(SimsBy.CssSelector(".search-result-tile-detail"));
                    break;
                }
            }

            if (taskResult == null)
            {
                throw new NoSuchElementException("Menu for " + itemMenu + "is not exist");
            }

            taskResult.Click();
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }


        public static T NavigateViaAction<T>(string actionName) where T : BaseComponent, new()
        {
            if (SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[title='Actions'] a")))
            {
                if (SeleniumHelper.FindElement(SimsBy.CssSelector("[title='Actions'] a")).Displayed)
                {
                    Get(SimsBy.CssSelector("[title='Actions'] a")).ClickByJS();
                    Wait.WaitUntilDisplayed(By.CssSelector("[data-section-id='contextual-actions']"));
                }
            }

            Wait.WaitForAndClick(BrowserDefaults.ElementTimeOut, SimsBy.CssSelector(String.Format("[data-loading-text='{0}']", actionName)));
            Wait.WaitForAjaxReady(By.Id("nprogress"));

            var pageDetail = default(T);

            Retry.Do(() =>
            {
                pageDetail = new T();
            });

            pageDetail.Refresh();
            return pageDetail;
        }

        /// <summary>
        /// Navigates to the page via the Quick Link
        /// </summary>
        /// <param name="item">e.g: Pupil Records</param>
        public static void NavigateQuickLink(string item)
        {
            IReadOnlyCollection<IWebElement> quickLinks = FindElements(SimsBy.CssSelector("#quick-links a"));
            foreach (var quickLink in quickLinks)
            {
                if (quickLink.GetText().Trim().Equals(item))
                {
                    quickLink.ClickByJS();
                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    break;
                }
            }
        }

        /// <summary>
        /// Close tab
        /// </summary>
        /// <param name="tabName">name of tab which want to close</param>
        public static void CloseTab(string tabName)
        {
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    IWebElement closeButton = tab.FindElement(By.CssSelector(".tab-close"));
                    Retry.Do(closeButton.Click);
                    Wait.WaitForElementReady(By.Id("nprogress"));
                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    SeleniumHelper.Sleep(2);
                    break;
                }
            }
        }

        /// <summary>
        /// Select tab
        /// </summary>
        /// <param name="tabName">name of tab which want to close</param>
        public static void SelectTab(string tabName)
        {
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    Retry.Do(tab.Click);
                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    break;
                }
            }
        }

        /// <summary>
        /// Check the tab is displayed.
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public static bool IsTabDisplay(string tabName)
        {
            Sleep(5);
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

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
        /// Log in as a given user.
        /// </summary>
        /// <param name="userType">The user type to login as.</param>
        /// <param name="enabledFeatures">The FeatureBee features to enable.</param>
        public static void Login(SeleniumHelper.iSIMSUserType userType, params string[] enabledFeatures)
        {
            if (TestSettings.Configuration.ForceTestUserLogin) userType = iSIMSUserType.TestUser;

            switch (userType)
            {
                case iSIMSUserType.TestUser:
                    Login(
                        TestDefaults.Default.TestUser,
                        TestDefaults.Default.TestUserPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        true,
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
                default:
                    throw new ArgumentOutOfRangeException("userType");
            }
        }

        /// <summary>
        /// Log in to iSIMS.
        /// </summary>
        public static void Login(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection, params string[] enabledFeatures)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            HomePage homePage = HomePage.NavigateTo(user, password, tenantId, schoolName, url, schoolSelection, enabledFeatures);
            /*
            if (schoolSelection)
                homePage.MenuBar().WaitFor();
             */
            //Wait.WaitForDocumentReady();
        }

        public static void Logout()
        {
            HomePage.SignOut();
        }
        #endregion

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
                var groupHeaderId = groupHeader.GetAttribute("data-parent");

                if (groupHeader.Text == checkBoxGroupText)
                {
                    var checkboxContainer =
                        checkBoxElement.FindElements(By.CssSelector("label[data-checkbox-container='" + groupHeaderId + "']"));

                    foreach (var checkbox in checkboxContainer.Where(cb => checkBoxText.Any(cb.Text.Contains)))
                    {
                        checkbox.Click();
                    }
                }
            }
        }
        #endregion

        #region Find and Click
        /// <summary>
        /// Find an element and click it.
        /// </summary>
        /// <param name="SeleniumHelper">A selector describing which element to click. Scoped to the document root.</param>
        public static void FindAndClick(By SeleniumHelper)
        {
            //TODO Use ElementRetrieval helper methods.
            Wait.WaitForAndClick(BrowserDefaults.TimeOut, SeleniumHelper);
        }

        /// <summary>
        /// Find an element and click it.
        /// </summary>
        /// <param name="SeleniumHelper">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="timeout">The longest time to wait for the action to be completed.</param>
        public static void FindAndClick(By SeleniumHelper, TimeSpan timeout)
        {
            //TODO Use ElementRetrieval helper methods.
            Wait.WaitForAndClick(timeout, SeleniumHelper);
        }

        /// <summary>
        /// Find and element with a specific part-name concatenated to main selector
        /// </summary>
        /// <param name="SeleniumHelper"></param>
        /// <param name="cssElements"></param>
        public static void FindAndClick(string SeleniumHelper, params object[] cssElements)
        {
            var link = By.CssSelector(string.Format(SeleniumHelper, cssElements));
            FindAndClick(link);
        }
        #endregion

        #region Other

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Au: Logigear
        /// Des: Get a current time
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static string Format(string value)
        {
            string date = String.Empty;
            if (!String.IsNullOrEmpty(value))
            {
                var dateTime = DateTime.Parse(value);
                date = dateTime.ToString("M/d/yyyy");
            }
            return date;
        }

        public static string Format(string value, bool formatTime = false)
        {
            if (!String.IsNullOrEmpty(value))
            {
                var dateTime = DateTime.Parse(value);
                if (formatTime)
                {
                    return dateTime.ToString("M/d/yyyy h:mm tt");
                }
                else
                {
                    return dateTime.ToString("M/d/yyyy");
                }
            }
            else { return String.Empty; }
        }

        public static string Format(string value, string valueFormat, bool formatTime = false, bool timeOnly = false)
        {
            if (!String.IsNullOrEmpty(value))
            {
                var dateTime = DateTime.ParseExact(value, valueFormat, CultureInfo.InvariantCulture);
                if (formatTime)
                {
                    if (timeOnly)
                    {
                        return dateTime.ToString("h:mm tt");
                    }
                    else
                    {
                        return dateTime.ToString("M/d/yyyy h:mm tt");
                    }
                }
                else
                {
                    return dateTime.ToString("M/d/yyyy");
                }
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Get First day of week (use for create data provider)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            int range = DayOfWeek.Monday - date.DayOfWeek;
            return date.AddDays(range);
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Get Last day of week (use for create data provider)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(DateTime date)
        {
            int range = date.DayOfWeek - DayOfWeek.Sunday;
            return date.AddDays(7 - range);
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get the last day of current month
        /// </summary>
        /// <returns></returns>
        public static string GetLastDayOfMonth()
        {
            DateTime now = DateTime.Now;
            int lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);
            return new DateTime(now.Year, now.Month, lastDayOfMonth).ToString("M/d/yyyy");
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get the day before today
        /// </summary>
        /// <returns></returns>
        public static string GetDayBeforeToday()
        {
            return DateTime.Today.AddDays(-1).ToString("M/d/yyyy");
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get today
        /// </summary>
        /// <returns></returns>
        public static string GetToDay()
        {
            DateTime now = DateTime.Now;
            return now.ToString("M/d/yyyy");
        }

        /// <summary>
        /// Au: Hieu Pham
        /// Get the day of week in the next week or current week
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static string GetDateFromDayOfWeek(int dayOfWeek, bool isCurrentWeek = false)
        {
            DateTime today = DateTime.Today;
            int daysUntilDestinationDay = (dayOfWeek - (int)today.DayOfWeek) % 7;
            if (isCurrentWeek)
            {
                return today.AddDays(daysUntilDestinationDay).ToString("M/d/yyyy");
            }
            return today.AddDays(daysUntilDestinationDay + 7).ToString("M/d/yyyy");
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get the day after or before current day in a number of days. Format input date required M/d/yyyy
        /// </summary>
        /// <param name="currentDate"></param> Current Date 
        /// <param name="numberOfDay"></param> Negative if before, positive if after.
        /// <returns></returns>
        public static string GetDayAfter(string currentDate, int numberOfDay)
        {
            DateTime currentDateTime = DateTime.ParseExact(currentDate, "M/d/yyyy", CultureInfo.InvariantCulture);
            return currentDateTime.AddDays(numberOfDay).ToString("M/d/yyyy");
        }

        /// <summary>
        /// Author: Huy Vo
        /// Des: Get years and months from birthday to now
        /// </summary>
        /// <param name="birthDate"></param>
        /// <param name="now"></param>


        public static string GetMonthsAndYears(this string dob)
        {
            string pattern = "M/d/yyyy";
            DateTime birthday = DateTime.ParseExact(dob, pattern, CultureInfo.InvariantCulture);
            DateTime dt = DateTime.Today;

            int days = dt.Day - birthday.Day;
            if (days < 0)
            {
                dt = dt.AddMonths(-1);
                days += DateTime.DaysInMonth(dt.Year, dt.Month);
            }

            int months = dt.Month - birthday.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            int years = dt.Year - birthday.Year;

            return string.Format("{0} Year{1} {2} Month{3}",
                                 years, (years == 1) ? "" : "s",
                                 months, (months == 1) ? "" : "(s)");

        }
        #endregion

        #region Webix Functions

        /// <summary>
        /// Au: Logigear
        /// Des: Scroll Down the specific cell in table grid until it is visible
        /// Only use with custom table (Webx DataTable) in Capita
        /// </summary>
        /// <param name="XpathElement"></param>
        /// <returns></returns>
        public static bool ScrollDownUntilName(IWebElement tableElement, string name)
        {
            try
            {
                bool doesRepeatElementList = false;
                List<IWebElement> ListWebElementFirst = tableElement.FindElements(By.CssSelector(".webix_ss_left .read-only .webix_cell")).ToList();
                List<string> webElementsFirst = new List<string>();
                ConvertToListItem(ListWebElementFirst, ref webElementsFirst);

                List<IWebElement> ListWebElementSecond;
                List<string> webElementsSecond = new List<string>();
                webElementsSecond.AddRange(webElementsFirst);

                bool isElementPresent = webElementsSecond.Contains(name);

                int currentPosition = 100;
                int stepScroll = 0;
                string scripCommand = "$$(document.getElementsByClassName(\"webix_ss_vscroll webix_vscroll_y\")[0]).scrollTo({0},{1})";

                while (isElementPresent == false && doesRepeatElementList == false)
                {
                    ExecuteJavascript(String.Format(scripCommand, stepScroll, currentPosition));
                    currentPosition += 100;
                    stepScroll += 100;

                    webElementsFirst.Clear();
                    webElementsFirst.AddRange(webElementsSecond);

                    ListWebElementSecond = FindElements(By.CssSelector(".webix_ss_left .read-only .webix_cell")).ToList();
                    ConvertToListItem(ListWebElementSecond, ref webElementsSecond);
                    doesRepeatElementList = DoesListItemEqual(webElementsFirst, webElementsSecond);
                    isElementPresent = webElementsSecond.Contains(name);
                }
                return isElementPresent;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Scroll Right the specific cell in table grid until it is visible
        /// Only use with custom table (Webx DataTable) in Capita
        /// </summary>
        /// <param name="XpathElement"></param>
        /// <returns></returns>
        public static bool ScrollRightUntilExist(IWebElement tableElement, By element)
        {
            bool isElementPresent = IsExist(FindElement(element));
            int currentPosition = 0;
            int stepScroll = 100;
            string scripCommand = "$$(document.getElementsByClassName(\"webix_ss_hscroll webix_vscroll_x\")[0]).scrollTo({0},{1})";

            while (isElementPresent == false)
            {
                ExecuteJavascript(String.Format(scripCommand, stepScroll, currentPosition));
                currentPosition += 100;
                stepScroll += 100;

                isElementPresent = IsExist(FindElement(element));
            }
            return isElementPresent;
        }
        #endregion
    }
}
