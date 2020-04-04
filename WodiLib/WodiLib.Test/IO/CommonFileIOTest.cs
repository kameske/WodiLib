using System;
using NUnit.Framework;
using WodiLib.Common;
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
            // テスト用ファイル出力
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
            CommonFileData data = null;
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

            var writer = new CommonFileWriter(
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void Common003to005IOTest()
        {
            const string inputFileName = "Common003to005_コモンイベント003.common";
            var outputFileName = $"Output{inputFileName}";

            var reader =
                new CommonFileReader($@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = false;
            CommonFileData data = null;
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

            var writer = new CommonFileWriter(
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [TestCase("Common005to006_コモンイベント005.common")]
        [TestCase("各種メニュー呼び出し.common")]
        public static void CommonReadTest(string inputFileName)
        {
            var outputFileName = $"Output{inputFileName}";

            var reader =
                new CommonFileReader($@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = false;
            CommonFileData data = null;
            try
            {
                data = reader.ReadSync();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var writer = new CommonFileWriter(
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;

            try
            {
                writer.WriteSync(data);
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
            // テスト用ファイル削除
            CommonEventDataFileTestItemGenerator.DeleteMapFile();
        }
    }
}