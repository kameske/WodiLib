using System;
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

            if (errorOccured) return;

            var setValue = instance.ItemName;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(itemName));
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

            if (errorOccured) return;

            var setValue = instance.SpecialSettingDesc;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(desc));
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

            if (errorOccured) return;

            var setValue = instance.ItemType;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(itemType));
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
        }
    }
}