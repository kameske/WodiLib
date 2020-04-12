using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class UserDatabaseAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(999999999, true)]
        [TestCase(1000000000, false)]
        [TestCase(1099999999, false)]
        [TestCase(1100000000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new UserDatabaseAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1000000000, 0)]
        [TestCase(1035002410, 35)]
        [TestCase(1099999999, 99)]
        public static void TypeIdTest(int variableAddress, int answer)
        {
            var varAddress = (UserDatabaseAddress) variableAddress;
            var answerTypeId = (TypeId) answer;

            Assert.AreEqual(varAddress.TypeId, answerTypeId);
        }

        [TestCase(1000000000, 0)]
        [TestCase(1035002410, 24)]
        [TestCase(1099999999, 9999)]
        public static void DataIdTest(int variableAddress, int answer)
        {
            var varAddress = (UserDatabaseAddress) variableAddress;
            var answerDataId = (DataId) answer;

            Assert.AreEqual(varAddress.DataId, answerDataId);
        }

        [TestCase(1000000000, 0)]
        [TestCase(1035002410, 10)]
        [TestCase(1099999999, 99)]
        public static void ItemIdTest(int variableAddress, int answer)
        {
            var varAddress = (UserDatabaseAddress) variableAddress;
            var answerItemId = (ItemId) answer;

            Assert.AreEqual(varAddress.ItemId, answerItemId);
        }

        [TestCase(1000000000)]
        [TestCase(1099999999)]
        public static void ToIntTest(int value)
        {
            var instance = new UserDatabaseAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(999999999, true)]
        [TestCase(1000000000, false)]
        [TestCase(1099999999, false)]
        [TestCase(1100000000, true)]
        public static void CastIntToUserDatabaseAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (UserDatabaseAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(1000000000)]
        [TestCase(1099999999)]
        public static void CastUserDatabaseAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new UserDatabaseAddress(value);

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

        [TestCase(1000000000, -1, true)]
        [TestCase(1000000000, 0, false)]
        [TestCase(1000000000, 99999999, false)]
        [TestCase(1000000000, 100000000, true)]
        [TestCase(1021003522, -21003523, true)]
        [TestCase(1021003522, -21003522, false)]
        [TestCase(1021003522, 78996477, false)]
        [TestCase(1021003522, 78996478, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new UserDatabaseAddress(variableAddress);
            UserDatabaseAddress result = null;

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

        [TestCase(1000000000, -100000000, true)]
        [TestCase(1000000000, -99999999, false)]
        [TestCase(1000000000, 0, false)]
        [TestCase(1000000000, 1, true)]
        [TestCase(1021003522, -78996478, true)]
        [TestCase(1021003522, -78996477, false)]
        [TestCase(1021003522, 21003522, false)]
        [TestCase(1021003522, 21003523, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new UserDatabaseAddress(variableAddress);
            UserDatabaseAddress result = null;

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

        [TestCase(1021003522, 1000000000)]
        [TestCase(1000000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new UserDatabaseAddress(srcVariableAddress);
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

        [TestCase(1021003522, 1000000000)]
        [TestCase(1000000000, 1099999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new UserDatabaseAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (UserDatabaseAddress) dstVariableAddress;
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
            var target = (UserDatabaseAddress) 1021003522;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}