using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeSetFileIOTest
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
            DBTypeSetFileTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をDBTypeSetインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [TestCase("タイプ設定_002_┣ 主人公行動AI.dbtypeset", "Outputタイプ設定_002_┣ 主人公行動AI.dbtypeset")]
        [TestCase("タイプ設定_008_状態設定.dbtypeset", "Outputタイプ設定_008_状態設定.dbtypeset")]
        public static void TypeSetIOTest(string inputFileName, string outputFileName)
        {
            var reader =
                new DBTypeSetFileReader(
                    $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            DBTypeSet data = null;
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

            var writer = new DBTypeSetFileWriter(
                $@"{DBTypeSetFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {DBTypeSetFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBTypeSetFileTestItemGenerator.DeleteFile();
        }
    }
}