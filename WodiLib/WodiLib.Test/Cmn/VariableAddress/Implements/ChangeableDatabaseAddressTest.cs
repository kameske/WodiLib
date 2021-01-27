using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class ChangeableDatabaseAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(1099999999, true)]
        [TestCase(1100000000, false)]
        [TestCase(1199999999, false)]
        [TestCase(1200000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ChangeableDatabaseAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1100000000, 0)]
        [TestCase(1135002410, 35)]
        [TestCase(1199999999, 99)]
        public static void TypeIdTest(int variableAddress, int answer)
        {
            var varAddress = (ChangeableDatabaseAddress) variableAddress;
            var answerTypeId = (TypeId) answer;

            Assert.AreEqual(varAddress.TypeId, answerTypeId);
        }

        [TestCase(1100000000, 0)]
        [TestCase(1135002410, 24)]
        [TestCase(1199999999, 9999)]
        public static void DataIdTest(int variableAddress, int answer)
        {
            var varAddress = (ChangeableDatabaseAddress) variableAddress;
            var answerDataId = (DataId) answer;

            Assert.AreEqual(varAddress.DataId, answerDataId);
        }

        [TestCase(1100000000, 0)]
        [TestCase(1135002410, 10)]
        [TestCase(1199999999, 99)]
        public static void ItemIdTest(int variableAddress, int answer)
        {
            var varAddress = (ChangeableDatabaseAddress) variableAddress;
            var answerItemId = (ItemId) answer;

            Assert.AreEqual(varAddress.ItemId, answerItemId);
        }

        [TestCase(1100000000)]
        [TestCase(1199999999)]
        public static void ToIntTest(int value)
        {
            var instance = new ChangeableDatabaseAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(1099999999, true)]
        [TestCase(1100000000, false)]
        [TestCase(1199999999, false)]
        [TestCase(1200000000, true)]
        public static void CastIntToChangeableDatabaseAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (ChangeableDatabaseAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1100000000)]
        [TestCase(1199999999)]
        public static void CastChangeableDatabaseAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new ChangeableDatabaseAddress(value);

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

        [TestCase(1100000000, -1, true)]
        [TestCase(1100000000, 0, false)]
        [TestCase(1100000000, 99999999, false)]
        [TestCase(1100000000, 100000000, true)]
        [TestCase(1125000050, -25000051, true)]
        [TestCase(1125000050, -25000050, false)]
        [TestCase(1125000050, 74999949, false)]
        [TestCase(1125000050, 74999950, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new ChangeableDatabaseAddress(variableAddress);
            ChangeableDatabaseAddress result = null;

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

        [TestCase(1100000000, -100000000, true)]
        [TestCase(1100000000, -99999999, false)]
        [TestCase(1100000000, 0, false)]
        [TestCase(1100000000, 1, true)]
        [TestCase(1125000050, -74999950, true)]
        [TestCase(1125000050, -74999949, false)]
        [TestCase(1125000050, 25000050, false)]
        [TestCase(1125000050, 25000051, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new ChangeableDatabaseAddress(variableAddress);
            ChangeableDatabaseAddress result = null;

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

        [TestCase(1125000050, 1100000000)]
        [TestCase(1100000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new ChangeableDatabaseAddress(srcVariableAddress);
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

        [TestCase(1125000050, 1100000000)]
        [TestCase(1100000000, 1100000000)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new ChangeableDatabaseAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (ChangeableDatabaseAddress) dstVariableAddress;
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
