using Castle.MicroKernel;
using Castle.Windsor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace Lansky.CloselyWatchedCompositionRoot
{
    [TestClass]
    public class LifestylesAreMatchingTest
    {
        [TestMethod]
        public void Indeed()
        {
            var container = CompositionRoot.Get();

            var host = (IDiagnosticsHost)container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
            var misconfigurations = host.GetDiagnostic<IPotentialLifestyleMismatchesDiagnostic>().Inspect();

            if (misconfigurations.Any())
            {
                var diagnosticOutput = new StringBuilder();

                diagnosticOutput.AppendLine("Following dependency chains have inconsistent accessibility:" + Environment.NewLine);

                foreach (var misconfiguration in misconfigurations)
                {
                    diagnosticOutput.AppendLine(
                        string.Join(
                            " -> ",
                            misconfiguration
                                .Where(m => m.ComponentModel.Services.Any())
                                .Select(m => $"{m.ComponentModel.Services.First().Name} ({m.ComponentModel.LifestyleType})")));
                }

                Assert.Fail(diagnosticOutput.ToString());
            }
        }
    }
}
