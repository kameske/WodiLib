using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBTypeSettingTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] TypeNameTestCaseSource =
        {
            new object[] {(TypeName) "", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(TypeNameTestCaseSource))]
        public static void TypeNameTest(TypeName typeName, bool isError)
        {
            var instance = new DBTypeSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.TypeName = typeName;
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
                var setValue = instance.TypeName;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(typeName));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBTypeSetting.TypeName)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void DataNameListTest(bool isSetNull, bool isError)
        {
            var instance = new DBTypeSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var dataNameList = isSetNull ? null : new DataNameList();

            var errorOccured = false;
            try
            {
                instance.DataNameList = dataNameList;
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
                var setValue = instance.DataNameList;

                Assert.NotNull(setValue);
                Assert.NotNull(dataNameList);

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(dataNameList));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBTypeSetting.DataNameList)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void ItemSettingListTest(bool isSetNull, bool isError)
        {
            var instance = new DBTypeSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var itemSettingList = isSetNull ? null : new DBItemSettingList();

            var errorOccured = false;
            try
            {
                instance.ItemSettingList = itemSettingList;
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
                var setValue = instance.ItemSettingList;

                Assert.NotNull(setValue);
                Assert.NotNull(itemSettingList);

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(itemSettingList));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBTypeSetting.ItemSettingList)));
            }
        }

        private static readonly object[] MemoTestCaseSource =
        {
            new object[] {(DatabaseMemo) "", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(MemoTestCaseSource))]
        public static void MemoTest(DatabaseMemo memo, bool isError)
        {
            var instance = new DBTypeSetting();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.Memo = memo;
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
                var setValue = instance.Memo;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue.Equals(memo));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DBTypeSetting.Memo)));
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new DBTypeSetting
            {
                TypeName = "TypeName"
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