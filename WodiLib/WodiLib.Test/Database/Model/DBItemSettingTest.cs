using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBItemSettingTest
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
            new object[] {(ItemName) "", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(ItemNameTestCaseSource))]
        public static void ItemNameTest(ItemName itemName, bool isSetError)
        {
            var instance = new DBItemSetting();
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
            Assert.AreEqual(errorOccured, isSetError);

            errorOccured = false;

            ItemName getResult = null;

            try
            {
                getResult = instance.ItemName;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            if (!isSetError)
            {
                // 設定した値と取得した値が一致すること
                Assert.AreEqual(getResult, itemName);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (isSetError)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSetting.ItemName)));
            }
        }


        private static readonly object[] SpecialSettingDescTestCaseSource =
        {
            new object[] {new DBItemSpecialSettingDesc(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(SpecialSettingDescTestCaseSource))]
        public static void SpecialSettingDescTest(DBItemSpecialSettingDesc desc, bool isSetError)
        {
            var instance = new DBItemSetting();
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
            Assert.AreEqual(errorOccured, isSetError);

            errorOccured = false;
            try
            {
                var _ = instance.SpecialSettingDesc;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            if (isSetError)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSetting.SpecialSettingDesc)));
            }
        }

        private static readonly object[] ItemTypeTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, DBItemType.Int, false},
            new object[] {DBItemSpecialSettingType.Normal, DBItemType.String, false},
            new object[] {DBItemSpecialSettingType.Normal, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, DBItemType.Int, true},
            new object[] {DBItemSpecialSettingType.LoadFile, DBItemType.String, false},
            new object[] {DBItemSpecialSettingType.LoadFile, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, DBItemType.Int, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, DBItemType.String, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, true},
            new object[] {DBItemSpecialSettingType.Manual, DBItemType.Int, false},
            new object[] {DBItemSpecialSettingType.Manual, DBItemType.String, true},
            new object[] {DBItemSpecialSettingType.Manual, null, true},
        };

        [TestCaseSource(nameof(ItemTypeTestCaseSource))]
        public static void ItemTypeTest(DBItemSpecialSettingType settingType, DBItemType type, bool isSetError)
        {
            var specialDesc = new DBItemSpecialSettingDesc();
            specialDesc.ChangeValueType(settingType, null);

            var instance = new DBItemSetting
            {
                SpecialSettingDesc = specialDesc
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ItemType = type;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isSetError);

            errorOccured = false;
            try
            {
                var _ = instance.SpecialSettingDesc;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            if (isSetError)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSetting.ItemType)));
            }
        }

        [TestCase("Same")]
        [TestCase("Null")]
        [TestCase("Diff_ItemName")]
        [TestCase("Diff_ItemType")]
        [TestCase("Diff_Desc")]
        public static void EqualsTest(string testTypeCode)
        {
            var specialDesc = new DBItemSpecialSettingDesc();
            specialDesc.ChangeValueType(DBItemSpecialSettingType.Normal, null);

            var target = new DBItemSetting
            {
                ItemName = "ItemName",
                ItemType = DBItemType.Int,
                SpecialSettingDesc = specialDesc
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBItemSetting another = null;
            bool result = false;
            switch (testTypeCode)
            {
                case "Same":
                    another = target;
                    result = true;
                    break;
                case "Null":
                    // another は null で初期化されているため、何もしない
                    break;
                case "Diff_ItemName":
                    another = new DBItemSetting
                    {
                        ItemName = "ItemName_Diff",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = specialDesc
                    };
                    break;
                case "Diff_ItemType":
                    another = new DBItemSetting
                    {
                        ItemName = "ItemName",
                        ItemType = DBItemType.String,
                        SpecialSettingDesc = specialDesc,
                    };
                    break;
                case "Diff_Desc":
                    var anotherSpecialDesc = new DBItemSpecialSettingDesc();
                    anotherSpecialDesc.ChangeValueType(DBItemSpecialSettingType.LoadFile, null);
                    another = new DBItemSetting
                    {
                        ItemName = "ItemName",
                        ItemType = DBItemType.Int,
                        SpecialSettingDesc = anotherSpecialDesc
                    };
                    break;
                default:
                    Assert.Fail();
                    break;
            }

            // 結果が意図した値と一致すること
            Assert.AreEqual(target.Equals(another), result);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DBItemSetting
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