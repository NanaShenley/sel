using SeSugar.Interfaces;
using System;

namespace TestRunner.SeSugar
{
    public class SeSettings : ISettings
    {
        private Guid _automatedRunId = Guid.Empty;
        private string _seleneApiUrl = string.Empty;

        public TimeSpan DefaultTimeout
        {
            get { return TestSettings.BrowserDefaults.WaitLoading; }
        }

        public string DatabaseConnectionString
        {
            get { return TestSettings.Configuration.GetSutDbConnStr(); }
        }

        public TimeSpan AjaxRequestTimeout
        {
            get { return TestSettings.TestDefaults.Default.AjaxElementTimeOut; }
        }

        public TimeSpan ElementRetrievalTimeout
        {
            get { return TestSettings.TestDefaults.Default.TimeOut; }
        }

        public int TenantId
        {
            get { return TestSettings.TestDefaults.Default.TenantId; }
        }

        public Guid AutomatedRunId
        {
            get { return _automatedRunId; }
            set { _automatedRunId = value; }
        }

        public string SeleneApiUrl
        {
            get { return _seleneApiUrl; }
            set { _seleneApiUrl = value; }
        }
    }
}
