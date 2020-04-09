using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBDataSettingTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] DataSettingTypeTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual},
            new object[] {DBDataSettingType.DesignatedType},
            new object[] {DBDataSettingType.EqualBefore},
            new object[] {DBDataSettingType.FirstStringData},
        };

        [TestCaseSource(nameof(DataSettingTypeTestCaseSource))]
        public static void DataSettingTypeTest(DBDataSettingType type)
        {
            DBDataSettingType result = null;

            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                result = instance.DataSettingType;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が一致すること
            Assert.AreEqual(result, type);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] DBKindTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, true},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.EqualBefore, true},
            new object[] {DBDataSettingType.FirstStringData, true},
        };

        [TestCaseSource(nameof(DBKindTestCaseSource))]
        public static void DBKindTest(DBDataSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DBKind;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] TypeIdTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, true},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.EqualBefore, true},
            new object[] {DBDataSettingType.FirstStringData, true},
        };

        [TestCaseSource(nameof(TypeIdTestCaseSource))]
        public static void TypeIdTest(DBDataSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.TypeId;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ConstructorATest()
        {
            var errorOccured = false;
            try
            {
                var _ = new DBDataSetting();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] ConstructorBTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, null, null, false},
            new object[] {DBDataSettingType.Manual, DBKind.User, null, false},
            new object[] {DBDataSettingType.Manual, DBKind.Changeable, null, false},
            new object[] {DBDataSettingType.Manual, DBKind.System, null, false},
            new object[] {DBDataSettingType.Manual, null, (TypeId) 0, false},
            new object[] {DBDataSettingType.Manual, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.Manual, DBKind.Changeable, (TypeId) 0, false},
            new object[] {DBDataSettingType.Manual, DBKind.System, (TypeId) 0, false},
            new object[] {DBDataSettingType.EqualBefore, null, null, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.User, null, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.Changeable, null, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.System, null, false},
            new object[] {DBDataSettingType.EqualBefore, null, (TypeId) 0, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.Changeable, (TypeId) 0, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.System, (TypeId) 0, false},
            new object[] {DBDataSettingType.DesignatedType, null, null, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.User, null, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.Changeable, null, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.System, null, true},
            new object[] {DBDataSettingType.DesignatedType, null, (TypeId) 0, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.DesignatedType, DBKind.Changeable, (TypeId) 0, false},
            new object[] {DBDataSettingType.DesignatedType, DBKind.System, (TypeId) 0, false},
            new object[] {DBDataSettingType.FirstStringData, null, null, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, null, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.Changeable, null, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.System, null, false},
            new object[] {DBDataSettingType.FirstStringData, null, (TypeId) 0, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.Changeable, (TypeId) 0, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.System, (TypeId) 0, false},
            new object[] {null, null, null, true},
            new object[] {null, DBKind.User, null, true},
            new object[] {null, DBKind.Changeable, null, true},
            new object[] {null, DBKind.System, null, true},
            new object[] {null, null, (TypeId) 0, true},
            new object[] {null, DBKind.User, (TypeId) 0, true},
            new object[] {null, DBKind.Changeable, (TypeId) 0, true},
            new object[] {null, DBKind.System, (TypeId) 0, true},
        };

        [TestCaseSource(nameof(ConstructorBTestCaseSource))]
        public static void ConstructorBTest(DBDataSettingType type, DBKind dbKind,
            TypeId? typeId, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DBDataSetting(type, dbKind, typeId);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] SettingValuesListSetterTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, false},
            new object[] {DBDataSettingType.DesignatedType, false},
            new object[] {DBDataSettingType.EqualBefore, false},
            new object[] {DBDataSettingType.FirstStringData, false},
        };

        [TestCaseSource(nameof(SettingValuesListSetterTestCaseSource))]
        public static void SettingValuesListSetterTest(DBDataSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.SettingValuesList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static readonly object[] SettingValuesListGetterTestCaseSource =
        {
            new object[] {DBDataSettingType.Manual, false, false},
            new object[] {DBDataSettingType.Manual, true, true},
            new object[] {DBDataSettingType.DesignatedType, false, false},
            new object[] {DBDataSettingType.DesignatedType, true, true},
            new object[] {DBDataSettingType.EqualBefore, false, false},
            new object[] {DBDataSettingType.EqualBefore, true, true},
            new object[] {DBDataSettingType.FirstStringData, false, false},
            new object[] {DBDataSettingType.FirstStringData, true, true},
        };

        [TestCaseSource(nameof(SettingValuesListGetterTestCaseSource))]
        public static void SettingValuesListGetterTest(DBDataSettingType type, bool isSetNull, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var setValue = isSetNull
                ? null
                : new DBItemValuesList();

            var errorOccured = false;
            try
            {
                instance.SettingValuesList = setValue;
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
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBDataSetting.SettingValuesList)));
            }
        }

        private static readonly object[] SetDataSettingTypeTestCaseSource =
        {
            new object[] {null, null, null, true},
            new object[] {null, null, (TypeId) 0, true},
            new object[] {null, DBKind.User, null, true},
            new object[] {null, DBKind.User, (TypeId) 0, true},
            new object[] {DBDataSettingType.Manual, null, null, false},
            new object[] {DBDataSettingType.Manual, null, (TypeId) 0, false},
            new object[] {DBDataSettingType.Manual, DBKind.User, null, false},
            new object[] {DBDataSettingType.Manual, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.DesignatedType, null, null, true},
            new object[] {DBDataSettingType.DesignatedType, null, (TypeId) 0, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.User, null, true},
            new object[] {DBDataSettingType.DesignatedType, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.EqualBefore, null, null, false},
            new object[] {DBDataSettingType.EqualBefore, null, (TypeId) 0, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.User, null, false},
            new object[] {DBDataSettingType.EqualBefore, DBKind.User, (TypeId) 0, false},
            new object[] {DBDataSettingType.FirstStringData, null, null, false},
            new object[] {DBDataSettingType.FirstStringData, null, (TypeId) 0, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, null, false},
            new object[] {DBDataSettingType.FirstStringData, DBKind.User, (TypeId) 0, false},
        };

        [TestCaseSource(nameof(SetDataSettingTypeTestCaseSource))]
        public static void SetDataSettingTypeTest(DBDataSettingType type,
            DBKind dbKind, TypeId? typeId, bool isError)
        {
            var instance = MakeInstance(DBDataSettingType.Manual);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetDataSettingType(type, dbKind, typeId);
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
                Assert.AreEqual(changedPropertyList.Count, 3);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBDataSetting.DataSettingType)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(DBDataSetting.DBKind)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(DBDataSetting.TypeId)));
            }
        }


        [Test]
        public static void SerializeTest()
        {
            var target = MakeInstance(DBDataSettingType.Manual);
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        /// <summary>
        /// タイプ種別から、例外が発生しないようにインスタンスを生成する。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static DBDataSetting MakeInstance(DBDataSettingType type)
        {
            var instance = new DBDataSetting();

            if (type != DBDataSettingType.DesignatedType)
            {
                instance.SetDataSettingType(type);
                return instance;
            }

            instance.SetDataSettingType(type, DBKind.User, 0);
            return instance;
        }
    }
}