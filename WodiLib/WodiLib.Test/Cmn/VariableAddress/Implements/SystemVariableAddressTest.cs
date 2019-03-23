using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class SystemVariableAddressTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(8999999, true)]
        [TestCase(9000000, false)]
        [TestCase(9099999, false)]
        [TestCase(9100000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new SystemVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9000000, 0)]
        [TestCase(9000031, 31)]
        [TestCase(9045678, 45678)]
        public static void VariableIndexTest(int variableAddress, int answer)
        {
            var instance = new SystemVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.VariableIndex, (SystemVariableIndex) answer);
        }

        [TestCase(9000000)]
        [TestCase(9099999)]
        public static void ToIntTest(int value)
        {
            var instance = new SystemVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(8999999, true)]
        [TestCase(9000000, false)]
        [TestCase(9099999, false)]
        [TestCase(9100000, true)]
        public static void CastIntToSystemVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (SystemVariableAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9000000)]
        [TestCase(9099999)]
        public static void CastSystemVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new SystemVariableAddress(value);

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

        [TestCase(9000000, -1, true)]
        [TestCase(9000000, 0, false)]
        [TestCase(9000000, 99999, false)]
        [TestCase(9000000, 100000, true)]
        [TestCase(9003213, -3214, true)]
        [TestCase(9003213, -3213, false)]
        [TestCase(9003213, 96786, false)]
        [TestCase(9003213, 96787, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new SystemVariableAddress(variableAddress);
            SystemVariableAddress result = null;

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

        [TestCase(9000000, -100000, true)]
        [TestCase(9000000, -99999, false)]
        [TestCase(9000000, 0, false)]
        [TestCase(9000000, 1, true)]
        [TestCase(9003213, -96787, true)]
        [TestCase(9003213, -96786, false)]
        [TestCase(9003213, 3213, false)]
        [TestCase(9003213, 3214, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new SystemVariableAddress(variableAddress);
            SystemVariableAddress result = null;

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

        [TestCase(9003213, 9000000)]
        [TestCase(9000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SystemVariableAddress(srcVariableAddress);
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

        [TestCase(9003213, 9000000)]
        [TestCase(9000000, 9099999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SystemVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (SystemVariableAddress) dstVariableAddress;
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