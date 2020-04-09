using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal.DBItemSettingDesc
{
    [TestFixture]
    public class DBItemSettingDescDatabaseTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [Test]
        public static void SettingTypeTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // プロパティが意図とした値と一致すること
            Assert.AreEqual(instance.SettingType, DBItemSpecialSettingType.ReferDatabase);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseReferKindGetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void DatabaseReferKindSetterTest(bool isSetNull, bool isError)
        {
            var setValue = isSetNull ? null : DBReferType.User;

            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseReferKind = setValue;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(instance.DatabaseReferKind)));
            }
        }

        [Test]
        public static void DatabaseDbTypeIdGetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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
        public static void DatabaseDbTypeIdSetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseDbTypeId = 10;
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
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(instance.DatabaseDbTypeId)));
        }

        [Test]
        public static void DatabaseUseAdditionalItemsFlagGetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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
        public static void DatabaseUseAdditionalItemsFlagSetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(instance.DatabaseUseAdditionalItemsFlag)));
        }


        [Test]
        public static void FolderNameGetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void FolderNameSetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void OmissionFolderNameFlagGetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void OmissionFolderNameFlagSetterTest()
        {
            var instance = new DBItemSettingDescDatabase();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DefaultType()
        {
            var instance = new DBItemSettingDescDatabase();
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
            DBItemSettingDescDatabase instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBItemSettingDescDatabase();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 選択肢が0であること
            Assert.AreEqual(instance.GetAllSpecialCase().Count, 0);
        }

        [TestCase(false, 0)]
        [TestCase(true, 3)]
        public static void GetAllSpecialCaseTest(bool useAdditionalItems,
            int resultLength)
        {
            var instance = new DBItemSettingDescDatabase
            {
                DatabaseUseAdditionalItemsFlag = useAdditionalItems
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            List<DatabaseValueCase> allCase = null;

            var errorOccured = false;
            try
            {
                allCase = instance.GetAllSpecialCase();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素数が意図した値と一致すること
            Assert.AreEqual(allCase.Count, resultLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        private static readonly object[] GetAllSpecialCaseNumberTestCaseSource =
        {
            new object[] {DBReferType.User, (TypeId) 3, false},
            new object[] {DBReferType.Changeable, (TypeId) 22, true},
        };

        [TestCaseSource(nameof(GetAllSpecialCaseNumberTestCaseSource))]
        public static void GetAllSpecialCaseNumberTest(DBReferType referType,
            TypeId typeId, bool useAdditionalItems)
        {
            var instance = new DBItemSettingDescDatabase
            {
                DatabaseReferKind = referType,
                DatabaseDbTypeId = typeId,
                DatabaseUseAdditionalItemsFlag = useAdditionalItems
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            List<DatabaseValueCaseNumber> result = null;

            var errorOccured = false;
            try
            {
                result = instance.GetAllSpecialCaseNumber();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素がそれぞれ意図した値であること
            Assert.AreEqual(result.Count, 3);
            Assert.AreEqual((int) result[0], referType.Code);
            Assert.AreEqual((int) result[1], (int) typeId);
            var flagValue = useAdditionalItems ? 1 : 0;
            Assert.AreEqual((int) result[2], flagValue);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [TestCase(true, 3)]
        [TestCase(false, 0)]
        public static void GetAllSpecialCaseDescriptionTest(bool useAdditionalItems,
            int resultLength)
        {
            var instance = new DBItemSettingDescDatabase
            {
                DatabaseUseAdditionalItemsFlag = useAdditionalItems
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            List<DatabaseValueCaseDescription> result = null;

            var errorOccured = false;
            try
            {
                result = instance.GetAllSpecialCaseDescription();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素数が意図した値であること
            Assert.AreEqual(result.Count, resultLength);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void AddSpecialCaseTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var specialCase = new DatabaseValueCase(0, "");
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddSpecialCase(specialCase);
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
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void AddRangeSpecialCaseTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var specialCases = new[]
            {
                new DatabaseValueCase(0, ""),
                new DatabaseValueCase(1, "a"),
                new DatabaseValueCase(2, "あいうえお"),
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.AddRangeSpecialCase(specialCases);
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
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void InsertSpecialCaseTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var specialCase = new DatabaseValueCase(0, "");
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertSpecialCase(0, specialCase);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void InsertRangeSpecialCaseTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var specialCases = new[]
            {
                new DatabaseValueCase(0, ""),
                new DatabaseValueCase(1, "a"),
                new DatabaseValueCase(2, "あいうえお"),
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.InsertRangeSpecialCase(0, specialCases);
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
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [TestCase(true, -4, "", true)]
        [TestCase(true, -3, "", false)]
        [TestCase(true, -1, "", false)]
        [TestCase(true, 0, "", true)]
        [TestCase(true, -4, "aaa", true)]
        [TestCase(true, -3, "aaa", false)]
        [TestCase(true, -1, "aaa", false)]
        [TestCase(true, 0, "aaa", true)]
        [TestCase(true, -4, "あいうえお", true)]
        [TestCase(true, -3, "あいうえお", false)]
        [TestCase(true, -1, "あいうえお", false)]
        [TestCase(true, 0, "あいうえお", true)]
        [TestCase(true, -4, "Hello\r\nWorld!", true)]
        [TestCase(true, -3, "Hello\r\nWorld!", true)]
        [TestCase(true, -1, "Hello\r\nWorld!", true)]
        [TestCase(true, 0, "Hello\r\nWorld!", true)]
        [TestCase(true, -4, "Wolf\nRPG\nEditor.", true)]
        [TestCase(true, -3, "Wolf\nRPG\nEditor.", true)]
        [TestCase(true, -1, "Wolf\nRPG\nEditor.", true)]
        [TestCase(true, 0, "Wolf\nRPG\nEditor.", true)]
        [TestCase(true, -4, null, true)]
        [TestCase(true, -3, null, true)]
        [TestCase(true, -1, null, true)]
        [TestCase(true, 0, null, true)]
        [TestCase(false, -4, "", true)]
        [TestCase(false, -3, "", false)]
        [TestCase(false, -1, "", false)]
        [TestCase(false, 0, "", true)]
        [TestCase(false, -4, "aaa", true)]
        [TestCase(false, -3, "aaa", false)]
        [TestCase(false, -1, "aaa", false)]
        [TestCase(false, 0, "aaa", true)]
        [TestCase(false, -4, "あいうえお", true)]
        [TestCase(false, -3, "あいうえお", false)]
        [TestCase(false, -1, "あいうえお", false)]
        [TestCase(false, 0, "あいうえお", true)]
        [TestCase(false, -4, "Hello\r\nWorld!", true)]
        [TestCase(false, -3, "Hello\r\nWorld!", true)]
        [TestCase(false, -1, "Hello\r\nWorld!", true)]
        [TestCase(false, 0, "Hello\r\nWorld!", true)]
        [TestCase(false, -4, "Wolf\nRPG\nEditor.", true)]
        [TestCase(false, -3, "Wolf\nRPG\nEditor.", true)]
        [TestCase(false, -1, "Wolf\nRPG\nEditor.", true)]
        [TestCase(false, 0, "Wolf\nRPG\nEditor.", true)]
        [TestCase(false, -4, null, true)]
        [TestCase(false, -3, null, true)]
        [TestCase(false, -1, null, true)]
        [TestCase(false, 0, null, true)]
        public static void UpdateDatabaseSpecialCaseTest(bool useAdditionalItemFlag,
            int caseNumber, string description, bool isError)
        {
            var instance = new DBItemSettingDescDatabase
            {
                DatabaseUseAdditionalItemsFlag = useAdditionalItemFlag
            };
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

            // エラーフラグが一致すること（追加項目仕様フラグによらないこと）
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                foreach (var specialCase in instance.GetAllSpecialCase())
                {
                    if (specialCase.CaseNumber == caseNumber)
                    {
                        // 指定した引数の文字列が正しく更新されていること
                        Assert.AreEqual((string) specialCase.Description, description);
                    }
                    else
                    {
                        // 指定していない引数の文字列が変化していないこと
                        Assert.AreEqual((string) specialCase.Description, "");
                    }
                }
            }

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            if (!errorOccured)
            {
                Assert.AreEqual(changedArgCaseList.Count, 1);
                Assert.IsTrue(changedArgCaseList[0].Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedArgCaseCollection.Count, 1);
                Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Replace);
                Assert.IsTrue(changedArgCaseCollection[0].OldStartingIndex == caseNumber * -1 - 1);
                Assert.IsTrue(changedArgCaseCollection[0].NewStartingIndex == caseNumber * -1 - 1);
            }
            else
            {
                Assert.AreEqual(changedArgCaseList.Count, 0);
                Assert.AreEqual(changedArgCaseCollection.Count, 0);
            }
        }

        [Test]
        public static void UpdateManualSpecialCaseTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

            var errorOccured = false;
            try
            {
                instance.UpdateManualSpecialCase(0, new DatabaseValueCase(0, ""));
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
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void RemoveSpecialCaseAtTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

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
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void ClearSpecialCaseTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

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
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void UpdateDefaultFolderTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.UpdateDefaultFolder("");
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
        public static void UpdateOmissionFolderNameFlagTest()
        {
            var instance = new DBItemSettingDescDatabase();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.UpdateOmissionFolderNameFlag(true);
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

        private static readonly object[] CanSetItemTestCaseSource =
        {
            new object[] {null, true, false},
            new object[] {DBItemType.Int, false, true},
            new object[] {DBItemType.String, false, false},
        };

        [TestCaseSource(nameof(CanSetItemTestCaseSource))]
        public static void CanSetItemTypeTest(DBItemType type, bool isError, bool answer)
        {
            var instance = new DBItemSettingDescDatabase();
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

            if (errorOccured) return;

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        private static readonly object[] EqualsTestCaseSource =
        {
            new object[] {null, false},
            new object[] {nameof(DBItemSettingDescNormal), false},
            new object[] {nameof(DBItemSettingDescLoadFile), false},
            new object[] {nameof(DBItemSettingDescManual), false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.Changeable)}_{3}_{false}", true},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.Changeable)}_{3}_{true}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.Changeable)}_{2}_{false}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.Changeable)}_{2}_{true}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.User)}_{3}_{false}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.User)}_{3}_{true}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.User)}_{2}_{false}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.User)}_{2}_{true}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.System)}_{3}_{false}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.System)}_{3}_{true}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.System)}_{2}_{false}", false},
            new object[] {$"{nameof(DBItemSettingDescDatabase)}_{nameof(DBReferType.System)}_{2}_{true}", false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void Equals(string settingDescCode, bool answer)
        {
            var instance = new DBItemSettingDescDatabase
            {
                DatabaseReferKind = DBReferType.Changeable,
                DatabaseDbTypeId = 3,
                DatabaseUseAdditionalItemsFlag = false,
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IDBItemSettingDesc desc = null;
            switch (settingDescCode)
            {
                case nameof(DBItemSettingDescNormal):
                    desc = new DBItemSettingDescNormal();
                    break;
                case nameof(DBItemSettingDescLoadFile):
                    desc = new DBItemSettingDescLoadFile();
                    break;
                case nameof(DBItemSettingDescManual):
                    desc = new DBItemSettingDescManual();
                    break;
                case null:
                    break;
                default:
                    var settings = settingDescCode.Split('_');
                    var dbDesc = new DBItemSettingDescDatabase();
                    switch (settings[1])
                    {
                        case nameof(DBReferType.Changeable):
                            dbDesc.DatabaseReferKind = DBReferType.Changeable;
                            break;
                        case nameof(DBReferType.User):
                            dbDesc.DatabaseReferKind = DBReferType.User;
                            break;
                        case nameof(DBReferType.System):
                            dbDesc.DatabaseReferKind = DBReferType.System;
                            break;
                        default:
                            Assert.Fail();
                            break;
                    }

                    dbDesc.DatabaseDbTypeId = int.Parse(settings[2]);
                    dbDesc.DatabaseUseAdditionalItemsFlag = bool.Parse(settings[3]);
                    desc = dbDesc;
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
            var target = new DBItemSettingDescDatabase
            {
                DatabaseDbTypeId = 32,
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