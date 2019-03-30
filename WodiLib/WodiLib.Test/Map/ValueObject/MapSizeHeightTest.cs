using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapSizeHeightTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(14, true)]
        [TestCase(15, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void ConstructorTest(int value, bool isError)
        {

            var errorOccured = false;
            try
            {
                var _ = new MapSizeHeight(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(15)]
        [TestCase(9999)]
        public static void ToIntTest(int value)
        {
            var instance = new MapSizeHeight(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }


        [TestCase(14, true)]
        [TestCase(15, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MapSizeHeight) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(15)]
        [TestCase(9999)]
        public static void CastToIntTest(int value)
        {
            var castValue = 0;

            var instance = new MapSizeHeight(value);

            var errorOccured = false;
            try
            {
                castValue = (int) instance;
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
            new object[] {20, 20, true},
            new object[] {20, 31, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapSizeHeight) left;
            var rightIndex = (MapSizeHeight) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapSizeHeight) left;
            var rightIndex = (MapSizeHeight) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapSizeHeight) left;
            var rightIndex = (MapSizeHeight) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}