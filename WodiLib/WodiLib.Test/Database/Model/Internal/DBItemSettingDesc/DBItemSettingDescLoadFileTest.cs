using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal.DBItemSettingDesc
{
    [TestFixture]
    public class DBItemSettingDescLoadFIleTest
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
            var instance = new DBItemSettingDescLoadFile((0, ""));
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // プロパティが意図した値と一致すること
            Assert.AreEqual(instance.SettingType, DBItemSpecialSettingType.LoadFile);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseReferenceDescGetterTest()
        {
            var instance = new DBItemSettingDescLoadFile();
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
            var instance = new DBItemSettingDescLoadFile();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void ManualDescGetterTest()
        {
            var instance = new DBItemSettingDescLoadFile();
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
            var instance = new DBItemSettingDescLoadFile();
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

            // エラーが発生すること
            Assert.IsTrue(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void FolderNameGetterTest()
        {
            var instance = new DBItemSettingDescLoadFile((0, ""));
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void FolderNameSetterTest(bool isSetNull, bool isError)
        {
            var instance = new DBItemSettingDescLoadFile((0, ""));
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var setValue = isSetNull ? null : (DBSettingFolderName) "";

            var errorOccured = false;
            try
            {
                instance.FolderName = setValue;
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

        [Test]
        public static void OmissionFolderNameFlagGetterTest()
        {
            var instance = new DBItemSettingDescLoadFile((0, ""));
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void OmissionFolderNameFlagSetterTest()
        {
            var instance = new DBItemSettingDescLoadFile((0, ""));
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.OmissionFolderNameFlag = false;
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
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBItemSettingDescLoadFile.OmissionFolderNameFlag)));
        }

        [Test]
        public static void DefaultType()
        {
            var instance = new DBItemSettingDescLoadFile();
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

        [TestCase(0, null, false)]
        [TestCase(-1, "", true)]
        [TestCase(-1, "あいうえお", true)]
        [TestCase(0, "", false)]
        [TestCase(0, "あいうえお", false)]
        [TestCase(1, "", false)]
        [TestCase(1, "あいうえお", false)]
        [TestCase(2, "", true)]
        [TestCase(2, "あいうえお", true)]
        public static void ConstructorTest(int caseNumber, string description, bool isError)
        {
            var valueCase = description == null
                ? null
                : new DatabaseValueCase(caseNumber, description);

            var errorOccured = false;
            try
            {
                var _ = new DBItemSettingDescLoadFile(valueCase);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void GetAllSpecialCaseTest()
        {
            var instance = new DBItemSettingDescLoadFile((0, ""));
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

            // 取得した要素数が1であること
            Assert.AreEqual(allCase.Count, 1);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [TestCase(true, 1)]
        [TestCase(false, 0)]
        public static void GetAllSpecialCaseNumberTest(bool isOmission, int resultCaseNumber)
        {
            var instance = new DBItemSettingDescLoadFile((0, ""))
            {
                OmissionFolderNameFlag = isOmission
            };
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

            // 取得した要素数が1であること
            Assert.AreEqual(result.Count, 1);

            // 取得した要素がそれぞれ意図した値であること
            Assert.AreEqual((int) result[0], resultCaseNumber);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [TestCase(false, 1)]
        [TestCase(true, 1)]
        public static void GetAllSpecialCaseDescriptionTest(bool useAdditionalItems,
            int resultLength)
        {
            var instance = new DBItemSettingDescLoadFile((0, ""));
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

            // 取得した要素数が意図した値であること
            Assert.AreEqual(result.Count, 1);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        private static readonly object[] CanSetItemTestCaseSource =
        {
            new object[] {null, true, false},
            new object[] {DBItemType.Int, false, false},
            new object[] {DBItemType.String, false, true},
        };

        [TestCaseSource(nameof(CanSetItemTestCaseSource))]
        public static void CanSetItemTypeTest(DBItemType type, bool isError, bool answer)
        {
            var instance = new DBItemSettingDescLoadFile();
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
            new object[] {nameof(DBItemSettingDescDatabase), false},
            new object[] {nameof(DBItemSettingDescManual), false},
            new object[] {$"{nameof(DBItemSettingDescLoadFile)}_folder_{true}", true},
            new object[] {$"{nameof(DBItemSettingDescLoadFile)}_folder_{false}", false},
            new object[] {$"{nameof(DBItemSettingDescLoadFile)}_directory_{true}", false},
            new object[] {$"{nameof(DBItemSettingDescLoadFile)}_directory_{false}", false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void Equals(string settingDescCode, bool answer)
        {
            var instance = new DBItemSettingDescLoadFile
            {
                FolderName = "folder",
                OmissionFolderNameFlag = true
            };
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            IDBItemSettingDesc desc = null;
            switch (settingDescCode)
            {
                case nameof(DBItemSettingDescNormal):
                    desc = new DBItemSettingDescNormal();
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
                    var settings = settingDescCode.Split('_');
                    var lfDesc = new DBItemSettingDescLoadFile();
                    lfDesc.FolderName = settings[1];
                    lfDesc.OmissionFolderNameFlag = bool.Parse(settings[2]);
                    desc = lfDesc;
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
            var target = new DBItemSettingDescLoadFile
            {
                FolderName = "FolderName"
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
