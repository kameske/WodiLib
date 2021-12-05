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
    public class RestrictedCapacityListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(TestClassType.Type1, false, false)]
        [TestCase(TestClassType.Type2, false, false)]
        [TestCase(TestClassType.Type3, false, false)]
#if DEBUG
        [TestCase(TestClassType.Type4, true, true)]
        [TestCase(TestClassType.Type5, true, true)]
        [TestCase(TestClassType.Type6, false, false)] // MakeDefaultValue が null を返却してもコンストラクタ中ではエラー発生しない
#elif RELEASE
        [TestCase(TestClassType.Type4, false, true)]
        [TestCase(TestClassType.Type5, false, false)]
        [TestCase(TestClassType.Type6, false, false)]
#endif
        public static void ConstructorTest1(TestClassType testType, bool isError, bool isErrorState)
        {
            var initLength = 0;

            var errorOccured = false;

            IRestrictedCapacityList<string> instance = null;
            try
            {
                switch (testType)
                {
                    case TestClassType.Type1:
                        initLength = ListTest1.MinCapacity;
                        instance = new ListTest1();
                        break;
                    case TestClassType.Type2:
                        initLength = ListTest2.MinCapacity;
                        instance = new ListTest2();
                        break;
                    case TestClassType.Type3:
                        initLength = ListTest3.MinCapacity;
                        instance = new ListTest3();
                        break;
                    case TestClassType.Type4:
                        initLength = ListTest4.MinCapacity;
                        instance = new ListTest4();
                        break;
                    case TestClassType.Type5:
                        initLength = ListTest5.MinCapacity;
                        instance = new ListTest5();
                        break;
                    case TestClassType.Type6:
                        initLength = ListTest6.MinCapacity;
                        instance = new ListTest6();
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
            Assert.AreEqual(instance.Count != initLength, isErrorState);
        }

        [TestCase(TestClassType.Type1, -1, true)]
        [TestCase(TestClassType.Type1, 0, false)]
        [TestCase(TestClassType.Type1, 10, false)]
        [TestCase(TestClassType.Type1, 11, true)]
        [TestCase(TestClassType.Type2, -1, true)]
        [TestCase(TestClassType.Type2, 4, true)]
        [TestCase(TestClassType.Type2, 5, false)]
        [TestCase(TestClassType.Type2, 10, false)]
        [TestCase(TestClassType.Type2, 11, true)]
        [TestCase(TestClassType.Type3, -1, true)]
        [TestCase(TestClassType.Type3, 9, true)]
        [TestCase(TestClassType.Type3, 10, false)]
        [TestCase(TestClassType.Type3, 11, true)]
        [TestCase(TestClassType.Type4, -1, true)]
#if DEBUG
        [TestCase(TestClassType.Type4, 0, true)]
        [TestCase(TestClassType.Type4, 10, true)]
#elif RELEASE
        [TestCase(TestClassType.Type4, 0, false)]
        [TestCase(TestClassType.Type4, 10, false)]
#endif
        [TestCase(TestClassType.Type4, 11, true)]
        [TestCase(TestClassType.Type5, -1, true)]
        [TestCase(TestClassType.Type5, 0, true)]
        [TestCase(TestClassType.Type5, 10, true)]
        [TestCase(TestClassType.Type5, 11, true)]
        [TestCase(TestClassType.Type6, -1, true)]
        [TestCase(TestClassType.Type6, 0, false)] // Ver 2.4 ~ コンストラクタではデフォルト値の null チェックを行わない
        [TestCase(TestClassType.Type6, 10, true)] // Ver 3.0 ~ 処理の都合上初期要素がある場合はデフォルト値の null チェックを行う
        [TestCase(TestClassType.Type6, 11, true)]
        public static void ConstructorTest2(TestClassType testType, int initLength, bool isError)
        {
            var errorOccured = false;

            var initList = MakeStringList(initLength);

            IRestrictedCapacityList<string> instance = null;
            try
            {
                switch (testType)
                {
                    case TestClassType.Type1:
                        instance = new ListTest1(initList);
                        break;
                    case TestClassType.Type2:
                        instance = new ListTest2(initList);
                        break;
                    case TestClassType.Type3:
                        instance = new ListTest3(initList);
                        break;
                    case TestClassType.Type4:
                        instance = new ListTest4(initList);
                        break;
                    case TestClassType.Type5:
                        instance = new ListTest5(initList);
                        break;
                    case TestClassType.Type6:
                        instance = new ListTest6(initList);
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

        [TestCase(-1, 7, true)]
        [TestCase(0, 7, false)]
        [TestCase(6, 7, false)]
        [TestCase(7, 7, true)]
        public static void IndexerGetTest(int index, int initLength, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [TestCase(-1, "abc", 7, true)]
        [TestCase(0, "abc", 7, false)]
        [TestCase(6, "abc", 7, false)]
        [TestCase(7, "abc", 7, true)]
        [TestCase(-1, null, 7, true)]
        [TestCase(0, null, 7, true)]
        [TestCase(6, null, 7, true)]
        [TestCase(7, null, 7, true)]
        public static void IndexerSetTest(int index, string setItem, int initLength, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (errorOccured) return;

            for (var i = 0; i < initLength; i++)
            {
                Assert.AreEqual(instance[i], i != index ? i.ToString() : setItem);
            }
        }

        // Count プロパティのテストは ConstructorTest1, ConstructorTest2 が兼ねる

        [TestCase(-1, -1, 7, true)]
        [TestCase(-1, 0, 7, true)]
        [TestCase(-1, 7, 7, true)]
        [TestCase(-1, 8, 7, true)]
        [TestCase(0, -1, 7, true)]
        [TestCase(0, 0, 7, false)]
        [TestCase(0, 7, 7, false)]
        [TestCase(0, 8, 7, true)]
        [TestCase(6, -1, 7, true)]
        [TestCase(6, 0, 7, false)]
        [TestCase(6, 1, 7, false)]
        [TestCase(6, 2, 7, true)]
        [TestCase(7, -1, 7, true)]
        [TestCase(7, 0, 7, true)]
        [TestCase(7, 1, 7, true)]
        [TestCase(7, 2, 7, true)]
        public static void GetRangeTest(int index, int count, int initLength, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
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

        [TestCase("", 0, false)]
        [TestCase("", 9, false)]
        [TestCase("", 10, true)]
        [TestCase(null, 0, true)]
        [TestCase(null, 9, true)]
        [TestCase(null, 10, true)]
        public static void AddTest(string item, int initLength, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.Add(item);
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isError)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                            var addEventArg = dic[nameof(NotifyCollectionChangedAction.Add)][0];
                            Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                            Assert.AreEqual(addEventArg.OldItems, null);
                            Assert.AreEqual(addEventArg.NewStartingIndex, initLength);
                            Assert.AreEqual(addEventArg.NewItems.Count, 1);
                            Assert.AreEqual(addEventArg.NewItems[0], item);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isError)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

            if (errorOccured) return;

            // 追加後の要素数が元の要素数+1であること
            Assert.AreEqual(instance.Count, initLength + 1);

            // 初期要素が変化していないこと
            for (var i = 0; i < initLength; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 末尾に意図した値が追加されていること
            Assert.AreEqual(instance[initLength], item);
        }

        [TestCase(0, -1, false, true)]
        [TestCase(0, 0, false, false)]
        [TestCase(0, 10, false, false)]
        [TestCase(0, 10, true, true)]
        [TestCase(0, 11, false, true)]
        [TestCase(0, 11, true, true)]
        [TestCase(7, -1, false, true)]
        [TestCase(7, 0, false, false)]
        [TestCase(7, 3, false, false)]
        [TestCase(7, 3, true, true)]
        [TestCase(7, 4, false, true)]
        [TestCase(7, 4, true, true)]
        public static void AddRangeTest(int initLength, int addLength, bool hasNullInAddLength, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount);
            var addList = MakeStringList2(addLength, hasNullInAddLength);
            var errorOccured = false;

            try
            {
                instance.AddRange(addList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            var isChanged = !isError && addLength > 0;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isChanged)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                            var addEventArg = dic[nameof(NotifyCollectionChangedAction.Add)][0];
                            Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                            Assert.AreEqual(addEventArg.OldItems, null);
                            Assert.AreEqual(addEventArg.NewStartingIndex, initLength);
                            Assert.AreEqual(addEventArg.NewItems.Count, addLength);
                            Assert.IsTrue(addEventArg.NewItems.Cast<string>().SequenceEqual(addList));
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            // 追加後の要素数が元の要素数+追加した要素数であること
            Assert.AreEqual(instance.Count, initLength + addLength);

            // 初期要素が変化していないこと
            for (var i = 0; i < initLength; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 末尾に意図した値が追加されていること
            for (var i = 0; i < addLength; i++)
            {
                Assert.AreEqual(instance[initLength + i], (i * 100).ToString());
            }
        }

        [TestCase(-1, "", 0, true)]
        [TestCase(-1, "", 9, true)]
        [TestCase(-1, "", 10, true)]
        [TestCase(-1, null, 0, true)]
        [TestCase(-1, null, 9, true)]
        [TestCase(-1, null, 10, true)]
        [TestCase(0, "", 0, false)]
        [TestCase(0, "", 9, false)]
        [TestCase(0, "", 10, true)]
        [TestCase(0, null, 0, true)]
        [TestCase(0, null, 9, true)]
        [TestCase(0, null, 10, true)]
        [TestCase(9, "", 0, true)]
        [TestCase(9, "", 9, false)]
        [TestCase(9, "", 10, true)]
        [TestCase(9, null, 0, true)]
        [TestCase(9, null, 9, true)]
        [TestCase(9, null, 10, true)]
        [TestCase(10, "", 0, true)]
        [TestCase(10, "", 9, true)]
        [TestCase(10, "", 10, true)]
        [TestCase(10, null, 0, true)]
        [TestCase(10, null, 9, true)]
        [TestCase(10, null, 10, true)]
        public static void InsertTest(int index, string item, int initLength, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.Insert(index, item);
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isError)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                            var addEventArg = dic[nameof(NotifyCollectionChangedAction.Add)][0];
                            Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                            Assert.AreEqual(addEventArg.OldItems, null);
                            Assert.AreEqual(addEventArg.NewStartingIndex, index);
                            Assert.AreEqual(addEventArg.NewItems.Count, 1);
                            Assert.AreEqual(addEventArg.NewItems[0], item);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isError)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

            if (errorOccured) return;

            // 追加後の要素数が元の要素数+1であること
            Assert.AreEqual(instance.Count, initLength + 1);

            // 初期要素（挿入位置より前）が変化していないこと
            for (var i = 0; i < index; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 挿入位置に意図した値が追加されていること
            Assert.AreEqual(instance[index], item);

            // 初期要素（挿入位置より後）が変化していないこと
            for (var i = index + 1; i < initLength + 1; i++)
            {
                Assert.AreEqual(instance[i], (i - 1).ToString());
            }
        }

        [TestCase(0, -1, -1, false, true)]
        [TestCase(0, -1, 0, false, true)]
        [TestCase(0, -1, 10, false, true)]
        [TestCase(0, -1, 10, true, true)]
        [TestCase(0, -1, 11, false, true)]
        [TestCase(0, -1, 11, true, true)]
        [TestCase(0, -1, -1, false, true)]
        [TestCase(0, 0, 0, false, false)]
        [TestCase(0, 0, 10, false, false)]
        [TestCase(0, 0, 10, true, true)]
        [TestCase(0, 0, 11, false, true)]
        [TestCase(0, 0, 11, true, true)]
        [TestCase(0, 1, 0, false, true)]
        [TestCase(0, 1, 10, false, true)]
        [TestCase(0, 1, 10, true, true)]
        [TestCase(0, 1, 11, false, true)]
        [TestCase(0, 1, 11, true, true)]
        [TestCase(7, -1, -1, false, true)]
        [TestCase(7, -1, 0, false, true)]
        [TestCase(7, -1, 3, false, true)]
        [TestCase(7, -1, 3, true, true)]
        [TestCase(7, -1, 4, false, true)]
        [TestCase(7, -1, 4, true, true)]
        [TestCase(7, 0, -1, false, true)]
        [TestCase(7, 0, 0, false, false)]
        [TestCase(7, 0, 3, false, false)]
        [TestCase(7, 0, 3, true, true)]
        [TestCase(7, 0, 4, false, true)]
        [TestCase(7, 0, 4, true, true)]
        [TestCase(7, 7, -1, false, true)]
        [TestCase(7, 7, 0, false, false)]
        [TestCase(7, 7, 3, false, false)]
        [TestCase(7, 7, 3, true, true)]
        [TestCase(7, 7, 4, false, true)]
        [TestCase(7, 7, 4, true, true)]
        [TestCase(7, 8, -1, false, true)]
        [TestCase(7, 8, 0, false, true)]
        [TestCase(7, 8, 3, false, true)]
        [TestCase(7, 8, 3, true, true)]
        [TestCase(7, 8, 4, false, true)]
        [TestCase(7, 8, 4, true, true)]
        public static void InsertRangeTest(int initLength, int index, int addLength, bool hasNullInAddLength,
            bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount);
            var addList = MakeStringList2(addLength, hasNullInAddLength);
            var errorOccured = false;

            try
            {
                instance.InsertRange(index, addList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            var isChanged = !isError && addLength > 0;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isChanged)
                        {
                            Assert.AreEqual(
                                collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                            var addEventArg =
                                collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                            Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                            Assert.AreEqual(addEventArg.OldItems, null);
                            Assert.AreEqual(addEventArg.NewStartingIndex, index);
                            Assert.AreEqual(addEventArg.NewItems.Count, addLength);
                            Assert.IsTrue(addEventArg.NewItems.Cast<string>().SequenceEqual(addList));
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            // 追加後の要素数が元の要素数+追加した要素数であること
            Assert.AreEqual(instance.Count, initLength + addLength);

            // 初期要素（挿入位置より前）が変化していないこと
            for (var i = 0; i < index; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 挿入位置に意図した値が追加されていること
            for (var i = 0; i < addLength; i++)
            {
                Assert.AreEqual(instance[i + index], (i * 100).ToString());
            }

            // 初期要素（挿入位置より後）が変化していないこと
            for (var i = index + addLength; i < initLength + addLength; i++)
            {
                Assert.AreEqual(instance[i], (i - addLength).ToString());
            }
        }

        [TestCase(0, -1, -1, false, true)]
        [TestCase(0, -1, 0, false, true)]
        [TestCase(0, -1, 1, false, true)]
        [TestCase(0, -1, 1, true, true)]
        [TestCase(0, 0, -1, false, true)]
        [TestCase(0, 0, 0, false, false)]
        [TestCase(0, 0, 1, false, false)]
        [TestCase(0, 0, 1, true, true)]
        [TestCase(0, 0, 10, false, false)]
        [TestCase(0, 0, 10, true, true)]
        [TestCase(0, 0, 11, false, true)]
        [TestCase(0, 0, 11, true, true)]
        [TestCase(0, 1, -1, false, true)]
        [TestCase(0, 1, 0, false, true)]
        [TestCase(0, 1, 1, false, true)]
        [TestCase(0, 1, 1, true, true)]
        [TestCase(0, 1, 10, false, true)]
        [TestCase(0, 1, 10, true, true)]
        [TestCase(0, 1, 11, false, true)]
        [TestCase(0, 1, 11, true, true)]
        [TestCase(10, -1, -1, false, true)]
        [TestCase(10, -1, 0, false, true)]
        [TestCase(10, -1, 1, false, true)]
        [TestCase(10, -1, 1, true, true)]
        [TestCase(10, 0, -1, false, true)]
        [TestCase(10, 0, 0, false, false)]
        [TestCase(10, 0, 1, false, false)]
        [TestCase(10, 0, 1, true, true)]
        [TestCase(10, 0, 10, false, false)]
        [TestCase(10, 0, 10, true, true)]
        [TestCase(10, 0, 11, false, true)]
        [TestCase(10, 0, 11, true, true)]
        [TestCase(10, 10, -1, false, true)]
        [TestCase(10, 10, 0, false, false)]
        [TestCase(10, 10, 1, false, true)]
        [TestCase(10, 10, 1, true, true)]
        [TestCase(10, 11, -1, false, true)]
        [TestCase(10, 11, 0, false, true)]
        [TestCase(10, 11, 1, false, true)]
        [TestCase(10, 11, 1, true, true)]
        public static void OverwriteTest(int initLength, int index, int overwriteLength, bool hasNullInAddLength,
            bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength,
                out var collectionChangingEventArgsList,
                out var collectionChangedEventArgsList,
                out var propertyChangingEventCalledCount,
                out var propertyChangedEventCalledCount);
            var addList = MakeStringList2(overwriteLength, hasNullInAddLength);
            var errorOccured = false;

            try
            {
                instance.Overwrite(index, addList);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            var isChanged = !isError && overwriteLength > 0;
            var replaceMax = initLength - index;
            var replaceCnt = Math.Min(replaceMax, overwriteLength);
            var addStartIndex = index + replaceCnt;
            var addCnt = overwriteLength - replaceCnt;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isChanged)
                        {
                            if (replaceCnt > 0)
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count,
                                    1);
                                {
                                    var eventArg = dic[nameof(NotifyCollectionChangedAction.Replace)][0];
                                    Assert.AreEqual(eventArg.OldStartingIndex, index);
                                    Assert.AreEqual(eventArg.OldItems.Count, replaceCnt);
                                    Assert.AreEqual(eventArg.NewStartingIndex, index);
                                    Assert.AreEqual(eventArg.NewItems.Count, replaceCnt);
                                }
                            }
                            else // replaceCnt == 0
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                            }

                            if (addCnt > 0)
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                                {
                                    var addEventArg = dic[nameof(NotifyCollectionChangedAction.Add)][0];
                                    Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                                    Assert.IsNull(addEventArg.OldItems);
                                    Assert.AreEqual(addEventArg.NewStartingIndex, addStartIndex);
                                    Assert.AreEqual(addEventArg.NewItems.Count, addCnt);
                                }
                            }
                            else // addCnt == 0
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                            }
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], addCnt > 0 ? 1 : 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], addCnt > 0 ? 1 : 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            var answerSetCount = initLength - index;
            if (answerSetCount > overwriteLength) answerSetCount = overwriteLength;
            var answerInsertCount = overwriteLength - answerSetCount;
            if (answerInsertCount < 0) answerInsertCount = 0;

            // 追加後の要素数が元の要素数+追加した要素数であること
            Assert.AreEqual(instance.Count, initLength + answerInsertCount);

            // 初期要素（上書き位置より前）が変化していないこと
            for (var i = 0; i < index; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 上書きが反映されていること
            for (var i = 0; i < answerSetCount; i++)
            {
                Assert.AreEqual(instance[i + index], (i * 100).ToString());
            }

            // 初期要素（上書き位置より後）が変化していないこと
            for (var i = index + answerSetCount; i < initLength; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 追加要素が反映されていること
            for (var i = initLength; i < instance.Count; i++)
            {
                Assert.AreEqual(instance[i], (i * 100).ToString());
            }
        }

        [TestCase(0, -1, -1, true)]
        [TestCase(0, -1, 0, true)]
        [TestCase(0, -1, 1, true)]
        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, true)]
        [TestCase(0, 0, 1, true)]
        [TestCase(0, 1, -1, true)]
        [TestCase(0, 1, 0, true)]
        [TestCase(0, 1, 1, true)]
        [TestCase(10, -1, -1, true)]
        [TestCase(10, -1, 0, true)]
        [TestCase(10, -1, 9, true)]
        [TestCase(10, -1, 10, true)]
        [TestCase(10, 0, -1, true)]
        [TestCase(10, 0, 0, false)]
        [TestCase(10, 0, 9, false)]
        [TestCase(10, 0, 10, true)]
        [TestCase(10, 3, -1, true)]
        [TestCase(10, 3, 0, false)]
        [TestCase(10, 3, 2, false)]
        [TestCase(10, 3, 3, false)]
        [TestCase(10, 3, 4, false)]
        [TestCase(10, 3, 9, false)]
        [TestCase(10, 3, 10, true)]
        [TestCase(10, 9, -1, true)]
        [TestCase(10, 9, 0, false)]
        [TestCase(10, 9, 9, false)]
        [TestCase(10, 9, 10, true)]
        [TestCase(10, 10, -1, true)]
        [TestCase(10, 10, 0, true)]
        [TestCase(10, 10, 9, true)]
        [TestCase(10, 10, 10, true)]
        public static void MoveTest(int initLength, int oldIndex, int newIndex, bool isError)
        {
            var instance =
                MakeCollectionForMethodTest(initLength,
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
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
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (errorOccured) return;

            // 要素数が変化していないこと
            Assert.AreEqual(instance.Count, initLength);

            if (oldIndex == newIndex)
            {
                // 移動元と移動先が同じ場合、並び順が変化していないこと
                for (var i = 0; i < initLength; i++)
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
                for (; i < initLength; i++)
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
                for (; i < initLength; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
        }

        [TestCase(0, -1, -1, 0, true)]
        [TestCase(0, -1, 0, 0, true)]
        [TestCase(0, -1, 1, 0, true)]
        [TestCase(0, 0, -1, 0, true)]
        [TestCase(0, 0, 0, 0, true)]
        [TestCase(0, 0, 1, 0, true)]
        [TestCase(0, 1, -1, 0, true)]
        [TestCase(0, 1, 0, 0, true)]
        [TestCase(0, 1, 1, 0, true)]
        [TestCase(10, -1, -1, 0, true)]
        [TestCase(10, -1, 0, 0, true)]
        [TestCase(10, -1, 9, 0, true)]
        [TestCase(10, -1, 10, 0, true)]
        [TestCase(10, 0, -1, 0, true)]
        [TestCase(10, 0, 0, -1, true)]
        [TestCase(10, 0, 0, 0, false)]
        [TestCase(10, 0, 0, 10, false)]
        [TestCase(10, 0, 0, 11, true)]
        [TestCase(10, 0, 9, -1, true)]
        [TestCase(10, 0, 9, 0, false)]
        [TestCase(10, 0, 9, 1, false)]
        [TestCase(10, 0, 9, 2, true)]
        [TestCase(10, 0, 10, -1, true)]
        [TestCase(10, 0, 10, 0, false)]
        [TestCase(10, 0, 10, 1, true)]
        [TestCase(10, 3, -1, 0, true)]
        [TestCase(10, 3, 0, -1, true)]
        [TestCase(10, 3, 0, 0, false)]
        [TestCase(10, 3, 0, 7, false)]
        [TestCase(10, 3, 0, 8, true)]
        [TestCase(10, 3, 2, -1, true)]
        [TestCase(10, 3, 2, 0, false)]
        [TestCase(10, 3, 2, 7, false)]
        [TestCase(10, 3, 2, 8, true)]
        [TestCase(10, 3, 3, -1, true)]
        [TestCase(10, 3, 3, 0, false)]
        [TestCase(10, 3, 3, 7, false)]
        [TestCase(10, 3, 3, 8, true)]
        [TestCase(10, 3, 4, -1, true)]
        [TestCase(10, 3, 4, 0, false)]
        [TestCase(10, 3, 4, 6, false)]
        [TestCase(10, 3, 4, 7, true)]
        [TestCase(10, 3, 9, -1, true)]
        [TestCase(10, 3, 9, 0, false)]
        [TestCase(10, 3, 9, 1, false)]
        [TestCase(10, 3, 9, 2, true)]
        [TestCase(10, 3, 10, -1, true)]
        [TestCase(10, 3, 10, 0, false)]
        [TestCase(10, 3, 10, 1, true)]
        [TestCase(10, 9, -1, 0, true)]
        [TestCase(10, 9, 0, -1, true)]
        [TestCase(10, 9, 0, 0, false)]
        [TestCase(10, 9, 0, 1, false)]
        [TestCase(10, 9, 0, 2, true)]
        [TestCase(10, 9, 9, -1, true)]
        [TestCase(10, 9, 9, 0, false)]
        [TestCase(10, 9, 9, 1, false)]
        [TestCase(10, 9, 9, 2, true)]
        [TestCase(10, 9, 10, -1, true)]
        [TestCase(10, 9, 10, 0, false)]
        [TestCase(10, 9, 10, 1, true)]
        [TestCase(10, 10, -1, 0, true)]
        [TestCase(10, 10, 0, -1, true)]
        [TestCase(10, 10, 0, 0, true)]
        [TestCase(10, 10, 0, 1, true)]
        [TestCase(10, 10, 9, -1, true)]
        [TestCase(10, 10, 9, 0, true)]
        [TestCase(10, 10, 9, 1, true)]
        [TestCase(10, 10, 10, -1, true)]
        [TestCase(10, 10, 10, 0, true)]
        [TestCase(10, 10, 10, 1, true)]
        public static void MoveRangeTest(int initLength, int oldIndex, int newIndex, int count, bool isError)
        {
            var instance =
                MakeCollectionForMethodTest(initLength,
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

            var isChanged = !isError && count > 0;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isChanged)
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
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], isChanged ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isChanged ? 1 : 0);

            if (errorOccured) return;

            // 要素数が変化していないこと
            Assert.AreEqual(instance.Count, initLength);

            if (oldIndex == newIndex)
            {
                // 移動元と移動先が同じ場合、並び順が変化していないこと
                for (var i = 0; i < initLength; i++)
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
                for (; i < initLength; i++)
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
                for (; i < initLength; i++)
                {
                    Assert.IsTrue(instance[i].Equals(i.ToString()));
                }
            }
        }

        [TestCase(5, -1, false, false)]
        [TestCase(5, 0, true, false)]
        [TestCase(5, 4, true, false)]
        [TestCase(5, 5, false, false)]
        [TestCase(7, -1, false, false)]
        [TestCase(7, 0, false, true)]
        [TestCase(7, 6, false, true)]
        [TestCase(7, 7, false, false)]
        public static void RemoveTest(int initLength, int removeIndex, bool isError, bool removeResult)
        {
            // initLength == removeIndex のとき、remove対象は要素中に含まれないものになる

            var initStrLength = initLength > removeIndex + 1 ? initLength : removeIndex + 1;
            var initStrList = MakeStringList(initStrLength);

            var instance =
                MakeCollection2ForMethodTest(initStrList, initLength,
                    out var collectionChangingEventArgsList,
                    out var collectionChangedEventArgsList,
                    out var propertyChangingEventCalledCount,
                    out var propertyChangedEventCalledCount);

            var removeItem = removeIndex == -1 ? null : initStrList[removeIndex];

            var errorOccured = false;
            var result = false;
            try
            {
                result = instance.Remove(removeItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // Remove() の結果が意図した値であること
            Assert.AreEqual(result, removeResult);

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isError || !removeResult)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Remove)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, removeIndex);
                            Assert.AreEqual(eventArg.OldItems.Count, 1);
                            Assert.AreEqual(eventArg.OldItems[0], removeItem);
                            Assert.AreEqual(eventArg.NewStartingIndex, -1);
                            Assert.AreEqual(eventArg.NewItems, null);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isError || !removeResult)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

            if (errorOccured) return;

            if (!removeResult)
            {
                // ---------- 削除に失敗している場合 ----------

                // 削除後の要素数が元の要素数と一致すること
                Assert.AreEqual(instance.Count, initLength);

                // 要素が変化していないこと
                for (var i = 0; i < initLength; i++)
                {
                    Assert.AreEqual(instance[i], i.ToString());
                }

                return;
            }

            // ---------- 削除に成功している場合 ----------

            // 削除後の要素数が元の要素数-1であること
            Assert.AreEqual(instance.Count, initLength - 1);

            // 初期要素（削除位置より前）が変化していないこと
            for (var i = 0; i < removeIndex; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 初期要素（削除位置より後）が変化していないこと
            for (var i = removeIndex + 1; i < initLength - 1; i++)
            {
                Assert.AreEqual(instance[i], (i + 1).ToString());
            }
        }

        [TestCase(5, -1, true)]
        [TestCase(5, 0, true)]
        [TestCase(5, 4, true)]
        [TestCase(5, 5, true)]
        [TestCase(7, -1, true)]
        [TestCase(7, 0, false)]
        [TestCase(7, 6, false)]
        [TestCase(7, 7, true)]
        public static void RemoveAtTest(int initLength, int index, bool isError)
        {
            var initStrList = MakeStringList(initLength);
            var instance =
                MakeCollection2ForMethodTest(initStrList, initLength,
                    out var collectionChangingEventArgsList,
                    out var collectionChangedEventArgsList,
                    out var propertyChangingEventCalledCount,
                    out var propertyChangedEventCalledCount);

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

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isError)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Remove)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, index);
                            Assert.AreEqual(eventArg.OldItems.Count, 1);
                            Assert.AreEqual(eventArg.NewStartingIndex, -1);
                            Assert.AreEqual(eventArg.NewItems, null);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isError)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

            if (errorOccured) return;

            // 削除後の要素数が元の要素数-1であること
            Assert.AreEqual(instance.Count, initLength - 1);

            // 初期要素（削除位置より前）が変化していないこと
            for (var i = 0; i < index; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 初期要素（削除位置より後）が変化していないこと
            for (var i = index + 1; i < initLength - 1; i++)
            {
                Assert.AreEqual(instance[i], (i + 1).ToString());
            }
        }

        [TestCase(5, -1, -1, true)]
        [TestCase(5, 0, -1, true)]
        [TestCase(5, 4, -1, true)]
        [TestCase(5, 5, -1, true)]
        [TestCase(5, -1, 0, true)]
        [TestCase(5, 0, 0, false)]
        [TestCase(5, 4, 0, false)]
        [TestCase(5, 5, 0, true)]
        [TestCase(5, -1, 1, true)]
        [TestCase(5, 0, 1, true)]
        [TestCase(5, 4, 1, true)]
        [TestCase(5, 5, 1, true)]
        [TestCase(7, -1, -1, true)]
        [TestCase(7, 0, -1, true)]
        [TestCase(7, 6, -1, true)]
        [TestCase(7, 7, -1, true)]
        [TestCase(7, -1, 0, true)]
        [TestCase(7, 0, 0, false)]
        [TestCase(7, 6, 0, false)]
        [TestCase(7, 7, 0, true)]
        [TestCase(7, -1, 1, true)]
        [TestCase(7, 0, 1, false)]
        [TestCase(7, 6, 1, false)]
        [TestCase(7, 7, 1, true)]
        [TestCase(7, -1, 2, true)]
        [TestCase(7, 0, 2, false)]
        [TestCase(7, 5, 2, false)]
        [TestCase(7, 6, 2, true)]
        [TestCase(7, -1, 3, true)]
        [TestCase(7, 0, 3, true)]
        [TestCase(7, 4, 3, true)]
        [TestCase(7, 5, 3, true)]
        public static void RemoveRangeTest(int initLength, int index, int count,
            bool isError)
        {
            var initStrList = MakeStringList(initLength);
            var instance =
                MakeCollection2ForMethodTest(initStrList, initLength,
                    out var collectionChangingEventArgsList,
                    out var collectionChangedEventArgsList,
                    out var propertyChangingEventCalledCount,
                    out var propertyChangedEventCalledCount);

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

            var isChanged = !isError && count > 0;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isChanged)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Remove)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, index);
                            Assert.AreEqual(eventArg.OldItems.Count, count);
                            Assert.AreEqual(eventArg.NewStartingIndex, -1);
                            Assert.AreEqual(eventArg.NewItems, null);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            // 削除後の要素数が元の要素数-削除数であること
            Assert.AreEqual(instance.Count, initLength - count);

            // 初期要素（削除位置より前）が変化していないこと
            for (var i = 0; i < index; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 初期要素（削除位置より後）が変化していないこと
            for (var i = index + count; i < initLength - count; i++)
            {
                Assert.AreEqual(instance[i], (i + count).ToString());
            }
        }

        [TestCase(TestClassType.Type1, 8, -1, true)]
        [TestCase(TestClassType.Type1, 8, 0, false)]
        [TestCase(TestClassType.Type1, 8, 8, false)]
        [TestCase(TestClassType.Type1, 8, 10, false)]
        [TestCase(TestClassType.Type1, 8, 11, true)]
        [TestCase(TestClassType.Type2, 8, -1, true)]
        [TestCase(TestClassType.Type2, 8, 4, true)]
        [TestCase(TestClassType.Type2, 8, 5, false)]
        [TestCase(TestClassType.Type2, 8, 8, false)]
        [TestCase(TestClassType.Type2, 8, 10, false)]
        [TestCase(TestClassType.Type2, 8, 11, true)]
        public static void AdjustLengthTest(TestClassType classType, int initLength,
            int adjustLength, bool isError)
        {
            var initList = MakeStringList(initLength);

            IRestrictedCapacityList<string> instance = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangingEventCalledCount = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                default:
                    Assert.Fail();
                    break;
            }

            var errorOccured = false;
            try
            {
                instance.AdjustLength(adjustLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            var isChanged = !isError && initLength != adjustLength;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        if (isChanged)
                        {
                            if (initLength < adjustLength)
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                                var eventArg = dic[nameof(NotifyCollectionChangedAction.Add)][0];
                                Assert.AreEqual(eventArg.OldStartingIndex, -1);
                                Assert.AreEqual(eventArg.OldItems, null);
                                Assert.AreEqual(eventArg.NewStartingIndex, initLength);
                                Assert.AreEqual(eventArg.NewItems.Count, adjustLength - initLength);
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                            }
                            else if (initLength == adjustLength)
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                            }
                            else // initLength > adjustLength
                            {
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                                Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 1);
                                var eventArg = dic[nameof(NotifyCollectionChangedAction.Remove)][0];
                                Assert.AreEqual(eventArg.OldStartingIndex, adjustLength);
                                Assert.AreEqual(eventArg.OldItems.Count, initLength - adjustLength);
                                Assert.AreEqual(eventArg.NewStartingIndex, -1);
                                Assert.AreEqual(eventArg.NewItems, null);
                            }
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            // 要素数が調整サイズと一致すること
            Assert.AreEqual(instance.Count, adjustLength);

            // 操作前の要素（要素追加した場合は初期要素、要素削除した場合はすべての要素）が変化していないこと
            var nonChangedCnt = initLength < adjustLength
                ? initLength
                : adjustLength;
            var i = 0;
            for (; i < nonChangedCnt; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 追加した要素がデフォルト要素と一致すること
            for (; i < instance.Count; i++)
            {
                Assert.AreEqual(instance[i], "test");
            }
        }

        [TestCase(TestClassType.Type1, 8, -1, -1, true)]
        [TestCase(TestClassType.Type1, 8, 0, 8, false)]
        [TestCase(TestClassType.Type1, 8, 10, 10, false)]
        [TestCase(TestClassType.Type1, 8, 11, -1, true)]
        [TestCase(TestClassType.Type2, 8, -1, -1, true)]
        [TestCase(TestClassType.Type2, 8, 4, -1, true)]
        [TestCase(TestClassType.Type2, 8, 5, 8, false)]
        [TestCase(TestClassType.Type2, 8, 10, 10, false)]
        [TestCase(TestClassType.Type2, 8, 11, -1, true)]
        public static void AdjustLengthIfShortTest(TestClassType classType, int initLength,
            int adjustLength, int answerLength, bool isError)
        {
            var initList = MakeStringList(initLength);

            IRestrictedCapacityList<string> instance = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangingEventCalledCount = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                default:
                    Assert.Fail();
                    break;
            }

            var errorOccured = false;
            try
            {
                instance.AdjustLengthIfShort(adjustLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            var isChanged = !isError && initLength < adjustLength;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        if (isChanged)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Add)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, -1);
                            Assert.AreEqual(eventArg.OldItems, null);
                            Assert.AreEqual(eventArg.NewStartingIndex, initLength);
                            Assert.AreEqual(eventArg.NewItems.Count, adjustLength - initLength);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            // 要素数が調整サイズと一致すること
            Assert.AreEqual(instance.Count, answerLength);

            // 操作前の要素（要素追加した場合は初期要素、要素削除した場合はすべての要素）が変化していないこと
            var nonChangedCnt = initLength;
            var i = 0;
            for (; i < nonChangedCnt; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }

            // 追加した要素がデフォルト要素と一致すること
            for (; i < instance.Count; i++)
            {
                Assert.AreEqual(instance[i], "test");
            }
        }

        [TestCase(TestClassType.Type1, 8, -1, -1, true)]
        [TestCase(TestClassType.Type1, 8, 0, 0, false)]
        [TestCase(TestClassType.Type1, 8, 10, 8, false)]
        [TestCase(TestClassType.Type1, 8, 11, -1, true)]
        [TestCase(TestClassType.Type2, 8, -1, -1, true)]
        [TestCase(TestClassType.Type2, 8, 4, -1, true)]
        [TestCase(TestClassType.Type2, 8, 5, 5, false)]
        [TestCase(TestClassType.Type2, 8, 10, 8, false)]
        [TestCase(TestClassType.Type2, 8, 11, -1, true)]
        public static void AdjustLengthIfLongTest(TestClassType classType, int initLength,
            int adjustLength, int answerLength, bool isError)
        {
            var initList = MakeStringList(initLength);

            IRestrictedCapacityList<string> instance = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangingEventCalledCount = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                default:
                    Assert.Fail();
                    break;
            }

            var errorOccured = false;
            try
            {
                instance.AdjustLengthIfLong(adjustLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            var isChanged = !isError && initLength > adjustLength;

            // 各イベントが意図した回数呼ばれていること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<NotifyCollectionChangedEventArgs>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        if (isChanged)
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 1);
                            var eventArg = dic[nameof(NotifyCollectionChangedAction.Remove)][0];
                            Assert.AreEqual(eventArg.OldStartingIndex, adjustLength);
                            Assert.AreEqual(eventArg.OldItems.Count, initLength - adjustLength);
                            Assert.AreEqual(eventArg.NewStartingIndex, -1);
                            Assert.AreEqual(eventArg.NewItems, null);
                        }
                        else
                        {
                            Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        }
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            if (isChanged)
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }
            else
            {
                Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);

                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }

            if (errorOccured) return;

            // 要素数が調整サイズと一致すること
            Assert.AreEqual(instance.Count, answerLength);

            // 操作前の要素が変化していないこと
            for (var i = 0; i < answerLength; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }
        }

        [TestCase(TestClassType.Type1, 0)]
        [TestCase(TestClassType.Type1, 10)]
        [TestCase(TestClassType.Type2, 5)]
        [TestCase(TestClassType.Type2, 10)]
        public static void ClearTest(TestClassType classType, int initLength)
        {
            var initList = MakeStringList(initLength);

            IRestrictedCapacityList<string> instance = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangingEventCalledCount = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength,
                        out collectionChangingEventArgsList,
                        out collectionChangedEventArgsList,
                        out propertyChangingEventCalledCount,
                        out propertyChangedEventCalledCount);
                    break;
                default:
                    Assert.Fail();
                    break;
            }

            var minCapacity = instance.GetMinCapacity();

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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count,
                            initLength != 0
                                ? 1
                                : 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 要素数が容量最小数と一致すること
            Assert.AreEqual(instance.Count, minCapacity);

            // すべての要素がデフォルト要素と一致すること
            foreach (var t in instance)
            {
                Assert.AreEqual(t, "test");
            }
        }

        private static readonly object[] ResetTestCaseSource =
        {
            new object[] { -1, false, true },
            new object[] { 4, false, true },
            new object[] { 5, false, false },
            new object[] { 5, true, true },
            new object[] { 7, false, false },
            new object[] { 7, true, true },
            new object[] { 10, false, false },
            new object[] { 10, true, true },
            new object[] { 11, false, true }
        };

        [TestCaseSource(nameof(ResetTestCaseSource))]
        public static void ResetTest(int resetItemLength, bool isResetItemHasNull, bool isError)
        {
            const int initItemLength = 7;
            var initItem = MakeStringList(initItemLength);
            var instance =
                MakeCollection2ForMethodTest(initItem, initItemLength,
                    out var collectionChangingEventArgsList,
                    out var collectionChangedEventArgsList,
                    out var propertyChangingEventCalledCount,
                    out var propertyChangedEventCalledCount);
            var resetItem = MakeStringList2(resetItemLength, isResetItemHasNull)?
                .Select(s => s is null ? null : $"reset_{s}").ToList();

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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 1);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 各要素が初期化した要素と一致すること
            Assert.IsTrue(instance.SequenceEqual(resetItem));
        }

        private static readonly object[] ResetAsWritableListTestCaseSource =
        {
            new object[] { -1, false, true },
            new object[] { 6, false, true },
            new object[] { 7, false, false },
            new object[] { 7, true, true },
            new object[] { 10, false, true }
        };

        [TestCaseSource(nameof(ResetAsWritableListTestCaseSource))]
        public static void ResetTestAsWritableList(int resetItemLength, bool isResetItemHasNull, bool isError)
        {
            const int initItemLength = 7;
            var initItem = MakeStringList(initItemLength);
            var instance =
                MakeCollection2ForMethodTest(initItem, initItemLength,
                    out var collectionChangingEventArgsList,
                    out var collectionChangedEventArgsList,
                    out var propertyChangingEventCalledCount,
                    out var propertyChangedEventCalledCount);
            var resetItem = MakeStringList2(resetItemLength, isResetItemHasNull)?
                .Select(s => s is null ? null : $"reset_{s}").ToList();

            var errorOccured = false;
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                ((IWritableList<string, string>)instance).Reset(resetItem);
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 1);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 1);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);

            // 各要素が初期化した要素と一致すること
            Assert.IsTrue(instance.SequenceEqual(resetItem));
        }

        [TestCase(5, "1", true)]
        [TestCase(5, "6", false)]
        [TestCase(5, null, false)]
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
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(collectionChangingEventArgsList);
            assertCollectionChangeEventArgsList(collectionChangedEventArgsList);
            Assert.AreEqual(propertyChangingEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangingEventCalledCount[ListConstant.IndexerName], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = MakeCollectionForMethodTest(5, out _, out _, out _, out _);
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
            var instance = MakeCollection8(
                out _,
                out var collectionChangedEventArgsList,
                out _,
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

            // CollectionChanged が発火していないこと
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);

            // NotifyPropertyChanged が発火していないこと
            Assert.AreEqual(propertyChangedEventCalledCount["Count"], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [Test]
        public static void ItemEqualsTest_IRestrictedCapacityList()
        {
            {
                // すべての要素が Equal であるリストの比較
                var items = MakeStringList(10);
                var left = new ListTest1(items);
                IRestrictedCapacityList<string> right = new ListTest1(items);

                Assert.IsTrue(left.ItemEquals(right));
            }
            {
                // 一つだけ要素が異なるリストの比較
                var left = new ListTest1(MakeStringList(10));
                var rightItems = MakeStringList(10);
                rightItems[2] = "NotEqual";
                IRestrictedCapacityList<string> right = new ListTest1(rightItems);

                Assert.IsFalse(left.ItemEquals(right));
            }
            {
                // 要素数が異なるリストの比較
                var left = new ListTest1(MakeStringList(10));
                IRestrictedCapacityList<string> right = new ListTest1(MakeStringList(9));

                Assert.IsFalse(left.ItemEquals(right));
            }
            {
                // nullとの比較
                var left = new ListTest1(MakeStringList(10));

                Assert.IsFalse(left.ItemEquals(null));
            }
        }

        [Test]
        public static void ItemEqualsTest_RestrictedCapacityList()
        {
            {
                // すべての要素が Equal であるリストの比較
                var left = new ListTest1(MakeStringList(10));
                var right = new ListTest1(MakeStringList(10));

                Assert.IsTrue(left.ItemEquals(right));
            }
            {
                // 一つだけ要素が異なるリストの比較
                var left = new ListTest1(MakeStringList(10));
                var rightItems = MakeStringList(10);
                rightItems[2] = "NotEqual";
                IRestrictedCapacityList<string> right = new ListTest1(rightItems);

                Assert.IsFalse(left.ItemEquals(right));
            }
            {
                // 要素数が異なるリストの比較
                var left = new ListTest1(MakeStringList(10));
                IRestrictedCapacityList<string> right = new ListTest1(MakeStringList(9));

                Assert.IsFalse(left.ItemEquals(right));
            }
            {
                // nullとの比較
                var left = new ListTest1(MakeStringList(10));

                Assert.IsFalse(left.ItemEquals(null));
            }
        }

        [Test]
        public static void ItemEqualsTest_IFixedLengthCollection()
        {
            {
                // すべての要素が  Equal であるリストの比較
                var left = new ListTest1(MakeStringList(10));
                IFixedLengthList<string> right = new CollectionTest7();
                left.ForEach((s, i) => right[i] = s);

                Assert.IsTrue(left.ItemEquals(right));
            }
            {
                // 一つだけ要素が異なるリストの比較
                var left = new ListTest1(MakeStringList(10));
                IFixedLengthList<string> right = new CollectionTest7();
                left.ForEach((s, i) => right[i] = s);
                right[5] = "NotFound";

                Assert.IsFalse(left.ItemEquals(right));
            }
            {
                // 要素数が異なるリストの比較
                var left = new ListTest1(MakeStringList(9));
                IFixedLengthList<string> right = new CollectionTest7();

                Assert.IsFalse(left.ItemEquals(right));
            }
            {
                // nullとの比較
                var left = new ListTest1(MakeStringList(10));

                Assert.IsFalse(left.ItemEquals(null));
            }
        }

        [Test]
        public static void DeepCloneTest()
        {
            const int initCount = 10;
            var instance = new ListTest9(MakeCollection9(initCount));
            ListTest9 clone = null;

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

        private static readonly object[] DeepCloneWithTestCaseSource =
        {
            new object[] { null, null, true },
            new object[] { null, Array.Empty<KeyValuePair<int, TestClass>>(), false },
            new object[] { null, new KeyValuePair<int, TestClass>[] { new(-1, new TestClass()) }, false },
            new object[] { null, new KeyValuePair<int, TestClass>[] { new(0, new TestClass()) }, false },
            new object[] { null, new KeyValuePair<int, TestClass>[] { new(4, new TestClass()) }, false },
            new object[] { null, new KeyValuePair<int, TestClass>[] { new(5, new TestClass()) }, false },
            new object[]
            {
                null, new KeyValuePair<int, TestClass>[] { new(-1, new TestClass()), new(0, new TestClass()) }, false
            },
            new object[]
            {
                null, new KeyValuePair<int, TestClass>[] { new(0, new TestClass()), new(4, new TestClass()) }, false
            },
            new object[]
            {
                null, new KeyValuePair<int, TestClass>[] { new(3, new TestClass()), new(5, new TestClass()) }, false
            },
            new object[] { 0, null, false },
            new object[] { 10, null, false },
            new object[] { 11, null, true },
            new object[] { 0, Array.Empty<KeyValuePair<int, TestClass>>(), false },
            new object[] { 0, new KeyValuePair<int, TestClass>[] { new(0, new TestClass()) }, false },
            new object[] { 3, Array.Empty<KeyValuePair<int, TestClass>>(), false },
            new object[]
                { 3, new KeyValuePair<int, TestClass>[] { new(0, new TestClass()), new(2, new TestClass()) }, false },
            new object[]
                { 3, new KeyValuePair<int, TestClass>[] { new(0, new TestClass()), new(3, new TestClass()) }, false }
        };

        [TestCaseSource(nameof(DeepCloneWithTestCaseSource))]
        public static void DeepCloneWithTest(int? length,
            IEnumerable<KeyValuePair<int, TestClass>> values, bool isError)
        {
            const int initCount = 5;
            var instance = MakeCollection9(initCount);

            var guidList = instance.Select(x => x.Guid).ToList();
            var valueList =
                new Dictionary<int, TestClass>(values?.ToArray() ?? Array.Empty<KeyValuePair<int, TestClass>>());

            var param = length is null && values is null
                ? null
                : new ListDeepCloneParam<TestClass>
                {
                    Length = length,
                    Values = valueList
                };

            ListTest9 clone = null;

            var errorOccured = false;
            try
            {
                clone = instance.DeepCloneWith(param);
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

            if (length is null)
            {
                // 元の要素数から変化していないこと
                Assert.AreEqual(clone.Count, initCount);
            }
            else
            {
                // 指定された要素数であること
                Assert.AreEqual(clone.Count, length.Value);

                // 追加された要素が null ではないこと
                for (var i = initCount; i < clone.Count; i++)
                {
                    Assert.NotNull(clone[i]);
                }
            }

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
                    if (0 <= key && key < guidList2.Count) guidList2[key] = value.Guid;
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

        private static List<string> MakeStringList2(int length, bool hasNull)
        {
            // hasNullInAddLength = true の場合、リスト中にひとつだけnullを混ぜる
            if (length < 0) return null;
            var result = new List<string>();
            for (var i = 0; i < length; i++)
            {
                result.Add(
                    hasNull && i == length / 2
                        ? null
                        : (i * 100).ToString()
                );
            }

            return result;
        }

        private static ListTest1 MakeCollectionForMethodTest(int initLength,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new ListTest1()
                : new ListTest1(initStringList);

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

        private static ListTest2 MakeCollection2ForMethodTest(List<string> initStringList, int initLength,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var result = initStringList == null
                ? new ListTest2()
                : new ListTest2(initStringList.GetRange(0, initLength));

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

        private static ListTest8 MakeCollection8(
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangingEventArgsList,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangingEventCalledCount,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var result = new ListTest8
            {
                NotifyPropertyChangingEventType = NotifyPropertyChangeEventType.Enabled,
                NotifyPropertyChangedEventType = NotifyPropertyChangeEventType.Enabled,
                NotifyCollectionChangingEventType = NotifyCollectionChangeEventType.Single,
                NotifyCollectionChangedEventType = NotifyCollectionChangeEventType.Single
            };

            result.AddRange(new[] { "", "", "", "", "" });
            result.IsThrowException = true;

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

        private static ListTest9 MakeCollection9(int? length)
        {
            var result = length == null
                ? null
                : new ListTest9(Enumerable.Repeat("", length.Value).Select(_ => new TestClass()));
            return result;
        }

        /// <summary>
        ///     CollectionChangedEventArgs を格納するための Dictionary インスタンスを生成する
        /// </summary>
        /// <returns>生成したインスタンス</returns>
        private static Dictionary<string, List<NotifyCollectionChangedEventArgs>> MakeCollectionChangeEventArgsDic()
            => new()
            {
                { nameof(NotifyCollectionChangedAction.Add), new List<NotifyCollectionChangedEventArgs>() },
                { nameof(NotifyCollectionChangedAction.Replace), new List<NotifyCollectionChangedEventArgs>() },
                { nameof(NotifyCollectionChangedAction.Remove), new List<NotifyCollectionChangedEventArgs>() },
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
            { "Count", 0 },
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

        public enum TestClassType
        {
            Type1,
            Type2,
            Type3,
            Type4,
            Type5,
            Type6
        }

        private abstract class AbsListTest<T> : RestrictedCapacityList<string, T>
            where T : AbsListTest<T>
        {
            protected AbsListTest()
            {
            }

            protected AbsListTest(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class ListTest1 : AbsListTest<ListTest1>
        {
            /**
             * 正常設定
             */

            private static int MaxCapacity => 10;

            public static int MinCapacity => 0;
            private static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public ListTest1()
            {
            }

            public ListTest1(IReadOnlyCollection<string> list) : base(list)
            {
            }

            public override ListTest1 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest1 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class ListTest2 : AbsListTest<ListTest2>
        {
            /**
             * 正常設定
             * 初期要素数非0
             */

            private static int MaxCapacity => 10;

            public static int MinCapacity => 5;
            private static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public ListTest2()
            {
            }

            public ListTest2(IReadOnlyCollection<string> list) : base(list)
            {
            }

            public override ListTest2 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest2 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class ListTest3 : AbsListTest<ListTest3>
        {
            /**
             * 正常設定
             * 初期要素数非0
             * MinCapacity = MaxCapacity
             */

            private static int MaxCapacity => 10;

            public static int MinCapacity => 10;
            private static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public ListTest3()
            {
            }

            public ListTest3(IReadOnlyCollection<string> list) : base(list)
            {
            }

            public override ListTest3 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest3 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class ListTest4 : AbsListTest<ListTest4>
        {
            /*
             * 異常設定（MinCapacity < 0）
             */

            private static int MaxCapacity => 10;
            public static int MinCapacity => -2;
            private static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public ListTest4()
            {
            }

            public ListTest4(IReadOnlyCollection<string> list) : base(list)
            {
            }

            public override ListTest4 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest4 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class ListTest5 : AbsListTest<ListTest5>
        {
            /**
             * 異常設定（MinCapacity > MaxCapacity）
             */

            private static int MaxCapacity => 10;

            public static int MinCapacity => 11;
            private static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public ListTest5()
            {
            }

            public ListTest5(IReadOnlyCollection<string> list) : base(list)
            {
            }

            public override ListTest5 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest5 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class ListTest6 : AbsListTest<ListTest6>
        {
            /**
             * 異常設定（DefaultValue＝null）
             */
            private static int MaxCapacity => 10;

            public static int MinCapacity => 0;
            private static string Default => null;

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public ListTest6()
            {
            }

            public ListTest6(IReadOnlyCollection<string> list) : base(list)
            {
            }

            public override ListTest6 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest6 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class CollectionTest7 : FixedLengthList<string, CollectionTest7>
        {
            public CollectionTest7() : base(Enumerable.Range(0, 10).Select(_ => ""))
            {
            }

            protected override string MakeDefaultItem(int index) => index.ToString();

            public override CollectionTest7 DeepClone()
            {
                throw new Exception();
            }

            protected override CollectionTest7 MakeInstance(IEnumerable<string> items)
            {
                throw new Exception();
            }
        }

        private class ListTest8 : AbsListTest<ListTest8>
        {
            /**
             * CollectionChanging, CollectionChanged メソッドテスト用
             */
            public override int GetMaxCapacity() => 10;

            public override int GetMinCapacity() => 0;

            public bool IsThrowException { get; set; }

            protected override string MakeDefaultItem(int index) => "";

            public ListTest8()
            {
                CollectionChanging += OnCollectionChanging;
            }

            private ListTest8(IEnumerable<string> initItems) : base(initItems.ToList())
            {
                CollectionChanging += OnCollectionChanging;
            }

            private void OnCollectionChanging(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (IsThrowException) throw new Exception();
            }

            public override ListTest8 DeepClone()
            {
                throw new Exception();
            }

            protected override ListTest8 MakeInstance(IEnumerable<string> items)
                => new(items.ToList());
        }

        private class ListTest9 : RestrictedCapacityList<TestClass, ListTest9>
        {
            public override int GetMaxCapacity() => 10;

            public override int GetMinCapacity() => 0;

            public ListTest9(IEnumerable<TestClass> list) : base(
                ((Func<IEnumerable<TestClass>>)(() => list.Select(x => x.DeepClone())))())
            {
            }

            public override ListTest9 DeepClone()
                => new(this);

            protected override TestClass MakeDefaultItem(int index)
                => new();

            protected override ListTest9 MakeInstance(IEnumerable<TestClass> items)
                => new(items);
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
