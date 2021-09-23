using System;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class HitExtendSideLengthTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(0, false)]
        [TestCase(250, false)]
        [TestCase(251, false)]
        [TestCase(255, false)]
        public static void ConstructorTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new HitExtendSideLength(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0, false)]
        [TestCase(250, false)]
        [TestCase(251, false)]
        [TestCase(255, false)]
        public static void CastFromIntTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (HitExtendSideLength)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(3)]
        [TestCase(250)]
        public static void CastToIntTest(byte value)
        {
            var castValue = 0;

            var instance = new HitExtendSideLength(value);

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
            new object[] { (byte)0, (byte)0, true },
            new object[] { (byte)0, (byte)1, false }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(byte left, byte right, bool isEqual)
        {
            var leftItem = (HitExtendSideLength)left;
            var rightItem = (HitExtendSideLength)right;
            Assert.AreEqual(leftItem == rightItem, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(byte left, byte right, bool isEqual)
        {
            var leftItem = (HitExtendSideLength)left;
            var rightItem = (HitExtendSideLength)right;
            Assert.AreEqual(leftItem != rightItem, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(byte left, byte right, bool isEqual)
        {
            var leftItem = (HitExtendSideLength)left;
            var rightItem = (HitExtendSideLength)right;
            Assert.AreEqual(leftItem.Equals(rightItem), isEqual);
        }
    }
}
