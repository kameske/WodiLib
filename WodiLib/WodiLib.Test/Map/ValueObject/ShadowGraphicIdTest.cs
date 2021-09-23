using System;
using Commons;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class ShadowGraphicIdTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(0, false)]
        [TestCase(255, false)]
        public static void ConstructorTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ShadowGraphicId(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0, false)]
        [TestCase(255, false)]
        public static void CastFromByteTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (ShadowGraphicId)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(0)]
        [TestCase(255)]
        public static void CastToByteTest(byte value)
        {
            var castValue = 0;

            var instance = new ShadowGraphicId(value);

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
            new object[] { 0, 0, true },
            new object[] { 0, 31, false }
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ShadowGraphicId)left;
            var rightIndex = (ShadowGraphicId)right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ShadowGraphicId)left;
            var rightIndex = (ShadowGraphicId)right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ShadowGraphicId)left;
            var rightIndex = (ShadowGraphicId)right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
