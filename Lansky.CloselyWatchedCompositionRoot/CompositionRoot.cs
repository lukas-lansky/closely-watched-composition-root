using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System.Collections.Generic;
using System.Configuration;

namespace Lansky.CloselyWatchedCompositionRoot
{
    public static class CompositionRoot
    {
        public static IWindsorContainer Get()
        {
            var container = new WindsorContainer();

            container.Register(
                Component.For<IMailReceiver>().ImplementedBy<ImapMailReceiver>().LifestyleTransient(),
                Component.For<IImapConfiguration>().ImplementedBy<AppConfigImapConfiguration>().LifestyleSingleton()
            );

            return container;
        }
    }

    public interface IMailReceiver
    {
        IEnumerable<(string sender, string body)> Receive(string filter);
    }

    public interface IImapConfiguration
    {
        string ImapAdress { get; }
    }

    public class ImapMailReceiver : IMailReceiver
    {
        private readonly IImapConfiguration _imapConfiguration;

        public ImapMailReceiver(IImapConfiguration imapConfiguration)
        {
            _imapConfiguration = imapConfiguration;
        }

        public IEnumerable<(string sender, string body)> Receive(string filter)
        {
            // code!

            yield return ("a", "b");
        }
    }

    public class AppConfigImapConfiguration : IImapConfiguration
    {
        public string ImapAdress
            => ConfigurationManager.AppSettings[nameof(ImapAdress)];
    }
}
