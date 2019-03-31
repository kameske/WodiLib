using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class AnimationIdTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void ConstructorTest(byte value, bool isError)
        {

            var errorOccured = false;
            try
            {
                var _ = new AnimationId(value);
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
        [TestCase(4)]
        public static void ToIntTest(byte value)
        {
            var instance = new AnimationId(value);

            var byteValue = instance.ToByte();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(byteValue, value);
        }

        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void CastFromByteTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (AnimationId) value;
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
        [TestCase(4)]
        public static void CastToByteTest(byte value)
        {
            var castValue = 0;

            var instance = new AnimationId(value);

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
            new object[] {0, 0, true},
            new object[] {0, 3, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (AnimationId) left;
            var rightIndex = (AnimationId) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (AnimationId) left;
            var rightIndex = (AnimationId) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (AnimationId) left;
            var rightIndex = (AnimationId) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}