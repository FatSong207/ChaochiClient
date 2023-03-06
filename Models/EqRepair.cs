
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using DapperExtensions.Mapper;

namespace Yuebon.Chaochi.Models
{
    /// <summary>
    /// ，數據實體對象
    /// </summary>
    [Table("Chaochi_EqRepair")]
    [Serializable]
    public class Chaochi_EqRepair
    {
        public string Id { get; set; }
        /// <summary>
        /// 報修流水編號
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 客戶身分證字號/統一編號 
        /// </summary>
        public string CustomerID { get; set; }


        /// <summary>
        /// 合約編號
        /// </summary>
        public string CID { get; set; }

        /// <summary>
        /// 建物地址 
        /// </summary>
        public string BAdd { get; set; }

        /// <summary>
        /// 報修人姓名 
        /// </summary>
        public string ReporterName { get; set; }

        /// <summary>
        /// 報修人電話
        /// </summary>
        public string ReporterCell { get; set; }

        /// <summary>
        /// 可修繕時間 
        /// </summary>
        public string RepairDateTime { get; set; }

        /// <summary>
        /// 修繕狀態 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 申請日期 
        /// </summary>
        public string ApplicationDate { get; set; }

        /// <summary>
        /// 結案日期 
        /// </summary>
        public string EndCaseDate { get; set; }

        public DateTime? CreatorTime { get ; set; }
        public string CreatorUserId { get; set ; }
        public string LastModifyUserId { get ; set; }
        public DateTime? LastModifyTime { get ; set; }
    }
}
