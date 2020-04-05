using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class LayerTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void PropertyTest()
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };
            var changedPropertyList = new List<string>();
            layer.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var _ = layer.Chips;
            // ここまでの処理でエラーにならないこと
            Assert.True(true);

            // 幅、高さが正しく取得できること
            Assert.AreEqual((int) layer.Width, 20);
            Assert.AreEqual((int) layer.Height, 15);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(20)]
        [TestCase(30)]
        [TestCase(40)]
        public static void UpdateWidthTest(int sizeWidth)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(30, 15))
            };
            var changedPropertyList = new List<string>();
            layer.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var width = (MapSizeWidth) sizeWidth;

            var errorOccured = false;
            try
            {
                layer.UpdateWidth(width);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティのマップサイズ横が一致すること
            Assert.AreEqual(layer.Width, width);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(Layer.Width)));
        }

        [TestCase(15)]
        [TestCase(20)]
        [TestCase(30)]
        public static void SetHeightTest(int heightSize)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 20))
            };
            var changedPropertyList = new List<string>();
            layer.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var height = (MapSizeHeight) heightSize;

            var errorOccured = false;
            try
            {
                layer.UpdateHeight(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティのマップサイズ横が一致すること
            Assert.AreEqual(layer.Height, height);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(Layer.Height)));
        }

        [Test]
        public static void UpdateSizeTest()
        {
            var instance = new MapChipList(GenerateTestChipsData(20, 20));
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var width = (MapSizeWidth) 30;
            var height = (MapSizeHeight) 24;

            var errorOccured = false;
            try
            {
                instance.UpdateSize(width, height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // サイズが変化していること
            Assert.AreEqual(instance.Width, width);
            Assert.AreEqual(instance.Height, height);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 4);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(instance.Count)));
            Assert.IsTrue(changedPropertyList[1].Equals(ListConstant.IndexerName));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(Layer.Width)));
            Assert.IsTrue(changedPropertyList[3].Equals(nameof(Layer.Height)));
        }

        private static readonly object[] SetChipTestCaseSource =
        {
            new object[] {2, 4, (MapChip) 322, false},
            new object[] {19, 4, (MapChip) 322, false},
            new object[] {20, 4, (MapChip) 322, true},
            new object[] {2, 14, (MapChip) 322, false},
            new object[] {2, 15, (MapChip) 322, true},
            // new object[] {2, 4, null, true},  /* MapChipを構造体にしたため、nullは設定不可能 */
        };

        [TestCaseSource(nameof(SetChipTestCaseSource))]
        public static void SetChipTest(int x, int y, MapChip chip, bool isError)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };
            var changedPropertyList = new List<string>();
            layer.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedChipsPropertyList = new List<string>();
            layer.Chips.PropertyChanged += (sender, args) => { changedChipsPropertyList.Add(args.PropertyName); };
            var changedChipsCollectionList = new List<NotifyCollectionChangedEventArgs>();
            layer.Chips.CollectionChanged += (sender, args) => { changedChipsCollectionList.Add(args); };
            var changedChipColumnsPropertyList = new List<string>();
            var changedChipColumnsCollectionList = new List<NotifyCollectionChangedEventArgs>();
            layer.Chips.ForEach(chipColumn =>
            {
                chipColumn.PropertyChanged += (sender, args) =>
                {
                    changedChipColumnsPropertyList.Add(args.PropertyName);
                };
                chipColumn.CollectionChanged += (sender, args) => { changedChipColumnsCollectionList.Add(args); };
            });

            var errorOccured = false;
            try
            {
                layer.Chips[x][y] = chip;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラー発生フラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // チップ番号値が指定した値になっていること
                var result = layer.Chips[x][y];
                Assert.AreEqual((int) result, (int) chip);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedChipsPropertyList.Count, 0);
            Assert.AreEqual(changedChipsCollectionList.Count, 0);
            if (errorOccured)
            {
                Assert.AreEqual(changedChipColumnsPropertyList.Count, 0);
                Assert.AreEqual(changedChipColumnsCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedChipColumnsPropertyList.Count, 1);
                Assert.IsTrue(changedChipColumnsPropertyList[0].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedChipColumnsCollectionList.Count, 1);
                Assert.IsTrue(changedChipColumnsCollectionList[0].Action == NotifyCollectionChangedAction.Replace);
            }
        }

        [TestCase(4, 2, false)]
        [TestCase(19, 2, false)]
        [TestCase(20, 2, true)]
        [TestCase(4, 14, false)]
        [TestCase(4, 15, true)]
        public static void GetChipTest(int x, int y, bool isError)
        {
            var layer = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };
            var changedPropertyList = new List<string>();
            layer.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedChipsPropertyList = new List<string>();
            layer.Chips.PropertyChanged += (sender, args) => { changedChipsPropertyList.Add(args.PropertyName); };
            var changedChipsCollectionList = new List<NotifyCollectionChangedEventArgs>();
            layer.Chips.CollectionChanged += (sender, args) => { changedChipsCollectionList.Add(args); };
            var changedChipColumnsPropertyList = new List<string>();
            var changedChipColumnsCollectionList = new List<NotifyCollectionChangedEventArgs>();
            layer.Chips.ForEach(chipColumn =>
            {
                chipColumn.PropertyChanged += (sender, args) =>
                {
                    changedChipColumnsPropertyList.Add(args.PropertyName);
                };
                chipColumn.CollectionChanged += (sender, args) => { changedChipColumnsCollectionList.Add(args); };
            });

            var errorOccured = false;
            var getChip = MapChip.Default;
            try
            {
                getChip = layer.Chips[x][y];
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
                // 座標の値が初期化した値になっていること
                Assert.AreEqual((int) getChip, x * 10 + y);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedChipsPropertyList.Count, 0);
            Assert.AreEqual(changedChipsCollectionList.Count, 0);
            Assert.AreEqual(changedChipColumnsPropertyList.Count, 0);
            Assert.AreEqual(changedChipColumnsCollectionList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new Layer
            {
                Chips = new MapChipList(GenerateTestChipsData(20, 15))
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static List<List<MapChip>> GenerateTestChipsData(int width, int height)
        {
            var testChips = new List<List<MapChip>>();
            for (var i = 0; i < width; i++)
            {
                var testChipList = new List<MapChip>();
                for (var j = 0; j < height; j++)
                {
                    var chip = (MapChip) (i * 10 + j);
                    testChipList.Add(chip);
                }

                testChips.Add(testChipList);
            }

            return testChips;
        }
    }
}