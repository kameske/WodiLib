using System;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class TypeIdTest
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
        public static void ConstructorTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new TypeId(value);
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
            var instance = new TypeId(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(99, false)]
        [TestCase(100, true)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var instance = default(TypeId);
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
            Assert.AreEqual((int) instance, value);
        }

        [TestCase(0)]
        [TestCase(99)]
        public static void CastIntTest(int value)
        {
            var instance = new TypeId(value);

            var castedValue = 0;

            var errorOccured = false;
            try
            {
                castedValue = instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // キャストした結果が一致すること
            Assert.AreEqual(castedValue, value);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {0, 0, true},
            new object[] {0, 3, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftInstance = (TypeId) left;
            var rightInstance = (TypeId) right;
            Assert.AreEqual(leftInstance == rightInstance, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftInstance = (TypeId) left;
            var rightInstance = (TypeId) right;
            Assert.AreEqual(leftInstance != rightInstance, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftInstance = (TypeId) left;
            var rightInstance = (TypeId) right;
            Assert.AreEqual(leftInstance.Equals(rightInstance), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (TypeId) 3;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}