using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Set.Common;
using System.Set.Extensions;
using System.Threading.Tasks;
using WebAdmin.Dtos;
using WebAdmin.Function.Helper;
using Yuebon.Chaochi.Models;

namespace WebAdmin.Controllers
{
    public class EqRepairController : BaseController
    {
        /// <summary>
        /// 修繕通報
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增/更新
        /// </summary>
        /// <param name="vt">頁面參數</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var tags = vt.Tags;
            var badd = tags.Value("C1");
            var tempTags = new Dictionary<string, string>();
            foreach (var tag in tags) {
                if (CheckData(tag)) {
                    tempTags[$"{tag.Key}"] = tag.Value;
                }
            }


            var db = Db.Orm();

            var cid = db.Query<string>($"select CID from Chaochi_Contract where RNID = '{tempTags.Value("CustomerId")}' and BAdd = '{badd}'").FirstOrDefault();
            var repairDateTime =Convert.ToDateTime(tempTags.Value("RepairDate") +" "+ tempTags.Value("RepairTime")+":00");

            var eqRepair = new Chaochi_EqRepair() {
                Id = Helper.CreateId(),
                Guid = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                CID = cid,
                CustomerID = tempTags.Value("CustomerId"),
                BAdd = badd,
                ReporterName = tags.Value("C2"),
                ReporterCell = tags.Value("C3"),
                RepairDateTime = repairDateTime.ToString(),
                State = "申請中",
                ApplicationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };
            try {
                using (var tran = db.Connection.BeginTransaction()) {
                    var success = db.Insert(eqRepair, tran);
                    var eqList = db.Query<string>($"select Id from Chaochi_BuildingEq where BAdd = '{badd.FixSql()}'", transaction: tran);
                    foreach (var item in tempTags) {
                        if (!eqList.Any(x => x == item.Key) && !item.Key.StartsWith("NINC")) {
                            tempTags.Remove(item.Key);
                        }
                    }
                    if (tempTags.Count == 0) {
                        rtn[1] = "error";
                        tran.Rollback();
                        tran.Dispose();
                        return Json(rtn);
                    }
                    foreach (var item in tempTags) {
                        if (!CheckRequiredData(item.Value)) {
                            rtn[1] = "error";
                            tran.Rollback();
                            tran.Dispose();
                            return Json(rtn);
                        }
                    }
                    var insertList = new List<Chaochi_EqRepairDetail>();
                    foreach (var item2 in tempTags) {
                        if (item2.Key.StartsWith("NINC")) {
                            var value = item2.Value.Split(',');
                            var eqName = value[0];
                            var repairReason = value[1];
                            var beforeClose = value[2] == "" ? "" : value[2].Split("\\")[2];
                            var beforeFar = value[3] == "" ? "" : value[3].Split("\\")[2];
                            var erRepairDetail = new Chaochi_EqRepairDetail() {
                                Id = Helper.CreateId(),
                                EqRepairId = eqRepair.Id,
                                EqId = item2.Key,
                                EqName = eqName,
                                EqBrand = "",
                                RepairReason = repairReason,
                                CurrentState = "未處理",
                                BeforeRepairClose = beforeClose == "" ? "" : beforeClose.Insert(beforeClose.IndexOf("."), "維修前近照"),
                                BeforeRepairFar = beforeFar == "" ? "" : beforeFar.Insert(beforeFar.IndexOf("."), "維修前遠照"),
                                PhotoCount = beforeClose != "" ? beforeFar != "" ? "2" : "1" : "0"
                            };
                            insertList.Add(erRepairDetail);
                        } else {
                            var buildingEq = db.Get<Chaochi_BuildingEq>($"select * from Chaochi_BuildingEq where Id = '{item2.Key.FixSql()}'", transaction: tran);
                            var value = item2.Value.Split(",");
                            var repairReason = value[0];
                            var beforeClose = value[1] == "" ? "" : value[1].Split("\\")[2];
                            var beforeFar = value[2] == "" ? "" : value[2].Split("\\")[2];
                            var erRepairDetail = new Chaochi_EqRepairDetail() {
                                Id = Helper.CreateId(),
                                EqRepairId = eqRepair.Id,
                                EqId = item2.Key,
                                EqName = buildingEq.EqName,
                                EqBrand = buildingEq.EqBrand,
                                RepairReason = repairReason,
                                CurrentState = "未處理",
                                BeforeRepairClose = beforeClose == "" ? "" : beforeClose.Insert(beforeClose.IndexOf("."), "維修前近照"),
                                BeforeRepairFar = beforeFar == "" ? "" : beforeFar.Insert(beforeFar.IndexOf("."), "維修前遠照"),
                                PhotoCount = beforeClose != "" ? beforeFar != "" ? "2" : "1" : "0"
                            };
                            insertList.Add(erRepairDetail);
                        }

                    }
                    var success2 = db.Insert((IEnumerable<Chaochi_EqRepairDetail>)insertList, tran);
                    if (success && success2) {
                        try {
                            foreach (var item3 in insertList) {
                                if (!string.IsNullOrEmpty(item3.BeforeRepairClose)) {
                                    var sourceDir = @$"D:\zzz\image\EqRepair\temp\{item3.EqId}";
                                    var destDir = @$"D:\zzz\image\EqRepair\{item3.EqRepairId}\{item3.Id}";
                                    if (!Directory.Exists(destDir)) {
                                        Directory.CreateDirectory(destDir);
                                    }
                                    System.IO.File.Copy(Path.Combine(sourceDir, item3.BeforeRepairClose), Path.Combine(destDir, item3.BeforeRepairClose), true);
                                    if (System.IO.File.Exists(Path.Combine(destDir, item3.BeforeRepairClose))) {
                                        System.IO.File.Delete(Path.Combine(sourceDir, item3.BeforeRepairClose));
                                    }
                                }
                                if (!string.IsNullOrEmpty(item3.BeforeRepairFar)) {
                                    var sourceDir = @$"D:\zzz\image\EqRepair\temp\{item3.EqId}";
                                    var destDir = @$"D:\zzz\image\EqRepair\{item3.EqRepairId}\{item3.Id}";
                                    if (!Directory.Exists(destDir)) {
                                        Directory.CreateDirectory(destDir);
                                    }
                                    System.IO.File.Copy(Path.Combine(sourceDir, item3.BeforeRepairFar), Path.Combine(destDir, item3.BeforeRepairFar), true);
                                    if (System.IO.File.Exists(Path.Combine(destDir, item3.BeforeRepairClose))) {
                                        System.IO.File.Delete(Path.Combine(sourceDir, item3.BeforeRepairClose));
                                    }
                                }
                            }
                            tran.Commit();
                        } catch (Exception) {
                            tran.Rollback();
                            throw;
                        }

                    }
                }
            } catch (Exception) {
                throw;
            }
            return Json(rtn);
        }

