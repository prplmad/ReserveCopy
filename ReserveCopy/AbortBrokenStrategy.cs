using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReserveCopy
{
    public class AbortBrokenStrategy : IBrokenStrategy
    {
        public void Action(string target, string[] source, NLog.Logger logger)
        {
            foreach (var folderFromSource in source)
            {
                try
                {
                    if (CreateFolder(target, source, logger, folderFromSource))
                    {
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("\nОшибка приложения - некорректно указан Source в файле конфигурации " + ex.Message + "\n");
                    logger.Debug("\nУдаляем созданные папки\n");
                    Directory.Delete(target, true);
                    return;
                }
                
            }

            foreach (var folderFromSource in source)
            {
                if (CopyFile(target, source, logger, folderFromSource))
                {
                    continue;
                }
                else
                {
                    return;
                }
            }
        }

        public bool CreateFolder(string target, string[] source, NLog.Logger logger, string folderFromSource)
        {
            foreach (var dirPath in Directory.GetDirectories(folderFromSource, "*", SearchOption.AllDirectories))
            {
                try
                {
                    logger.Debug($"\nСоздаем папку {dirPath.Replace(folderFromSource, target)}\n");
                    Directory.CreateDirectory(dirPath.Replace(folderFromSource, target));
                    logger.Debug("\nПапка создана\n");
                }
                catch (Exception ex)
                {
                    logger.Error($"\nНе удается создать папку {dirPath.Replace(folderFromSource, target)}\n" + ex.Message + ". Завершаем работу программы и удаляем то, что успели скопировать!\n");
                    Directory.Delete(target, true);
                    return false;
                }
            }
            return true;
        }

        public bool CopyFile(string target, string[] source, NLog.Logger logger, string folderFromSource)
        {
            foreach (string filePath in Directory.GetFiles(folderFromSource, "*", SearchOption.AllDirectories))
            {
                try
                {
                    logger.Debug($"\nКопируем файл {filePath}\n");
                    File.Copy(filePath, filePath.Replace(folderFromSource, target), true);
                    logger.Debug($"\nСкопирован файл {filePath}\n");
                }
                catch (Exception ex)
                {
                    logger.Error($"\nНе удается скопировать файл {filePath} " + ex.Message + ". Завершаем работу программы!\n");
                    return false;
                }
            }
            return true;
        }
    }
}
