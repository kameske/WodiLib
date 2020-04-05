using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBItemSpecialSettingDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] SettingTypeTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal},
            new object[] {DBItemSpecialSettingType.Manual},
            new object[] {DBItemSpecialSettingType.LoadFile},
            new object[] {DBItemSpecialSettingType.ReferDatabase},
        };

        [TestCaseSource(nameof(SettingTypeTestCaseSource))]
        public static void SettingTypeTest(DBItemSpecialSettingType type)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.SettingType;
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

        private static readonly object[] DatabaseReferKindTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, true},
            new object[] {DBItemSpecialSettingType.Manual, true},
            new object[] {DBItemSpecialSettingType.LoadFile, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, false},
        };

        [TestCaseSource(nameof(DatabaseReferKindTestCaseSource))]
        public static void DatabaseReferKindGetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DatabaseReferKind;
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

        [TestCaseSource(nameof(DatabaseReferKindTestCaseSource))]
        public static void DatabaseReferKindSetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseReferKind = DBReferType.User;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSpecialSettingDesc.DatabaseReferKind)));
            }
        }

        private static readonly object[] DatabaseDbTypeIdTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, true},
            new object[] {DBItemSpecialSettingType.Manual, true},
            new object[] {DBItemSpecialSettingType.LoadFile, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, false},
        };

        [TestCaseSource(nameof(DatabaseDbTypeIdTestCaseSource))]
        public static void DatabaseDbTypeIdGetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DatabaseDbTypeId;
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

        [TestCaseSource(nameof(DatabaseDbTypeIdTestCaseSource))]
        public static void DatabaseDbTypeIdSetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseDbTypeId = 0;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSpecialSettingDesc.DatabaseDbTypeId)));
            }
        }

        private static readonly object[] DatabaseUseAdditionalItemsFlagTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, true},
            new object[] {DBItemSpecialSettingType.Manual, true},
            new object[] {DBItemSpecialSettingType.LoadFile, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, false},
        };

        [TestCaseSource(nameof(DatabaseUseAdditionalItemsFlagTestCaseSource))]
        public static void DatabaseUseAdditionalItemsFlagGetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DatabaseUseAdditionalItemsFlag;
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

        [TestCaseSource(nameof(DatabaseUseAdditionalItemsFlagTestCaseSource))]
        public static void DatabaseUseAdditionalItemsFlagSetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseUseAdditionalItemsFlag = false;
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
                Assert.IsTrue(changedPropertyList[0]
                    .Equals(nameof(DBItemSpecialSettingDesc.DatabaseUseAdditionalItemsFlag)));
            }
        }

        private static readonly object[] FolderNameTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, true},
            new object[] {DBItemSpecialSettingType.Manual, true},
            new object[] {DBItemSpecialSettingType.LoadFile, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, true},
        };

        [TestCaseSource(nameof(FolderNameTestCaseSource))]
        public static void FolderNameGetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.FolderName;
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

        [TestCaseSource(nameof(FolderNameTestCaseSource))]
        public static void FolderNameSetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.FolderName = "";
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(instance.FolderName)));
            }
        }

        private static readonly object[] OmissionFolderNameFlagTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, true},
            new object[] {DBItemSpecialSettingType.Manual, true},
            new object[] {DBItemSpecialSettingType.LoadFile, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, true},
        };

        [TestCaseSource(nameof(OmissionFolderNameFlagTestCaseSource))]
        public static void OmissionFolderNameFlagGetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.OmissionFolderNameFlag;
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

        [TestCaseSource(nameof(OmissionFolderNameFlagTestCaseSource))]
        public static void OmissionFolderNameFlagSetterTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.OmissionFolderNameFlag = true;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSpecialSettingDesc.OmissionFolderNameFlag)));
            }
        }

        private static readonly object[] ArgCaseListGetterTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal},
            new object[] {DBItemSpecialSettingType.Manual},
            new object[] {DBItemSpecialSettingType.LoadFile},
            new object[] {DBItemSpecialSettingType.ReferDatabase},
        };

        [TestCaseSource(nameof(ArgCaseListGetterTestCaseSource))]
        public static void ArgCaseListGetterTest(DBItemSpecialSettingType type)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.ArgCaseList;
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

        private static readonly object[] InitValueTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, true},
            new object[] {DBItemSpecialSettingType.Normal, new DBItemValue(0), false},
            new object[] {DBItemSpecialSettingType.Manual, null, true},
            new object[] {DBItemSpecialSettingType.Manual, new DBItemValue(0), false},
            new object[] {DBItemSpecialSettingType.LoadFile, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, new DBItemValue(0), false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, new DBItemValue(0), false},
        };

        [TestCaseSource(nameof(InitValueTestCaseSource))]
        public static void InitValueTest(DBItemSpecialSettingType type, DBItemValue setValue, bool isSetError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.InitValue;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            try
            {
                instance.InitValue = setValue;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isSetError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSpecialSettingDesc.InitValue)));
            }
        }

        private static readonly object[] ItemMemoTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, true},
            new object[] {DBItemSpecialSettingType.Normal, new ItemMemo(""), false},
            new object[] {DBItemSpecialSettingType.Manual, null, true},
            new object[] {DBItemSpecialSettingType.Manual, new ItemMemo(""), false},
            new object[] {DBItemSpecialSettingType.LoadFile, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, new ItemMemo(""), false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, new ItemMemo(""), false},
        };

        [TestCaseSource(nameof(ItemMemoTestCaseSource))]
        public static void ItemMemoTest(DBItemSpecialSettingType type, ItemMemo setValue, bool isSetError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.ItemMemo;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            try
            {
                instance.ItemMemo = setValue;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isSetError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSpecialSettingDesc.ItemMemo)));
            }
        }

        private static readonly object[] ChangeValueTypeTestCaseSource =
        {
            new object[] {null, -1, true},
            new object[] {null, 0, true},
            new object[] {null, 1, true},
            new object[] {null, 3, true},
            new object[] {DBItemSpecialSettingType.Normal, -1, false},
            new object[] {DBItemSpecialSettingType.Normal, 0, false},
            new object[] {DBItemSpecialSettingType.Normal, 1, false},
            new object[] {DBItemSpecialSettingType.Normal, 3, false},
            new object[] {DBItemSpecialSettingType.Manual, -1, false},
            new object[] {DBItemSpecialSettingType.Manual, 0, false},
            new object[] {DBItemSpecialSettingType.Manual, 1, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, false},
            new object[] {DBItemSpecialSettingType.LoadFile, -1, false},
            new object[] {DBItemSpecialSettingType.LoadFile, 0, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 1, false},
            new object[] {DBItemSpecialSettingType.LoadFile, 3, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -1, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 0, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 1, false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 3, false},
        };

        [TestCaseSource(nameof(ChangeValueTypeTestCaseSource))]
        public static void ChangeValueTypeTest(DBItemSpecialSettingType type, int argCaseLength,
            bool isError)
        {
            var instance = new DBItemSpecialSettingDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var cases = MakeCaseList(argCaseLength);

            var errorOccured = false;
            try
            {
                instance.ChangeValueType(type, cases);
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSpecialSettingDesc.SettingType)));
            }
        }

        private static readonly object[] GetAllSpecialCaseTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal},
            new object[] {DBItemSpecialSettingType.Manual},
            new object[] {DBItemSpecialSettingType.LoadFile},
            new object[] {DBItemSpecialSettingType.ReferDatabase},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseTestCaseSource))]
        public static void GetAllSpecialCaseTest(DBItemSpecialSettingType type)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.GetAllSpecialCase();
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

        private static readonly object[] GetAllSpecialCaseNumberTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal},
            new object[] {DBItemSpecialSettingType.Manual},
            new object[] {DBItemSpecialSettingType.LoadFile},
            new object[] {DBItemSpecialSettingType.ReferDatabase},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseNumberTestCaseSource))]
        public static void GetAllSpecialCaseNumberTest(DBItemSpecialSettingType type)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.GetAllSpecialCaseNumber();
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

        private static readonly object[] GetAllSpecialCaseDescriptionTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal},
            new object[] {DBItemSpecialSettingType.Manual},
            new object[] {DBItemSpecialSettingType.LoadFile},
            new object[] {DBItemSpecialSettingType.ReferDatabase},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseDescriptionTestCaseSource))]
        public static void GetAllSpecialCaseDescriptionTest(DBItemSpecialSettingType type)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.GetAllSpecialCaseDescription();
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

        private static readonly object[] AddSpecialCaseTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Normal, null, true},
            new object[] {DBItemSpecialSettingType.Manual, new DatabaseValueCase(0, ""), false},
            new object[] {DBItemSpecialSettingType.Manual, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, true},
        };

        [TestCaseSource(nameof(AddSpecialCaseTestCaseSource))]
        public static void AddSpecialCaseTest(DBItemSpecialSettingType type,
            DatabaseValueCase argCase, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddSpecialCase(argCase);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] AddSpecialCaseRangeTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, -1, false, true},
            new object[] {DBItemSpecialSettingType.Normal, 0, false, true},
            new object[] {DBItemSpecialSettingType.Normal, 1, false, true},
            new object[] {DBItemSpecialSettingType.Normal, 1, true, true},
            new object[] {DBItemSpecialSettingType.Normal, 3, false, true},
            new object[] {DBItemSpecialSettingType.Normal, 3, true, true},
            new object[] {DBItemSpecialSettingType.Manual, -1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 0, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 1, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 1, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, true, true},
            new object[] {DBItemSpecialSettingType.LoadFile, -1, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 0, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 1, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 1, true, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 3, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 3, true, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -1, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 0, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 1, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 1, true, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 3, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 3, true, true},
        };

        [TestCaseSource(nameof(AddSpecialCaseRangeTestCaseSource))]
        public static void AddSpecialCaseRangeTest(DBItemSpecialSettingType type,
            int caseLength, bool hasNullItem, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var cases = MakeCaseList(caseLength, hasNullItem);

            var errorOccured = false;
            try
            {
                instance.AddSpecialCaseRange(cases);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }


        private static readonly object[] InsertSpecialCaseTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, null, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, new DatabaseValueCase(0, ""), false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, new DatabaseValueCase(0, ""), false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, null, true},
        };

        [TestCaseSource(nameof(InsertSpecialCaseTestCaseSource))]
        public static void InsertSpecialCaseTest(DBItemSpecialSettingType type, int? caseLength,
            int index, DatabaseValueCase argCase, bool isError)
        {
            var instance = MakeInstance(type);
            if (caseLength != null)
            {
                var addList = MakeCaseList(caseLength.Value);
                instance.AddSpecialCaseRange(addList);
            }

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertSpecialCase(index, argCase);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] InsertSpecialCaseRangeTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, -1, -1, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, 0, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, 1, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, 1, true, true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, 3, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, 3, true, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, -1, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, 0, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, 1, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, 1, true, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, 3, false, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, 3, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, -1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 0, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 1, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 3, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 3, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, -1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 0, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 1, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 1, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 3, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 3, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, -1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 0, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 1, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 1, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 3, false, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 3, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, -1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, 0, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, 1, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, 1, true, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, 3, false, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 4, 3, true, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, -1, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, 0, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, 1, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, 1, true, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, 3, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, 3, true, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, -1, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, 0, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, 1, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, 1, true, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, 3, false, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, 3, true, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, -1, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, 0, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, 1, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, 1, true, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, 3, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, 3, true, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, -1, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, 0, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, 1, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, 1, true, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, 3, false, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, 3, true, true},
        };

        [TestCaseSource(nameof(InsertSpecialCaseRangeTestCaseSource))]
        public static void InsertSpecialCaseRangeTest(DBItemSpecialSettingType type, int? caseLength,
            int index, int insertLength, bool hasNullItem, bool isError)
        {
            var instance = MakeInstance(type);
            if (caseLength != null)
            {
                var addList = MakeCaseList(caseLength.Value);
                instance.AddSpecialCaseRange(addList);
            }

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var cases = MakeCaseList(insertLength, hasNullItem);

            var errorOccured = false;
            try
            {
                instance.InsertSpecialCaseRange(index, cases);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] UpdateDatabaseSpecialCaseTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, -4, "", true},
            new object[] {DBItemSpecialSettingType.Normal, -4, null, true},
            new object[] {DBItemSpecialSettingType.Normal, -3, "", true},
            new object[] {DBItemSpecialSettingType.Normal, -3, null, true},
            new object[] {DBItemSpecialSettingType.Normal, -1, "", true},
            new object[] {DBItemSpecialSettingType.Normal, -1, null, true},
            new object[] {DBItemSpecialSettingType.Normal, 0, "", true},
            new object[] {DBItemSpecialSettingType.Normal, 0, null, true},
            new object[] {DBItemSpecialSettingType.Manual, -4, "", true},
            new object[] {DBItemSpecialSettingType.Manual, -4, null, true},
            new object[] {DBItemSpecialSettingType.Manual, -3, "", true},
            new object[] {DBItemSpecialSettingType.Manual, -3, null, true},
            new object[] {DBItemSpecialSettingType.Manual, -1, "", true},
            new object[] {DBItemSpecialSettingType.Manual, -1, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 0, "", true},
            new object[] {DBItemSpecialSettingType.Manual, 0, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, -4, "", true},
            new object[] {DBItemSpecialSettingType.LoadFile, -4, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, -3, "", true},
            new object[] {DBItemSpecialSettingType.LoadFile, -3, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, -1, "", true},
            new object[] {DBItemSpecialSettingType.LoadFile, -1, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, 0, "", true},
            new object[] {DBItemSpecialSettingType.LoadFile, 0, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -4, "", true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -4, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -3, "", false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -3, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -1, "", false},
            new object[] {DBItemSpecialSettingType.ReferDatabase, -1, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 0, "", true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, 0, null, true},
        };

        [TestCaseSource(nameof(UpdateDatabaseSpecialCaseTestCaseSource))]
        public static void UpdateDatabaseSpecialCaseTest(DBItemSpecialSettingType type,
            int caseNumber, string description, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.UpdateDatabaseSpecialCase(caseNumber, description);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 1);
                Assert.IsTrue(changedArgCaseList[0].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Replace);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] UpdateManualSpecialCaseTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, null, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, new DatabaseValueCase(0, ""), false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, new DatabaseValueCase(0, ""), false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, null, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, null, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, null, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, new DatabaseValueCase(0, ""), true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, null, true},
        };

        [TestCaseSource(nameof(UpdateManualSpecialCaseTestCaseSource))]
        public static void UpdateManualSpecialCaseTest(DBItemSpecialSettingType type, int? caseLength,
            int index, DatabaseValueCase argCase, bool isError)
        {
            var instance = MakeInstance(type);
            if (caseLength != null)
            {
                var addList = MakeCaseList(caseLength.Value);
                instance.AddSpecialCaseRange(addList);
            }

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.UpdateManualSpecialCase(index, argCase);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 1);
                Assert.IsTrue(changedArgCaseList[0].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Replace);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] RemoveAtTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, -1, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, true},
        };

        [TestCaseSource(nameof(RemoveAtTestCaseSource))]
        public static void RemoveAtTest(DBItemSpecialSettingType type, int? caseLength,
            int index, bool isError)
        {
            var instance = MakeInstance(type);
            if (caseLength != null)
            {
                var addList = MakeCaseList(caseLength.Value);
                instance.AddSpecialCaseRange(addList);
            }

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.RemoveAt(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Remove);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] RemoveRangeTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, null, -1, -1, true},
            new object[] {DBItemSpecialSettingType.Normal, null, -1, 0, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, -1, true},
            new object[] {DBItemSpecialSettingType.Normal, null, 0, 0, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, -1, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 0, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 2, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, -1, 3, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, -1, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 0, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 3, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 0, 4, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, -1, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, 0, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, 1, false},
            new object[] {DBItemSpecialSettingType.Manual, 3, 2, 2, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, -1, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 0, true},
            new object[] {DBItemSpecialSettingType.Manual, 3, 3, 1, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, -1, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, -1, 0, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, -1, true},
            new object[] {DBItemSpecialSettingType.LoadFile, null, 0, 0, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, -1, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, -1, 0, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, -1, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, null, 0, 0, true},
        };

        [TestCaseSource(nameof(RemoveRangeTestCaseSource))]
        public static void RemoveRangeTest(DBItemSpecialSettingType type, int? caseLength,
            int index, int count, bool isError)
        {
            var instance = MakeInstance(type);
            if (caseLength != null)
            {
                var addList = MakeCaseList(caseLength.Value);
                instance.AddSpecialCaseRange(addList);
            }

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.RemoveRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Remove);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        private static readonly object[] ClearTestCaseSource =
        {
            new object[] {DBItemSpecialSettingType.Normal, true},
            new object[] {DBItemSpecialSettingType.Manual, false},
            new object[] {DBItemSpecialSettingType.LoadFile, true},
            new object[] {DBItemSpecialSettingType.ReferDatabase, true},
        };

        [TestCaseSource(nameof(ClearTestCaseSource))]
        public static void ClearTest(DBItemSpecialSettingType type, bool isError)
        {
            var instance = MakeInstance(type);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 2);
                Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
                Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Reset);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = MakeInstance(DBItemSpecialSettingType.LoadFile);
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        public static DBItemSpecialSettingDesc MakeInstance(DBItemSpecialSettingType type)
        {
            var instance = new DBItemSpecialSettingDesc();
            if (type != DBItemSpecialSettingType.ReferDatabase)
            {
                instance.ChangeValueType(type, null);
                return instance;
            }

            instance.ChangeValueType(type, new[] {new DatabaseValueCase(0, "")});
            return instance;
        }

        private static List<DatabaseValueCase> MakeCaseList(int length, bool hasItemNull = false)
        {
            if (length == -1) return null;

            var result = new List<DatabaseValueCase>();
            for (var i = 0; i < length; i++)
            {
                result.Add(
                    hasItemNull && i == length / 2
                        ? null
                        : new DatabaseValueCase(i, i.ToString())
                );
            }

            return result;
        }
    }
}