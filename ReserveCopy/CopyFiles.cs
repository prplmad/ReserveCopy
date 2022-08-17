using Microsoft.Extensions.Configuration;
using System.IO;

namespace ReserveCopy
{
    public static class CopyFiles
    {
        public static void Copy(NLog.Logger logger, string target, string[] source, string strategy)
        {
            try
            {
                IBrokenStrategy strategyImplementation = strategy switch
                {
                    "Skip" => new SkipBrokenStrategy(),
                    "Abort" => new AbortBrokenStrategy(),
                    "AbortAndCleanUp" => new AbortAndCleanUpBrokenStrategy(),
                    _ => throw new NotSupportedException("Некорректный ввод настройки BrokenFileOrDirectoryStrategy в конфигурации приложения.Завершаем программу.")
                };
                strategyImplementation.Action(target, source, logger);
            }
            catch (Exception ex)
            {
                logger.Error($"\n{ex.Message}\n");
                return;
            }
        }
    }

}
