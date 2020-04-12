using System;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBSettingTypePermutationTest
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
        [TestCase(99, false)]
        [TestCase(100, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DBSettingTypePermutation(value);
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
            var instance = new DBSettingTypePermutation(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99, false)]
        [TestCase(100, true)]
        public static void CastIntToDBSettingTypePermutationTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (DBSettingTypePermutation) value;
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
        public static void CastDBSettingTypePermutationToIntTest(int value)
        {
            var castValue = 0;

            var instance = new DBSettingTypePermutation(value);

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
            new object[] {0, 99, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DBSettingTypePermutation) left;
            var rightIndex = (DBSettingTypePermutation) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DBSettingTypePermutation) left;
            var rightIndex = (DBSettingTypePermutation) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DBSettingTypePermutation) left;
            var rightIndex = (DBSettingTypePermutation) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (DBSettingTypePermutation) 3;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}