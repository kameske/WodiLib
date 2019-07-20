using System;
using System.IO;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class EditorIniFileIOTest
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
            EditorIniDataTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をEditorIniインスタンスに変換して
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [TestCase(@"Dir0\Editor.ini", @"Output0\Editor.ini")]
        [TestCase(@"Dir1\Editor.ini", @"Output1\Editor.ini")]
        public static void FileIOTest(string inputFileName, string outputFileName)
        {
            var dir = Path.GetDirectoryName($@"{EditorIniDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            dir.CreateDirectoryIfNeed();

            var reader =
                new EditorIniFileReader(
                    $@"{EditorIniDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
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

            var writer = new EditorIniFileWriter(data,
                $@"{EditorIniDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {EditorIniDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            EditorIniDataTestItemGenerator.DeleteFile();
        }
    }
}