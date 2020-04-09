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
    public class TileSetDataFileIOTest
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
            TileSetDataTestItemGenerator.OutputFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をMapDataインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [Test]
        public static void File0Test()
        {
            const string inputFileName = @"Dir0\TileSetData.dat";
            const string outputFileName = @"OutputDir0\TileSetData.dat";

            var inputDir = Path.GetDirectoryName($@"{MapTreeDataFileItemGenerator.TestWorkRootDir}\{inputFileName}");
            inputDir.CreateDirectoryIfNeed();
            var outputDir = Path.GetDirectoryName($@"{MapTreeDataFileItemGenerator.TestWorkRootDir}\{outputFileName}");
            outputDir.CreateDirectoryIfNeed();

            var reader = new TileSetDataFileReader($@"{TileSetDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            TileSetData data = null;
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

            var writer = new TileSetDataFileWriter(
                $@"{TileSetDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

            Console.WriteLine($@"Written FilePath : {TileSetDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void File1Test()
        {
            const string inputFileName = @"Dir1\TileSetData.dat";
            const string outputFileName = @"OutputDir1\TileSetData.dat";

            var inputDir = Path.GetDirectoryName($@"{MapTreeDataFileItemGenerator.TestWorkRootDir}\{inputFileName}");
            inputDir.CreateDirectoryIfNeed();
            var outputDir = Path.GetDirectoryName($@"{MapTreeDataFileItemGenerator.TestWorkRootDir}\{outputFileName}");
            outputDir.CreateDirectoryIfNeed();

            var reader = new TileSetDataFileReader($@"{TileSetDataTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            TileSetData data = null;
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

            var writer = new TileSetDataFileWriter(
                $@"{TileSetDataTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
            TileSetDataTestItemGenerator.DeleteFile();
        }
    }
}