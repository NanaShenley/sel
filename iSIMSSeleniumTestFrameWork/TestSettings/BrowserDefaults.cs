using System;
namespace TestSettings
{
    public static class Configuration
    {
        public const string lab_two = "lab-two";
        public const string lab_five = "lab-five";
        public const string lab_sims8 = "lab-sims8";

        public static string SystemUnderTest = "setting";
        public static bool ForceTestUserLogin = false;

        public static string GetSutUrl()
        {
            switch (SystemUnderTest.ToLowerInvariant())
            {
                case lab_two:
                    return TestDefaults.Default.LabTwoURL;
                case lab_five:
                    return TestDefaults.Default.LabFiveURL;
                case lab_sims8:
                    return TestDefaults.Default.LabSims8URL;
                default:
                    return TestDefaults.Default.URL;
            }
        }
        
        public static string GetSutDbConnStr()
        {
            switch (SystemUnderTest.ToLowerInvariant())
            {
                case lab_two:
                    return TestDefaults.Default.LabTwoDatabaseConnectionString;
                case lab_five:
                    return TestDefaults.Default.LabFiveDatabaseConnectionString;
                case lab_sims8:
                    return TestDefaults.Default.LabSims8DatabaseConnectionString;
                default:
                    return TestDefaults.Default.DatabaseConnectionString;
            }
        }
    }

    public static class BrowserDefaults
    {
        public const string Chrome = "chrome";
        public const string Ie = "internet explorer";
        public const string Firefox = "firefox";
        public const string Edge = "edge";
        public const string Safari = "safari";

#if DEBUG
        /// <summary>
        /// The default timeout does not allow debugging as the test times out (and fails) 
        /// before the developer can inspect the values etc.
        /// 
        /// This is the Debug mode timeout value; compile in Release mode to restore the shorter timeout value.
        /// </summary>
        public static TimeSpan TimeOut = TestDefaults.Default.TimeOutDebug;

        public static TimeSpan ElementTimeOut = TestDefaults.Default.ElementTimeOut;

        public static TimeSpan AjaxElementTimeOut = TestDefaults.Default.AjaxElementTimeOut;

        public static TimeSpan WaitLoading = TestDefaults.Default.WaitLoading;

        public static int ObjectTimeOut = TestDefaults.Default.ObjectTimeOut;

        public static string SchoolName = TestDefaults.Default.SchoolName;

#else
        /// <summary>
		/// This is the standard (Release mode) timeout value
		/// </summary>
		public static TimeSpan TimeOut = TestDefaults.Default.TimeOut;

        public static TimeSpan ElementTimeOut = TestDefaults.Default.ElementTimeOut;

        public static TimeSpan AjaxElementTimeOut = TestDefaults.Default.AjaxElementTimeOut;

        public static TimeSpan WaitLoading = TestDefaults.Default.WaitLoading;

        public static int ObjectTimeOut = TestDefaults.Default.ObjectTimeOut;
#endif
    }
}
