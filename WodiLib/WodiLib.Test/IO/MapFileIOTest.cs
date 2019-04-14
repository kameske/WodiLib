using System;
using NUnit.Framework;
using WodiLib.IO;
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
            // テスト用マップファイル出力
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

            var mapData = reader.MapData;

            var writer = new MpsFileWriter(mapData,
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

            Console.WriteLine($@"Written FilePath : {MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void SampleMapBIOTest()
        {
            const string inputFileName = "SampleMapB.mps";
            const string outputFileName = "OutputSampleMapB.mps";

            var reader = new MpsFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
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

            var mapData = reader.MapData;

            var writer = new MpsFileWriter(mapData,
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

            Console.WriteLine($@"Written FilePath : {MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void DungeonIOTest()
        {
            const string inputFileName = "Dungeon.mps";
            const string outputFileName = "OutputDungeon.mps";

            var reader = new MpsFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
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

            var mapData = reader.MapData;

            var writer = new MpsFileWriter(mapData, $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
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

        [Test]
        public static void Map100IOTest()
        {
            const string inputFileName = "Map100.mps";
            const string outputFileName = "OutputMap100.mps";

            var readFile = new MpsFile($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = false;
            try
            {
                readFile.ReadSync();
                isSuccessRead = true;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
            }

            Assert.IsTrue(isSuccessRead);

            var mapData = readFile.MapData;

            var writeFile = new MpsFile($@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = false;

            try
            {
                writeFile.WriteSync(mapData);
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
            MapFileTestItemGenerator.DeleteMapFile();
        }
    }
}