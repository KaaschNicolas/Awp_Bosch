using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace App.Core.Helpers
{
    public class ConfigurationHelper
    {
        private static readonly Lazy<IConfigurationRoot> configurationInstance = new Lazy<IConfigurationRoot>(() => CreateConfiguration());
        private static readonly Lazy<IConfigurationBuilder> builderInstance = new Lazy<IConfigurationBuilder>(() => CreateBuilder());

        public static IConfigurationRoot Configuration => configurationInstance.Value;

        private ConfigurationHelper()
        {
        }

        public static IConfigurationBuilder GetConfigurationBuilder()
        {
            return builderInstance.Value;
        }

        public static IConfigurationRoot CreateConfiguration()
        {
            return GetConfigurationBuilder().Build();
        }

        public static IConfigurationBuilder CreateBuilder()
        {
            var builder = new ConfigurationBuilder();
            var runtimePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var appsettings = "appsettings.json";
            var fullPath = Path.Combine(runtimePath, appsettings);

            return builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fullPath, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
