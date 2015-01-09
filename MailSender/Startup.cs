using Hangfire;
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
            });

            MessageSender.NotifyCompleted = SentNotification.Notify;
        }
    }
}
