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
        }
    }
}
