using System;
using Commons;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventSpecialArgCaseTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-2000000001, null, true)]
        [TestCase(-2000000000, null, true)]
        [TestCase(2000000000, null, true)]
        [TestCase(2000000001, null, true)]
        [TestCase(-2000000001, "", true)]
        [TestCase(-2000000000, "", false)]
        [TestCase(2000000000, "", false)]
        [TestCase(2000000001, "", true)]
        [TestCase(-2000000001, "abc", true)]
        [TestCase(-2000000000, "abc", false)]
        [TestCase(2000000000, "abc", false)]
        [TestCase(2000000001, "abc", true)]
        [TestCase(-2000000001, "あいうえお", true)]
        [TestCase(-2000000000, "あいうえお", false)]
        [TestCase(2000000000, "あいうえお", false)]
        [TestCase(2000000001, "あいうえお", true)]
        [TestCase(-2000000001, "Hello\r\nWorld.", true)]
        [TestCase(-2000000000, "Hello\r\nWorld.", true)]
        [TestCase(2000000000, "Hello\r\nWorld.", true)]
        [TestCase(2000000001, "Hello\r\nWorld.", true)]
        [TestCase(-2000000001, "Wolf\nRPG\nEditor!", true)]
        [TestCase(-2000000000, "Wolf\nRPG\nEditor!", true)]
        [TestCase(2000000000, "Wolf\nRPG\nEditor!", true)]
        [TestCase(2000000001, "Wolf\nRPG\nEditor!", true)]
        public static void ConstructorTest(int caseNumber, string description, bool isError)
        {
            var instance = default(CommonEventSpecialArgCase);
            var errorOccured = false;
            try
            {
                instance = new CommonEventSpecialArgCase
                {
                    CaseNumber = caseNumber,
                    Description = description,
                };
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // セットした値が正しく取得できること
            Assert.AreEqual((int)instance.CaseNumber, caseNumber);
            Assert.AreEqual((string)instance.Description, description);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] { 1, "a", 1, "a", true },
            new object[] { 1, "a", 1, "b", false },
            new object[] { 1, "a", 5, "a", false },
            new object[] { 1, "a", 5, "b", false }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(
            int leftCaseNumber,
            string leftDescription,
            int rightCaseNumber,
            string rightDescription,
            bool isEqual
        )
        {
            var leftInstance = new CommonEventSpecialArgCase
            {
                CaseNumber = leftCaseNumber,
                Description = leftDescription,
            };
            var rightInstance = new CommonEventSpecialArgCase
            {
                CaseNumber = rightCaseNumber,
                Description = rightDescription,
            };
            Assert.AreEqual(leftInstance == rightInstance, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(
            int leftCaseNumber,
            string leftDescription,
            int rightCaseNumber,
            string rightDescription,
            bool isEqual
        )
        {
            var leftInstance = new CommonEventSpecialArgCase
            {
                CaseNumber = leftCaseNumber,
                Description = leftDescription,
            };
            var rightInstance = new CommonEventSpecialArgCase
            {
                CaseNumber = rightCaseNumber,
                Description = rightDescription,
            };
            Assert.AreEqual(leftInstance == rightInstance, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(
            int leftCaseNumber,
            string leftDescription,
            int rightCaseNumber,
            string rightDescription,
            bool isEqual
        )
        {
            var leftInstance = new CommonEventSpecialArgCase
            {
                CaseNumber = leftCaseNumber,
                Description = leftDescription,
            };
            var rightInstance = new CommonEventSpecialArgCase
            {
                CaseNumber = rightCaseNumber,
                Description = rightDescription,
            };
            Assert.AreEqual(leftInstance.Equals(rightInstance), isEqual);
        }
    }
}
