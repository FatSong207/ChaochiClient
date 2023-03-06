using Microsoft.AspNetCore.Mvc.Filters;
using System.Set.Common;
using System.Set.Filter;
using System.Threading.Tasks;
using WebAdmin.Function.Common;

namespace WebAdmin.Controllers
{
    public class BaseController : SysBaseController
    {
        public BaseController()
        {
        }

        public UserRole LoginUser
        {
            get
            {
                return Page.Session<UserRole>("User");
            }
            set
            {
                Page.Session("User", value);
            }
        }
    }
}