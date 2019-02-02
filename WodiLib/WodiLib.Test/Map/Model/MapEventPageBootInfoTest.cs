using System;
using NUnit.Framework;
using WodiLib.Map;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageBootInfoTest
    {

        private static readonly object[] EventBootTypeTestCaseSource =
        {
            new object[] {EventBootType.Auto, false},
            new object[] {null, true}
        };
        [TestCaseSource(nameof(EventBootTypeTestCaseSource))]
        public static void EventBootTypeTest(EventBootType bootType, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.EventBootType = bootType;
            }
            catch (Exception)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(3, false)]
        [TestCase(4, true)]
        public static void SetHasEventBootConditionTest(int index, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.SetHasEventBootCondition(index, true);
            }
            catch (Exception)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition1SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.EventBootCondition1 = isNull ? null : new EventBootCondition();
            }
            catch (Exception)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition2SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.EventBootCondition2 = isNull ? null : new EventBootCondition();
            }
            catch (Exception)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition3SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.EventBootCondition3 = isNull ? null : new EventBootCondition();
            }
            catch (Exception)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public static void EventBootCondition4SetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.EventBootCondition4 = isNull ? null : new EventBootCondition();
            }
            catch (Exception)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        [TestCase(-1, false, true)]
        [TestCase(0, false, false)]
        [TestCase(0, true, true)]
        [TestCase(3, false, false)]
        [TestCase(3, true, true)]
        [TestCase(4, false, true)]
        public static void SetEventBootConditionTest(int index, bool isNull, bool isError)
        {
            var instance = new MapEventPageBootInfo();
            var errorOccured = false;
            try
            {
                instance.SetEventBootCondition(index, isNull ? null : new EventBootCondition());
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