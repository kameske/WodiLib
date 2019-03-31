using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Extension
{
    [TestFixture]
    public class UserDatabaseAddressExtensionTest
    {
        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
        }

        [TestCase(1000000000, 0)]
        [TestCase(1035002410, 35)]
        [TestCase(1099999999, 99)]
        public static void GetTypeIdTest(int variableAddress, int answer)
        {
            var varAddress = (UserDatabaseAddress) variableAddress;
            var answerTypeId = (TypeId) answer;

            Assert.AreEqual(varAddress.GetTypeId(), answerTypeId);
        }

        [TestCase(1000000000, 0)]
        [TestCase(1035002410, 24)]
        [TestCase(1099999999, 9999)]
        public static void GetDataIdTest(int variableAddress, int answer)
        {
            var varAddress = (UserDatabaseAddress) variableAddress;
            var answerDataId = (DataId) answer;

            Assert.AreEqual(varAddress.GetDataId(), answerDataId);
        }

        [TestCase(1000000000, 0)]
        [TestCase(1035002410, 10)]
        [TestCase(1099999999, 99)]
        public static void GetItemIdTest(int variableAddress, int answer)
        {
            var varAddress = (UserDatabaseAddress) variableAddress;
            var answerItemId = (ItemId) answer;

            Assert.AreEqual(varAddress.GetItemId(), answerItemId);
        }
    }
}