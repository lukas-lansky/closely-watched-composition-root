using Castle.MicroKernel;
using Castle.Windsor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace Lansky.CloselyWatchedCompositionRoot
{
    [TestClass]
    public class ServiceLocatorNotPresentTest
    {
        [TestMethod]
        public void Indeed()
        {
            var container = CompositionRoot.Get();

            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var locators = host.GetDiagnostic<IUsingContainerAsServiceLocatorDiagnostic>().Inspect();

            if (locators.Any())
            {
                var diagnosticOutput = new StringBuilder();

                diagnosticOutput.AppendLine("Following components looks suspiciously like a service locator:" + Environment.NewLine);
                foreach (var locator in locators)
                {
                    diagnosticOutput.AppendLine(locator.ToString());
                }

                Assert.Fail(diagnosticOutput.ToString());
            }
        }
    }
}
