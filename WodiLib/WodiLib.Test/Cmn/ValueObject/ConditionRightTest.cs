using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class ConditionRightTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1000000, true)]
        [TestCase(-999999, false)]
        [TestCase(999999, false)]
        [TestCase(1000000, true)]
        public static void ConstructorTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ConditionRight(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1000000, true)]
        [TestCase(-999999, false)]
        [TestCase(999999, false)]
        [TestCase(1000000, true)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (ConditionRight)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // キャストした結果が一致すること
            Assert.AreEqual((int)(ConditionRight)value, value);
        }

        [TestCase(-999999)]
        [TestCase(999999)]
        public static void CastToIntTest(int value)
        {
            var instance = new ConditionRight(value);

            var errorOccured = false;
            try
            {
                var _ = (int)instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // キャストした結果が一致すること
            Assert.AreEqual((int)instance, value);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] { 0, 0, true },
            new object[] { 0, 31, false }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ConditionRight)left;
            var rightIndex = (ConditionRight)right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ConditionRight)left;
            var rightIndex = (ConditionRight)right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ConditionRight)left;
            var rightIndex = (ConditionRight)right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
