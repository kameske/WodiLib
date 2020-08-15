using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal.DBItemSettingDesc
{
    [TestFixture]
    public class DBItemSettingDescNormalTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void SettingTypeTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // プロパティが意図した値と一致すること
            Assert.AreEqual(instance.SettingType, DBItemSpecialSettingType.Normal);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseReferenceDescGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DatabaseReferenceDesc;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void LoadFileDescGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.LoadFileDesc;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ManualDescGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.ManualDesc;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void NormalDescGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.NormalDesc;
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
        public static void DefaultType()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DefaultType;
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
        public static void ConstructorTest()
        {
            DBItemSettingDescNormal instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBItemSettingDescNormal();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 選択肢が0であること
            Assert.AreEqual(instance.GetAllSpecialCase().ToList().Count, 0);
        }

        [Test]
        public static void GetAllSpecialCaseTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            IReadOnlyList<DatabaseValueCase> allCase = null;

            var errorOccured = false;
            try
            {
                allCase = instance.GetAllSpecialCase().ToList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素数が0であること
            Assert.AreEqual(allCase.Count, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void GetAllSpecialCaseNumberTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            IReadOnlyList<DatabaseValueCaseNumber> result = null;

            var errorOccured = false;
            try
            {
                result = instance.GetAllSpecialCaseNumber().ToList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素数が0であること
            Assert.AreEqual(result.Count, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void GetAllSpecialCaseDescriptionTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            IReadOnlyList<DatabaseValueCaseDescription> result = null;

            var errorOccured = false;
            try
            {
                result = instance.GetAllSpecialCaseDescription().ToList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素数が0であること
            Assert.AreEqual(result.Count, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        private static readonly object[] CanSetItemTestCaseSource =
        {
            new object[] {null, true, false},
            new object[] {DBItemType.Int, false, true},
            new object[] {DBItemType.String, false, true},
        };

        [TestCaseSource(nameof(CanSetItemTestCaseSource))]
        public static void CanSetItemTypeTest(DBItemType type, bool isError, bool answer)
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            bool result = false;

            var errorOccured = false;
            try
            {
                result = instance.CanSetItemType(type);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured)
            {
                // 結果が意図した値と一致すること
                Assert.AreEqual(result, answer);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] EqualsTestCaseSource =
        {
            new object[] {null, false},
            new object[] {nameof(DBItemSettingDescNormal), true},
            new object[] {nameof(DBItemSettingDescLoadFile), false},
            new object[] {nameof(DBItemSettingDescDatabase), false},
            new object[] {nameof(DBItemSettingDescManual), false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void Equals(string settingDescCode, bool answer)
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IDBItemSettingDesc desc = null;
            switch (settingDescCode)
            {
                case nameof(DBItemSettingDescLoadFile):
                    desc = new DBItemSettingDescLoadFile();
                    break;
                case nameof(DBItemSettingDescDatabase):
                    desc = new DBItemSettingDescDatabase();
                    break;
                case nameof(DBItemSettingDescManual):
                    desc = new DBItemSettingDescManual();
                    break;
                case null:
                    break;
                default:
                    desc = new DBItemSettingDescNormal();
                    break;
            }

            bool result = false;

            var errorOccured = false;
            try
            {
                result = instance.Equals(desc);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}
