using System;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventOpacityTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(0, false)]
        [TestCase(255, false)]
        public static void ConstructorTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MapEventOpacity(value);
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
        public static void ToIntTest(byte value)
        {
            var instance = new MapEventOpacity(value);

            var byteValue = instance.ToByte();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(byteValue, value);
        }


        [TestCase(0, false)]
        [TestCase(255, false)]
        public static void CastFromByteTest(byte value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MapEventOpacity) value;
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

            var instance = new MapEventOpacity(value);

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
            new object[] {0, 31, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapEventOpacity) left;
            var rightIndex = (MapEventOpacity) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapEventOpacity) left;
            var rightIndex = (MapEventOpacity) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (MapEventOpacity) left;
            var rightIndex = (MapEventOpacity) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}