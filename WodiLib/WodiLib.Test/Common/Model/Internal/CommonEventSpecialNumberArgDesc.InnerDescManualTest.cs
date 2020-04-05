using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialNumberArgDesc_InnerDescManualTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void ArgTypeTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // 取得した値が意図した値であること
            var type = instance.ArgType;
            Assert.AreEqual(type, CommonEventArgType.Manual);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseDbKindTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseDbTypeIdTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseUseAdditionalItemsFlagTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] SetDatabaseReferTestCaseSource =
        {
            new object[] {DBKind.Changeable, 0},
            new object[] {DBKind.User, 99},
            new object[] {DBKind.System, 32},
            new object[] {null, 85},
        };

        [TestCaseSource(nameof(SetDatabaseReferTestCaseSource))]
        public static void SetDatabaseReferTest(DBKind dbKind, int dbTypeId)
        {
            var typeId = (TypeId) dbTypeId;

            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void SetDatabaseUseAdditionalItemsFlagTest(bool flag)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SpecialArgCaseListTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] GetSpecialCaseTestCaseSource =
        {
            new object[] {0, 0},
            new object[] {4, 4},
        };

        [TestCaseSource(nameof(GetSpecialCaseTestCaseSource))]
        public static void GetSpecialCaseTest(int initCaseLength, int answerLength)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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
            new object[] {0, 0},
            new object[] {4, 4},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseNumberTestCaseSource))]
        public static void GetAllSpecialCaseNumberTest(int initCaseLength, int answerLength)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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

        private static readonly object[] GetAllSpecialCaseDescriptionTestCaseSource =
        {
            new object[] {0, 0},
            new object[] {4, 4},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseDescriptionTestCaseSource))]
        public static void GetAllSpecialCaseDescriptionTest(int initCaseLength, int answerLength)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AddSpecialCaseTest(bool isNullArgCase, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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
                    .Equals(nameof(changedSpecialArgCaseListCollectionArgList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Add);
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AddRangeSpecialCaseTest(bool isNullArgCases, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(null);
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
                    : new List<CommonEventSpecialArgCase>();
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
                    .Equals(nameof(changedSpecialArgCaseListCollectionArgList.Count)));
                Assert.IsTrue(changedSpecialArgCaseListPropertyList[1]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 1);
                Assert.AreEqual(changedSpecialArgCaseListCollectionArgList[0].Action,
                    NotifyCollectionChangedAction.Add);
            }
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, false)]
        [TestCase(1, 1, true, true)]
        [TestCase(1, 2, false, true)]
        [TestCase(1, 2, true, true)]
        [TestCase(4, -1, false, true)]
        [TestCase(4, -1, true, true)]
        [TestCase(4, 0, false, false)]
        [TestCase(4, 0, true, true)]
        [TestCase(4, 4, false, false)]
        [TestCase(4, 4, true, true)]
        [TestCase(4, 5, false, true)]
        [TestCase(4, 5, true, true)]
        public static void InsertSpecialCaseTest(int initCaseLength, int index, bool isNullArgCase, bool isError)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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

        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, false)]
        [TestCase(1, 1, 1, false)]
        [TestCase(1, 2, -1, true)]
        [TestCase(1, 2, 0, true)]
        [TestCase(1, 2, 1, true)]
        [TestCase(4, -1, -1, true)]
        [TestCase(4, -1, 0, true)]
        [TestCase(4, -1, 1, true)]
        [TestCase(4, 0, -1, true)]
        [TestCase(4, 0, 0, false)]
        [TestCase(4, 0, 1, false)]
        [TestCase(4, 4, -1, true)]
        [TestCase(4, 4, 0, false)]
        [TestCase(4, 4, 1, false)]
        [TestCase(4, 5, -1, true)]
        [TestCase(4, 5, 0, true)]
        [TestCase(4, 5, 1, true)]
        public static void InsertRangeSpecialCaseTest(int initCaseLength, int index,
            int insertLength, bool isError)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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

        [Test]
        public static void UpdateDatabaseSpecialCase()
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(10).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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
                instance.UpdateDatabaseSpecialCase(0, "");
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        [TestCase(1, -1, false, true)]
        [TestCase(1, -1, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 0, true, true)]
        [TestCase(1, 1, false, true)]
        [TestCase(1, 1, true, true)]
        [TestCase(4, -1, false, true)]
        [TestCase(4, -1, true, true)]
        [TestCase(4, 0, false, false)]
        [TestCase(4, 0, true, true)]
        [TestCase(4, 3, false, false)]
        [TestCase(4, 3, true, true)]
        [TestCase(4, 4, false, true)]
        [TestCase(4, 4, true, true)]
        public static void UpdateManualSpecialCaseTest(int initCaseLength, int index,
            bool isNullArgCase, bool isError)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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

        [TestCase(0, -1, true)]
        [TestCase(0, 0, true)]
        [TestCase(0, 1, true)]
        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(4, -1, true)]
        [TestCase(4, 0, false)]
        [TestCase(4, 3, false)]
        [TestCase(4, 4, true)]
        public static void RemoveSpecialCaseAtTest(int initCaseLength, int index, bool isError)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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
                instance.RemoveSpecialCaseAt(index);
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

        [TestCase(0, -1, -1, true)]
        [TestCase(0, -1, 0, true)]
        [TestCase(0, -1, 1, true)]
        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, true)]
        [TestCase(0, 0, 1, true)]
        [TestCase(0, 1, -1, true)]
        [TestCase(0, 1, 0, true)]
        [TestCase(0, 1, 1, true)]
        [TestCase(1, -1, -1, true)]
        [TestCase(1, -1, 0, true)]
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 0, 2, true)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, true)]
        [TestCase(1, 1, 1, true)]
        [TestCase(4, -1, -1, true)]
        [TestCase(4, -1, 0, true)]
        [TestCase(4, -1, 3, true)]
        [TestCase(4, -1, 4, true)]
        [TestCase(4, 0, -1, true)]
        [TestCase(4, 0, 0, false)]
        [TestCase(4, 0, 4, false)]
        [TestCase(4, 0, 5, true)]
        [TestCase(4, 3, -1, true)]
        [TestCase(4, 3, 0, false)]
        [TestCase(4, 3, 1, false)]
        [TestCase(4, 3, 2, true)]
        [TestCase(4, 4, -1, true)]
        [TestCase(4, 4, 0, true)]
        [TestCase(4, 4, 3, true)]
        [TestCase(4, 4, 4, true)]
        public static void RemoveSpecialCaseRangeTest(int initCaseLength, int index,
            int count, bool isError)
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(initCaseLength).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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
                instance.RemoveSpecialCaseRange(index, count);
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

        [Test]
        public static void ClearSpecialCaseTest()
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(10).ToArray());
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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
                instance.ClearSpecialCase();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
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

        [Test]
        public static void SerializeTest()
        {
            var argCaseList = new CommonEventSpecialArgCaseList(MakeArgCaseList(2).ToArray());
            var target = new CommonEventSpecialNumberArgDesc.InnerDescManual(argCaseList);
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