using System;
using Commons;
using NUnit.Framework;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.CharaMoveCommand
{
    [TestFixture]
    public class CalledEventVariableAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(1099999, true)]
        [TestCase(1100000, false)]
        [TestCase(1100009, false)]
        [TestCase(1100010, true)]
        [TestCase(1599999, true)]
        [TestCase(1600000, false)]
        [TestCase(1600009, false)]
        [TestCase(1600010, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CalledEventVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1100009)]
        public static void ToIntTest(int value)
        {
            var instance = new CalledEventVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(1099999, true)]
        [TestCase(1100000, false)]
        [TestCase(1100009, false)]
        [TestCase(1100010, true)]
        [TestCase(1599999, true)]
        [TestCase(1600000, false)]
        [TestCase(1600009, false)]
        [TestCase(1600010, true)]
        public static void CastIntToCalledEventVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (CalledEventVariableAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1100009)]
        public static void CastCalledEventVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new CalledEventVariableAddress(value);

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
            new object[] {1100000, 1100000, true},
            new object[] {1100000, 1600003, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CalledEventVariableAddress) left;
            var rightIndex = (CalledEventVariableAddress) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CalledEventVariableAddress) left;
            var rightIndex = (CalledEventVariableAddress) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CalledEventVariableAddress) left;
            var rightIndex = (CalledEventVariableAddress) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
