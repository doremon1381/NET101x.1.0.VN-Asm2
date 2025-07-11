using MedicalModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Asm2.Attributes
{
    public class PersonRoleAcceptedAttribute : Attribute, IActionFilter
    {
        public PersonRole Role { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.ModelState.AddModelError("Unauthorized", "User is not authenticated.");
                return;
            }
            if (!user.IsInRole(Role.ToString()))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }
    }
}
