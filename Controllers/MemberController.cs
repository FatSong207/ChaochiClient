using Microsoft.AspNetCore.Mvc;
using System.Set.Common;
using System.Set.Extensions;

namespace WebAdmin.Controllers
{
    public class MemberController : BaseController
    {
        /// <summary>
        /// 會員管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增 / 更新會員
        /// </summary>
        /// <param name="vt">頁面參數</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var tags = vt.Tags;

            var tb = Db.Table("Member", new { MemberSN = tags.Value("SN").FixInt() });
            var dr = tb.dr;

            if (dr == null) {
                dr = tb.NewRow();
            }
            dr["CName"] = tags.Value("B1");
            dr["UserId"] = tags.Value("B2");

            if (!tb.Update()) {
                rtn[0] = "更新失敗 !";
            }
            return Json(rtn);
        }

        /// <summary>
        /// 刪除會員
        /// </summary>
        /// <param name="vt">頁面參數</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            if (vt.Error) { return Page.VarTagsError(); }
            var tags = vt.Tags;

            if (!Db.Delete("Member", new { MemberSN = tags.Value("SN").FixInt() })) {
                rtn[0] = "刪除失敗 !";
            }
            return Json(rtn);
        }

        /// <summary>
        /// 會員資訊
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

            p.AreaId(".modal-content .dialog-form");

            var dr = Db.GetRow("Member", new { MemberSN = tags.Value("SN").FixInt() });
            if (dr != null) {
                p.Add("B1", "", dr["CName"]);
                p.Add("B2", "", dr["UserId"]);
            }
            rtn[1] = p.Build();
            return Json(rtn);
        }

        /// <summary>
        /// 會員管理列表
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
            string sql;

            var pi = new PageInfo(ViewBag);

            sql = "SELECT ";
            sql += "    a.MemberSN, ";
            sql += "    a.UserId, ";
            sql += "    a.CName ";
            sql += "FROM ";
            sql += "    Member a with (nolock) ";
            sql += "WHERE ";
            sql += "    1=1 ";

            if (tags.Value("C1") != "") {
                sql += $" AND a.CName = '{tags.Value("C1")}' ";
            }
            pi.Init(tags, sql);
            var pi2 = pi;
            rtn[2] = pi.TableView("_TableView1");
            rtn[3] = p.Build();

            return Json(rtn);
        }
    }
}