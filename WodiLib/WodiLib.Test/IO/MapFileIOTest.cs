using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    [TestFixture]
    public class MapFileIOTest
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
            MapFileTestItemGenerator.OutputMapFile();
        }

        // ################################################################################
        //    Resourceファイルの内容をMapDataインスタンスに変換してバイナリ変換した後
        //    別ファイルとして書き出すテスト
        // ################################################################################

        [Test]
        public static void SampleMapAIOTest()
        {
            const string inputFileName = "SampleMapA.mps";
            const string outputFileName = "OutputSampleMapA.mps";

            var reader = new MpsFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            MapData data = null;
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

            var writer = new MpsFileWriter(
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

            Console.WriteLine($@"Written FilePath : {MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void SampleMapBIOTest()
        {
            const string inputFileName = "SampleMapB.mps";
            const string outputFileName = "OutputSampleMapB.mps";

            var reader = new MpsFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            MapData data = null;
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

            var writer = new MpsFileWriter(
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

            Console.WriteLine($@"Written FilePath : {MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void DungeonIOTest()
        {
            const string inputFileName = "Dungeon.mps";
            const string outputFileName = "OutputDungeon.mps";

            var reader = new MpsFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            MapData data = null;
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

            var writer = new MpsFileWriter(
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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
        public static void Map100IOTest()
        {
            const string inputFileName = "Map100.mps";
            const string outputFileName = "OutputMap100.mps";

            var readFile = new MpsFile($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            MapData data = null;
            var isSuccessRead = false;
            try
            {
                data = readFile.ReadSync();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var writeFile = new MpsFile($@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;

            try
            {
                writeFile.WriteSync(data);
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
            MapFileTestItemGenerator.DeleteMapFile();
        }
    }
}