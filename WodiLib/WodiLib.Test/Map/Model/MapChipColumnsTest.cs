using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapChipColumnsTest
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
            MapChipColumns instance = null;

            var errorOccured = false;
            try
            {
                instance = new MapChipColumns();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が15件であること
            Assert.AreEqual(instance.Count, 15);
        }

        private static readonly object[] ConstructorTestBCaseSource =
        {
            new object[] {(MapSizeHeight) 15},
            new object[] {(MapSizeHeight) 31},
        };

        [TestCaseSource(nameof(ConstructorTestBCaseSource))]
        public static void ConstructorTestB(MapSizeHeight height)
        {
            MapChipColumns instance = null;

            var errorOccured = false;
            try
            {
                instance = new MapChipColumns(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が一致すること
            Assert.AreEqual(instance.Count, height.ToInt());

            // すべての要素がデフォルト値で初期化されていること
            foreach (var chip in instance)
            {
                Assert.AreEqual(chip, MapChip.Default);
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(14, true)]
        [TestCase(15, false)]
        public static void ConstructorTestC(int initLength, bool isError)
        {
            MapChipColumns instance = null;

            var chips = MakeMapChipList(initLength);

            var errorOccured = false;
            try
            {
                instance = new MapChipColumns(chips);
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
            Assert.AreEqual(instance.Count, initLength);

            // すべての要素がセットした値になっていること
            for (var i = 0; i < instance.Count; i++)
            {
                Assert.AreEqual(instance[i], MakeMapChip(i));
            }
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new MapChipColumns();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapChipColumns.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new MapChipColumns();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, MapChipColumns.MinCapacity);
        }

        [Test]
        public static void InitializeChipsTestA()
        {
            var instance = new MapChipColumns();
            var beforeCount = instance.Count;

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
            Assert.AreEqual(instance.Count, beforeCount);

            // すべての要素がデフォルト値で初期化されていること
            foreach (var chip in instance)
            {
                Assert.AreEqual(chip, MapChip.Default);
            }
        }


        private static readonly object[] InitializeChipsTestBCaseSource =
        {
            new object[] {(MapSizeHeight) 15},
            new object[] {(MapSizeHeight) 31},
        };

        [TestCaseSource(nameof(InitializeChipsTestBCaseSource))]
        public static void InitializeChipsTestB(MapSizeHeight height)
        {
            var instance = new MapChipColumns();

            var errorOccured = false;
            try
            {
                instance.InitializeChips(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が意図した値と一致すること
            Assert.AreEqual(instance.Count, height.ToInt());

            // すべての要素がデフォルト値で初期化されていること
            foreach (var chip in instance)
            {
                Assert.AreEqual(chip, MapChip.Default);
            }
        }

        private static readonly object[] UpdateSizeTestCaseSource =
        {
            new object[] {20, (MapSizeHeight) 15},
            new object[] {20, (MapSizeHeight) 20},
            new object[] {20, (MapSizeHeight) 31},
        };

        [TestCaseSource(nameof(UpdateSizeTestCaseSource))]
        public static void UpdateSizeTest(int initLength, MapSizeHeight height)
        {
            var chips = MakeMapChipList(initLength);

            var instance = new MapChipColumns(chips);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (sender, args) => { changedCollectionList.Add(args); };

            var errorOccured = false;
            try
            {
                instance.UpdateSize(height);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 件数が意図した値と一致すること
            Assert.AreEqual(instance.Count, height.ToInt());

            var notChangedLength = initLength < height ? initLength : height.ToInt();

            var i = 0;
            // 操作しなかった項目がもとの値のままであること
            for (; i < notChangedLength; i++)
            {
                Assert.AreEqual(instance[i], MakeMapChip(i));
            }

            // 新たに追加された項目がデフォルト値であること
            for (; i < instance.Count; i++)
            {
                Assert.AreEqual(instance[i], MapChip.Default);
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapChipColumns
            {
                [3] = 10,
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static IReadOnlyList<MapChip> MakeMapChipList(int length)
        {
            if (length == -1) return null;

            return Enumerable.Range(0, length).Select(MakeMapChip).ToList();
        }

        private static MapChip MakeMapChip(int index)
        {
            return index;
        }
    }
}