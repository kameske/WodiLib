using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal.DBItemSettingDesc
{
    [TestFixture]
    public class DBItemSettingDescNormalTest
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
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // プロパティが意図した値と一致すること
            Assert.AreEqual(instance.SettingType, DBItemSpecialSettingType.Normal);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseReferKindGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseReferKindSetterTest()
        {
            var instance = new DBItemSettingDescNormal();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseDbTypeIdGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
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
        public static void DatabaseDbTypeIdSetterTest()
        {
            var instance = new DBItemSettingDescNormal();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseUseAdditionalItemsFlagGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
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
        public static void DatabaseUseAdditionalItemsFlagSetterTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DatabaseUseAdditionalItemsFlag = true;
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
        public static void FolderNameGetterTest()
        {
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            Assert.AreEqual(instance.GetAllSpecialCase().Count, 0);
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

            // 取得した要素数が0であること
            Assert.AreEqual(result.Count, 0);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void AddSpecialCaseTest()
        {
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
        public static void InsertRangeSpecialCaseTest()
        {
            var instance = new DBItemSettingDescNormal();
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

        [Test]
        public static void UpdateDatabaseSpecialCaseTest()
        {
            var instance = new DBItemSettingDescNormal();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedArgCaseList = new List<string>();
            instance.ArgCaseList.PropertyChanged += (sender, args) => { changedArgCaseList.Add(args.PropertyName); };
            var changedArgCaseCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.ArgCaseList.CollectionChanged += (sender, args) => { changedArgCaseCollection.Add(args); };

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
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void UpdateManualSpecialCaseTest()
        {
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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
            var instance = new DBItemSettingDescNormal();
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