using System;

namespace WebAdmin.Function.Helper
{
    public static class Helper
    {
        /// <summary>
        /// 生成隨機號
        /// </summary>
        /// <returns></returns>
        public static string CreateId()
        {
            Random random = new Random();
            string strRandom = random.Next(1000, 100000000).ToString();
            string code = DateTime.Now.ToString("yyyyMMddHHmmssffff") + strRandom;
            return code;
        }

        /// <summary>
        /// 生成Guid
        /// </summary>
        /// <returns></returns>
        public static string GuId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
