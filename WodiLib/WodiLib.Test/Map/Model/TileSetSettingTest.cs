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
    public class TileSetSettingTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(-1, -1, -1, false)]
        [TestCase(30, -1, -1, true)]
        [TestCase(-1, 30, -1, true)]
        [TestCase(-1, -1, 15, false)]
        [TestCase(30, 30, -1, false)]
        [TestCase(30, 30, 15, false)]
        [TestCase(30, 31, -1, true)]
        [TestCase(30, 29, -1, true)]
        public static void ConstructorTest(
            int tileTagNumberLength, int tilePathSettingLength,
            int autoTileFileNameLength, bool isError)
        {
            TileSetSetting instance = null;
            var tileTagNumbers = MakeTileTagNumberList(tileTagNumberLength);
            var tilePathSettings = MakeTilePathSettingList(tilePathSettingLength);
            var autoTileFileNames = MakeAutoTileFileNameList(autoTileFileNameLength);

            var errorOccured = false;
            try
            {
                instance = new TileSetSetting(tileTagNumbers, tilePathSettings, autoTileFileNames);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットした値と各プロパティの値が一致すること
            if (tileTagNumbers != null)
            {
                Assert.IsTrue(instance.TileTagNumberList.Equals(tileTagNumbers));
            }
            else
            {
                Assert.NotNull(instance.TileTagNumberList);
            }

            if (tilePathSettings != null)
            {
                Assert.IsTrue(instance.TilePathSettingList.Equals(tilePathSettings));
            }
            else
            {
                Assert.NotNull(instance.TilePathSettingList);
            }

            if (autoTileFileNames != null)
            {
                Assert.IsTrue(instance.AutoTileFileNameList.Equals(autoTileFileNames));
            }
            else
            {
                Assert.NotNull(instance.AutoTileFileNameList);
            }
        }

        private static readonly object[] NameTestCaseSource =
        {
            new object[] {new TileSetName(""), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(NameTestCaseSource))]
        public static void NameTest(TileSetName name, bool isError)
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Name = name;
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
                var setValue = instance.Name;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(name));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileSetSetting.Name)));
            }
        }

        private static readonly object[] BaseTileSetFileNameTestCaseSource =
        {
            new object[] {new BaseTileSetFileName(""), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(BaseTileSetFileNameTestCaseSource))]
        public static void BaseTileSetFileNameTest(BaseTileSetFileName fileName, bool isError)
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.BaseTileSetFileName = fileName;
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
                var setValue = instance.BaseTileSetFileName;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(fileName));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(TileSetSetting.BaseTileSetFileName)));
            }
        }

        [Test]
        public static void AutoTileFileNameListTest()
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.AutoTileFileNameList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void TileTagNumberListTest()
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.TileTagNumberList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void TilePathSettingListTest()
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.TilePathSettingList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SettingLengthTest()
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var length = 0;

            var errorOccured = false;
            try
            {
                length = instance.SettingLength;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した結果が意図した値であること
            Assert.AreEqual(length, instance.TileTagNumberList.Count);
            Assert.AreEqual(length, instance.TilePathSettingList.Count);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ChangeSettingLengthTest()
        {
            var instance = new TileSetSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedTileTagNumberListPropertyList = new List<string>();
            instance.TileTagNumberList.PropertyChanged += (sender, args) =>
            {
                changedTileTagNumberListPropertyList.Add(args.PropertyName);
            };
            var changedTileTagNumberListCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.TileTagNumberList.CollectionChanged += (sender, args) =>
            {
                changedTileTagNumberListCollectionList.Add(args);
            };
            var changedTilePathSettingListPropertyList = new List<string>();
            instance.TilePathSettingList.PropertyChanged += (sender, args) =>
            {
                changedTilePathSettingListPropertyList.Add(args.PropertyName);
            };
            var changedTilePathSettingListCollectionList = new List<NotifyCollectionChangedEventArgs>();
            instance.TilePathSettingList.CollectionChanged += (sender, args) =>
            {
                changedTilePathSettingListCollectionList.Add(args);
            };

            const int length = 30;

            var errorOccured = false;
            try
            {
                instance.ChangeSettingLength(length);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した結果が意図した値であること
            Assert.AreEqual(length, instance.SettingLength);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedTileTagNumberListPropertyList.Count, 2);
            Assert.IsTrue(changedTileTagNumberListPropertyList[0].Equals(nameof(instance.TileTagNumberList.Count)));
            Assert.IsTrue(changedTileTagNumberListPropertyList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedTileTagNumberListCollectionList.Count, 1);
            Assert.IsTrue(changedTileTagNumberListCollectionList[0].Action == NotifyCollectionChangedAction.Add);
            Assert.AreEqual(changedTilePathSettingListPropertyList.Count, 2);
            Assert.IsTrue(changedTilePathSettingListPropertyList[0].Equals(nameof(instance.TilePathSettingList.Count)));
            Assert.IsTrue(changedTilePathSettingListPropertyList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedTilePathSettingListCollectionList.Count, 1);
            Assert.IsTrue(changedTilePathSettingListCollectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new TileSetSetting
            {
                Name = "Name"
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }


        private static TileTagNumberList MakeTileTagNumberList(int length)
        {
            if (length == -1) return null;

            var list = new List<TileTagNumber>();

            for (var i = 0; i < length; i++)
            {
                list.Add(new TileTagNumber());
            }

            return new TileTagNumberList(list);
        }

        private static TilePathSettingList MakeTilePathSettingList(int length)
        {
            if (length == -1) return null;

            var list = new List<TilePathSetting>();

            for (var i = 0; i < length; i++)
            {
                list.Add(new TilePathSetting());
            }

            return new TilePathSettingList(list);
        }

        private static AutoTileFileNameList MakeAutoTileFileNameList(int length)
        {
            if (length == -1) return null;

            var list = new List<AutoTileFileName>();

            for (var i = 0; i < length; i++)
            {
                list.Add(new AutoTileFileName(""));
            }

            return new AutoTileFileNameList(list);
        }
    }
}