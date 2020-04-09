using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DatabaseDataFileIOTest
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
            DatabaseDatFileTestItemGenerator.OutputFile();
        }

        private static readonly object[] DatabaseDatIOTestCaseSource =
        {
            new object[]
            {
                (UserDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\Database1.dat",
                (UserDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputDatabase1.dat",
            },
            new object[]
            {
                (ChangeableDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\CDatabase1.dat",
                (ChangeableDatabaseDatFilePath)
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputCDatabase1.dat",
            },
            new object[]
            {
                (SystemDatabaseDatFilePath) $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\SysDatabase1.dat",
                (SystemDatabaseDatFilePath)
                $@"{DatabaseDatFileTestItemGenerator.TestWorkRootDir}\OutputSysDatabase1.dat",
            },
        };

        [TestCaseSource(nameof(DatabaseDatIOTestCaseSource))]
        public static void DatabaseDatIOTest(DatabaseDatFilePath inputFileName, DatabaseDatFilePath outputFileName)
        {
            var reader =
                new DatabaseDatFileReader(
                    inputFileName,
                    DBKind.User);
            var isSuccessRead = false;
            DatabaseDat data = null;
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

            var writer = new DatabaseDatFileWriter(outputFileName);
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
                $@"Written FilePath : {DatabaseDatFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DatabaseDatFileTestItemGenerator.DeleteFile();
        }
    }
}