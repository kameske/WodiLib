using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class UserDatabaseVariableAddressTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
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
                var _ = new UserDatabaseVariableAddress(value);
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
        public static void ToIntTest(int value)
        {
            var instance = new UserDatabaseVariableAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(999999999, true)]
        [TestCase(1000000000, false)]
        [TestCase(1099999999, false)]
        [TestCase(1100000000, true)]
        public static void CastIntToUserDatabaseVariableAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (UserDatabaseVariableAddress)value;
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
        public static void CastUserDatabaseVariableAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new UserDatabaseVariableAddress(value);

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
            var instance = new UserDatabaseVariableAddress(variableAddress);
            UserDatabaseVariableAddress result = null;

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
            var instance = new UserDatabaseVariableAddress(variableAddress);
            UserDatabaseVariableAddress result = null;

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

        [TestCase(1021003522, 1000000000)]
        [TestCase(1000000000, 1000000)]
        public static void OperatorMinusVariableAddressTestA(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new UserDatabaseVariableAddress(srcVariableAddress);
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

        [TestCase(1021003522, 1000000000)]
        [TestCase(1000000000, 1099999999)]
        public static void OperatorMinusVariableAddressTestB(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new UserDatabaseVariableAddress(srcVariableAddress);
            var result = 0;

            var errorOccured = false;
            try
            {
                result = instance - (UserDatabaseVariableAddress)dstVariableAddress;
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
