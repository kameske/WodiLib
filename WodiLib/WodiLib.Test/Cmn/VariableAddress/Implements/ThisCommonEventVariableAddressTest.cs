using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class ThisCommonEventVariableAddressTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(1599999, true)]
        [TestCase(1600000, false)]
        [TestCase(1600099, false)]
        [TestCase(1600100, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ThisCommonEventVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1600000)]
        [TestCase(1600099)]
        public static void ToIntTest(int value)
        {
            var instance = new ThisCommonEventVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(1599999, true)]
        [TestCase(1600000, false)]
        [TestCase(1600099, false)]
        [TestCase(1600100, true)]
        public static void CastIntToThisCommonEventVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (ThisCommonEventVariableAddress)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1600000)]
        [TestCase(1600099)]
        public static void CastThisCommonEventVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new ThisCommonEventVariableAddress(value);

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

        [TestCase(1600000, -1, true)]
        [TestCase(1600000, 0, false)]
        [TestCase(1600000, 99, false)]
        [TestCase(1600000, 100, true)]
        [TestCase(1600002, -3, true)]
        [TestCase(1600002, -2, false)]
        [TestCase(1600002, 97, false)]
        [TestCase(1600002, 98, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new ThisCommonEventVariableAddress(variableAddress);
            ThisCommonEventVariableAddress result = null;

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

        [TestCase(1600000, -100, true)]
        [TestCase(1600000, -99, false)]
        [TestCase(1600000, 0, false)]
        [TestCase(1600000, 1, true)]
        [TestCase(1600002, -98, true)]
        [TestCase(1600002, -97, false)]
        [TestCase(1600002, 2, false)]
        [TestCase(1600002, 3, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new ThisCommonEventVariableAddress(variableAddress);
            ThisCommonEventVariableAddress result = null;

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

        [TestCase(1600002, 1600000)]
        [TestCase(1600000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new ThisCommonEventVariableAddress(srcVariableAddress);
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

        [TestCase(1600002, 1600000)]
        [TestCase(1600000, 1600099)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new ThisCommonEventVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (ThisCommonEventVariableAddress)dstVariableAddress;
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
