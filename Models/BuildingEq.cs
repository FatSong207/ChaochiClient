
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yuebon.Chaochi.Models
{
    /// <summary>
    /// 房屋物件，數據實體對象
    /// </summary>
    [Table("Chaochi_BuildingEq")]
    [Serializable]
    public class Chaochi_BuildingEq
    {
        /// <summary>
        /// 默認構造函數
        /// </summary>
        public Chaochi_BuildingEq()
        {

        }

        #region 建物設備相關

        public string Id { get; set; }
        public virtual string BAdd { get; set; }
        public virtual string  EqName { get; set; }
        public virtual string Category { get; set; }
        public virtual string EqCount { get; set; }
        public virtual string EqBrand { get; set; }

        #endregion 建物設備相關
    }
}