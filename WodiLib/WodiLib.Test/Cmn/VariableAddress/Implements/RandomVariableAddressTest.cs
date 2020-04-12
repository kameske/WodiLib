using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class RandomVariableAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(7999999, true)]
        [TestCase(8000000, false)]
        [TestCase(8999999, false)]
        [TestCase(9000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new RandomVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(8000000, 0)]
        [TestCase(8000442, 442)]
        [TestCase(8987654, 987654)]
        public static void RandomValueTest(int variableAddress, int answer)
        {
            var instance = new RandomVariableAddress(variableAddress);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(instance.RandomValue, (RandomVariableValue) answer);
        }

        [TestCase(8000000)]
        [TestCase(8999999)]
        public static void ToIntTest(int value)
        {
            var instance = new RandomVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(7999999, true)]
        [TestCase(8000000, false)]
        [TestCase(8999999, false)]
        [TestCase(9000000, true)]
        public static void CastIntToRandomVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (RandomVariableAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(8000000)]
        [TestCase(8999999)]
        public static void CastRandomVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new RandomVariableAddress(value);

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

        [TestCase(8000000, -1, true)]
        [TestCase(8000000, 0, false)]
        [TestCase(8000000, 999999, false)]
        [TestCase(8000000, 1000000, true)]
        [TestCase(8320005, -320006, true)]
        [TestCase(8320005, -320005, false)]
        [TestCase(8320005, 679994, false)]
        [TestCase(8320005, 679995, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new RandomVariableAddress(variableAddress);
            RandomVariableAddress result = null;

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

        [TestCase(8000000, -1000000, true)]
        [TestCase(8000000, -999999, false)]
        [TestCase(8000000, 0, false)]
        [TestCase(8000000, 1, true)]
        [TestCase(8320005, -679995, true)]
        [TestCase(8320005, -679994, false)]
        [TestCase(8320005, 320005, false)]
        [TestCase(8320005, 320006, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new RandomVariableAddress(variableAddress);
            RandomVariableAddress result = null;

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

        [TestCase(8320005, 8000000)]
        [TestCase(8000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new RandomVariableAddress(srcVariableAddress);
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

        [TestCase(8320005, 8000000)]
        [TestCase(8000000, 8999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new RandomVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (RandomVariableAddress) dstVariableAddress;
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
            var target = (RandomVariableAddress) 8320005;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}