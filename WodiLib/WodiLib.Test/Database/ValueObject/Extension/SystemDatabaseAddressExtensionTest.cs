using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Extension
{
    [TestFixture]
    public class SystemDatabaseAddressExtensionTest
    {
        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
        }

        [TestCase(1300000000, 0)]
        [TestCase(1335002410, 35)]
        [TestCase(1399999999, 99)]
        public static void GetTypeIdTest(int variableAddress, int answer)
        {
            var varAddress = (SystemDatabaseAddress) variableAddress;
            var answerTypeId = (TypeId) answer;

            Assert.AreEqual(varAddress.GetTypeId(), answerTypeId);
        }

        [TestCase(1300000000, 0)]
        [TestCase(1335002410, 24)]
        [TestCase(1399999999, 9999)]
        public static void GetDataIdTest(int variableAddress, int answer)
        {
            var varAddress = (SystemDatabaseAddress) variableAddress;
            var answerDataId = (DataId) answer;

            Assert.AreEqual(varAddress.GetDataId(), answerDataId);
        }

        [TestCase(1300000000, 0)]
        [TestCase(1335002410, 10)]
        [TestCase(1399999999, 99)]
        public static void GetItemIdTest(int variableAddress, int answer)
        {
            var varAddress = (SystemDatabaseAddress) variableAddress;
            var answerItemId = (ItemId) answer;

            Assert.AreEqual(varAddress.GetItemId(), answerItemId);
        }
    }
}