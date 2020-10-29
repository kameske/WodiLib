using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
        [TestCase(TestClassType.Type3, 10, false)] // Ver2.4 ～ MakeDefaultItems の戻り値チェックはコンストラクタで実施しない
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
            var instance = MakeCollectionForMethodTest(10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);
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

            // 各イベントが一度も呼ばれていないこと
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
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
            var instance = MakeCollectionForMethodTest(10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);

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

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, isError ? 0 : 1);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

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
            var instance = MakeCollectionForMethodTest(10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);
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

            // 各イベントが一度も呼ばれていないこと
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

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
            var instance = MakeCollectionForMethodTest(length,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);

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

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isError)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Move)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, oldIndex);
                            Assert.AreEqual(eventArg.OldItems.Count, 1);
                            Assert.AreEqual(eventArg.NewStartingIndex, newIndex);
                            Assert.AreEqual(eventArg.NewItems.Count, 1);
                            Assert.IsTrue(ReferenceEquals(eventArg.OldItems[0], eventArg.NewItems[0]));
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isError)
            {
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

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
            var instance = MakeCollectionForMethodTest(length,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);

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

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isError)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Move)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, oldIndex);
                            Assert.AreEqual(eventArg.OldItems.Count, count);
                            Assert.AreEqual(eventArg.NewStartingIndex, newIndex);
                            Assert.AreEqual(eventArg.NewItems.Count, count);
                            for (var i = 0; i < count; i++)
                            {
                                Assert.IsTrue(ReferenceEquals(eventArg.OldItems[i], eventArg.NewItems[i]));
                            }
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isError)
            {
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

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
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);

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

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 1);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 要素数が容量と一致すること
            Assert.AreEqual(instance.Count, capacity);

            // すべての要素がデフォルト要素と一致すること
            foreach (var t in instance)
            {
                Assert.AreEqual(t, "test");
            }
        }

        private static readonly object[] ResetTestCaseSource =
        {
            new object[] {-1, true},
            new object[] {9, true},
            new object[] {10, false},
            new object[] {11, true},
        };

        [TestCaseSource(nameof(ResetTestCaseSource))]
        public static void ResetTest(int resetItemLength, bool isError)
        {
            const int initLength = 10;
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);
            var resetItem = MakeStringList(resetItemLength)?.Select(s => s is null ? null : $"reset_{s}").ToList();

            var errorOccured = false;
            try
            {
                instance.Reset(resetItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 要素数が初期化要素と一致すること
            Assert.AreEqual(instance.Count, resetItemLength);

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 1);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 各要素が初期化した要素と一致すること
            Assert.IsTrue(instance.SequenceEqual(resetItem));
        }

        [TestCase(10, "1", true)]
        [TestCase(10, "11", false)]
        [TestCase(10, null, false)]
        public static void ContainsTest(int initLength, string item, bool result)
        {
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);

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

            // 各イベントが呼ばれていないこと
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [TestCase("1", 1)]
        [TestCase("11", -1)]
        [TestCase(null, -1)]
        public static void IndexOfTest(string item, int result)
        {
            var instance = MakeCollectionForMethodTest(10, out _, out _, out _);

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
            var instance = MakeCollectionForMethodTest(10, out _, out _, out _);
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
            var instance = MakeCollectionForMethodTest(10, out _, out _, out _);

            // foreachを用いた処理で要素が正しく取得できること
            var i = 0;
            foreach (var value in instance)
            {
                Assert.AreEqual(value, i.ToString());
                i++;
            }
        }

        [Test]
        public static void CollectionChangingErrorTest()
        {
            var instance = MakeCollection4ForOrdinalEventTest(
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangedEventCalledCount);

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

            // 各イベントが呼ばれていないこと
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = MakeCollectionForMethodTest(10, out _, out _, out _);

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
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new CollectionTest1()
                : new CollectionTest1(initStringList);

            collectionChangingEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            propertyChangedEventCalledCount = MakePropertyChangedArgsDic();
            result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedEventCalledCount);

            return result;
        }

        private static CollectionTest4 MakeCollection4ForOrdinalEventTest(
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var result = new CollectionTest4();

            collectionChangingEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            propertyChangedEventCalledCount = MakePropertyChangedArgsDic();
            result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedEventCalledCount);

            return result;
        }

        /// <summary>
        /// CollectionChangedEventArgs を格納するための Dictionary インスタンスを生成する
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        private static Dictionary<string, List<NotifyCollectionChangedEventArgs>> MakeCollectionChangeEventArgsDic()
            => new Dictionary<string, List<NotifyCollectionChangedEventArgs>>
            {
                {nameof(NotifyCollectionChangedAction.Replace), new List<NotifyCollectionChangedEventArgs>()},
                {nameof(NotifyCollectionChangedAction.Reset), new List<NotifyCollectionChangedEventArgs>()},
                {nameof(NotifyCollectionChangedAction.Move), new List<NotifyCollectionChangedEventArgs>()},
            };

        /// <summary>
        /// CollectionChanging, CollectionChanged に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="isBefore">CollectionChanging にセットする場合true, CollectionChanged にセットする場合false</param>
        /// <param name="resultDic">発生したイベント引数を格納するDirectory</param>
        /// <returns>生成したインスタンス</returns>
        private static NotifyCollectionChangedEventHandler MakeCollectionChangeEventHandler(bool isBefore,
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> resultDic)
            => (sender, args) =>
            {
                resultDic[args.Action.ToString()].Add(args);
                logger.Debug($"Collection{(isBefore ? "Changing" : "Changed")} Event Raise. ");
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.Action)}: {args.Action}");
                logger.Debug($"    {nameof(args.OldStartingIndex)}: {args.OldStartingIndex}");
                logger.Debug($"    {nameof(args.OldItems)}: {args.OldItems}");
                logger.Debug($"    {nameof(args.NewStartingIndex)}: {args.NewStartingIndex}");
                logger.Debug($"    {nameof(args.NewItems)}: {args.NewItems}");
                logger.Debug("}");
            };

        /// <summary>
        /// PropertyChanged が発火したプロパティ名/回数を格納するための Dictionary インスタンスを生成する。
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        private static Dictionary<string, int> MakePropertyChangedArgsDic() => new Dictionary<string, int>
        {
            {ListConstant.IndexerName, 0},
        };

        /// <summary>
        /// PropertyChanged に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="resultDic">プロパティごとに通知された回数を格納するためのDictionary</param>
        /// <returns>生成したインスタンス</returns>
        private static PropertyChangedEventHandler MakePropertyChangedEventHandler(Dictionary<string, int> resultDic) =>
            (sender, args) =>
            {
                resultDic[args.PropertyName] += 1;
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                logger.Debug("}");
            };

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

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest1()
            {
            }

            public CollectionTest1(IReadOnlyCollection<string> list) : base(list)
            {
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
            public override int GetCapacity() => 10;

            protected override string MakeDefaultItem(int index) => "";

            public CollectionTest4()
            {
                CollectionChanging += OnCollectionChanging;
            }

            private void OnCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
            {
                throw new Exception();
            }
        }
    }
}
