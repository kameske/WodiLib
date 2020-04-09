using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class RestrictedCapacityCollectionTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(TestClassType.Type1, false)]
        [TestCase(TestClassType.Type2, false)]
        [TestCase(TestClassType.Type3, false)]
        [TestCase(TestClassType.Type4, true)]
        [TestCase(TestClassType.Type5, true)]
        [TestCase(TestClassType.Type6, true)]
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
                        initLength = CollectionTest1.MinCapacity;
                        instance = new CollectionTest1();
                        break;
                    case TestClassType.Type2:
                        initLength = CollectionTest2.MinCapacity;
                        instance = new CollectionTest2();
                        break;
                    case TestClassType.Type3:
                        initLength = CollectionTest3.MinCapacity;
                        instance = new CollectionTest3();
                        break;
                    case TestClassType.Type4:
                        initLength = CollectionTest4.MinCapacity;
                        instance = new CollectionTest4();
                        break;
                    case TestClassType.Type5:
                        initLength = CollectionTest5.MinCapacity;
                        instance = new CollectionTest5();
                        break;
                    case TestClassType.Type6:
                        initLength = CollectionTest6.MinCapacity;
                        instance = new CollectionTest6();
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
        [TestCase(TestClassType.Type4, 0, true)]
        [TestCase(TestClassType.Type4, 10, true)]
        [TestCase(TestClassType.Type4, 11, true)]
        [TestCase(TestClassType.Type5, -1, true)]
        [TestCase(TestClassType.Type5, 0, true)]
        [TestCase(TestClassType.Type5, 10, true)]
        [TestCase(TestClassType.Type5, 11, true)]
        [TestCase(TestClassType.Type6, -1, true)]
        [TestCase(TestClassType.Type6, 0, true)]
        [TestCase(TestClassType.Type6, 10, true)]
        [TestCase(TestClassType.Type6, 11, true)]
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
                    case TestClassType.Type4:
                        instance = new CollectionTest4(initList);
                        break;
                    case TestClassType.Type5:
                        instance = new CollectionTest5(initList);
                        break;
                    case TestClassType.Type6:
                        instance = new CollectionTest6(initList);
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが一度も呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが一度も呼ばれていないこと
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count,
                0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString],
                isError ? 0 : 1);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count,
                isError ? 0 : 1);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (errorOccured) return;

            for (var i = 0; i < initLength; i++)
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが一度も呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが一度も呼ばれていないこと
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                isError ? 0 : 1);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                {
                    var addEventArg = collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                    Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                    Assert.AreEqual(addEventArg.OldItems, null);
                    Assert.AreEqual(addEventArg.NewStartingIndex, initLength);
                    Assert.AreEqual(addEventArg.NewItems.Count, 1);
                    Assert.AreEqual(addEventArg.NewItems[0], item);
                }
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], isError ? 0 : addLength);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                isError ? 0 : addLength);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                {
                    var addEventArg = collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                    Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                    Assert.AreEqual(addEventArg.OldItems, null);
                    Assert.AreEqual(addEventArg.NewStartingIndex, initLength);
                    Assert.AreEqual(addEventArg.NewItems.Count, addLength);
                    Assert.IsTrue(addEventArg.NewItems.Cast<string>().SequenceEqual(addList));
                }
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                isError ? 0 : 1);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                {
                    var addEventArg = collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                    Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                    Assert.AreEqual(addEventArg.OldItems, null);
                    Assert.AreEqual(addEventArg.NewStartingIndex, index);
                    Assert.AreEqual(addEventArg.NewItems.Count, 1);
                    Assert.AreEqual(addEventArg.NewItems[0], item);
                }
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], isError ? 0 : addLength);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                isError ? 0 : addLength);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                {
                    var addEventArg = collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                    Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                    Assert.AreEqual(addEventArg.OldItems, null);
                    Assert.AreEqual(addEventArg.NewStartingIndex, index);
                    Assert.AreEqual(addEventArg.NewItems.Count, addLength);
                    Assert.IsTrue(addEventArg.NewItems.Cast<string>().SequenceEqual(addList));
                }
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
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
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            var answerSetCount = initLength - index;
            if (answerSetCount > overwriteLength) answerSetCount = overwriteLength;
            var answerInsertCount = overwriteLength - answerSetCount;
            if (answerInsertCount < 0) answerInsertCount = 0;
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], isError ? 0 : answerSetCount);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], isError ? 0 : answerInsertCount);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString],
                isError ? 0 : answerSetCount);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                isError ? 0 : answerInsertCount);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count,
                    0);
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                var replaceMax = initLength - index;
                var replaceCnt = Math.Min(replaceMax, overwriteLength);
                var addStartIndex = index + replaceCnt;
                var addCnt = overwriteLength - replaceCnt;

                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count,
                    1);
                {
                    var eventArg =
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)][0];
                    Assert.AreEqual(eventArg.OldStartingIndex, index);
                    Assert.AreEqual(eventArg.OldItems.Count, replaceCnt);
                    Assert.AreEqual(eventArg.NewStartingIndex, index);
                    Assert.AreEqual(eventArg.NewItems.Count, replaceCnt);
                }
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 1);
                {
                    var addEventArg = collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                    Assert.AreEqual(addEventArg.OldStartingIndex, -1);
                    Assert.IsNull(addEventArg.OldItems);
                    Assert.AreEqual(addEventArg.NewStartingIndex, addStartIndex);
                    Assert.AreEqual(addEventArg.NewItems.Count, addCnt);
                }
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

            if (errorOccured) return;

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
                MakeCollectionForMethodTest(initLength, out var countDic,
                    out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 1);
                {
                    var eventArg =
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)][0];
                    Assert.AreEqual(eventArg.OldStartingIndex, oldIndex);
                    Assert.AreEqual(eventArg.OldItems.Count, 1);
                    Assert.AreEqual(eventArg.NewStartingIndex, newIndex);
                    Assert.AreEqual(eventArg.NewItems.Count, 1);
                    Assert.IsTrue(ReferenceEquals(eventArg.OldItems[0], eventArg.NewItems[0]));
                }
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

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
                MakeCollectionForMethodTest(initLength, out var countDic,
                    out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], isError ? 0 : count);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 1);
                {
                    var eventArg =
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)][0];
                    Assert.AreEqual(eventArg.OldStartingIndex, oldIndex);
                    Assert.AreEqual(eventArg.OldItems.Count, count);
                    Assert.AreEqual(eventArg.NewStartingIndex, newIndex);
                    Assert.AreEqual(eventArg.NewItems.Count, count);
                    for (var i = 0; i < count; i++)
                    {
                        Assert.IsTrue(ReferenceEquals(eventArg.OldItems[i], eventArg.NewItems[i]));
                    }
                }
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
            }

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
                MakeCollection2ForMethodTest(initStrList, initLength, out var countDic,
                    out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], !isError && removeResult ? 1 : 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString],
                !isError && removeResult ? 1 : 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError || !removeResult)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    1);
                {
                    var eventArg =
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)][0];
                    Assert.AreEqual(eventArg.OldStartingIndex, removeIndex);
                    Assert.AreEqual(eventArg.OldItems.Count, 1);
                    Assert.AreEqual(eventArg.OldItems[0], removeItem);
                    Assert.AreEqual(eventArg.NewStartingIndex, -1);
                    Assert.AreEqual(eventArg.NewItems, null);
                }
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
                MakeCollection2ForMethodTest(initStrList, initLength, out var countDic,
                    out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString],
                isError ? 0 : 1);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    1);
                {
                    var eventArg =
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)][0];
                    Assert.AreEqual(eventArg.OldStartingIndex, index);
                    Assert.AreEqual(eventArg.OldItems.Count, 1);
                    Assert.AreEqual(eventArg.NewStartingIndex, -1);
                    Assert.AreEqual(eventArg.NewItems, null);
                }
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
                MakeCollection2ForMethodTest(initStrList, initLength, out var countDic,
                    out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], isError ? 0 : count);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString],
                isError ? 0 : count);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    0);
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
            }
            else
            {
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    1);
                {
                    var eventArg =
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)][0];
                    Assert.AreEqual(eventArg.OldStartingIndex, index);
                    Assert.AreEqual(eventArg.OldItems.Count, count);
                    Assert.AreEqual(eventArg.NewStartingIndex, -1);
                    Assert.AreEqual(eventArg.NewItems, null);
                }
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
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
        [TestCase(TestClassType.Type1, 8, 10, false)]
        [TestCase(TestClassType.Type1, 8, 11, true)]
        [TestCase(TestClassType.Type2, 8, -1, true)]
        [TestCase(TestClassType.Type2, 8, 4, true)]
        [TestCase(TestClassType.Type2, 8, 5, false)]
        [TestCase(TestClassType.Type2, 8, 10, false)]
        [TestCase(TestClassType.Type2, 8, 11, true)]
        public static void AdjustLengthTest(TestClassType classType, int initLength,
            int adjustLength, bool isError)
        {
            var initList = MakeStringList(initLength);

            AbsCollectionTest instance = null;
            Dictionary<string, int> countDic = null;
            Dictionary<string, Dictionary<string, int>> handlerCalledCount = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength, out countDic,
                        out handlerCalledCount, out collectionChangedEventArgsList,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength, out countDic,
                        out handlerCalledCount, out collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            var insertedCnt = errorOccured
                ? 0
                : initLength < adjustLength
                    ? adjustLength - initLength
                    : 0;
            var removedCnt = errorOccured
                ? 0
                : initLength > adjustLength
                    ? initLength - adjustLength
                    : 0;
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], insertedCnt);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], removedCnt);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                insertedCnt);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString],
                removedCnt);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    0);
            }
            else
            {
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
                if (initLength < adjustLength)
                {
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count,
                        1);
                    {
                        var eventArg =
                            collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                        Assert.AreEqual(eventArg.OldStartingIndex, -1);
                        Assert.AreEqual(eventArg.OldItems, null);
                        Assert.AreEqual(eventArg.NewStartingIndex, initLength);
                        Assert.AreEqual(eventArg.NewItems.Count, adjustLength - initLength);
                    }
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                        0);
                }
                else if (initLength == adjustLength)
                {
                    Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count,
                        0);
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                        0);
                }
                else
                {
                    // initLength > adjustLength
                    Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count,
                        0);
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                        1);
                    {
                        var eventArg =
                            collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)][0];
                        Assert.AreEqual(eventArg.OldStartingIndex, adjustLength);
                        Assert.AreEqual(eventArg.OldItems.Count, initLength - adjustLength);
                        Assert.AreEqual(eventArg.NewStartingIndex, -1);
                        Assert.AreEqual(eventArg.NewItems, null);
                    }
                }
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

            AbsCollectionTest instance = null;
            Dictionary<string, int> countDic = null;
            Dictionary<string, Dictionary<string, int>> handlerCalledCount = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength, out countDic,
                        out handlerCalledCount, out collectionChangedEventArgsList,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength, out countDic,
                        out handlerCalledCount, out collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            var insertedCnt = errorOccured
                ? 0
                : initLength < adjustLength
                    ? adjustLength - initLength
                    : 0;
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], insertedCnt);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString],
                insertedCnt);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            if (isError)
            {
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
                Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                    0);
            }
            else
            {
                Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 1);
                Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 1);
                if (initLength >= adjustLength)
                {
                    Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count,
                        0);
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                        0);
                }
                else
                {
                    // initLength < adjustLength
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count,
                        1);
                    {
                        var eventArg =
                            collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)][0];
                        Assert.AreEqual(eventArg.OldStartingIndex, -1);
                        Assert.AreEqual(eventArg.OldItems, null);
                        Assert.AreEqual(eventArg.NewStartingIndex, initLength);
                        Assert.AreEqual(eventArg.NewItems.Count, adjustLength - initLength);
                    }
                    Assert.AreEqual(
                        collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count,
                        0);
                }
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

        [TestCase(TestClassType.Type1, 0)]
        [TestCase(TestClassType.Type1, 10)]
        [TestCase(TestClassType.Type2, 5)]
        [TestCase(TestClassType.Type2, 10)]
        public static void ClearTest(TestClassType classType, int initLength)
        {
            var initList = MakeStringList(initLength);

            AbsCollectionTest instance = null;
            Dictionary<string, int> countDic = null;
            Dictionary<string, Dictionary<string, int>> handlerCalledCount = null;
            Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList = null;
            Dictionary<string, int> propertyChangedEventCalledCount = null;
            switch (classType)
            {
                case TestClassType.Type1:
                    instance = MakeCollectionForMethodTest(initLength, out countDic,
                        out handlerCalledCount, out collectionChangedEventArgsList,
                        out propertyChangedEventCalledCount);
                    break;
                case TestClassType.Type2:
                    instance = MakeCollection2ForMethodTest(initList, initLength, out countDic,
                        out handlerCalledCount, out collectionChangedEventArgsList,
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

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], minCapacity);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 1);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 1);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが意図した回数呼ばれていること
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 1);
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

        [TestCase(5, "1", true)]
        [TestCase(5, "6", false)]
        [TestCase(5, null, false)]
        public static void ContainsTest(int initLength, string item, bool result)
        {
            var instance = MakeCollectionForMethodTest(initLength, out var countDic,
                out var handlerCalledCount, out var collectionChangedEventArgsList,
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

            // 各Virtualメソッドが呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnInsertItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnMoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnRemoveItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 各イベントが呼ばれていないこと
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Replace)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Move)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Add)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Remove)].Count, 0);
            Assert.AreEqual(collectionChangedEventArgsList[nameof(NotifyCollectionChangedAction.Reset)].Count, 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.Count)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);
        }

        [TestCase(5, "1", 1)]
        [TestCase(5, "6", -1)]
        [TestCase(5, null, -1)]
        public static void IndexOfTest(int initLength, string item, int result)
        {
            var instance = MakeCollectionForMethodTest(initLength, out _, out _, out _, out _);
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
            for (var i = 0; i < initLength; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, false)]
        [TestCase(0, 0, 1, true)]
        [TestCase(1, 0, -1, true)]
        [TestCase(1, 0, 0, true)]
        [TestCase(1, 0, 1, true)]
        [TestCase(1, 1, -1, true)]
        [TestCase(1, 1, 0, false)]
        [TestCase(1, 1, 1, true)]
        [TestCase(1, 2, -1, true)]
        [TestCase(1, 2, 0, false)]
        [TestCase(1, 2, 1, false)]
        public static void CopyToTest(int initLength, int arrayLength, int index, bool isError)
        {
            var instance = MakeCollectionForMethodTest(initLength, out _, out _, out _, out _);
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
            for (var j = 0; j < initLength; j++)
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
            for (; i < initLength; i++)
            {
                Assert.AreEqual(copyArray[i + index], i.ToString());
            }
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
        public static void EqualsTest_IReadOnlyRestrictedCapacityCollection()
        {
            {
                // すべての要素が Equal であるリストの比較
                var items = MakeStringList(10);
                var left = new CollectionTest1(items);
                IReadOnlyRestrictedCapacityCollection<string> right = new CollectionTest1(items);

                Assert.IsTrue(left.Equals(right));
            }
            {
                // 一つだけ要素が異なるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                var rightItems = MakeStringList(10);
                rightItems[2] = "NotEqual";
                IReadOnlyRestrictedCapacityCollection<string> right = new CollectionTest1(rightItems);

                Assert.IsFalse(left.Equals(right));
            }
            {
                // 要素数が異なるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                IReadOnlyRestrictedCapacityCollection<string> right = new CollectionTest1(MakeStringList(9));

                Assert.IsFalse(left.Equals(right));
            }
            {
                // nullとの比較
                var left = new CollectionTest1(MakeStringList(10));

                Assert.IsFalse(left.Equals((IReadOnlyRestrictedCapacityCollection<string>) null));
            }
        }

        [Test]
        public static void EqualsTest_RestrictedCapacityCollection()
        {
            {
                // すべての要素が Equal であるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                RestrictedCapacityCollection<string> right = new CollectionTest1(MakeStringList(10));

                Assert.IsTrue(left.Equals(right));
            }
            {
                // 一つだけ要素が異なるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                var rightItems = MakeStringList(10);
                rightItems[2] = "NotEqual";
                RestrictedCapacityCollection<string> right = new CollectionTest1(rightItems);

                Assert.IsFalse(left.Equals(right));
            }
            {
                // 要素数が異なるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                RestrictedCapacityCollection<string> right = new CollectionTest1(MakeStringList(9));

                Assert.IsFalse(left.Equals(right));
            }
            {
                // nullとの比較
                var left = new CollectionTest1(MakeStringList(10));

                Assert.IsFalse(left.Equals((RestrictedCapacityCollection<string>) null));
            }
        }

        [Test]
        public static void EqualsTest_IFixedLengthCollection()
        {
            {
                // すべての要素が  Equal であるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                IFixedLengthCollection<string> right = new CollectionTest7();
                left.ForEach((s, i) => right[i] = s);

                Assert.IsTrue(left.Equals(right));
            }
            {
                // 一つだけ要素が異なるリストの比較
                var left = new CollectionTest1(MakeStringList(10));
                IFixedLengthCollection<string> right = new CollectionTest7();
                left.ForEach((s, i) => right[i] = s);
                right[5] = "NotFound";

                Assert.IsFalse(left.Equals(right));
            }
            {
                // 要素数が異なるリストの比較
                var left = new CollectionTest1(MakeStringList(9));
                IFixedLengthCollection<string> right = new CollectionTest7();

                Assert.IsFalse(left.Equals(right));
            }
            {
                // nullとの比較
                var left = new CollectionTest1(MakeStringList(10));

                Assert.IsFalse(left.Equals((IFixedLengthCollection<string>) null));
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = MakeCollectionForMethodTest(5, out _, out _, out _, out _);
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
            out Dictionary<string, int> methodCalledCount,
            out Dictionary<string, Dictionary<string, int>> handlerCalledCount,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new CollectionTest1()
                : new CollectionTest1(initStringList);

            methodCalledCount = new Dictionary<string, int>
            {
                {nameof(CollectionTest1.OnSetItemCalled), 0},
                {nameof(CollectionTest1.OnInsertItemCalled), 0},
                {nameof(CollectionTest1.OnMoveItemCalled), 0},
                {nameof(CollectionTest1.OnRemoveItemCalled), 0},
                {nameof(CollectionTest1.OnClearItemsCalled), 0},
            };

            var ints = methodCalledCount;
            result.OnSetItemCalled = () => ints[nameof(CollectionTest1.OnSetItemCalled)]++;
            result.OnInsertItemCalled = () => ints[nameof(CollectionTest1.OnInsertItemCalled)]++;
            result.OnMoveItemCalled = () => ints[nameof(CollectionTest1.OnMoveItemCalled)]++;
            result.OnRemoveItemCalled = () => ints[nameof(CollectionTest1.OnRemoveItemCalled)]++;
            result.OnClearItemsCalled = () => ints[nameof(CollectionTest1.OnClearItemsCalled)]++;

            handlerCalledCount = new Dictionary<string, Dictionary<string, int>>
            {
                {
                    nameof(CollectionTest1.OnSetItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnInsertItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnRemoveItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnClearItemsCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
            };

            var hccs = handlerCalledCount;
            result.SetItemHandlerList.Add(
                new OnSetItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.SetItemHandlerList.Add(
                new OnSetItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );

            result.InsertItemHandlerList.Add(
                new OnInsertItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.InsertItemHandlerList.Add(
                new OnInsertItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString]++; },
                    bool.FalseString, false, false
                )
            );
            result.RemoveItemHandlerList.Add(
                new OnRemoveItemHandler<string>(
                    i => { hccs[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.RemoveItemHandlerList.Add(
                new OnRemoveItemHandler<string>(
                    i => { hccs[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );
            result.ClearItemHandlerList.Add(
                new OnClearItemHandler<string>(
                    () => { hccs[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString]++; }, bool.TrueString,
                    false
                )
            );
            result.ClearItemHandlerList.Add(
                new OnClearItemHandler<string>(
                    () => { hccs[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );

            {
                var makeEmptyList =
                    new Func<List<NotifyCollectionChangedEventArgs>>(() =>
                        new List<NotifyCollectionChangedEventArgs>());

                collectionChangedEventArgsList = new Dictionary<string, List<NotifyCollectionChangedEventArgs>>
                {
                    {nameof(NotifyCollectionChangedAction.Add), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Replace), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Remove), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Reset), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Move), makeEmptyList()},
                };
                var cceaList = collectionChangedEventArgsList;

                result.CollectionChanged += (sender, args) =>
                {
                    cceaList[args.Action.ToString()].Add(args);
                    logger.Debug($"{nameof(args)}: {{");
                    logger.Debug($"    {nameof(args.Action)}: {args.Action}");
                    logger.Debug($"    {nameof(args.OldStartingIndex)}: {args.OldStartingIndex}");
                    logger.Debug($"    {nameof(args.OldItems)}: {args.OldItems}");
                    logger.Debug($"    {nameof(args.NewStartingIndex)}: {args.NewStartingIndex}");
                    logger.Debug($"    {nameof(args.NewItems)}: {args.NewItems}");
                    logger.Debug("}");
                };
            }

            {
                propertyChangedEventCalledCount = new Dictionary<string, int>
                {
                    {nameof(result.Count), 0},
                    {ListConstant.IndexerName, 0},
                };
                var pceaList = propertyChangedEventCalledCount;

                result.PropertyChanged += (sender, args) =>
                {
                    pceaList[args.PropertyName] += 1;
                    logger.Debug($"{nameof(args)}: {{");
                    logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                    logger.Debug("}");
                };
            }

            return result;
        }

        private static CollectionTest2 MakeCollection2ForMethodTest(List<string> initStringList, int initLength,
            out Dictionary<string, int> methodCalledCount,
            out Dictionary<string, Dictionary<string, int>> handlerCalledCount,
            out Dictionary<string, List<NotifyCollectionChangedEventArgs>> collectionChangedEventArgsList,
            out Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var result = initStringList == null
                ? new CollectionTest2()
                : new CollectionTest2(initStringList.GetRange(0, initLength));

            methodCalledCount = new Dictionary<string, int>
            {
                {nameof(CollectionTest1.OnSetItemCalled), 0},
                {nameof(CollectionTest1.OnInsertItemCalled), 0},
                {nameof(CollectionTest1.OnMoveItemCalled), 0},
                {nameof(CollectionTest1.OnRemoveItemCalled), 0},
                {nameof(CollectionTest1.OnClearItemsCalled), 0},
            };

            var ints = methodCalledCount;
            result.OnSetItemCalled = () => ints[nameof(CollectionTest1.OnSetItemCalled)]++;
            result.OnInsertItemCalled = () => ints[nameof(CollectionTest1.OnInsertItemCalled)]++;
            result.OnMoveItemCalled = () => ints[nameof(CollectionTest1.OnMoveItemCalled)]++;
            result.OnRemoveItemCalled = () => ints[nameof(CollectionTest1.OnRemoveItemCalled)]++;
            result.OnClearItemsCalled = () => ints[nameof(CollectionTest1.OnClearItemsCalled)]++;

            handlerCalledCount = new Dictionary<string, Dictionary<string, int>>
            {
                {
                    nameof(CollectionTest1.OnSetItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnInsertItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnRemoveItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnClearItemsCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
            };

            var hccs = handlerCalledCount;
            result.SetItemHandlerList.Add(
                new OnSetItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.SetItemHandlerList.Add(
                new OnSetItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );

            result.InsertItemHandlerList.Add(
                new OnInsertItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnInsertItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.InsertItemHandlerList.Add(
                new OnInsertItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnInsertItemCalled)][bool.FalseString]++; },
                    bool.FalseString, false, false
                )
            );
            result.RemoveItemHandlerList.Add(
                new OnRemoveItemHandler<string>(
                    i => { hccs[nameof(CollectionTest1.OnRemoveItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.RemoveItemHandlerList.Add(
                new OnRemoveItemHandler<string>(
                    i => { hccs[nameof(CollectionTest1.OnRemoveItemCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );
            result.ClearItemHandlerList.Add(
                new OnClearItemHandler<string>(
                    () => { hccs[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString]++; }, bool.TrueString,
                    false
                )
            );
            result.ClearItemHandlerList.Add(
                new OnClearItemHandler<string>(
                    () => { hccs[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );

            {
                var makeEmptyList =
                    new Func<List<NotifyCollectionChangedEventArgs>>(() =>
                        new List<NotifyCollectionChangedEventArgs>());

                collectionChangedEventArgsList = new Dictionary<string, List<NotifyCollectionChangedEventArgs>>
                {
                    {nameof(NotifyCollectionChangedAction.Add), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Replace), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Remove), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Reset), makeEmptyList()},
                    {nameof(NotifyCollectionChangedAction.Move), makeEmptyList()},
                };
                var cceaList = collectionChangedEventArgsList;

                result.CollectionChanged += (sender, args) =>
                {
                    cceaList[args.Action.ToString()].Add(args);
                    logger.Debug($"{nameof(args)}: {{");
                    logger.Debug($"    {nameof(args.Action)}: {args.Action}");
                    logger.Debug($"    {nameof(args.OldStartingIndex)}: {args.OldStartingIndex}");
                    logger.Debug($"    {nameof(args.OldItems)}: {args.OldItems}");
                    logger.Debug($"    {nameof(args.NewStartingIndex)}: {args.NewStartingIndex}");
                    logger.Debug($"    {nameof(args.NewItems)}: {args.NewItems}");
                    logger.Debug("}");
                };
            }

            {
                propertyChangedEventCalledCount = new Dictionary<string, int>
                {
                    {nameof(result.Count), 0},
                    {ListConstant.IndexerName, 0},
                };
                var pceaList = propertyChangedEventCalledCount;

                result.PropertyChanged += (sender, args) =>
                {
                    pceaList[args.PropertyName] += 1;
                    logger.Debug($"{nameof(args)}: {{");
                    logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                    logger.Debug("}");
                };
            }

            return result;
        }

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

        private abstract class AbsCollectionTest : RestrictedCapacityCollection<string>
        {
            public AbsCollectionTest()
            {
            }

            public AbsCollectionTest(IReadOnlyCollection<string> list) : base(list)
            {
            }

            // RestrictedCapacityCollection<T>継承クラスはデシリアライズ時に呼び出せるconstructor(SerializationInfo, StreamingContext)が必須
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

            public static int MaxCapacity => 10;

            public static int MinCapacity => 0;
            public static string Default => "test";

            [field: NonSerialized] public Action OnSetItemCalled { get; set; }
            [field: NonSerialized] public Action OnInsertItemCalled { get; set; }
            [field: NonSerialized] public Action OnMoveItemCalled { get; set; }
            [field: NonSerialized] public Action OnRemoveItemCalled { get; set; }
            [field: NonSerialized] public Action OnClearItemsCalled { get; set; }

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest1()
            {
            }

            public CollectionTest1(IReadOnlyCollection<string> list) : base(list)
            {
            }

            protected override void SetItem(int index, string item)
            {
                base.SetItem(index, item);
                OnSetItemCalled?.Invoke();
            }

            protected override void InsertItem(int index, string item)
            {
                base.InsertItem(index, item);
                OnInsertItemCalled?.Invoke();
            }

            protected override void MoveItem(int oldIndex, int newIndex)
            {
                base.MoveItem(oldIndex, newIndex);
                OnMoveItemCalled?.Invoke();
            }

            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
                OnRemoveItemCalled?.Invoke();
            }

            protected override void ClearItems()
            {
                base.ClearItems();
                OnClearItemsCalled?.Invoke();
            }

            protected CollectionTest1(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        private class CollectionTest2 : AbsCollectionTest
        {
            /**
             * 正常設定
             * 初期要素数非0
             */

            public static int MaxCapacity => 10;

            public static int MinCapacity => 5;
            public static string Default => "test";

            public Action OnSetItemCalled { get; set; }
            public Action OnInsertItemCalled { get; set; }
            public Action OnMoveItemCalled { get; set; }
            public Action OnRemoveItemCalled { get; set; }
            public Action OnClearItemsCalled { get; set; }

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest2()
            {
            }

            public CollectionTest2(IReadOnlyCollection<string> list) : base(list)
            {
            }

            protected override void SetItem(int index, string item)
            {
                base.SetItem(index, item);
                OnSetItemCalled?.Invoke();
            }

            protected override void InsertItem(int index, string item)
            {
                base.InsertItem(index, item);
                OnInsertItemCalled?.Invoke();
            }

            protected override void MoveItem(int oldIndex, int newIndex)
            {
                base.MoveItem(oldIndex, newIndex);
                OnMoveItemCalled?.Invoke();
            }

            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
                OnRemoveItemCalled?.Invoke();
            }

            protected override void ClearItems()
            {
                base.ClearItems();
                OnClearItemsCalled?.Invoke();
            }
        }

        private class CollectionTest3 : AbsCollectionTest
        {
            /**
             * 正常設定
             * 初期要素数非0
             * MinCapacity = MaxCapacity
             */

            public static int MaxCapacity => 10;

            public static int MinCapacity => 10;
            public static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

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
            /*
             * 異常設定（MinCapacity < 0）
             */

            public static int MaxCapacity => 10;
            public static int MinCapacity => -2;
            public static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest4()
            {
            }

            public CollectionTest4(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class CollectionTest5 : AbsCollectionTest
        {
            /**
             * 異常設定（MinCapacity > MaxCapacity）
             */

            public static int MaxCapacity => 10;

            public static int MinCapacity => 11;
            public static string Default => "test";

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest5()
            {
            }

            public CollectionTest5(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class CollectionTest6 : AbsCollectionTest
        {
            /**
             * 異常設定（DefaultValue＝null）
             */
            public static int MaxCapacity => 10;

            public static int MinCapacity => 0;
            public static string Default => null;

            public override int GetMaxCapacity() => MaxCapacity;

            public override int GetMinCapacity() => MinCapacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest6()
            {
            }

            public CollectionTest6(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class CollectionTest7 : FixedLengthList<string>
        {
            public override int GetCapacity() => 10;

            protected override string MakeDefaultItem(int index) => index.ToString();
        }
    }
}