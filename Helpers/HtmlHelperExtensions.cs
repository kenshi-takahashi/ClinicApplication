using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Clinic.Models;

namespace Clinic.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent GetUserName(this IHtmlHelper<dynamic> htmlHelper)
        {
            var context = htmlHelper.ViewContext.HttpContext.RequestServices.GetService<ClinicDbContext>();
            var userEmail = htmlHelper.ViewContext.HttpContext.User.Identity.Name;
            var patient = context.Patients.FirstOrDefault(p => p.Phone == userEmail);
            var userName = patient != null ? patient.FullName : userEmail;
            return new HtmlString(userName);
        }
    }
}
