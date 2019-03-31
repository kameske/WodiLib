using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventBootConditionTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] CommonEventBootTypeTestCaseSource =
        {
            new object[] {null, true},
            new object[] {CommonEventBootType.Parallel, false},
        };

        [TestCaseSource(nameof(CommonEventBootTypeTestCaseSource))]
        public static void CommonEventBootTypeTest(CommonEventBootType type, bool isError)
        {
            var instance = new CommonEventBootCondition();

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

            if (errorOccured) return;

            var getValue = instance.CommonEventBootType;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(getValue == type);
        }

        [TestCase(-1, true)] // null
        [TestCase(1000000, true)] // MapEventSelfVariableAddress (Not NumberVariableAddress)
        [TestCase(2000000, false)] // NormalNumberVariableAddress
        [TestCase(2100000, false)] // SpareNumberVariableAddress
        public static void LeftSideTest(int leftSide, bool isError)
        {
            var instance = new CommonEventBootCondition();

            var leftSideAddress = leftSide == -1
                ? null
                : VariableAddressFactory.Create(leftSide);

            var errorOccured = false;
            try
            {
                instance.LeftSide = leftSideAddress;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var getValue = instance.LeftSide;

            // セットした値と取得した値が一致すること
            Assert.NotNull(getValue);
            Assert.IsTrue(getValue.Equals(leftSideAddress));
        }

        [Test]
        public static void RightSideTest()
        {
            var instance = new CommonEventBootCondition();
            var rightSide = (ConditionRight) 100;

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
        }
    }
}