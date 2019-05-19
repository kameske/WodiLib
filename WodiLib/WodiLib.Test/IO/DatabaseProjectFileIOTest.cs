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
            const string inputFileName = "Database1.project";
            const string outputFileName = "OutputDatabase1.project";

            var reader =
                new DatabaseProjectFileReader(
                    $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{inputFileName}",
                    DBKind.User);
            var isSuccessRead = false;
            try
            {
                reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var data = reader.Data;

            var writer = new DatabaseProjectFileWriter(data,
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync().GetAwaiter().GetResult();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine(
                $@"Written FilePath : {DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void CDatabase0ProjectIOTest()
        {
            const string inputFileName = "CDatabase1.project";
            const string outputFileName = "OutputCDatabase1.project";

            var reader =
                new DatabaseProjectFileReader(
                    $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{inputFileName}",
                    DBKind.Changeable);
            var isSuccessRead = false;
            try
            {
                reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var data = reader.Data;

            var writer = new DatabaseProjectFileWriter(data,
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync().GetAwaiter().GetResult();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine(
                $@"Written FilePath : {DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void SysDatabase0ProjectIOTest()
        {
            const string inputFileName = "SysDatabase1.project";
            const string outputFileName = "OutputSysDatabase1.project";

            var reader =
                new DatabaseProjectFileReader(
                    $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{inputFileName}",
                    DBKind.System);
            var isSuccessRead = false;
            try
            {
                reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var data = reader.Data;

            var writer = new DatabaseProjectFileWriter(data,
                $@"{DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync().GetAwaiter().GetResult();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine(
                $@"Written FilePath : {DatabaseProjectFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DatabaseProjectFileTestItemGenerator.DeleteFile();
        }
    }
}