using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialNumberArgDesc_InnerDescDatabaseTest
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // 取得した値が意図した値であること
            var type = instance.ArgType;
            Assert.AreEqual(type, CommonEventArgType.ReferDatabase);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseDbKindTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseDbTypeIdTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseUseAdditionalItemsFlagTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SpecialArgCaseListTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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

        private static readonly object[] SetDatabaseReferTestCaseSource =
        {
            new object[] {DBKind.Changeable, 0, false},
            new object[] {DBKind.User, 99, false},
            new object[] {DBKind.System, 30, false},
            new object[] {null, 55, true},
        };

        [TestCaseSource(nameof(SetDatabaseReferTestCaseSource))]
        public static void SetDatabaseReferTest(DBKind dbKind, int dbTypeId, bool isError)
        {
            var typeId = (TypeId) dbTypeId;

            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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

            // 意図したとおりプロパティ変更通知が発火していること
            if (isError)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 2);
                Assert.IsTrue(changedPropertyList[0]
                    .Equals(nameof(CommonEventSpecialNumberArgDesc.InnerDescDatabase.DatabaseUseDbKind)));
                Assert.IsTrue(changedPropertyList[1]
                    .Equals(nameof(CommonEventSpecialNumberArgDesc.InnerDescDatabase.DatabaseDbTypeId)));
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void SetDatabaseUseAdditionalItemsFlagTest(bool flag)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0]
                .Equals(nameof(CommonEventSpecialNumberArgDesc.InnerDescDatabase.DatabaseUseAdditionalItemsFlag)));
        }

        [TestCase(true, 3)]
        [TestCase(false, 0)]
        public static void GetSpecialCaseTest(bool flag, int answerLength)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
            instance.SetDatabaseUseAdditionalItemsFlag(flag);
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

            // 取得した配列数が意図した値と一致すること
            var argCaseLength = instance.GetAllSpecialCase().Count;
            Assert.AreEqual(argCaseLength, answerLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] GetAllSpecialCaseNumberTestCaseSource =
        {
            new object[] {DBKind.Changeable, 0, true, 3, 2, 1},
            new object[] {DBKind.User, 10, false, 3, 1, 0},
            new object[] {DBKind.System, 99, false, 3, 0, 0},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseNumberTestCaseSource))]
        public static void GetAllSpecialCaseNumberTest(DBKind dbKind, int dbTypeId, bool isUseAddition,
            int answerCaseNumberLength, int answerDbTypeCode, int answerUseAdditionValue)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
            instance.SetDatabaseRefer(dbKind, dbTypeId);
            instance.SetDatabaseUseAdditionalItemsFlag(isUseAddition);
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

            // 意図した値が取得できること
            var caseNumberList = instance.GetAllSpecialCaseNumber();
            Assert.AreEqual(caseNumberList.Count, answerCaseNumberLength);

            Assert.AreEqual(caseNumberList[0], answerDbTypeCode);
            Assert.AreEqual(caseNumberList[1], dbTypeId);
            Assert.AreEqual(caseNumberList[2], answerUseAdditionValue);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] GetAllSpecialCaseDescriptionTestCaseSource =
        {
            new object[] {true, "a", "b", "c", 3},
            new object[] {true, "a", null, "c", 3},
            new object[] {false, null, null, "c", 0},
            new object[] {false, null, null, null, 0},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseDescriptionTestCaseSource))]
        public static void GetAllSpecialCaseDescriptionTest(bool isUseAddition,
            string strMinus1, string strMinus2, string strMinus3, int resultLength)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
            instance.SetDatabaseUseAdditionalItemsFlag(isUseAddition);

            if (strMinus1 != null)
            {
                instance.UpdateDatabaseSpecialCase(-1, strMinus1);
            }

            if (strMinus2 != null)
            {
                instance.UpdateDatabaseSpecialCase(-2, strMinus2);
            }

            if (strMinus3 != null)
            {
                instance.UpdateDatabaseSpecialCase(-3, strMinus3);
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
                instance.GetAllSpecialCaseDescription();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図した値が取得できること
            var caseDescList = instance.GetAllSpecialCaseDescription();
            var caseDescListCount = caseDescList.Count;
            Assert.AreEqual(caseDescListCount, resultLength);

            if (caseDescListCount == 3)
            {
                Assert.AreEqual(caseDescList[0], strMinus1 ?? string.Empty);
                Assert.AreEqual(caseDescList[1], strMinus2 ?? string.Empty);
                Assert.AreEqual(caseDescList[2], strMinus3 ?? string.Empty);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        [Test]
        public static void AddSpecialCaseTest()
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
                var argCase = new CommonEventSpecialArgCase(0, "");
                instance.AddSpecialCase(argCase);
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

        [Test]
        public static void AddRangeSpecialCaseTest()
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
                var argCaseList = new List<CommonEventSpecialArgCase>
                {
                    new CommonEventSpecialArgCase(0, "")
                };
                instance.AddRangeSpecialCase(argCaseList);
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

        [Test]
        public static void InsertSpecialCaseTest()
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
                var argCase = new CommonEventSpecialArgCase(0, "");
                instance.InsertSpecialCase(0, argCase);
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

        [Test]
        public static void InsertRangeSpecialCaseTest()
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
                var argCaseList = new List<CommonEventSpecialArgCase>
                {
                    new CommonEventSpecialArgCase(0, "")
                };
                instance.InsertRangeSpecialCase(0, argCaseList);
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

        private static readonly object[] UpdateDatabaseSpecialCaseTestCaseSource =
        {
            new object[] {true, -4, null, true},
            new object[] {true, -4, "", true},
            new object[] {true, -4, "abc", true},
            new object[] {true, -4, "あいうえお", true},
            new object[] {true, -4, "New\r\nLine\r\nCRLF", true},
            new object[] {true, -4, "New\nLine\nLF", true},
            new object[] {true, -3, null, true},
            new object[] {true, -3, "", false},
            new object[] {true, -3, "abc", false},
            new object[] {true, -3, "あいうえお", false},
            new object[] {true, -3, "New\r\nLine\r\nCRLF", true},
            new object[] {true, -3, "New\nLine\nLF", true},
            new object[] {true, -1, null, true},
            new object[] {true, -1, "", false},
            new object[] {true, -1, "abc", false},
            new object[] {true, -1, "あいうえお", false},
            new object[] {true, -1, "New\r\nLine\r\nCRLF", true},
            new object[] {true, -1, "New\nLine\nLF", true},
            new object[] {true, 0, null, true},
            new object[] {true, 0, "", true},
            new object[] {true, 0, "abc", true},
            new object[] {true, 0, "あいうえお", true},
            new object[] {true, 0, "New\r\nLine\r\nCRLF", true},
            new object[] {true, 0, "New\nLine\nLF", true},
            new object[] {false, -4, null, true},
            new object[] {false, -4, "", true},
            new object[] {false, -4, "abc", true},
            new object[] {false, -4, "あいうえお", true},
            new object[] {false, -4, "New\r\nLine\r\nCRLF", true},
            new object[] {false, -4, "New\nLine\nLF", true},
            new object[] {false, -3, null, true},
            new object[] {false, -3, "", false},
            new object[] {false, -3, "abc", false},
            new object[] {false, -3, "あいうえお", false},
            new object[] {false, -3, "New\r\nLine\r\nCRLF", true},
            new object[] {false, -3, "New\nLine\nLF", true},
            new object[] {false, -1, null, true},
            new object[] {false, -1, "", false},
            new object[] {false, -1, "abc", false},
            new object[] {false, -1, "あいうえお", false},
            new object[] {false, -1, "New\r\nLine\r\nCRLF", true},
            new object[] {false, -1, "New\nLine\nLF", true},
            new object[] {false, 0, null, true},
            new object[] {false, 0, "", true},
            new object[] {false, 0, "abc", true},
            new object[] {false, 0, "あいうえお", true},
            new object[] {false, 0, "New\r\nLine\r\nCRLF", true},
            new object[] {false, 0, "New\nLine\nLF", true},
        };

        [TestCaseSource(nameof(UpdateDatabaseSpecialCaseTestCaseSource))]
        public static void UpdateDatabaseSpecialCaseTest(bool isUseAddition,
            int caseNumber, string description, bool isError)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
            instance.SetDatabaseUseAdditionalItemsFlag(isUseAddition);
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

        [Test]
        public static void UpdateManualSpecialCaseTest()
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
                var argCase = new CommonEventSpecialArgCase(0, "");
                instance.UpdateManualSpecialCase(-1, argCase);
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

        [Test]
        public static void RemoveSpecialCaseAtTest()
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
                instance.RemoveSpecialCaseAt(0);
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

        [Test]
        public static void RemoveSpecialCaseRangeTest()
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
                instance.RemoveSpecialCaseRange(0, 1);
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

        [Test]
        public static void ClearSpecialCaseTest()
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
                instance.ClearSpecialCase();
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

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonEventSpecialNumberArgDesc.InnerDescDatabase();
            target.SetDatabaseRefer(DBKind.System, 20);
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}