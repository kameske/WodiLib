using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class HeroInfoAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(9179999, true)]
        [TestCase(9180000, false)]
        [TestCase(9180009, false)]
        [TestCase(9180010, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new HeroInfoAddress(value);
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
            new object[] {9180000, InfoAddressInfoType.PositionX},
            new object[] {9180001, InfoAddressInfoType.PositionY},
            new object[] {9180002, InfoAddressInfoType.PositionXPrecise},
            new object[] {9180003, InfoAddressInfoType.PositionYPrecise},
            new object[] {9180004, InfoAddressInfoType.Height},
            new object[] {9180005, InfoAddressInfoType.ShadowGraphicId},
            new object[] {9180006, InfoAddressInfoType.Direction},
            new object[] {9180009, InfoAddressInfoType.CharacterGraphicName},
        };

        [TestCaseSource(nameof(InfoTypeTestCaseSource))]
        public static void InfoTypeTest(int variableAddress, InfoAddressInfoType infoType)
        {
            var instance = new HeroInfoAddress(variableAddress);

            // プロパティの値が意図した値と一致すること
            Assert.AreEqual(instance.InfoType, infoType);
        }

        [TestCase(9180000)]
        [TestCase(9180009)]
        public static void ToIntTest(int value)
        {
            var instance = new HeroInfoAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(9179999, true)]
        [TestCase(9180000, false)]
        [TestCase(9180009, false)]
        [TestCase(9180010, true)]
        public static void CastIntToHeroPositionAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (HeroInfoAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9180000)]
        [TestCase(9180009)]
        public static void CastHeroPositionAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new HeroInfoAddress(value);

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

        [TestCase(9180000, -1, true)]
        [TestCase(9180000, 0, false)]
        [TestCase(9180000, 9, false)]
        [TestCase(9180000, 10, true)]
        [TestCase(9180003, -4, true)]
        [TestCase(9180003, -3, false)]
        [TestCase(9180003, 6, false)]
        [TestCase(9180003, 7, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new HeroInfoAddress(variableAddress);
            HeroInfoAddress result = null;

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

        [TestCase(9180000, -10, true)]
        [TestCase(9180000, -9, false)]
        [TestCase(9180000, 0, false)]
        [TestCase(9180000, 1, true)]
        [TestCase(9180003, -7, true)]
        [TestCase(9180003, -6, false)]
        [TestCase(9180003, 3, false)]
        [TestCase(9180003, 4, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new HeroInfoAddress(variableAddress);
            HeroInfoAddress result = null;

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

        [TestCase(9180003, 9180000)]
        [TestCase(9180000, 1000000)]
        public static void OperatorMinusVariableAddressTest(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new HeroInfoAddress(srcVariableAddress);
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

        [Test]
        public static void SerializeTest()
        {
            var target = (HeroInfoAddress) 9180003;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}