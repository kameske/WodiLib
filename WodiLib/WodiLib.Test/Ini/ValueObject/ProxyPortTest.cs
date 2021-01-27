using System;
using Commons;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.ValueObject
{
    [TestFixture]
    public class ProxyPortTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(-2, true)]
        [TestCase(-1, false)]
        [TestCase(65535, false)]
        [TestCase(65536, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ProxyPort(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1)]
        [TestCase(65535)]
        public static void ToIntTest(int value)
        {
            var instance = new ProxyPort(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(-2, true)]
        [TestCase(-1, false)]
        [TestCase(65535, false)]
        [TestCase(65536, true)]
        public static void CastIntToProxyPortTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (ProxyPort) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1)]
        [TestCase(65535)]
        public static void CastProxyPortToIntTest(int value)
        {
            var castValue = 0;

            var instance = new ProxyPort(value);

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
            new object[] {-1, -1, true},
            new object[] {-1, 65535, false},
        };

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ProxyPort) left;
            var rightIndex = (ProxyPort) right;
            Assert.AreEqual(leftIndex == rightIndex, isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorNotEqualTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ProxyPort) left;
            var rightIndex = (ProxyPort) right;
            Assert.AreEqual(leftIndex != rightIndex, !isEqual);
        }

        [TestCaseSource(nameof(EqualTestCaseSource))]
        public static void OperatorEqualsTest(int left, int right, bool isEqual)
        {
            var leftIndex = (ProxyPort) left;
            var rightIndex = (ProxyPort) right;
            Assert.AreEqual(leftIndex.Equals(rightIndex), isEqual);
        }
    }
}
