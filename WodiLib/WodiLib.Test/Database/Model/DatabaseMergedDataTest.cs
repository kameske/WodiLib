using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseMergedDataTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void TypeDescListGetterTest()
        {
            var instance = new DatabaseMergedData();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.TypeDescList;
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
        public static void ConstructorATest()
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseMergedData();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        [TestCase(3, 3, false)]
        [TestCase(2, 1, true)]
        [TestCase(-1, 1, true)]
        [TestCase(1, -1, true)]
        [TestCase(-1, -1, true)]
        public static void ConstructorBTest(int typeSettingListLength, int dataSettingListLength,
            bool isError)
        {
            var typeSettingList = CreateTypeSettingList(typeSettingListLength);
            DatabaseProject project = null;
            if (typeSettingList != null)
            {
                project = new DatabaseProject
                {
                    TypeSettingList = typeSettingList
                };
            }

            var dataSettingList = CreateDataSettingList(dataSettingListLength);
            DatabaseDat dat = null;
            if (dataSettingList != null)
            {
                dat = new DatabaseDat
                {
                    SettingList = dataSettingList
                };
            }

            var errorOccured = false;
            try
            {
                var _ = new DatabaseMergedData(dat, project);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(3, 3, false)]
        [TestCase(2, 1, true)]
        [TestCase(-1, 1, true)]
        [TestCase(1, -1, true)]
        [TestCase(-1, -1, true)]
        public static void ConstructorCTest(int typeSettingListLength, int dataSettingListLength,
            bool isError)
        {
            var typeSettingList = CreateTypeSettingList(typeSettingListLength);
            var dataSettingList = CreateDataSettingList(dataSettingListLength);

            var errorOccured = false;
            try
            {
                var _ = new DatabaseMergedData(typeSettingList, dataSettingList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] GenerateDatabaseDatTestCaseSource =
        {
            new object[] {5, DBKind.User},
            new object[] {7, null},
        };

        [TestCaseSource(nameof(GenerateDatabaseDatTestCaseSource))]
        public static void GenerateDatabaseDatTest(int listLength, DBKind dbKind)
        {
            var typeSettingList = CreateTypeSettingList(listLength);
            var dataSettingList = CreateDataSettingList(listLength);
            var instance = new DatabaseMergedData(typeSettingList, dataSettingList);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DatabaseDat result = null;
            var errorOccured = false;
            try
            {
                result = instance.GenerateDatabaseDat(dbKind);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // データ数が正しいこと
            Assert.AreEqual(result.SettingList.Count, listLength);
            // DB種別が一致すること
            Assert.AreEqual(result.DBKind, dbKind);

            for (var i = 0; i < listLength; i++)
            {
                // 内容が一致すること
                Assert.AreEqual(result.SettingList[i].TypeId, dataSettingList[i].TypeId);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] GenerateDatabaseProjectTestCaseSource =
        {
            new object[] {5, DBKind.User},
            new object[] {7, null},
        };

        [TestCaseSource(nameof(GenerateDatabaseProjectTestCaseSource))]
        public static void GenerateDatabaseProjectTest(int listLength, DBKind dbKind)
        {
            var typeSettingList = CreateTypeSettingList(listLength);
            var dataSettingList = CreateDataSettingList(listLength);
            var instance = new DatabaseMergedData(typeSettingList, dataSettingList);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DatabaseProject result = null;
            var errorOccured = false;
            try
            {
                result = instance.GenerateDatabaseProject(dbKind);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
            // DB種別が一致すること
            Assert.AreEqual(result.DBKind, dbKind);

            // データ数が正しいこと
            Assert.AreEqual(result.TypeSettingList.Count, listLength);
            for (var i = 0; i < listLength; i++)
            {
                // 内容が一致すること
                Assert.AreEqual(result.TypeSettingList[i].TypeName, typeSettingList[i].TypeName);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] GenerateDBTypeSetTestCaseSource =
        {
            new object[] {4, (TypeId) 0, false},
            new object[] {4, (TypeId) 3, false},
            new object[] {4, (TypeId) 4, true},
        };

        [TestCaseSource(nameof(GenerateDBTypeSetTestCaseSource))]
        public static void GenerateDBTypeSetTest(int listLength, TypeId typeId,
            bool isError)
        {
            var typeSettingList = CreateTypeSettingList(listLength);
            var dataSettingList = CreateDataSettingList(listLength);
            var instance = new DatabaseMergedData(typeSettingList, dataSettingList);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBTypeSet result = null;
            var errorOccured = false;
            try
            {
                result = instance.GenerateDBTypeSet(typeId);
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
                // データが正しいこと
                Assert.NotNull(result);
                Assert.AreEqual(result.TypeName, typeSettingList[typeId].TypeName);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static readonly object[] GenerateDBTypeTestCaseSource =
        {
            new object[] {4, (TypeId) 0, false},
            new object[] {4, (TypeId) 3, false},
            new object[] {4, (TypeId) 4, true},
        };

        [TestCaseSource(nameof(GenerateDBTypeTestCaseSource))]
        public static void GenerateDBTypeTest(int listLength, TypeId typeId,
            bool isError)
        {
            var typeSettingList = CreateTypeSettingList(listLength);
            var dataSettingList = CreateDataSettingList(listLength);
            var instance = new DatabaseMergedData(typeSettingList, dataSettingList);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            DBType result = null;
            var errorOccured = false;
            try
            {
                result = instance.GenerateDBType(typeId);
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
                // データが正しいこと
                Assert.NotNull(result);
                Assert.AreEqual(result.TypeName, typeSettingList[typeId].TypeName);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DatabaseMergedData
            {
                TypeDescList =
                {
                    new DatabaseTypeDesc
                    {
                        Memo = "Memo"
                    }
                }
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static DBTypeSettingList CreateTypeSettingList(int length)
        {
            if (length == -1) return null;

            var result = new DBTypeSettingList();

            for (var i = 0; i < length; i++)
            {
                result.Add(new DBTypeSetting
                {
                    TypeName = $"TypeName_{i}",
                });
            }

            return result;
        }

        private static DBDataSettingList CreateDataSettingList(int length)
        {
            if (length == -1) return null;

            var result = new DBDataSettingList();

            for (var i = 0; i < length; i++)
            {
                result.Add(new DBDataSetting(DBDataSettingType.DesignatedType, DBKind.User, i));
            }

            return result;
        }
    }
}