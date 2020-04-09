using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class CommonEventDataFileIOTest
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
            CommonEventDataFileTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をCommonEventDataインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [Test]
        public static void CommonEvent00DataIOTest()
        {
            const string inputFileName = "CommonEvent_00.dat";
            const string outputFileName = "OutputCommonEvent_00.dat";

            var reader =
                new CommonEventDatFileReader(
                    $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = false;
            CommonEventData commonEventData = null;
            try
            {
                commonEventData = reader.ReadAsync().GetAwaiter().GetResult();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var writer = new CommonEventDatFileWriter(
                $@"{CommonEventDataFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;
            try
            {
                writer.WriteAsync(commonEventData).GetAwaiter().GetResult();
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

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            CommonEventDataFileTestItemGenerator.DeleteMapFile();
        }
    }
}