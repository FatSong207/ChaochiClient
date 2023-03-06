using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuebon.Chaochi.Models
{
    /// <summary>
    /// ，數據實體對象
    /// </summary>
    [Table("Chaochi_Complaint")]
    [Serializable]
    public class Chaochi_Complaint
    {
        public string Id { get; set; }
        /// <summary>
        /// 設置或獲取客訴單編號
        /// </summary>
        public string CCode { get; set; }

        /// <summary>
        /// 設置或獲取投訴人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 設置或獲取投訴人聯絡電話
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 設置或獲取投訴人聯絡信箱
        /// </summary>
        public string ContactMail { get; set; }

        /// <summary>
        /// 設置或獲取投訴人可連絡時段
        /// </summary>
        public string ContactDatetime { get; set; }

        /// <summary>
        /// 設置或獲取客訴類別
        /// </summary>
        public string ComplaintType { get; set; }

        /// <summary>
        /// 設置或獲取客訴部門/公司
        /// </summary>
        public string ComplaintDept { get; set; }

        /// <summary>
        /// 設置或獲取被客訴人
        /// </summary>
        public string Complainee { get; set; }

        /// <summary>
        /// 設置或獲取有糾紛的合約編號
        /// </summary>
        public string CId { get; set; }

        /// <summary>
        /// 設置或獲取客訴事由
        /// </summary>
        public string ComplaintReason { get; set; }

        /// <summary>
        /// 設置或獲取投訴人意見提供
        /// </summary>
        public string ProvideAdvice { get; set; }

        /// <summary>
        /// 設置或獲取單據狀態
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 設置或獲取客服人員
        /// </summary>
        public string HandleUser { get; set; }

        /// <summary>
        /// 設置或獲取填單日期
        /// </summary>
        public DateTime? ComplaintCreatorTime { get; set; }

        /// <summary>
        /// 設置或獲取送審日期
        /// </summary>
        public DateTime? SendTrialTime { get; set; }

        /// <summary>
        /// 設置或獲取結案日期
        /// </summary>
        public DateTime? EndCaseTime { get; set; }

        /// <summary>
        /// 設置或獲取客服人員處理備註
        /// </summary>
        public string HandleNote { get; set; }

        /// <summary>
        /// 設置或獲取累積發信次數
        /// </summary>
        public int? SendMailCount { get; set; }

        /// <summary>
        /// 設置或獲取 
        /// </summary>
        public DateTime? CreatorTime { get; set; }

        /// <summary>
        /// 設置或獲取 
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 設置或獲取 
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 設置或獲取 
        /// </summary>
        public string LastModifyUserId { get; set; }


    }
}
