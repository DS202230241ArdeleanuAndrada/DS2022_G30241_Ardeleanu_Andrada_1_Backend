using App.Models;
using DbUp;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace App.Database
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            Console.WriteLine("Database migration started...");
            var x = Directory.GetCurrentDirectory();
            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectDirectory)
                .AddJsonFile("appsettings.Database.json", false)
                .AddEnvironmentVariables()
                .Build();

            var config = configuration.GetSection("CustomSettings").Get<CustomSettings>();

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(config.ConnectionString)
                    .WithTransactionPerScript()
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Console.ReadLine();

                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            Console.ReadLine();

            return 0;
        }
    }
}
