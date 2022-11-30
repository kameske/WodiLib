using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using LinqExtension = Commons.Linq.Extension.LinqExtension;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class SimpleListTest
    {
        private static Logger logger = default!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void BaseClassTest()
        {
            // ObservableCollection を継承していること
            Assert.IsTrue(typeof(SimpleList<object>).IsSubclassOf(typeof(ObservableCollection<object>)));
        }

        [Test]
        public static void ConstructorTestA_NoInitItems()
        {
            TestTemplate.Constructor(
                () => new SimpleList<StubModel>(InitInstance.MakeListDefaultItem),
                expectedThrowCreateNewInstance: false,
                logger
            );
        }

        private static readonly object[] ConstructorTestBTestCaseSource =
        {
            new object[]
            {
                new[]
                {
                    new StubModel(),
                    new StubModel("item"),
                    new StubModel("テスト"),
                },
            },
            new object[]
            {
                null!
            },
        };

        [TestCaseSource(nameof(ConstructorTestBTestCaseSource))]
        public static void ConstructorTestB_InitItemIsDeepCloneable(
            IEnumerable<StubModel>? initValues
        )
        {
            var initValuesArray = initValues?.ToArray();

            var createResult = TestTemplate.Constructor(
                () => new SimpleList<StubModel>(InitInstance.MakeListDefaultItem, initValuesArray),
                expectedThrowCreateNewInstance: false,
                logger
            );

            if (initValuesArray != null)
            {
                // SimpleListの要素がコンストラクタで指定した要素と同一であること

                Assert.IsTrue(createResult.Count == initValuesArray.Length);

                LinqExtension.ForEach(
                    WodiLib.Sys.LinqExtension.Zip(createResult, initValuesArray),
                    zip =>
                    {
                        var (createdItem, originalItem) = zip;
                        Assert.IsTrue(ReferenceEquals(createdItem, originalItem));
                    }
                );
            }
            else
            {
                // 空要素であること
                Assert.IsTrue(createResult.Count == 0);
            }
        }

        [Test]
        public static void GetTest()
        {
            var (instance, collectionList) = InitInstance.Generate();

            const int index = 1;
            const int count = 3;

            var isExpectedResult = new Func<IEnumerable<StubModel>, bool>(
                result =>
                {
                    var resultArray = result.ToArray();
                    // 意図した範囲の要素が取得できていること
                    return resultArray.Length == count
                           && resultArray[0].ItemEquals(InitInstance.InitItems[index + 0])
                           && resultArray[1].ItemEquals(InitInstance.InitItems[index + 1])
                           && resultArray[2].ItemEquals(InitInstance.InitItems[index + 2]);
                }
            );

            TestTemplate.PureMethod(
                instance,
                target => target.Get(index, count),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void SetTest_ItemIsEmpty()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 1;
            // 何も通知されないこと
            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Set(index, Array.Empty<StubModel>()),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void SetTest_ItemIsSingle()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 1;
            var setItem = new StubModel("update item 1");

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した範囲の値が更新されていること
                          && target[1].ItemEquals(setItem)
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Set(index, setItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Replace);
        }

        [Test]
        public static void SetTest_ItemIsMulti()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 1;
            var setItems = new StubModel[]
            {
                new("update item 1"),
                new("update item 2"),
            };

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した範囲の値が更新されていること
                          && target[1].ItemEquals(setItems[0])
                          && target[2].ItemEquals(setItems[1])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Set(index, setItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void AddTest_ItemIsEmpty()
        {
            var (instance, collectionList) = InitInstance.Generate();

            // 何も通知されないこと
            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 既存の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Add(Array.Empty<StubModel>()),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void AddTest_ItemIsSingle()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var addItem = new StubModel("add item");

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 1
                          // 既存の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 要素が正しく追加されていること
                          && target[5].ItemEquals(addItem)
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Add(addItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void AddTest_ItemIsSingleInArray()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var addItem = new StubModel("add item");

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 1
                          // 既存の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 要素が正しく追加されていること
                          && target[5].ItemEquals(addItem)
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Add(addItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void AddTest_ItemIsMulti()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var addItems = new StubModel[]
            {
                new("add item 1"),
                new("add item 2"),
            };

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 2
                          // 既存の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 要素が正しく追加されていること
                          && target[5].ItemEquals(addItems[0])
                          && target[6].ItemEquals(addItems[1])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Add(addItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void InsertTest_ItemIsEmpty()
        {
            var (instance, collectionList) = InitInstance.Generate();

            // 何も通知されないこと
            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 既存の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Insert(0, Array.Empty<StubModel>()),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void InsertTest_ItemIsSingle()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var insertItem = new StubModel("add item");

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 1
                          // 既存の値が更新されていないこと（挿入箇所より前）
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          // 要素が正しく挿入加されていること
                          && target[2].ItemEquals(insertItem)
                          // 既存の値が更新されていないこと（挿入箇所より後）
                          && target[3].ItemEquals(InitInstance.InitItems[2])
                          && target[4].ItemEquals(InitInstance.InitItems[3])
                          && target[5].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Insert(2, insertItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void InsertTest_ItemIsMulti()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var insertItems = new StubModel[]
            {
                new("add item 1"),
                new("add item 2"),
            };

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 2
                          // 既存の値が更新されていないこと（挿入箇所より前）
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          // 要素が正しく挿入加されていること
                          && target[2].ItemEquals(insertItems[0])
                          && target[3].ItemEquals(insertItems[1])
                          // 既存の値が更新されていないこと（挿入箇所より後）
                          && target[4].ItemEquals(InitInstance.InitItems[2])
                          && target[5].ItemEquals(InitInstance.InitItems[3])
                          && target[6].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Insert(2, insertItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void OverwriteTest_ItemIsEmpty()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 1;
            // 何も通知されないこと
            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Overwrite(index, Array.Empty<StubModel>()),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void OverwriteTest_ItemIsSingle_OnlyReplace()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 1;
            var overwriteItem = new StubModel("overwrite item 1");

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した範囲の値が更新されていること
                          && target[1].ItemEquals(overwriteItem)
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Overwrite(index, overwriteItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Replace);
        }

        [Test]
        public static void OverwriteTest_ItemIsSingle_OnlyAdd()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 5;
            var overwriteItem = new StubModel("overwrite item 1");

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 1
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した範囲の値が更新されていること
                          && target[5].ItemEquals(overwriteItem)
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Overwrite(index, overwriteItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void OverwriteTest_ItemIsMulti_OnlyReplace()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 1;
            var overwriteItems = new StubModel[]
            {
                new("overwrite item 1"),
                new("overwrite item 2"),
            };

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した範囲の値が更新されていること
                          && target[1].ItemEquals(overwriteItems[0])
                          && target[2].ItemEquals(overwriteItems[1])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Overwrite(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void OverwriteTest_ItemIsMulti_OnlyAdd()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 5;
            var overwriteItems = new StubModel[]
            {
                new("overwrite item 1"),
                new("overwrite item 2"),
            };

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 2
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した範囲の値が更新されていること
                          && target[5].ItemEquals(overwriteItems[0])
                          && target[6].ItemEquals(overwriteItems[1])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Overwrite(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void OverwriteTest_ItemIsMulti_ReplaceAndAdd()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 4;
            var overwriteItems = new StubModel[]
            {
                new("overwrite item 1"),
                new("overwrite item 2"),
            };

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength + 1
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          // 意図した範囲の値が更新されていること
                          && target[4].ItemEquals(overwriteItems[0])
                          && target[5].ItemEquals(overwriteItems[1])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Overwrite(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void MoveTest_CountIsZero()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int oldIndex = 2;
            const int newIndex = 4;
            const int count = 0;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Move(oldIndex, newIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void MoveTest_CountIsOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int oldIndex = 2;
            const int newIndex = 4;
            const int count = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 要素が正しく移動されていること
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[3])
                          && target[3].ItemEquals(InitInstance.InitItems[4])
                          && target[4].ItemEquals(InitInstance.InitItems[2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Move(oldIndex, newIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Move);
        }

        [Test]
        public static void MoveTest_CountIsTwo()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int oldIndex = 3;
            const int newIndex = 0;
            const int count = 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 要素が正しく移動されていること
                          && target[0].ItemEquals(InitInstance.InitItems[3])
                          && target[1].ItemEquals(InitInstance.InitItems[4])
                          && target[2].ItemEquals(InitInstance.InitItems[0])
                          && target[3].ItemEquals(InitInstance.InitItems[1])
                          && target[4].ItemEquals(InitInstance.InitItems[2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Move(oldIndex, newIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void RemoveTest_CountIsZero()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 2;
            const int count = 0;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Remove(index, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void RemoveTest_CountIsOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 2;
            const int count = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count)
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength - 1
                          // 要素が正しく削除されていること
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[3])
                          && target[3].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Remove(index, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Remove);
        }

        [Test]
        public static void RemoveTest_CountIsTwo()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int index = 2;
            const int count = 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count)
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength - 2
                          // 要素が正しく削除されていること
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Remove(index, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void AdjustTest_AddOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 6;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した要素が追加されていること
                          && target[5].ItemEquals(InitInstance.GenerateTestModel(5))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Adjust(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void AdjustTest_AddTwo()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 7;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した要素が追加されていること
                          && target[5].ItemEquals(InitInstance.GenerateTestModel(5))
                          && target[6].ItemEquals(InitInstance.GenerateTestModel(6))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Adjust(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void AdjustTest_NoAddAndRemove()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 5;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Adjust(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void AdjustTest_RemoveOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 4;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Adjust(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Remove);
        }

        [Test]
        public static void AdjustTest_RemoveTwo()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 3;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Adjust(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void AdjustIfLongTest_AddOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 6;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfLong(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void AdjustIfLongTest_NoAddAndRemove()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 5;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfLong(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void AdjustIfLongTest_RemoveOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 4;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfLong(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Remove);
        }

        [Test]
        public static void AdjustIfLongTest_RemoveTwo()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 3;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfLong(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void AdjustIfShortTest_AddOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 6;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した要素が追加されていること
                          && target[5].ItemEquals(InitInstance.GenerateTestModel(5))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfShort(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Add);
        }

        [Test]
        public static void AdjustIfShortTest_AddTwo()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 7;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
                          // 意図した要素が追加されていること
                          && target[5].ItemEquals(InitInstance.GenerateTestModel(5))
                          && target[6].ItemEquals(InitInstance.GenerateTestModel(6))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfShort(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void AdjustIfShortTest_NoAddAndRemove()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 5;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == length
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfShort(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void AdjustIfShortTest_RemoveOne()
        {
            var (instance, collectionList) = InitInstance.Generate();
            const int length = 4;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == InitInstance.InitLength
                          // 意図しない範囲の値が更新されていないこと
                          && target[0].ItemEquals(InitInstance.InitItems[0])
                          && target[1].ItemEquals(InitInstance.InitItems[1])
                          && target[2].ItemEquals(InitInstance.InitItems[2])
                          && target[3].ItemEquals(InitInstance.InitItems[3])
                          && target[4].ItemEquals(InitInstance.InitItems[4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustIfShort(length),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void ResetTest_Empty()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var resetItems = Array.Empty<StubModel>();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.Count),
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == 0
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Reset(resetItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void ResetTest_CountNotChange()
        {
            var (instance, collectionList) = InitInstance.Generate();
            var resetItems = InitInstance.InitLength.Iterate(i => InitInstance.GenerateTestModel(i + 100)).ToArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<ISimpleList<StubModel>, bool>(
                target => target.Count == resetItems.Length
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.Reset(resetItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // 意図したコレクション変更通知が起きていること
            Assert.IsTrue(collectionList.Count == 1);
            Assert.IsTrue(collectionList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        private static readonly object[] ItemEqualsTest1CaseSource =
        {
            new object?[]
            {
                null, false,
            },
            new object[]
            {
                Array.Empty<StubModel>(), false
            },
            new object[]
            {
                InitInstance.InitItems.Take(4).ToArray(), false
            },
            new object[]
            {
                InitInstance.InitItems, true
            },
            new object[]
            {
                InitInstance.InitItems.Moved(0, 1).ToArray(), false
            },
            new object[]
            {
                InitInstance.InitItems.Added(new StubModel("append")).ToArray(), false
            },
        };

        [TestCaseSource(nameof(ItemEqualsTest1CaseSource))]
        public static void ItemEqualsTest1_TestModelArray(StubModel[]? otherItems, bool expected)
        {
            var (instance, collectionList) = InitInstance.Generate();
            var other = otherItems is null
                ? null
                : new SimpleList<StubModel>(InitInstance.MakeListDefaultItem, otherItems);

            var isExpectedResult = new Func<bool, bool>(
                actual => actual == expected
            );

            TestTemplate.PureMethod(
                instance,
                target => target.ItemEquals(other),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        private static readonly object[] ItemEqualsTest2CaseSource =
        {
            new object?[]
            {
                null, false,
            },
            new object[]
            {
                Array.Empty<StubModel>(), false
            },
            new object[]
            {
                InitInstance.InitItems.Take(4).ToArray(), false
            },
            new object[]
            {
                InitInstance.InitItems, true
            },
            new object[]
            {
                InitInstance.InitItems.Moved(0, 1).ToArray(), false
            },
            new object[]
            {
                InitInstance.InitItems.Added(new StubModel("append")).ToArray(), false
            },
            new object[]
            {
                InitInstance.Generate().instance, true
            },
            new object[]
            {
                new SimpleList<StubModel>(InitInstance.MakeListDefaultItem, InitInstance.InitItems.Take(4)), false
            },
        };

        [TestCaseSource(nameof(ItemEqualsTest2CaseSource))]
        public static void ItemEqualsTest_object(object? other, bool expected)
        {
            var (instance, collectionList) = InitInstance.Generate();

            var isExpectedResult = new Func<bool, bool>(
                actual => actual == expected
            );

            TestTemplate.PureMethod(
                instance,
                target => target.ItemEquals(other),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        [Test]
        public static void DeepCloneTest()
        {
            var (instance, collectionList) = InitInstance.Generate();

            TestTemplate.DeepClone<ISimpleList<StubModel>>(
                instance,
                logger
            );

            // コレクション変更通知が起きていないこと
            Assert.IsTrue(collectionList.Count == 0);
        }

        private static class InitInstance
        {
            public static readonly StubModel[] InitItems =
            {
                new("InitStr"),
                new("\t_"),
                new("初期文字列"),
                new("Init String"),
                new("string123"),
            };

            public static readonly DelegateMakeListDefaultItem<StubModel> MakeListDefaultItem = GenerateTestModel;

            public static int InitLength => InitItems.Length;

            public static (SimpleList<StubModel> instance, List<NotifyCollectionChangedEventArgs>
                raiseCollectionChangeEventArgsList) Generate()
            {
                var raiseCollectionChangeEventArgsList = new List<NotifyCollectionChangedEventArgs>();
                var instance = new SimpleList<StubModel>(MakeListDefaultItem, InitItems);
                instance.CollectionChanged += (_, args) => raiseCollectionChangeEventArgsList.Add(args);
                return (instance, raiseCollectionChangeEventArgsList);
            }

            public static StubModel GenerateTestModel(int index) => new($"{index}");
        }
    }
}
