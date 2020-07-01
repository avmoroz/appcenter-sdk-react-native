using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ReactNative.Managed;

namespace Microsoft.ReactNative.AppCenter.Analytics
{
    public sealed class ReactPackageProvider : IReactPackageProvider
    {
        public void CreatePackage(IReactPackageBuilder packageBuilder)
        {
            packageBuilder.AddAttributedModules();
            packageBuilder.AddViewManagers();
        }
    }
}
