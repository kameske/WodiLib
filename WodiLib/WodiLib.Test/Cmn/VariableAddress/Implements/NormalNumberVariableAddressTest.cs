using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class NormalNumberVariableAddressTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(1999999, true)]
        [TestCase(2000000, false)]
        [TestCase(2099999, false)]
        [TestCase(2100000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new NormalNumberVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(2000000, 0)]
        [TestCase(2000031, 31)]
        [TestCase(2045678, 45678)]
        public static void VariableIndexTest(int variableAddress, int answer)
        {
            var instance = new NormalNumberVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.VariableIndex, (NormalNumberVariableIndex)answer);
        }

        [TestCase(2000000)]
        [TestCase(2099999)]
        public static void ToIntTest(int value)
        {
            var instance = new NormalNumberVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(1999999, true)]
        [TestCase(2000000, false)]
        [TestCase(2099999, false)]
        [TestCase(2100000, true)]
        public static void CastIntToNormalNumberVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (NormalNumberVariableAddress)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(2000000)]
        [TestCase(2099999)]
        public static void CastNormalNumberVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new NormalNumberVariableAddress(value);

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

        [TestCase(2000000, -1, true)]
        [TestCase(2000000, 0, false)]
        [TestCase(2000000, 99999, false)]
        [TestCase(2000000, 100000, true)]
        [TestCase(2053002, -53003, true)]
        [TestCase(2053002, -53002, false)]
        [TestCase(2053002, 46997, false)]
        [TestCase(2053002, 46998, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new NormalNumberVariableAddress(variableAddress);
            NormalNumberVariableAddress result = null;

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

        [TestCase(2000000, -100000, true)]
        [TestCase(2000000, -99999, false)]
        [TestCase(2000000, 0, false)]
        [TestCase(2000000, 1, true)]
        [TestCase(2053002, -46998, true)]
        [TestCase(2053002, -46997, false)]
        [TestCase(2053002, 53002, false)]
        [TestCase(2053002, 53003, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new NormalNumberVariableAddress(variableAddress);
            NormalNumberVariableAddress result = null;

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

        [TestCase(2053002, 2000000)]
        [TestCase(2000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new NormalNumberVariableAddress(srcVariableAddress);
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

        [TestCase(2053002, 2000000)]
        [TestCase(2000000, 2099999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new NormalNumberVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (NormalNumberVariableAddress)dstVariableAddress;
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
