
using SharedComponents.Helpers;
using TestSettings;

namespace Migration.POM.Helpers
{
    public class LoginHelper
    {
        public static void Login()
        {
            SeleniumHelper.EnterpriseLogin(
                        TestDefaults.Default.EnterpriseUser,
                        TestDefaults.Default.EnterprisePassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.EnterpriseName,
                        Configuration.GetSutUrl()
                        );
        }
    }
}
