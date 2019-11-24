using System;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventNumberArgInitValueTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(int.MinValue, false)]
        [TestCase(-1400000000, false)]
        [TestCase(-1399999999, false)]
        [TestCase(1399999999, false)]
        [TestCase(1400000000, false)]
        [TestCase(int.MaxValue, false)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CommonEventNumberArgInitValue(value);
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
        [TestCase(-1400000000)]
        [TestCase(-1399999999)]
        [TestCase(1399999999)]
        [TestCase(1400000000)]
        [TestCase(int.MaxValue)]
        public static void ToIntTest(int value)
        {
            var instance = new CommonEventNumberArgInitValue(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(int.MinValue, false)]
        [TestCase(-1400000000, false)]
        [TestCase(-1399999999, false)]
        [TestCase(1399999999, false)]
        [TestCase(1400000000, false)]
        [TestCase(int.MaxValue, false)]
        public static void CastFromIntTest(int value, bool isError)
        {
            var instance = default(CommonEventNumberArgInitValue);
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
            Assert.AreEqual((int) instance, value);
        }

        [TestCase(int.MinValue)]
        [TestCase(-1400000000)]
        [TestCase(-1399999999)]
        [TestCase(1399999999)]
        [TestCase(1400000000)]
        [TestCase(int.MaxValue)]
        public static void CastToIntTest(int value)
        {
            var castValue = 0;

            var instance = new CommonEventNumberArgInitValue(value);

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
            new object[] {0, 0, true},
            new object[] {0, 3, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CommonEventNumberArgInitValue) left;
            var rightIndex = (CommonEventNumberArgInitValue) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CommonEventNumberArgInitValue) left;
            var rightIndex = (CommonEventNumberArgInitValue) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (CommonEventNumberArgInitValue) left;
            var rightIndex = (CommonEventNumberArgInitValue) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = (CommonEventNumberArgInitValue) 1234;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}