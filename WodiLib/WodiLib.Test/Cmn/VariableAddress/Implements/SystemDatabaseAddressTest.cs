using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class SystemDatabaseAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(1299999999, true)]
        [TestCase(1300000000, false)]
        [TestCase(1399999919, false)]
        [TestCase(1399999920, false)]
        [TestCase(1399999999, false)]
        [TestCase(1400000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new SystemDatabaseAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1300000000, 0)]
        [TestCase(1335002410, 35)]
        [TestCase(1399999999, 99)]
        public static void TypeIdTest(int variableAddress, int answer)
        {
            var varAddress = (SystemDatabaseAddress)variableAddress;
            var answerTypeId = (TypeId)answer;

            Assert.AreEqual(varAddress.TypeId, answerTypeId);
        }

        [TestCase(1300000000, 0)]
        [TestCase(1335002410, 24)]
        [TestCase(1399999999, 9999)]
        public static void DataIdTest(int variableAddress, int answer)
        {
            var varAddress = (SystemDatabaseAddress)variableAddress;
            var answerDataId = (DataId)answer;

            Assert.AreEqual(varAddress.DataId, answerDataId);
        }

        [TestCase(1300000000, 0)]
        [TestCase(1335002410, 10)]
        [TestCase(1399999999, 99)]
        public static void ItemIdTest(int variableAddress, int answer)
        {
            var varAddress = (SystemDatabaseAddress)variableAddress;
            var answerItemId = (ItemId)answer;

            Assert.AreEqual(varAddress.ItemId, answerItemId);
        }

        [TestCase(1300000000)]
        [TestCase(1399999919)]
        [TestCase(1399999920)]
        [TestCase(1399999999)]
        public static void ToIntTest(int value)
        {
            var instance = new SystemDatabaseAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(1299999999, true)]
        [TestCase(1300000000, false)]
        [TestCase(1399999919, false)]
        [TestCase(1399999920, false)]
        [TestCase(1399999999, false)]
        [TestCase(1400000000, true)]
        public static void CastIntToSystemDatabaseAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (SystemDatabaseAddress)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1300000000)]
        [TestCase(1399999919)]
        [TestCase(1399999920)]
        [TestCase(1399999999)]
        public static void CastSystemDatabaseAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new SystemDatabaseAddress(value);

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

        [TestCase(1300000000, -1, true)]
        [TestCase(1300000000, 0, false)]
        [TestCase(1300000000, 99999999, false)]
        [TestCase(1300000000, 100000000, true)]
        [TestCase(1325002410, -25002411, true)]
        [TestCase(1325002410, -25002410, false)]
        [TestCase(1325002410, 74997589, false)]
        [TestCase(1325002410, 74997590, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new SystemDatabaseAddress(variableAddress);
            SystemDatabaseAddress result = null;

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

        [TestCase(1300000000, -100000000, true)]
        [TestCase(1300000000, -99999999, false)]
        [TestCase(1300000000, 0, false)]
        [TestCase(1300000000, 1, true)]
        [TestCase(1325002410, -74997590, true)]
        [TestCase(1325002410, -74997589, false)]
        [TestCase(1325002410, 25002410, false)]
        [TestCase(1325002410, 25002411, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new SystemDatabaseAddress(variableAddress);
            SystemDatabaseAddress result = null;

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

        [TestCase(1325002410, 1300000000)]
        [TestCase(1300000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SystemDatabaseAddress(srcVariableAddress);
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

        [TestCase(1325002410, 1300000000)]
        [TestCase(1300000000, 1399999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new SystemDatabaseAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (SystemDatabaseAddress)dstVariableAddress;
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
