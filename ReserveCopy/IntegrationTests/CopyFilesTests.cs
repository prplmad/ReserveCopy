using NUnit.Framework;
using NLog;
using NLog.Web;

namespace ReserveCopy.IntegrationTests
{
    //Не тестирую весь функционал намеренно, поскольку это тестовое задание

    [TestFixture]
    public class CopyFilesTests
    {
        private Logger logger;
        private string target;
        private string[] source;
         
        
        [SetUp]
        public void Init()
        {
            Logger logger = NLog.LogManager.Setup().LoadConfiguration(builder =>
            {
                builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToConsole();
                builder.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToFile(fileName: $"C:\\ForTesting\\{DateTime.Now}.txt".Replace(':', '-'));
            }).GetCurrentClassLogger();
            target = "C:\\ForTesting\\targetFolder" + $"\\{DateTime.Now}".Replace(':', '-');
            source = "C:\\ForTesting\\base, C:\\ForTesting\\base2".Replace(" ", "").Split(',');
        }

        [Test]
        public void CopyFiles_SkipStrategy_FilesCopiedWithoutAnAccessExceptions()
        {

            File.Create(source[0] + "\\textFile.txt").Close();
            File.Create(source[1] + "\\textFile2.txt").Close();
            string strategy = "Skip";

            CopyFiles.Copy(logger, target, source, strategy);

            Assert.IsTrue(File.Exists(target + "\\textFile.txt"));
            Assert.IsTrue(File.Exists(target + "\\textFile2.txt"));
            File.Delete(source[0] + "\\textFile.txt");
            File.Delete(source[1] + "\\textFile2.txt");
            foreach (string dir in Directory.GetDirectories(target))
            {
                Directory.Delete(dir , true);
            }
        }
    }
}
