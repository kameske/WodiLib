using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventNameTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("Hello\r\nWorld!", true)]
        [TestCase("Wolf\nRPG\nEditor.", true)]
        public static void ConstructorTest(string value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CommonEventName(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase("")]
        [TestCase("abc")]
        [TestCase("あいうえお")]
        public static void ToStringTest(string value)
        {
            var instance = new CommonEventName(value);

            var strValue = instance.ToString();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(strValue, value);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("abc")]
        [TestCase("あいうえお")]
        public static void CastToStringTest(string value)
        {
            var castValue = "_DEFAULT_";
            var instance = value != null
                ? new CommonEventName(value)
                : null;

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

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("Hello\r\nWorld!", true)]
        [TestCase("Wolf\nRPG\nEditor.", true)]
        public static void CastFromStringTest(string value, bool isError)
        {
            CommonEventName instance = null;

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
            Assert.AreEqual((string) instance, value);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {"a", "a", true},
            new object[] {"a", "b", false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(string left, string right, bool isEqual)
        {
            var leftIndex = (CommonEventName) left;
            var rightIndex = (CommonEventName) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTestA(string left, string right, bool isEqual)
        {
            var leftIndex = (CommonEventName) left;
            var rightIndex = (CommonEventName) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTestA(string left, string right, bool isEqual)
        {
            var leftIndex = (CommonEventName) left;
            var rightIndex = (CommonEventName) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTestB(string left, string right, bool isEqual)
        {
            var leftIndex = (CommonEventName) left;
            Assert.AreEqual(leftIndex.Equals(right), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (CommonEventName) "CommonEventName";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}