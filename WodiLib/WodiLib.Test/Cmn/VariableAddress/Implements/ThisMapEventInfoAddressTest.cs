using System;
using Commons;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn
{
    [TestFixture]
    public class ThisMapEventInfoAddressTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(9189999, true)]
        [TestCase(9190000, false)]
        [TestCase(9199999, false)]
        [TestCase(9200000, true)]
        public static void ConstructorIntTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = new ThisMapEventInfoAddress(value);
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
            new object[] {9190000, InfoAddressInfoType.PositionX},
            new object[] {9190001, InfoAddressInfoType.PositionY},
            new object[] {9190002, InfoAddressInfoType.PositionXPrecise},
            new object[] {9190003, InfoAddressInfoType.PositionYPrecise},
            new object[] {9190004, InfoAddressInfoType.Height},
            new object[] {9190005, InfoAddressInfoType.ShadowGraphicId},
            new object[] {9190006, InfoAddressInfoType.Direction},
            new object[] {9199999, InfoAddressInfoType.CharacterGraphicName},
        };

        [TestCaseSource(nameof(InfoTypeTestCaseSource))]
        public static void InfoTypeTest(int variableAddress, InfoAddressInfoType infoType)
        {
            var instance = new ThisMapEventInfoAddress(variableAddress);

            // プロパティの値が意図した値と一致すること
            Assert.AreEqual(instance.InfoType, infoType);
        }

        [TestCase(9190000)]
        [TestCase(9199999)]
        public static void ToIntTest(int value)
        {
            var instance = new ThisMapEventInfoAddress(value);

            var intValue = instance.ToInt();

            // セットした値と取得した値が一致すること
            Assert.AreEqual(intValue, value);
        }

        [TestCase(9189999, true)]
        [TestCase(9190000, false)]
        [TestCase(9199999, false)]
        [TestCase(9200000, true)]
        public static void CastIntToHeroPositionAddressTest(int value, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = (ThisMapEventInfoAddress) value;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(9190000)]
        [TestCase(9199999)]
        public static void CastHeroPositionAddressToIntTest(int value)
        {
            var castValue = 0;

            var instance = new ThisMapEventInfoAddress(value);

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

        [TestCase(9190000, -1, true)]
        [TestCase(9190000, 0, false)]
        [TestCase(9190000, 9, false)]
        [TestCase(9190000, 10000, true)]
        [TestCase(9190003, -4, true)]
        [TestCase(9190003, -3, false)]
        [TestCase(9190003, 9996, false)]
        [TestCase(9190003, 9997, true)]
        public static void OperatorPlusTest(int variableAddress, int value, bool isError)
        {
            var instance = new ThisMapEventInfoAddress(variableAddress);
            ThisMapEventInfoAddress result = null;

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

        [TestCase(9190009, -9991, true)]
        [TestCase(9190009, -9990, false)]
        [TestCase(9190009, 9, false)]
        [TestCase(9190009, 10, true)]
        [TestCase(9199996, -4, true)]
        [TestCase(9199996, -3, false)]
        [TestCase(9199996, 9996, false)]
        [TestCase(9199996, 9997, true)]
        public static void OperatorMinusIntTest(int variableAddress, int value, bool isError)
        {
            var instance = new ThisMapEventInfoAddress(variableAddress);
            ThisMapEventInfoAddress result = null;

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

        [TestCase(9190003, 9190000)]
        [TestCase(9190000, 1000000)]
        public static void OperatorMinusVariableAddressTest(int srcVariableAddress, int dstVariableAddress)
        {
            var instance = new ThisMapEventInfoAddress(srcVariableAddress);
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
            var target = (ThisMapEventInfoAddress) 9190003;
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}