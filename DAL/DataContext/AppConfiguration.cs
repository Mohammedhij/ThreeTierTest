using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;


namespace DAL.DataContext
{
    public class AppConfiguration
    {
        public AppConfiguration() {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            IConfigurationRoot root = configurationBuilder.Build();
            IConfigurationSection appSettings = root.GetSection("ConnectionStrings:ThreetierTest");
            SqlConnectionString = appSettings.Value;

        
        }
        public String SqlConnectionString { get; set; }
    }
}
