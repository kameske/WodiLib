using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class MemberInfoAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(9180009, true)]
        [TestCase(9180010, false)]
        [TestCase(9180059, false)]
        [TestCase(9180060, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new MemberInfoAddress(value);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] InfoTypeTestCaseSource =
        {
            new object[] { 9180010, InfoAddressInfoType.PositionX },
            new object[] { 9180021, InfoAddressInfoType.PositionY },
            new object[] { 9180022, InfoAddressInfoType.PositionXPrecise },
            new object[] { 9180013, InfoAddressInfoType.PositionYPrecise },
            new object[] { 9180024, InfoAddressInfoType.Height },
            new object[] { 9180055, InfoAddressInfoType.ShadowGraphicId },
            new object[] { 9180026, InfoAddressInfoType.Direction },
            new object[] { 9180049, InfoAddressInfoType.CharacterGraphicName }
        };

        [TestCaseSource(nameof(InfoTypeTestCaseSource))]
        public static void InfoTypeTest(int variableAddress, InfoAddressInfoType infoType)
        {
            var instance = new MemberInfoAddress(variableAddress);

            // プロパティの値が意図した値と一致すること
            Assert.AreEqual(instance.InfoType, infoType);
        }

        [TestCase(9180010)]
        [TestCase(9180059)]
        public static void ToIntTest(int value)
        {
            var instance = new MemberInfoAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(9180009, true)]
        [TestCase(9180010, false)]
        [TestCase(9180059, false)]
        [TestCase(9180060, true)]
        public static void CastIntToMemberPositionAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (MemberInfoAddress)value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9180010)]
        [TestCase(9180059)]
        public static void CastMemberPositionAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new MemberInfoAddress(value);

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

        [TestCase(9180010, -1, true)]
        [TestCase(9180010, 0, false)]
        [TestCase(9180010, 49, false)]
        [TestCase(9180010, 50, true)]
        [TestCase(9180030, -21, true)]
        [TestCase(9180030, -20, false)]
        [TestCase(9180030, 29, false)]
        [TestCase(9180030, 30, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new MemberInfoAddress(variableAddress);
            MemberInfoAddress result = null;

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

        [TestCase(9180010, -50, true)]
        [TestCase(9180010, -49, false)]
        [TestCase(9180010, 0, false)]
        [TestCase(9180010, 1, true)]
        [TestCase(9180030, -30, true)]
        [TestCase(9180030, -29, false)]
        [TestCase(9180030, 20, false)]
        [TestCase(9180030, 21, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new MemberInfoAddress(variableAddress);
            MemberInfoAddress result = null;

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

        [TestCase(9180030, 9180010)]
        [TestCase(9180010, 1000000)]
        public static void OperatorMinusVariableAddressTest(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new MemberInfoAddress(srcVariableAddress);
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
    }
}
