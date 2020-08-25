using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class FixedLengthListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(TestClassType.Type1, false)]
        [TestCase(TestClassType.Type2, true)]
#if DEBUG
        [TestCase(TestClassType.Type3, true)]
#elif RELEASE
        [TestCase(TestClassType.Type3, false)]
#endif
        public static void ConstructorTest1(TestClassType testType, bool isError)
        {
            var initLength = 0;

            var errorOccured = false;

            AbsCollectionTest instance = null;
            try
            {
                switch (testType)
                {
                    case TestClassType.Type1:
                        initLength = CollectionTest1.Capacity;
                        instance = new CollectionTest1();
                        break;
                    case TestClassType.Type2:
                        initLength = CollectionTest2.Capacity;
                        instance = new CollectionTest2();
                        break;
                    case TestClassType.Type3:
                        initLength = CollectionTest3.Capacity;
                        instance = new CollectionTest3();
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 初期要素が容量最小数と一致すること
            Assert.AreEqual(instance.Count, initLength);
        }

        [TestCase(TestClassType.Type1, -1, true)]
        [TestCase(TestClassType.Type1, 0, true)]
        [TestCase(TestClassType.Type1, 10, false)]
        [TestCase(TestClassType.Type1, 11, true)]
        [TestCase(TestClassType.Type2, -1, true)]
        [TestCase(TestClassType.Type2, 0, true)]
        [TestCase(TestClassType.Type2, 10, true)]
        [TestCase(TestClassType.Type2, 11, true)]
        [TestCase(TestClassType.Type3, -1, true)]
        [TestCase(TestClassType.Type3, 0, true)]
#if DEBUG
        [TestCase(TestClassType.Type3, 10, true)]
#elif RELEASE
        [TestCase(TestClassType.Type3, 10, false)]
#endif
        [TestCase(TestClassType.Type3, 11, true)]
        public static void ConstructorTest2(TestClassType testType, int initLength, bool isError)
        {
            var errorOccured = false;

            var initList = MakeStringList(initLength);

            AbsCollectionTest instance = null;
            try
            {
                switch (testType)
                {
                    case TestClassType.Type1:
                        instance = new CollectionTest1(initList);
                        break;
                    case TestClassType.Type2:
                        instance = new CollectionTest2(initList);
                        break;
                    case TestClassType.Type3:
                        instance = new CollectionTest3(initList);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 要素数がセットした要素数と一致すること
            Assert.AreEqual(instance.Count, initLength);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(9, false)]
        [TestCase(10, true)]
        public static void IndexerGetTest(int index, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out var countDic);
            var errorOccured = false;
            try
            {
                var _ = instance[index];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが一度も呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 0);
        }

        [TestCase(-1, "abc", true)]
        [TestCase(0, "abc", false)]
        [TestCase(9, "abc", false)]
        [TestCase(10, "abc", true)]
        [TestCase(-1, null, true)]
        [TestCase(0, null, true)]
        [TestCase(9, null, true)]
        [TestCase(10, null, true)]
        public static void IndexerSetTest(int index, string setItem, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out var countDic);

            var errorOccured = false;
            try
            {
                instance[index] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 0);

            if (errorOccured) return;

            for (var i = 0; i < 10; i++)
            {
                if (i != index)
                {
                    // 初期値が変化していないこと
                    Assert.AreEqual(instance[i], i.ToString());
                }
                else
                {
                    // セットした値が反映されていること
                    Assert.AreEqual(instance[i], setItem);
                }
            }
        }

        // Count プロパティのテストは ConstructorTest1, ConstructorTest2 が兼ねる

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, 7, true)]
        [TestCase(-1, 8, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, 10, false)]
        [TestCase(0, 11, true)]
        [TestCase(9, -1, true)]
        [TestCase(9, 0, false)]
        [TestCase(9, 1, false)]
        [TestCase(9, 2, true)]
        [TestCase(10, -1, true)]
        [TestCase(10, 0, true)]
        [TestCase(10, 1, true)]
        public static void GetRangeTest(int index, int count, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out var countDic);
            var errorOccured = false;
            IEnumerable<string> result = null;

            try
            {
                result = instance.GetRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが一度も呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 0);

            if (errorOccured) return;

            var resultArray = result as string[] ?? result.ToArray();

            // 取得した要素数が一致すること
            Assert.AreEqual(resultArray.Length, count);

            // 取得した要素が取得元と一致すること
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(resultArray[i], instance[index + i]);
            }
        }

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, 9, true)]
        [TestCase(-1, 10, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, 9, false)]
        [TestCase(0, 10, true)]
        [TestCase(3, -1, true)]
        [TestCase(3, 0, false)]
        [TestCase(3, 2, false)]
        [TestCase(3, 3, false)]
        [TestCase(3, 4, false)]
        [TestCase(3, 9, false)]
        [TestCase(3, 10, true)]
        [TestCase(9, -1, true)]
        [TestCase(9, 0, false)]
        [TestCase(9, 9, false)]
        [TestCase(9, 10, true)]
        [TestCase(10, -1, true)]
        [TestCase(10, 0, true)]
        [TestCase(10, 9, true)]
        [TestCase(10, 10, true)]
        public static void MoveTest(int oldIndex, int newIndex, bool isError)
        {
            const int length = 10;
            var instance = MakeCollectionForMethodTest(length, out var countDic);

            var errorOccured = false;
            try
            {
                instance.Move(oldIndex, newIndex);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 0);

            if (errorOccured) return;

            if (oldIndex == newIndex)
            {
                // 移動元と移動先が同じ場合、並び順が変化していないこと
                for (var i = 0; i < length; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
            else if (oldIndex < newIndex)
            {
                // 後方へ移動させた場合
                // 移動させた要素以前の要素が変化していないこと
                var i = 0;
                for (; i < oldIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }

                // 移動元番号～移動先番号-1間の要素が一つ前にずれていること
                for (; i < newIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals((i + 1).ToString()));
                }

                // 移動先番号の要素が移動元要素と一致すること
                Assert.IsTrue(instance[i].Equals(oldIndex.ToString()));
                i++;

                // 移動先番号+1以降の要素が変化していないこと
                for (; i < length; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
            else
            {
                // 前方へ移動させた場合
                // 移動先要素以前の要素が変化していないこと
                var i = 0;
                for (; i < newIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }

                // 移動先番号の要素が移動元要素と一致すること
                Assert.IsTrue(instance[i].Equals(oldIndex.ToString()));
                i++;

                // 移動先番号+1～移動元番号間の要素が一つ後ろにずれていること
                for (; i <= oldIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals((i - 1).ToString()));
                }

                // 移動元番号以降の要素が変化していないこと
                for (; i < length; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
        }

        [TestCase(-1, -1, 0, true)]
        [TestCase(-1, 0, 0, true)]
        [TestCase(-1, 9, 0, true)]
        [TestCase(-1, 10, 0, true)]
        [TestCase(0, -1, 0, true)]
        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, false)]
        [TestCase(0, 0, 10, false)]
        [TestCase(0, 0, 11, true)]
        [TestCase(0, 9, -1, true)]
        [TestCase(0, 9, 0, false)]
        [TestCase(0, 9, 1, false)]
        [TestCase(0, 9, 2, true)]
        [TestCase(0, 10, -1, true)]
        [TestCase(0, 10, 0, false)]
        [TestCase(0, 10, 1, true)]
        [TestCase(3, -1, 0, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 7, false)]
        [TestCase(3, 0, 8, true)]
        [TestCase(3, 2, -1, true)]
        [TestCase(3, 2, 0, false)]
        [TestCase(3, 2, 7, false)]
        [TestCase(3, 2, 8, true)]
        [TestCase(3, 3, -1, true)]
        [TestCase(3, 3, 0, false)]
        [TestCase(3, 3, 7, false)]
        [TestCase(3, 3, 8, true)]
        [TestCase(3, 4, -1, true)]
        [TestCase(3, 4, 0, false)]
        [TestCase(3, 4, 6, false)]
        [TestCase(3, 4, 7, true)]
        [TestCase(3, 9, -1, true)]
        [TestCase(3, 9, 0, false)]
        [TestCase(3, 9, 1, false)]
        [TestCase(3, 9, 2, true)]
        [TestCase(3, 10, -1, true)]
        [TestCase(3, 10, 0, false)]
        [TestCase(3, 10, 1, true)]
        [TestCase(9, -1, 0, true)]
        [TestCase(9, 0, -1, true)]
        [TestCase(9, 0, 0, false)]
        [TestCase(9, 0, 1, false)]
        [TestCase(9, 0, 2, true)]
        [TestCase(9, 9, -1, true)]
        [TestCase(9, 9, 0, false)]
        [TestCase(9, 9, 1, false)]
        [TestCase(9, 9, 2, true)]
        [TestCase(9, 10, -1, true)]
        [TestCase(9, 10, 0, false)]
        [TestCase(9, 10, 1, true)]
        [TestCase(10, -1, 0, true)]
        [TestCase(10, 0, -1, true)]
        [TestCase(10, 0, 0, true)]
        [TestCase(10, 0, 1, true)]
        [TestCase(10, 9, -1, true)]
        [TestCase(10, 9, 0, true)]
        [TestCase(10, 9, 1, true)]
        [TestCase(10, 10, -1, true)]
        [TestCase(10, 10, 0, true)]
        [TestCase(10, 10, 1, true)]
        public static void MoveRangeTest(int oldIndex, int newIndex, int count, bool isError)
        {
            const int length = 10;
            var instance = MakeCollectionForMethodTest(length, out var countDic);

            var errorOccured = false;
            try
            {
                instance.MoveRange(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], isError ? 0 : count);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], isError ? 0 : count);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 0);

            if (errorOccured) return;

            if (oldIndex == newIndex)
            {
                // 移動元と移動先が同じ場合、並び順が変化していないこと
                for (var i = 0; i < length; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
            else if (oldIndex < newIndex)
            {
                // 後方へ移動させた場合
                // 移動させた要素以前の要素が変化していないこと
                var i = 0;
                for (; i < oldIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }

                // 移動元番号～移動先番号-1間の要素が移動させる要素数だけ前にずれていること
                for (; i < newIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals((i + count).ToString()));
                }

                // 移動先番号を始点に移動元要素と一致すること
                for (var j = 0; j < count; j++)
                {
                    Assert.IsTrue(instance[i].Equals((oldIndex + j).ToString()));
                    i++;
                }

                // 移動先番号+1以降の要素が変化していないこと
                for (; i < length; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
            else
            {
                // 前方へ移動させた場合
                // 移動先要素以前の要素が変化していないこと
                var i = 0;
                for (; i < newIndex; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }

                // 移動先番号を始点に移動元要素と一致すること
                for (var j = 0; j < count; j++)
                {
                    Assert.IsTrue(instance[i].Equals((oldIndex + j).ToString()));
                    i++;
                }

                // 移動先番号+count～移動元番号+count間の要素が移動させる要素数だけ後ろにずれていること
                for (; i < oldIndex + count; i++)
                {
                    Assert.IsTrue(instance[i].Equals((i - count).ToString()));
                }

                // 移動元番号+count以降の要素が変化していないこと
                for (; i < length; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
        }

        [TestCase(10)]
        public static void ClearTest(int initLength)
        {
            var instance = MakeCollectionForMethodTest(initLength, out var countDic);

            var capacity = instance.GetCapacity();

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 1);

            // 要素数が容量と一致すること
            Assert.AreEqual(instance.Count, capacity);

            // すべての要素がデフォルト要素と一致すること
            foreach (var t in instance)
            {
                Assert.AreEqual(t, "test");
            }
        }

        [TestCase(10, "1", true)]
        [TestCase(10, "11", false)]
        [TestCase(10, null, false)]
        public static void ContainsTest(int initLength, string item, bool result)
        {
            var instance = MakeCollectionForMethodTest(initLength, out var countDic);
            var containsResult = false;

            var errorOccured = false;
            try
            {
                containsResult = instance.Contains(item);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(containsResult, result);

            // 各Virtualメソッドが呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPreClearItemsCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnPostClearItemsCalled)], 0);
        }

        [TestCase("1", 1)]
        [TestCase("11", -1)]
        [TestCase(null, -1)]
        public static void IndexOfTest(string item, int result)
        {
            var instance = MakeCollectionForMethodTest(10, out _);
            var indexOfResult = -1;

            var errorOccured = false;
            try
            {
                indexOfResult = instance.IndexOf(item);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(indexOfResult, result);


            // 初期値が変化していないこと
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }
        }

        [TestCase(9, -1, true)]
        [TestCase(9, 0, true)]
        [TestCase(9, 1, true)]
        [TestCase(10, -1, true)]
        [TestCase(10, 0, false)]
        [TestCase(10, 1, true)]
        [TestCase(11, -1, true)]
        [TestCase(11, 0, false)]
        [TestCase(11, 1, false)]
        [TestCase(11, 2, true)]
        public static void CopyToTest(int arrayLength, int index, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out _);
            var copyArray = MakeStringArray(arrayLength);

            var errorOccured = false;
            try
            {
                instance.CopyTo(copyArray, index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 初期値が変化していないこと
            for (var j = 0; j < 10; j++)
            {
                Assert.AreEqual(instance[j], j.ToString());
            }

            if (errorOccured) return;

            // 配列の要素（コピー領域より前）が変化していないこと
            var i = 0;
            for (; i < index; i++)
            {
                Assert.AreEqual(copyArray[i], (i * 100).ToString());
            }

            // 配列の要素（コピーした領域）がコピーした内容で上書きされていること
            for (; i < 10; i++)
            {
                Assert.AreEqual(copyArray[i + index], i.ToString());
            }
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = MakeCollectionForMethodTest(10, out _);
            // foreachを用いた処理で要素が正しく取得できること
            var i = 0;
            foreach (var value in instance)
            {
                Assert.AreEqual(value, i.ToString());
                i++;
            }
        }

        [Test]
        public static void PreSetErrorTest()
        {
            var instance = MakeCollection4ForPrePostTest(out var methodCalledCount);
            instance.IsPreMethodRaiseError = true;

            var errorOccured = false;
            try
            {
                instance[0] = "new value";
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生していること
            Assert.IsTrue(errorOccured);

            // PostSetが実行されていないこと
            Assert.IsTrue(methodCalledCount[nameof(CollectionTest1.OnPostSetItemCalled)] == 0);
        }

        [Test]
        public static void PreMoveErrorTest()
        {
            var instance = MakeCollection4ForPrePostTest(out var methodCalledCount);
            instance.IsPreMethodRaiseError = true;

            var errorOccured = false;
            try
            {
                instance.Move(0, 1);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生していること
            Assert.IsTrue(errorOccured);

            // PostMoveが実行されていないこと
            Assert.IsTrue(methodCalledCount[nameof(CollectionTest1.OnPostMoveItemCalled)] == 0);
        }

        [Test]
        public static void PreClearErrorTest()
        {
            var instance = MakeCollection4ForPrePostTest(out var methodCalledCount);
            instance.IsPreMethodRaiseError = true;

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生していること
            Assert.IsTrue(errorOccured);

            // PostClearが実行されていないこと
            Assert.IsTrue(methodCalledCount[nameof(CollectionTest1.OnPostClearItemsCalled)] == 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = MakeCollectionForMethodTest(10, out _);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      テストデータ
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static List<string> MakeStringList(int length)
        {
            if (length < 0) return null;
            var result = new List<string>();
            for (var i = 0; i < length; i++)
            {
                result.Add(i.ToString());
            }

            return result;
        }

        private static string[] MakeStringArray(int length)
        {
            var result = new string[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = (i * 100).ToString();
            }

            return result;
        }

        private static CollectionTest1 MakeCollectionForMethodTest(int initLength,
            out Dictionary<string, int> methodCalledCount)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new CollectionTest1()
                : new CollectionTest1(initStringList);

            methodCalledCount = new Dictionary<string, int>
            {
                {nameof(CollectionTest1.OnPreSetItemCalled), 0},
                {nameof(CollectionTest1.OnPreMoveItemCalled), 0},
                {nameof(CollectionTest1.OnPreClearItemsCalled), 0},
                {nameof(CollectionTest1.OnPostSetItemCalled), 0},
                {nameof(CollectionTest1.OnPostMoveItemCalled), 0},
                {nameof(CollectionTest1.OnPostClearItemsCalled), 0},
            };

            var ints = methodCalledCount;
            result.OnPreSetItemCalled = () => ints[nameof(CollectionTest1.OnPreSetItemCalled)]++;
            result.OnPreMoveItemCalled = () => ints[nameof(CollectionTest1.OnPreMoveItemCalled)]++;
            result.OnPreClearItemsCalled = () => ints[nameof(CollectionTest1.OnPreClearItemsCalled)]++;
            result.OnPostSetItemCalled = () => ints[nameof(CollectionTest1.OnPostSetItemCalled)]++;
            result.OnPostMoveItemCalled = () => ints[nameof(CollectionTest1.OnPostMoveItemCalled)]++;
            result.OnPostClearItemsCalled = () => ints[nameof(CollectionTest1.OnPostClearItemsCalled)]++;

            return result;
        }

        private static CollectionTest4 MakeCollection4ForPrePostTest(
            out Dictionary<string, int> methodCalledCount)
        {
            var result = new CollectionTest4();

            methodCalledCount = new Dictionary<string, int>
            {
                {nameof(CollectionTest1.OnPostSetItemCalled), 0},
                {nameof(CollectionTest1.OnPostMoveItemCalled), 0},
                {nameof(CollectionTest1.OnPostClearItemsCalled), 0},
            };

            var ints = methodCalledCount;
            result.OnPostSetItemCalled = () => ints[nameof(CollectionTest1.OnPostSetItemCalled)]++;
            result.OnPostMoveItemCalled = () => ints[nameof(CollectionTest1.OnPostMoveItemCalled)]++;
            result.OnPostClearItemsCalled = () => ints[nameof(CollectionTest1.OnPostClearItemsCalled)]++;

            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      テスト用クラス
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public enum TestClassType
        {
            Type1,
            Type2,
            Type3
        }

        private abstract class AbsCollectionTest : FixedLengthList<string>
        {
            public AbsCollectionTest()
            {
            }

            public AbsCollectionTest(IReadOnlyCollection<string> list) : base(list)
            {
            }

            // FixedLengthList<T>継承クラスはデシリアライズ時に呼び出せるconstructor(SerializationInfo, StreamingContext)が必須
            protected AbsCollectionTest(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        private class CollectionTest1 : AbsCollectionTest
        {
            /**
             * 正常設定
             */

            public static int Capacity => 10;

            public static string Default => "test";

            [field: NonSerialized] public Action OnPreSetItemCalled { get; set; }
            [field: NonSerialized] public Action OnPreMoveItemCalled { get; set; }
            [field: NonSerialized] public Action OnPreClearItemsCalled { get; set; }
            [field: NonSerialized] public Action OnPostSetItemCalled { get; set; }
            [field: NonSerialized] public Action OnPostMoveItemCalled { get; set; }
            [field: NonSerialized] public Action OnPostClearItemsCalled { get; set; }

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest1()
            {
            }

            public CollectionTest1(IReadOnlyCollection<string> list) : base(list)
            {
            }

            protected override void PreSetItem(int index, string item)
            {
                OnPreSetItemCalled?.Invoke();
            }

            protected override void PreMoveItem(int oldIndex, int newIndex)
            {
                OnPreMoveItemCalled?.Invoke();
            }

            protected override void PreClearItems()
            {
                OnPreClearItemsCalled?.Invoke();
            }

            protected override void PostSetItem(int index, string item)
            {
                OnPostSetItemCalled?.Invoke();
            }

            protected override void PostMoveItem(int oldIndex, int newIndex)
            {
                OnPostMoveItemCalled?.Invoke();
            }

            protected override void PostClearItems()
            {
                OnPostClearItemsCalled?.Invoke();
            }

            protected CollectionTest1(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        private class CollectionTest2 : AbsCollectionTest
        {
            /*
             * 異常設定（Capacity < 0）
             */

            public static int Capacity => -2;
            public static string Default => "test";

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest2()
            {
            }

            public CollectionTest2(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class CollectionTest3 : AbsCollectionTest
        {
            /**
             * 異常設定（DefaultValue＝null）
             */
            public static int Capacity => 10;

            public static string Default => null;

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest3()
            {
            }

            public CollectionTest3(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class CollectionTest4 : AbsCollectionTest
        {
            /**
             * Pre, Post メソッドテスト用
             */
            public override int GetCapacity() => 10;

            protected override string MakeDefaultItem(int index) => "";

            public Action OnPostSetItemCalled { get; set; }
            public Action OnPostMoveItemCalled { get; set; }
            public Action OnPostClearItemsCalled { get; set; }

            /**
             * 初期化中にPreメソッドで例外発生するとテストにならないため初期化後だけエラー発生させる
             */
            public bool IsPreMethodRaiseError { get; set; }

            protected override void PreSetItem(int index, string item)
            {
                if (IsPreMethodRaiseError)
                {
                    throw new Exception();
                }
            }

            protected override void PreMoveItem(int oldIndex, int newIndex)
            {
                if (IsPreMethodRaiseError)
                {
                    throw new Exception();
                }
            }

            protected override void PreClearItems()
            {
                if (IsPreMethodRaiseError)
                {
                    throw new Exception();
                }
            }

            protected override void PostSetItem(int index, string item)
            {
                OnPostSetItemCalled?.Invoke();
            }

            protected override void PostMoveItem(int oldIndex, int newIndex)
            {
                OnPostMoveItemCalled?.Invoke();
            }

            protected override void PostClearItems()
            {
                OnPostClearItemsCalled?.Invoke();
            }
        }
    }
}
