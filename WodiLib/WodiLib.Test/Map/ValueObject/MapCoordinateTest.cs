using System;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapCoordinateTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void ConstructorTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MapCoordinate(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MapCoordinate)value;
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
        [TestCase(9999)]
        public static void CastToIntTest(int value)
        {
            var castValue = 0;

            var instance = new MapCoordinate(value);

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
            new object[] { 0, 1, false }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftItem = (MapCoordinate)left;
            var rightItem = (MapCoordinate)right;
            Assert.AreEqual(leftItem == rightItem, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftItem = (MapCoordinate)left;
            var rightItem = (MapCoordinate)right;
            Assert.AreEqual(leftItem != rightItem, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftItem = (MapCoordinate)left;
            var rightItem = (MapCoordinate)right;
            Assert.AreEqual(leftItem.Equals(rightItem), isEqual);
        }
    }
}
