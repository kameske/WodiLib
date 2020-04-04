using System;
using System.IO;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class TileSetFileIOTest
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
            TileSetFileDataTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をdataインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [Test]
        public static void File0Test()
        {
            const string inputFileName = @"000_設定名.tile";
            const string outputFileName = @"Output_000_設定名.tile";

            var inputDir = Path.GetDirectoryName($@"{MapTreeDataFileItemGenerator.TestWorkRootDir}\{inputFileName}");
            inputDir.CreateDirectoryIfNeed();
            var outputDir = Path.GetDirectoryName($@"{MapTreeDataFileItemGenerator.TestWorkRootDir}\{outputFileName}");
            outputDir.CreateDirectoryIfNeed();

            var reader = new TileSetFileReader($@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            TileSetFileData data = null;
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

            var writer = new TileSetFileWriter(
                $@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
                $@"Written FilePath : {TileSetFileDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void File1Test()
        {
            const string inputFileName = @"001_aaa.tile";
            const string outputFileName = @"Output_001_aaa.tile";

            var reader = new TileSetFileReader($@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            TileSetFileData data = null;
            var isSuccessRead = false;
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

            var writer = new TileSetFileWriter(
                $@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

        [Test]
        public static void File2Test()
        {
            const string inputFileName = @"000_街.tile";
            const string outputFileName = @"Output_000_街.tile";

            var reader = new TileSetFileReader($@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            TileSetFileData data = null;
            var isSuccessRead = false;
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

            var writer = new TileSetFileWriter(
                $@"{TileSetFileDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
            TileSetFileDataTestItemGenerator.DeleteFile();
        }
    }
}