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
    public class TwoDimensionalListTest
    {
        private static Logger logger = default!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region Notify

        #region PropertyChanged

        [Test]
        public static void NotifyPropertyChangedTest_ChangeSingleRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedPropertyNameList = new List<string>();
            instance.PropertyChanged += (_, args) => notifiedPropertyNameList.Add(args.PropertyName);

            instance.SetRowRangeCore(0, InitInstance.GenerateRows(1, InitInstance.InitColumnLength));

            // 通知が行われていること
            Assert.IsTrue(notifiedPropertyNameList.Count == 1);
            Assert.IsTrue(notifiedPropertyNameList[0] == ListConstant.IndexerName);
        }

        [Test]
        public static void NotifyPropertyChangedTest_ChangeMultiRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedPropertyNameList = new List<string>();
            instance.PropertyChanged += (_, args) => notifiedPropertyNameList.Add(args.PropertyName);

            instance.SetRowRangeCore(0, InitInstance.GenerateRows(2, InitInstance.InitColumnLength));

            // 通知が行われていること
            Assert.IsTrue(notifiedPropertyNameList.Count == 1);
            Assert.IsTrue(notifiedPropertyNameList[0] == ListConstant.IndexerName);
        }

        [Test]
        public static void NotifyPropertyChangedTest_ChangeColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedPropertyNameList = new List<string>();
            instance.PropertyChanged += (_, args) => notifiedPropertyNameList.Add(args.PropertyName);

            instance.SetColumnRangeCore(0, InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitRowLength));

            // 通知が行われていること
            Assert.IsTrue(notifiedPropertyNameList.Count == 1);
            Assert.IsTrue(notifiedPropertyNameList[0] == ListConstant.IndexerName);
        }

        [Test]
        public static void NotifyPropertyChangedTest_ChangeOneItem()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedPropertyNameList = new List<string>();
            instance.PropertyChanged += (_, args) => notifiedPropertyNameList.Add(args.PropertyName);

            instance.SetItemCore(0, 0, new StubModel());

            // 通知が行われていること
            Assert.IsTrue(notifiedPropertyNameList.Count == 1);
            Assert.IsTrue(notifiedPropertyNameList[0] == ListConstant.IndexerName);
        }

        #endregion

        #region CollectionChanged

        [Test]
        public static void CollectionChangedTest_ChangeSingleRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedCollectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (_, args) => notifiedCollectionChangedEventArgsList.Add(args);

            instance.SetRowRangeCore(0, InitInstance.GenerateRows(1, InitInstance.InitColumnLength));

            // 通知が行われていること
            Assert.IsTrue(notifiedCollectionChangedEventArgsList.Count == 1);
            Assert.IsTrue(notifiedCollectionChangedEventArgsList[0].Action == NotifyCollectionChangedAction.Replace);
        }

        [Test]
        public static void CollectionChangedTest_ChangeMultiRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedCollectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (_, args) => notifiedCollectionChangedEventArgsList.Add(args);

            instance.SetRowRangeCore(0, InitInstance.GenerateRows(2, InitInstance.InitColumnLength));

            // 通知が行われていること
            Assert.IsTrue(notifiedCollectionChangedEventArgsList.Count == 1);
            Assert.IsTrue(notifiedCollectionChangedEventArgsList[0].Action == NotifyCollectionChangedAction.Reset);
        }

        [Test]
        public static void CollectionChangedTest_ChangeColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedCollectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (_, args) => notifiedCollectionChangedEventArgsList.Add(args);

            instance.SetColumnRangeCore(0, InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength));

            // 通知が行われていないこと
            Assert.IsTrue(notifiedCollectionChangedEventArgsList.Count == 0);
        }

        [Test]
        public static void CollectionChangedTest_ChangeItem()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var notifiedCollectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (_, args) => notifiedCollectionChangedEventArgsList.Add(args);

            instance.SetItemCore(0, 0, new StubModel());

            // 通知が行われていないこと
            Assert.IsTrue(notifiedCollectionChangedEventArgsList.Count == 0);
        }

        #endregion

        #endregion

        #region Implementations

        [TestCase(0, 0, true)]
        [TestCase(1, 0, false)]
        [TestCase(1, 1, false)]
        public static void IsEmptyGetterTest(int rowLength, int columnLength, bool expected)
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest(
                rowLength,
                columnLength
            );

            TestTemplate.PropertyGet(
                instance,
                propertyGetter: target => target.IsEmpty,
                expectedThrowActPropertyGet: false,
                isExpectedItem: actual => actual == expected,
                logger
            );
        }

        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(1, 3)]
        public static void ColumnCountGetterTest(int rowLength, int columnLength)
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest(
                rowLength,
                columnLength
            );

            TestTemplate.PropertyGet(
                instance,
                propertyGetter: target => target.ColumnCount,
                expectedThrowActPropertyGet: false,
                isExpectedItem: actual => actual == columnLength,
                logger
            );
        }

        [Test]
        public static void ValidatorGetterTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            TestTemplate.PropertyGet(
                instance,
                propertyGetter: target => target.Validator,
                expectedThrowActPropertyGet: false,
                /* TwoDimensionalListForImplementationsTest 内部で validator に MockTwoDimensionalListValidator<InitInstance.ExtendedListForRow, StubModel> を指定している */
                isExpectedItem: actual
                    => actual is MockTwoDimensionalListValidator<InitInstance.ExtendedListForRow, StubModel>,
                logger
            );
        }

        [Test]
        public static void GetMaxRowCapacityTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var isExpectedResult = new Func<int, bool>(
                result => result == config.MaxRowCapacity
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetMaxRowCapacity(),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void GetMinRowCapacityTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var isExpectedResult = new Func<int, bool>(
                result => result == config.MinRowCapacity
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetMinRowCapacity(),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void GetMaxColumnCapacityTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var isExpectedResult = new Func<int, bool>(
                result => result == config.MaxColumnCapacity
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetMaxColumnCapacity(),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void GetMinColumnCapacityTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var isExpectedResult = new Func<int, bool>(
                result => result == config.MinColumnCapacity
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetMinColumnCapacity(),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void ConstructorTest_ExtendedLists()
        {
            var initItems = InitInstance.GenerateRows(InitInstance.InitRowLength, InitInstance.InitColumnLength)
                .ToArray();

            var instance = TestTemplate.Constructor(
                factory: () => new InitInstance.TwoDimensionalListForImplementationsTest(initItems),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期化された各種要素が正しいこと
            Assert.IsTrue(instance.RowCount == InitInstance.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == InitInstance.InitColumnLength);
            for (var r = 0; r < InitInstance.InitRowLength; r++)
            for (var c = 0; c < InitInstance.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(initItems[r][c]));
            }
        }

        [Test]
        public static void ConstructorTest_ITwoDimItems()
        {
            var initItems = InitInstance.GenerateRows(InitInstance.InitRowLength, InitInstance.InitColumnLength)
                .ToTwoDimensionalArray();

            var instance = TestTemplate.Constructor(
                factory: () => new InitInstance.TwoDimensionalListForImplementationsTest(initItems),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期化された各種要素が正しいこと
            Assert.IsTrue(instance.RowCount == InitInstance.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == InitInstance.InitColumnLength);
            for (var r = 0; r < InitInstance.InitRowLength; r++)
            for (var c = 0; c < InitInstance.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(initItems[r][c]));
            }
        }

        [Test]
        public static void ConstructorTest_Length()
        {
            var instance = TestTemplate.Constructor(
                factory: () => new InitInstance.TwoDimensionalListForImplementationsTest(
                    InitInstance.InitRowLength,
                    InitInstance.InitColumnLength
                ),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期化された行数/列数が正しいこと
            Assert.IsTrue(instance.RowCount == InitInstance.InitRowLength);
            Assert.IsTrue(instance.ColumnCount == InitInstance.InitColumnLength);
        }

        [Test]
        public static void ConstructorTest_OnlyConfig()
        {
            var instance = TestTemplate.Constructor(
                factory: () => new InitInstance.TwoDimensionalListForImplementationsTest(),
                expectedThrowCreateNewInstance: false,
                logger
            );
            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            // 初期化された行数/列数が最小の行数/列数と同じであること
            Assert.IsTrue(instance.RowCount == config.MinRowCapacity);
            Assert.IsTrue(instance.ColumnCount == config.MinColumnCapacity);
        }

        [Test]
        public static void GetRowRangeCoreTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var startRow = 1;
            var count = 2;

            var isExpectedResult = new Func<IEnumerable<InitInstance.ExtendedListForRow>, bool>(
                result =>
                {
                    var resultArray = result.ToTwoDimensionalArray();
                    return resultArray.Length == 2
                           && resultArray[0].Length == InitInstance.InitColumnLength
                           && resultArray[0][0].ItemEquals(instance[1][0])
                           && resultArray[0][1].ItemEquals(instance[1][1])
                           && resultArray[0][2].ItemEquals(instance[1][2])
                           && resultArray[1].Length == InitInstance.InitColumnLength
                           && resultArray[1][0].ItemEquals(instance[2][0])
                           && resultArray[1][1].ItemEquals(instance[2][1])
                           && resultArray[1][2].ItemEquals(instance[2][2]);
                }
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetRowRangeCore(startRow, count),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void GetColumnRangeCoreTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var startColumn = 1;
            var count = 2;

            var isExpectedResult = new Func<IEnumerable<IEnumerable<StubModel>>, bool>(
                result =>
                {
                    var resultArray = result.ToTwoDimensionalArray();
                    return resultArray.Length == 2
                           && resultArray[0].Length == InitInstance.InitRowLength
                           && resultArray[0][0].ItemEquals(instance[0][1])
                           && resultArray[0][1].ItemEquals(instance[1][1])
                           && resultArray[0][2].ItemEquals(instance[2][1])
                           && resultArray[0][3].ItemEquals(instance[3][1])
                           && resultArray[1].Length == InitInstance.InitRowLength
                           && resultArray[1][0].ItemEquals(instance[0][2])
                           && resultArray[1][1].ItemEquals(instance[1][2])
                           && resultArray[1][2].ItemEquals(instance[2][2])
                           && resultArray[1][3].ItemEquals(instance[3][2]);
                }
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetColumnRangeCore(startColumn, count),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void GetItemCoreTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var rowIndex = 1;
            var rowCount = 3;
            var columnIndex = 2;
            var columnCount = 2;

            var isExpectedResult = new Func<IEnumerable<IEnumerable<StubModel>>, bool>(
                result =>
                {
                    var resultArray = result.ToTwoDimensionalArray();
                    return resultArray.Length == rowCount
                           && resultArray[0].Length == columnCount
                           && resultArray[0][0].ItemEquals(instance[rowIndex][columnIndex])
                           && resultArray[0][1].ItemEquals(instance[rowIndex][columnIndex + 1])
                           && resultArray[1].Length == columnCount
                           && resultArray[1][0].ItemEquals(instance[rowIndex + 1][columnIndex])
                           && resultArray[1][1].ItemEquals(instance[rowIndex + 1][columnIndex + 1])
                           && resultArray[2].Length == columnCount
                           && resultArray[2][0].ItemEquals(instance[rowIndex + 2][columnIndex])
                           && resultArray[2][1].ItemEquals(instance[rowIndex + 2][columnIndex + 1]);
                }
            );

            TestTemplate.PureMethod(
                instance,
                target => target.GetItemCore(rowIndex, rowCount, columnIndex, columnCount),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        [Test]
        public static void SetRowRangeCoreTest_SingleRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var setRows = InitInstance.GenerateRows(1, InitInstance.InitColumnLength)
                .ToArray();

            var removeRow = instance[index];

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0, 2, 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          // row 1 : 変更されている
                          && instance[1][0].ItemEquals(setRows[0][0])
                          && instance[1][1].ItemEquals(setRows[0][1])
                          && instance[1][2].ItemEquals(setRows[0][2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.SetRowRangeCore(index, setRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(
                instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Replace
            );
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 除去した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが無効になっていること
            instance.ClearNotifiedEventList();
            removeRow.AddCore(new StubModel());
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 追加した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが有効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength);
        }

        [Test]
        public static void SetRowRangeCoreTest_MultiRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var setRows = InitInstance.GenerateRows(2, InitInstance.InitColumnLength)
                .ToArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0, 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          // row 1, 2 : 変更されている
                          && instance[1][0].ItemEquals(setRows[0][0])
                          && instance[1][1].ItemEquals(setRows[0][1])
                          && instance[1][2].ItemEquals(setRows[0][2])
                          && instance[2][0].ItemEquals(setRows[1][0])
                          && instance[2][1].ItemEquals(setRows[1][1])
                          && instance[2][2].ItemEquals(setRows[1][2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.SetRowRangeCore(index, setRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void SetColumnRangeCoreTest_SingleColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var setItems = InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // column 0 , 2: 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          // column 1 : 変更されている
                          && instance[0][1].ItemEquals(setItems[0][0])
                          && instance[1][1].ItemEquals(setItems[0][1])
                          && instance[2][1].ItemEquals(setItems[0][2])
                          && instance[3][1].ItemEquals(setItems[0][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.SetColumnRangeCore(index, setItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Replace
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void SetColumnRangeCoreTest_MultiColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var setItems = InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitColumnLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // column 0 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          // column 1, 2 : 変更されている
                          && instance[0][1].ItemEquals(setItems[0][0])
                          && instance[1][1].ItemEquals(setItems[0][1])
                          && instance[2][1].ItemEquals(setItems[0][2])
                          && instance[3][1].ItemEquals(setItems[0][3])
                          && instance[0][2].ItemEquals(setItems[1][0])
                          && instance[1][2].ItemEquals(setItems[1][1])
                          && instance[2][2].ItemEquals(setItems[1][2])
                          && instance[3][2].ItemEquals(setItems[1][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.SetColumnRangeCore(index, setItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void SetItemCoreTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var row = 1;
            var column = 2;
            var setItem = new StubModel($"{10000 + column}");
            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // (1, 2) : 変更されている
                          && instance[1][2].ItemEquals(setItem)
                          // それ以外 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.SetItemCore(row, column, setItem),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 1);
            Assert.IsTrue(ReferenceEquals(instance.NotifiedRowPropertyChangedEvents[0].sender, instance[row]));
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents[0].args.PropertyName == ListConstant.IndexerName);
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 1);
            Assert.IsTrue(ReferenceEquals(instance.NotifiedRowCollectionChangedEvents[0].sender, instance[row]));
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents[0].args.Action == NotifyCollectionChangedAction.Replace
            );
        }

        [Test]
        public static void AddRowRangeCoreTest_SingleRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var addRows = InitInstance.GenerateRows(1, InitInstance.InitColumnLength)
                .ToArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 1
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          // row 4 : 追加されている
                          && instance[4][0].ItemEquals(addRows[0][0])
                          && instance[4][1].ItemEquals(addRows[0][1])
                          && instance[4][2].ItemEquals(addRows[0][2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AddRowRangeCore(addRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Add);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 追加した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが有効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength + 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength + 1
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength + 1);
        }

        [Test]
        public static void AddRowRangeCoreTest_MultiRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var addRows = InitInstance.GenerateRows(2, InitInstance.InitColumnLength)
                .ToArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 2
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          // row 4 ～ 5 : 追加されている
                          && instance[4][0].ItemEquals(addRows[0][0])
                          && instance[4][1].ItemEquals(addRows[0][1])
                          && instance[4][2].ItemEquals(addRows[0][2])
                          && instance[5][0].ItemEquals(addRows[1][0])
                          && instance[5][1].ItemEquals(addRows[1][1])
                          && instance[5][2].ItemEquals(addRows[1][2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AddRowRangeCore(addRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void AddColumnRangeCoreTest_SingleColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var addColumns = InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 1
                          // column 0 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 5 : 追加されている
                          && instance[0][5].ItemEquals(addColumns[0][0])
                          && instance[1][5].ItemEquals(addColumns[0][1])
                          && instance[2][5].ItemEquals(addColumns[0][2])
                          && instance[3][5].ItemEquals(addColumns[0][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AddColumnRangeCore(addColumns),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Add
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AddColumnRangeCoreTest_MultiColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var addColumns = InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 2
                          // column 0 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 5 ～ 6 : 追加されている
                          && instance[0][5].ItemEquals(addColumns[0][0])
                          && instance[1][5].ItemEquals(addColumns[0][1])
                          && instance[2][5].ItemEquals(addColumns[0][2])
                          && instance[3][5].ItemEquals(addColumns[0][3])
                          && instance[0][6].ItemEquals(addColumns[1][0])
                          && instance[1][6].ItemEquals(addColumns[1][1])
                          && instance[2][6].ItemEquals(addColumns[1][2])
                          && instance[3][6].ItemEquals(addColumns[1][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AddColumnRangeCore(addColumns),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void InsertRowRangeCoreTest_SingleRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var insertRows = InitInstance.GenerateRows(1, InitInstance.InitColumnLength)
                .ToArray();
            var insertIndex = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 1
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          // row 1 : 挿入されている
                          && instance[1][0].ItemEquals(insertRows[0][0])
                          && instance[1][1].ItemEquals(insertRows[0][1])
                          && instance[1][2].ItemEquals(insertRows[0][2])
                          && instance[1][3].ItemEquals(insertRows[0][3])
                          && instance[1][4].ItemEquals(insertRows[0][4])
                          // row 2 ～ 4 : 行挿入により後ろ方向にずれている
                          && instance[2][0].ItemEquals(originalItems[1][0])
                          && instance[2][1].ItemEquals(originalItems[1][1])
                          && instance[2][2].ItemEquals(originalItems[1][2])
                          && instance[2][3].ItemEquals(originalItems[1][3])
                          && instance[2][4].ItemEquals(originalItems[1][4])
                          && instance[3][0].ItemEquals(originalItems[2][0])
                          && instance[3][1].ItemEquals(originalItems[2][1])
                          && instance[3][2].ItemEquals(originalItems[2][2])
                          && instance[3][3].ItemEquals(originalItems[2][3])
                          && instance[3][4].ItemEquals(originalItems[2][4])
                          && instance[4][0].ItemEquals(originalItems[3][0])
                          && instance[4][1].ItemEquals(originalItems[3][1])
                          && instance[4][2].ItemEquals(originalItems[3][2])
                          && instance[4][3].ItemEquals(originalItems[3][3])
                          && instance[4][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.InsertRowRangeCore(insertIndex, insertRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Add);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 挿入した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが有効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength + 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength + 1
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength + 1);
        }

        [Test]
        public static void InsertRowRangeCoreTest_MultiRow()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var insertRows = InitInstance.GenerateRows(2, InitInstance.InitColumnLength)
                .ToArray();
            var insertIndex = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 2
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          // row 1 ～ 2 : 挿入されている
                          && instance[1][0].ItemEquals(insertRows[0][0])
                          && instance[1][1].ItemEquals(insertRows[0][1])
                          && instance[1][2].ItemEquals(insertRows[0][2])
                          && instance[1][3].ItemEquals(insertRows[0][3])
                          && instance[1][4].ItemEquals(insertRows[0][4])
                          && instance[2][0].ItemEquals(insertRows[1][0])
                          && instance[2][1].ItemEquals(insertRows[1][1])
                          && instance[2][2].ItemEquals(insertRows[1][2])
                          && instance[2][3].ItemEquals(insertRows[1][3])
                          && instance[2][4].ItemEquals(insertRows[1][4])
                          // row 3 ～ 5 : 行挿入により後ろ方向にずれている
                          && instance[3][0].ItemEquals(originalItems[1][0])
                          && instance[3][1].ItemEquals(originalItems[1][1])
                          && instance[3][2].ItemEquals(originalItems[1][2])
                          && instance[3][3].ItemEquals(originalItems[1][3])
                          && instance[3][4].ItemEquals(originalItems[1][4])
                          && instance[4][0].ItemEquals(originalItems[2][0])
                          && instance[4][1].ItemEquals(originalItems[2][1])
                          && instance[4][2].ItemEquals(originalItems[2][2])
                          && instance[4][3].ItemEquals(originalItems[2][3])
                          && instance[4][4].ItemEquals(originalItems[2][4])
                          && instance[5][0].ItemEquals(originalItems[3][0])
                          && instance[5][1].ItemEquals(originalItems[3][1])
                          && instance[5][2].ItemEquals(originalItems[3][2])
                          && instance[5][3].ItemEquals(originalItems[3][3])
                          && instance[5][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.InsertRowRangeCore(insertIndex, insertRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void InsertColumnRangeCoreTest_SingleColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var insertColumns = InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();
            var insertIndex = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 1
                          // column 0 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          // column 1 : 挿入されている
                          && instance[0][1].ItemEquals(insertColumns[0][0])
                          && instance[1][1].ItemEquals(insertColumns[0][1])
                          && instance[2][1].ItemEquals(insertColumns[0][2])
                          && instance[3][1].ItemEquals(insertColumns[0][3])
                          // column 2 ～ 5 : 列挿入により後ろ方向にずれている
                          && instance[0][2].ItemEquals(originalItems[0][1])
                          && instance[1][2].ItemEquals(originalItems[1][1])
                          && instance[2][2].ItemEquals(originalItems[2][1])
                          && instance[3][2].ItemEquals(originalItems[3][1])
                          && instance[0][3].ItemEquals(originalItems[0][2])
                          && instance[1][3].ItemEquals(originalItems[1][2])
                          && instance[2][3].ItemEquals(originalItems[2][2])
                          && instance[3][3].ItemEquals(originalItems[3][2])
                          && instance[0][4].ItemEquals(originalItems[0][3])
                          && instance[1][4].ItemEquals(originalItems[1][3])
                          && instance[2][4].ItemEquals(originalItems[2][3])
                          && instance[3][4].ItemEquals(originalItems[3][3])
                          && instance[0][5].ItemEquals(originalItems[0][4])
                          && instance[1][5].ItemEquals(originalItems[1][4])
                          && instance[2][5].ItemEquals(originalItems[2][4])
                          && instance[3][5].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.InsertColumnRangeCore(insertIndex, insertColumns),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Add
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void InsertColumnRangeCoreTest_MultiColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var insertColumns = InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();
            var insertIndex = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 2
                          // column 0 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          // column 1 ～ 2 : 挿入されている
                          && instance[0][1].ItemEquals(insertColumns[0][0])
                          && instance[1][1].ItemEquals(insertColumns[0][1])
                          && instance[2][1].ItemEquals(insertColumns[0][2])
                          && instance[3][1].ItemEquals(insertColumns[0][3])
                          && instance[0][2].ItemEquals(insertColumns[1][0])
                          && instance[1][2].ItemEquals(insertColumns[1][1])
                          && instance[2][2].ItemEquals(insertColumns[1][2])
                          && instance[3][2].ItemEquals(insertColumns[1][3])
                          // column 3 ～ 6 : 列挿入により後ろ方向にずれている
                          && instance[0][3].ItemEquals(originalItems[0][1])
                          && instance[1][3].ItemEquals(originalItems[1][1])
                          && instance[2][3].ItemEquals(originalItems[2][1])
                          && instance[3][3].ItemEquals(originalItems[3][1])
                          && instance[0][4].ItemEquals(originalItems[0][2])
                          && instance[1][4].ItemEquals(originalItems[1][2])
                          && instance[2][4].ItemEquals(originalItems[2][2])
                          && instance[3][4].ItemEquals(originalItems[3][2])
                          && instance[0][5].ItemEquals(originalItems[0][3])
                          && instance[1][5].ItemEquals(originalItems[1][3])
                          && instance[2][5].ItemEquals(originalItems[2][3])
                          && instance[3][5].ItemEquals(originalItems[3][3])
                          && instance[0][6].ItemEquals(originalItems[0][4])
                          && instance[1][6].ItemEquals(originalItems[1][4])
                          && instance[2][6].ItemEquals(originalItems[2][4])
                          && instance[3][6].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.InsertColumnRangeCore(insertIndex, insertColumns),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void OverwriteRowCoreTest_ItemIsEmpty()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == originalItems.Length
                          && LinqExtension.Zip(target, originalItems)
                              .All(
                                  zip =>
                                  {
                                      var (row, ordinalRow) = zip;
                                      return row.Count == ordinalRow.Length
                                             && LinqExtension.Zip(row, ordinalRow)
                                                 .All(zip2 => zip2.Item1.Equals(zip2.Item2));
                                  }
                              )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteRowCore(index, Array.Empty<InitInstance.ExtendedListForRow>()),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void OverwriteRowCoreTest_ItemIsSingle_OnlyReplace()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var overwriteRows = InitInstance.GenerateRows(1, InitInstance.InitColumnLength)
                .ToArray();

            var removeRow = instance[index];

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0, 2, 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // row 1 : 変更されている
                          && instance[1][0].ItemEquals(overwriteRows[0][0])
                          && instance[1][1].ItemEquals(overwriteRows[0][1])
                          && instance[1][2].ItemEquals(overwriteRows[0][2])
                          && instance[1][3].ItemEquals(originalItems[0][3])
                          && instance[1][4].ItemEquals(originalItems[0][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteRowCore(index, overwriteRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(
                instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Replace
            );
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 除去した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが無効になっていること
            instance.ClearNotifiedEventList();
            removeRow.AddCore(new StubModel());
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 追加した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが有効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength);
        }

        [Test]
        public static void OverwriteRowCoreTest_ItemIsSingle_OnlyAdd()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var overwriteRows = InitInstance.GenerateRows(1, InitInstance.InitColumnLength)
                .ToArray();
            var index = InitInstance.InitRowLength;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 1
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // row 4 : 追加されている
                          && instance[4][0].ItemEquals(overwriteRows[0][0])
                          && instance[4][1].ItemEquals(overwriteRows[0][1])
                          && instance[4][2].ItemEquals(overwriteRows[0][2])
                          && instance[4][3].ItemEquals(originalItems[0][3])
                          && instance[4][4].ItemEquals(originalItems[0][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteRowCore(index, overwriteRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Add);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 追加した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが有効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength + 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength + 1
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength + 1);
        }

        [Test]
        public static void OverwriteRowCoreTest_ItemIsMulti_OnlyReplace()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var overwriteRows = InitInstance.GenerateRows(2, InitInstance.InitColumnLength)
                .ToArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0, 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // row 1, 2 : 変更されている
                          && instance[1][0].ItemEquals(overwriteRows[0][0])
                          && instance[1][1].ItemEquals(overwriteRows[0][1])
                          && instance[1][2].ItemEquals(overwriteRows[0][2])
                          && instance[1][3].ItemEquals(overwriteRows[0][3])
                          && instance[1][4].ItemEquals(overwriteRows[0][4])
                          && instance[2][0].ItemEquals(overwriteRows[1][0])
                          && instance[2][1].ItemEquals(overwriteRows[1][1])
                          && instance[2][2].ItemEquals(overwriteRows[1][2])
                          && instance[2][3].ItemEquals(overwriteRows[1][3])
                          && instance[2][4].ItemEquals(overwriteRows[1][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteRowCore(index, overwriteRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void OverwriteRowCoreTest_ItemIsMulti_OnlyAdd()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var overwriteRows = InitInstance.GenerateRows(2, InitInstance.InitColumnLength)
                .ToArray();
            var index = InitInstance.InitRowLength;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 2
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // row 4 ～ 5 : 追加されている
                          && instance[4][0].ItemEquals(overwriteRows[0][0])
                          && instance[4][1].ItemEquals(overwriteRows[0][1])
                          && instance[4][2].ItemEquals(overwriteRows[0][2])
                          && instance[4][3].ItemEquals(overwriteRows[0][3])
                          && instance[4][4].ItemEquals(overwriteRows[0][4])
                          && instance[5][0].ItemEquals(overwriteRows[1][0])
                          && instance[5][1].ItemEquals(overwriteRows[1][1])
                          && instance[5][2].ItemEquals(overwriteRows[1][2])
                          && instance[5][3].ItemEquals(overwriteRows[1][3])
                          && instance[5][4].ItemEquals(overwriteRows[1][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteRowCore(index, overwriteRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void OverwriteRowCoreTest_ItemIsMulti_ReplaceAndAdd()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var overwriteRows = InitInstance.GenerateRows(2, InitInstance.InitColumnLength)
                .ToArray();
            var index = InitInstance.InitRowLength - 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 1
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 ～ 2 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          // row 3 : 変更されている
                          && instance[3][0].ItemEquals(overwriteRows[0][0])
                          && instance[3][1].ItemEquals(overwriteRows[0][1])
                          && instance[3][2].ItemEquals(overwriteRows[0][2])
                          && instance[3][3].ItemEquals(overwriteRows[0][3])
                          && instance[3][4].ItemEquals(overwriteRows[0][4])
                          // row 4 : 追加されている
                          && instance[4][0].ItemEquals(overwriteRows[1][0])
                          && instance[4][1].ItemEquals(overwriteRows[1][1])
                          && instance[4][2].ItemEquals(overwriteRows[1][2])
                          && instance[4][3].ItemEquals(overwriteRows[1][3])
                          && instance[4][4].ItemEquals(overwriteRows[1][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteRowCore(index, overwriteRows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void OverwriteColumnCoreTest_ItemIsEmpty()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => LinqExtension.Zip(target, originalItems)
                    .All(
                        zip => LinqExtension.Zip(zip.Item1, zip.Item2).All(zip2 => zip2.Item1.ItemEquals(zip2.Item2))
                    )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteColumnCore(index, Array.Empty<IEnumerable<StubModel>>()),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void OverwriteColumnCoreTest_ItemIsSingle_OnlyReplace()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var overwriteItems = InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // column 0 , 2 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 1 : 変更されている
                          && instance[0][1].ItemEquals(overwriteItems[0][0])
                          && instance[1][1].ItemEquals(overwriteItems[0][1])
                          && instance[2][1].ItemEquals(overwriteItems[0][2])
                          && instance[3][1].ItemEquals(overwriteItems[0][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteColumnCore(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Replace
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void OverwriteColumnCoreTest_ItemIsSingle_OnlyAdd()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = InitInstance.InitColumnLength;
            var overwriteItems = InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 1
                          // column 0 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 5 : 追加されている
                          && instance[0][5].ItemEquals(overwriteItems[0][0])
                          && instance[1][5].ItemEquals(overwriteItems[0][1])
                          && instance[2][5].ItemEquals(overwriteItems[0][2])
                          && instance[3][5].ItemEquals(overwriteItems[0][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteColumnCore(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Add
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void OverwriteColumnCoreTest_ItemIsMulti_OnlyReplace()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = 1;
            var overwriteItems = InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // column 0, 3 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 1, 2 : 変更されている
                          && instance[0][1].ItemEquals(overwriteItems[0][0])
                          && instance[1][1].ItemEquals(overwriteItems[0][1])
                          && instance[2][1].ItemEquals(overwriteItems[0][2])
                          && instance[3][1].ItemEquals(overwriteItems[0][3])
                          && instance[0][2].ItemEquals(overwriteItems[1][0])
                          && instance[1][2].ItemEquals(overwriteItems[1][1])
                          && instance[2][2].ItemEquals(overwriteItems[1][2])
                          && instance[3][2].ItemEquals(overwriteItems[1][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteColumnCore(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void OverwriteColumnCoreTest_ItemIsMulti_OnlyAdd()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = InitInstance.InitColumnLength;
            var overwriteItems = InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 2
                          // column 0 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 5 ～ 6 : 追加されている
                          && instance[0][5].ItemEquals(overwriteItems[0][0])
                          && instance[1][5].ItemEquals(overwriteItems[0][1])
                          && instance[2][5].ItemEquals(overwriteItems[0][2])
                          && instance[3][5].ItemEquals(overwriteItems[0][3])
                          && instance[0][6].ItemEquals(overwriteItems[1][0])
                          && instance[1][6].ItemEquals(overwriteItems[1][1])
                          && instance[2][6].ItemEquals(overwriteItems[1][2])
                          && instance[3][6].ItemEquals(overwriteItems[1][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteColumnCore(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void OverwriteColumnCoreTest_ItemIsMulti_ReplaceAndAdd()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var index = InitInstance.InitColumnLength - 1;
            var overwriteItems = InitInstance.GenerateTwoDimStubModels(2, InitInstance.InitRowLength)
                .ToTwoDimensionalArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 1
                          // column 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          // column 4 : 変更されている
                          && instance[0][4].ItemEquals(overwriteItems[0][0])
                          && instance[1][4].ItemEquals(overwriteItems[0][1])
                          && instance[2][4].ItemEquals(overwriteItems[0][2])
                          && instance[3][4].ItemEquals(overwriteItems[0][3])
                          // column 5 : 追加されている
                          && instance[0][5].ItemEquals(overwriteItems[1][0])
                          && instance[1][5].ItemEquals(overwriteItems[1][1])
                          && instance[2][5].ItemEquals(overwriteItems[1][2])
                          && instance[3][5].ItemEquals(overwriteItems[1][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.OverwriteColumnCore(index, overwriteItems),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void MoveRowRangeCoreTest_CountIsZero()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var oldRowIndex = 1;
            var newRowIndex = 2;
            var count = 0;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == originalItems.Length
                          && LinqExtension.Zip(target, originalItems)
                              .All(
                                  zip =>
                                  {
                                      var (row, ordinalRow) = zip;
                                      return row.Count == ordinalRow.Length
                                             && LinqExtension.Zip(row, ordinalRow)
                                                 .All(zip2 => zip2.Item1.Equals(zip2.Item2));
                                  }
                              )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.MoveRowRangeCore(oldRowIndex, newRowIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void MoveRowRangeCoreTest_CountIsOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var oldRowIndex = 1;
            var newRowIndex = 2;
            var count = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[2][0])
                          && instance[1][1].ItemEquals(originalItems[2][1])
                          && instance[1][2].ItemEquals(originalItems[2][2])
                          && instance[1][3].ItemEquals(originalItems[2][3])
                          && instance[1][4].ItemEquals(originalItems[2][4])
                          && instance[2][0].ItemEquals(originalItems[1][0])
                          && instance[2][1].ItemEquals(originalItems[1][1])
                          && instance[2][2].ItemEquals(originalItems[1][2])
                          && instance[2][3].ItemEquals(originalItems[1][3])
                          && instance[2][4].ItemEquals(originalItems[1][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.MoveRowRangeCore(oldRowIndex, newRowIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Move);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void MoveRowRangeCoreTest_CountIsTwo()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var oldRowIndex = 1;
            var newRowIndex = 2;
            var count = 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[3][0])
                          && instance[1][1].ItemEquals(originalItems[3][1])
                          && instance[1][2].ItemEquals(originalItems[3][2])
                          && instance[1][3].ItemEquals(originalItems[3][3])
                          && instance[1][4].ItemEquals(originalItems[3][4])
                          && instance[2][0].ItemEquals(originalItems[1][0])
                          && instance[2][1].ItemEquals(originalItems[1][1])
                          && instance[2][2].ItemEquals(originalItems[1][2])
                          && instance[2][3].ItemEquals(originalItems[1][3])
                          && instance[2][4].ItemEquals(originalItems[1][4])
                          && instance[3][0].ItemEquals(originalItems[2][0])
                          && instance[3][1].ItemEquals(originalItems[2][1])
                          && instance[3][2].ItemEquals(originalItems[2][2])
                          && instance[3][3].ItemEquals(originalItems[2][3])
                          && instance[3][4].ItemEquals(originalItems[2][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.MoveRowRangeCore(oldRowIndex, newRowIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void MoveColumnRangeCoreTest_CountIsZero()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var oldColumnIndex = 0;
            var newColumnIndex = 2;
            var count = 0;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == originalItems.Length
                          && LinqExtension.Zip(target, originalItems)
                              .All(
                                  zip =>
                                  {
                                      var (row, ordinalRow) = zip;
                                      return row.Count == ordinalRow.Length
                                             && LinqExtension.Zip(row, ordinalRow)
                                                 .All(zip2 => zip2.Item1.Equals(zip2.Item2));
                                  }
                              )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.MoveColumnRangeCore(oldColumnIndex, newColumnIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void MoveColumnRangeCoreTest_CountIsOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var oldColumnIndex = 0;
            var newColumnIndex = 2;
            var count = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][1])
                          && instance[1][0].ItemEquals(originalItems[1][1])
                          && instance[2][0].ItemEquals(originalItems[2][1])
                          && instance[3][0].ItemEquals(originalItems[3][1])
                          && instance[0][1].ItemEquals(originalItems[0][2])
                          && instance[1][1].ItemEquals(originalItems[1][2])
                          && instance[2][1].ItemEquals(originalItems[2][2])
                          && instance[3][1].ItemEquals(originalItems[3][2])
                          && instance[3][1].ItemEquals(originalItems[3][2])
                          && instance[0][2].ItemEquals(originalItems[0][0])
                          && instance[1][2].ItemEquals(originalItems[1][0])
                          && instance[2][2].ItemEquals(originalItems[2][0])
                          && instance[3][2].ItemEquals(originalItems[3][0])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.MoveColumnRangeCore(oldColumnIndex, newColumnIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Move
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void MoveColumnRangeCoreTest_CountIsTwo()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var oldColumnIndex = 0;
            var newColumnIndex = 2;
            var count = 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][2])
                          && instance[1][0].ItemEquals(originalItems[1][2])
                          && instance[2][0].ItemEquals(originalItems[2][2])
                          && instance[3][0].ItemEquals(originalItems[3][2])
                          && instance[0][1].ItemEquals(originalItems[0][3])
                          && instance[1][1].ItemEquals(originalItems[1][3])
                          && instance[2][1].ItemEquals(originalItems[2][3])
                          && instance[3][1].ItemEquals(originalItems[3][3])
                          && instance[0][2].ItemEquals(originalItems[0][0])
                          && instance[1][2].ItemEquals(originalItems[1][0])
                          && instance[2][2].ItemEquals(originalItems[2][0])
                          && instance[3][2].ItemEquals(originalItems[3][0])
                          && instance[0][3].ItemEquals(originalItems[0][1])
                          && instance[1][3].ItemEquals(originalItems[1][1])
                          && instance[2][3].ItemEquals(originalItems[2][1])
                          && instance[3][3].ItemEquals(originalItems[3][1])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.MoveColumnRangeCore(oldColumnIndex, newColumnIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void RemoveRowRangeCoreTest_CountIsZero()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowIndex = 1;
            var count = 0;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == originalItems.Length
                          && LinqExtension.Zip(target, originalItems)
                              .All(
                                  zip =>
                                  {
                                      var (row, ordinalRow) = zip;
                                      return row.Count == ordinalRow.Length
                                             && LinqExtension.Zip(row, ordinalRow)
                                                 .All(zip2 => zip2.Item1.Equals(zip2.Item2));
                                  }
                              )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.RemoveRowRangeCore(rowIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void RemoveRowRangeCoreTest_CountIsOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowIndex = 1;
            var count = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength - 1
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[2][0])
                          && instance[1][1].ItemEquals(originalItems[2][1])
                          && instance[1][2].ItemEquals(originalItems[2][2])
                          && instance[1][3].ItemEquals(originalItems[2][3])
                          && instance[1][4].ItemEquals(originalItems[2][4])
                          && instance[2][0].ItemEquals(originalItems[3][0])
                          && instance[2][1].ItemEquals(originalItems[3][1])
                          && instance[2][2].ItemEquals(originalItems[3][2])
                          && instance[2][3].ItemEquals(originalItems[3][3])
                          && instance[2][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.RemoveRowRangeCore(rowIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(
                instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Remove
            );
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 挿入した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが無効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength - 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength - 1
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength - 1);
        }

        [Test]
        public static void RemoveRowRangeCoreTest_CountIsTwo()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowIndex = 1;
            var count = 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength - 2
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[3][0])
                          && instance[1][1].ItemEquals(originalItems[3][1])
                          && instance[1][2].ItemEquals(originalItems[3][2])
                          && instance[1][3].ItemEquals(originalItems[3][3])
                          && instance[1][4].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.RemoveRowRangeCore(rowIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void RemoveColumnRangeCoreTest_CountIsZero()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var columnIndex = 1;
            var count = 0;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == originalItems.Length
                          && LinqExtension.Zip(target, originalItems)
                              .All(
                                  zip =>
                                  {
                                      var (row, ordinalRow) = zip;
                                      return row.Count == ordinalRow.Length
                                             && LinqExtension.Zip(row, ordinalRow)
                                                 .All(zip2 => zip2.Item1.Equals(zip2.Item2));
                                  }
                              )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.RemoveColumnRangeCore(columnIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void RemoveColumnRangeCoreTest_CountIsOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var columnIndex = 1;
            var count = 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength - 1
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][2])
                          && instance[1][1].ItemEquals(originalItems[1][2])
                          && instance[2][1].ItemEquals(originalItems[2][2])
                          && instance[3][1].ItemEquals(originalItems[3][2])
                          && instance[0][2].ItemEquals(originalItems[0][3])
                          && instance[1][2].ItemEquals(originalItems[1][3])
                          && instance[2][2].ItemEquals(originalItems[2][3])
                          && instance[3][2].ItemEquals(originalItems[3][3])
                          && instance[0][3].ItemEquals(originalItems[0][4])
                          && instance[1][3].ItemEquals(originalItems[1][4])
                          && instance[2][3].ItemEquals(originalItems[2][4])
                          && instance[3][3].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.RemoveColumnRangeCore(columnIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Remove
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void RemoveColumnRangeCoreTest_CountIsTwo()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var columnIndex = 1;
            var count = 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength - 2
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][3])
                          && instance[1][1].ItemEquals(originalItems[1][3])
                          && instance[2][1].ItemEquals(originalItems[2][3])
                          && instance[3][1].ItemEquals(originalItems[3][3])
                          && instance[0][2].ItemEquals(originalItems[0][4])
                          && instance[1][2].ItemEquals(originalItems[1][4])
                          && instance[2][2].ItemEquals(originalItems[2][4])
                          && instance[3][2].ItemEquals(originalItems[3][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.RemoveColumnRangeCore(columnIndex, count),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_NoAddOrRemoveRow_NoAddOrRemoveColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowLenght = InitInstance.InitRowLength;
            var columnLength = InitInstance.InitColumnLength;

            var expectedNotifyPropertyChange = Array.Empty<string>();

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == originalItems.Length
                          && LinqExtension.Zip(target, originalItems)
                              .All(
                                  zip =>
                                  {
                                      var (row, ordinalRow) = zip;
                                      return row.Count == ordinalRow.Length
                                             && LinqExtension.Zip(row, ordinalRow)
                                                 .All(zip2 => zip2.Item1.Equals(zip2.Item2));
                                  }
                              )
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void AdjustLengthCore_AddRowOne_NoAddOrRemoveColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength + 1;
            var columnLength = InitInstance.InitColumnLength;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == rowLenght
                          && target.ColumnCount == columnLength
                          // row 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // row 4 : 追加されている
                          && instance[4][0].ItemEquals(config.ItemFactory(4, 0))
                          && instance[4][1].ItemEquals(config.ItemFactory(4, 1))
                          && instance[4][2].ItemEquals(config.ItemFactory(4, 2))
                          && instance[4][3].ItemEquals(config.ItemFactory(4, 3))
                          && instance[4][4].ItemEquals(config.ItemFactory(4, 4))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Add);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 追加した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが有効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength);
        }

        [Test]
        public static void AdjustLengthCore_AddRowTwo_NoAddOrRemoveColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength + 2;
            var columnLength = InitInstance.InitColumnLength;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength + 2
                          && target.ColumnCount == InitInstance.InitColumnLength
                          // row 0 ～ 3 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // row 4 ～ 5 : 追加されている
                          && instance[4][0].ItemEquals(config.ItemFactory(4, 0))
                          && instance[4][1].ItemEquals(config.ItemFactory(4, 1))
                          && instance[4][2].ItemEquals(config.ItemFactory(4, 2))
                          && instance[4][3].ItemEquals(config.ItemFactory(4, 3))
                          && instance[4][4].ItemEquals(config.ItemFactory(4, 4))
                          && instance[5][0].ItemEquals(config.ItemFactory(5, 0))
                          && instance[5][1].ItemEquals(config.ItemFactory(5, 1))
                          && instance[5][2].ItemEquals(config.ItemFactory(5, 2))
                          && instance[5][3].ItemEquals(config.ItemFactory(5, 3))
                          && instance[5][4].ItemEquals(config.ItemFactory(5, 4))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void AdjustLengthCore_RemoveRowOne_NoAddOrRemoveColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowLenght = InitInstance.InitRowLength - 1;
            var columnLength = InitInstance.InitColumnLength;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength - 1
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(
                instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Remove
            );
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);

            // 挿入した行に対して、"AddRowPropertyChanged" "AddRowCollectionChanged" メソッドで追加したイベントが無効になっていること
            instance.ClearNotifiedEventList();
            instance.AddColumnRangeCore(InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitColumnLength));
            //      正しくイベントが設定されているならば、列追加したときに イベント発火回数 = 行数 となるはず
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength - 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength - 1
            );
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == InitInstance.InitRowLength - 1);
        }

        [Test]
        public static void AdjustLengthCore_RemoveRowTwo_NoAddOrRemoveColumn()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowLenght = InitInstance.InitRowLength - 2;
            var columnLength = InitInstance.InitColumnLength;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength - 2
                          && target.ColumnCount == InitInstance.InitColumnLength
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void AdjustLengthCore_NoAddOrRemoveRow_AddColumnOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength;
            var columnLength = InitInstance.InitColumnLength + 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 1
                          // column 0 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 5 : 追加されている
                          && instance[0][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[1][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[2][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[3][5].ItemEquals(config.ItemFactory(0, 5))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Add
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_NoAddOrRemoveRow_AddColumnTwo()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength;
            var columnLength = InitInstance.InitColumnLength + 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength + 2
                          // column 0 ～ 4 : 変更されていない
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          // column 5 ～ 6 : 追加されている
                          && instance[0][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[1][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[2][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[3][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[0][6].ItemEquals(config.ItemFactory(0, 6))
                          && instance[1][6].ItemEquals(config.ItemFactory(0, 6))
                          && instance[2][6].ItemEquals(config.ItemFactory(0, 6))
                          && instance[3][6].ItemEquals(config.ItemFactory(0, 6))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_NoAddOrRemoveRow_RemoveColumnOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowLenght = InitInstance.InitRowLength;
            var columnLength = InitInstance.InitColumnLength - 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength - 1
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][3].ItemEquals(originalItems[3][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Remove
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_NoAddOrRemoveRow_RemoveColumnTwo()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowLenght = InitInstance.InitRowLength;
            var columnLength = InitInstance.InitColumnLength - 2;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == InitInstance.InitRowLength
                          && target.ColumnCount == InitInstance.InitColumnLength - 2
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[3][2].ItemEquals(originalItems[3][2])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 0);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Reset
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_AddRowOne_AddColumnOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength + 1;
            var columnLength = InitInstance.InitColumnLength + 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == rowLenght
                          && target.ColumnCount == columnLength
                          // row 0 ～ 3 : column 5 が追加されている
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[0][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[1][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[2][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          && instance[3][4].ItemEquals(originalItems[3][4])
                          && instance[3][5].ItemEquals(config.ItemFactory(0, 5))
                          // row 4 : 追加されている
                          && instance[4][0].ItemEquals(config.ItemFactory(4, 0))
                          && instance[4][1].ItemEquals(config.ItemFactory(4, 1))
                          && instance[4][2].ItemEquals(config.ItemFactory(4, 2))
                          && instance[4][3].ItemEquals(config.ItemFactory(4, 3))
                          && instance[4][4].ItemEquals(config.ItemFactory(4, 4))
                          && instance[4][5].ItemEquals(config.ItemFactory(4, 5))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Add);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Add
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_AddRowOne_RemoveColumnOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength + 1;
            var columnLength = InitInstance.InitColumnLength - 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == rowLenght
                          && target.ColumnCount == columnLength
                          // row 0 ～ 3 : column 4 が除去されている
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[3][0].ItemEquals(originalItems[3][0])
                          && instance[3][1].ItemEquals(originalItems[3][1])
                          && instance[3][2].ItemEquals(originalItems[3][2])
                          && instance[3][3].ItemEquals(originalItems[3][3])
                          // row 4 : 追加されている
                          && instance[4][0].ItemEquals(config.ItemFactory(4, 0))
                          && instance[4][1].ItemEquals(config.ItemFactory(4, 1))
                          && instance[4][2].ItemEquals(config.ItemFactory(4, 2))
                          && instance[4][3].ItemEquals(config.ItemFactory(4, 3))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Add);
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Remove
                )
                == InitInstance.InitRowLength
            );
        }

        [Test]
        public static void AdjustLengthCore_RemoveRowOne_AddColumnOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var rowLenght = InitInstance.InitRowLength - 1;
            var columnLength = InitInstance.InitColumnLength + 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == rowLenght
                          && target.ColumnCount == columnLength
                          // row 0 ～ 2 : column 5 が追加されている
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[0][4].ItemEquals(originalItems[0][4])
                          && instance[0][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[1][4].ItemEquals(originalItems[1][4])
                          && instance[1][5].ItemEquals(config.ItemFactory(0, 5))
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
                          && instance[2][4].ItemEquals(originalItems[2][4])
                          && instance[2][5].ItemEquals(config.ItemFactory(0, 5))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(
                instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Remove
            );
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength - 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength - 1
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Add
                )
                == InitInstance.InitRowLength - 1
            );
        }

        [Test]
        public static void AdjustLengthCore_RemoveRowOne_RemoveColumnOne()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();
            var originalItems = instance.ToTwoDimensionalArray();

            var rowLenght = InitInstance.InitRowLength - 1;
            var columnLength = InitInstance.InitColumnLength - 1;

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == rowLenght
                          && target.ColumnCount == columnLength
                          // row 0 ～ 2 : column 4 が除去されている
                          && instance[0][0].ItemEquals(originalItems[0][0])
                          && instance[0][1].ItemEquals(originalItems[0][1])
                          && instance[0][2].ItemEquals(originalItems[0][2])
                          && instance[0][3].ItemEquals(originalItems[0][3])
                          && instance[1][0].ItemEquals(originalItems[1][0])
                          && instance[1][1].ItemEquals(originalItems[1][1])
                          && instance[1][2].ItemEquals(originalItems[1][2])
                          && instance[1][3].ItemEquals(originalItems[1][3])
                          && instance[2][0].ItemEquals(originalItems[2][0])
                          && instance[2][1].ItemEquals(originalItems[2][1])
                          && instance[2][2].ItemEquals(originalItems[2][2])
                          && instance[2][3].ItemEquals(originalItems[2][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.AdjustLengthCore(rowLenght, columnLength),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(
                instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Remove
            );
            // 各行要素プロパティ変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == nameof(IExtendedList<StubModel>.Count)
                )
                == InitInstance.InitRowLength - 1
            );
            Assert.IsTrue(
                instance.NotifiedRowPropertyChangedEvents.Count(
                    tuple => tuple.args.PropertyName == ListConstant.IndexerName
                )
                == InitInstance.InitRowLength - 1
            );
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(
                instance.NotifiedRowCollectionChangedEvents.Count(
                    tuple => tuple.args.Action == NotifyCollectionChangedAction.Remove
                )
                == InitInstance.InitRowLength - 1
            );
        }

        [Test]
        public static void ResetCoreTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var resetRowLength = InitInstance.InitRowLength + 2;
            var resetColumnLength = InitInstance.InitColumnLength - 1;
            var rows = InitInstance.GenerateRows(resetRowLength, resetColumnLength).ToArray();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == resetRowLength
                          && target.ColumnCount == resetColumnLength
                          && instance[0][0].ItemEquals(rows[0][0])
                          && instance[0][1].ItemEquals(rows[0][1])
                          && instance[0][2].ItemEquals(rows[0][2])
                          && instance[0][3].ItemEquals(rows[0][3])
                          && instance[1][0].ItemEquals(rows[1][0])
                          && instance[1][1].ItemEquals(rows[1][1])
                          && instance[1][2].ItemEquals(rows[1][2])
                          && instance[1][3].ItemEquals(rows[1][3])
                          && instance[2][0].ItemEquals(rows[2][0])
                          && instance[2][1].ItemEquals(rows[2][1])
                          && instance[2][2].ItemEquals(rows[2][2])
                          && instance[2][3].ItemEquals(rows[2][3])
                          && instance[3][0].ItemEquals(rows[3][0])
                          && instance[3][1].ItemEquals(rows[3][1])
                          && instance[3][2].ItemEquals(rows[3][2])
                          && instance[3][3].ItemEquals(rows[3][3])
                          && instance[4][0].ItemEquals(rows[4][0])
                          && instance[4][1].ItemEquals(rows[4][1])
                          && instance[4][2].ItemEquals(rows[4][2])
                          && instance[4][3].ItemEquals(rows[4][3])
                          && instance[5][0].ItemEquals(rows[5][0])
                          && instance[5][1].ItemEquals(rows[5][1])
                          && instance[5][2].ItemEquals(rows[5][2])
                          && instance[5][3].ItemEquals(rows[5][3])
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.ResetCore(rows),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        [Test]
        public static void ClearCoreTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var config = InitInstance.TwoDimensionalListForImplementationsTest.GenerateTestTwoDimensionalListConfig();

            var expectedNotifyPropertyChange = new[]
            {
                ListConstant.IndexerName,
                nameof(instance.AllCount),
                nameof(instance.RowCount),
                nameof(instance.ColumnCount)
            };

            var isExpectedState = new Func<TwoDimensionalList<InitInstance.ExtendedListForRow, StubModel>, bool>(
                target => target.RowCount == config.MinRowCapacity
                          && target.ColumnCount == config.MinColumnCapacity
                          && instance[0][0].ItemEquals(config.ItemFactory(0, 0))
                          && instance[0][1].ItemEquals(config.ItemFactory(0, 1))
                          && instance[1][0].ItemEquals(config.ItemFactory(1, 0))
                          && instance[1][1].ItemEquals(config.ItemFactory(1, 1))
                          && instance[2][0].ItemEquals(config.ItemFactory(2, 0))
                          && instance[2][1].ItemEquals(config.ItemFactory(2, 1))
            );

            TestTemplate.MutableMethod(
                instance,
                target => target.ClearCore(),
                expectedThrowExecute: false,
                expectedNotifyPropertyChange,
                isExpectedState,
                logger
            );

            // コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents.Count == 1);
            Assert.IsTrue(instance.NotifiedTablePropertyChangedEvents[0].Action == NotifyCollectionChangedAction.Reset);
            // 各行要素プロパティ変更通知が発火していないこと
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            Assert.IsTrue(instance.NotifiedRowPropertyChangedEvents.Count == 0);
            // 各行要素コレクション変更通知が発火していること
            Assert.IsTrue(instance.NotifiedRowCollectionChangedEvents.Count == 0);
        }

        private static readonly object[] ItemEqualsTest1CaseSource =
        {
            new object?[]
            {
                null, false,
            },
            new object[]
            {
                Array.Empty<StubModel[]>(), false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength - 1,
                    InitInstance.InitColumnLength - 1
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength - 1,
                    InitInstance.InitColumnLength
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength - 1,
                    InitInstance.InitColumnLength + 1
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength,
                    InitInstance.InitColumnLength - 1
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength,
                    InitInstance.InitColumnLength
                ),
                true
            },
            new object[]
            {
                InitInstance.InitRowLength
                    .Iterate(_ => InitInstance.InitColumnLength.Iterate(_ => new StubModel("init value")))
                    .ToTwoDimensionalArray(),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength,
                    InitInstance.InitColumnLength + 1
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength + 1,
                    InitInstance.InitColumnLength - 1
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength + 1,
                    InitInstance.InitColumnLength
                ),
                false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength + 1,
                    InitInstance.InitColumnLength + 1
                ),
                false
            },
        };

        [TestCaseSource(nameof(ItemEqualsTest1CaseSource))]
        public static void ItemEqualsTest1_TestModelArray(StubModel[][]? otherItems, bool expected)
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var instanceItems = otherItems?.Select(row => new InitInstance.ExtendedListForRow(row));
            var other = instanceItems is null
                ? null
                : new InitInstance.TwoDimensionalListForImplementationsTest(instanceItems);

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
                Array.Empty<StubModel[]>(), false
            },
            new object[]
            {
                InitInstance.TwoDimensionalListForImplementationsTest.GenerateInitStubModelTwoDimArrays(
                    InitInstance.InitRowLength,
                    InitInstance.InitColumnLength
                ),
                true
            },
            new object[]
            {
                InitInstance.InitRowLength
                    .Iterate(_ => InitInstance.InitColumnLength.Iterate(_ => new StubModel("init value")))
                    .ToTwoDimensionalArray(),
                false
            },
        };

        [TestCaseSource(nameof(ItemEqualsTest2CaseSource))]
        public static void ItemEqualsTest_object(object? other, bool expected)
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

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
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void ToTwoDimensionalArrayTest(bool isTranspose)
        {
            var instance = InitInstance.GenerateTwoDimensionalListForImplementationsTest();

            var isExpectedResult = new Func<StubModel[][], bool>(
                result => isTranspose switch
                {
                    true => result.Length == InitInstance.InitColumnLength
                            && result[0].Length == InitInstance.InitRowLength
                            && result[0][0].ItemEquals(instance[0][0])
                            && result[0][1].ItemEquals(instance[1][0])
                            && result[0][2].ItemEquals(instance[2][0])
                            && result[0][3].ItemEquals(instance[3][0])
                            && result[1][0].ItemEquals(instance[0][1])
                            && result[1][1].ItemEquals(instance[1][1])
                            && result[1][2].ItemEquals(instance[2][1])
                            && result[1][3].ItemEquals(instance[3][1])
                            && result[2][0].ItemEquals(instance[0][2])
                            && result[2][1].ItemEquals(instance[1][2])
                            && result[2][2].ItemEquals(instance[2][2])
                            && result[2][3].ItemEquals(instance[3][2]),
                    false => result.Length == InitInstance.InitRowLength
                             && result[0].Length == InitInstance.InitColumnLength
                             && result[0][0].ItemEquals(instance[0][0])
                             && result[0][1].ItemEquals(instance[0][1])
                             && result[0][2].ItemEquals(instance[0][2])
                             && result[1][0].ItemEquals(instance[1][0])
                             && result[1][1].ItemEquals(instance[1][1])
                             && result[1][2].ItemEquals(instance[1][2])
                             && result[2][0].ItemEquals(instance[2][0])
                             && result[2][1].ItemEquals(instance[2][1])
                             && result[2][2].ItemEquals(instance[2][2])
                             && result[3][0].ItemEquals(instance[3][0])
                             && result[3][1].ItemEquals(instance[3][1])
                             && result[3][2].ItemEquals(instance[3][2]),
                }
            );

            TestTemplate.PureMethod(
                instance,
                target => target.ToTwoDimensionalArray(isTranspose),
                expectedThrowExecute: false,
                isExpectedResult,
                logger
            );
        }

        #endregion

        #region Transport Methods or Properties

        [Test]
        public static void RowCountGetterTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            var _ = instance.RowCount;

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.Count)
            );
        }

        [Test]
        public static void ConstructorTest_TransportValidate()
        {
            var initValues = InitInstance.InitRowLength.Iterate(_ => new MockExtendedList<StubModel>());
            var instance = TestTemplate.Constructor(
                factory: () => new InitInstance.TwoDimensionalListForTransportTest(initValues),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.Constructor)
            );
        }

        [Test]
        public static void GetRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            var _ = instance.GetRow(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.GetRangeCore)
            );
        }

        [Test]
        public static void GetRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            var _ = instance.GetRowRange(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.GetRangeCore)
            );
        }

        [Test]
        public static void GetColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            var _ = instance.GetColumn(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.GetRangeCore)
            );
        }

        [Test]
        public static void GetColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            var _ = instance.GetColumnRange(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.GetRangeCore)
            );
        }

        [Test]
        public static void GetItemTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            var _ = instance.GetItem(0, 1, 0, 2);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetItem)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.GetRangeCore)
            );
        }

        [Test]
        public static void SetRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.SetRow(0, new MockExtendedList<StubModel>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.SetRangeCore)
            );
        }

        [Test]
        public static void SetRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.SetRowRange(
                0,
                InitInstance.GenerateTwoDimensionalListForTransportTest(1)
            );

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.SetRangeCore)
            );
        }

        [Test]
        public static void SetColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.SetColumn(0, InitInstance.GenerateStubModels(0, InitInstance.InitRowLength));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetColumn)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.SetRangeCore)
            );
        }

        [Test]
        public static void SetColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.SetColumnRange(0, InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetColumn)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.SetRangeCore)
            );
        }

        [Test]
        public static void SetItemTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.SetItem(0, 0, new StubModel());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetItem)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.GetRangeCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.SetRangeCore)
            );
        }

        [Test]
        public static void AddRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AddRow(new MockExtendedList<StubModel>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void AddRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AddRowRange(new List<MockExtendedList<StubModel>>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void AddColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AddColumn(InitInstance.InitRowLength.Iterate(_ => new StubModel()));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void AddColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AddColumnRange(new[] { InitInstance.InitRowLength.Iterate(_ => new StubModel()) });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.InsertRow(0, new MockExtendedList<StubModel>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.InsertRowRange(0, new List<MockExtendedList<StubModel>>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.InsertColumn(0, InitInstance.GenerateStubModels(0, InitInstance.InitRowLength));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.InsertColumnRange(0, InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.InsertRangeCore)
            );
        }

        [Test]
        public static void OverwriteRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.OverwriteRow(0, new List<MockExtendedList<StubModel>>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.OverwriteRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.OverwriteCore)
            );
        }

        [Test]
        public static void OverwriteColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.OverwriteColumn(0, InitInstance.GenerateTwoDimStubModels(1, InitInstance.InitRowLength));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.OverwriteColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.OverwriteCore)
            );
        }

        [Test]
        public static void MoveRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.MoveRow(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.MoveRangeCore)
            );
        }

        [Test]
        public static void MoveRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.MoveRowRange(0, 0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.MoveRangeCore)
            );
        }

        [Test]
        public static void MoveColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.MoveColumn(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.MoveRangeCore)
            );
        }

        [Test]
        public static void MoveColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.MoveColumnRange(0, 0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.MoveRangeCore)
            );
        }

        [Test]
        public static void RemoveRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.RemoveRow(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.RemoveRangeCore)
            );
        }

        [Test]
        public static void RemoveRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.RemoveRowRange(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveRow)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.RemoveRangeCore)
            );
        }

        [Test]
        public static void RemoveColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.RemoveColumn(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.RemoveRangeCore)
            );
        }

        [Test]
        public static void RemoveColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.RemoveColumnRange(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveColumn)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.RemoveRangeCore)
            );
        }

        [Test]
        public static void AdjustLengthTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustLength(1, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustLengthIfShortTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustLengthIfShort(1, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustLengthIfLongTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustLengthIfLong(1, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustRowLengthTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustRowLength(1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustRowLengthIfShortTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustRowLengthIfShort(1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustRowLengthIfLongTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustRowLengthIfLong(1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustColumnLengthTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustColumnLength(1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustColumnLengthIfShortTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustColumnLengthIfShort(1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustColumnLengthIfLongTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.AdjustColumnLengthIfLong(1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.AdjustLengthCore)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems[0],
                nameof(instance.MockItems.AdjustLengthCore)
            );
        }

        [Test]
        public static void ResetTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.Reset(Array.Empty<MockExtendedList<StubModel>>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.Reset)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.ResetCore)
            );
        }

        [Test]
        public static void ClearTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.Clear();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.Clear)
            );
            TestTemplateWithMock.AssertContainsCalledMemberHistory(
                instance.MockItems,
                nameof(instance.MockItems.ResetCore)
            );
        }

        [Test]
        public static void ValidateGetRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateGetRow(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetRow)
            );
        }

        [Test]
        public static void ValidateGetRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateGetRowRange(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetRow)
            );
        }

        [Test]
        public static void ValidateGetColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateGetColumn(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetColumn)
            );
        }

        [Test]
        public static void ValidateGetColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateGetColumnRange(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetColumn)
            );
        }

        [Test]
        public static void ValidateGetItemTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateGetItem(0, 1, 2, 3);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.GetItem)
            );
        }

        [Test]
        public static void ValidateSetRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateSetRow(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetRow)
            );
        }

        [Test]
        public static void ValidateSetRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateSetRowRange(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetRow)
            );
        }

        [Test]
        public static void ValidateSetColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateSetColumn(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetColumn)
            );
        }

        [Test]
        public static void ValidateSetColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateSetColumnRange(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetColumn)
            );
        }

        [Test]
        public static void ValidateSetItemTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateSetItem(0, 0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.SetItem)
            );
        }

        [Test]
        public static void ValidateAddRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAddRow(default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
        }

        [Test]
        public static void ValidateAddRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAddRowRange(default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
        }

        [Test]
        public static void ValidateAddColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAddColumn(default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
        }

        [Test]
        public static void ValidateAddColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAddColumnRange(default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
        }

        [Test]
        public static void ValidateInsertRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateInsertRow(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
        }

        [Test]
        public static void ValidateInsertRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateInsertRowRange(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertRow)
            );
        }

        [Test]
        public static void ValidateInsertColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateInsertColumn(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
        }

        [Test]
        public static void ValidateInsertColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateInsertColumnRange(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.InsertColumn)
            );
        }

        [Test]
        public static void ValidateOverwriteRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateOverwriteRow(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.OverwriteRow)
            );
        }

        [Test]
        public static void ValidateOverwriteColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateOverwriteColumn(0, default!);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.OverwriteColumn)
            );
        }

        [Test]
        public static void ValidateMoveRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateMoveRow(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveRow)
            );
        }

        [Test]
        public static void ValidateMoveRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateMoveRowRange(0, 0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveRow)
            );
        }

        [Test]
        public static void ValidateMoveColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateMoveColumn(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveColumn)
            );
        }

        [Test]
        public static void ValidateMoveColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateMoveColumnRange(0, 0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.MoveColumn)
            );
        }

        [Test]
        public static void ValidateRemoveRowTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateRemoveRow(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveRow)
            );
        }

        [Test]
        public static void ValidateRemoveRowRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateRemoveRowRange(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveRow)
            );
        }

        [Test]
        public static void ValidateRemoveColumnTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateRemoveColumn(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveColumn)
            );
        }

        [Test]
        public static void ValidateRemoveColumnRangeTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateRemoveColumnRange(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.RemoveColumn)
            );
        }

        [Test]
        public static void ValidateAdjustLengthTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustLength(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustLengthIfShortTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustLengthIfShort(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustLengthIfLongTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustLengthIfLong(0, 0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustRowLengthTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustRowLength(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustRowLengthIfShortTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustRowLengthIfShort(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustRowLengthIfLongTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustRowLengthIfLong(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustColumnLengthTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustColumnLength(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustColumnLengthIfShortTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustColumnLengthIfShort(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateAdjustColumnLengthIfLongTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateAdjustColumnLengthIfLong(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.AdjustLength)
            );
        }

        [Test]
        public static void ValidateResetTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateReset(Array.Empty<MockExtendedList<StubModel>>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.Reset)
            );
        }

        [Test]
        public static void ValidateClearTest()
        {
            var instance = InitInstance.GenerateTwoDimensionalListForTransportTest();
            instance.ValidateClear();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.MockTwoDimensionalListValidator,
                nameof(instance.MockTwoDimensionalListValidator.Clear)
            );
        }

        #endregion

        private static class InitInstance
        {
            public const int InitRowLength = 4;
            public const int InitColumnLength = 5;

            #region StubModel

            public static StubModel GenerateStubModel(int index) => new(index.ToString());

            public static IEnumerable<StubModel> GenerateStubModels(int index, int count)
                => count.Iterate(i => GenerateStubModel(index + i));

            public static IEnumerable<IEnumerable<StubModel>> GenerateTwoDimStubModels(
                int outerLength,
                int innerLength
            )
                => outerLength.Iterate(i => innerLength.Iterate(j => new StubModel($"{i * 10000 + j}")));

            #endregion

            #region ExtendedListForRow

            public static IEnumerable<ExtendedListForRow> GenerateRows(int count, int columnLength)
                => count.Iterate(_ => new ExtendedListForRow(GenerateStubModels(0, columnLength)));

            #endregion

            #region TwoDimensionalListForImplementationsTest

            public static TwoDimensionalListForImplementationsTest GenerateTwoDimensionalListForImplementationsTest(
                int rowLength = InitRowLength,
                int columnLength = InitColumnLength
            ) => new(
                rowLength,
                columnLength
            );

            public class ExtendedListForRow : ExtendedList<StubModel>
            {
                public ExtendedListForRow(IEnumerable<StubModel>? initItems = null) : base(
                    GenerateStubModel,
                    new MockWodiLibListValidator<StubModel>(),
                    initItems
                )
                {
                }
            }

            public class TwoDimensionalListForImplementationsTest :
                TwoDimensionalList<ExtendedListForRow, StubModel>
            {
                public static StubModel[][] GenerateInitStubModelTwoDimArrays(int rowLength, int columnLength)
                    => rowLength.Iterate(r => columnLength.Iterate(c => GenerateInitStubModel(r, c)))
                        .ToTwoDimensionalArray();

                public static StubModel GenerateInitStubModel(int rowIndex, int columnIndex)
                    => GenerateStubModel(rowIndex * 10000 + columnIndex);

                public static Config
                    GenerateTestTwoDimensionalListConfig()
                {
                    return new Config(
                        RowFactoryFromItems: items => new ExtendedListForRow(items),
                        ItemFactory: GenerateInitStubModel,
                        ItemComparer: (a, b) => a.ItemEquals(b),
                        ValidatorFactory: _
                            => new MockTwoDimensionalListValidator<ExtendedListForRow, StubModel>()
                    )
                    {
                        MaxRowCapacity = 20,
                        MinRowCapacity = 3,
                        MaxColumnCapacity = 15,
                        MinColumnCapacity = 2,
                    };
                }

                private readonly List<NotifyCollectionChangedEventArgs> notifiedTablePropertyChangedEvents = new();

                private readonly List<(object sender, PropertyChangedEventArgs args)> notifiedRowPropertyChangedEvents =
                    new();

                private readonly List<(object sender, NotifyCollectionChangedEventArgs args)>
                    notifiedRowCollectionChangedEvents = new();

                public IReadOnlyList<NotifyCollectionChangedEventArgs> NotifiedTablePropertyChangedEvents
                    => notifiedTablePropertyChangedEvents;

                public IReadOnlyList<(object sender, PropertyChangedEventArgs args)> NotifiedRowPropertyChangedEvents
                    => notifiedRowPropertyChangedEvents;

                public IReadOnlyList<(object sender, NotifyCollectionChangedEventArgs args)>
                    NotifiedRowCollectionChangedEvents
                    => notifiedRowCollectionChangedEvents;

                internal TwoDimensionalListForImplementationsTest(IEnumerable<IExtendedList<StubModel>> values) : base(
                    values,
                    GenerateTestTwoDimensionalListConfig()
                )
                {
                    SetupAfterConstruct();
                }

                internal TwoDimensionalListForImplementationsTest(IEnumerable<IEnumerable<StubModel>> items) : base(
                    items,
                    GenerateTestTwoDimensionalListConfig()
                )
                {
                    SetupAfterConstruct();
                }

                internal TwoDimensionalListForImplementationsTest(int rowLength, int columnLength) : base(
                    rowLength,
                    columnLength,
                    GenerateTestTwoDimensionalListConfig()
                )
                {
                    SetupAfterConstruct();
                }

                internal TwoDimensionalListForImplementationsTest() : base(GenerateTestTwoDimensionalListConfig())
                {
                    SetupAfterConstruct();
                }

                public void ClearNotifiedEventList()
                {
                    notifiedTablePropertyChangedEvents.Clear();
                    notifiedRowPropertyChangedEvents.Clear();
                    notifiedRowCollectionChangedEvents.Clear();
                }

                private void SetupAfterConstruct()
                {
                    SetupCollectionChangedEventHandler();
                    SetupRowPropertyOrCollectionChangedEventHandler();
                }

                private void SetupCollectionChangedEventHandler()
                {
                    CollectionChanged += (_, args) => notifiedTablePropertyChangedEvents.Add(args);
                }

                private void SetupRowPropertyOrCollectionChangedEventHandler()
                {
                    AddRowPropertyChanged((sender, args) => notifiedRowPropertyChangedEvents.Add((sender, args)));
                    AddRowCollectionChanged((sender, args) => notifiedRowCollectionChangedEvents.Add((sender, args)));
                }
            }

            #endregion

            #region TwoDimensionalListForTransportTest

            public static TwoDimensionalListForTransportTest GenerateTwoDimensionalListForTransportTest(
                int rowLength = InitRowLength,
                int columnLength = InitColumnLength
            )
            {
                var result = new TwoDimensionalListForTransportTest(rowLength, columnLength);
                result.MockTwoDimensionalListValidator.ClearCalledHistory();
                result.MockItems.ClearCalledHistory();
                result.MockItems.ForEach(
                    item => { item.ClearCalledHistory(); }
                );
                return result;
            }

            public class TwoDimensionalListForTransportTest : TwoDimensionalList<MockExtendedList<StubModel>, StubModel>
            {
                public MockTwoDimensionalListValidator<MockExtendedList<StubModel>, StubModel>
                    MockTwoDimensionalListValidator
                    => (MockTwoDimensionalListValidator<MockExtendedList<StubModel>, StubModel>)Validator;

                public MockExtendedList<MockExtendedList<StubModel>> MockItems
                    => (MockExtendedList<MockExtendedList<StubModel>>)Items;

                private protected override IExtendedList<MockExtendedList<StubModel>> Items { get; }
                    = new MockExtendedList<MockExtendedList<StubModel>>();

                private static Config
                    GenerateTestTwoDimensionalListConfig()
                {
                    return new Config(
                        RowFactoryFromItems: _ => new MockExtendedList<StubModel>(),
                        ItemFactory: (r, c) => GenerateStubModel(r * 10000 + c),
                        ItemComparer: (a, b) => a.ItemEquals(b),
                        ValidatorFactory: _
                            => new MockTwoDimensionalListValidator<MockExtendedList<StubModel>, StubModel>()
                    );
                }

                internal TwoDimensionalListForTransportTest(int rowLength, int columnLength) : this(
                    rowLength.Iterate(r => columnLength.Iterate(c => new StubModel((r * 10000 + c).ToString())))
                )
                {
                }

                internal TwoDimensionalListForTransportTest(IEnumerable<IEnumerable<StubModel>> values) : base(
                    values,
                    GenerateTestTwoDimensionalListConfig()
                )
                {
                    Items = new MockExtendedList<MockExtendedList<StubModel>>
                    {
                        Impl = new ExtendedList<MockExtendedList<StubModel>>(
                            makeListDefaultItem: _ => new MockExtendedList<StubModel>
                            {
                                Impl = new ExtendedList<StubModel>(
                                    makeListDefaultItem: i => new StubModel(i.ToString()),
                                    validator: new MockWodiLibListValidator<StubModel>()
                                )
                            },
                            validator: new MockWodiLibListValidator<MockExtendedList<StubModel>>(),
                            values.Select(
                                    rowValues => new MockExtendedList<StubModel>
                                    {
                                        Impl = new ExtendedList<StubModel>(
                                            makeListDefaultItem: i => new StubModel(i.ToString()),
                                            validator: new MockWodiLibListValidator<StubModel>(),
                                            rowValues
                                        )
                                    }
                                )
                                .ToArray()
                        )
                    };
                }
            }

            #endregion
        }
    }
}
