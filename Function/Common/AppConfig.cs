using System.Diagnostics;
using System.Set.Common;
using System.Set.Extensions;

namespace WebAdmin.Function
{
    /// <summary>
    /// 系統環境設定
    /// </summary>
    public static class AppConfig
    {
        public static bool IsTestSite = false;
        public static string Version = "1.0.0"; //版本號碼 (更新系統之前要考量是否變更版號)

        /// <summary>
        /// 系統參數初始化
        /// </summary>
        public static void Init()
        {
            App.DbLog = true;

            IsTestSite = Debugger.IsAttached || App.Setting("IsTestSite").IsTrue();
        }
    }
}
