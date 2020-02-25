using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database
{
    [TestFixture]
    public class DBValueStringTest
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
        [TestCase("Hello\r\nWorld!", false)]
        [TestCase("Wolf\nRPG\nEditor.", false)]
        public static void ConstructorTest(string value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new DBValueString(value);
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
            var instance = new DBValueString(value);

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
            var instance = value != null
                ? new DBValueString(value)
                : null;

            var errorOccured = false;
            try
            {
                var _ = (string) instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが発生しないこと
            Assert.IsFalse(errorOccured);

            // キャストした結果が一致すること
            Assert.AreEqual((string) instance, value);
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("Hello\r\nWorld!", false)]
        [TestCase("Wolf\nRPG\nEditor.", false)]
        public static void CastFromStringTest(string value, bool isError)
        {
            DBValueString instance = null;

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
            var leftIndex = (DBValueString) left;
            var rightIndex = (DBValueString) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(string left, string right, bool isEqual)
        {
            var leftIndex = (DBValueString) left;
            var rightIndex = (DBValueString) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(string left, string right, bool isEqual)
        {
            var leftIndex = (DBValueString) left;
            var rightIndex = (DBValueString) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (DBValueString) "StringItem";
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}