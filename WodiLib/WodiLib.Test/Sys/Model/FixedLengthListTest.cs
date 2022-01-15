using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
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

        [TestCase(-1, true)]
        [TestCase(0, true)]
        [TestCase(10, false)]
        public static void ConstructorTest1(int initLength, bool isError)
        {
            var initItems = initLength == -1
                ? null
                : MakeStringList(initLength);

            var errorOccured = false;

            IFixedLengthList<string> instance = null;
            try
            {
                instance = new CollectionTest1(initItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 初期要素が容量数と一致すること
            Assert.AreEqual(instance.Count, initLength);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(9, false)]
        [TestCase(10, true)]
        public static void IndexerGetTest(int index, bool isError)
        {
            var instance = MakeCollectionForMethodTest(
                10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );
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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
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
            var instance = MakeCollectionForMethodTest(
                10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );

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
                        Assert.AreEqual(
                            dic[nameof(NotifyCollectionChangedAction.Replace)].Count,
                            isError
                                ? 0
                                : 1
                        );
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(
                propertyChangingEventCalledCount[ListConstant.IndexerName],
                isError
                    ? 0
                    : 1
            );
            Assert.AreEqual(
                propertyChangedEventCalledCount[ListConstant.IndexerName],
                isError
                    ? 0
                    : 1
            );

            if (errorOccured) return;

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(
                    instance[i],
                    i != index
                        ? i.ToString()
                        : setItem
                );
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
            var instance = MakeCollectionForMethodTest(
                10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );
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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
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
            var instance = MakeCollectionForMethodTest(
                length,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );

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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(
                propertyChangingEventCalledCount[ListConstant.IndexerName],
                isError
                    ? 0
                    : 1
            );
            Assert.AreEqual(
                propertyChangedEventCalledCount[ListConstant.IndexerName],
                isError
                    ? 0
                    : 1
            );

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
            var instance = MakeCollectionForMethodTest(
                length,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );

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

            var isMoved = !isError && count > 0;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isMoved)
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
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        }
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(
                propertyChangingEventCalledCount[ListConstant.IndexerName],
                isMoved
                    ? 1
                    : 0
            );
            Assert.AreEqual(
                propertyChangedEventCalledCount[ListConstant.IndexerName],
                isMoved
                    ? 1
                    : 0
            );

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

        [Test]
        public static void ResetEmptyTest()
        {
            var initLength = 10;
            var instance = MakeCollectionForMethodTest(
                initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );

            // 各要素をデフォルト値以外に書き換える
            for (var i = 0; i < instance.Count; i++)
            {
                instance[i] = $"{instance[i]}_editValue";
            }

            // 要素書き換えで通知が起こるため通知情報クリア
            var keys = propertyChangingEventCalledCount.Keys.ToList();
            foreach (var key in keys)
            {
                propertyChangingEventCalledCount[key] = 0;
                propertyChangedEventCalledCount[key] = 0;
            }

            keys = collectionChangingEventArgsList.Keys.ToList();
            foreach (var key in keys)
            {
                collectionChangingEventArgsList[key].Clear();
                collectionChangedEventArgsList[key].Clear();
            }

            var errorOccured = false;
            try
            {
                instance.Reset();
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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 要素数が変化していないこと
            Assert.AreEqual(instance.Count, initLength);

            // すべての要素がデフォルト要素と一致すること
            foreach (var t in instance)
            {
                Assert.AreEqual(t, CollectionTest1.Default);
            }
        }

        private static readonly object[] ResetTestCaseSource =
        {
            new object[] { -1, true },
            new object[] { 9, true },
            new object[] { 10, false },
            new object[] { 11, true }
        };

        [TestCaseSource(nameof(ResetTestCaseSource))]
        public static void ResetTest(int resetItemLength, bool isError)
        {
            const int initLength = 10;
            var instance = MakeCollectionForMethodTest(
                initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );
            var resetItem = MakeStringList(resetItemLength)
                ?.Select(
                    s => s is null
                        ? null
                        : $"reset_{s}"
                )
                .ToList();

            var errorOccured = false;
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 各要素が初期化した要素と一致すること
            Assert.IsTrue(instance.SequenceEqual(resetItem));
        }

        [TestCase(10, "1", true)]
        [TestCase(10, "11", false)]
        [TestCase(10, null, false)]
        public static void ContainsTest(int initLength, string item, bool result)
        {
            var instance = MakeCollectionForMethodTest(
                initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );

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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = MakeCollectionForMethodTest(10, out _, out _, out _, out _);

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
            /*
             * NotifyCollectionChanging に登録したイベントの中でエラーを起こす。
             * NotifyCollectionChanging は勿論、NotifyPropertyChanging もイベントの検出ができないこと。
             */

            var instance = MakeCollection4ForOrdinalEventTest(
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount
            );

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
                    }
                );
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [Test]
        public static void DeepCloneTest()
        {
            const int initCount = 10;
            var instance = MakeCollection5(initCount);
            CollectionTest4 clone = null;

            var guidList = instance.Select(x => x.Guid).ToList();

            var errorOccured = false;
            try
            {
                clone = instance.DeepClone();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // ----- Original Test -----

            // 元のリストが変化していないこと
            Assert.AreEqual(instance.Count, guidList.Count);
            for (var i = 0; i < instance.Count; i++)
            {
                Assert.IsTrue(instance[i].Guid.Equals(guidList[i]));
            }

            // ----- Clone Test -----

            // 元とは異なる参照であること
            Assert.False(ReferenceEquals(clone, instance));

            // 元の要素数から変化していないこと
            Assert.AreEqual(clone.Count, initCount);

            // 各要素がディープクローンであること
            for (var i = 0; i < initCount; i++)
            {
                Assert.IsTrue(clone[i].Guid.Equals(guidList[i]));
                Assert.IsFalse(ReferenceEquals(instance[i], clone[i]));
            }
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

        private static CollectionTest1 MakeCollectionForMethodTest(
            int initLength,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount
        )
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new CollectionTest1(Array.Empty<string>())
                : new CollectionTest1(initStringList);

            // Observerに購読させないよう、イベントObserver登録より前に通知フラグ設定
            result.NotifyPropertyChangingEventType = NotifyPropertyChangeEventType.Enabled;
            result.NotifyPropertyChangedEventType = NotifyPropertyChangeEventType.Enabled;
            result.NotifyCollectionChangingEventType = NotifyCollectionChangeEventType.Single;
            result.NotifyCollectionChangedEventType = NotifyCollectionChangeEventType.Single;

            collectionChangingEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            propertyChangingEventCalledCount = MakePropertyChangedArgsDic();
            result.PropertyChanging += MakePropertyChangingEventHandler(propertyChangingEventCalledCount);

            propertyChangedEventCalledCount = MakePropertyChangedArgsDic();
            result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedEventCalledCount);

            return result;
        }

        private static CollectionTest3 MakeCollection4ForOrdinalEventTest(
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount
        )
        {
            var result = new CollectionTest3(MakeStringList(10))
            {
                // Observerに購読させないよう、イベントObserver登録より前に通知フラグ設定
                NotifyPropertyChangingEventType = NotifyPropertyChangeEventType.Enabled,
                NotifyPropertyChangedEventType = NotifyPropertyChangeEventType.Enabled,
                NotifyCollectionChangingEventType = NotifyCollectionChangeEventType.Single,
                NotifyCollectionChangedEventType = NotifyCollectionChangeEventType.Single
            };

            collectionChangingEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = MakeCollectionChangeEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            propertyChangingEventCalledCount = MakePropertyChangedArgsDic();
            result.PropertyChanging += MakePropertyChangingEventHandler(propertyChangingEventCalledCount);

            propertyChangedEventCalledCount = MakePropertyChangedArgsDic();
            result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedEventCalledCount);

            return result;
        }

        private static CollectionTest4 MakeCollection5(int length)
        {
            return new CollectionTest4(Enumerable.Repeat("", length).Select(_ => new TestClass()));
        }

        /// <summary>
        ///     CollectionChangedEventArgs を格納するための Dictionary インスタンスを生成する
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        private static Dictionary<string, List<NotifyCollectionChangedEventArgs>> MakeCollectionChangeEventArgsDic()
            => new()
            {
                { nameof(NotifyCollectionChangedAction.Replace), new List<NotifyCollectionChangedEventArgs>() },
                { nameof(NotifyCollectionChangedAction.Reset), new List<NotifyCollectionChangedEventArgs>() },
                { nameof(NotifyCollectionChangedAction.Move), new List<NotifyCollectionChangedEventArgs>() }
            };

        /// <summary>
        ///     CollectionChanging, CollectionChanged に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="isBefore">CollectionChanging にセットする場合true, CollectionChanged にセットする場合false</param>
        /// <param name="resultDic">発生したイベント引数を格納するDirectory</param>
        /// <returns>生成したインスタンス</returns>
        private static EventHandler<NotifyCollectionChangedEventArgsEx<string>> MakeCollectionChangeEventHandler(
            bool isBefore,
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> resultDic
        )
            => (_, args) =>
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
        ///     PropertyChanged が発火したプロパティ名/回数を格納するための Dictionary インスタンスを生成する。
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        private static Dictionary<string, int> MakePropertyChangedArgsDic() => new()
        {
            { ListConstant.IndexerName, 0 }
        };

        /// <summary>
        ///     PropertyChanging に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="resultDic">プロパティごとに通知された回数を格納するためのDictionary</param>
        /// <returns>生成したインスタンス</returns>
        private static PropertyChangingEventHandler
            MakePropertyChangingEventHandler(Dictionary<string, int> resultDic) =>
            (_, args) =>
            {
                resultDic[args.PropertyName] += 1;
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                logger.Debug("}");
            };

        /// <summary>
        ///     PropertyChanged に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="resultDic">プロパティごとに通知された回数を格納するためのDictionary</param>
        /// <returns>生成したインスタンス</returns>
        private static PropertyChangedEventHandler MakePropertyChangedEventHandler(Dictionary<string, int> resultDic) =>
            (_, args) =>
            {
                resultDic[args.PropertyName] += 1;
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                logger.Debug("}");
            };

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      テスト用クラス
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private abstract class AbsCollectionTest<T> : FixedLengthList<string, T>
            where T : AbsCollectionTest<T>
        {
            protected AbsCollectionTest(IEnumerable<string> list) : base(list)
            {
            }

            protected override int GetCapacity() => 10;
        }

        private class CollectionTest1 : AbsCollectionTest<CollectionTest1>
        {
            public static string Default => "test";

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest1(IEnumerable<string> list) : base(list)
            {
            }

            protected override CollectionTest1 MakeInstance(IEnumerable<string> items)
                => new(items);
        }

        private class CollectionTest3 : AbsCollectionTest<CollectionTest3>
        {
            protected override string MakeDefaultItem(int index) => "";

            public CollectionTest3(IEnumerable<string> list) : base(list)
            {
                CollectionChanging += OnCollectionChanging;
            }

            private void OnCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
            {
                throw new Exception();
            }

            protected override CollectionTest3 MakeInstance(IEnumerable<string> items)
                => new(items);
        }

        public class CollectionTest4 : FixedLengthList<TestClass, CollectionTest4>
        {
            public CollectionTest4(IEnumerable<TestClass> values) : base(values.Select(x => x.DeepClone()))
            {
            }

            protected override TestClass MakeDefaultItem(int index)
                => new();

            protected override CollectionTest4 MakeInstance(IEnumerable<TestClass> items)
                => new(items);

            protected override int GetCapacity() => 10;
        }

        public class TestClass : IEqualityComparable<TestClass>
        {
            public string Guid { get; private init; } = System.Guid.NewGuid().ToString();

            public bool ItemEquals(TestClass other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (ReferenceEquals(null, other)) return false;

                return Guid.Equals(other.Guid);
            }

            public bool ItemEquals(object other)
                => ItemEquals(other as TestClass);

            public TestClass DeepClone() => new()
            {
                Guid = Guid
            };
        }
    }
}
