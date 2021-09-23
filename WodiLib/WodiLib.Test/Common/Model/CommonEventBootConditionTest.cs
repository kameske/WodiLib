using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventBootConditionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] CommonEventBootTypeTestCaseSource =
        {
            new object[] { null, true },
            new object[] { CommonEventBootType.Parallel, false }
        };

        [TestCaseSource(nameof(CommonEventBootTypeTestCaseSource))]
        public static void CommonEventBootTypeTest(CommonEventBootType type, bool isError)
        {
            var instance = new CommonEventBootCondition();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CommonEventBootType = type;
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
                var getValue = instance.CommonEventBootType;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(getValue == type);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventBootCondition.CommonEventBootType)));
            }
        }

        [TestCase(-1, false)] // null
        [TestCase(1000000, false)] // MapEventSelfVariableAddress (Not NumberVariableAddress)
        [TestCase(2000000, false)] // NormalNumberVariableAddress
        [TestCase(2100000, false)] // SpareNumberVariableAddress
        public static void LeftSideTest(int leftSide, bool isError)
        {
            var instance = new CommonEventBootCondition();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.LeftSide = leftSide;
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
                var getValue = instance.LeftSide;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(getValue == leftSide);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventBootCondition.LeftSide)));
        }

        [Test]
        public static void RightSideTest()
        {
            var instance = new CommonEventBootCondition();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var rightSide = (ConditionRight)100;

            var errorOccured = false;
            try
            {
                instance.RightSide = rightSide;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            var getValue = instance.RightSide;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(getValue == rightSide);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 1);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEventBootCondition.RightSide)));
        }
    }
}
