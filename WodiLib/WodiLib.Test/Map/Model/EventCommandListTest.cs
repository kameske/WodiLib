using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Event.EventCommand;
using WodiLib.Map;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class EventCommandListTest
    {
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
            catch (Exception)
            {
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
        [TestCase(9999, false)]
        public static void AddTest(int length, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));

            var errorOccured = false;
            try
            {
                instance.Add(new Blank());
            }
            catch (Exception)
            {
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
        [TestCase(9999, 1, false)]
        public static void AddRangeTest(int length, int count, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));
            
            var errorOccured = false;
            try
            {
                IEnumerable<IEventCommand> addItems = null;
                if(count == 0) addItems = new List<IEventCommand>();
                else if (count > 0) addItems = GenerateEventCommandList(count);
                instance.AddRange(addItems);
            }
            catch (Exception)
            {
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
        [TestCase(9999, 999, false, false)]
        public static void InsertTest(int length, int index, bool isAddNull, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));
            
            var errorOccured = false;
            try
            {
                var insertItem = isAddNull ? null : new Blank();
                instance.Insert(index, insertItem);
            }
            catch (Exception)
            {
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
        [TestCase(1, 0, 10, false)]
        [TestCase(9, 1, 1, false)]
        [TestCase(9, 4, 2, false)]
        public static void InsertRangeTest(int length, int index, int count, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));
            
            var errorOccured = false;
            try
            {
                var insertItems = count == -1 ? null : GenerateEventCommandList(count);
                instance.InsertRange(index, insertItems);
            }
            catch (Exception)
            {
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
        [TestCase(1, 0, true, false)]
        [TestCase(2, -1, false, true)]
        [TestCase(2, 0, true, false)]
        [TestCase(2, 1, true, false)]
        [TestCase(2, 2, false, false)]
        [TestCase(255, 1, true, false)]
        public static void RemoveTest(int length, int removeNum, bool result, bool isError)
        {
            var pages = GenerateEventCommandList(length).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].Indent = (byte)i;
            }
            var instance = new EventCommandList(pages);
            
            var errorOccured = false;
            var removeTileId = -1;
            var isSuccess = false;
            try
            {
                IEventCommand removeItem = null;
                if (removeNum >= length)
                {
                    // 除去項目 = 追加していないインスタンス
                    removeItem = new Blank
                    {
                        Indent = 99
                    };
                }
                else if (removeNum >= 0)
                {
                    removeItem = pages[removeNum];
                }
                if (removeItem != null) removeTileId = removeItem.Indent;
                isSuccess = instance.Remove(removeItem);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!isError)
            {
                // 成否フラグが正しいこと
                Assert.AreEqual(isSuccess, result);
                // 正しく除去されていること
                var ansLength = length + (isSuccess ? -1 : 0);
                Assert.AreEqual(instance.Count, ansLength);
                for (var i = 0; i < instance.Count; i++)
                {
                    Assert.AreNotEqual(instance.Get(i).Indent, removeTileId);
                }
            }
        }

        /// <summary>
        /// RemoveAtメソッドテスト
        /// </summary>
        /// <param name="length"></param>
        /// <param name="index"></param>
        /// <param name="isError"></param>
        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(2, -1, true)]
        [TestCase(2, 0, false)]
        [TestCase(2, 1, false)]
        [TestCase(2, 2, true)]
        [TestCase(9999, 0, false)]
        public static void RemoveAtTest(int length, int index, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));
            
            var errorOccured = false;
            try
            {
                instance.RemoveAt(index);
            }
            catch (Exception)
            {
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
        [TestCase(1, 0, 1, false)]
        [TestCase(1, 0, 2, true)]
        [TestCase(2, -1, 0, true)]
        [TestCase(2, 0, 0, false)]
        [TestCase(2, 0, 2, false)]
        [TestCase(2, 0, 3, true)]
        [TestCase(2, 1, 1, false)]
        [TestCase(2, 2, 0, true)]
        [TestCase(9999, 0, 999, false)]
        public static void RemoveRangeTest(int length, int index, int count, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));
            
            var errorOccured = false;
            try
            {
                instance.RemoveRange(index, count);
            }
            catch (Exception)
            {
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
        [TestCase(9999, 18, false)]
        public static void GetTest(int length, int index, bool isError)
        {
            var pages = GenerateEventCommandList(length).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].Indent = (byte)i;
            }
            var instance = new EventCommandList(pages);
            
            var errorOccured = false;
            IEventCommand getResult = null;
            try
            {
                getResult = instance.Get(index);
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!isError)
            {
                // 取得した項目が正しいこと
                Assert.NotNull(getResult);
                Assert.AreEqual(getResult.Indent, index);
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
        [TestCase(9999, 10, 200, false)]
        public static void GetRangeTest(int length, int index, int count, bool isError)
        {
            var instance = new EventCommandList(GenerateEventCommandList(length));
            IEnumerable<IEventCommand> getRangeResult = null;
            
            var errorOccured = false;
            try
            {
                getRangeResult = instance.GetRange(index, count);
            }
            catch (Exception)
            {
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
        [TestCase(999)]
        public static void GetAllTest(int length)
        {
            var pages = GenerateEventCommandList(length).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].Indent = (byte)i;
            }
            var instance = new EventCommandList(pages);
            
            var errorOccured = false;
            IEnumerable<IEventCommand> getResult = null;
            try
            {
                getResult = instance.GetAll();
            }
            catch (Exception)
            {
                errorOccured = true;
            }

            // エラーが起こらないこと
            Assert.IsFalse(errorOccured);

            if (!errorOccured)
            {
                // 取得した項目がセットした値と一致すること
                var srcList = pages.ToList();
                var dstList = getResult.ToList();
                
                Assert.AreEqual(srcList.Count, dstList.Count);
                for (var i = 0; i < srcList.Count; i++)
                {
                    Assert.AreEqual(srcList[i].Indent, dstList[i].Indent);
                }
            }
        }
        
        private static IEnumerable<IEventCommand> GenerateEventCommandList(int length)
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