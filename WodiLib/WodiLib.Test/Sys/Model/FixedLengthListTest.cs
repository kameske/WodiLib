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

            IFixedLengthList<string> instance = null;
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

            IFixedLengthList<string> instance = null;
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
                out var propertyChangingEventCalledCount,
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
            var instance = MakeCollectionForMethodTest(10,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
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
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (errorOccured) return;

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(instance[i], i != index ? i.ToString() : setItem);
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
                out var propertyChangingEventCalledCount,
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
            var instance = MakeCollectionForMethodTest(length,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
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
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

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
                out var propertyChangingEventCalledCount,
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
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], isMoved ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isMoved ? 1 : 0);

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
                out var propertyChangingEventCalledCount,
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
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);
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
                out var propertyChangingEventCalledCount,
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
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
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
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [TestCase("1", 1)]
        [TestCase("11", -1)]
        [TestCase(null, -1)]
        public static void IndexOfTest(string item, int result)
        {
            var instance = MakeCollectionForMethodTest(10, out _, out _, out _, out _);

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
            var instance = MakeCollectionForMethodTest(10, out _, out _, out _, out _);
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
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [Test]
        public static void DeepCloneTest()
        {
            const int initCount = 10;
            var instance = MakeCollection5(initCount);
            CollectionTest5 clone = null;

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

        [Test]
        public static void DeepCloneWithTest()
        {
            const int initCount = 10;
            var instance = MakeCollection5(initCount);

            var guidList = instance.Select(x => x.Guid).ToList();

            IFixedLengthList<TestClass> clone = null;

            var errorOccured = false;
            try
            {
                clone = instance.DeepCloneWith(new Dictionary<int, TestClass>());
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
            for (var i = 0; i < Math.Min(initCount, clone.Count); i++)
            {
                Assert.IsTrue(clone[i].Guid.Equals(guidList[i]));
                Assert.IsFalse(ReferenceEquals(instance[i], clone[i]));
            }
        }

        private static readonly object[] DeepCloneWithTestCaseSource =
        {
            new object[] {null, false},
            new object[] {Array.Empty<KeyValuePair<int, TestClass>>(), false},
            new object[] {new KeyValuePair<int, TestClass>[] {new(-1, new TestClass())}, false},
            new object[] {new KeyValuePair<int, TestClass>[] {new(0, new TestClass())}, false},
            new object[] {new KeyValuePair<int, TestClass>[] {new(4, new TestClass())}, false},
            new object[] {new KeyValuePair<int, TestClass>[] {new(5, new TestClass())}, false},
            new object[]
                {new KeyValuePair<int, TestClass>[] {new(-1, new TestClass()), new(0, new TestClass())}, false},
            new object[] {new KeyValuePair<int, TestClass>[] {new(0, new TestClass()), new(4, new TestClass())}, false},
            new object[] {new KeyValuePair<int, TestClass>[] {new(1, null)}, true},
            new object[] {new KeyValuePair<int, TestClass>[] {new(3, new TestClass()), new(2, null)}, true},
        };

        [TestCaseSource(nameof(DeepCloneWithTestCaseSource))]
        public static void DeepCloneWithTest(IEnumerable<KeyValuePair<int, TestClass>> values, bool isError)
        {
            const int initCount = 10;
            var instance = MakeCollection5(initCount);

            var guidList = instance.Select(x => x.Guid).ToList();
            var valueList =
                new Dictionary<int, TestClass>(values?.ToArray() ?? Array.Empty<KeyValuePair<int, TestClass>>());

            IFixedLengthList<TestClass> clone = null;

            var errorOccured = false;
            try
            {
                clone = instance.DeepCloneWith(valueList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

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

            if (values is null)
            {
                // 各要素がディープクローンであること（既存要素のみ）
                for (var i = 0; i < Math.Min(initCount, clone.Count); i++)
                {
                    Assert.IsTrue(clone[i].Guid.Equals(guidList[i]));
                    Assert.IsFalse(ReferenceEquals(instance[i], clone[i]));
                }
            }
            else
            {
                // 指定された要素のみ変化していること
                var guidList2 = new List<string>(guidList);
                valueList.ForEach(pair =>
                {
                    var (key, value) = pair;
                    if (0 <= key && key < initCount) guidList2[key] = value.Guid;
                });

                for (var i = 0; i < Math.Min(initCount, clone.Count); i++)
                {
                    Assert.IsTrue(clone[i].Guid.Equals(guidList2[i]));
                }
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
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new CollectionTest1()
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

        private static CollectionTest4 MakeCollection4ForOrdinalEventTest(
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var result = new CollectionTest4
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

        public static CollectionTest5 MakeCollection5(int length)
        {
            return new(Enumerable.Repeat("", length).Select(_ => new TestClass()));
        }

        /// <summary>
        ///     CollectionChangedEventArgs を格納するための Dictionary インスタンスを生成する
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        private static Dictionary<string, List<NotifyCollectionChangedEventArgs>> MakeCollectionChangeEventArgsDic()
            => new()
            {
                {nameof(NotifyCollectionChangedAction.Replace), new List<NotifyCollectionChangedEventArgs>()},
                {nameof(NotifyCollectionChangedAction.Reset), new List<NotifyCollectionChangedEventArgs>()},
                {nameof(NotifyCollectionChangedAction.Move), new List<NotifyCollectionChangedEventArgs>()},
            };

        /// <summary>
        ///     CollectionChanging, CollectionChanged に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="isBefore">CollectionChanging にセットする場合true, CollectionChanged にセットする場合false</param>
        /// <param name="resultDic">発生したイベント引数を格納するDirectory</param>
        /// <returns>生成したインスタンス</returns>
        private static NotifyCollectionChangedEventHandler MakeCollectionChangeEventHandler(bool isBefore,
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> resultDic)
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
            {ListConstant.IndexerName, 0},
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

        public enum TestClassType
        {
            Type1,
            Type2,
            Type3
        }

        private abstract class AbsCollectionTest<T> : FixedLengthList<string, T>
            where T : AbsCollectionTest<T>
        {
            public AbsCollectionTest()
            {
            }

            public AbsCollectionTest(IEnumerable<string> list) : base(list)
            {
            }
        }

        private class CollectionTest1 : AbsCollectionTest<CollectionTest1>
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

            public CollectionTest1(IEnumerable<string> list) : base(list)
            {
            }

            public override CollectionTest1 DeepClone()
                => new(this);
        }

        private class CollectionTest2 : AbsCollectionTest<CollectionTest2>
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

            public override CollectionTest2 DeepClone()
            {
                throw new NotImplementedException();
            }
        }

        private class CollectionTest3 : AbsCollectionTest<CollectionTest3>
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

            public override CollectionTest3 DeepClone()
            {
                throw new NotImplementedException();
            }
        }

        private class CollectionTest4 : AbsCollectionTest<CollectionTest4>
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

            public override CollectionTest4 DeepClone()
            {
                throw new NotImplementedException();
            }
        }

        public class CollectionTest5 : FixedLengthList<TestClass, CollectionTest5>
        {
            public override int GetCapacity() => 10;

            public CollectionTest5()
            {
            }

            public CollectionTest5(IEnumerable<TestClass> values) : base(values.Select(x => x.DeepClone()))
            {
            }

            public override CollectionTest5 DeepClone()
                => new(this);

            protected override TestClass MakeDefaultItem(int index)
                => new();
        }

        public class TestClass : IEqualityComparable<TestClass>
        {
            public string Guid { get; private set; } = System.Guid.NewGuid().ToString();

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
                Guid = Guid,
            };
        }
    }
}
