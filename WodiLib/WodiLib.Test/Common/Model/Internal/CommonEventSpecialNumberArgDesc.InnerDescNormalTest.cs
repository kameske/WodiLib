using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialNumberArgDesc_InnerDescNormalTest
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // 取得した値が意図した値であること
            var type = instance.ArgType;
            Assert.AreEqual(type, CommonEventArgType.Normal);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseDbKindTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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

            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
        public static void GetSpecialCaseTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            Assert.AreEqual(argCaseLength, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        [Test]
        public static void GetAllSpecialCaseNumberTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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

            // 取得した配列数が0件であること
            var argCaseLength = instance.GetAllSpecialCase().Count;
            Assert.AreEqual(argCaseLength, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        private static readonly object[] GetAllSpecialCaseDescriptionTestCaseSource =
        {
            new object[] {true, "a", "b", "c"},
            new object[] {true, "a", null, "c"},
            new object[] {false, null, null, "c"},
            new object[] {false, null, null, null},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseDescriptionTestCaseSource))]
        public static void GetAllSpecialCaseDescriptionTest(bool isUseAddition,
            string strMinus1, string strMinus2, string strMinus3)
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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

            // 取得した配列数が0件であること
            var argCaseLength = instance.GetAllSpecialCase().Count;
            Assert.AreEqual(argCaseLength, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialArgCaseListCollectionArgList.Count, 0);
        }

        [Test]
        public static void AddSpecialCaseTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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

        [Test]
        public static void UpdateDatabaseSpecialCaseTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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

        [Test]
        public static void UpdateDatabaseSpecialCase()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
        public static void ClearTest()
        {
            var instance = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
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
            var target = new CommonEventSpecialNumberArgDesc.InnerDescNormal();
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}