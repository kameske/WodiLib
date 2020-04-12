using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Common;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class CommonEventVariableAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(14999999, true)]
        [TestCase(15000000, false)]
        [TestCase(15999999, false)]
        [TestCase(16000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new CommonEventVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(15000020, 0)]
        [TestCase(15004103, 41)]
        [TestCase(15012899, 128)]
        public static void GetCommonEventId(int variableAddress, int answer)
        {
            var test = (CommonEventVariableAddress) variableAddress;
            var commonEventId = (CommonEventId) answer;
            Assert.AreEqual(test.CommonEventId, commonEventId);
        }

        [TestCase(15000000)]
        [TestCase(15999999)]
        public static void ToIntTest(int value)
        {
            var instance = new CommonEventVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(10999999, true)]
        [TestCase(15000000, false)]
        [TestCase(15999999, false)]
        [TestCase(16000000, true)]
        public static void CastIntToCommonEventVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (CommonEventVariableAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(15000000)]
        [TestCase(15999999)]
        public static void CastCommonEventVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new CommonEventVariableAddress(value);

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

        [TestCase(15000000, -1, true)]
        [TestCase(15000000, 0, false)]
        [TestCase(15000000, 999999, false)]
        [TestCase(15000000, 1000000, true)]
        [TestCase(15004500, -4501, true)]
        [TestCase(15004500, -4500, false)]
        [TestCase(15004500, 995499, false)]
        [TestCase(15004500, 995500, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new CommonEventVariableAddress(variableAddress);
            CommonEventVariableAddress result = null;

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

        [TestCase(15000000, -1000000, true)]
        [TestCase(15000000, -999999, false)]
        [TestCase(15000000, 0, false)]
        [TestCase(15000000, 1, true)]
        [TestCase(15004500, -995500, true)]
        [TestCase(15004500, -995499, false)]
        [TestCase(15004500, 4500, false)]
        [TestCase(15004500, 4501, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new CommonEventVariableAddress(variableAddress);
            CommonEventVariableAddress result = null;

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

        [TestCase(15004500, 15000000)]
        [TestCase(15000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new CommonEventVariableAddress(srcVariableAddress);
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

        [TestCase(15004500, 15000000)]
        [TestCase(15000000, 15000000)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new CommonEventVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (CommonEventVariableAddress) dstVariableAddress;
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
            var target = (CommonEventVariableAddress) 15004500;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}