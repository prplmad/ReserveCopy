using Microsoft.Extensions.Configuration;
using System.IO;

namespace ReserveCopy
{
    public static class CopyFiles
    {
        public static void Copy(IConfigurationRoot configuration, NLog.Logger logger, string newFolderPath, string[] baseFolderPath, string newFolderPathForDeleting)
        {
            foreach (var baseFolder in baseFolderPath)
            {
                try
                {
                    if (!Directory.Exists(newFolderPath))
                    {
                        logger.Debug($"\nСоздаем папку {newFolderPath}");
                    }
                    Directory.CreateDirectory(newFolderPath);
                    DirectoryInfo dirInfo = new DirectoryInfo(baseFolder);

                    foreach (FileInfo file in dirInfo.GetFiles())
                    {
                        try
                        {
                            logger.Debug($"\nКопируем файл {file.Name}");
                            File.Copy(file.FullName, newFolderPath + "\\" + file.Name, true);  // overwrite is true
                            logger.Debug($"\nСкопирован файл {file.Name}");
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"\nНе удается скопировать файл {file.Name}\n Ошибка {ex.Message} \n Введите '1', чтобы пропустить этот файл и перейти к следующему, '2' - чтобы прервать процесс, '3' - чтобы прервать процесс и удалить уже скопированные файлы");
                            while (true)
                            {
                                var input = Console.ReadLine();
                                if (input == "1")
                                {
                                    goto End;
                                }
                                if (input == "2")
                                {
                                    return;
                                }
                                if (input == "3")
                                {
                                    Directory.Delete(newFolderPathForDeleting, true);
                                    return;
                                }
                                else
                                {
                                    logger.Debug($"\nУдаляем директорию {newFolderPathForDeleting}");
                                    Console.WriteLine("Введите '1', чтобы пропустить этот файл и перейти к следующему, '2' - чтобы прервать процесс, '3' - чтобы прервать процесс и удалить уже скопированные файлы");
                                    continue;
                                }
                            }
                        End: continue;

                        }

                    }
                    foreach (string s in Directory.GetDirectories(baseFolder))
                    {
                        Copy(configuration, logger, newFolderPath + "\\" + Path.GetFileName(s), new[] { s }, newFolderPathForDeleting);
                    }

                }
                catch (Exception ex)
                {
                    logger.Error("\nВозникла ошибка " + ex.Message);
                    throw;
                }
            }

        }

    }

}
