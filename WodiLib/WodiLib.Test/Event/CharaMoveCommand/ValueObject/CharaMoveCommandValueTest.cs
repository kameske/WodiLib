using System;
using NUnit.Framework;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.CharaMoveCommand
{
    [TestFixture]
    public class CharaMoveCommandValueTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(-2000000001, true)]
        [TestCase(-2000000000, false)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(9999999, false)]
        [TestCase(10000000, false)]
        [TestCase(2000000000, false)]
        [TestCase(2000000001, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CharaMoveCommandValue(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-2000000000)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(9999999)]
        [TestCase(10000000)]
        [TestCase(2000000000)]
        public static void ToIntTest(int value)
        {
            var instance = new CharaMoveCommandValue(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(-2000000001, true)]
        [TestCase(-2000000000, false)]
        [TestCase(-1, false)]
        [TestCase(0, false)]
        [TestCase(9999999, false)]
        [TestCase(10000000, false)]
        [TestCase(2000000000, false)]
        [TestCase(2000000001, true)]
        public static void CastIntToCharaMoveCommandValueTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (CharaMoveCommandValue)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-2000000000)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(9999999)]
        [TestCase(10000000)]
        [TestCase(2000000000)]
        public static void CastCharaMoveCommandValueToIntTest(int value)
        {
            var castValue = 0;

            var instance = new CharaMoveCommandValue(value);

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
            new object[] {10, 10, true},
            new object[] {10, 11, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CharaMoveCommandValue) left;
            var rightIndex = (CharaMoveCommandValue) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CharaMoveCommandValue) left;
            var rightIndex = (CharaMoveCommandValue) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CharaMoveCommandValue) left;
            var rightIndex = (CharaMoveCommandValue) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}