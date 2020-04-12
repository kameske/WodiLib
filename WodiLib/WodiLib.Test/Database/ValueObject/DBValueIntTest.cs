using System;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBValueIntTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(int.MinValue, false)]
        [TestCase(-1000000, false)]
        [TestCase(-999999, false)]
        [TestCase(1400000000, false)]
        [TestCase(int.MaxValue, false)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DBValueInt(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-999999)]
        [TestCase(1400000000)]
        public static void ToIntTest(int value)
        {
            var instance = new DBValueInt(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(int.MinValue, false)]
        [TestCase(-1000000, false)]
        [TestCase(-999999, false)]
        [TestCase(1400000000, false)]
        [TestCase(int.MaxValue, false)]
        public static void CastIntToDBValueIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (DBValueInt) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-999999)]
        [TestCase(1400000000)]
        public static void CastDBValueIntToIntTest(int value)
        {
            var castValue = 0;

            var instance = new DBValueInt(value);

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
            new object[] {-999999, -999999, true},
            new object[] {-999999, 1400000000, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DBValueInt) left;
            var rightIndex = (DBValueInt) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DBValueInt) left;
            var rightIndex = (DBValueInt) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DBValueInt) left;
            var rightIndex = (DBValueInt) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (DBValueInt) 3322;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}