using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common.Internal
{
    [TestFixture]
    public class CommonEventSpecialArgCaseTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(-1, null, true)]
        [TestCase(-1, "", false)]
        [TestCase(-1, "abc", false)]
        [TestCase(-1, "あいうえお", false)]
        [TestCase(-1, "New\r\nLine\r\nCRLF", true)]
        [TestCase(-1, "New\nLine\nLF", true)]
        [TestCase(0, null, true)]
        [TestCase(0, "", false)]
        [TestCase(0, "abc", false)]
        [TestCase(0, "あいうえお", false)]
        [TestCase(0, "New\r\nLine\r\nCRLF", true)]
        [TestCase(0, "New\nLine\nLF", true)]
        [TestCase(100, null, true)]
        [TestCase(100, "", false)]
        [TestCase(100, "abc", false)]
        [TestCase(100, "あいうえお", false)]
        [TestCase(100, "New\r\nLine\r\nCRLF", true)]
        [TestCase(100, "New\nLine\nLF", true)]
        public static void ConstructorTest(int caseNumber, string description, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CommonEventSpecialArgCase(caseNumber, description);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0, "a", 0, "a", true)]
        [TestCase(0, "a", 1, "a", false)]
        [TestCase(0, "a", 0, "b", false)]
        [TestCase(0, "a", -1, "d", false)]
        public static void EqualsTest(int leftCaseNumber, string leftDescription,
            int rightCaseNumber, string rightDescription, bool isEqual)
        {
            var left = new CommonEventSpecialArgCase(leftCaseNumber, leftDescription);
            var right = new CommonEventSpecialArgCase(rightCaseNumber, rightDescription);

            Assert.AreEqual(left.Equals(right), isEqual);
        }

        [TestCase(0, "a", 0, "a", true)]
        [TestCase(0, "a", 1, "a", false)]
        [TestCase(0, "a", 0, "b", false)]
        [TestCase(0, "a", -1, "d", false)]
        public static void EqualOperatorTest(int leftCaseNumber, string leftDescription,
            int rightCaseNumber, string rightDescription, bool isEqual)
        {
            var left = new CommonEventSpecialArgCase(leftCaseNumber, leftDescription);
            var right = new CommonEventSpecialArgCase(rightCaseNumber, rightDescription);

            Assert.AreEqual(left == right, isEqual);
        }

        [TestCase(0, "a", 0, "a", false)]
        [TestCase(0, "a", 1, "a", true)]
        [TestCase(0, "a", 0, "b", true)]
        [TestCase(0, "a", -1, "d", true)]
        public static void NotEqualOperatorTest(int leftCaseNumber, string leftDescription,
            int rightCaseNumber, string rightDescription, bool isEqual)
        {
            var left = new CommonEventSpecialArgCase(leftCaseNumber, leftDescription);
            var right = new CommonEventSpecialArgCase(rightCaseNumber, rightDescription);

            Assert.AreEqual(left != right, isEqual);
        }
    }
}