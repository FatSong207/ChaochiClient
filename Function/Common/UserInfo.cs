using System;

namespace WebAdmin.Function.Common
{
    /// <summary>
    /// 登入者資訊
    /// </summary>
    [Serializable]
    public class UserRole
    {
        public string UnitId { get; set; } //單位名稱代碼
        public string AreaId { get; set; } //行政區代碼
        public string RoleId { get; set; } //管理者角色代碼
        public string UserId { get; set; } //使用者帳號
        public string CName { get; set; }  //使用者名稱
        public string Email { get; set; }
    }
}
