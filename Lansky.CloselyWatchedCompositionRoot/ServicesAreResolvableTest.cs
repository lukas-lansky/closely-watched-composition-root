using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Lansky.CloselyWatchedCompositionRoot
{
    [TestClass]
    public class ServicesAreResolvableTest
    {
        [DataTestMethod]
        [DataRow(typeof(IMailReceiver))]
        [DataRow(typeof(IImapConfiguration))]
        public void Indeed(Type t)
        {
            CompositionRoot.Get().Resolve(t);
        }
    }
}
