using System;
using NUnit.Framework;
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

        [TestCase(-1, false)]
        [TestCase(1600000, false)]
        [TestCase(2000000, false)]
        public static void LeftSideTest(int leftSide, bool isError)
        {
            var instance = new CommonEventBootCondition();

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

            if (errorOccured) return;

            var getValue = instance.LeftSide;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(getValue == leftSide);
        }

        [TestCase(-1000000, true)]
        [TestCase(-999999, false)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(999999, false)]
        [TestCase(1000000, true)]
        public static void RightSideTest(int rightSide, bool isError)
        {
            var instance = new CommonEventBootCondition();

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

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            var getValue = instance.RightSide;

            // セットした値と取得した値が一致すること
            Assert.IsTrue(getValue == rightSide);
        }
    }
}