using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class CommonFileIOTest
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
            // テスト用マップファイル出力
            CommonFileTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をCommonEventDataインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [Test]
        public static void Common000DataIOTest()
        {
            const string inputFileName = "000_コモンイベント000.common";
            var outputFileName = $"Output{inputFileName}";

            var reader = new CommonFileReader(
                $@"{CommonFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
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

            var commonEventData = reader.CommonFileData;

            var writer = new CommonFileWriter(commonEventData,
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FileName : {CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void Common003to005IOTest()
        {
            const string inputFileName = "Common003to005_コモンイベント003.common";
            var outputFileName = $"Output{inputFileName}";

            var reader =
                new CommonFileReader($@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
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

            var commonEventData = reader.CommonFileData;

            var writer = new CommonFileWriter(commonEventData,
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FileName : {CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void Common005To006Test()
        {
            const string inputFileName = "Common005to006_コモンイベント005.common";
            var outputFileName = $"Output{inputFileName}";

            var reader =
                new CommonFileReader($@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = false;
            try
            {
                reader.ReadSync();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var commonEventData = reader.CommonFileData;

            var writer = new CommonFileWriter(commonEventData,
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;

            try
            {
                writer.WriteSync();
                isSuccessWrite = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessWrite);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用マップファイル削除
            CommonEventDataFileTestItemGenerator.DeleteMapFile();
        }
    }
}