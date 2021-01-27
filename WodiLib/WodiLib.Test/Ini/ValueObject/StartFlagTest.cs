using System;
using Commons;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.ValueObject
{
    [TestFixture]
    public class StartFlagTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(int.MinValue, false)]
        [TestCase(int.MaxValue, false)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new StartFlag(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public static void ToIntTest(int value)
        {
            var instance = new StartFlag(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(int.MinValue, false)]
        [TestCase(int.MaxValue, false)]
        public static void CastIntToStartFlagTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (StartFlag) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public static void CastStartFlagToIntTest(int value)
        {
            var castValue = 0;

            var instance = new StartFlag(value);

            var errorOccured = false;
            try
            {
                castValue =  instance;
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
            new object[] {int.MinValue, int.MinValue, true},
            new object[] {int.MinValue, int.MaxValue, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (StartFlag) left;
            var rightIndex = (StartFlag) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (StartFlag) left;
            var rightIndex = (StartFlag) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (StartFlag) left;
            var rightIndex = (StartFlag) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
