using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class SpareNumberVariableAddressTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(2099999, true)]
        [TestCase(2100000, false)]
        [TestCase(2999999, false)]
        [TestCase(3000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new SpareNumberVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(2100000, 1)]
        [TestCase(2500031, 5)]
        [TestCase(2999999, 9)]
        public static void VariableNumberTest(int variableAddress, int answer)
        {
            var instance = new SpareNumberVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.VariableNumber, (SpareNumberVariableNumber) answer);
        }

        [TestCase(2100000, 0)]
        [TestCase(2500031, 31)]
        [TestCase(2999999, 99999)]
        public static void VariableIndexTest(int variableAddress, int answer)
        {
            var instance = new SpareNumberVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.VariableIndex, (SpareNumberVariableIndex) answer);
        }

        [TestCase(2100000)]
        [TestCase(2999999)]
        public static void ToIntTest(int value)
        {
            var instance = new SpareNumberVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(2099999, true)]
        [TestCase(2100000, false)]
        [TestCase(2999999, false)]
        [TestCase(3000000, true)]
        public static void CastIntToSpareNumberVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (SpareNumberVariableAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(2100000)]
        [TestCase(2999999)]
        public static void CastSpareNumberVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new SpareNumberVariableAddress(value);

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

        [TestCase(2100000, -1, true)]
        [TestCase(2100000, 0, false)]
        [TestCase(2100000, 899999, false)]
        [TestCase(2100000, 900000, true)]
        [TestCase(2300212, -200213, true)]
        [TestCase(2300212, -200212, false)]
        [TestCase(2300212, 699787, false)]
        [TestCase(2300212, 699788, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new SpareNumberVariableAddress(variableAddress);
            SpareNumberVariableAddress result = null;

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

        [TestCase(2100000, -900000, true)]
        [TestCase(2100000, -899999, false)]
        [TestCase(2100000, 0, false)]
        [TestCase(2100000, 1, true)]
        [TestCase(2300212, -699788, true)]
        [TestCase(2300212, -699787, false)]
        [TestCase(2300212, 200212, false)]
        [TestCase(2300212, 200213, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new SpareNumberVariableAddress(variableAddress);
            SpareNumberVariableAddress result = null;

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

        [TestCase(2300212, 2100000)]
        [TestCase(2100000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SpareNumberVariableAddress(srcVariableAddress);
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

        [TestCase(2300212, 2100000)]
        [TestCase(2100000, 2999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SpareNumberVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (SpareNumberVariableAddress) dstVariableAddress;
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

        [Test]
        public static void SerializeTest()
        {
            var target = (SpareNumberVariableAddress) 2999999;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}