using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Map;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventTest
    {

        [TestCase(-1, true)]
        [TestCase(1, false)]
        [TestCase(10, false)]
        public static void SetMapEventPagesTest(int length, bool isError)
        {
            var instance = new MapEvent();
            var setPages = length == -1 ? null : GenerateMapEventOnePageList(length);

            var errorOccured = false;
            try
            {
                instance.MapEventPageList = setPages;
            }
            catch (ArgumentNullException)
            {
                errorOccured = true;
            }
            catch (ArgumentOutOfRangeException)
            {
                errorOccured = true;
            }
            
            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                // ページ数が正しく取得できること
                Assert.AreEqual(instance.PageValue, length);
            }
        }

        private static MapEventPageList GenerateMapEventOnePageList(int length)
        {
            var list = new List<MapEventPage>();
            for (var i = 0; i < length; i++)
            {
                list.Add(new MapEventPage());
            }

            return new MapEventPageList(list);
        }
    }
}