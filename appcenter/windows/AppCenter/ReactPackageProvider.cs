using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

namespace AppCenter
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
