﻿using SharedComponents.Helpers;
using System.Threading;
using iSIMSSeleniumHelper = SharedComponents.SeleniumHelperObsolete;

namespace SharedComponents.HomePages
{
    public static class ShellAction
    {
        public const string ShellMenuId = "#shell-menu";

        public static void OpenTaskMenu()
        {
            var cssFormat = string.Format("{0} {1}", ShellMenuId,SeleniumHelper.AutomationId("task_menu"));

            SeleniumHelper.FindAndClick(cssFormat);
           
            Thread.Sleep(400); //wait to open
        }
    }
}