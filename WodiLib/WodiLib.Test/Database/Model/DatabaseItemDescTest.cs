using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseItemDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] ItemNameTestCaseSource =
        {
            new object[] {(ItemName) "itemName", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(ItemNameTestCaseSource))]
        public static void ItemNameTest(ItemName itemName, bool isError)
        {
            var instance = new DatabaseItemDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ItemName = itemName;
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
                var setValue = instance.ItemName;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(itemName));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseItemDesc.ItemName)));
            }
        }

        private static readonly object[] SpecialSettingDescTestCaseSource =
        {
            new object[]
            {
                new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal,
                    new[] {new DatabaseValueCase(0, "")}),
                false
            },
            new object[] {null, true},
        };

        [TestCaseSource(nameof(SpecialSettingDescTestCaseSource))]
        public static void SpecialSettingDescTest(DBItemSpecialSettingDesc desc, bool isError)
        {
            var instance = new DatabaseItemDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SpecialSettingDesc = desc;
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
                var setValue = instance.SpecialSettingDesc;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(desc));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseItemDesc.SpecialSettingDesc)));
            }
        }

        private static readonly object[] ValueTestCaseSource =
        {
            new object[] {DBItemType.Int, false},
            new object[] {DBItemType.String, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(ValueTestCaseSource))]
        public static void ValueTest(DBItemType itemType, bool isError)
        {
            var instance = new DatabaseItemDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ItemType = itemType;
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
                var setValue = instance.ItemType;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(itemType));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseItemDesc.ItemType)));
            }
        }

        [Test]
        public static void ToDBItemSettingTest()
        {
            // テスト用要素
            var itemName = (ItemName) "testItemName";
            var specialSettingDesc = new DBItemSpecialSettingDesc(DBItemSpecialSettingType.Normal,
                new[] {new DatabaseValueCase(0, "case")});
            var itemType = DBItemType.String;

            var instance = new DatabaseItemDesc();
            instance.ItemName = itemName;
            instance.SpecialSettingDesc = specialSettingDesc;
            instance.ItemType = itemType;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBItemSetting result = null;

            var errorOccured = false;
            try
            {
                result = instance.ToDBItemSetting();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var answer = new DBItemSetting
            {
                ItemName = itemName,
                ItemType = itemType,
                SpecialSettingDesc = specialSettingDesc
            };

            // 取得した結果が意図した値であること
            Assert.IsTrue(result.Equals(answer));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DatabaseItemDesc
            {
                ItemName = "ItemName"
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}