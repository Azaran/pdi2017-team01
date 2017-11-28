using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using WebApp.Models;

namespace WebApp.Account
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Forgot(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Ověřit e-mailovou adresu uživatele
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.FindByName(Email.Text);
                if (user == null || !manager.IsEmailConfirmed(user.Id))
                {
                    FailureText.Text = "Uživatel neexistuje nebo není potvrzený.";
                    ErrorMessage.Visible = true;
                    return;
                }
                // Další informace o tom, jak povolit potvrzení účtu a resetování hesla najdete na webu https://go.microsoft.com/fwlink/?LinkID=320771.
                // Odeslat e-mail s kódem a přesměrováním na stránku pro resetování hesla
                //string code = manager.GeneratePasswordResetToken(user.Id);
                //string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                //manager.SendEmail(user.Id, "Resetovat heslo", "Resetujte heslo kliknutím <a href=\"" + callbackUrl + "\">sem</a>.");
                loginForm.Visible = false;
                DisplayEmail.Visible = true;
            }
        }
    }
}