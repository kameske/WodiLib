using System;
using Commons;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventSpecialArgCaseNumberTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }


        [TestCase(-2000000001, true)]
        [TestCase(-2000000000, false)]
        [TestCase(2000000000, false)]
        [TestCase(2000000001, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CommonEventSpecialArgCaseNumber(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-2000000001, true)]
        [TestCase(-2000000000, false)]
        [TestCase(2000000000, false)]
        [TestCase(2000000001, true)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var instance = default(CommonEventSpecialArgCaseNumber);
            var errorOccured = false;
            try
            {
                instance = value;
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
            Assert.AreEqual((int)instance, value);
        }

        [TestCase(-2000000000)]
        [TestCase(2000000000)]
        public static void CastToIntTest(int value)
        {
            var castValue = 0;

            var instance = new CommonEventSpecialArgCaseNumber(value);

            var errorOccured = false;
            try
            {
                castValue = instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 元の値と一致すること
            Assert.AreEqual(castValue, value);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] { 0, 0, true },
            new object[] { 0, 3, false }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CommonEventSpecialArgCaseNumber)left;
            var rightIndex = (CommonEventSpecialArgCaseNumber)right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CommonEventSpecialArgCaseNumber)left;
            var rightIndex = (CommonEventSpecialArgCaseNumber)right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CommonEventSpecialArgCaseNumber)left;
            var rightIndex = (CommonEventSpecialArgCaseNumber)right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
