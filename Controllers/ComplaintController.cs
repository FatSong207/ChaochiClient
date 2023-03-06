using Microsoft.AspNetCore.Mvc;
using System;
using System.Set.Common;
using System.Set.Extensions;
using System.Threading.Tasks;
using WebAdmin.Function.Helper;
using Yuebon.Chaochi.Models;
using Yuebon.Commons.VerificationCode;
using System.Web;
using Microsoft.Extensions.Caching.Distributed;

namespace WebAdmin.Controllers
{
    public class ComplaintController : BaseController
    {
        private readonly IDistributedCache _distributedCache;

        public ComplaintController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 檢查驗證碼並新增資料
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult Insert(VarTags vt)
        {
            var rtn = new string[] { "", "" };
            var tags = vt.Tags;
            //var code = TempData["code"].ToString();
            var code = _distributedCache.GetString("code_").ToString();
            if (tags.Value("C1").FixSql() == code) {
                var complaint = new Chaochi_Complaint() {
                    Id = Helper.CreateId(),
                    CCode = DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                    Name = tags.Value("C2"),
                    ContactPhone = tags.Value("C3"),
                    ContactMail = tags.Value("C4"),
                    ContactDatetime = tags.Value("C5"),
                    ComplaintType = tags.Value("C6"),
                    ComplaintDept = tags.Value("C7"),
                    Complainee = tags.Value("C8"),
                    CId = tags.Value("C9"),
                    ComplaintReason = tags.Value("C10"),
                    ProvideAdvice = tags.Value("C11"),
                    State = "待處理",
                    ComplaintCreatorTime = DateTime.Now,
                    SendMailCount = 0
                };
                try {
                    var db = Db.Orm();
                    var success = db.Insert(complaint);
                    rtn[1] = success == true ? "Y" : "N";
                } catch (Exception) {
                    throw;
                }
            } else {
                //rtn[0] = "驗證碼錯誤";
                rtn[1] = "C";
            }
            return Json(rtn);
        }

        /// <summary>
        /// 獲取驗證碼+將驗證四碼存在Cache
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Captcha()
        {
            byte[] data = null;
            Captcha captcha = new Captcha();
            string code = await captcha.GenerateRandomCaptchaAsync().ConfigureAwait(false);
            _distributedCache.SetString("code_",code);
            data = await captcha.GenerateCaptchaImageAsync(code);
            return File(data, "image/jpeg");
        }
    }
}