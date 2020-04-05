using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventSpecialNumberArgDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void ArgNameTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var argName = (CommonEventArgName) "test";

            var errorOccured = false;
            try
            {
                instance.ArgName = argName;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.ArgName;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue.Equals(argName));

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventSpecialNumberArgDesc.ArgName)));
        }

        private static readonly object[] ArgTypeTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, false},
            new object[] {CommonEventArgType.ReferDatabase, false},
            new object[] {CommonEventArgType.Manual, false}
        };

        [TestCaseSource(nameof(ArgTypeTestCaseSource))]
        public static void ArgTypeTest(CommonEventArgType type, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.ArgType;
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

        private static readonly object[] DatabaseUseDbKindTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, true},
            new object[] {CommonEventArgType.ReferDatabase, false},
            new object[] {CommonEventArgType.Manual, true},
        };

        [TestCaseSource(nameof(DatabaseUseDbKindTestCaseSource))]
        public static void DatabaseUseDbKindTest(CommonEventArgType type, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.DatabaseUseDbKind;
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

        private static readonly object[] DatabaseDbTypeIdTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, true},
            new object[] {CommonEventArgType.ReferDatabase, false},
            new object[] {CommonEventArgType.Manual, true},
        };

        [TestCaseSource(nameof(DatabaseDbTypeIdTestCaseSource))]
        public static void DatabaseDbTypeIdTest(CommonEventArgType type, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
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

        private static readonly object[] DatabaseUseAdditionalItemsFlagTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, true},
            new object[] {CommonEventArgType.ReferDatabase, false},
            new object[] {CommonEventArgType.Manual, true},
        };

        [TestCaseSource(nameof(DatabaseUseAdditionalItemsFlagTestCaseSource))]
        public static void DatabaseUseAdditionalItemsFlagTest(CommonEventArgType type, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
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

        [Test]
        public static void InitValueTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var initValue = (CommonEventNumberArgInitValue) 0;

            var errorOccured = false;
            try
            {
                instance.InitValue = initValue;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var setValue = instance.InitValue;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(setValue == initValue);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventSpecialNumberArgDesc.InitValue)));
        }

        [Test]
        public static void SpecialArgCaseListTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                var _ = instance.SpecialArgCaseList;
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

        [TestCase(0xFF, -1, true)]
        [TestCase(0xFF, 0, true)]
        [TestCase(0xFF, 3, true)]
        [TestCase(0xFF, 10, true)]
        [TestCase(0x00, -1, false)]
        [TestCase(0x00, 0, false)]
        [TestCase(0x01, 3, false)]
        [TestCase(0x02, 10, false)]
        public static void ChangeArgTypeTest(byte typeCode, int caseLength, bool isError)
        {
            var type = typeCode == 0xFF ? null : CommonEventArgType.FromByte(typeCode);
            var argCaseList = MakeArgCaseList(caseLength);

            var instance = new CommonEventSpecialNumberArgDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.ChangeArgType(type, argCaseList);
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
                Assert.AreEqual(changedPropertyList.Count, 5);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventSpecialNumberArgDesc.ArgType)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(CommonEventSpecialNumberArgDesc.DatabaseUseDbKind)));
                Assert.IsTrue(changedPropertyList[2].Equals(nameof(CommonEventSpecialNumberArgDesc.DatabaseDbTypeId)));
                Assert.IsTrue(changedPropertyList[3]
                    .Equals(nameof(CommonEventSpecialNumberArgDesc.DatabaseUseAdditionalItemsFlag)));
                Assert.IsTrue(changedPropertyList[4]
                    .Equals(nameof(CommonEventSpecialNumberArgDesc.SpecialArgCaseList)));
            }
        }

        private static readonly object[] SetDatabaseReferTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, DBKind.Changeable, true},
            new object[] {CommonEventArgType.Normal, DBKind.User, true},
            new object[] {CommonEventArgType.Normal, DBKind.System, true},
            new object[] {CommonEventArgType.Normal, null, true},
            new object[] {CommonEventArgType.ReferDatabase, DBKind.Changeable, false},
            new object[] {CommonEventArgType.ReferDatabase, DBKind.User, false},
            new object[] {CommonEventArgType.ReferDatabase, DBKind.System, false},
            new object[] {CommonEventArgType.ReferDatabase, null, true},
            new object[] {CommonEventArgType.Manual, DBKind.Changeable, true},
            new object[] {CommonEventArgType.Manual, DBKind.User, true},
            new object[] {CommonEventArgType.Manual, DBKind.System, true},
            new object[] {CommonEventArgType.Manual, null, true},
        };

        [TestCaseSource(nameof(SetDatabaseReferTestCaseSource))]
        public static void SetDatabaseReferTest(CommonEventArgType type, DBKind dbKind, bool isError)
        {
            var typeId = (TypeId) 0;

            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetDatabaseRefer(dbKind, typeId);
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
                // セットした値が反映されていること
                Assert.AreEqual(instance.DatabaseUseDbKind, dbKind);
                Assert.AreEqual(instance.DatabaseDbTypeId, typeId);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventSpecialNumberArgDesc.DatabaseUseDbKind)));
                Assert.IsTrue(changedPropertyList[1].Equals(nameof(CommonEventSpecialNumberArgDesc.DatabaseDbTypeId)));
            }
        }

        private static readonly object[] SetDatabaseUseAdditionalItemsFlagTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, true, true},
            new object[] {CommonEventArgType.Normal, false, true},
            new object[] {CommonEventArgType.ReferDatabase, true, false},
            new object[] {CommonEventArgType.ReferDatabase, false, false},
            new object[] {CommonEventArgType.Manual, true, true},
            new object[] {CommonEventArgType.Manual, false, true},
        };

        [TestCaseSource(nameof(SetDatabaseUseAdditionalItemsFlagTestCaseSource))]
        public static void SetDatabaseUseAdditionalItemsFlagTest(CommonEventArgType type, bool flag, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetDatabaseUseAdditionalItemsFlag(flag);
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
                    .Equals(nameof(CommonEventSpecialNumberArgDesc.DatabaseUseAdditionalItemsFlag)));
            }
        }

        private static readonly object[] GetSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, false, 0, 0},
            new object[] {CommonEventArgType.Normal, false, 4, 0},
            new object[] {CommonEventArgType.ReferDatabase, false, 0, 0},
            new object[] {CommonEventArgType.ReferDatabase, false, 4, 0},
            new object[] {CommonEventArgType.ReferDatabase, true, 0, 3},
            new object[] {CommonEventArgType.ReferDatabase, true, 4, 3},
            new object[] {CommonEventArgType.Manual, false, 0, 0},
            new object[] {CommonEventArgType.Manual, false, 4, 4},
        };

        [TestCaseSource(nameof(GetSpecialCaseTestCaseSource))]
        public static void GetSpecialCaseTest(CommonEventArgType type, bool isSetDatabaseAddition, int initCaseLength,
            int answerLength)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            if (type == CommonEventArgType.ReferDatabase)
            {
                instance.SetDatabaseUseAdditionalItemsFlag(isSetDatabaseAddition);
            }

            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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

            // 選択肢数が意図した値と一致すること
            var caseLength = instance.GetAllSpecialCase().Count;
            Assert.AreEqual(caseLength, answerLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] GetAllSpecialCaseNumberTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, false, 0, 0},
            new object[] {CommonEventArgType.Normal, false, 4, 0},
            new object[] {CommonEventArgType.ReferDatabase, false, 0, 3},
            new object[] {CommonEventArgType.ReferDatabase, false, 4, 3},
            new object[] {CommonEventArgType.ReferDatabase, true, 0, 3},
            new object[] {CommonEventArgType.ReferDatabase, true, 4, 3},
            new object[] {CommonEventArgType.Manual, false, 0, 0},
            new object[] {CommonEventArgType.Manual, false, 4, 4},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseNumberTestCaseSource))]
        public static void GetAllSpecialCaseNumberTest(CommonEventArgType type, bool isSetDatabaseAddition,
            int initCaseLength, int answerLength)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            if (type == CommonEventArgType.ReferDatabase)
            {
                instance.SetDatabaseUseAdditionalItemsFlag(isSetDatabaseAddition);
            }

            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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

            // 選択肢数が意図した値と一致すること
            var caseLength = instance.GetAllSpecialCaseNumber().Count;
            Assert.AreEqual(caseLength, answerLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] GetAllSpecialCaseDescriptionTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, false, 0, 0},
            new object[] {CommonEventArgType.Normal, false, 4, 0},
            new object[] {CommonEventArgType.ReferDatabase, false, 0, 0},
            new object[] {CommonEventArgType.ReferDatabase, false, 4, 0},
            new object[] {CommonEventArgType.ReferDatabase, true, 0, 3},
            new object[] {CommonEventArgType.ReferDatabase, true, 4, 3},
            new object[] {CommonEventArgType.Manual, false, 0, 0},
            new object[] {CommonEventArgType.Manual, false, 4, 4},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseDescriptionTestCaseSource))]
        public static void GetAllSpecialCaseDescriptionTest(CommonEventArgType type, bool isSetDatabaseAddition,
            int initCaseLength, int answerLength)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            if (type == CommonEventArgType.ReferDatabase)
            {
                instance.SetDatabaseUseAdditionalItemsFlag(isSetDatabaseAddition);
            }

            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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

            // 選択肢数が意図した値と一致すること
            var caseLength = instance.GetAllSpecialCase().Count;
            Assert.AreEqual(caseLength, answerLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] AddSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, false, true},
            new object[] {CommonEventArgType.Normal, true, true},
            new object[] {CommonEventArgType.ReferDatabase, false, true},
            new object[] {CommonEventArgType.ReferDatabase, true, true},
            new object[] {CommonEventArgType.Manual, false, false},
            new object[] {CommonEventArgType.Manual, true, true},
        };

        [TestCaseSource(nameof(AddSpecialCaseTestCaseSource))]
        public static void AddSpecialCaseTest(CommonEventArgType type, bool isNullArgCase, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

            var errorOccured = false;
            try
            {
                var argCase = isNullArgCase ? null : new CommonEventSpecialArgCase(0, "");
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
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] AddRangeSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, false, true},
            new object[] {CommonEventArgType.Normal, true, true},
            new object[] {CommonEventArgType.ReferDatabase, false, true},
            new object[] {CommonEventArgType.ReferDatabase, true, true},
            new object[] {CommonEventArgType.Manual, false, false},
            new object[] {CommonEventArgType.Manual, true, true},
        };

        [TestCaseSource(nameof(AddRangeSpecialCaseTestCaseSource))]
        public static void AddRangeSpecialCaseTest(CommonEventArgType type, bool isNullArgCases, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

            var errorOccured = false;
            try
            {
                var argCases = isNullArgCases
                    ? null
                    : new List<CommonEventSpecialArgCase>
                    {
                        new CommonEventSpecialArgCase(0, "case0"),
                    };
                instance.AddRangeSpecialCase(argCases);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] InsertSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, 0, 0, false, true},
            new object[] {CommonEventArgType.Normal, 0, 0, true, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, false, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, true, true},
            new object[] {CommonEventArgType.Manual, 1, -1, false, true},
            new object[] {CommonEventArgType.Manual, 1, -1, true, true},
            new object[] {CommonEventArgType.Manual, 1, 0, false, false},
            new object[] {CommonEventArgType.Manual, 1, 0, true, true},
            new object[] {CommonEventArgType.Manual, 1, 1, false, false},
            new object[] {CommonEventArgType.Manual, 1, 1, true, true},
            new object[] {CommonEventArgType.Manual, 1, 2, false, true},
            new object[] {CommonEventArgType.Manual, 1, 2, true, true},
            new object[] {CommonEventArgType.Manual, 4, -1, false, true},
            new object[] {CommonEventArgType.Manual, 4, -1, true, true},
            new object[] {CommonEventArgType.Manual, 4, 0, false, false},
            new object[] {CommonEventArgType.Manual, 4, 0, true, true},
            new object[] {CommonEventArgType.Manual, 4, 4, false, false},
            new object[] {CommonEventArgType.Manual, 4, 4, true, true},
            new object[] {CommonEventArgType.Manual, 4, 5, false, true},
            new object[] {CommonEventArgType.Manual, 4, 5, true, true},
        };

        [TestCaseSource(nameof(InsertSpecialCaseTestCaseSource))]
        public static void InsertSpecialCaseTest(CommonEventArgType type, int initCaseLength, int index,
            bool isNullArgCase, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

            var errorOccured = false;
            try
            {
                var argCase = isNullArgCase ? null : new CommonEventSpecialArgCase(0, "");
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
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] InsertRangeSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, 0, 0, -1, true},
            new object[] {CommonEventArgType.Normal, 0, 0, 0, true},
            new object[] {CommonEventArgType.Normal, 0, 0, 1, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, -1, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, 0, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 1, 0, true},
            new object[] {CommonEventArgType.Manual, 1, -1, -1, true},
            new object[] {CommonEventArgType.Manual, 1, -1, 0, true},
            new object[] {CommonEventArgType.Manual, 1, -1, 1, true},
            new object[] {CommonEventArgType.Manual, 1, 0, -1, true},
            new object[] {CommonEventArgType.Manual, 1, 0, 0, false},
            new object[] {CommonEventArgType.Manual, 1, 0, 1, false},
            new object[] {CommonEventArgType.Manual, 1, 1, -1, true},
            new object[] {CommonEventArgType.Manual, 1, 1, 0, false},
            new object[] {CommonEventArgType.Manual, 1, 1, 1, false},
            new object[] {CommonEventArgType.Manual, 1, 2, -1, true},
            new object[] {CommonEventArgType.Manual, 1, 2, 0, true},
            new object[] {CommonEventArgType.Manual, 1, 2, 1, true},
            new object[] {CommonEventArgType.Manual, 4, -1, -1, true},
            new object[] {CommonEventArgType.Manual, 4, -1, 0, true},
            new object[] {CommonEventArgType.Manual, 4, -1, 1, true},
            new object[] {CommonEventArgType.Manual, 4, 0, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 0, 0, false},
            new object[] {CommonEventArgType.Manual, 4, 0, 1, false},
            new object[] {CommonEventArgType.Manual, 4, 4, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 4, 0, false},
            new object[] {CommonEventArgType.Manual, 4, 4, 1, false},
            new object[] {CommonEventArgType.Manual, 4, 5, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 5, 0, true},
            new object[] {CommonEventArgType.Manual, 4, 5, 1, true},
        };

        [TestCaseSource(nameof(InsertRangeSpecialCaseTestCaseSource))]
        public static void InsertRangeSpecialCaseTest(CommonEventArgType type, int initCaseLength, int index,
            int insertLength, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

            var errorOccured = false;
            try
            {
                instance.InsertRangeSpecialCase(index, MakeArgCaseList(insertLength));
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Add);
            }
        }

        private static readonly object[] UpdateDatabaseSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, -4, null, true},
            new object[] {CommonEventArgType.Normal, -4, "", true},
            new object[] {CommonEventArgType.Normal, -4, "abc", true},
            new object[] {CommonEventArgType.Normal, -4, "あいうえお", true},
            new object[] {CommonEventArgType.Normal, -4, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Normal, -4, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Normal, -3, null, true},
            new object[] {CommonEventArgType.Normal, -3, "", true},
            new object[] {CommonEventArgType.Normal, -3, "abc", true},
            new object[] {CommonEventArgType.Normal, -3, "あいうえお", true},
            new object[] {CommonEventArgType.Normal, -3, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Normal, -3, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Normal, -1, null, true},
            new object[] {CommonEventArgType.Normal, -1, "", true},
            new object[] {CommonEventArgType.Normal, -1, "abc", true},
            new object[] {CommonEventArgType.Normal, -1, "あいうえお", true},
            new object[] {CommonEventArgType.Normal, -1, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Normal, -1, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Normal, 0, null, true},
            new object[] {CommonEventArgType.Normal, 0, "", true},
            new object[] {CommonEventArgType.Normal, 0, "abc", true},
            new object[] {CommonEventArgType.Normal, 0, "あいうえお", true},
            new object[] {CommonEventArgType.Normal, 0, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Normal, 0, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.ReferDatabase, -4, null, true},
            new object[] {CommonEventArgType.ReferDatabase, -4, "", true},
            new object[] {CommonEventArgType.ReferDatabase, -4, "abc", true},
            new object[] {CommonEventArgType.ReferDatabase, -4, "あいうえお", true},
            new object[] {CommonEventArgType.ReferDatabase, -4, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.ReferDatabase, -4, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.ReferDatabase, -3, null, true},
            new object[] {CommonEventArgType.ReferDatabase, -3, "", false},
            new object[] {CommonEventArgType.ReferDatabase, -3, "abc", false},
            new object[] {CommonEventArgType.ReferDatabase, -3, "あいうえお", false},
            new object[] {CommonEventArgType.ReferDatabase, -3, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.ReferDatabase, -3, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.ReferDatabase, -1, null, true},
            new object[] {CommonEventArgType.ReferDatabase, -1, "", false},
            new object[] {CommonEventArgType.ReferDatabase, -1, "abc", false},
            new object[] {CommonEventArgType.ReferDatabase, -1, "あいうえお", false},
            new object[] {CommonEventArgType.ReferDatabase, -1, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.ReferDatabase, -1, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.ReferDatabase, 0, null, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, "", true},
            new object[] {CommonEventArgType.ReferDatabase, 0, "abc", true},
            new object[] {CommonEventArgType.ReferDatabase, 0, "あいうえお", true},
            new object[] {CommonEventArgType.ReferDatabase, 0, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.ReferDatabase, 0, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Manual, -4, null, true},
            new object[] {CommonEventArgType.Manual, -4, "", true},
            new object[] {CommonEventArgType.Manual, -4, "abc", true},
            new object[] {CommonEventArgType.Manual, -4, "あいうえお", true},
            new object[] {CommonEventArgType.Manual, -4, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Manual, -4, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Manual, -3, null, true},
            new object[] {CommonEventArgType.Manual, -3, "", true},
            new object[] {CommonEventArgType.Manual, -3, "abc", true},
            new object[] {CommonEventArgType.Manual, -3, "あいうえお", true},
            new object[] {CommonEventArgType.Manual, -3, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Manual, -3, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Manual, -1, null, true},
            new object[] {CommonEventArgType.Manual, -1, "", true},
            new object[] {CommonEventArgType.Manual, -1, "abc", true},
            new object[] {CommonEventArgType.Manual, -1, "あいうえお", true},
            new object[] {CommonEventArgType.Manual, -1, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Manual, -1, "New\nLine\nLF", true},
            new object[] {CommonEventArgType.Manual, 0, null, true},
            new object[] {CommonEventArgType.Manual, 0, "", true},
            new object[] {CommonEventArgType.Manual, 0, "abc", true},
            new object[] {CommonEventArgType.Manual, 0, "あいうえお", true},
            new object[] {CommonEventArgType.Manual, 0, "New\r\nLine\r\nCRLF", true},
            new object[] {CommonEventArgType.Manual, 0, "New\nLine\nLF", true},
        };

        [TestCaseSource(nameof(UpdateDatabaseSpecialCaseTestCaseSource))]
        public static void UpdateDatabaseSpecialCase(CommonEventArgType type, int caseNumber,
            string description, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 1);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Replace);
            }
        }

        private static readonly object[] UpdateManualSpecialCaseTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, 0, 0, false, true},
            new object[] {CommonEventArgType.Normal, 0, 0, true, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, -1, false, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, -1, true, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, false, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, true, true},
            new object[] {CommonEventArgType.Manual, 1, -1, false, true},
            new object[] {CommonEventArgType.Manual, 1, -1, true, true},
            new object[] {CommonEventArgType.Manual, 1, 0, false, false},
            new object[] {CommonEventArgType.Manual, 1, 0, true, true},
            new object[] {CommonEventArgType.Manual, 1, 1, false, true},
            new object[] {CommonEventArgType.Manual, 1, 1, true, true},
            new object[] {CommonEventArgType.Manual, 4, -1, false, true},
            new object[] {CommonEventArgType.Manual, 4, -1, true, true},
            new object[] {CommonEventArgType.Manual, 4, 0, false, false},
            new object[] {CommonEventArgType.Manual, 4, 0, true, true},
            new object[] {CommonEventArgType.Manual, 4, 3, false, false},
            new object[] {CommonEventArgType.Manual, 4, 3, true, true},
            new object[] {CommonEventArgType.Manual, 4, 4, false, true},
            new object[] {CommonEventArgType.Manual, 4, 4, true, true},
        };

        [TestCaseSource(nameof(UpdateManualSpecialCaseTestCaseSource))]
        public static void UpdateManualSpecialCaseTest(CommonEventArgType type, int initCaseLength, int index,
            bool isNullArgCase, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

            var errorOccured = false;
            try
            {
                var argCase = isNullArgCase
                    ? null
                    : new CommonEventSpecialArgCase(0, "");
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
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 1);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Replace);
            }
        }

        private static readonly object[] RemoveAtTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, 0, 0, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, -1, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, true},
            new object[] {CommonEventArgType.Manual, 0, -1, true},
            new object[] {CommonEventArgType.Manual, 0, 0, true},
            new object[] {CommonEventArgType.Manual, 0, 1, true},
            new object[] {CommonEventArgType.Manual, 1, -1, true},
            new object[] {CommonEventArgType.Manual, 1, 0, false},
            new object[] {CommonEventArgType.Manual, 1, 1, true},
            new object[] {CommonEventArgType.Manual, 4, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 0, false},
            new object[] {CommonEventArgType.Manual, 4, 3, false},
            new object[] {CommonEventArgType.Manual, 4, 4, true},
        };

        [TestCaseSource(nameof(RemoveAtTestCaseSource))]
        public static void RemoveAtTest(CommonEventArgType type, int initCaseLength, int index, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Remove);
            }
        }

        private static readonly object[] RemoveRangeTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, 0, 0, 0, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, -1, -1, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, -1, 0, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, -1, 1, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, -1, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, 0, true},
            new object[] {CommonEventArgType.ReferDatabase, 0, 0, 1, true},
            new object[] {CommonEventArgType.Manual, 0, -1, -1, true},
            new object[] {CommonEventArgType.Manual, 0, -1, 0, true},
            new object[] {CommonEventArgType.Manual, 0, -1, 1, true},
            new object[] {CommonEventArgType.Manual, 0, 0, -1, true},
            new object[] {CommonEventArgType.Manual, 0, 0, 0, true},
            new object[] {CommonEventArgType.Manual, 0, 0, 1, true},
            new object[] {CommonEventArgType.Manual, 0, 1, -1, true},
            new object[] {CommonEventArgType.Manual, 0, 1, 0, true},
            new object[] {CommonEventArgType.Manual, 0, 1, 1, true},
            new object[] {CommonEventArgType.Manual, 1, -1, -1, true},
            new object[] {CommonEventArgType.Manual, 1, -1, 0, true},
            new object[] {CommonEventArgType.Manual, 1, -1, 1, true},
            new object[] {CommonEventArgType.Manual, 1, 0, -1, true},
            new object[] {CommonEventArgType.Manual, 1, 0, 0, false},
            new object[] {CommonEventArgType.Manual, 1, 0, 1, false},
            new object[] {CommonEventArgType.Manual, 1, 0, 2, true},
            new object[] {CommonEventArgType.Manual, 1, 1, -1, true},
            new object[] {CommonEventArgType.Manual, 1, 1, 0, true},
            new object[] {CommonEventArgType.Manual, 1, 1, 1, true},
            new object[] {CommonEventArgType.Manual, 1, 1, 2, true},
            new object[] {CommonEventArgType.Manual, 4, -1, -1, true},
            new object[] {CommonEventArgType.Manual, 4, -1, 0, true},
            new object[] {CommonEventArgType.Manual, 4, -1, 3, true},
            new object[] {CommonEventArgType.Manual, 4, -1, 4, true},
            new object[] {CommonEventArgType.Manual, 4, 0, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 0, 0, false},
            new object[] {CommonEventArgType.Manual, 4, 0, 4, false},
            new object[] {CommonEventArgType.Manual, 4, 0, 5, true},
            new object[] {CommonEventArgType.Manual, 4, 3, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 3, 0, false},
            new object[] {CommonEventArgType.Manual, 4, 3, 1, false},
            new object[] {CommonEventArgType.Manual, 4, 3, 2, true},
            new object[] {CommonEventArgType.Manual, 4, 4, -1, true},
            new object[] {CommonEventArgType.Manual, 4, 4, 0, true},
            new object[] {CommonEventArgType.Manual, 4, 4, 1, true},
            new object[] {CommonEventArgType.Manual, 4, 4, 4, true},
        };

        [TestCaseSource(nameof(RemoveRangeTestCaseSource))]
        public static void RemoveRangeTest(CommonEventArgType type, int initCaseLength, int index,
            int count, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, MakeArgCaseList(initCaseLength));
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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
            if (isError)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Remove);
            }
        }

        private static readonly object[] ClearTestCaseSource =
        {
            new object[] {CommonEventArgType.Normal, true},
            new object[] {CommonEventArgType.ReferDatabase, true},
            new object[] {CommonEventArgType.Manual, false},
        };

        [TestCaseSource(nameof(ClearTestCaseSource))]
        public static void ClearTest(CommonEventArgType type, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc();
            instance.ChangeArgType(type, null);
            var changedDescPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedDescPropertyList.Add(args.PropertyName); };
            var changedSpecialArgCaseListPropertyList = new List<string>();
            instance.SpecialArgCaseList.PropertyChanged += (sender, args) =>
            {
                changedSpecialArgCaseListPropertyList.Add(args.PropertyName);
            };
            var changedSpecialArgCaseListCollectionArgList = new List<NotifyCollectionChangedEventArgs>();
            instance.SpecialArgCaseList.CollectionChanged += (sender, args) =>
            {
                changedSpecialArgCaseListCollectionArgList.Add(args);
            };

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
            if (errorOccured)
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 2);
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[0]
                    .Equals(nameof(instance.SpecialArgCaseList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Reset);
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonEventSpecialNumberArgDesc
            {
                ArgName = "ArgName",
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }


        private static IEnumerable<CommonEventSpecialArgCase> MakeArgCaseList(int length)
        {
            if (length == -1) return null;

            // yieldを使用したリスト返却
            IEnumerable<CommonEventSpecialArgCase> FMakeList()
            {
                for (var i = 0; i < length; i++)
                {
                    yield return new CommonEventSpecialArgCase(i, "");
                }
            }

            return FMakeList();
        }
    }
}