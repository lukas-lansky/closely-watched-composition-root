using Castle.Core;
using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lansky.CloselyWatchedCompositionRoot
{
    [TestClass]
    public class ServicesAreUsedTest
    {
        [TestMethod]
        public void Indeed()
        {
            var coreServices = new[] {
                "Lansky.CloselyWatchedCompositionRoot.ImapMailReceiver"
            };

            var container = CompositionRoot.Get();

            var services = container.Kernel.GraphNodes.Where(n => n is ComponentModel).Cast<ComponentModel>();
            var usefulServices = new HashSet<ComponentModel>();
            var servicesToProcess = new Stack<ComponentModel>(
                services.Where(service =>
                    coreServices.Any(coreService =>
                        service.ComponentName.Name == coreService)));

            while (servicesToProcess.TryPop(out var service))
            {
                usefulServices.Add(service);
                
                foreach (var subservice in service.Dependents.Where(n => n is ComponentModel).Cast<ComponentModel>())
                {
                    servicesToProcess.Push(subservice);
                }
            }

            var unusefulServices = new HashSet<ComponentModel>(services);
            unusefulServices.ExceptWith(usefulServices);

            Assert.AreEqual(0, unusefulServices.Count, "Following services are registered and are not needed: " + string.Join(", ", unusefulServices));
        }
    }
}
