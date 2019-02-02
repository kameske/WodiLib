using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Event;
using NUnit.Framework;
using WodiLib.Event.CharaMoveCommand;

namespace WodiLib.Test.Event
{
    [TestFixture]
    public class ActionEntryTest
    {
        private static readonly object[] testCaseSource =
        {
            new object[] { MakeCommands(1), false, false, false},
            new object[] { MakeCommands(2), false, false, true},
            new object[] { MakeCommands(3), false, true, false},
            new object[] { MakeCommands(4), true, false, false},
            new object[] { MakeCommands(5), false, true, true},
            new object[] { MakeCommands(0), true, true, false},
            new object[] { MakeCommands(4), true, false, true},
            new object[] { MakeCommands(1), true, true, true},
        };

        [TestCaseSource(nameof(testCaseSource))]
        public static void IsWaitForCompleteTest(
            IEnumerable<ICharaMoveCommand> commands,
            bool isWaitForComplete,
            bool isRepeatAction,
            bool isSkipIfCannotMove)
        {
            bool result;
            try
            {
                var instance = GetInstance(commands, isWaitForComplete, isRepeatAction, isSkipIfCannotMove);
                result = instance.IsWaitForComplete == isWaitForComplete;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(testCaseSource))]
        public static void IsRepeatActionTest(
            IEnumerable<ICharaMoveCommand> commands,
            bool isWaitForComplete,
            bool isRepeatAction,
            bool isSkipIfCannotMove)
        {
            bool result;
            try
            {
                var instance = GetInstance(commands, isWaitForComplete, isRepeatAction, isSkipIfCannotMove);

                result = instance.IsRepeatAction == isRepeatAction;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(testCaseSource))]
        public static void IsSkipIfCannotMoveTest(
            IEnumerable<ICharaMoveCommand> commands,
            bool isWaitForComplete,
            bool isRepeatAction,
            bool isSkipIfCannotMove)
        {
            bool result;
            try
            {
                var instance = GetInstance(commands, isWaitForComplete, isRepeatAction, isSkipIfCannotMove);

                result = instance.IsSkipIfCannotMove == isSkipIfCannotMove;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(testCaseSource))]
        public static void CommandsTest(
            IEnumerable<ICharaMoveCommand> commands,
            bool isWaitForComplete,
            bool isRepeatAction,
            bool isSkipIfCannotMove)
        {
            bool result;
            try
            {
                var charaMoveCommands = commands as ICharaMoveCommand[] ?? commands.ToArray();
                var instance = GetInstance(charaMoveCommands, isWaitForComplete, isRepeatAction, isSkipIfCannotMove);
                result = instance.CommandList.Count == charaMoveCommands.Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            Assert.IsTrue(result);
        }

        [TestCaseSource(nameof(testCaseSource))]
        public static void MakeOptionByteTest(
            IEnumerable<ICharaMoveCommand> commands,
            bool isWaitForComplete,
            bool isRepeatAction,
            bool isSkipIfCannotMove)
        {
            bool result;
            try
            {
                var instance = GetInstance(commands, isWaitForComplete, isRepeatAction, isSkipIfCannotMove);

                byte flagByte = 0x00;
                if(isWaitForComplete) flagByte += ActionEntry.FlgWaitForCompleteOn;
                if(isRepeatAction) flagByte += ActionEntry.FlgRepeatAction;
                if(isSkipIfCannotMove) flagByte += ActionEntry.FlgSkipIfCannotMove;

                result = instance.MakeOptionByte() == flagByte;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }

            Assert.IsTrue(result);
        }

        /// <summary>
        /// 動作指定コマンドリストを指定したコマンド数リストにして返す。
        /// </summary>
        /// <param name="value">コマンド数</param>
        /// <returns>動作指定コマンドリスト</returns>
        private static IEnumerable<ICharaMoveCommand> MakeCommands(int value)
        {
            var result = new List<ICharaMoveCommand>();
            for (var i = 0; i < value; i++)
            {
                result.Add(new MoveLeft());
            }
            return result;
        }

        private static ActionEntry GetInstance(
            IEnumerable<ICharaMoveCommand> commands,
            bool isWaitForComplete,
            bool isRepeatAction,
            bool isSkipIfCannotMove)
        {
            var instance = new ActionEntry(commands)
            {
                IsWaitForComplete = isWaitForComplete,
                IsRepeatAction = isRepeatAction,
                IsSkipIfCannotMove = isSkipIfCannotMove
            };
            return instance;
        }
    }
}