using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class SystemStringVariableAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(9899999, true)]
        [TestCase(9900000, false)]
        [TestCase(9999999, false)]
        [TestCase(10000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new SystemStringVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9900000, 0)]
        [TestCase(9900031, 31)]
        [TestCase(9945678, 45678)]
        public static void VariableIndexTest(int variableAddress, int answer)
        {
            var instance = new SystemStringVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.VariableIndex, (SystemStringVariableIndex)answer);
        }

        [TestCase(9900000)]
        [TestCase(9999999)]
        public static void ToIntTest(int value)
        {
            var instance = new SystemStringVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(9899999, true)]
        [TestCase(9900000, false)]
        [TestCase(9999999, false)]
        [TestCase(10000000, true)]
        public static void CastIntToSystemStringVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (SystemStringVariableAddress)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9900000)]
        [TestCase(9999999)]
        public static void CastSystemStringVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new SystemStringVariableAddress(value);

            var errorOccured = false;
            try
            {
                castValue = (int)instance;
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

        [TestCase(9900000, -1, true)]
        [TestCase(9900000, 0, false)]
        [TestCase(9900000, 99999, false)]
        [TestCase(9900000, 100000, true)]
        [TestCase(9920132, -20133, true)]
        [TestCase(9920132, -20132, false)]
        [TestCase(9920132, 79867, false)]
        [TestCase(9920132, 79868, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new SystemStringVariableAddress(variableAddress);
            SystemStringVariableAddress result = null;

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
            Assert.AreEqual((int)result, variableAddress + value);

            // もとの値が変化していないこと
            Assert.AreEqual((int)instance, variableAddress);
        }

        [TestCase(9900000, -100000, true)]
        [TestCase(9900000, -99999, false)]
        [TestCase(9900000, 0, false)]
        [TestCase(9900000, 1, true)]
        [TestCase(9920132, -79868, true)]
        [TestCase(9920132, -79867, false)]
        [TestCase(9920132, 20132, false)]
        [TestCase(9920132, 20133, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new SystemStringVariableAddress(variableAddress);
            SystemStringVariableAddress result = null;

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
            Assert.AreEqual((int)result, variableAddress - value);

            // もとの値が変化していないこと
            Assert.AreEqual((int)instance, variableAddress);
        }

        [TestCase(9920132, 9900000)]
        [TestCase(9900000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SystemStringVariableAddress(srcVariableAddress);
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
            Assert.AreEqual((int)instance, srcVariableAddress);
        }

        [TestCase(9920132, 9900000)]
        [TestCase(9900000, 9999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SystemStringVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (SystemStringVariableAddress)dstVariableAddress;
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
            Assert.AreEqual((int)instance, srcVariableAddress);
        }
    }
}
