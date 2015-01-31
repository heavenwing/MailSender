using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MailSender.Startup))]
namespace MailSender
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.UseHangfire(config =>
            {
                config.UseSqlServerStorage("Hangfire");
                config.UseServer();

                //config.UseAuthorizationFilters(new AuthorizationFilter
                //{
                //    Users = "admin, superuser", // allow only specified users
                //    Roles = "admins" // allow only specified roles
                //});

                //// or

                //config.UseAuthorizationFilters(
                //    new ClaimsBasedAuthorizationFilter("hangfire", "access"));
            });

            app.MapSignalR();

            MessageSender.NotifyCompleted = SentNotification.Notify;
        }
    }
}
