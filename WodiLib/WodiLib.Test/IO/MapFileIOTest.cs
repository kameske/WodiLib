using System;
using NUnit.Framework;
using WodiLib.IO;

namespace WodiLib.Test
{
    [TestFixture]
    public class MapFileIOTest
    {
        [OneTimeSetUp]
        public static void SetUp()
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

            var reader = new MapFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = reader.ReadAsync().GetAwaiter().GetResult();
            Assert.IsTrue(isSuccessRead);

            var mapData = reader.MapData;

            var writer = new MapFileWriter(mapData,
                $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = writer.WriteAsync().GetAwaiter().GetResult();
            Assert.IsTrue(isSuccessWrite);

            Console.WriteLine($@"Written FileName : {MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
        }

        [Test]
        public static void SampleMapBIOTest()
        {
            const string inputFileName = "SampleMapB.mps";
            const string outputFileName = "OutputSampleMapB.mps";

            var reader = new MapFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            reader.CreateObservable().Subscribe(isSuccessRead =>
                {
                    Assert.IsTrue(isSuccessRead);

                    var mapData = reader.MapData;

                    var writer = new MapFileWriter(mapData,
                        $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
                    writer.CreateObservable().Subscribe(isSuccessWrite =>
                        {
                            Assert.IsTrue(isSuccessWrite);

                            Console.WriteLine(
                                $@"Written FileName : {MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
                        }, error => throw new Exception("", error),
                        () => Console.WriteLine("Write Observable OnComplete."));
                }, error => throw new Exception("", error),
                () => Console.WriteLine("Read Observable OnComplete."));


        }

        [Test]
        public static void DungeonIOTest()
        {
            const string inputFileName = "Dungeon.mps";
            const string outputFileName = "OutputDungeon.mps";

            var reader = new MapFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = reader.ReadSync();
            Assert.IsTrue(isSuccessRead);

            var mapData = reader.MapData;

            var writer = new MapFileWriter(mapData, $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = writer.WriteSync();

            Assert.IsTrue(isSuccessWrite);
        }

        [Test]
        public static void Map100IOTest()
        {
            const string inputFileName = "Map100.mps";
            const string outputFileName = "OutputMap100.mps";

            var reader = new MapFileReader($@"{MapFileTestItemGenerator.TestWorkRootDir}\{inputFileName}");
            var isSuccessRead = reader.ReadSync();
            Assert.IsTrue(isSuccessRead);

            var mapData = reader.MapData;

            var writer = new MapFileWriter(mapData, $@"{MapFileTestItemGenerator.TestWorkRootDir}\{outputFileName}");
            var isSuccessWrite = writer.WriteSync();

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