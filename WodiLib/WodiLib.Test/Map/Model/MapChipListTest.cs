using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapChipListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }


        [Test]
        public static void ConstructorTestA()
        {
            MapChipList instance = null;

            var errorOccured = false;
            try
            {
                instance = new MapChipList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が20件であること
            Assert.AreEqual(instance.Count, 20);
        }

        private static readonly object[] ConstructorTestBCaseSource =
        {
            new object[] {(MapSizeWidth) 20, (MapSizeHeight) 15},
            new object[] {(MapSizeWidth) 30, (MapSizeHeight) 31},
        };

        [TestCaseSource(nameof(ConstructorTestBCaseSource))]
        public static void ConstructorTestB(MapSizeWidth width, MapSizeHeight height)
        {
            MapChipList instance = null;

            var errorOccured = false;
            try
            {
                instance = new MapChipList(width, height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が一致すること
            Assert.AreEqual(instance.Count, width.ToInt());
            foreach (var columns in instance)
            {
                Assert.AreEqual(columns.Count, height.ToInt());
            }
        }

        [TestCase(-1, false, 0, true)]
        [TestCase(-1, false, 14, true)]
        [TestCase(-1, false, 15, true)]
        [TestCase(0, false, 0, true)]
        [TestCase(0, false, 14, true)]
        [TestCase(0, false, 15, true)]
        [TestCase(19, false, 0, true)]
        [TestCase(19, false, 14, true)]
        [TestCase(19, false, 15, true)]
        [TestCase(20, false, 0, true)]
        [TestCase(20, false, 14, true)]
        [TestCase(20, false, 15, false)]
        [TestCase(20, true, 0, true)]
        [TestCase(20, true, 14, true)]
        [TestCase(20, true, 15, true)]
        public static void ConstructorTestC(int initWidth, bool hasNullInWidth,
            int initHeight, bool isError)
        {
            MapChipList instance = null;

            var chips = MakeMapChipList(initWidth, hasNullInWidth, initHeight);

            var errorOccured = false;
            try
            {
                instance = new MapChipList(chips);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            Assert.NotNull(instance);

            // 件数が一致すること
            Assert.AreEqual(instance.Count, initWidth);
            foreach (var columns in instance)
            {
                Assert.AreEqual(columns.Count, initHeight);
            }
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new MapChipList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapChipList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new MapChipList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapChipList.MinCapacity);
        }

        [Test]
        public static void InitializeChipsTestA()
        {
            var instance = new MapChipList();
            var beforeWidth = instance.Width;
            var beforeHeight = instance.Height;

            var errorOccured = false;
            try
            {
                instance.InitializeChips();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が変化していないこと
            Assert.AreEqual(instance.Width, beforeWidth);
            Assert.AreEqual(instance.Height, beforeHeight);

            // すべての要素がデフォルト値で初期化されていること
            foreach (var columns in instance)
            foreach (var chip in columns)
            {
                Assert.AreEqual(chip, MapChip.Default);
            }
        }

        private static readonly object[] InitializeChipsTestBCaseSource =
        {
            new object[] {(MapSizeWidth) 20, (MapSizeHeight) 15},
            new object[] {(MapSizeWidth) 33, (MapSizeHeight) 31},
        };

        [TestCaseSource(nameof(InitializeChipsTestBCaseSource))]
        public static void InitializeChipsTestB(
            MapSizeWidth width, MapSizeHeight height)
        {
            var instance = new MapChipList();

            var errorOccured = false;
            try
            {
                instance.InitializeChips(width, height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が意図した値と一致すること
            Assert.AreEqual(instance.Width, width);
            Assert.AreEqual(instance.Height, height);

            // すべての要素がデフォルト値で初期化されていること
            foreach (var columns in instance)
            foreach (var chip in columns)
            {
                Assert.AreEqual(chip, MapChip.Default);
            }
        }

        private static readonly object[] UpdateWidthTestCaseSource =
        {
            new object[] {30, 20, (MapSizeWidth) 25},
            new object[] {30, 20, (MapSizeWidth) 30},
            new object[] {30, 20, (MapSizeWidth) 35},
        };

        [TestCaseSource(nameof(UpdateWidthTestCaseSource))]
        public static void UpdateWidthTest(int initWidth,
            int initHeight, MapSizeWidth width)
        {
            var chips = MakeMapChipList(initWidth, false, initHeight);

            var instance = new MapChipList(chips);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedCollectionList.Add(args); };
            var changedColumnsPropertyList = new List<string>();
            var changedColumnsCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.ForEach(column =>
            {
                column.PropertyChanged += (sender, args) => { changedColumnsPropertyList.Add(args.PropertyName); };
                column.CollectionChanged += (sender, args) => { changedColumnsCollectionList.Add(args); };
            });

            var errorOccured = false;
            try
            {
                instance.UpdateWidth(width);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が意図した値と一致すること
            Assert.AreEqual(instance.Width, width);
            Assert.AreEqual(instance.Height.ToInt(), initHeight);

            var notChangedLength = initWidth < width ? initWidth : width.ToInt();

            var i = 0;
            // 操作しなかった項目がもとの値のままであること
            for (; i < notChangedLength; i++)
            for (var j = 0; j < instance.Height; j++)
            {
                Assert.AreEqual(instance[i][j], MakeMapChip(i, j));
            }

            // 新たに追加された項目がデフォルト値であること
            for (; i < instance.Count; i++)
            for (var j = 0; j < instance.Height; j++)
            {
                Assert.AreEqual(instance[i][j], MapChip.Default);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 3);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(instance.Count)));
            Assert.IsTrue(changedPropertyList[1].Equals(ListConstant.IndexerName));
            Assert.IsTrue(changedPropertyList[2].Equals(nameof(instance.Width)));
            if (initWidth == width)
            {
                Assert.AreEqual(changedCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedCollectionList.Count, 1);
                if (initWidth > width)
                {
                    Assert.IsTrue(changedCollectionList[0].Action == NotifyCollectionChangedAction.Remove);
                    Assert.IsTrue(changedCollectionList[0].OldStartingIndex == width);
                    Assert.IsTrue(changedCollectionList[0].OldItems.Count == initWidth - width);
                    Assert.IsTrue(changedCollectionList[0].NewStartingIndex == -1);
                    Assert.IsTrue(changedCollectionList[0].NewItems == null);
                }
                else
                {
                    Assert.IsTrue(changedCollectionList[0].Action == NotifyCollectionChangedAction.Add);
                    Assert.IsTrue(changedCollectionList[0].OldStartingIndex == -1);
                    Assert.IsTrue(changedCollectionList[0].OldItems == null);
                    Assert.IsTrue(changedCollectionList[0].NewStartingIndex == initWidth);
                    Assert.IsTrue(changedCollectionList[0].NewItems.Count == width - initWidth);
                }
            }

            Assert.AreEqual(changedColumnsPropertyList.Count, 0);
            Assert.AreEqual(changedColumnsCollectionList.Count, 0);
        }

        private static readonly object[] UpdateHeightTestCaseSource =
        {
            new object[] {30, 20, (MapSizeHeight) 15},
            new object[] {30, 20, (MapSizeHeight) 20},
            new object[] {30, 20, (MapSizeHeight) 25},
        };

        [TestCaseSource(nameof(UpdateHeightTestCaseSource))]
        public static void UpdateHeightTest(int initWidth,
            int initHeight, MapSizeHeight height)
        {
            var chips = MakeMapChipList(initWidth, false, initHeight);

            var instance = new MapChipList(chips);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedCollectionList.Add(args); };
            var changedColumnsPropertyList = new List<string>();
            var changedColumnsCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.ForEach(column =>
            {
                column.PropertyChanged += (sender, args) => { changedColumnsPropertyList.Add(args.PropertyName); };
                column.CollectionChanged += (sender, args) => { changedColumnsCollectionList.Add(args); };
            });

            var errorOccured = false;
            try
            {
                instance.UpdateHeight(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が意図した値と一致すること
            Assert.AreEqual(instance.Width.ToInt(), initWidth);
            Assert.AreEqual(instance.Height, height);

            var notChangedLength = initHeight < height ? initHeight : height.ToInt();

            for (var i = 0; i < instance.Width; i++)
            {
                var j = 0;
                // 操作しなかった項目がもとの値のままであること
                for (; j < notChangedLength; j++)
                {
                    Assert.AreEqual(instance[i][j], MakeMapChip(i, j));
                }

                // 新たに追加された項目がデフォルト値であること
                for (; j < instance.Height; j++)
                {
                    Assert.AreEqual(instance[i][j], MapChip.Default);
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapChipList.Height)));
            Assert.AreEqual(changedCollectionList.Count, 0);
            Assert.AreEqual(changedColumnsPropertyList.Count, initWidth * 2);
            for (var i = 0; i < initWidth; i++)
            {
                Assert.IsTrue(changedColumnsPropertyList[2 * i].Equals(nameof(IFixedLengthMapChipColumns.Count)));
                Assert.IsTrue(changedColumnsPropertyList[2 * i + 1].Equals(ListConstant.IndexerName));
            }

            if (initHeight == height)
            {
                Assert.AreEqual(changedColumnsCollectionList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedColumnsCollectionList.Count, initWidth);
                if (initHeight > height)
                {
                    for (var i = 0; i < initWidth; i++)
                    {
                        Assert.IsTrue(changedColumnsCollectionList[i].Action == NotifyCollectionChangedAction.Remove);
                        Assert.IsTrue(changedColumnsCollectionList[i].OldStartingIndex == height);
                        Assert.IsTrue(changedColumnsCollectionList[i].OldItems.Count == initHeight - height);
                        Assert.IsTrue(changedColumnsCollectionList[i].NewStartingIndex == -1);
                        Assert.IsTrue(changedColumnsCollectionList[i].NewItems == null);
                    }
                }
                else
                {
                    for (var i = 0; i < initWidth; i++)
                    {
                        Assert.IsTrue(changedColumnsCollectionList[i].Action == NotifyCollectionChangedAction.Add);
                        Assert.IsTrue(changedColumnsCollectionList[i].OldStartingIndex == -1);
                        Assert.IsTrue(changedColumnsCollectionList[i].OldItems == null);
                        Assert.IsTrue(changedColumnsCollectionList[i].NewStartingIndex == initHeight);
                        Assert.IsTrue(changedColumnsCollectionList[i].NewItems.Count == height - initHeight);
                    }
                }
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var chips = MakeMapChipList(30, false, 40);
            var target = new MapChipList(chips);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }


        private static IReadOnlyList<IReadOnlyList<MapChip>> MakeMapChipList(int width,
            bool hasNullInWidth, int height)
        {
            if (width == -1) return null;

            return Enumerable.Range(0, width).Select(w =>
            {
                if (hasNullInWidth && w == width / 2) return null;
                return Enumerable.Range(0, height)
                    .Select(h => MakeMapChip(w, h))
                    .ToList();
            }).ToList();
        }

        private static MapChip MakeMapChip(int width, int height)
        {
            return width * 100 + height;
        }
    }
}