using System;
using System.Collections.Generic;
using WodiLib.Map;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Event.EventCommand;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventOnePageTest
    {
        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void GraphicInfoSetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPage();
            var errorOccured = false;
            try
            {
                instance.GraphicInfo = isNull ? null : new MapEventPageGraphicInfo();
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void BootInfoSetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPage();
            var errorOccured = false;
            try
            {
                instance.BootInfo = isNull ? null : new MapEventPageBootInfo();
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
        
        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void MoveRouteInfoSetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPage();
            var errorOccured = false;
            try
            {
                instance.MoveRouteInfo = isNull ? null : new MapEventPageMoveRouteInfo();
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void SetOptionFlagTest(bool isNull, bool isError)
        {
            var instance = new MapEventPage();
            var errorOccured = false;
            try
            {
                instance.Option = isNull ? null : new MapEventPageOption();
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] EventCommandsSetTestCaseSource =
        {
            new object[] {new List<IEventCommand>(), false},
            new object[] {new List<IEventCommand>{ new Blank()}, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(EventCommandsSetTestCaseSource))]
        public static void EventCommandsSetTest(IEnumerable<IEventCommand> commands, bool isError)
        {
            var instance = new MapEventPage();
            var errorOccured = false;
            try
            {
                instance.EventCommands = new EventCommandList(commands);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}