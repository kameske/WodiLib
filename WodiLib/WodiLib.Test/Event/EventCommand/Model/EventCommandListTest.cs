using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand.Model
{
    [TestFixture]
    public class EventCommandListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        /// <summary>
        /// コンストラクタテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isError"></param>
        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(10, false)]
        [TestCase(9999, false)]
        public static void ConstructorTest(int length, bool isError)
        {
            var errorOccured = false;
            try
            {
                var pageList = length == -1 ? null : GenerateEventCommandList(length);
                var _ = new EventCommandList(pageList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [Test]
        public static void GetMaxCapacityTest()
        {
            var instance = new EventCommandList();
            var maxCapacity = instance.GetMaxCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, EventCommandList.MaxCapacity);
        }

        [Test]
        public static void GetMinCapacityTest()
        {
            var instance = new EventCommandList();
            var maxCapacity = instance.GetMinCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, EventCommandList.MinCapacity);
        }

        private static readonly object[] ValidateTestCaseSource =
        {
            new object[] {new IEventCommand[] {new Blank()}, true},
            new object[] {new IEventCommand[] {new CallCommonEventById()}, false},
            new object[] {new IEventCommand[] {new Message(), new Blank {Indent = 0}}, true},
            new object[] {new IEventCommand[] {new Message(), new Blank {Indent = 1}}, false},
            new object[] {new IEventCommand[] {new Message(), new DebugText()}, false},
        };

        [TestCaseSource(nameof(ValidateTestCaseSource))]
        public static void ValidateTest(IReadOnlyList<IEventCommand> commands, bool result)
        {
            var instance = new EventCommandList(commands);
            var validFlag = instance.Validate();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            // エラーフラグが一致すること
            Assert.AreEqual(validFlag, result);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new EventCommandList(GenerateEventCommandList(3));
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }

        private static IReadOnlyList<IEventCommand> GenerateEventCommandList(int length)
        {
            var list = new List<IEventCommand>();
            for (var i = 0; i < length; i++)
            {
                list.Add(new Blank());
            }

            return list;
        }
    }
}