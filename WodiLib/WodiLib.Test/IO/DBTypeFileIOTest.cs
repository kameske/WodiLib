using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class DBTypeFileIOTest
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
            DBTypeFileTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をDBTypeインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [TestCase("タイプ(データ含む)_002_┣ 主人公行動AI.dbtype", "Outputタイプ(データ含む)_002_┣ 主人公行動AI.dbtype")]
        [TestCase("タイプ(データ含む)_008_状態設定.dbtype", "Outputタイプ(データ含む)_008_状態設定.dbtype")]
        public static void DBTypeIOTest(string inputFileName, string outputFileName)
        {
            var reader =
                new DBTypeFileReader(
                    $@"{DBTypeFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
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

            var writer = new DBTypeFileWriter(data,
                $@"{DBTypeFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {DBTypeFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            DBTypeFileTestItemGenerator.DeleteFile();
        }
    }
}