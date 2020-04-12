using Commons;
using NUnit.Framework;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Ini.Model
{
    [TestFixture]
    public class EventCommandShortCutKeyListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new EventCommandShortCutKeyList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, EventCommandShortCutKeyList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new EventCommandShortCutKeyList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, EventCommandShortCutKeyList.MinCapacity);
        }

        [TestCase("Success", true)]
        [TestCase("DuplicateShortCutKeyList", false)]
        [TestCase("SetValueNotUseShortCutKeyList", false)]
        public static void Validate(string testItemCode, bool answer)
        {
            var instance = new EventCommandShortCutKeyList();

            switch (testItemCode)
            {
                case "Success":
                    // 初期状態 = 正常な状態
                    break;

                case "DuplicateShortCutKeyList":
                    instance[0] = EventCommandShortCutKey.A;
                    instance[1] = EventCommandShortCutKey.A;
                    break;

                case "SetValueNotUseShortCutKeyList":
                    instance[19] = EventCommandShortCutKey.A;
                    break;

                default:
                    Assert.Fail();
                    break;
            }

            var result = instance.Validate(out var errorMsg);

            // 結果が意図した値と一致すること
            Assert.AreEqual(result, answer);

            // チェックOKの場合は以降のテスト不要
            if (result) return;

            // エラーメッセージが格納されていること
            Assert.IsNotEmpty(errorMsg);

            logger.Debug(errorMsg);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new EventCommandShortCutKeyList
            {
                [2] = EventCommandShortCutKey.Eight,
            };
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}