using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class EventInfoAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(9099999, true)]
        [TestCase(9100000, false)]
        [TestCase(9179999, false)]
        [TestCase(9180000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new EventInfoAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] InfoTypeTestCaseSource =
        {
            new object[] {9100000, InfoAddressInfoType.PositionX},
            new object[] {9100201, InfoAddressInfoType.PositionY},
            new object[] {9100022, InfoAddressInfoType.PositionXPrecise},
            new object[] {9100003, InfoAddressInfoType.PositionYPrecise},
            new object[] {9102024, InfoAddressInfoType.Height},
            new object[] {9100005, InfoAddressInfoType.ShadowGraphicId},
            new object[] {9103006, InfoAddressInfoType.Direction},
            new object[] {9100019, InfoAddressInfoType.CharacterGraphicName},
        };

        [TestCaseSource(nameof(InfoTypeTestCaseSource))]
        public static void InfoTypeTest(int variableAddress, InfoAddressInfoType infoType)
        {
            var instance = new EventInfoAddress(variableAddress);

            // プロパティの値が意図した値と一致すること
            Assert.AreEqual(instance.InfoType, infoType);
        }

        [TestCase(9100000)]
        [TestCase(9179999)]
        public static void ToIntTest(int value)
        {
            var instance = new EventInfoAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(9099999, true)]
        [TestCase(9100000, false)]
        [TestCase(9179999, false)]
        [TestCase(9180000, true)]
        public static void CastIntToEventPositionAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (EventInfoAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9100000)]
        [TestCase(9179999)]
        public static void CastEventPositionAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new EventInfoAddress(value);

            var errorOccured = false;
            try
            {
                castValue = (int) instance;
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

        [TestCase(9100000, -1, true)]
        [TestCase(9100000, 0, false)]
        [TestCase(9100000, 79999, false)]
        [TestCase(9100000, 80000, true)]
        [TestCase(9125546, -25547, true)]
        [TestCase(9125546, -25546, false)]
        [TestCase(9125546, 54453, false)]
        [TestCase(9125546, 54454, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new EventInfoAddress(variableAddress);
            EventInfoAddress result = null;

            var errorOccured = false;
            try
            {
                result = instance + value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 意図した値と一致すること
            Assert.AreEqual((int) result, variableAddress + value);

            // もとの値が変化していないこと
            Assert.AreEqual((int) instance, variableAddress);
        }

        [TestCase(9100000, -80000, true)]
        [TestCase(9100000, -79999, false)]
        [TestCase(9100000, 0, false)]
        [TestCase(9100000, 1, true)]
        [TestCase(9125546, -54454, true)]
        [TestCase(9125546, -54453, false)]
        [TestCase(9125546, 25546, false)]
        [TestCase(9125546, 25547, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new EventInfoAddress(variableAddress);
            EventInfoAddress result = null;

            var errorOccured = false;
            try
            {
                result = instance - value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 意図した値と一致すること
            Assert.AreEqual((int) result, variableAddress - value);

            // もとの値が変化していないこと
            Assert.AreEqual((int) instance, variableAddress);
        }

        [TestCase(9125546, 9100000)]
        [TestCase(9100000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new EventInfoAddress(srcVariableAddress);
            var dstInstance = VariableAddressFactory.Create(dstVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - dstInstance;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図した値と一致すること
            Assert.AreEqual(result, srcVariableAddress - dstVariableAddress);

            // もとの値が変化していないこと
            Assert.AreEqual((int) instance, srcVariableAddress);
        }

        [TestCase(9125546, 9100000)]
        [TestCase(9100000, 9179999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new EventInfoAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (EventInfoAddress) dstVariableAddress;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図した値と一致すること
            Assert.AreEqual(result, srcVariableAddress - dstVariableAddress);

            // もとの値が変化していないこと
            Assert.AreEqual((int) instance, srcVariableAddress);
        }
    }
}
