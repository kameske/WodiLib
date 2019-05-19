using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageListTest
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
        [TestCase(0, true)]
        [TestCase(1, false)]
        [TestCase(10, false)]
        [TestCase(11, true)]
        public static void ConstructorTest(int length, bool isError)
        {
            var errorOccured = false;
            try
            {
                var pageList = length == -1 ? null : GenerateMapEventOnePageList(length);
                var _ = new MapEventPageList(pageList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// Addメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="isError"></param>
        [TestCase(1, false)]
        [TestCase(9, false)]
        [TestCase(10, true)]
        public static void AddTest(int length, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));

            var errorOccured = false;
            try
            {
                instance.Add(new MapEventPage());
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// AddRangeメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="count"></param>
        /// <param name="isError"></param>
        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(10, 0, false)]
        [TestCase(9, 0, false)]
        [TestCase(9, 1, false)]
        [TestCase(9, 2, true)]
        public static void AddRangeTest(int length, int count, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));

            var errorOccured = false;
            try
            {
                IEnumerable<MapEventPage> addItems = null;
                if (count == 0) addItems = new List<MapEventPage>();
                else if (count > 0) addItems = GenerateMapEventOnePageList(count);
                instance.AddRange(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// Insertメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="isAddNull"></param>
        /// <param name="isError"></param>
        [TestCase(1, 0, true, true)]
        [TestCase(1, 0, false, false)]
        [TestCase(1, 1, false, true)]
        [TestCase(9, 8, false, false)]
        [TestCase(9, 9, false, true)]
        [TestCase(10, 0, false, true)]
        [TestCase(10, 10, false, true)]
        public static void InsertTest(int length, int index, bool isAddNull, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));

            var errorOccured = false;
            try
            {
                var insertItem = isAddNull ? null : new MapEventPage();
                instance.Insert(index, insertItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// InsertRangeメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="isError"></param>
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 1, 1, true)]
        [TestCase(1, 0, 9, false)]
        [TestCase(1, 0, 10, true)]
        [TestCase(9, 1, 1, false)]
        [TestCase(9, 1, 2, true)]
        public static void InsertRangeTest(int length, int index, int count, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));

            var errorOccured = false;
            try
            {
                var insertItems = count == -1 ? null : GenerateMapEventOnePageList(count);
                instance.InsertRange(index, insertItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// Removeメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="removeNum">-1のとき除去項目=null、=lengthのとき除去項目=追加していないインスタンス</param>
        /// <param name="result"></param>
        /// <param name="isError"></param>
        [TestCase(1, -1, false, true)]
        [TestCase(1, 0, false, true)]
        [TestCase(2, -1, false, true)]
        [TestCase(2, 0, true, false)]
        [TestCase(2, 1, true, false)]
        [TestCase(2, 2, false, true)]
        public static void RemoveTest(int length, int removeNum, bool result, bool isError)
        {
            var pages = GenerateMapEventOnePageList(length).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].GraphicInfo.IsGraphicTileChip = true;
                pages[i].GraphicInfo.GraphicTileId = i;
            }

            var instance = new MapEventPageList(pages);

            var graphicTileId = (MapEventTileId) 99;

            var errorOccured = false;
            var removeTileId = MapEventTileId.NotUse;
            var isSuccess = false;
            try
            {
                MapEventPage removeItem = null;
                if (removeNum >= length)
                {
                    // 除去項目 = 追加していないインスタンス
                    removeItem = new MapEventPage
                    {
                        GraphicInfo = {GraphicTileId = graphicTileId}
                    };
                }
                else if (removeNum >= 0)
                {
                    removeItem = pages[removeNum];
                }

                if (removeItem != null) removeTileId = removeItem.GraphicInfo.GraphicTileId;
                isSuccess = instance.Remove(removeItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 成否フラグが正しいこと
            Assert.AreEqual(isSuccess, result);
            // 正しく除去されていること
            Assert.AreEqual(instance.Count, length - 1);
            for (var i = 0; i < instance.Count; i++)
            {
                Assert.AreNotEqual(instance.Get(i).GraphicInfo.GraphicTileId,
                    removeTileId);
            }
        }

        /// <summary>
        /// RemoveAtメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="isError"></param>
        [TestCase(1, 0, true)]
        [TestCase(2, -1, true)]
        [TestCase(2, 0, false)]
        [TestCase(2, 1, false)]
        [TestCase(2, 2, true)]
        [TestCase(10, -1, true)]
        [TestCase(10, 0, false)]
        [TestCase(10, 9, false)]
        [TestCase(10, 10, true)]
        public static void RemoveAtTest(int length, int index, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));

            var errorOccured = false;
            try
            {
                instance.RemoveAt(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// RemoveRangeメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="isError"></param>
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 1, 0, true)]
        [TestCase(1, 0, 1, true)]
        [TestCase(2, -1, 0, true)]
        [TestCase(2, 0, 0, false)]
        [TestCase(2, 0, 1, false)]
        [TestCase(2, 0, 2, true)]
        [TestCase(2, 1, 1, false)]
        [TestCase(2, 2, 0, true)]
        [TestCase(10, 0, 9, false)]
        [TestCase(10, 0, 10, true)]
        [TestCase(10, 1, 9, false)]
        [TestCase(10, 1, 10, true)]
        [TestCase(10, 9, 1, false)]
        [TestCase(10, 10, 1, true)]
        public static void RemoveRangeTest(int length, int index, int count, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));

            var errorOccured = false;
            try
            {
                instance.RemoveRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        /// <summary>
        /// Getメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="isError"></param>
        [TestCase(1, -1, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(10, 9, false)]
        [TestCase(10, 10, true)]
        public static void GetTest(int length, int index, bool isError)
        {
            var pages = GenerateMapEventOnePageList(length).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].GraphicInfo.IsGraphicTileChip = true;
                pages[i].GraphicInfo.GraphicTileId = i;
            }

            var instance = new MapEventPageList(pages);

            var errorOccured = false;
            MapEventPage getResult = null;
            try
            {
                getResult = instance.Get(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!isError)
            {
                // 取得した項目が正しいこと
                Assert.NotNull(getResult);
                Assert.AreEqual((int) getResult.GraphicInfo.GraphicTileId, index);
            }
        }

        /// <summary>
        /// GetRangeメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="isError"></param>
        [TestCase(1, -1, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, false)]
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 0, 2, true)]
        [TestCase(1, 1, 0, true)]
        [TestCase(10, 0, 10, false)]
        [TestCase(10, 0, 11, true)]
        [TestCase(10, 9, 1, false)]
        [TestCase(10, 10, 0, true)]
        public static void GetRangeTest(int length, int index, int count, bool isError)
        {
            var instance = new MapEventPageList(GenerateMapEventOnePageList(length));
            IEnumerable<MapEventPage> getRangeResult = null;

            var errorOccured = false;
            try
            {
                getRangeResult = instance.GetRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!isError)
            {
                // 取得件数が一致すること
                Assert.NotNull(getRangeResult);
                Assert.AreEqual(getRangeResult.Count(), count);
            }
        }

        /// <summary>
        /// GetAllメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        [TestCase(1)]
        [TestCase(10)]
        public static void GetAllTest(int length)
        {
            var pages = GenerateMapEventOnePageList(length).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].GraphicInfo.IsGraphicTileChip = true;
                pages[i].GraphicInfo.GraphicTileId = i;
            }

            var instance = new MapEventPageList(pages);

            var errorOccured = false;
            IEnumerable<MapEventPage> getResult = null;
            try
            {
                getResult = instance.GetAll();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが起こらないこと
            Assert.IsFalse(errorOccured);

            // 取得した項目がセットした値と一致すること
            var srcList = pages.ToList();
            var dstList = getResult.ToList();

            Assert.AreEqual(srcList.Count, dstList.Count);
            for (var i = 0; i < srcList.Count; i++)
            {
                Assert.AreEqual(srcList[i].GraphicInfo.GraphicTileId,
                    dstList[i].GraphicInfo.GraphicTileId);
            }
        }

        private static IEnumerable<MapEventPage> GenerateMapEventOnePageList(int length)
        {
            var list = new List<MapEventPage>();
            for (var i = 0; i < length; i++)
            {
                list.Add(new MapEventPage());
            }

            return list;
        }
    }
}