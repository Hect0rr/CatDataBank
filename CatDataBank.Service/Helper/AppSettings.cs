using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CatDataBank.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CatDataBank.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}