using Microsoft.Extensions.Configuration;
using ReserveCopy;
using NLog;
using NLog.Web;



IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
IConfigurationRoot configuration = builder.Build();

Logger logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

logger.Info("\nСтарт приложения\n");


var target = configuration.GetSection("Config:Target").Value + $"\\{DateTime.Now}".Replace(':', '-');
var source = configuration.GetSection("Config:Source").Value.Replace(" ", "").Split(',');
var strategy = configuration.GetSection("Config:BrokenFileOrDirectoryStrategy").Value.Replace(" ", "");
CopyFiles.Copy(logger, target, source, strategy);

logger.Info("\nОкончание работы приложения\n");


