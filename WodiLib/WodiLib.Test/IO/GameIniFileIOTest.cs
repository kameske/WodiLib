using System;
using System.IO;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class GameIniFileIOTest
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
            GameIniDataTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をGameIniインスタンスに変換して
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [TestCase(@"Dir0\Game.ini", @"Output0\Game.ini")]
        [TestCase(@"Dir1\Game.ini", @"Output1\Game.ini")]
        public static void FileIOTest(string inputFileName, string outputFileName)
        {
            var dir = Path.GetDirectoryName($@"{EditorIniDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            dir.CreateDirectoryIfNeed();

            var reader =
                new GameIniFileReader(
                    $@"{GameIniDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            GameIniData data = null;
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

            var writer = new GameIniFileWriter(
                $@"{GameIniDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {GameIniDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            GameIniDataTestItemGenerator.DeleteFile();
        }
    }
}