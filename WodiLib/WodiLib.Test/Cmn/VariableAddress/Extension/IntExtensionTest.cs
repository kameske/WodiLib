using NUnit.Framework;
using WodiLib.Cmn;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class IntExtensionTest
    {
        [TestCase(999999, false)]
        [TestCase(1000000, true)]
        [TestCase(1099999, true)]
        [TestCase(1100000, true)]
        [TestCase(1100009, true)]
        [TestCase(1100010, false)]
        [TestCase(1599999, false)]
        [TestCase(1600000, true)]
        [TestCase(1600099, true)]
        [TestCase(1600100, false)]
        [TestCase(1999999, false)]
        [TestCase(2000000, true)]
        [TestCase(2099999, true)]
        [TestCase(2100000, true)]
        [TestCase(2999999, true)]
        [TestCase(3000000, true)]
        [TestCase(3999999, true)]
        [TestCase(4000000, false)]
        [TestCase(7999999, false)]
        [TestCase(8000000, true)]
        [TestCase(8999999, true)]
        [TestCase(9000000, true)]
        [TestCase(9099999, true)]
        [TestCase(9100000, true)]
        [TestCase(9179999, true)]
        [TestCase(9180000, true)]
        [TestCase(9180009, true)]
        [TestCase(9180010, true)]
        [TestCase(9180059, true)]
        [TestCase(9180060, false)]
        [TestCase(9899999, false)]
        [TestCase(9900000, true)]
        [TestCase(9999999, true)]
        [TestCase(10000000, false)]
        [TestCase(14899999, false)]
        [TestCase(15000000, true)]
        [TestCase(15999999, true)]
        [TestCase(16000000, false)]
        [TestCase(999999999, false)]
        [TestCase(1000000000, true)]
        [TestCase(1099999999, true)]
        [TestCase(1100000000, true)]
        [TestCase(1199999999, true)]
        [TestCase(1200000000, false)]
        [TestCase(1299999999, false)]
        [TestCase(1300000000, true)]
        [TestCase(1399999920, true)]
        [TestCase(1399999921, true)]
        [TestCase(1399999999, true)]
        [TestCase(1400000000, false)]
        public static void IsVariableAddressTest(int value, bool isVariableAddress)
        {
            var result = value.IsVariableAddress();

            // 結果が一致すること
            Assert.AreEqual(result, isVariableAddress);
        }
    }
}