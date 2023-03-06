
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuebon.Chaochi.Models
{
    /// <summary>
    /// ，數據實體對象
    /// </summary>
    [Table("Chaochi_EqRepairDetail")]
    [Serializable]
    public class Chaochi_EqRepairDetail
    {
        public string Id { get; set; }

        /// <summary>
        /// 設置或獲取 
        /// </summary>
        public string EqRepairId { get; set; }

        /// <summary>
        /// 設備Id
        /// </summary>
        public string EqId { get; set; }

        /// <summary>
        /// 設置或獲取設備名稱
        /// </summary>
        public string EqName { get; set; }

        /// <summary>
        /// 設置或獲取設備品牌(規格)
        /// </summary>
        public string EqBrand { get; set; }

        /// <summary>
        /// 設置或獲取修繕原因
        /// </summary>
        public string RepairReason { get; set; }

        /// <summary>
        /// 設置或獲取處理備註
        /// </summary>
        public string HandleNote { get; set; }

        /// <summary>
        /// 設置或獲取當前狀態
        /// </summary>
        public string CurrentState { get; set; }

        /// <summary>
        /// 設置或獲取成本
        /// </summary>
        public string Cost { get; set; }

        /// <summary>
        /// 設置或獲取報價
        /// </summary>
        public string Quote { get; set; }

        /// <summary>
        /// 設置或獲取修繕廠商
        /// </summary>
        public string RepairCompany { get; set; }

        /// <summary>
        /// 設置或獲取修繕前近照
        /// </summary>
        public string BeforeRepairClose { get; set; }

        /// <summary>
        /// 設置或獲取修繕前遠照
        /// </summary>
        public string BeforeRepairFar { get; set; }

        /// <summary>
        /// 設置或獲取修繕中近照
        /// </summary>
        public string RepairingClose { get; set; }

        /// <summary>
        /// 設置或獲取修繕中遠照
        /// </summary>
        public string RepairingFar { get; set; }

        /// <summary>
        /// 設置或獲取修繕後近照
        /// </summary>
        public string AfterRepairClose { get; set; }

        /// <summary>
        /// 設置或獲取修繕後遠照
        /// </summary>
        public string AfterRepairFar { get; set; }

        /// <summary>
        /// 照片數量
        /// </summary>
        public string PhotoCount { get; set; }
    }
}
