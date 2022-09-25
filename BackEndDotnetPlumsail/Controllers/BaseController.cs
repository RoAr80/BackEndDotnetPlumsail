using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndDotnetPlumsail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        protected IActionResult ExceptionResult(Exception ex, object args = null)
        {
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;

            var msg = $"{ex.Message}";

            var msgAll = $"{ex.Message} {ex.InnerException?.Message}";


            if (ex is ArgumentException)
            {
                #if DEBUG
                return BadRequest(ex.Message);
                #else
                return BadRequest(msg);
                #endif
            }
            return BadRequest(msgAll);
        }
    }
}
