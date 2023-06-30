using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace App.Core.Helpers
{
    public class ConfigurationHelper
    {
        // Statische Instanz des IConfigurationRoot, die über Lazy Loading erstellt wird
        private static readonly Lazy<IConfigurationRoot> configurationInstance = new Lazy<IConfigurationRoot>(() => CreateConfiguration());

        // Statische Instanz des IConfigurationBuilder, die über Lazy Loading erstellt wird
        private static readonly Lazy<IConfigurationBuilder> builderInstance = new Lazy<IConfigurationBuilder>(() => CreateBuilder());

        // Statische Eigenschaft, um auf die IConfigurationRoot-Instanz zuzugreifen
        public static IConfigurationRoot Configuration => configurationInstance.Value;

        private ConfigurationHelper()
        {
            // Privater Konstruktor, um die Instanziierung der Klasse zu verhindern
        }

        // Statische Methode, um den IConfigurationBuilder abzurufen
        public static IConfigurationBuilder GetConfigurationBuilder()
        {
            return builderInstance.Value;
        }

        // Statische Methode, um die IConfigurationRoot-Instanz zu erstellen
        public static IConfigurationRoot CreateConfiguration()
        {
            return GetConfigurationBuilder().Build();
        }

        // Statische Methode, um den IConfigurationBuilder zu erstellen
        public static IConfigurationBuilder CreateBuilder()
        {
            var builder = new ConfigurationBuilder();
            var runtimePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var appsettings = "appsettings.json";
            var fullPath = Path.Combine(runtimePath, appsettings);

            // Konfiguration des IConfigurationBuilder mit JSON-Datei, Umgebungsvariablen usw.
            return builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(fullPath, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