        /// <summary>
        /// 申請時下方table
        /// </summary>
        /// <param name="vt">頁面參數</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Info(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var p = new PutInfo();
            var tags = vt.Tags;
            var address = MergeAdd(tags);

            p.AreaId(".modal-content .dialog-form");

            var dr = Db.GetRow("Chaochi_EqRepair", new { Id = tags.Value("SN").FixSql() });

            if (dr != null) {
                address = dr["BAdd"].ToString();
                p.Add("C1", "r", address);
                p.Add("C2", "", dr["ReporterName"]);
                p.Add("C3", "", dr["ReporterCell"]);
            } else {
                p.Add("C1", "r", MergeAdd(tags));
            }

            var db = Db.Orm();
            string sql = " select  * ";
            sql += " from ";
            sql += "    Chaochi_BuildingEq b with (nolock) ";
            sql += " where ";
            sql += $"    b.BAdd = '{address}' ";
            sql += "    and ( b.EqCount is not null and b.EqCount != '' and b.EqCount != 0) ";
            var list = db.Query<EqRepairDetailsDto>(sql).ToList();

            p.View("#ListPage2", "_TableViewEqRepairDetails", list.Count == 0 ? new List<EqRepairDetailsDto>() : list);


            rtn[1] = p.Build();
            return Json(rtn);
        }

        /// <summary>
        /// Detail下方table
        /// </summary>
        /// <param name="vt">頁面參數</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Info2(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var p = new PutInfo();
            var tags = vt.Tags;

            var dr = Db.Query($"select b.Id,b.EqRepairId,b.EqName,b.CurrentState,b.BeforeRepairClose,b.BeforeRepairClose,b.BeforeRepairFar,b.RepairingClose,b.RepairingFar,b.AfterRepairClose,b.AfterRepairFar from Chaochi_EqRepair a with (nolock) join Chaochi_EqRepairDetail b with (nolock) on a.Id = b.EqRepairId where a.guid ='{tags.Value("guid").FixSql()}'");
            p.View("#DetailPage", "_TableVIewDetailImg", dr);

            rtn[1] = p.Build();
            return Json(rtn);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="vt">頁面參數</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListProvider(VarTags vt)
        {
            var rtn = new string[] { "", vt.Tags.Value("TagId"), "", "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var p = new PutInfo();
            var tags = vt.Tags.FixSql();
            string sql = "";
            string address = MergeAdd(tags);

            var isExists = Db.Exists($"select * from Chaochi_Contract where RNID = '{tags.Value("B1").FixSql()}' and BAdd = '{address.FixSql()}' and CStatus = '效期中'");
            if (isExists) {
                p.Show("#apply");
                var eqRepairIds = Db.GetArray("Id", "Chaochi_EqRepair", new { CustomerId = tags.Value("B1").FixSql(), BAdd = address });
                sql += " select distinct ";
                sql += "    a.guid, ";
                sql += "    C.CID, ";
                sql += "    a.BAdd, ";
                sql += "    a.ApplicationDate, ";
                sql += "    a.State, ";
                sql += "    a.EndCaseDate, ";
                sql += "    EqRepairId, ";
                sql += "    (select cast(EqName as NVARCHAR) + ',' from Chaochi_EqRepairDetail where EqRepairId=eqD.EqRepairId FOR XML PATH ('')) as EqName ";
                sql += " from ";
                sql += "    Chaochi_EqRepairDetail eqD with (nolock) ";
                sql += " join ";
                sql += "    Chaochi_EqRepair a with (nolock) on a.Id = eqD.EqRepairId ";
                sql += " join ";
                sql += "    Chaochi_Contract c with (nolock) on a.CustomerID = c.RNID and a.BAdd = c.BAdd ";
                sql += " where ";
                sql += $"    EqRepairId in ('{eqRepairIds.Join("','")}') ";
            } else {
                p.Show("#apply", false);
                rtn[0] = "非效期中客戶";

            }
            var pi = new PageInfo(ViewBag);

            pi.Init(tags, sql);
            rtn[2] = pi.TableView("_TableView1");
            rtn[3] = p.Build();

            return Json(rtn);
        }

        /// <summary>
        /// 上傳圖檔(先放在暫存的目錄下)
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadRepairImg([FromForm] IFormCollection formCollection)
        {
            //CommonResult result = new CommonResult();
            string[] rtn = new string[] { "", "", "" };
            string distance = formCollection["distance"];
            string id = formCollection["id"];
            FormFileCollection filelist = (FormFileCollection)formCollection.Files;
            var file = filelist[0];
            var savePath = $"D:\\zzz\\image\\EqRepair\\temp\\{id}\\";
            var fileName = file.FileName.Insert(file.FileName.IndexOf("."), distance == "近" ? "維修前近照" : "維修前遠照");
            try {
                string filePath = Path.Combine(savePath, fileName);
                if (!Directory.Exists(savePath)) {
                    Directory.CreateDirectory(savePath);
                }
                using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                    file.CopyTo(fileStream);
                }

            } catch (Exception ex) {
                throw ex;
            }
            return Json(rtn);
        }

        /// <summary>
        /// 顯示圖檔(detail下方table)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="EqRepairId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult GetImg(string id, string EqRepairId, string fileName)
        {
            var sourceDir = @$"D:\zzz\image\EqRepair\{EqRepairId}\{id}\";
            var bytes = System.IO.File.ReadAllBytes(Path.Combine(sourceDir, fileName));
            return File(bytes, "image/png");
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="vt"></param>
        /// <returns></returns>
        public ActionResult InitPage(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var p = new PutInfo();
            var tags = vt.Tags;
            var isInit = tags.Value("isInit") == "Y";
            if (isInit) {
                var list = Db.GetArray("select distinct city from OpenDataRoad").Join(",");
                p.Select("A1", "", "請選擇", list, "");
                p.Select("A2", "", "請選擇", "", "");
                p.Select("A3", "", "請選擇", "", "");
                p.Add("A5", "");

            }
            rtn[1] = p.Build();
            return Json(rtn);
        }

        /// <summary>
        /// 下拉選單
        /// </summary>
        /// <param name="vt"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSiteByCity(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var p = new PutInfo();
            var tags = vt.Tags;
            var list = Db.GetArray($"select distinct site_id from OpenDataRoad where city = '{tags.Value("city").FixSql()}'").Join(",");
            p.Select("A2", "", "請選擇", list, "");

            rtn[1] = p.Build();
            return Json(rtn);
        }

        /// <summary>
        /// 下拉選單
        /// </summary>
        /// <param name="vt"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRoadByCitySite(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var p = new PutInfo();
            var tags = vt.Tags;
            var list = Db.GetArray($"select distinct road from OpenDataRoad where city = '{tags.Value("city").FixSql()}' and site_id = '{tags.Value("site").FixSql()}'").Join(",");
            p.Select("A3", "", "請選擇", list, "");

            rtn[1] = p.Build();
            return Json(rtn);
        }

        #region 輔助方法

        /// <summary>
        /// 合併地址欄位(tag的key必須為A1~A9)
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private string MergeAdd(Dictionary<string, string> tags)
        {
            string result = "";
            if (!string.IsNullOrEmpty(tags.Value("A1")) && tags.Value("A1") != "請選擇")
                result += $"{tags.Value("A1")}";
            if (!string.IsNullOrEmpty(tags.Value("A2")) && tags.Value("A2") != "請選擇")
                result += $"{tags.Value("A2")}";
            if (!string.IsNullOrEmpty(tags.Value("A3")) && tags.Value("A3") != "請選擇")
                result += $"{tags.Value("A3")}";
            if (!string.IsNullOrEmpty(tags.Value("A4")))
                result += $"{tags.Value("A4")}";
            if (!string.IsNullOrEmpty(tags.Value("A5")))
                result += $"{tags.Value("A5")}巷";
            if (!string.IsNullOrEmpty(tags.Value("A6")))
                result += $"{tags.Value("A6")}弄";
            if (!string.IsNullOrEmpty(tags.Value("A7")))
                result += $"{tags.Value("A7")}號";
            if (!string.IsNullOrEmpty(tags.Value("A8")))
                result += $"{tags.Value("A8")}樓";
            if (!string.IsNullOrEmpty(tags.Value("A9")))
                result += $"之{tags.Value("A9")}";
            return result;
        }

        /// <summary>
        /// 由於修繕原因跟上船圖檔都得name都是Id所要判斷是否有填資料進去(若是",,"代表此筆資料沒有要放進待修繕資料)
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private bool CheckData(KeyValuePair<string, string> tags)
        {
            var tag = tags.Value;
            if (tag == ",," || tag == ",,,") {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// 檢查設備tag是否都有包含修繕原因+修繕前進照+修繕前遠照({修繕原因},{修繕前近照},{修繕前遠照} 缺一不可) 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool CheckRequiredData(string value)
        {
            var splitChar = value.Split(",");
            foreach (var item in splitChar) {
                if (string.IsNullOrEmpty(item)) {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}