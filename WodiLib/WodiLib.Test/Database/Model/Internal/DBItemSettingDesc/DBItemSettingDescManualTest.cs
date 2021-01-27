using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Internal.DBItemSettingDesc
{
    [TestFixture]
    public class DBItemSettingDescManualTest
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
            var instance = new DBItemSettingDescManual();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // プロパティが意図した値と一致すること
            Assert.AreEqual(instance.SettingType, DBItemSpecialSettingType.Manual);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void DatabaseReferenceDescGetterTest()
        {
            var instance = new DBItemSettingDescManual();
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
            var instance = new DBItemSettingDescManual();
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
            var instance = new DBItemSettingDescManual();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void NormalDescGetterTest()
        {
            var instance = new DBItemSettingDescManual();
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
        public static void ConstructorTestA()
        {
            DBItemSettingDescManual instance = null;

            var errorOccured = false;
            try
            {
                instance = new DBItemSettingDescManual();
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

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public static void ConstructorTestB(int initItemLength)
        {
            DBItemSettingDescManual instance = null;

            var initList = MakeInitList(initItemLength);

            var errorOccured = false;
            try
            {
                instance = new DBItemSettingDescManual(initList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 選択肢が意図した数であること
            var answerResultLength = initItemLength != -1
                ? initItemLength
                : 0;
            Assert.AreEqual(instance.GetAllSpecialCase().ToList().Count, answerResultLength);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(5)]
        public static void GetAllSpecialCaseTest(int initItemLength)
        {
            var initList = MakeInitList(initItemLength);

            var instance = new DBItemSettingDescManual(initList);
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

            // 取得した要素数が意図した値と一致すること
            var answerResultLength = initItemLength != -1
                ? initItemLength
                : 0;
            Assert.AreEqual(allCase.Count, answerResultLength);

            // 取得した要素がそれぞれ意図した値であること
            for (var i = 0; i < answerResultLength; i++)
            {
                var answerCaseValue = new DatabaseValueCase(i, i.ToString());
                Assert.AreEqual(allCase[i], answerCaseValue);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(5)]
        public static void GetAllSpecialCaseNumberTest(int initItemLength)
        {
            var initList = MakeInitList(initItemLength);

            var instance = new DBItemSettingDescManual(initList);
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

            // 取得した要素数が意図した値と一致すること
            var answerResultLength = initItemLength != -1
                ? initItemLength
                : 0;
            Assert.AreEqual(result.Count, answerResultLength);

            // 取得した要素がそれぞれ意図した値であること
            for (var i = 0; i < answerResultLength; i++)
            {
                Assert.AreEqual((int) result[i], i);
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(5)]
        public static void GetAllSpecialCaseDescriptionTest(int initItemLength)
        {
            var initList = MakeInitList(initItemLength);

            var instance = new DBItemSettingDescManual(initList);
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
                result = instance.GetAllSpecialCaseDescription().ToList();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した要素数が意図した値と一致すること
            var answerResultLength = initItemLength != -1
                ? initItemLength
                : 0;
            Assert.AreEqual(result.Count, answerResultLength);

            // 取得した要素がそれぞれ意図した値であること
            for (var i = 0; i < answerResultLength; i++)
            {
                Assert.AreEqual((string) result[i], i.ToString());
            }

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 0);
            Assert.AreEqual(changedArgCaseCollection.Count, 0);
        }

        [Test]
        public static void AddSpecialCaseTest()
        {
            var instance = new DBItemSettingDescManual();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 2);
            Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
            Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedArgCaseCollection.Count, 1);
            Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void AddRangeSpecialCaseTest()
        {
            var instance = new DBItemSettingDescManual();
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
                instance.AddSpecialCaseRange(specialCases);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 2);
            Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
            Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedArgCaseCollection.Count, 1);
            Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void InsertSpecialCaseTest()
        {
            var instance = new DBItemSettingDescManual();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 2);
            Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
            Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedArgCaseCollection.Count, 1);
            Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void InsertRangeSpecialCaseTest()
        {
            var instance = new DBItemSettingDescManual();
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
                instance.InsertSpecialCaseRange(0, specialCases);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 2);
            Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
            Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedArgCaseCollection.Count, 1);
            Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void UpdateManualSpecialCaseTest()
        {
            var instance = new DBItemSettingDescManual();
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
            var instance = new DBItemSettingDescManual();
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
            var instance = new DBItemSettingDescManual();
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

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedArgCaseList.Count, 2);
            Assert.IsTrue(changedArgCaseList[0].Equals(nameof(instance.ArgCaseList.Count)));
            Assert.IsTrue(changedArgCaseList[1].Equals(ListConstant.IndexerName));
            Assert.AreEqual(changedArgCaseCollection.Count, 1);
            Assert.IsTrue(changedArgCaseCollection[0].Action == NotifyCollectionChangedAction.Reset);
        }

        private static DatabaseValueCaseList MakeInitList(int length)
        {
            if (length == -1) return null;

            var result = new DatabaseValueCaseList();
            for (var i = 0; i < length; i++)
            {
                result.Add(new DatabaseValueCase(i, i.ToString()));
            }

            return result;
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
            new object[] {nameof(DBItemSettingDescNormal), false},
            new object[] {nameof(DBItemSettingDescLoadFile), false},
            new object[] {nameof(DBItemSettingDescDatabase), false},
            new object[] {$"{nameof(DBItemSettingDescManual)}_2-sentence,4-wolf", true},
            new object[] {$"{nameof(DBItemSettingDescManual)}_2-sentence,4-falcon", false},
            new object[] {$"{nameof(DBItemSettingDescManual)}_2-sentence,3-wolf", false},
            new object[] {$"{nameof(DBItemSettingDescManual)}_1-text", false},
            new object[] {$"{nameof(DBItemSettingDescManual)}_2-sentence", false},
            new object[] {$"{nameof(DBItemSettingDescManual)}_2-sentence,3-wolf,4-falcon", false},
        };

        [TestCaseSource(nameof(EqualsTestCaseSource))]
        public static void Equals(string settingDescCode, bool answer)
        {
            var instance = new DBItemSettingDescManual();
            instance.AddSpecialCase(new DatabaseValueCase(2, "sentence"));
            instance.AddSpecialCase(new DatabaseValueCase(4, "wolf"));
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
                case nameof(DBItemSettingDescDatabase):
                    desc = new DBItemSettingDescDatabase();
                    break;
                case null:
                    break;
                default:
                    var settings = settingDescCode.Split('_');
                    var maDesc = new DBItemSettingDescManual();
                    var argDescsCode = settings[1].Split(',');
                    foreach (var argDescs in argDescsCode)
                    {
                        var argDescSettings = argDescs.Split('-');
                        maDesc.AddSpecialCase(new DatabaseValueCase(int.Parse(argDescSettings[0]), argDescSettings[1]));
                    }

                    desc = maDesc;
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
    }
}
