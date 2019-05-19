using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
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
            CommonEventSpecialArgCase instance = default(CommonEventSpecialArgCase);
            var errorOccured = false;
            try
            {
                instance = new CommonEventSpecialArgCase(caseNumber, description);
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
            Assert.AreEqual(instance.CaseNumber, caseNumber);
            Assert.AreEqual(instance.Description, description);
        }

        [TestCase(0, "")]
        [TestCase(-2, "abc")]
        [TestCase(3, "あいうえお")]
        public static void CastToTupleTest(int caseNumber, string description)
        {
            Tuple<int, string> castValue = null;
            var instance = new CommonEventSpecialArgCase(caseNumber, description);

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
            Assert.AreEqual(castValue.Item1, caseNumber);
            Assert.AreEqual(castValue.Item2, description);
        }

        [TestCase(0, null, true)]
        [TestCase(-1, "", false)]
        [TestCase(20, "abc", false)]
        [TestCase(120, "あいうえお", false)]
        [TestCase(3, "Hello\r\nWorld!", true)]
        [TestCase(-350, "Wolf\nRPG\nEditor.", true)]
        public static void CastFromTupleTest(int caseNumber, string description, bool isError)
        {
            var instance = default(CommonEventSpecialArgCase);
            var src = new Tuple<int, string>(caseNumber, description);

            var errorOccured = false;
            try
            {
                instance = src;
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
            Assert.AreEqual(instance.CaseNumber, caseNumber);
            Assert.AreEqual(instance.Description, description);
        }

        [TestCase(0, "")]
        [TestCase(-2, "abc")]
        [TestCase(3, "あいうえお")]
        public static void CastToValueTupleTest(int caseNumber, string description)
        {
            var castValue = default((int, string));
            var instance = new CommonEventSpecialArgCase(caseNumber, description);

            var errorOccured = false;
            try
            {
                castValue = ((int, string)) instance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 元の値と一致すること
            Assert.AreEqual(castValue.Item1, caseNumber);
            Assert.AreEqual(castValue.Item2, description);
        }

        [TestCase(0, null, true)]
        [TestCase(-1, "", false)]
        [TestCase(20, "abc", false)]
        [TestCase(120, "あいうえお", false)]
        [TestCase(3, "Hello\r\nWorld!", true)]
        [TestCase(-350, "Wolf\nRPG\nEditor.", true)]
        public static void CastFromValueTupleTest(int caseNumber, string description, bool isError)
        {
            var instance = default(CommonEventSpecialArgCase);
            var src = (caseNumber, description);

            var errorOccured = false;
            try
            {
                instance = src;
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
            Assert.AreEqual(instance.CaseNumber, caseNumber);
            Assert.AreEqual(instance.Description, description);
        }

        private static readonly object[] EqualTestCaseSource =
        {
            new object[] {1, "a", 1, "a", true},
            new object[] {1, "a", 1, "b", false},
            new object[] {1, "a", 5, "a", false},
            new object[] {1, "a", 5, "b", false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int leftCaseNumber, string leftDescription,
            int rightCaseNumber, string rightDescription, bool isEqual)
        {
            var leftInstance = (CommonEventSpecialArgCase) (leftCaseNumber, leftDescription);
            var rightInstance = (CommonEventSpecialArgCase) (rightCaseNumber, rightDescription);
            Assert.AreEqual(leftInstance == rightInstance, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int leftCaseNumber, string leftDescription,
            int rightCaseNumber, string rightDescription, bool isEqual)
        {
            var leftInstance = (CommonEventSpecialArgCase) (leftCaseNumber, leftDescription);
            var rightInstance = (CommonEventSpecialArgCase) (rightCaseNumber, rightDescription);
            Assert.AreEqual(leftInstance == rightInstance, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int leftCaseNumber, string leftDescription,
            int rightCaseNumber, string rightDescription, bool isEqual)
        {
            var leftInstance = (CommonEventSpecialArgCase) (leftCaseNumber, leftDescription);
            var rightInstance = (CommonEventSpecialArgCase) (rightCaseNumber, rightDescription);
            Assert.AreEqual(leftInstance.Equals(rightInstance), isEqual);
        }
    }
}