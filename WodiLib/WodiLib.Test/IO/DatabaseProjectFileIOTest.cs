using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseProjectFileIOTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [OneTimeSetUp]
        public static void OneTimeSetUp()
        {
            // テスト用ファイル出力
            DatabaseProjectFileTestItemGenerator.OutputFile();
        }

        [Test]
        public static void Database1ProjectIOTest()
        {
            UserDatabaseProjectFilePath inputFileName =
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\Database1.project";
            UserDatabaseProjectFilePath outputFileName =
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputDatabase1.project";

            var reader =
                new DatabaseProjectFileReader(
                    inputFileName,
                    DBKind.User);
            DatabaseProject data = null;
            var isSuccessRead = false;
            try
            {
                data = reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var writer = new DatabaseProjectFileWriter(outputFileName);
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync(data).GetAwaiter().GetResult();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine(
                $"Written FilePath : {outputFileName}");
        }

        [Test]
        public static void CDatabase0ProjectIOTest()
        {
            ChangeableDatabaseProjectFilePath inputFileName =
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\CDatabase1.project";
            ChangeableDatabaseProjectFilePath outputFileName =
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputCDatabase1.project";

            var reader =
                new DatabaseProjectFileReader(
                    inputFileName,
                    DBKind.Changeable);
            DatabaseProject data = null;
            var isSuccessRead = false;
            try
            {
                data = reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var writer = new DatabaseProjectFileWriter(outputFileName);
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync(data).GetAwaiter().GetResult();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine(
                $"Written FilePath : {outputFileName}");
        }

        [Test]
        public static void SysDatabase0ProjectIOTest()
        {
            SystemDatabaseProjectFilePath inputFileName =
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\SysDatabase1.project";
            SystemDatabaseProjectFilePath outputFileName =
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\OutputSysDatabase1.project";

            var reader =
                new DatabaseProjectFileReader(
                    inputFileName,
                    DBKind.System);
            DatabaseProject data = null;
            var isSuccessRead = false;
            try
            {
                data = reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var writer = new DatabaseProjectFileWriter(outputFileName);
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync(data).GetAwaiter().GetResult();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine(
                $"Written FilePath : {outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DatabaseProjectFileTestItemGenerator.DeleteFile();
        }
    }
}