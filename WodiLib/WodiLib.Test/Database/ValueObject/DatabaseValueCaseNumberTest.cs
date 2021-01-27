using System;
using Commons;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DatabaseValueCaseNumberTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-10000000, true)]
        [TestCase(-9999999, false)]
        [TestCase(1400000000, false)]
        [TestCase(1400000001, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DatabaseValueCaseNumber(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-9999999)]
        [TestCase(1400000000)]
        public static void ToIntTest(int value)
        {
            var instance = new DatabaseValueCaseNumber(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(-10000000, true)]
        [TestCase(-9999999, false)]
        [TestCase(1400000000, false)]
        [TestCase(1400000001, true)]
        public static void CastIntToDatabaseValueCaseNumberTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (DatabaseValueCaseNumber) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-9999999)]
        [TestCase(1400000000)]
        public static void CastDatabaseValueCaseNumberToIntTest(int value)
        {
            var castValue = 0;

            var instance = new DatabaseValueCaseNumber(value);

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
            new object[] {-9999999, -9999999, true},
            new object[] {-9999999, 1400000000, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DatabaseValueCaseNumber) left;
            var rightIndex = (DatabaseValueCaseNumber) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DatabaseValueCaseNumber) left;
            var rightIndex = (DatabaseValueCaseNumber) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (DatabaseValueCaseNumber) left;
            var rightIndex = (DatabaseValueCaseNumber) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
