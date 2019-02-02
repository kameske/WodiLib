using NUnit.Framework;
using WodiLib.Event;

namespace WodiLib.Test.Event
{
    [TestFixture]
    public class VariableAddressHelperTest
    {
        private static readonly object[] TestCaseSource =
        {
            new object[] {-1, "Not"},
            new object[] {999999, "Not"},
            new object[] {1000000, "MapEvent"},
            new object[] {1099999, "MapEvent"},
            new object[] {1100000, "ThisMapEvent"},
            new object[] {1100009, "ThisMapEvent"},
            new object[] {1100010, "None"},
            new object[] {1999999, "None"},
            new object[] {2000000, "NormalNumberVariable"},
            new object[] {2099999, "NormalNumberVariable"},
            new object[] {2100000, "SpareNumberVariable"},
            new object[] {2199999, "SpareNumberVariable"},
            new object[] {3000000, "StringVariable"},
            new object[] {3999999, "StringVariable"},
            new object[] {4000000, "None"},
            new object[] {7999999, "None"},
            new object[] {8000000, "Random"},
            new object[] {8999999, "Random"},
            new object[] {9000000, "System"},
            new object[] {9099999, "System"},
            new object[] {9100000, "EventPosition"},
            new object[] {9179999, "EventPosition"},
            new object[] {9180000, "HeroPosition"},
            new object[] {9180007, "None"},
            new object[] {9180008, "None"},
            new object[] {9180009, "HeroPosition"},
            new object[] {9180010, "MemberPosition"},
            new object[] {9180027, "None"},
            new object[] {9180038, "None"},
            new object[] {9180059, "MemberPosition"},
            new object[] {9180060, "None"},
            new object[] {9900000, "SystemString"},
            new object[] {9999999, "SystemString"},
            new object[] {15000000, "CommonEvent"},
            new object[] {15999999, "CommonEvent"},
            new object[] {16000000, "ThisCommonEvent"},
            new object[] {16000099, "ThisCommonEvent"},
            new object[] {16000100, "None"},
            new object[] {999999999, "None"},
            new object[] {1000000000, "UserDatabase"},
            new object[] {1099999999, "UserDatabase"},
            new object[] {1100000000, "ChangeableDatabase"},
            new object[] {1199999999, "ChangeableDatabase"},
            new object[] {1200000000, "None"},
            new object[] {1299999999, "None"},
            new object[] {1300000000, "SystemDatabase"},
            new object[] {1399999999, "SystemDatabase"},
            new object[] {1400000000, "Not"},
        };
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsVariableAddressTest(int number, string typeCode)
        {
            var isTrue = !typeCode.Equals("Not");
            var result = VariableAddressHelper.IsVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }

        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsVariableAddressStrictTest(int number, string typeCode)
        {
            var isTrue = !typeCode.Equals("Not") && !typeCode.Equals("None");
            var result = VariableAddressHelper.IsVariableAddressStrict(number);
            Assert.AreEqual(result, isTrue);
        }
     
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsMapEventVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("MapEvent") ;
            var result = VariableAddressHelper.IsMapEventVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsThisMapEventVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("ThisMapEvent") ;
            var result = VariableAddressHelper.IsThisMapEventVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsCommonEventVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("CommonEvent") ;
            var result = VariableAddressHelper.IsCommonEventVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsThisCommonEventVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("ThisCommonEvent") ;
            var result = VariableAddressHelper.IsThisCommonEventVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsNormalNumberVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("NormalNumberVariable") ;
            var result = VariableAddressHelper.IsNormalNumberVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsSpareNumberVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("SpareNumberVariable") ;
            var result = VariableAddressHelper.IsSpareNumberVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsStringVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("StringVariable") ;
            var result = VariableAddressHelper.IsStringVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsRandomVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("Random") ;
            var result = VariableAddressHelper.IsRandomVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsSystemVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("System") ;
            var result = VariableAddressHelper.IsSystemVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsEventPositionAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("EventPosition") ;
            var result = VariableAddressHelper.IsEventPositionAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsHeroPositionAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("HeroPosition") ;
            var result = VariableAddressHelper.IsHeroPositionAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsMemberPositionAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("MemberPosition") ;
            var result = VariableAddressHelper.IsMemberPositionAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsSystemStringVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("SystemString") ;
            var result = VariableAddressHelper.IsSystemStringVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsUserDatabaseVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("UserDatabase") ;
            var result = VariableAddressHelper.IsUserDatabaseVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsChangeableDatabaseVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("ChangeableDatabase") ;
            var result = VariableAddressHelper.IsChangeableDatabaseVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
        
        [TestCaseSource(nameof(TestCaseSource))]
        public static void IsSystemDatabaseVariableAddressTest(int number, string typeCode)
        {
            var isTrue = typeCode.Equals("SystemDatabase") ;
            var result = VariableAddressHelper.IsSystemDatabaseVariableAddress(number);
            Assert.AreEqual(result, isTrue);
        }
    }
}