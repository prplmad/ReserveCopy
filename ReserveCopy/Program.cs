using Microsoft.Extensions.Configuration;
using ReserveCopy;
using NLog;



NLog.LogManager.Setup().LoadConfiguration(builder => {
    builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToConsole();
    builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"{DateTime.Now}.txt".Replace(':', '-'));
});
NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
IConfigurationRoot configuration = builder.Build();

logger.Info("\nСтарт приложения");

var newFolderPath = configuration.GetSection("Data:newFolder").Value + $"\\{DateTime.Now}".Replace(':', '-');
var newFolderPathForDeleting = newFolderPath;
var baseFolders = configuration.GetSection("Data:Base").Get<string[]>();
CopyFiles.Copy(configuration, logger, newFolderPath, baseFolders , newFolderPathForDeleting);

logger.Info("\nОкончание работы приложения");


