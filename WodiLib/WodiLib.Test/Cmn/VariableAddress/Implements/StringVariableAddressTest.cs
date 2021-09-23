using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class StringVariableAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(2999999, true)]
        [TestCase(3000000, false)]
        [TestCase(3999999, false)]
        [TestCase(4000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new StringVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(3000000, 0)]
        [TestCase(3000031, 31)]
        [TestCase(3045678, 45678)]
        public static void VariableIndexTest(int variableAddress, int answer)
        {
            var instance = new StringVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.VariableIndex, (StringVariableIndex)answer);
        }

        [TestCase(3000000)]
        [TestCase(3999999)]
        public static void ToIntTest(int value)
        {
            var instance = new StringVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(2999999, true)]
        [TestCase(3000000, false)]
        [TestCase(3999999, false)]
        [TestCase(4000000, true)]
        public static void CastIntToStringVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (StringVariableAddress)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(3000000)]
        [TestCase(3999999)]
        public static void CastStringVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new StringVariableAddress(value);

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

        [TestCase(3000000, -1, true)]
        [TestCase(3000000, 0, false)]
        [TestCase(3000000, 999999, false)]
        [TestCase(3000000, 1000000, true)]
        [TestCase(3021551, -21552, true)]
        [TestCase(3021551, -21551, false)]
        [TestCase(3021551, 978448, false)]
        [TestCase(3021551, 978449, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new StringVariableAddress(variableAddress);
            StringVariableAddress result = null;

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

        [TestCase(3000000, -1000000, true)]
        [TestCase(3000000, -999999, false)]
        [TestCase(3000000, 0, false)]
        [TestCase(3000000, 1, true)]
        [TestCase(3021551, -978449, true)]
        [TestCase(3021551, -978448, false)]
        [TestCase(3021551, 21551, false)]
        [TestCase(3021551, 21552, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new StringVariableAddress(variableAddress);
            StringVariableAddress result = null;

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

        [TestCase(3021551, 3000000)]
        [TestCase(3000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new StringVariableAddress(srcVariableAddress);
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

        [TestCase(3021551, 3000000)]
        [TestCase(3000000, 3999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new StringVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (StringVariableAddress)dstVariableAddress;
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
