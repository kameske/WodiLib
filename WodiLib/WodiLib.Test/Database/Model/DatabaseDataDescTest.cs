using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseDataDescTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] DataNameTestCaseSource =
        {
            new object[] {(DataName) "dataName", false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(DataNameTestCaseSource))]
        public static void DataNameTest(DataName dataName, bool isError)
        {
            var instance = new DatabaseDataDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.DataName = dataName;
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
                var propertyValue = instance.DataName;
                // セットした値と取得した値が一致すること
                Assert.IsTrue(propertyValue.Equals(dataName));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseDataDesc.DataName)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void ItemValueListTest(bool isSetNull, bool isError)
        {
            var instance = new DatabaseDataDesc();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var itemValueList = isSetNull ? null : new DBItemValueList();

            var errorOccured = false;
            try
            {
                instance.ItemValueList = itemValueList;
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
                var propertyValue = instance.ItemValueList;

                Assert.NotNull(propertyValue);
                Assert.NotNull(itemValueList);

                // セットした値と取得した値が一致すること
                Assert.IsTrue(propertyValue.Equals(itemValueList));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(DatabaseDataDesc.ItemValueList)));
            }
        }

        [Test]
        public static void ConstructorTestA()
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseDataDesc();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
        }

        private static readonly object[] ConstructorBTestCaseSource =
        {
            new object[] {(DataName) "dataName", new DBItemValueList(), false},
            new object[] {(DataName) "dataName", null, true},
            new object[] {null, new DBItemValueList(), true},
            new object[] {null, null, true},
        };

        [TestCaseSource(nameof(ConstructorBTestCaseSource))]
        public static void ConstructorTestB(DataName dataName, DBItemValueList itemValueList, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseDataDesc(dataName, itemValueList);
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
        public static void SerializeTest()
        {
            var target = new DatabaseDataDesc
            {
                DataName = "DataName",
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