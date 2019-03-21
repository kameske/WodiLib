using System;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class VariableAddressFactoryTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] CreateTestCaseSource =
        {
            new object[] {int.MinValue, true, null},
            new object[] {-2000000000, true, null},
            new object[] {999999, true, null},
            new object[] {1000000, false, typeof(MapEventVariableAddress)},
            new object[] {1099999, false, typeof(MapEventVariableAddress)},
            new object[] {1100000, false, typeof(ThisMapEventVariableAddress)},
            new object[] {1100009, false, typeof(ThisMapEventVariableAddress)},
            new object[] {1100010, true, null},
            new object[] {1599999, true, null},
            new object[] {1600000, false, typeof(ThisCommonEventVariableAddress)},
            new object[] {1600099, false, typeof(ThisCommonEventVariableAddress)},
            new object[] {1600100, true, null},
            new object[] {1999999, true, null},
            new object[] {2000000, false, typeof(NormalNumberVariableAddress)},
            new object[] {2099999, false, typeof(NormalNumberVariableAddress)},
            new object[] {2100000, false, typeof(SpareNumberVariableAddress)},
            new object[] {2999999, false, typeof(SpareNumberVariableAddress)},
            new object[] {3000000, false, typeof(StringVariableAddress)},
            new object[] {3999999, false, typeof(StringVariableAddress)},
            new object[] {4000000, true, null},
            new object[] {7999999, true, null},
            new object[] {8000000, false, typeof(RandomVariableAddress)},
            new object[] {8999999, false, typeof(RandomVariableAddress)},
            new object[] {9000000, false, typeof(SystemVariableAddress)},
            new object[] {9099999, false, typeof(SystemVariableAddress)},
            new object[] {9100000, false, typeof(EventInfoAddress)},
            new object[] {9179999, false, typeof(EventInfoAddress)},
            new object[] {9180000, false, typeof(HeroInfoAddress)},
            new object[] {9180009, false, typeof(HeroInfoAddress)},
            new object[] {9180010, false, typeof(MemberInfoAddress)},
            new object[] {9180059, false, typeof(MemberInfoAddress)},
            new object[] {9180060, true, null},
            new object[] {9899999, true, null},
            new object[] {9900000, false, typeof(SystemStringVariableAddress)},
            new object[] {9999999, false, typeof(SystemStringVariableAddress)},
            new object[] {10000000, true, null},
            new object[] {14899999, true, null},
            new object[] {15000000, false, typeof(CommonEventVariableAddress)},
            new object[] {15999999, false, typeof(CommonEventVariableAddress)},
            new object[] {16000000, true, null},
            new object[] {999999999, true, null},
            new object[] {1000000000, false, typeof(UserDatabaseVariableAddress)},
            new object[] {1099999999, false, typeof(UserDatabaseVariableAddress)},
            new object[] {1100000000, false, typeof(ChangeableDatabaseVariableAddress)},
            new object[] {1199999999, false, typeof(ChangeableDatabaseVariableAddress)},
            new object[] {1300000000, false, typeof(SystemDatabaseVariableAddress)},
            new object[] {1399999920, false, typeof(SystemDatabaseVariableAddress)},
            new object[] {1399999921, false, typeof(SystemDatabaseVariableAddress)},
            new object[] {1399999999, false, typeof(SystemDatabaseVariableAddress)},
            new object[] {1400000000, true, null},
            new object[] {2000000000, true, null},
            new object[] {int.MaxValue, true, null},
        };

        [TestCaseSource(nameof(CreateTestCaseSource))]
        public static void CreateTest(int value, bool isError, Type createdInstanceType)
        {
            VariableAddress instance = null;

            var errorOccured = false;
            try
            {
                instance = VariableAddressFactory.Create(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 意図した型にキャストできること
            Assert.AreEqual(instance.GetType(), createdInstanceType);

            // セットした値が正しく取得できること
            Assert.AreEqual(instance.ToInt(), value);
        }

    }
}