using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapDataTest
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

        [Test]
        public static void SetMemoTest()
        {
            var instance = new MapData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var value = (MapDataMemo) "test";
            var errorOccured = false;
            try
            {
                instance.Memo = value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Memo)));
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(2, false)]
        [TestCase(3, true)]
        public static void GetLayerTest(int index, bool isError)
        {
            var layer1 = new Layer();
            var layer2 = new Layer();
            var layer3 = new Layer();

            var instance = new MapData
            {
                Layer1 = layer1,
                Layer2 = layer2,
                Layer3 = layer3,
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            Layer getResult = null;
            try
            {
                getResult = instance.GetLayer(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                Layer setObject;
                switch (index)
                {
                    case 0:
                        setObject = layer1;
                        break;
                    case 1:
                        setObject = layer2;
                        break;
                    default:
                        setObject = layer3;
                        break;
                }

                // 取得したインスタンスが初期化内容と一致すること
                Assert.AreSame(getResult, setObject);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(-1, false, true)]
        [TestCase(0, false, false)]
        [TestCase(0, true, true)]
        [TestCase(2, false, false)]
        [TestCase(3, false, true)]
        public static void SetLayerTest(int index, bool isNull, bool isError)
        {
            var instance = new MapData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            Layer layer = null;
            if (!isNull) layer = new Layer();
            var errorOccured = false;
            try
            {
                instance.SetLayer(index, layer);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // 内容が正しくセットされていること
                Layer setObject;
                switch (index)
                {
                    case 0:
                        setObject = instance.Layer1;
                        break;
                    case 1:
                        setObject = instance.Layer2;
                        break;
                    default:
                        setObject = instance.Layer3;
                        break;
                }

                Assert.AreSame(setObject, layer);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(changedPropertyList.Count, 3);
                        Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer1)));
                        Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapData.MapSizeWidth)));
                        Assert.IsTrue(changedPropertyList[2].Equals(nameof(MapData.MapSizeHeight)));
                        break;
                    case 1:
                        Assert.AreEqual(changedPropertyList.Count, 1);
                        Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer2)));
                        break;
                    case 2:
                        Assert.AreEqual(changedPropertyList.Count, 1);
                        Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer3)));
                        break;
                }
            }
        }

        [TestCase(0, false, false)]
        [TestCase(0, true, true)]
        [TestCase(1, false, false)]
        [TestCase(1, true, true)]
        [TestCase(2, false, false)]
        [TestCase(2, true, true)]
        public static void SetLayerTest2(int index, bool isNull, bool isError)
        {
            var instance = new MapData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                switch (index)
                {
                    case 0:
                        instance.Layer1 = isNull ? null : new Layer();
                        break;
                    case 1:
                        instance.Layer2 = isNull ? null : new Layer();
                        break;
                    default:
                        instance.Layer3 = isNull ? null : new Layer();
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                switch (index)
                {
                    case 0:
                        Assert.AreEqual(changedPropertyList.Count, 3);
                        Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer1)));
                        Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapData.MapSizeWidth)));
                        Assert.IsTrue(changedPropertyList[2].Equals(nameof(MapData.MapSizeHeight)));
                        break;
                    case 1:
                        Assert.AreEqual(changedPropertyList.Count, 1);
                        Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer2)));
                        break;
                    case 2:
                        Assert.AreEqual(changedPropertyList.Count, 1);
                        Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer3)));
                        break;
                }
            }
        }

        [Test]
        public static void SetLayer1Test()
        {
            var instance = new MapData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var width = (MapSizeWidth) 30;
            var height = (MapSizeHeight) 24;

            var setLayer = new Layer();
            setLayer.UpdateSize(width, height);

            var errorOccured = false;
            try
            {
                instance.Layer1 = setLayer;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // マップサイズが変化していること
            Assert.AreEqual(instance.MapSizeWidth, width);
            Assert.AreEqual(instance.MapSizeHeight, height);

            // レイヤーのサイズも変化していること
            Assert.AreEqual(instance.Layer1.Width, width);
            Assert.AreEqual(instance.Layer1.Height, height);
            Assert.AreEqual(instance.Layer2.Width, width);
            Assert.AreEqual(instance.Layer2.Height, height);
            Assert.AreEqual(instance.Layer3.Width, width);
            Assert.AreEqual(instance.Layer3.Height, height);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 3);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer1)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(MapData.MapSizeWidth)));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(MapData.MapSizeHeight)));
        }

        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, false, true)]
        public static void SetLayer2Test(bool isEqualWidth, bool isEqualHeight, bool isError)
        {
            var instance = new MapData();

            var width = (MapSizeWidth) 30;
            var height = (MapSizeHeight) 24;

            instance.UpdateMapSize(width, height);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var layerWidth = isEqualWidth ? width : (MapSizeWidth) (width + 1);
            var layerHeight = isEqualHeight ? height : (MapSizeHeight) (height + 1);

            var setLayer = new Layer();
            setLayer.UpdateSize(layerWidth, layerHeight);

            var errorOccured = false;
            try
            {
                instance.Layer2 = setLayer;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // マップサイズが変化していないこと
                Assert.AreEqual(instance.MapSizeWidth, width);
                Assert.AreEqual(instance.MapSizeHeight, height);

                // レイヤーのサイズも変化していないこと
                Assert.AreEqual(instance.Layer1.Width, width);
                Assert.AreEqual(instance.Layer1.Height, height);
                Assert.AreEqual(instance.Layer2.Width, width);
                Assert.AreEqual(instance.Layer2.Height, height);
                Assert.AreEqual(instance.Layer3.Width, width);
                Assert.AreEqual(instance.Layer3.Height, height);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer2)));
            }
        }

        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(true, false, true)]
        [TestCase(false, false, true)]
        public static void SetLayer3Test(bool isEqualWidth, bool isEqualHeight, bool isError)
        {
            var instance = new MapData();

            var width = (MapSizeWidth) 30;
            var height = (MapSizeHeight) 24;

            instance.UpdateMapSize(width, height);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var layerWidth = isEqualWidth ? width : (MapSizeWidth) (width + 1);
            var layerHeight = isEqualHeight ? height : (MapSizeHeight) (height + 1);

            var setLayer = new Layer();
            setLayer.UpdateSize(layerWidth, layerHeight);

            var errorOccured = false;
            try
            {
                instance.Layer3 = setLayer;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // マップサイズが変化していないこと
                Assert.AreEqual(instance.MapSizeWidth, width);
                Assert.AreEqual(instance.MapSizeHeight, height);

                // レイヤーのサイズも変化していないこと
                Assert.AreEqual(instance.Layer1.Width, width);
                Assert.AreEqual(instance.Layer1.Height, height);
                Assert.AreEqual(instance.Layer2.Width, width);
                Assert.AreEqual(instance.Layer2.Height, height);
                Assert.AreEqual(instance.Layer3.Width, width);
                Assert.AreEqual(instance.Layer3.Height, height);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.Layer3)));
            }
        }

        [Test]
        public static void UpdateMapSizeWidthTest()
        {
            var instance = new MapData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var width = (MapSizeWidth) 30;

            var errorOccured = false;
            try
            {
                instance.UpdateMapSizeWidth(width);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // マップサイズ横が設定した値であること
            Assert.AreEqual(instance.MapSizeWidth, width);

            // 各レイヤのマップサイズ横が設定した値であること
            Assert.AreEqual(instance.Layer1.Width, width);
            Assert.AreEqual(instance.Layer2.Width, width);
            Assert.AreEqual(instance.Layer3.Width, width);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.MapSizeWidth)));
        }

        [Test]
        public static void UpdateMapSizeHeightTest()
        {
            var instance = new MapData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var height = (MapSizeHeight) 30;

            var errorOccured = false;
            try
            {
                instance.UpdateMapSizeHeight(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // マップサイズ縦が設定した値であること
            Assert.AreEqual(instance.MapSizeHeight, height);

            // 各レイヤのマップサイズ縦が設定した値であること
            Assert.AreEqual(instance.Layer1.Height, height);
            Assert.AreEqual(instance.Layer2.Height, height);
            Assert.AreEqual(instance.Layer3.Height, height);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapData.MapSizeHeight)));
        }

        [Test]
        public static void ToBinaryMap023Test()
        {
            MapFileTestItemGenerator.OutputMapFile();
            var map023Data = MapFileTestItemGenerator.GenerateMap023Data();
            var map023DataBuf = map023Data.ToBinary();

            using (var fs = new FileStream($@"{MapFileTestItemGenerator.TestWorkRootDir}\Map023.mps", FileMode.Open))
            {
                var length = (int) fs.Length;
                // ファイルサイズが規定でない場合誤作動防止の為テスト失敗にする
                Assert.AreEqual(length, 6615);

                var fileData = new byte[length];
                fs.Read(fileData, 0, length);

                // binデータ出力用
                var builder = new StringBuilder();
                foreach (var str in fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                var result = builder.ToString();
                Console.WriteLine(result);

                builder = new StringBuilder();
                foreach (var str in map023DataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                result = builder.ToString();
                Console.WriteLine(result);

                for (var i = 0; i < map023DataBuf.Length; i++)
                {
                    if (i == fileData.Length)
                        Assert.Fail(
                            $"データ長が異なります。（期待値：{fileData.Length}, 実際：{map023DataBuf.Length}）");

                    if (fileData[i] != map023DataBuf[i])
                        Assert.Fail(
                            $"offset: {i} のバイナリが異なります。（期待値：{fileData[i]}, 実際：{map023DataBuf[i]}）");
                }

                if (fileData.Length != map023DataBuf.Length)
                    Assert.Fail(
                        $"データ長が異なります。（期待値：{fileData.Length}, 実際：{map023DataBuf.Length}）");
            }
        }

        [Test]
        public static void ToBinaryMap024Test()
        {
            MapFileTestItemGenerator.OutputMapFile();
            var map024Data = MapFileTestItemGenerator.GenerateMap024Data();
            var map024DataBuf = map024Data.ToBinary();

            using (var fs = new FileStream($@"{MapFileTestItemGenerator.TestWorkRootDir}\Map024.mps", FileMode.Open))
            {
                var length = (int) fs.Length;
                // ファイルサイズが規定でない場合誤作動防止の為テスト失敗にする
                Assert.AreEqual(length, 6080);

                var fileData = new byte[length];
                fs.Read(fileData, 0, length);

                // binデータ出力用
                var builder = new StringBuilder();
                foreach (var str in fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                var result = builder.ToString();
                Console.WriteLine(result);

                builder = new StringBuilder();
                foreach (var str in map024DataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                result = builder.ToString();
                Console.WriteLine(result);

                for (var i = 0; i < map024DataBuf.Length; i++)
                {
                    if (i == fileData.Length)
                        Assert.Fail(
                            $"データ長が異なります。（期待値：{fileData.Length}, 実際：{map024DataBuf.Length}）");

                    if (fileData[i] != map024DataBuf[i])
                        Assert.Fail(
                            $"offset: {i} のバイナリが異なります。（期待値：{fileData[i]}, 実際：{map024DataBuf[i]}）");
                }

                if (fileData.Length != map024DataBuf.Length)
                    Assert.Fail(
                        $"データ長が異なります。（期待値：{fileData.Length}, 実際：{map024DataBuf.Length}）");
            }
        }

        [Test]
        public static void ToBinaryMap025Test()
        {
            MapFileTestItemGenerator.OutputMapFile();
            var map025Data = MapFileTestItemGenerator.GenerateMap025Data();
            var map025DataBuf = map025Data.ToBinary();

            using (var fs = new FileStream($@"{MapFileTestItemGenerator.TestWorkRootDir}\Map025.mps", FileMode.Open))
            {
                var length = (int) fs.Length;
                // ファイルサイズが規定でない場合誤作動防止の為テスト失敗にする
                Assert.AreEqual(length, 9211);

                var fileData = new byte[length];
                fs.Read(fileData, 0, length);

                // binデータ出力用
                var builder = new StringBuilder();
                foreach (var str in fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                var result = builder.ToString();
                Console.WriteLine(result);

                builder = new StringBuilder();
                foreach (var str in map025DataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                result = builder.ToString();
                Console.WriteLine(result);

                for (var i = 0; i < map025DataBuf.Length; i++)
                {
                    if (i == fileData.Length)
                        Assert.Fail(
                            $"データ長が異なります。（期待値：{fileData.Length}, 実際：{map025DataBuf.Length}）");

                    if (fileData[i] != map025DataBuf[i])
                        Assert.Fail(
                            $"offset: {i} のバイナリが異なります。（期待値：{fileData[i]}, 実際：{map025DataBuf[i]}）");
                }

                if (fileData.Length != map025DataBuf.Length)
                    Assert.Fail(
                        $"データ長が異なります。（期待値：{fileData.Length}, 実際：{map025DataBuf.Length}）");
            }
        }

        [Test]
        public static void ToBinaryFixMapTest()
        {
            MapFileTestItemGenerator.OutputMapFile();
            var fixMapData = MapFileTestItemGenerator.GenerateFixMapData();
            var fixMapDataBuf = fixMapData.ToBinary();

            using (var fs = new FileStream($@"{MapFileTestItemGenerator.TestWorkRootDir}\fix.mps", FileMode.Open))
            {
                var length = (int) fs.Length;
                // ファイルサイズが規定でない場合誤作動防止の為テスト失敗にする
                Assert.AreEqual(length, 4398);

                var fileData = new byte[length];
                fs.Read(fileData, 0, length);

                // binデータ出力用
                var builder = new StringBuilder();
                foreach (var str in fileData.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                var result = builder.ToString();
                Console.WriteLine(result);

                builder = new StringBuilder();
                foreach (var str in fixMapDataBuf.Select((s, index) => $"=\"[{index}] = {{byte}} {s}\""))
                {
                    builder.AppendLine(str);
                }

                result = builder.ToString();
                Console.WriteLine(result);

                for (var i = 0; i < fixMapDataBuf.Length; i++)
                {
                    if (i == fileData.Length)
                        Assert.Fail(
                            $"データ長が異なります。（期待値：{fileData.Length}, 実際：{fixMapDataBuf.Length}）");

                    if (fileData[i] != fixMapDataBuf[i])
                        Assert.Fail(
                            $"offset: {i} のバイナリが異なります。（期待値：{fileData[i]}, 実際：{fixMapDataBuf[i]}）");
                }

                if (fileData.Length != fixMapDataBuf.Length)
                    Assert.Fail(
                        $"データ長が異なります。（期待値：{fileData.Length}, 実際：{fixMapDataBuf.Length}）");
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapData
            {
                TileSetId = 2,
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            // テスト用ファイル削除
            MapFileTestItemGenerator.DeleteMapFile();
        }
    }
}