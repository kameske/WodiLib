using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class MapEventVariableAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(999999, true)]
        [TestCase(1000000, false)]
        [TestCase(1099999, false)]
        [TestCase(1100000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MapEventVariableAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
        [TestCase(1000005, 0)]
        [TestCase(1099997, 9999)]
        public static void MapEventId(int variableAddress, int answer)
        {
            var varAddress = new MapEventVariableAddress(variableAddress);
            var result = varAddress.MapEventId;

            // 取得した値が結果と一致すること
            Assert.AreEqual(result, (MapEventId) answer);
        }

        [TestCase(1000000)]
        [TestCase(1099999)]
        public static void ToIntTest(int value)
        {
            var instance = new MapEventVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(999999, true)]
        [TestCase(1000000, false)]
        [TestCase(1099999, false)]
        [TestCase(1100000, true)]
        public static void CastIntToMapEventVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MapEventVariableAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1000000)]
        [TestCase(1099999)]
        public static void CastMapEventVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new MapEventVariableAddress(value);

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

        [TestCase(1000000, -1, true)]
        [TestCase(1000000, 0, false)]
        [TestCase(1000000, 99999, false)]
        [TestCase(1000000, 100000, true)]
        [TestCase(1035210, -35211, true)]
        [TestCase(1035210, -35210, false)]
        [TestCase(1035210, 64789, false)]
        [TestCase(1035210, 64790, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new MapEventVariableAddress(variableAddress);
            MapEventVariableAddress result = null;

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

        [TestCase(1000000, -100000, true)]
        [TestCase(1000000, -99999, false)]
        [TestCase(1000000, 0, false)]
        [TestCase(1000000, 1, true)]
        [TestCase(1035210, -64790, true)]
        [TestCase(1035210, -64789, false)]
        [TestCase(1035210, 35210, false)]
        [TestCase(1035210, 35211, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new MapEventVariableAddress(variableAddress);
            MapEventVariableAddress result = null;

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

        [TestCase(1035210, 1000000)]
        [TestCase(1000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new MapEventVariableAddress(srcVariableAddress);
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

        [TestCase(1035210, 1000000)]
        [TestCase(1000000, 1099999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new MapEventVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (MapEventVariableAddress) dstVariableAddress;
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
