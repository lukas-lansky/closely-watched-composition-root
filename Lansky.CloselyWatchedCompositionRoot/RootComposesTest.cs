using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace Lansky.CloselyWatchedCompositionRoot
{
    [TestClass]
    public class RootComposesTest
    {
        [TestMethod]
        public void Indeed()
        {
            var container = CompositionRoot.Get();

            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var misconfigurations = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>().Inspect();

            if (misconfigurations.Any())
            {
                var diagnosticOutput = new StringBuilder();

                foreach (IExposeDependencyInfo problem in misconfigurations)
                {
                    problem.ObtainDependencyDetails(new DependencyInspector(diagnosticOutput));
                }

                Assert.Fail(diagnosticOutput.ToString());
            }
        }
    }
}
