using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ChoiceCaseListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(12, false)]
        [TestCase(13, true)]
        public static void CaseValueSetterTest(int setValue, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ChoiceCaseList {CaseValue = setValue};
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0, 1, false)]
        [TestCase(0, 2, false)]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        [TestCase(0, 9, false)]
        [TestCase(9, 9, true)]
        [TestCase(10, 9, true)]
        public static void GetAccessorTest(int index, int caseValue, bool isError)
        {
            var errorOccured = false;
            var instance = new ChoiceCaseList {CaseValue = caseValue};
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            try
            {
                var _ = instance[index];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(0, 1, false)]
        [TestCase(0, 2, false)]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        [TestCase(0, 9, false)]
        [TestCase(9, 9, true)]
        [TestCase(10, 9, true)]
        public static void GetTest(int index, int caseValue, bool isError)
        {
            var errorOccured = false;
            var instance = new ChoiceCaseList {CaseValue = caseValue};
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            try
            {
                var _ = instance.Get(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.AreEqual(errorOccured, isError);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [TestCase(0, 1, false)]
        [TestCase(0, 2, false)]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        [TestCase(0, 9, false)]
        [TestCase(8, 9, false)]
        [TestCase(9, 9, true)]
        public static void SetAccessorTest(int index, int caseValue, bool isError)
        {
            var errorOccured = false;
            var instance = new ChoiceCaseList {CaseValue = caseValue};
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            try
            {
                instance[index] = "";
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(ListConstant.IndexerName));
            }
        }

        [TestCase(0, 1, false)]
        [TestCase(0, 2, false)]
        [TestCase(1, 2, false)]
        [TestCase(2, 2, true)]
        [TestCase(0, 9, false)]
        [TestCase(8, 9, false)]
        [TestCase(9, 9, true)]
        public static void SetTest(int index, int caseValue, bool isError)
        {
            var errorOccured = false;
            var instance = new ChoiceCaseList {CaseValue = caseValue};
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            try
            {
                instance.Set(index, "");
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(ListConstant.IndexerName));
            }
        }

        [TestCase(0, 1, "abc")]
        [TestCase(0, 9, "woditor")]
        [TestCase(8, 9, "Test")]
        public static void AccessorTest(int index, int caseValue, string str)
        {
            var initObj = new ChoiceCaseList {CaseValue = caseValue};
            var instance = new ChoiceCaseList {CaseValue = caseValue};
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            instance.Set(index, str);

            // 設定した文字列が取得できること
            var getStr = instance.Get(index);
            Assert.IsTrue(getStr.Equals(str));

            // 設定していない箇所が変化していないこと
            for (var i = 0; i < caseValue; i++)
                if (i != index)
                    Assert.IsTrue(instance.Get(i).Equals(initObj.Get(i)));
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new ChoiceCaseList
            {
                CaseValue = 3,
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