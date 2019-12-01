using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapCharacterIdTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(int.MinValue, false)]
        [TestCase(int.MaxValue, false)]
        public static void ConstructorTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MapCharacterId(value);
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
            var instance = new MapCharacterId(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        private static readonly object[] CompareTestCaseSource =
        {
            new object[] {-5, -7},
            new object[] {-5, -5},
            new object[] {-5, -1},
            new object[] {-5, 0},
            new object[] {-5, 1},
            new object[] {-5, 5},
            new object[] {-5, 8},
            new object[] {0, -7},
            new object[] {0, -5},
            new object[] {0, -1},
            new object[] {0, 0},
            new object[] {0, 1},
            new object[] {0, 5},
            new object[] {0, 8},
            new object[] {5, -7},
            new object[] {5, -5},
            new object[] {5, -1},
            new object[] {5, 0},
            new object[] {5, 1},
            new object[] {5, 5},
            new object[] {5, 8},
        };

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void CompareToTest(int left, int right)
        {
            var leftInstance = (MapCharacterId) left;
            var rightInstance = (MapCharacterId) right;

            var result = leftInstance.CompareTo(rightInstance);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result < 0, left.CompareTo(right) < 0);
            Assert.AreEqual(result == 0, left.CompareTo(right) == 0);
            Assert.AreEqual(result > 0, left.CompareTo(right) > 0);
        }

        [TestCase(int.MaxValue, false)]
        [TestCase(int.MinValue, false)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MapCharacterId) value;
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

            var instance = new MapCharacterId(value);

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
            var leftIndex = (MapCharacterId) left;
            var rightIndex = (MapCharacterId) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapCharacterId) left;
            var rightIndex = (MapCharacterId) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapCharacterId) left;
            var rightIndex = (MapCharacterId) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorLessTest(int left, int right)
        {
            var leftInstance = (MapCharacterId) left;
            var rightInstance = (MapCharacterId) right;
            Assert.AreEqual(leftInstance < rightInstance, left < right);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorLessOrEqualTest(int left, int right)
        {
            var leftInstance = (MapCharacterId) left;
            var rightInstance = (MapCharacterId) right;
            Assert.AreEqual(leftInstance <= rightInstance, left <= right);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorGreaterOrEqualTest(int left, int right)
        {
            var leftInstance = (MapCharacterId) left;
            var rightInstance = (MapCharacterId) right;
            Assert.AreEqual(leftInstance >= rightInstance, left >= right);
        }

        [TestCaseSource(nameof(CompareTestCaseSource))]
        public static void OperatorGreaterTest(int left, int right)
        {
            var leftInstance = (MapCharacterId) left;
            var rightInstance = (MapCharacterId) right;
            Assert.AreEqual(leftInstance > rightInstance, left > right);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (MapCharacterId) 3;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}