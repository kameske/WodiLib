using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventIdTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(-2, true)]
        [TestCase(-1, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void ConstructorTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MapEventId(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0)]
        [TestCase(99)]
        public static void ToIntTest(int value)
        {
            var instance = new MapEventId(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        private static readonly object[] CompareTestCaseSource =
        {
            new object[] {-1, -1},
            new object[] {-1, 0},
            new object[] {-1, 1},
            new object[] {-1, 5},
            new object[] {1, -1},
            new object[] {1, 1},
            new object[] {1, 5},
            new object[] {3, -1},
            new object[] {3, 1},
            new object[] {3, 3},
            new object[] {3, 5},
        };

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void CompareToTest(int left, int right)
        {
            var leftInstance = (MapEventId) left;
            var rightInstance = (MapEventId) right;

            var result = leftInstance.CompareTo(rightInstance);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result < 0, left.CompareTo(right) < 0);
            Assert.AreEqual(result == 0, left.CompareTo(right) == 0);
            Assert.AreEqual(result > 0, left.CompareTo(right) > 0);
        }

        [TestCase(-2, true)]
        [TestCase(-1, false)]
        [TestCase(9999, false)]
        [TestCase(10000, true)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MapEventId) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0)]
        [TestCase(99)]
        public static void CastToIntTest(int value)
        {
            var castValue = 0;

            var instance = new MapEventId(value);

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
            new object[] {0, 0, true},
            new object[] {0, 31, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapEventId) left;
            var rightIndex = (MapEventId) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapEventId) left;
            var rightIndex = (MapEventId) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapEventId) left;
            var rightIndex = (MapEventId) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorLessTest(int left, int right)
        {
            var leftInstance = (MapEventId) left;
            var rightInstance = (MapEventId) right;
            Assert.AreEqual(leftInstance < rightInstance, left < right);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorLessOrEqualTest(int left, int right)
        {
            var leftInstance = (MapEventId) left;
            var rightInstance = (MapEventId) right;
            Assert.AreEqual(leftInstance <= rightInstance, left <= right);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorGreaterOrEqualTest(int left, int right)
        {
            var leftInstance = (MapEventId) left;
            var rightInstance = (MapEventId) right;
            Assert.AreEqual(leftInstance >= rightInstance, left >= right);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorGreaterTest(int left, int right)
        {
            var leftInstance = (MapEventId) left;
            var rightInstance = (MapEventId) right;
            Assert.AreEqual(leftInstance > rightInstance, left > right);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (MapEventId) 121;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}