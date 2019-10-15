using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseTypeDesc_FactoryTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void CreateForDBTypeSetATest()
        {
            DatabaseTypeDesc instance = null;

            var errorOccured = false;
            try
            {
                instance = DatabaseTypeDesc.Factory.CreateForDBTypeSet();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 初期データ数が一致すること
            Assert.AreEqual(instance.WritableItemValuesList.Count, 1);
            Assert.AreEqual(instance.WritableItemValuesList.Count, instance.DataDescList.Count);
            Assert.AreEqual(instance.WritableItemValuesList.Count, instance.DataNameList.Count);
            // 初期項目数が一致すること
            Assert.AreEqual(instance.WritableItemSettingList.Count, 0);
            Assert.AreEqual(instance.WritableItemSettingList.Count, instance.ItemDescList.Count);
            Assert.AreEqual(instance.WritableItemSettingList.Count, instance.WritableItemValuesList[0].Count);

            instance.WritableItemValuesList.AddNewValues();

            // データ追加後、データ数が一致すること
            Assert.AreEqual(instance.WritableItemValuesList.Count, 2);
            Assert.AreEqual(instance.WritableItemValuesList.Count, instance.DataDescList.Count);
            Assert.AreEqual(instance.WritableItemValuesList.Count, instance.DataNameList.Count);

            var itemSetting = new DBItemSetting
            {
                ItemName = "ItemName",
                ItemType = DBItemType.String,
            };

            instance.WritableItemSettingList.Add(itemSetting);

            // 項目追加後、項目数が一致すること
            Assert.AreEqual(instance.WritableItemSettingList.Count, 1);
            Assert.AreEqual(instance.WritableItemSettingList.Count, instance.DBItemSettingList.Count);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void CreateForDBTypeSetBTest(bool isNullItemSettingList,
            bool isError)
        {
            var itemSettingList = isNullItemSettingList
                ? null
                : new DBItemSettingList
                {
                    new DBItemSetting()
                };
            DatabaseTypeDesc instance = null;

            var errorOccured = false;
            try
            {
                instance = DatabaseTypeDesc.Factory.CreateForDBTypeSet(itemSettingList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;
            Assert.NotNull(itemSettingList);

            // 初期データ数が意図した値であること
            Assert.AreEqual(instance.DataDescList.Count, 1);
            // 初期項目数が一致すること
            Assert.AreEqual(instance.ItemDescList.Count, 1);
            Assert.AreEqual(instance.ItemDescList.Count, itemSettingList.Count);
        }

        [Test]
        public static void CreateForDBTypeTest()
        {
            DatabaseTypeDesc instance = null;

            var errorOccured = false;
            try
            {
                instance = DatabaseTypeDesc.Factory.CreateForDBType();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 初期データ数が一致すること
            Assert.AreEqual(instance.DataDescList.Count, 1);
            Assert.AreEqual(instance.DataDescList.Count, instance.DataNameList.Count);
            // 初期項目数が一致すること
            Assert.AreEqual(instance.ItemDescList.Count, 0);
            Assert.AreEqual(instance.ItemDescList.Count, instance.DBItemSettingList.Count);

            var valueList = new DBItemValueList();
            var dataDesc = new DatabaseDataDesc("DataName", valueList);
            instance.DataDescList.Add(dataDesc);

            // データ追加後、データ数が一致すること
            Assert.AreEqual(instance.DataDescList.Count, 2);
            Assert.AreEqual(instance.DataDescList.Count, instance.DataNameList.Count);

            var itemDesc = new DatabaseItemDesc
            {
                ItemName = "ItemName",
                ItemType = DBItemType.String,
            };

            instance.ItemDescList.Add(itemDesc);

            // 項目追加後、項目数が一致すること
            Assert.AreEqual(instance.ItemDescList.Count, 1);
            Assert.AreEqual(instance.ItemDescList.Count, instance.DBItemSettingList.Count);
        }

        [Test]
        public static void CreateForDBDataTest()
        {
            DatabaseTypeDesc instance = null;

            var errorOccured = false;
            try
            {
                instance = DatabaseTypeDesc.Factory.CreateForDBData();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 初期データ数が一致すること
            Assert.AreEqual(instance.WritableItemValuesList.Count, 1);
            Assert.AreEqual(instance.WritableItemValuesList.Count, instance.DataDescList.Count);
            Assert.AreEqual(instance.WritableItemValuesList.Count, instance.DataNameList.Count);
            // 初期項目数が一致すること
            Assert.AreEqual(instance.WritableItemValuesList[0].Count, 0);
            Assert.AreEqual(instance.WritableItemValuesList[0].Count, instance.ItemDescList.Count);
            Assert.AreEqual(instance.WritableItemValuesList[0].Count, instance.DBItemSettingList.Count);

            instance.WritableDataNameList.Add("DataName");

            // データ追加後、データ数が一致すること
            Assert.AreEqual(instance.WritableDataNameList.Count, 2);
            Assert.AreEqual(instance.WritableDataNameList.Count, instance.DataNameList.Count);
            Assert.AreEqual(instance.DataNameList.Count, instance.DataDescList.Count);
            Assert.AreEqual(instance.DataNameList.Count, instance.WritableItemValuesList.Count);

            instance.WritableItemValuesList.AddField(DBItemType.Int);

            // 項目追加後、項目数が一致すること
            Assert.AreEqual(instance.WritableItemValuesList[0].Count, 1);
            Assert.AreEqual(instance.WritableItemValuesList[0].Count, instance.ItemDescList.Count);
            Assert.AreEqual(instance.WritableItemValuesList[0].Count, instance.DBItemSettingList.Count);
        }
    }
}