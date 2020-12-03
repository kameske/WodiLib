using System;
using System.Collections.Generic;
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
    public class RestrictedCapacityTwoDimensionalListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        [TestCase(TestClass.TestClassType.Type1, false, false)]
        [TestCase(TestClass.TestClassType.Type2, false, false)]
        [TestCase(TestClass.TestClassType.Type3, false, false)]
#if DEBUG
        [TestCase(TestClass.TestClassType.Type4, true, true)]
        [TestCase(TestClass.TestClassType.Type5, true, true)]
        [TestCase(TestClass.TestClassType.Type6, true, true)]
        [TestCase(TestClass.TestClassType.Type7, true, true)]
        [TestCase(TestClass.TestClassType.Type8, true, true)]
#elif RELEASE
        [TestCase(TestClass.TestClassType.Type4, false, true)]
        [TestCase(TestClass.TestClassType.Type5, false, true)]
        [TestCase(TestClass.TestClassType.Type6, false, true)]
        [TestCase(TestClass.TestClassType.Type7, false, true)]
        [TestCase(TestClass.TestClassType.Type8, false, false)]
#endif
        public static void ConstructorTest1(TestClass.TestClassType testType, bool isError, bool isErrorState)
        {
            var errorOccured = false;

            TestClass.IListTest instance = null;

            try
            {
                switch (testType)
                {
                    case TestClass.TestClassType.Type1:
                        instance = new TestClass.ListTest1();
                        break;

                    case TestClass.TestClassType.Type2:
                        instance = new TestClass.ListTest2();
                        break;

                    case TestClass.TestClassType.Type3:
                        instance = new TestClass.ListTest3();
                        break;

                    case TestClass.TestClassType.Type4:
                        instance = new TestClass.ListTest4();
                        break;

                    case TestClass.TestClassType.Type5:
                        instance = new TestClass.ListTest5();
                        break;

                    case TestClass.TestClassType.Type6:
                        instance = new TestClass.ListTest6();
                        break;

                    case TestClass.TestClassType.Type7:
                        instance = new TestClass.ListTest7();
                        break;

                    case TestClass.TestClassType.Type8:
                        instance = new TestClass.ListTest8();
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

            // 初期行数・列数が最小行数・列数と一致すること（Releaseビルドかつ異常設定の場合この条件を満たさない可能性あり）
            if (!isErrorState)
            {
                Assert.AreEqual(instance.RowCount, instance.GetMinRowCapacity());
                Assert.AreEqual(instance.ColumnCount, instance.GetMinColumnCapacity());

                // すべての要素が意図した値で初期化されていること
                instance.ForEach((row, rowIdx) =>
                {
                    row.ForEach((item, colIdx) =>
                        Assert.IsTrue(item.Equals(TestClass.MakeDefaultValueItem(rowIdx, colIdx))));
                });
            }
        }

        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 0, 0, false)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 5, 10, false)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 5, 11, true)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 6, 10, true)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 6, 11, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 4, 9, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 4, 10, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 4, 20, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 4, 21, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 5, 9, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 5, 10, false)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 5, 20, false)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 5, 21, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 10, 9, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 10, 10, false)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 10, 20, false)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 10, 21, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 11, 9, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 11, 10, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 11, 20, true)]
        [TestCase(TestClass.TestClassType.Type2, nameof(TestClass.ListType.Normal), 11, 21, true)]
        // 行数 == 0 かつ 列数 != 0 の二重リストは作れない
        //        [TestCase(TestClass.TestClassType.Type1, 0, 10, false)]
        // 行数 != 0 かつ 列数 == 0 の二重リストは作れる。
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 5, 0, false)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.Normal), 6, 0, true)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.SelfNull), 5, 10, true)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.RowHasNull), 5, 10, true)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.ColumnHasNull), 5, 10, true)]
        [TestCase(TestClass.TestClassType.Type1, nameof(TestClass.ListType.ColumnSizeDifference), 5, 10, true)]
        public static void ConstructorTest2(TestClass.TestClassType testType,
            string initItemTypeId, int initRowLength, int initColumnLength,
            bool isError)
        {
            var errorOccured = false;
            var initList = TestClass.ListType.FromId(initItemTypeId)
                .GetMultiLine(initRowLength, initColumnLength);
            TestClass.IListTest instance = null;

            try
            {
                switch (testType)
                {
                    case TestClass.TestClassType.Type1:
                        instance = new TestClass.ListTest1(initList);
                        break;
                    case TestClass.TestClassType.Type2:
                        instance = new TestClass.ListTest2(initList);
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

            // 行数・列数がセットした行数・列数と一致すること
            Assert.AreEqual(instance.RowCount, initRowLength);
            Assert.AreEqual(instance.ColumnCount, initColumnLength);

            // すべての要素がコンストラクタで与えた値で初期化されていること
            instance.ForEach((row, rowIdx) =>
            {
                row.ForEach((item, colIdx) =>
                    Assert.IsTrue(item.Equals(initList[rowIdx][colIdx])));
            });
        }

        /* RowCount, ColumnCount プロパティのテストは
         * ConstructorTest1, ConstructorTest2 参照
         */

        [TestCase(0, 0, true)]
        [TestCase(3, 0, false)]
        // 行数のみが0の状態は存在しない。
        //        [TestCase(0, 5, true)]
        [TestCase(3, 5, false)]
        public static void IsEmptyTest(int initRowLength, int initColumnLength, bool answer)
        {
            var initList = TestClass.MakeStringList(initRowLength, initColumnLength);
            var instance = new TestClass.ListTest1(initList);

            // RowCount, ColumnCount が一致すること
            Assert.AreEqual(instance.RowCount, initRowLength);
            Assert.AreEqual(instance.ColumnCount, initColumnLength);

            // IsEmpty プロパティが意図した値であること
            Assert.AreEqual(instance.IsEmpty, answer);
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 0, true)]
        [TestCase(3, 0, 2, 0, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 4, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 4, false)]
        [TestCase(3, 5, 0, 5, true)]
        [TestCase(3, 5, 2, -1, true)]
        [TestCase(3, 5, 2, 0, false)]
        [TestCase(3, 5, 2, 4, false)]
        [TestCase(3, 5, 2, 5, true)]
        [TestCase(3, 5, 3, -1, true)]
        [TestCase(3, 5, 3, 0, true)]
        [TestCase(3, 5, 3, 4, true)]
        [TestCase(3, 5, 3, 5, true)]
        public static void IndexerGetTest(int initRowLength, int initColumnLength,
            int row, int column, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;
            string result = null;

            try
            {
                result = instance[row, column];
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

            if (errorOccured) return;

            // 意図した値が取得されること
            Assert.IsTrue(result.Equals(TestClass.MakeDefaultValueItemForList(row, column)));
        }

        [TestCase(0, 0, 0, 0, "abc", true)]
        [TestCase(3, 0, 0, 0, "abc", true)]
        [TestCase(3, 0, 2, 0, "abc", true)]
        [TestCase(3, 5, -1, -1, null, true)]
        [TestCase(3, 5, -1, -1, "abc", true)]
        [TestCase(3, 5, -1, 0, null, true)]
        [TestCase(3, 5, -1, 0, "abc", true)]
        [TestCase(3, 5, -1, 4, null, true)]
        [TestCase(3, 5, -1, 4, "abc", true)]
        [TestCase(3, 5, -1, 5, null, true)]
        [TestCase(3, 5, -1, 5, "abc", true)]
        [TestCase(3, 5, 0, -1, null, true)]
        [TestCase(3, 5, 0, -1, "abc", true)]
        [TestCase(3, 5, 0, 0, null, true)]
        [TestCase(3, 5, 0, 0, "abc", false)]
        [TestCase(3, 5, 0, 4, null, true)]
        [TestCase(3, 5, 0, 4, "abc", false)]
        [TestCase(3, 5, 0, 5, null, true)]
        [TestCase(3, 5, 0, 5, "abc", true)]
        [TestCase(3, 5, 2, -1, null, true)]
        [TestCase(3, 5, 2, -1, "abc", true)]
        [TestCase(3, 5, 2, 0, null, true)]
        [TestCase(3, 5, 2, 0, "abc", false)]
        [TestCase(3, 5, 2, 4, null, true)]
        [TestCase(3, 5, 2, 4, "abc", false)]
        [TestCase(3, 5, 2, 5, null, true)]
        [TestCase(3, 5, 2, 5, "abc", true)]
        [TestCase(3, 5, 3, -1, null, true)]
        [TestCase(3, 5, 3, -1, "abc", true)]
        [TestCase(3, 5, 3, 0, null, true)]
        [TestCase(3, 5, 3, 0, "abc", true)]
        [TestCase(3, 5, 3, 4, null, true)]
        [TestCase(3, 5, 3, 4, "abc", true)]
        [TestCase(3, 5, 3, 5, null, true)]
        [TestCase(3, 5, 3, 5, "abc", true)]
        public static void IndexerSetTest(int initRowLength, int initColumnLength,
            int row, int column, string setItem, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance[row, column] = setItem;
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count,
                            isError ? 0 : 1);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (errorOccured) return;

            // 発生した Change イベントのパラメータが正しいこと
            var assertCollectionChangeEventArgsProperty = new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                args =>
                {
                    Assert.IsTrue(args.Direction == Direction.None);
                    Assert.IsTrue(args.OldStartRow == row);
                    Assert.IsTrue(args.OldStartColumn == column);
                    {
                        var oldItems = args.OldItems!
                            .ToTwoDimensionalArray();
                        Assert.IsTrue(oldItems.Length == 1);
                        Assert.IsTrue(oldItems[0].Length == 1);
                        Assert.IsTrue(oldItems[0][0].Equals(TestClass.MakeDefaultValueItemForList(row, column)));
                    }
                    Assert.IsTrue(args.NewStartRow == row);
                    Assert.IsTrue(args.NewStartColumn == column);
                    {
                        var newItems = args.NewItems!
                            .ToTwoDimensionalArray();
                        Assert.IsTrue(newItems.Length == 1);
                        Assert.IsTrue(newItems[0].Length == 1);
                        Assert.IsTrue(newItems[0][0].Equals(setItem));
                    }
                });
            assertCollectionChangeEventArgsProperty(
                changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Replace)][0]);
            assertCollectionChangeEventArgsProperty(
                changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Replace)][0]);

            // 意図した要素が編集されること
            instance.ForEach((rowItems, rowIdx) =>
                rowItems.ForEach((item, colIdx) =>
                {
                    if (rowIdx == row && colIdx == column)
                    {
                        Assert.IsTrue(item.Equals(setItem));
                    }
                    else
                    {
                        Assert.IsTrue(item.Equals(TestClass.MakeDefaultValueItemForList(rowIdx, colIdx)));
                    }
                }));
        }

        [TestCase(0, 0, 0, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 2, false)]
        [TestCase(3, 0, 3, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 2, false)]
        [TestCase(3, 5, 3, true)]
        public static void GetRowTest(int initRowLength, int initColumnLength,
            int row, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;
            IEnumerable<string> result = null;

            try
            {
                result = instance.GetRow(row);
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

            if (errorOccured) return;

            // 意図した行数・列数が取得されること
            var resultArray = result.ToArray();
            Assert.AreEqual(resultArray.Length, initColumnLength);

            // 意図した値が取得されること
            resultArray.ForEach((items, colIdx) =>
                Assert.IsTrue(
                    items.Equals(TestClass.MakeDefaultValueItemForList(row, colIdx))));
        }

        [TestCase(0, 0, 0, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, true)]
        [TestCase(3, 0, 1, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 4, false)]
        [TestCase(3, 5, 5, true)]
        public static void GetColumnTest(int initRowLength, int initColumnLength,
            int column, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;
            IEnumerable<string> result = null;

            try
            {
                result = instance.GetColumn(column);
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

            if (errorOccured) return;

            // 意図した行数・列数が取得されること
            var resultArray = result.ToArray();
            Assert.AreEqual(resultArray.Length, initRowLength);

            // 意図した値が取得されること
            resultArray.ForEach((items, rowIdx) =>
                Assert.IsTrue(
                    items.Equals(TestClass.MakeDefaultValueItemForList(rowIdx, column))));
        }

        [TestCase(0, 0, 0, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 3, 0, 0, true)]
        [TestCase(3, 0, 2, 1, 0, 0, true)]
        [TestCase(3, 0, 0, 0, 0, 1, true)]
        [TestCase(3, 5, -1, -1, -1, -1, true)]
        [TestCase(3, 5, -1, 0, 5, 1, true)]
        [TestCase(3, 5, -1, 3, 4, 0, true)]
        [TestCase(3, 5, -1, 4, 0, 5, true)]
        [TestCase(3, 5, 0, -1, 0, 6, true)]
        [TestCase(3, 5, 0, 0, 4, -1, true)]
        [TestCase(3, 5, 0, 4, -1, 0, true)]
        [TestCase(3, 5, 0, 4, 5, -1, true)]
        [TestCase(3, 5, 2, -1, -1, -1, true)]
        [TestCase(3, 5, 3, 0, 4, -1, true)]
        [TestCase(3, 5, 2, 2, 4, 1, true)]
        [TestCase(3, 5, 3, -1, 4, 0, true)]
        [TestCase(3, 5, 3, -1, 5, 0, true)]
        [TestCase(3, 5, 3, 0, -1, 5, true)]
        [TestCase(3, 5, 3, 0, -1, 5, true)]
        [TestCase(3, 5, 3, 0, 0, -1, true)]
        [TestCase(3, 5, 0, 0, 0, 0, false)]
        [TestCase(3, 5, 0, 3, 0, 5, false)]
        [TestCase(3, 5, 0, 3, 4, 0, false)]
        [TestCase(3, 5, 2, 0, 0, 5, false)]
        [TestCase(3, 5, 2, 0, 4, 0, false)]
        public static void GetRangeTest(int initRowLength, int initColumnLength,
            int row, int rowCount, int column, int columnCount, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;
            IEnumerable<IEnumerable<string>> result = null;

            try
            {
                result = instance.GetRange(row, rowCount, column, columnCount);
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

            if (errorOccured) return;

            // 意図した行数・列数が取得されること
            var resultArray = result.ToTwoDimensionalArray();
            Assert.AreEqual(resultArray.Length, rowCount);
            if (resultArray.Length > 0)
            {
                Assert.AreEqual(resultArray[0].Length, columnCount);
            }

            // 意図した値が取得されること
            resultArray.ForEach((rowItems, rowIdx) =>
                rowItems.ForEach((resultItem, colIdx) =>
                    Assert.IsTrue(
                        resultItem.Equals(TestClass.MakeDefaultValueItemForList(row + rowIdx, column + colIdx)))));
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 0, false)]
        [TestCase(3, 0, 2, 1, false)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 3, true)]
        [TestCase(3, 5, -1, 4, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 3, false)]
        [TestCase(3, 5, 0, 4, true)]
        [TestCase(3, 5, 2, -1, true)]
        [TestCase(3, 5, 2, 0, false)]
        [TestCase(3, 5, 2, 1, false)]
        [TestCase(3, 5, 2, 2, true)]
        [TestCase(3, 5, 3, -1, true)]
        [TestCase(3, 5, 3, 0, true)]
        [TestCase(3, 5, 3, 1, true)]
        public static void GetRowRangeTest(int initRowLength, int initColumnLength,
            int row, int count, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;
            IEnumerable<IEnumerable<string>> result = null;

            try
            {
                result = instance.GetRowRange(row, count);
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

            if (errorOccured) return;

            // 意図した行数・列数が取得されること
            var resultArray = result.ToTwoDimensionalArray();
            Assert.AreEqual(resultArray.Length, count);
            if (resultArray.Length > 0)
            {
                Assert.AreEqual(resultArray[0].Length, instance.ColumnCount);
            }

            // 意図した値が取得されること
            resultArray.ForEach((rowItems, rowIdx) =>
                rowItems.ForEach((resultItem, colIdx) =>
                    Assert.IsTrue(
                        resultItem.Equals(TestClass.MakeDefaultValueItemForList(row + rowIdx, colIdx)))));
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 1, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, -1, 6, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 5, false)]
        [TestCase(3, 5, 0, 6, true)]
        [TestCase(3, 5, 4, -1, true)]
        [TestCase(3, 5, 4, 0, false)]
        [TestCase(3, 5, 4, 1, false)]
        [TestCase(3, 5, 4, 2, true)]
        [TestCase(3, 5, 5, -1, true)]
        [TestCase(3, 5, 5, 0, true)]
        [TestCase(3, 5, 5, 1, true)]
        public static void GetColumnRangeTest(int initRowLength, int initColumnLength,
            int column, int count, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowLength, initColumnLength,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;
            IEnumerable<IEnumerable<string>> result = null;

            try
            {
                result = instance.GetColumnRange(column, count);
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
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], 0);

            if (errorOccured) return;

            // 意図した行数・列数が取得されること
            var resultArray = result.ToTwoDimensionalArray();
            Assert.AreEqual(resultArray.Length, count);
            if (resultArray.Length > 0)
            {
                Assert.AreEqual(resultArray[0].Length, instance.RowCount);
            }

            // 意図した値が取得されること
            resultArray.ForEach((colItems, colIdx) =>
                colItems.ForEach((resultItem, rowIdx) =>
                    Assert.IsTrue(
                        resultItem.Equals(TestClass.MakeDefaultValueItemForList(rowIdx, column + colIdx)))));
        }

        [TestCase(0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 1, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 10, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(0, 0, 10, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(5, 5, 5, nameof(TestClass.ListType.Normal), true)]
        public static void AddRowTest(int initRowCount, int initColumnCount,
            int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddRow(addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Row, initRowCount, initColumnCount,
                new[] {addItem}, initRowCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(0, 0, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 0, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 0, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 10, 3, nameof(TestClass.ListType.Normal), true)]
        public static void AddColumnTest(int initRowCount, int initColumnCount,
            int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddColumn(addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Column, initRowCount, initColumnCount,
                new[] {addItem}, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 5, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 5, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 5, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 6, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 6, 10, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 6, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 5, 10, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(0, 0, 5, 10, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(0, 0, 5, 10, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(0, 0, 5, 10, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 2, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 2, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(5, 5, 2, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 2, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 2, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 2, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 2, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 2, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 2, 5, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 5, 2, 5, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(5, 5, 1, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(5, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(5, 0, 1, 0, nameof(TestClass.ListType.Normal), true)]
        public static void AddRowRangeTest(int initRowCount, int initColumnCount,
            int addRowLength, int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetMultiLine(addRowLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddRowRange(addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Row, initRowCount, initColumnCount,
                addItem, initRowCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 1, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 1, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 1, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 1, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 1, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 10, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 10, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 10, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 10, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 11, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 11, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 11, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 11, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 10, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 0, 10, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 0, 10, 3, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 0, 10, 3, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 5, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 5, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 6, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 5, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 5, 3, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 5, 5, 3, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 10, 0, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 10, 1, 3, nameof(TestClass.ListType.Normal), true)]
        public static void AddColumnRangeTest(int initRowCount, int initColumnCount,
            int addColumnLength, int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetMultiLine(addColumnLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.AddColumnRange(addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Column, initRowCount, initColumnCount,
                addItem, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 10, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(0, 0, 0, 10, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, -1, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 4, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, -1, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 4, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 0, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(5, 5, 0, 5, nameof(TestClass.ListType.Normal), true)]
        public static void InsertRowTest(int initRowCount, int initColumnCount,
            int insertRow, int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertRow(insertRow, addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Row, initRowCount, initColumnCount,
                new[] {addItem}, insertRow, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 1, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, -1, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 6, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 0, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 10, 0, 3, nameof(TestClass.ListType.Normal), true)]
        public static void InsertColumnTest(int initRowCount, int initColumnCount,
            int insertColumn, int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetLine(addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertColumn(insertColumn, addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Column, initRowCount, initColumnCount,
                new[] {addItem}, insertColumn, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, -1, 5, 10, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 5, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 5, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 5, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 1, 5, 10, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 10, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 2, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(0, 0, 0, 2, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(0, 0, 0, 2, 5, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(0, 0, 0, 2, 5, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 2, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 2, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 3, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 3, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 2, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 2, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 2, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 0, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 3, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, -1, 2, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 2, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 4, 2, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 2, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 0, 2, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 0, 2, 5, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 5, 0, 2, 5, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(5, 5, 0, 0, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(5, 5, 0, 1, 5, nameof(TestClass.ListType.Normal), true)]
        public static void InsertRowRangeTest(int initRowCount, int initColumnCount,
            int insertRow, int addRowLength, int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetMultiLine(addRowLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertRowRange(insertRow, addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Row, initRowCount, initColumnCount,
                addItem, insertRow, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 1, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 1, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, -1, 10, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 10, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 1, 10, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 5, 0, 5, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 6, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 0, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, -1, 5, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 6, 5, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 10, 0, 0, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 10, 0, 1, 3, nameof(TestClass.ListType.Normal), true)]
        public static void InsertColumnRangeTest(int initRowCount, int initColumnCount,
            int insertColumn, int addColumnLength, int addLineItemLength,
            string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var addItem = TestClass.ListType.FromId(addTypeId)
                .GetMultiLine(addColumnLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.InsertColumnRange(insertColumn, addItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdd(instance, Direction.Column, initRowCount, initColumnCount,
                addItem, insertColumn, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        private static void CommonAssertionAdd(TestClass.ListTest1 instance, Direction direction, int initRowCount,
            int initColCount, string[][] addItems, int insertIndex,
            bool isError,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changingEventArgsList,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changedEventArgsList,
            Dictionary<string, int> propertyChangedEventCalledCount)
        {
            // 意図したイベントが発生すること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count,
                            isError ? 0 : 1);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)],
                isError
                    ? 0
                    : direction == Direction.Row
                        ? 1
                        : initRowCount == 0
                            ? 1
                            : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)],
                isError
                    ? 0
                    : direction == Direction.Column
                        ? 1
                        : initRowCount == 0 && addItems.Length != 0
                            ? 1
                            : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (isError) return;

            // 発生した Change イベントのパラメータが正しいこと
            var assertCollectionChangeEventArgsProperty =
                new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                    args =>
                    {
                        Assert.IsTrue(args.Direction == direction);
                        Assert.IsTrue(args.OldStartRow == -1);
                        Assert.IsTrue(args.OldStartColumn == -1);
                        Assert.IsTrue(args.OldItems == null);
                        Assert.IsTrue(args.NewStartRow == (direction == Direction.Row ? insertIndex : 0));
                        Assert.IsTrue(args.NewStartColumn == (direction == Direction.Column ? insertIndex : 0));
                        {
                            var newItems = args.NewItems!
                                .ToTwoDimensionalArray();
                            CommonAssertion.ArraySizeEqual(newItems, addItems);

                            Assert.IsTrue(newItems.Equals<string>(addItems));
                        }
                    });
            assertCollectionChangeEventArgsProperty(
                changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)][0]);
            assertCollectionChangeEventArgsProperty(
                changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)][0]);

            // 行数・列数が意図した値であること
            CommonAssertion.SizeEqual(instance,
                direction == Direction.Row
                    ? initRowCount + addItems.Length
                    : initRowCount != 0
                        ? initRowCount
                        : addItems.GetInnerArrayLength(),
                direction == Direction.Column
                    ? initColCount + addItems.Length
                    : initColCount != 0
                        ? initColCount
                        : addItems.GetInnerArrayLength());

            // 各要素が意図した値であること
            var isInsertElement = new Func<int, int, bool>((row, col) => direction == Direction.Row
                ? insertIndex <= row && row < insertIndex + addItems.Length
                : insertIndex <= col && col < insertIndex + addItems.Length);
            var getForAddItems = new Func<int, int, string>((row, col) => direction == Direction.Row
                ? addItems[row - insertIndex][col]
                : addItems[col - insertIndex][row]);
            var getInitItem = new Func<int, int, string>((row, col) => direction == Direction.Row
                ? TestClass.MakeDefaultValueItemForList(
                    row - (row < insertIndex ? 0 : addItems.Length), col)
                : TestClass.MakeDefaultValueItemForList(
                    row, col - (col < insertIndex ? 0 : addItems.Length)));
            instance.ForEach((line, i) =>
                line.ForEach((item, j) =>
                {
                    Assert.IsTrue(instance[i, j].Equals(
                            isInsertElement(i, j)
                                ? getForAddItems(i, j) // 追加要素
                                : getInitItem(i, j) // 既存要素
                        )
                    );
                }));
        }

        [TestCase(0, 0, -1, 5, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 1, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 5, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 5, 10, nameof(TestClass.ListType.Normal), false)]
        [TestCase(0, 0, 0, 5, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 10, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 6, 11, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 5, 10, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(0, 0, 0, 5, 10, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(0, 0, 0, 5, 10, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(0, 0, 0, 5, 10, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 5, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 5, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 6, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 6, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 3, 2, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 3, 2, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, 3, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 3, 3, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 4, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 4, 1, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, -1, 1, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 5, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 6, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 2, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 2, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 3, 2, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 3, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 3, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 3, 3, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 4, 1, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, 5, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 0, 5, 5, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 0, 5, 5, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 5, 0, 5, 5, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(5, 5, 0, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(5, 5, 0, 5, 5, nameof(TestClass.ListType.Normal), false)]
        [TestCase(5, 5, 0, 5, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(5, 5, 0, 6, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(5, 5, 1, 5, 5, nameof(TestClass.ListType.Normal), true)]
        public static void OverwriteRowTest(int initRowCount, int initColumnCount,
            int row, int itemRowLength, int addLineItemLength, string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var items = TestClass.ListType.FromId(addTypeId)
                .GetMultiLine(itemRowLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.OverwriteRow(row, items);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionOverwrite(instance, Direction.Row, initRowCount, initColumnCount,
                items, row, isError, changingEventArgsList, changedEventArgsList,
                propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 1, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 1, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 1, 5, nameof(TestClass.ListType.Normal), true)]
        [TestCase(0, 0, 0, 1, 6, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, -1, 5, 1, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 1, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 1, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 1, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 0, 0, 10, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 0, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 11, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 0, 0, 10, 3, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 5, -1, 1, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 0, 0, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 5, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 0, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 11, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 5, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 5, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 5, 5, 5, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 6, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 6, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 5, 6, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 6, 1, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.SelfNull), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.RowHasNull), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.ColumnHasNull), true)]
        [TestCase(3, 5, 0, 5, 3, nameof(TestClass.ListType.ColumnSizeDifference), true)]
        [TestCase(3, 10, 0, 10, 2, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 10, 0, 10, 3, nameof(TestClass.ListType.Normal), false)]
        [TestCase(3, 10, 0, 10, 4, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 10, 0, 11, 3, nameof(TestClass.ListType.Normal), true)]
        [TestCase(3, 10, 1, 10, 3, nameof(TestClass.ListType.Normal), true)]
        public static void OverwriteColumnTest(int initRowCount, int initColumnCount,
            int column, int itemColumnLength, int addLineItemLength,
            string addTypeId, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var items = TestClass.ListType.FromId(addTypeId)
                .GetMultiLine(itemColumnLength, addLineItemLength);
            var errorOccured = false;

            try
            {
                instance.OverwriteColumn(column, items);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionOverwrite(instance, Direction.Column, initRowCount, initColumnCount,
                items, column, isError, changingEventArgsList, changedEventArgsList,
                propertyChangedEventCalledCount);
        }

        private static void CommonAssertionOverwrite(TestClass.ListTest1 instance, Direction direction,
            int initRowCount,
            int initColCount, string[][] overwriteItems, int index,
            bool isError,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changingEventArgsList,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changedEventArgsList,
            Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var isOverwriteItemSizeZero =
                overwriteItems == null || overwriteItems.Length == 0;
            var replaceLength = isOverwriteItemSizeZero
                ? 0
                : Math.Min(
                    (direction == Direction.Row
                        ? initRowCount
                        : initColCount
                    ) - index,
                    overwriteItems.Length);
            var addLength = isOverwriteItemSizeZero
                ? 0
                : overwriteItems.Length - replaceLength;
            var isReplaced = !isError
                             && replaceLength > 0;
            var isAdded = !isError
                          && addLength > 0;

            // 意図したイベントが発生すること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count,
                            isReplaced ? 1 : 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count,
                            isAdded ? 1 : 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)],
                !isError && direction == Direction.Row ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)],
                !isError && direction == Direction.Column ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (isError) return;
            Assert.NotNull(overwriteItems);

            // 発生した Change イベントのパラメータが正しいこと
            if (isReplaced)
            {
                var assertCollectionReplaceEventArgsProperty =
                    new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                        args =>
                        {
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == (direction == Direction.Row ? index : 0));
                            Assert.IsTrue(args.OldStartColumn == (direction == Direction.Column ? index : 0));
                            {
                                var oldItems = args.OldItems!
                                    .ToTwoDimensionalArray();
                                CommonAssertion.ArraySizeEqual(oldItems, replaceLength,
                                    direction == Direction.Row
                                        ? initColCount
                                        : initRowCount);

                                oldItems.ForEach((line, i) =>
                                    line.ForEach((item, j) =>
                                        Assert.IsTrue(item.Equals(direction == Direction.Row
                                            ? TestClass.MakeDefaultValueItemForList(index + i, j)
                                            : TestClass.MakeDefaultValueItemForList(index + j, i)))));
                            }
                            Assert.IsTrue(args.NewStartRow == args.OldStartRow);
                            Assert.IsTrue(args.NewStartColumn == args.OldStartColumn);
                            {
                                var newItems = args.NewItems!
                                    .ToTwoDimensionalArray();
                                Assert.IsTrue(newItems.Length == replaceLength);
                                CommonAssertion.ArraySizeEqual(newItems, replaceLength,
                                    direction == Direction.Row
                                        ? initColCount
                                        : initRowCount);

                                newItems.ForEach((line, i) =>
                                    line.ForEach((item, j) =>
                                        Assert.IsTrue(item.Equals(direction == Direction.Row
                                            ? overwriteItems[index + i][j]
                                            : overwriteItems[i][index + j])))
                                );
                            }
                        });
                assertCollectionReplaceEventArgsProperty(
                    changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Replace)][0]);
                assertCollectionReplaceEventArgsProperty(
                    changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Replace)][0]);
            }

            if (isAdded)
            {
                var addItems = overwriteItems.Skip(replaceLength).ToArray();

                var assertCollectionChangeEventArgsProperty =
                    new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                        args =>
                        {
                            Assert.IsTrue(args.Direction == direction);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems == null);
                            Assert.IsTrue(args.NewStartRow
                                          == (direction == Direction.Row
                                              ? initRowCount
                                              : 0));
                            Assert.IsTrue(args.NewStartColumn
                                          == (direction == Direction.Column
                                              ? initColCount
                                              : 0));
                            {
                                var newItems = args.NewItems!
                                    .ToTwoDimensionalArray();
                                CommonAssertion.ArraySizeEqual(newItems, addItems);

                                Assert.IsTrue(newItems.Equals<string>(addItems));
                            }
                        });
                assertCollectionChangeEventArgsProperty(
                    changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)][0]);
                assertCollectionChangeEventArgsProperty(
                    changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)][0]);
            }

            // 行数・列数が意図した値であること
            Assert.AreEqual(instance.RowCount,
                direction == Direction.Row
                    ? initRowCount != 0
                        ? initRowCount + addLength
                        : isOverwriteItemSizeZero
                            ? 0
                            : addLength
                    : initRowCount != 0
                        ? initRowCount
                        : isOverwriteItemSizeZero
                            ? 0
                            : overwriteItems[0].Length);
            Assert.AreEqual(instance.ColumnCount,
                direction == Direction.Row
                    ? initColCount != 0
                        ? initColCount
                        : isOverwriteItemSizeZero
                            ? 0
                            : overwriteItems[0].Length
                    : initColCount != 0
                        ? initColCount + addLength
                        : isOverwriteItemSizeZero
                            ? 0
                            : addLength);

            // 各要素が意図した値であること
            var isOverwriteElement = new Func<int, int, bool>((row, col) => direction == Direction.Row
                ? index <= row && row < index + overwriteItems.Length
                : index <= col && col < index + overwriteItems.Length);
            var getForOverwriteItems = new Func<int, int, string>((row, col) => direction == Direction.Row
                ? overwriteItems[row - index][col]
                : overwriteItems[col - index][row]);
            instance.ForEach((line, i) =>
                line.ForEach((item, j) =>
                {
                    Assert.IsTrue(item.Equals(
                            isOverwriteElement(i, j)
                                ? getForOverwriteItems(i, j) // 追加要素
                                : TestClass.MakeDefaultValueItemForList(i, j) // 既存要素
                        )
                    );
                }));
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, -1, -1, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 2, true)]
        [TestCase(3, 0, -1, 3, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, false)]
        [TestCase(3, 0, 0, 2, false)]
        [TestCase(3, 0, 0, 3, true)]
        [TestCase(3, 0, 2, -1, true)]
        [TestCase(3, 0, 2, 0, false)]
        [TestCase(3, 0, 2, 2, false)]
        [TestCase(3, 0, 2, 3, true)]
        [TestCase(3, 0, 3, -1, true)]
        [TestCase(3, 0, 3, 0, true)]
        [TestCase(3, 0, 3, 2, true)]
        [TestCase(3, 0, 3, 3, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 2, true)]
        [TestCase(3, 5, -1, 3, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 2, false)]
        [TestCase(3, 5, 0, 3, true)]
        [TestCase(3, 5, 2, -1, true)]
        [TestCase(3, 5, 2, 0, false)]
        [TestCase(3, 5, 2, 2, false)]
        [TestCase(3, 5, 2, 3, true)]
        [TestCase(3, 5, 3, -1, true)]
        [TestCase(3, 5, 3, 0, true)]
        [TestCase(3, 5, 3, 2, true)]
        [TestCase(3, 5, 3, 3, true)]
        public static void MoveRowTest(int initRowCount, int initColumnCount,
            int oldRow, int newRow, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.MoveRow(oldRow, newRow);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionMove(instance, Direction.Row, oldRow, newRow, 1, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, -1, -1, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 4, true)]
        [TestCase(3, 0, -1, 5, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 4, true)]
        [TestCase(3, 0, 0, 5, true)]
        [TestCase(3, 0, 4, -1, true)]
        [TestCase(3, 0, 4, 0, true)]
        [TestCase(3, 0, 4, 4, true)]
        [TestCase(3, 0, 4, 5, true)]
        [TestCase(3, 0, 5, -1, true)]
        [TestCase(3, 0, 5, 0, true)]
        [TestCase(3, 0, 5, 4, true)]
        [TestCase(3, 0, 5, 5, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 4, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 4, false)]
        [TestCase(3, 5, 0, 5, true)]
        [TestCase(3, 5, 4, -1, true)]
        [TestCase(3, 5, 4, 0, false)]
        [TestCase(3, 5, 4, 4, false)]
        [TestCase(3, 5, 4, 5, true)]
        [TestCase(3, 5, 5, -1, true)]
        [TestCase(3, 5, 5, 0, true)]
        [TestCase(3, 5, 5, 4, true)]
        [TestCase(3, 5, 5, 5, true)]
        public static void MoveColumnTest(int initRowCount, int initColumnCount,
            int oldColumn, int newColumn, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.MoveColumn(oldColumn, newColumn);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionMove(instance, Direction.Column, oldColumn, newColumn, 1, initRowCount, initColumnCount,
                isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, 0, true)]
        [TestCase(3, 0, -1, -1, 0, true)]
        [TestCase(3, 0, -1, 0, 2, true)]
        [TestCase(3, 0, -1, 2, 3, true)]
        [TestCase(3, 0, -1, 3, -1, true)]
        [TestCase(3, 0, 0, -1, -1, true)]
        [TestCase(3, 0, 0, 0, 0, false)]
        [TestCase(3, 0, 0, 0, 3, false)]
        [TestCase(3, 0, 0, 0, 4, true)]
        [TestCase(3, 0, 0, 1, 2, false)]
        [TestCase(3, 0, 0, 1, 3, true)]
        [TestCase(3, 0, 0, 2, 1, false)]
        [TestCase(3, 0, 0, 3, 0, false)]
        [TestCase(3, 0, 2, -1, 1, true)]
        [TestCase(3, 0, 2, 0, -1, true)]
        [TestCase(3, 0, 2, 2, 0, false)]
        [TestCase(3, 0, 2, 3, 1, true)]
        [TestCase(3, 0, 3, -1, 1, true)]
        [TestCase(3, 0, 3, 0, 0, true)]
        [TestCase(3, 0, 3, 2, -1, true)]
        [TestCase(3, 0, 3, 3, 0, true)]
        [TestCase(3, 5, -1, -1, 0, true)]
        [TestCase(3, 5, -1, 0, 2, true)]
        [TestCase(3, 5, -1, 2, 3, true)]
        [TestCase(3, 5, -1, 3, -1, true)]
        [TestCase(3, 5, 0, -1, -1, true)]
        [TestCase(3, 5, 0, 0, 0, false)]
        [TestCase(3, 5, 0, 0, 3, false)]
        [TestCase(3, 5, 0, 0, 4, true)]
        [TestCase(3, 5, 0, 1, 2, false)]
        [TestCase(3, 5, 0, 1, 3, true)]
        [TestCase(3, 5, 0, 2, 1, false)]
        [TestCase(3, 5, 0, 3, 0, false)]
        [TestCase(3, 5, 2, -1, 1, true)]
        [TestCase(3, 5, 2, 0, -1, true)]
        [TestCase(3, 5, 2, 2, 0, false)]
        [TestCase(3, 5, 2, 3, 1, true)]
        [TestCase(3, 5, 3, -1, 1, true)]
        [TestCase(3, 5, 3, 0, 0, true)]
        [TestCase(3, 5, 3, 2, -1, true)]
        [TestCase(3, 5, 3, 3, 0, true)]
        public static void MoveRowRangeTest(int initRowCount, int initColumnCount,
            int oldRow, int newRow, int count, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.MoveRowRange(oldRow, newRow, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionMove(instance, Direction.Row, oldRow, newRow, count, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, 0, true)]
        [TestCase(3, 0, -1, -1, 0, true)]
        [TestCase(3, 0, -1, 0, 4, true)]
        [TestCase(3, 0, -1, 4, 5, true)]
        [TestCase(3, 0, -1, 5, -1, true)]
        [TestCase(3, 0, 0, -1, -1, true)]
        [TestCase(3, 0, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 0, 5, true)]
        [TestCase(3, 0, 0, 0, 6, true)]
        [TestCase(3, 0, 0, 1, 4, true)]
        [TestCase(3, 0, 0, 1, 5, true)]
        [TestCase(3, 0, 0, 4, 1, true)]
        [TestCase(3, 0, 0, 5, 0, true)]
        [TestCase(3, 0, 4, -1, 1, true)]
        [TestCase(3, 0, 4, 0, -1, true)]
        [TestCase(3, 0, 4, 4, 0, true)]
        [TestCase(3, 0, 4, 5, 1, true)]
        [TestCase(3, 0, 5, -1, 1, true)]
        [TestCase(3, 0, 5, 0, 0, true)]
        [TestCase(3, 0, 5, 4, -1, true)]
        [TestCase(3, 0, 5, 5, 0, true)]
        [TestCase(3, 5, -1, -1, 0, true)]
        [TestCase(3, 5, -1, 0, 4, true)]
        [TestCase(3, 5, -1, 4, 5, true)]
        [TestCase(3, 5, -1, 5, -1, true)]
        [TestCase(3, 5, 0, -1, -1, true)]
        [TestCase(3, 5, 0, 0, 0, false)]
        [TestCase(3, 5, 0, 0, 5, false)]
        [TestCase(3, 5, 0, 0, 6, true)]
        [TestCase(3, 5, 0, 1, 4, false)]
        [TestCase(3, 5, 0, 1, 5, true)]
        [TestCase(3, 5, 0, 4, 1, false)]
        [TestCase(3, 5, 0, 5, 0, false)]
        [TestCase(3, 5, 4, -1, 1, true)]
        [TestCase(3, 5, 4, 0, -1, true)]
        [TestCase(3, 5, 4, 4, 0, false)]
        [TestCase(3, 5, 4, 5, 1, true)]
        [TestCase(3, 5, 5, -1, 1, true)]
        [TestCase(3, 5, 5, 0, 0, true)]
        [TestCase(3, 5, 5, 4, -1, true)]
        [TestCase(3, 5, 5, 5, 0, true)]
        public static void MoveColumnRangeTest(int initRowCount, int initColumnCount,
            int oldColumn, int newColumn, int count, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.MoveColumnRange(oldColumn, newColumn, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionMove(instance, Direction.Column, oldColumn, newColumn, count, initRowCount, initColumnCount,
                isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        private static void CommonAssertionMove(TestClass.ListTest1 instance, Direction direction,
            int oldIndex, int newIndex, int moveCount, int initRowLength, int initColumnLength, bool isError,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changingEventArgsList,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changedEventArgsList,
            Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var isMoved = !isError;

            // 意図したイベントが発生すること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count,
                            isMoved ? 1 : 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)], 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (isError) return;

            // 発生した Change イベントのパラメータが正しいこと
            var assertCollectionChangeEventArgsProperty =
                new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                    args =>
                    {
                        Assert.IsTrue(args.Direction == direction);
                        Assert.IsTrue(args.OldStartRow ==
                                      (direction == Direction.Row
                                          ? oldIndex
                                          : 0));
                        Assert.IsTrue(args.OldStartColumn ==
                                      (direction == Direction.Column
                                          ? oldIndex
                                          : 0));
                        {
                            var oldItems = args.OldItems!.ToTwoDimensionalArray();

                            CommonAssertion.ArraySizeEqual(oldItems, moveCount, direction == Direction.Row
                                ? initColumnLength
                                : initRowLength);

                            oldItems.ForEach((line, i) =>
                                line.ForEach((item, j) =>
                                    Assert.IsTrue(item.Equals(
                                        direction == Direction.Row
                                            ? TestClass.MakeDefaultValueItemForList(oldIndex + i, j)
                                            : TestClass.MakeDefaultValueItemForList(j, oldIndex + i))
                                    )));
                        }
                        Assert.IsTrue(args.NewStartRow == (direction == Direction.Row ? newIndex : 0));
                        Assert.IsTrue(args.NewStartColumn == (direction == Direction.Column ? newIndex : 0));
                        {
                            var newItems = args.NewItems!.ToTwoDimensionalArray();

                            Assert.IsTrue(newItems.Length == moveCount);
                            CommonAssertion.ArraySizeEqual(newItems, moveCount, direction == Direction.Row
                                ? initColumnLength
                                : initRowLength);

                            newItems.ForEach((line, i) =>
                                line.ForEach((item, j) =>
                                    Assert.IsTrue(item.Equals(
                                        direction == Direction.Row
                                            ? TestClass.MakeDefaultValueItemForList(oldIndex + i, j)
                                            : TestClass.MakeDefaultValueItemForList(j, oldIndex + i))
                                    )));
                        }
                    });
            assertCollectionChangeEventArgsProperty(
                changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Move)][0]);
            assertCollectionChangeEventArgsProperty(
                changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Move)][0]);

            // 移動後の行数・列数が正しいこと
            CommonAssertion.SizeEqual(instance, initRowLength, initColumnLength);

            // 移動前後の要素関係を取得 -> 取得されるべき文字列に変換
            var outer = Enumerable.Range(0, initRowLength).ToList();
            if (direction == Direction.Row)
            {
                var moveItems = outer.GetRange(oldIndex, moveCount);
                outer.RemoveRange(oldIndex, moveCount);
                outer.InsertRange(newIndex, moveItems);
            }

            var inner = Enumerable.Range(0, initColumnLength).ToList();
            if (direction == Direction.Column)
            {
                var moveItems = inner.GetRange(oldIndex, moveCount);
                inner.RemoveRange(oldIndex, moveCount);
                inner.InsertRange(newIndex, moveItems);
            }

            var movedItems = outer.Select(i =>
                    inner.Select(j => TestClass.MakeDefaultValueItemForList(i, j)).ToList())
                .ToList();

            // 移動後の各要素が正しいこと
            Assert.IsTrue(instance.Equals(movedItems));
        }

        [TestCase(0, 0, 0, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 2, false)]
        [TestCase(3, 0, 3, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 2, false)]
        [TestCase(3, 5, 3, true)]
        public static void RemoveRowTest(int initRowCount, int initColumnCount,
            int row, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.RemoveRow(row);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionRemove(instance, Direction.Row, row, 1, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, true)]
        [TestCase(3, 0, 4, true)]
        [TestCase(3, 0, 5, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 4, false)]
        [TestCase(3, 5, 5, true)]
        public static void RemoveColumnTest(int initRowCount, int initColumnCount,
            int column, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.RemoveColumn(column);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionRemove(instance, Direction.Column, column, 1, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 2, true)]
        [TestCase(3, 0, -1, 3, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, false)]
        [TestCase(3, 0, 0, 3, false)]
        [TestCase(3, 0, 0, 4, true)]
        [TestCase(3, 0, 2, -1, true)]
        [TestCase(3, 0, 2, 0, false)]
        [TestCase(3, 0, 2, 1, false)]
        [TestCase(3, 0, 2, 2, true)]
        [TestCase(3, 0, 3, 0, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 2, true)]
        [TestCase(3, 5, -1, 3, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 3, false)]
        [TestCase(3, 5, 0, 4, true)]
        [TestCase(3, 5, 2, -1, true)]
        [TestCase(3, 5, 2, 0, false)]
        [TestCase(3, 5, 2, 1, false)]
        [TestCase(3, 5, 2, 2, true)]
        [TestCase(3, 5, 3, 0, true)]
        public static void RemoveRowRangeTest(int initRowCount, int initColumnCount,
            int row, int count, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.RemoveRowRange(row, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionRemove(instance, Direction.Row, row, count, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, 0, 0, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 4, true)]
        [TestCase(3, 0, -1, 5, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, true)]
        [TestCase(3, 0, 0, 5, true)]
        [TestCase(3, 0, 0, 6, true)]
        [TestCase(3, 0, 4, -1, true)]
        [TestCase(3, 0, 4, 0, true)]
        [TestCase(3, 0, 4, 1, true)]
        [TestCase(3, 0, 4, 2, true)]
        [TestCase(3, 0, 5, 0, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 4, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 5, false)]
        [TestCase(3, 5, 0, 6, true)]
        [TestCase(3, 5, 4, -1, true)]
        [TestCase(3, 5, 4, 0, false)]
        [TestCase(3, 5, 4, 1, false)]
        [TestCase(3, 5, 4, 2, true)]
        [TestCase(3, 5, 5, 0, true)]
        public static void RemoveColumnRangeTest(int initRowCount, int initColumnCount,
            int column, int count, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.RemoveColumnRange(column, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionRemove(instance, Direction.Column, column, count, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        private static void CommonAssertionRemove(TestClass.IListTest instance, Direction direction,
            int index, int count, int initRowLength, int initColumnLength, bool isError,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changingEventArgsList,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changedEventArgsList,
            Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var isRemoved = !isError;
            var allRemoved = new Func<bool>(() =>
            {
                if (!isRemoved) return false;
                if (direction == Direction.Row && initRowLength != count) return false;
                if (direction == Direction.Column) return false;
                return index == 0;
            })();

            // 意図したイベントが発生すること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count,
                            isRemoved ? 1 : 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            var isRowCountEventRaise = new Func<bool>(() =>
            {
                if (!isRemoved) return false;
                if (direction == Direction.Row) return true;
                return allRemoved;
            })();
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)],
                isRowCountEventRaise ? 1 : 0);
            var isColumnCountEventRaise = new Func<bool>(() =>
            {
                if (!isRemoved) return false;
                if (direction == Direction.Column) return true;
                return allRemoved;
            })();
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)],
                isColumnCountEventRaise ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (isError) return;

            // 発生した Change イベントのパラメータが正しいこと
            var assertCollectionChangeEventArgsProperty =
                new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                    args =>
                    {
                        Assert.IsTrue(args.Direction == direction);
                        Assert.IsTrue(args.OldStartRow ==
                                      (direction == Direction.Row
                                          ? index
                                          : 0));
                        Assert.IsTrue(args.OldStartColumn ==
                                      (direction == Direction.Column
                                          ? index
                                          : 0));
                        {
                            var oldItems = args.OldItems!.ToTwoDimensionalArray();

                            CommonAssertion.ArraySizeEqual(oldItems, count, direction == Direction.Row
                                ? initColumnLength
                                : initRowLength);

                            oldItems.ForEach((line, i) =>
                                line.ForEach((item, j) =>
                                    Assert.IsTrue(item.Equals(
                                        direction == Direction.Row
                                            ? TestClass.MakeDefaultValueItemForList(index + i, j)
                                            : TestClass.MakeDefaultValueItemForList(j, index + i))
                                    )));
                        }
                        Assert.IsTrue(args.NewStartRow == -1);
                        Assert.IsTrue(args.NewStartColumn == -1);
                        Assert.IsTrue(args.NewItems == null);
                    });
            assertCollectionChangeEventArgsProperty(
                changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Remove)][0]);
            assertCollectionChangeEventArgsProperty(
                changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Remove)][0]);

            // 除去後の行数・列数が正しいこと
            var removedRowCount = new Func<int>(() =>
            {
                if (allRemoved) return 0;
                if (direction == Direction.Column) return initRowLength;
                return initRowLength - count;
            })();
            var removedColumnCount = new Func<int>(() =>
            {
                if (allRemoved) return 0;
                if (direction == Direction.Row) return initColumnLength;
                return initColumnLength - count;
            })();
            CommonAssertion.SizeEqual(instance,
                removedRowCount,
                removedColumnCount);

            // 除去前後の要素関係を取得 -> 取得されるべき文字列に変換
            var outer = Enumerable.Range(0, initRowLength).ToList();
            if (direction == Direction.Row)
            {
                outer.RemoveRange(index, count);
            }

            var inner = Enumerable.Range(0, initColumnLength).ToList();
            if (direction == Direction.Column)
            {
                inner.RemoveRange(index, count);
            }

            var movedItems = outer.Select(i =>
                    inner.Select(j => TestClass.MakeDefaultValueItemForList(i, j)).ToList())
                .ToList();

            // 除去後の各要素が正しいこと
            Assert.IsTrue(instance.Equals(movedItems));
        }

        [TestCase(0, 0, -1, -1, true)]
        [TestCase(0, 0, -1, 0, true)]
        [TestCase(0, 0, -1, 1, true)]
        [TestCase(0, 0, -1, 10, true)]
        [TestCase(0, 0, -1, 11, true)]
        [TestCase(0, 0, 0, -1, true)]
        [TestCase(0, 0, 0, 0, false)]
        [TestCase(0, 0, 0, 1, true)]
        [TestCase(0, 0, 0, 10, true)]
        [TestCase(0, 0, 0, 11, true)]
        [TestCase(0, 0, 1, -1, true)]
        [TestCase(0, 0, 1, 0, false)]
        [TestCase(0, 0, 1, 1, false)]
        [TestCase(0, 0, 1, 10, false)]
        [TestCase(0, 0, 1, 11, true)]
        [TestCase(0, 0, 3, -1, true)]
        [TestCase(0, 0, 3, 0, false)]
        [TestCase(0, 0, 3, 1, false)]
        [TestCase(0, 0, 3, 10, false)]
        [TestCase(0, 0, 3, 11, true)]
        [TestCase(0, 0, 5, -1, true)]
        [TestCase(0, 0, 5, 0, false)]
        [TestCase(0, 0, 5, 1, false)]
        [TestCase(0, 0, 5, 10, false)]
        [TestCase(0, 0, 5, 11, true)]
        [TestCase(0, 0, 6, -1, true)]
        [TestCase(0, 0, 6, 0, true)]
        [TestCase(0, 0, 6, 1, true)]
        [TestCase(0, 0, 6, 10, true)]
        [TestCase(0, 0, 6, 11, true)]
        [TestCase(3, 0, -1, -1, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 1, true)]
        [TestCase(3, 0, -1, 10, true)]
        [TestCase(3, 0, -1, 11, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, false)]
        [TestCase(3, 0, 0, 1, true)]
        [TestCase(3, 0, 0, 10, true)]
        [TestCase(3, 0, 0, 11, true)]
        [TestCase(3, 0, 1, -1, true)]
        [TestCase(3, 0, 1, 0, false)]
        [TestCase(3, 0, 1, 1, false)]
        [TestCase(3, 0, 1, 10, false)]
        [TestCase(3, 0, 1, 11, true)]
        [TestCase(3, 0, 3, -1, true)]
        [TestCase(3, 0, 3, 0, false)]
        [TestCase(3, 0, 3, 1, false)]
        [TestCase(3, 0, 3, 10, false)]
        [TestCase(3, 0, 3, 11, true)]
        [TestCase(3, 0, 5, -1, true)]
        [TestCase(3, 0, 5, 0, false)]
        [TestCase(3, 0, 5, 1, false)]
        [TestCase(3, 0, 5, 10, false)]
        [TestCase(3, 0, 5, 11, true)]
        [TestCase(3, 0, 6, -1, true)]
        [TestCase(3, 0, 6, 0, true)]
        [TestCase(3, 0, 6, 1, true)]
        [TestCase(3, 0, 6, 10, true)]
        [TestCase(3, 0, 6, 11, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, -1, 10, true)]
        [TestCase(3, 5, -1, 11, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 1, true)]
        [TestCase(3, 5, 0, 5, true)]
        [TestCase(3, 5, 0, 10, true)]
        [TestCase(3, 5, 0, 11, true)]
        [TestCase(3, 5, 1, -1, true)]
        [TestCase(3, 5, 1, 0, false)]
        [TestCase(3, 5, 1, 1, false)]
        [TestCase(3, 5, 1, 5, false)]
        [TestCase(3, 5, 1, 10, false)]
        [TestCase(3, 5, 1, 11, true)]
        [TestCase(3, 5, 3, -1, true)]
        [TestCase(3, 5, 3, 0, false)]
        [TestCase(3, 5, 3, 1, false)]
        [TestCase(3, 5, 3, 5, false)]
        [TestCase(3, 5, 3, 10, false)]
        [TestCase(3, 5, 3, 11, true)]
        [TestCase(3, 5, 5, -1, true)]
        [TestCase(3, 5, 5, 0, false)]
        [TestCase(3, 5, 5, 1, false)]
        [TestCase(3, 5, 5, 5, false)]
        [TestCase(3, 5, 5, 10, false)]
        [TestCase(3, 5, 5, 11, true)]
        [TestCase(3, 5, 6, -1, true)]
        [TestCase(3, 5, 6, 0, true)]
        [TestCase(3, 5, 6, 1, true)]
        [TestCase(3, 5, 6, 5, true)]
        [TestCase(3, 5, 6, 10, true)]
        [TestCase(3, 5, 6, 11, true)]
        public static void AdjustLengthTest(int initRowCount, int initColumnCount,
            int rowLength, int columnLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustLength(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthBothDirection(instance, AdjustLengthType.None,
                rowLength, columnLength, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, -1, true)]
        [TestCase(0, 0, -1, 0, true)]
        [TestCase(0, 0, -1, 1, true)]
        [TestCase(0, 0, -1, 10, true)]
        [TestCase(0, 0, -1, 11, true)]
        [TestCase(0, 0, 0, -1, true)]
        [TestCase(0, 0, 0, 0, false)]
        [TestCase(0, 0, 0, 1, true)]
        [TestCase(0, 0, 0, 10, true)]
        [TestCase(0, 0, 0, 11, true)]
        [TestCase(0, 0, 1, -1, true)]
        [TestCase(0, 0, 1, 0, false)]
        [TestCase(0, 0, 1, 1, false)]
        [TestCase(0, 0, 1, 10, false)]
        [TestCase(0, 0, 1, 11, true)]
        [TestCase(0, 0, 3, -1, true)]
        [TestCase(0, 0, 3, 0, false)]
        [TestCase(0, 0, 3, 1, false)]
        [TestCase(0, 0, 3, 10, false)]
        [TestCase(0, 0, 3, 11, true)]
        [TestCase(0, 0, 5, -1, true)]
        [TestCase(0, 0, 5, 0, false)]
        [TestCase(0, 0, 5, 1, false)]
        [TestCase(0, 0, 5, 10, false)]
        [TestCase(0, 0, 5, 11, true)]
        [TestCase(0, 0, 6, -1, true)]
        [TestCase(0, 0, 6, 0, true)]
        [TestCase(0, 0, 6, 1, true)]
        [TestCase(0, 0, 6, 10, true)]
        [TestCase(0, 0, 6, 11, true)]
        [TestCase(3, 0, -1, -1, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 1, true)]
        [TestCase(3, 0, -1, 10, true)]
        [TestCase(3, 0, -1, 11, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, false)]
        [TestCase(3, 0, 0, 1, true)]
        [TestCase(3, 0, 0, 10, true)]
        [TestCase(3, 0, 0, 11, true)]
        [TestCase(3, 0, 1, -1, true)]
        [TestCase(3, 0, 1, 0, false)]
        [TestCase(3, 0, 1, 1, false)]
        [TestCase(3, 0, 1, 10, false)]
        [TestCase(3, 0, 1, 11, true)]
        [TestCase(3, 0, 3, -1, true)]
        [TestCase(3, 0, 3, 0, false)]
        [TestCase(3, 0, 3, 1, false)]
        [TestCase(3, 0, 3, 10, false)]
        [TestCase(3, 0, 3, 11, true)]
        [TestCase(3, 0, 5, -1, true)]
        [TestCase(3, 0, 5, 0, false)]
        [TestCase(3, 0, 5, 1, false)]
        [TestCase(3, 0, 5, 10, false)]
        [TestCase(3, 0, 5, 11, true)]
        [TestCase(3, 0, 6, -1, true)]
        [TestCase(3, 0, 6, 0, true)]
        [TestCase(3, 0, 6, 1, true)]
        [TestCase(3, 0, 6, 10, true)]
        [TestCase(3, 0, 6, 11, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, -1, 10, true)]
        [TestCase(3, 5, -1, 11, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 1, true)]
        [TestCase(3, 5, 0, 5, true)]
        [TestCase(3, 5, 0, 10, true)]
        [TestCase(3, 5, 0, 11, true)]
        [TestCase(3, 5, 1, -1, true)]
        [TestCase(3, 5, 1, 0, false)]
        [TestCase(3, 5, 1, 1, false)]
        [TestCase(3, 5, 1, 5, false)]
        [TestCase(3, 5, 1, 10, false)]
        [TestCase(3, 5, 1, 11, true)]
        [TestCase(3, 5, 3, -1, true)]
        [TestCase(3, 5, 3, 0, false)]
        [TestCase(3, 5, 3, 1, false)]
        [TestCase(3, 5, 3, 5, false)]
        [TestCase(3, 5, 3, 10, false)]
        [TestCase(3, 5, 3, 11, true)]
        [TestCase(3, 5, 5, -1, true)]
        [TestCase(3, 5, 5, 0, false)]
        [TestCase(3, 5, 5, 1, false)]
        [TestCase(3, 5, 5, 5, false)]
        [TestCase(3, 5, 5, 10, false)]
        [TestCase(3, 5, 5, 11, true)]
        [TestCase(3, 5, 6, -1, true)]
        [TestCase(3, 5, 6, 0, true)]
        [TestCase(3, 5, 6, 1, true)]
        [TestCase(3, 5, 6, 5, true)]
        [TestCase(3, 5, 6, 10, true)]
        [TestCase(3, 5, 6, 11, true)]
        public static void AdjustLengthIfLongTest(int initRowCount, int initColumnCount,
            int rowLength, int columnLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustLengthIfLong(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthBothDirection(instance, AdjustLengthType.IfLong,
                rowLength, columnLength, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, -1, true)]
        [TestCase(0, 0, -1, 0, true)]
        [TestCase(0, 0, -1, 1, true)]
        [TestCase(0, 0, -1, 10, true)]
        [TestCase(0, 0, -1, 11, true)]
        [TestCase(0, 0, 0, -1, true)]
        [TestCase(0, 0, 0, 0, false)]
        [TestCase(0, 0, 0, 1, true)]
        [TestCase(0, 0, 0, 10, true)]
        [TestCase(0, 0, 0, 11, true)]
        [TestCase(0, 0, 1, -1, true)]
        [TestCase(0, 0, 1, 0, false)]
        [TestCase(0, 0, 1, 1, false)]
        [TestCase(0, 0, 1, 10, false)]
        [TestCase(0, 0, 1, 11, true)]
        [TestCase(0, 0, 3, -1, true)]
        [TestCase(0, 0, 3, 0, false)]
        [TestCase(0, 0, 3, 1, false)]
        [TestCase(0, 0, 3, 10, false)]
        [TestCase(0, 0, 3, 11, true)]
        [TestCase(0, 0, 5, -1, true)]
        [TestCase(0, 0, 5, 0, false)]
        [TestCase(0, 0, 5, 1, false)]
        [TestCase(0, 0, 5, 10, false)]
        [TestCase(0, 0, 5, 11, true)]
        [TestCase(0, 0, 6, -1, true)]
        [TestCase(0, 0, 6, 0, true)]
        [TestCase(0, 0, 6, 1, true)]
        [TestCase(0, 0, 6, 10, true)]
        [TestCase(0, 0, 6, 11, true)]
        [TestCase(3, 0, -1, -1, true)]
        [TestCase(3, 0, -1, 0, true)]
        [TestCase(3, 0, -1, 1, true)]
        [TestCase(3, 0, -1, 10, true)]
        [TestCase(3, 0, -1, 11, true)]
        [TestCase(3, 0, 0, -1, true)]
        [TestCase(3, 0, 0, 0, false)]
        [TestCase(3, 0, 0, 1, true)]
        [TestCase(3, 0, 0, 10, true)]
        [TestCase(3, 0, 0, 11, true)]
        [TestCase(3, 0, 1, -1, true)]
        [TestCase(3, 0, 1, 0, false)]
        [TestCase(3, 0, 1, 1, false)]
        [TestCase(3, 0, 1, 10, false)]
        [TestCase(3, 0, 1, 11, true)]
        [TestCase(3, 0, 3, -1, true)]
        [TestCase(3, 0, 3, 0, false)]
        [TestCase(3, 0, 3, 1, false)]
        [TestCase(3, 0, 3, 10, false)]
        [TestCase(3, 0, 3, 11, true)]
        [TestCase(3, 0, 5, -1, true)]
        [TestCase(3, 0, 5, 0, false)]
        [TestCase(3, 0, 5, 1, false)]
        [TestCase(3, 0, 5, 10, false)]
        [TestCase(3, 0, 5, 11, true)]
        [TestCase(3, 0, 6, -1, true)]
        [TestCase(3, 0, 6, 0, true)]
        [TestCase(3, 0, 6, 1, true)]
        [TestCase(3, 0, 6, 10, true)]
        [TestCase(3, 0, 6, 11, true)]
        [TestCase(3, 5, -1, -1, true)]
        [TestCase(3, 5, -1, 0, true)]
        [TestCase(3, 5, -1, 5, true)]
        [TestCase(3, 5, -1, 10, true)]
        [TestCase(3, 5, -1, 11, true)]
        [TestCase(3, 5, 0, -1, true)]
        [TestCase(3, 5, 0, 0, false)]
        [TestCase(3, 5, 0, 1, true)]
        [TestCase(3, 5, 0, 5, true)]
        [TestCase(3, 5, 0, 10, true)]
        [TestCase(3, 5, 0, 11, true)]
        [TestCase(3, 5, 1, -1, true)]
        [TestCase(3, 5, 1, 0, false)]
        [TestCase(3, 5, 1, 1, false)]
        [TestCase(3, 5, 1, 5, false)]
        [TestCase(3, 5, 1, 10, false)]
        [TestCase(3, 5, 1, 11, true)]
        [TestCase(3, 5, 3, -1, true)]
        [TestCase(3, 5, 3, 0, false)]
        [TestCase(3, 5, 3, 1, false)]
        [TestCase(3, 5, 3, 5, false)]
        [TestCase(3, 5, 3, 10, false)]
        [TestCase(3, 5, 3, 11, true)]
        [TestCase(3, 5, 5, -1, true)]
        [TestCase(3, 5, 5, 0, false)]
        [TestCase(3, 5, 5, 1, false)]
        [TestCase(3, 5, 5, 5, false)]
        [TestCase(3, 5, 5, 10, false)]
        [TestCase(3, 5, 5, 11, true)]
        [TestCase(3, 5, 6, -1, true)]
        [TestCase(3, 5, 6, 0, true)]
        [TestCase(3, 5, 6, 1, true)]
        [TestCase(3, 5, 6, 5, true)]
        [TestCase(3, 5, 6, 10, true)]
        [TestCase(3, 5, 6, 11, true)]
        public static void AdjustLengthIfShortTest(int initRowCount, int initColumnCount,
            int rowLength, int columnLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustLengthIfShort(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthBothDirection(instance, AdjustLengthType.IfShort,
                rowLength, columnLength, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        private static void CommonAssertionAdjustLengthBothDirection(TestClass.IListTest instance,
            AdjustLengthType type, int rowCount, int columnCount,
            int initRowLength, int initColumnLength, bool isError,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changingEventArgsList,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changedEventArgsList,
            Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var isEmptyTo = rowCount == 0;
            var isEmptyFrom = initRowLength == 0;
            var isAddedRow = type switch
            {
                AdjustLengthType.None => isEmptyFrom & !isEmptyTo
                                         || initRowLength < rowCount,
                AdjustLengthType.IfShort => isEmptyFrom & !isEmptyTo
                                            || initRowLength < rowCount,
                AdjustLengthType.IfLong => false,
                _ => throw new InvalidOperationException()
            };
            var isRemovedRow = type switch
            {
                AdjustLengthType.None => !isEmptyFrom & isEmptyTo
                                         || initRowLength > rowCount,
                AdjustLengthType.IfShort => false,
                AdjustLengthType.IfLong => !isEmptyFrom & isEmptyTo
                                           || initRowLength > rowCount,
                _ => throw new InvalidOperationException()
            };
            var isAddedColumn = type switch
            {
                AdjustLengthType.None => isEmptyFrom & !isEmptyTo
                                         || initColumnLength < columnCount,
                AdjustLengthType.IfShort => isEmptyFrom & !isEmptyTo
                                            || initColumnLength < columnCount,
                AdjustLengthType.IfLong => false,
                _ => throw new InvalidOperationException()
            };
            var isRemovedColumn = type switch
            {
                AdjustLengthType.None => !isEmptyFrom & isEmptyTo
                                         || initColumnLength > columnCount,
                AdjustLengthType.IfShort => false,
                AdjustLengthType.IfLong => !isEmptyFrom & isEmptyTo
                                           || initColumnLength > columnCount,
                _ => throw new InvalidOperationException()
            };

            isAddedRow &= !isError;
            isRemovedRow &= !isError;
            isAddedColumn &= !isError;
            isRemovedColumn &= !isError;

            // 意図したイベントが発生すること
            var addEventCount = (isError, isAddedRow, isAddedColumn, isEmptyFrom) switch
            {
                (true, _, _, _) => 0,
                (_, true, true, false) => 2,
                (_, false, false, _) => 0,
                _ => 1
            };
            var removeEventCount = (isError, isRemovedRow, isRemovedColumn, isEmptyTo) switch
            {
                (true, _, _, _) => 0,
                (_, true, true, false) => 2,
                (_, false, false, _) => 0,
                _ => 1
            };

            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count,
                            addEventCount);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count,
                            removeEventCount);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)],
                !isError ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)],
                !isError ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (isError) return;

            // 発生した Add イベントのパラメータが正しいこと
            if (isAddedRow || isAddedColumn)
            {
                Assert.IsFalse(isEmptyTo);
                Assert.IsFalse(type == AdjustLengthType.IfLong);
                var addRowRowLength = (type, isEmptyFrom) switch
                {
                    (_, true) => rowCount,
                    (AdjustLengthType.None, _) => rowCount - initRowLength,
                    (AdjustLengthType.IfShort, _) => rowCount - initRowLength,
                    _ => throw new InvalidOperationException()
                };
                var addRowColumnLength = (type, isEmptyFrom) switch
                {
                    (_, true) => columnCount,
                    (AdjustLengthType.None, _) => Math.Min(columnCount, initColumnLength),
                    (AdjustLengthType.IfShort, _) => initColumnLength,
                    _ => throw new InvalidOperationException()
                };
                var addColumnRowLength = (type, isEmptyFrom) switch
                {
                    (_, true) => -1, // -1: 使用しない値だがエラーではない
                    (AdjustLengthType.None, _) => rowCount,
                    (AdjustLengthType.IfShort, _) => Math.Max(rowCount, initRowLength),
                    _ => throw new InvalidOperationException()
                };
                var addColumnColumnLength = (type, isEmptyFrom) switch
                {
                    (_, true) => -1,
                    (AdjustLengthType.None, _) => columnCount - initColumnLength,
                    (AdjustLengthType.IfShort, _) => columnCount - initColumnLength,
                    _ => throw new InvalidOperationException()
                };
                var assertCollectionChangeEventArgsProperty =
                    new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                        args =>
                        {
                            var newItems = args.NewItems!.ToTwoDimensionalArray();

                            {
                                // 通知によらず共通
                                Assert.IsTrue(args.OldStartRow == -1);
                                Assert.IsTrue(args.OldStartColumn == -1);
                                Assert.IsTrue(args.OldItems == null);
                            }

                            if (args.Direction == Direction.Row)
                            {
                                // 行追加通知
                                Assert.IsTrue(args.NewStartColumn == 0);
                                CommonAssertion.ArraySizeEqual(newItems, addRowRowLength, addRowColumnLength);

                                var addItems = Enumerable.Range(0, addRowRowLength).Select(i =>
                                    Enumerable.Range(0, addRowColumnLength).Select(j =>
                                    {
                                        var row = initRowLength + i;
                                        var column = j;
                                        return TestClass.MakeDefaultValueItem(row, column);
                                    }).ToArray()).ToArray();
                                Assert.IsTrue(newItems.Equals<string>(addItems));
                            }
                            else if (args.Direction == Direction.Column)
                            {
                                // 列追加通知
                                Assert.IsTrue(args.NewStartRow == 0);
                                CommonAssertion.ArraySizeEqual(newItems, addColumnColumnLength, addColumnRowLength);

                                var addItems = Enumerable.Range(0, addColumnColumnLength).Select(i =>
                                    Enumerable.Range(0, addColumnRowLength).Select(j =>
                                    {
                                        var row = j;
                                        var column = initColumnLength + i;
                                        return TestClass.MakeDefaultValueItem(row, column);
                                    }).ToArray()).ToArray();
                                Assert.IsTrue(newItems.Equals<string>(addItems));
                            }
                            else
                            {
                                Assert.Fail();
                            }
                        });
                changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)]
                    .ForEach(assertCollectionChangeEventArgsProperty);
                changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)]
                    .ForEach(assertCollectionChangeEventArgsProperty);
            }

            // 発生した Remove イベントのパラメータが正しいこと
            if (isRemovedRow || isRemovedColumn)
            {
                var removeRowRowLength = (type, isEmptyTo) switch
                {
                    (_, true) => initRowLength,
                    (AdjustLengthType.None, _) => initRowLength - rowCount,
                    (AdjustLengthType.IfLong, _) => initRowLength - rowCount,
                    _ => throw new InvalidOperationException()
                };
                var removeRowColumnLength = (type, isEmptyTo) switch
                {
                    (_, true) => initColumnLength,
                    (AdjustLengthType.None, _) => initColumnLength,
                    (AdjustLengthType.IfLong, _) => initColumnLength,
                    _ => throw new InvalidOperationException()
                };
                var removeColumnRowLength = (type, isEmptyTo) switch
                {
                    (_, true) => -1,
                    (AdjustLengthType.None, _) => initRowLength - Math.Max(removeRowRowLength, 0),
                    (AdjustLengthType.IfLong, _) => initRowLength - Math.Max(removeRowRowLength, 0),
                    _ => throw new InvalidOperationException()
                };
                var removeColumnColumnLength = (type, isEmptyTo) switch
                {
                    (_, true) => -1,
                    (AdjustLengthType.None, _) => initColumnLength - columnCount,
                    (AdjustLengthType.IfLong, _) => initColumnLength - columnCount,
                    _ => throw new InvalidOperationException()
                };

                var assertCollectionChangeEventArgsProperty =
                    new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                        args =>
                        {
                            var oldItems = args.OldItems!.ToTwoDimensionalArray();

                            {
                                // 通知によらず共通
                                Assert.IsTrue(args.NewStartRow == -1);
                                Assert.IsTrue(args.NewStartColumn == -1);
                                Assert.IsTrue(args.NewItems == null);
                            }

                            if (args.Direction == Direction.Row)
                            {
                                // 行除去通知
                                Assert.IsTrue(args.OldStartRow == rowCount);
                                Assert.IsTrue(args.OldStartColumn == 0);
                                CommonAssertion.ArraySizeEqual(oldItems, removeRowRowLength, removeRowColumnLength);

                                var addItems = Enumerable.Range(0, removeRowRowLength).Select(i =>
                                    Enumerable.Range(0, removeRowColumnLength).Select(j =>
                                    {
                                        var row = rowCount + i;
                                        var column = j;
                                        return TestClass.MakeDefaultValueItemForList(row, column);
                                    }).ToArray()).ToArray();
                                Assert.IsTrue(oldItems.Equals<string>(addItems));
                            }
                            else if (args.Direction == Direction.Column)
                            {
                                // 列除去通知
                                Assert.IsTrue(args.OldStartRow == 0);
                                Assert.IsTrue(args.OldStartColumn == columnCount);
                                CommonAssertion.ArraySizeEqual(oldItems, removeColumnColumnLength,
                                    removeColumnRowLength);

                                var addItems = Enumerable.Range(0, removeColumnColumnLength).Select(i =>
                                    Enumerable.Range(0, removeColumnRowLength).Select(j =>
                                    {
                                        var row = j;
                                        var column = columnCount + i;
                                        return TestClass.MakeDefaultValueItemForList(row, column);
                                    }).ToArray()).ToArray();
                                Assert.IsTrue(oldItems.Equals<string>(addItems));
                            }
                            else
                            {
                                Assert.Fail();
                            }
                        });
                changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Remove)]
                    .ForEach(assertCollectionChangeEventArgsProperty);
                changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Remove)]
                    .ForEach(assertCollectionChangeEventArgsProperty);
            }

            var afterRowLength = type switch
            {
                AdjustLengthType.None => rowCount,
                AdjustLengthType.IfShort => Math.Max(initRowLength, rowCount),
                AdjustLengthType.IfLong => Math.Min(initRowLength, rowCount),
                _ => throw new InvalidOperationException()
            };
            var afterColumnLength = type switch
            {
                AdjustLengthType.None => columnCount,
                AdjustLengthType.IfShort => Math.Max(initColumnLength, columnCount),
                AdjustLengthType.IfLong => Math.Min(initColumnLength, columnCount),
                _ => throw new InvalidOperationException()
            };

            if (afterRowLength == 0)
            {
                afterRowLength = afterColumnLength = 0;
            }

            // 除去後の行数・列数が正しいこと
            CommonAssertion.SizeEqual(instance, afterRowLength, afterColumnLength);

            // 除去前後の要素関係を取得 -> 取得されるべき文字列に変換
            var outer = Enumerable.Range(0, initRowLength).ToList();
            if (isAddedRow)
            {
                outer.AddRange(Enumerable.Range(initRowLength, rowCount - initRowLength));
            }

            if (isRemovedRow)
            {
                outer.RemoveRange(rowCount, initRowLength - rowCount);
            }

            var inner = Enumerable.Range(0, initColumnLength).ToList();
            if (isAddedColumn)
            {
                inner.AddRange(Enumerable.Range(initColumnLength, columnCount - initColumnLength));
            }

            if (isRemovedColumn)
            {
                inner.RemoveRange(columnCount, initColumnLength - columnCount);
            }

            var makeCompareText = new Func<int, int, string>((r, c) =>
            {
                if (r < initRowLength && c < initColumnLength) return TestClass.MakeDefaultValueItemForList(r, c);
                return TestClass.MakeDefaultValueItem(r, c);
            });

            var movedItems = outer.Select(i =>
                inner.Select(j => makeCompareText(i, j)).ToArray()).ToArray();

            // 処理後の各要素が正しいこと
            Assert.IsTrue(instance.Equals(movedItems));
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, false)]
        [TestCase(0, 0, 5, false)]
        [TestCase(0, 0, 6, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 3, false)]
        [TestCase(3, 0, 5, false)]
        [TestCase(3, 0, 6, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 3, false)]
        [TestCase(3, 5, 5, false)]
        [TestCase(3, 5, 6, true)]
        [TestCase(5, 10, 0, false)]
        [TestCase(5, 10, 5, false)]
        public static void AdjustRowLengthTest(int initRowCount, int initColumnCount,
            int rowLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustRowLength(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthOneDirection(instance, Direction.Row, AdjustLengthType.None,
                rowLength, initColumnCount, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, false)]
        [TestCase(0, 0, 5, false)]
        [TestCase(0, 0, 6, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 3, false)]
        [TestCase(3, 0, 5, false)]
        [TestCase(3, 0, 6, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 3, false)]
        [TestCase(3, 5, 5, false)]
        [TestCase(3, 5, 6, true)]
        [TestCase(5, 10, 0, false)]
        [TestCase(5, 10, 5, false)]
        public static void AdjustRowLengthIfLongTest(int initRowCount, int initColumnCount,
            int rowLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustRowLengthIfLong(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthOneDirection(instance, Direction.Row, AdjustLengthType.IfLong,
                rowLength, initColumnCount, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, false)]
        [TestCase(0, 0, 5, false)]
        [TestCase(0, 0, 6, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 3, false)]
        [TestCase(3, 0, 5, false)]
        [TestCase(3, 0, 6, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 3, false)]
        [TestCase(3, 5, 5, false)]
        [TestCase(3, 5, 6, true)]
        [TestCase(5, 10, 0, false)]
        [TestCase(5, 10, 5, false)]
        public static void AdjustRowLengthIfShortTest(int initRowCount, int initColumnCount,
            int rowLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustRowLengthIfShort(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthOneDirection(instance, Direction.Row, AdjustLengthType.IfShort,
                rowLength, initColumnCount, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, true)]
        [TestCase(0, 0, 10, true)]
        [TestCase(0, 0, 11, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 10, false)]
        [TestCase(3, 0, 11, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 5, false)]
        [TestCase(3, 5, 10, false)]
        [TestCase(3, 5, 11, true)]
        [TestCase(3, 10, 0, false)]
        [TestCase(3, 10, 10, false)]
        public static void AdjustColumnLengthTest(int initRowCount, int initColumnCount,
            int columnLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustColumnLength(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthOneDirection(instance, Direction.Column, AdjustLengthType.None,
                initRowCount, columnLength, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, true)]
        [TestCase(0, 0, 10, true)]
        [TestCase(0, 0, 11, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 10, false)]
        [TestCase(3, 0, 11, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 5, false)]
        [TestCase(3, 5, 10, false)]
        [TestCase(3, 5, 11, true)]
        [TestCase(3, 10, 0, false)]
        [TestCase(3, 10, 10, false)]
        public static void AdjustColumnLengthIfLongTest(int initRowCount, int initColumnCount,
            int columnLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustColumnLengthIfLong(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthOneDirection(instance, Direction.Column, AdjustLengthType.IfLong,
                initRowCount, columnLength, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        [TestCase(0, 0, -1, true)]
        [TestCase(0, 0, 0, true)]
        [TestCase(0, 0, 10, true)]
        [TestCase(0, 0, 11, true)]
        [TestCase(3, 0, -1, true)]
        [TestCase(3, 0, 0, false)]
        [TestCase(3, 0, 10, false)]
        [TestCase(3, 0, 11, true)]
        [TestCase(3, 5, -1, true)]
        [TestCase(3, 5, 0, false)]
        [TestCase(3, 5, 5, false)]
        [TestCase(3, 5, 10, false)]
        [TestCase(3, 5, 11, true)]
        [TestCase(3, 10, 0, false)]
        [TestCase(3, 10, 10, false)]
        public static void AdjustColumnLengthIfShortTest(int initRowCount, int initColumnCount,
            int columnLength, bool isError)
        {
            var instance = TestClass.MakeListFoeMethodTest(initRowCount, initColumnCount,
                out var changingEventArgsList, out var changedEventArgsList,
                out var propertyChangedEventCalledCount);
            var errorOccured = false;

            try
            {
                instance.AdjustColumnLengthIfShort(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            CommonAssertionAdjustLengthOneDirection(instance, Direction.Column, AdjustLengthType.IfShort,
                initRowCount, columnLength, initRowCount, initColumnCount, isError,
                changingEventArgsList, changedEventArgsList, propertyChangedEventCalledCount);
        }

        private static void CommonAssertionAdjustLengthOneDirection(TestClass.IListTest instance, Direction direction,
            AdjustLengthType type, int rowCount, int columnCount,
            int initRowLength, int initColumnLength, bool isError,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changingEventArgsList,
            Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> changedEventArgsList,
            Dictionary<string, int> propertyChangedEventCalledCount)
        {
            var isEmptyTo = rowCount == 0;
            var isEmptyFrom = initRowLength == 0;
            var isAdded = type switch
            {
                AdjustLengthType.None => (isEmptyFrom && !isEmptyTo)
                                         || (direction == Direction.Column
                                             ? initColumnLength < columnCount
                                             : initRowLength < rowCount),
                AdjustLengthType.IfShort => direction == Direction.Column
                    ? initColumnLength < columnCount
                    : initRowLength < rowCount,
                AdjustLengthType.IfLong => false,
                _ => throw new InvalidOperationException()
            };
            var isRemoved = type switch
            {
                AdjustLengthType.None => (!isEmptyFrom && isEmptyTo)
                                         || (direction == Direction.Column
                                             ? initColumnLength > columnCount
                                             : initRowLength > rowCount),
                AdjustLengthType.IfShort => false,
                AdjustLengthType.IfLong => direction == Direction.Column
                    ? initColumnLength > columnCount
                    : initRowLength > rowCount,
                _ => throw new InvalidOperationException()
            };
            var isNotifiedRowCount = new Func<bool>(() =>
            {
                if (direction != Direction.Column) return true;
                return isEmptyFrom || isEmptyTo;
            })();
            var isNotifiedColumnCount = new Func<bool>(() =>
            {
                if (direction != Direction.Row) return true;
                return isEmptyFrom || isEmptyTo;
            })();

            isAdded &= !isError;
            isRemoved &= !isError;
            isNotifiedRowCount &= !isError;
            isNotifiedColumnCount &= !isError;

            // 意図したイベントが発生すること
            var assertCollectionChangeEventArgsList =
                new Action<Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>>(
                    dic =>
                    {
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Replace)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Add)].Count,
                            isAdded ? 1 : 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Move)].Count, 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Remove)].Count,
                            isRemoved ? 1 : 0);
                        Assert.AreEqual(dic[nameof(TwoDimensionalCollectionChangeAction.Reset)].Count, 0);
                    });
            assertCollectionChangeEventArgsList(changingEventArgsList);
            assertCollectionChangeEventArgsList(changedEventArgsList);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.RowCount)],
                isNotifiedRowCount ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[nameof(instance.ColumnCount)],
                isNotifiedColumnCount ? 1 : 0);
            Assert.AreEqual(propertyChangedEventCalledCount[ListConstant.IndexerName], isError ? 0 : 1);

            if (isError) return;

            // 発生した Add イベントのパラメータが正しいこと
            if (isAdded)
            {
                var addLength = direction == Direction.Column
                    ? columnCount - initColumnLength
                    : rowCount - initRowLength;
                var addItemCount = direction == Direction.Column
                    ? initRowLength
                    : initColumnLength;
                var assertCollectionChangeEventArgsProperty =
                    new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                        args =>
                        {
                            Assert.IsTrue(args.Direction == direction);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems == null);
                            Assert.IsTrue(args.NewStartRow == (direction == Direction.Column ? 0 : initRowLength));
                            Assert.IsTrue(args.NewStartColumn ==
                                          (direction == Direction.Column ? initColumnLength : 0));
                            {
                                var newItems = args.NewItems!.ToTwoDimensionalArray();
                                CommonAssertion.ArraySizeEqual(newItems, addLength, addItemCount);

                                var addItems = Enumerable.Range(0, addLength).Select(i =>
                                    Enumerable.Range(0, addItemCount).Select(j =>
                                    {
                                        var row = direction == Direction.Column
                                            ? j
                                            : initRowLength + i;
                                        var column = direction == Direction.Column
                                            ? initColumnLength + i
                                            : j;
                                        return TestClass.MakeDefaultValueItem(row, column);
                                    }).ToArray()).ToArray();

                                Assert.IsTrue(newItems.Equals<string>(addItems));
                            }
                        });
                assertCollectionChangeEventArgsProperty(
                    changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)][0]);
                assertCollectionChangeEventArgsProperty(
                    changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Add)][0]);
            }

            // 発生した Remove イベントのパラメータが正しいこと
            if (isRemoved)
            {
                var removeLength = direction == Direction.Column
                    ? initColumnLength - columnCount
                    : initRowLength - rowCount;
                var removeItemCount = direction == Direction.Column
                    ? initRowLength
                    : initColumnLength;

                var assertCollectionChangeEventArgsProperty =
                    new Action<TwoDimensionalCollectionChangeEventArgs<string>>(
                        args =>
                        {
                            Assert.IsTrue(args.Direction == direction);
                            Assert.IsTrue(args.OldStartRow ==
                                          (direction == Direction.Column
                                              ? 0
                                              : rowCount));
                            Assert.IsTrue(args.OldStartColumn ==
                                          (direction == Direction.Column
                                              ? columnCount
                                              : 0));
                            {
                                var oldItems = args.OldItems!.ToTwoDimensionalArray();

                                CommonAssertion.ArraySizeEqual(oldItems, removeLength, removeItemCount);

                                var removeItems = Enumerable.Range(0, removeLength).Select(i =>
                                    Enumerable.Range(0, removeItemCount).Select(j =>
                                    {
                                        var row = direction == Direction.Column
                                            ? initRowLength - rowCount + j
                                            : rowCount + i;
                                        var column = direction == Direction.Column
                                            ? columnCount + i
                                            : initColumnLength - columnCount + j;
                                        return TestClass.MakeDefaultValueItemForList(row, column);
                                    }).ToArray()).ToArray();

                                Assert.IsTrue(oldItems.Equals<string>(removeItems));
                            }
                            Assert.IsTrue(args.NewStartRow == -1);
                            Assert.IsTrue(args.NewStartColumn == -1);
                            Assert.IsTrue(args.NewItems == null);
                        });
                assertCollectionChangeEventArgsProperty(
                    changingEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Remove)][0]);
                assertCollectionChangeEventArgsProperty(
                    changedEventArgsList[nameof(TwoDimensionalCollectionChangeAction.Remove)][0]);
            }

            var afterRowLength = direction == Direction.Row
                ? type switch
                {
                    AdjustLengthType.None => rowCount,
                    AdjustLengthType.IfShort => Math.Max(initRowLength, rowCount),
                    AdjustLengthType.IfLong => Math.Min(initRowLength, rowCount),
                    _ => throw new InvalidOperationException()
                }
                : initRowLength;
            var afterColumnLength = direction == Direction.Column
                ? type switch
                {
                    AdjustLengthType.None => columnCount,
                    AdjustLengthType.IfShort => Math.Max(initColumnLength, columnCount),
                    AdjustLengthType.IfLong => Math.Min(initColumnLength, columnCount),
                    _ => throw new InvalidOperationException()
                }
                : initColumnLength;

            if (afterRowLength == 0)
            {
                afterRowLength = afterColumnLength = 0;
            }

            // 除去後の行数・列数が正しいこと
            CommonAssertion.SizeEqual(instance, afterRowLength, afterColumnLength);

            // 除去前後の要素関係を取得 -> 取得されるべき文字列に変換
            var outer = Enumerable.Range(0, initRowLength).ToList();
            if (direction != Direction.Column && isRemoved)
            {
                outer.RemoveRange(rowCount, initRowLength - rowCount);
            }

            var inner = Enumerable.Range(0, initColumnLength).ToList();
            if (direction == Direction.Column && isRemoved)
            {
                inner.RemoveRange(columnCount, initColumnLength - columnCount);
            }

            var movedItems = afterRowLength != 0
                ? outer.Select(i =>
                        inner.Select(j => TestClass.MakeDefaultValueItemForList(i, j))
                            .Concat(!isAdded || direction != Direction.Column
                                ? new string[] { }
                                : Enumerable.Range(initColumnLength, columnCount - initColumnLength)
                                    .Select(newJ => TestClass.MakeDefaultValueItem(i, newJ))).ToList())
                    .Concat(!isAdded || direction == Direction.Column
                        ? new List<List<string>>()
                        : Enumerable.Range(initRowLength, rowCount - initRowLength)
                            .Select(newI => inner.Select(j =>
                                    TestClass.MakeDefaultValueItem(newI, j))
                                .ToList()).ToList())
                    .ToList()
                : new List<List<string>>();

            // 処理後の各要素が正しいこと
            Assert.IsTrue(instance.Equals(movedItems));
        }

        private enum AdjustLengthType
        {
            None,
            IfLong,
            IfShort,
        }

        [TestCase(TestClass.TestClassType.Type1)]
        [TestCase(TestClass.TestClassType.Type3)]
        public static void ClearTest(TestClass.TestClassType type)
        {
            string[][] initItems = TestClass.MakeStringList(2, 5);
            TestClass.IListTest instance = type switch
            {
                TestClass.TestClassType.Type1 => new TestClass.ListTest1(initItems),
                TestClass.TestClassType.Type3 => new TestClass.ListTest3(initItems),
                _ => throw new InvalidOperationException()
            };

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception e)
            {
                logger.Exception(e);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 行数・列数が最小であること
            Assert.IsTrue(instance.RowCount == instance.GetMinRowCapacity());
            Assert.IsTrue(instance.ColumnCount == instance.GetMinColumnCapacity());

            // 各要素が意図した値で初期化されていること
            if (instance.GetMinRowCapacity() != 0 && instance.GetMinColumnCapacity() != 0)
            {
                var answer = Enumerable.Range(0, instance.RowCount).Select(i =>
                    Enumerable.Range(0, instance.ColumnCount).Select(j
                        => TestClass.MakeDefaultValueItem(i, j)));
                Assert.IsTrue(instance.Equals(answer));
            }
        }

        [TestCase(4, 9, true)]
        [TestCase(4, 10, true)]
        [TestCase(4, 20, true)]
        [TestCase(4, 21, true)]
        [TestCase(5, 9, true)]
        [TestCase(5, 10, false)]
        [TestCase(5, 20, false)]
        [TestCase(5, 21, true)]
        [TestCase(10, 9, true)]
        [TestCase(10, 10, false)]
        [TestCase(10, 20, false)]
        [TestCase(10, 21, true)]
        [TestCase(11, 9, true)]
        [TestCase(11, 10, true)]
        [TestCase(11, 20, true)]
        [TestCase(11, 21, true)]
        public static void ResetTest(int rowCount, int columnCount, bool isError)
        {
            var resetItems = TestClass.MakeStringList(rowCount, columnCount);
            var instance = new TestClass.ListTest2();

            var errorOccured = false;
            try
            {
                instance.Reset(resetItems);
            }
            catch (Exception e)
            {
                logger.Exception(e);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 行数・列数が初期化要素と一致すること
            Assert.IsTrue(instance.RowCount == rowCount);
            Assert.IsTrue(instance.ColumnCount == columnCount);

            // 各要素が意図した値で初期化されていること
            Assert.IsTrue(instance.Equals(resetItems));
        }

        [TestCase]
        public static void EqualsTest_IRestrictedCapacityTwoDimensionalList()
        {
            /*
             * IRestrictedCapacityTwoDimensionalList<string> 同士の比較
             */

            var initItems = TestClass.MakeStringList(3, 5);
            var target = new TestClass.ListTest1(initItems);

            // 同一インスタンスの比較：一致すること
            {
                var other = target;
                Assert.IsTrue(target.Equals(other));
            }

            // null との比較：一致しないこと
            {
                // nullを引数に取ると Equals(IRestrictedCapacityTwoDimensionalList<T>?) メソッドが実行されるためキャスト不要
                Assert.IsFalse(target.Equals(null));
            }

            // 同一要素別インスタンスとの比較：一致すること
            {
                var other = new TestClass.ListTest1(initItems);
                Assert.IsTrue(target.Equals(other));
            }

            // 一要素が異なるインスタンスとの比較：一致しないこと
            {
                var other = new TestClass.ListTest1(TestClass.MakeStringList(3, 5))
                {
                    [1, 2] = "modify"
                };
                Assert.IsFalse(target.Equals(other));
            }

            // 行数が異なるインスタンスとの比較：一致しないこと
            {
                var other = new TestClass.ListTest1(TestClass.MakeStringList(2, 5));
                Assert.IsFalse(target.Equals(other));
            }

            // 列数が異なるインスタンスとの比較：一致しないこと
            {
                var other = new TestClass.ListTest1(TestClass.MakeStringList(3, 4));
                Assert.IsFalse(target.Equals(other));
            }
        }

        [TestCase]
        public static void EqualsTest_IEnumerable()
        {
            /*
             * IEnumerable<IEnumerable<string>> 同士の比較
             */

            var initItems = TestClass.MakeStringList(3, 5);
            var target = new TestClass.ListTest1(initItems);

            // null との比較：一致しないこと
            {
                // nullを引数に取ると Equals(IRestrictedCapacityTwoDimensionalList<T>?) メソッドが実行されるためキャスト不要
                Assert.IsFalse(target.Equals(null));
            }

            // 同一要素インスタンスとの比較：一致すること
            {
                var other = Enumerable.Range(0, 3).Select(i =>
                    Enumerable.Range(0, 5).Select(j =>
                        TestClass.MakeDefaultValueItemForList(i, j)
                    ));
                Assert.IsTrue(target.Equals(other));
            }

            // 一要素が異なるインスタンスとの比較：一致しないこと
            {
                var other = Enumerable.Range(0, 3).Select(i =>
                    Enumerable.Range(0, 5).Select(j =>
                    {
                        if (i == 1 && j == 2)
                        {
                            return "modify";
                        }

                        return TestClass.MakeDefaultValueItemForList(i, j);
                    }));
                Assert.IsFalse(target.Equals(other));
            }

            // 行数が異なるインスタンスとの比較：一致しないこと
            {
                var other = Enumerable.Range(0, 2).Select(i =>
                    Enumerable.Range(0, 5).Select(j =>
                        TestClass.MakeDefaultValueItemForList(i, j)
                    ));
                Assert.IsFalse(target.Equals(other));
            }

            // 列数が異なるインスタンスとの比較：一致しないこと
            {
                var other = Enumerable.Range(0, 3).Select(i =>
                    Enumerable.Range(0, 4).Select(j =>
                        TestClass.MakeDefaultValueItemForList(i, j)
                    ));
                Assert.IsFalse(target.Equals(other));
            }
        }

        public static class TestClass
        {
            public static string MakeDefaultValueItem(int row, int column)
                => $"({row}, {column})";

            public static string MakeDefaultValueItemForList(int row, int column)
                => $"{MakeDefaultValueItem(row, column)} for List";

            public static string[][] MakeStringList(int rowCount, int columnCount)
            {
                return Enumerable.Range(0, rowCount).Select(rowIdx =>
                        Enumerable.Range(0, columnCount)
                            .Select(colIdx => MakeDefaultValueItemForList(rowIdx, colIdx))
                            .ToArray())
                    .ToArray();
            }

            private static string MakeDefaultValueItemForAddItem(int row, int column)
                => $"({row}, {column}) for AddItem";


            public enum TestClassType
            {
                /// <summary>正常設定リストその１（要素最小数=0 + Changeイベント）</summary>
                Type1,

                /// <summary>正常設定リストその２（要素最小数!=0）</summary>
                Type2,

                /// <summary>正常設定リストその３（要素最小数=要素最大数）</summary>
                Type3,

                /// <summary>異常設定リストその１（最小行数 &lt; 0）</summary>
                Type4,

                /// <summary>異常設定リストその２（最小列数 &lt; 0）</summary>
                Type5,

                /// <summary>異常設定リストその３（最大行数 &lt; 最小行数）</summary>
                Type6,

                /// <summary>異常設定リストその４（最大列数 &lt; 最小列数）</summary>
                Type7,

                /// <summary>異常設定リストその５（defaultValue == null）</summary>
                Type8
            }

            public interface IListTest : IRestrictedCapacityTwoDimensionalList<string>
            {
            }

            #region 正常系

            /// <summary>
            /// 正常設定リストその１（要素最小数=0 + Changeイベント）
            /// </summary>
            /// <remarks>
            /// 行範囲：0 ~ 5<br/>
            /// 列範囲：0 ~ 10<br/>
            /// </remarks>
            [Serializable]
            public class ListTest1 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                public static int RowMaxCapacity => 5;
                public static int RowMinCapacity => 0;
                public static int ColumnMaxCapacity => 10;
                public static int ColumnMinCapacity => 0;

                public static bool IsEventThrowException { get; set; }

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);

                public ListTest1()
                {
                    TwoDimensionListChanging += OnTwoDimensionListChanging;
                }

                public ListTest1(IEnumerable<IEnumerable<string>> list) : base(list)
                {
                }

                protected ListTest1(SerializationInfo info, StreamingContext context) : base(info, context)
                {
                }

                private void OnTwoDimensionListChanging(object sender,
                    TwoDimensionalCollectionChangeEventArgs<string> e)
                {
                    if (IsEventThrowException) throw new Exception();
                }
            }

            /// <summary>
            /// 正常設定リストその２（要素最小数!=0）
            /// </summary>
            /// <remarks>
            /// 行範囲：5 ~ 10<br/>
            /// 列範囲：10 ~ 20<br/>
            /// </remarks>
            [Serializable]
            public class ListTest2 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                public static int RowMaxCapacity => 10;
                public static int RowMinCapacity => 5;
                public static int ColumnMaxCapacity => 20;
                public static int ColumnMinCapacity => 10;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);

                public ListTest2()
                {
                }

                public ListTest2(IEnumerable<IEnumerable<string>> list) : base(list)
                {
                }

                protected ListTest2(SerializationInfo info, StreamingContext context) : base(info, context)
                {
                }
            }

            /// <summary>
            /// 正常設定リストその３（要素最小数＝要素最大数）
            /// </summary>
            /// <remarks>
            /// 行範囲：2<br/>
            /// 列範囲：5<br/>
            /// </remarks>
            [Serializable]
            public class ListTest3 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                public static int RowMaxCapacity => 2;
                public static int RowMinCapacity => 2;
                public static int ColumnMaxCapacity => 5;
                public static int ColumnMinCapacity => 5;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);

                public ListTest3()
                {
                }

                public ListTest3(IEnumerable<IEnumerable<string>> list) : base(list)
                {
                }

                protected ListTest3(SerializationInfo info, StreamingContext context) : base(info, context)
                {
                }
            }

            #endregion

            #region 異常系

            /// <summary>
            /// 異常設定リストその１（最小行数 &lt; 0）
            /// </summary>
            /// <remarks>
            /// 行範囲：-3 ~ 5<br/>
            /// 列範囲：5 ~ 10<br/>
            /// </remarks>
            public class ListTest4 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                private static int RowMaxCapacity => 5;
                private static int RowMinCapacity => -3;
                private static int ColumnMaxCapacity => 10;
                private static int ColumnMinCapacity => 5;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);
            }

            /// <summary>
            /// 異常設定リストその２（最小列数 &lt; 0）
            /// </summary>
            /// <remarks>
            /// 行範囲：0 ~ 5<br/>
            /// 列範囲：-1 ~ 10<br/>
            /// </remarks>
            public class ListTest5 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                private static int RowMaxCapacity => 5;
                private static int RowMinCapacity => 0;
                private static int ColumnMaxCapacity => 10;
                private static int ColumnMinCapacity => -1;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);
            }

            /// <summary>
            /// 異常設定リストその３（最大行数 &lt; 最小行数）
            /// </summary>
            /// <remarks>
            /// 行範囲：7 ~ 6<br/>
            /// 列範囲：5 ~ 10<br/>
            /// </remarks>
            public class ListTest6 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                private static int RowMaxCapacity => 6;
                private static int RowMinCapacity => 7;
                private static int ColumnMaxCapacity => 10;
                private static int ColumnMinCapacity => 5;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);
            }

            /// <summary>
            /// 異常設定リストその４（最大列数 &lt; 最小列数）
            /// </summary>
            /// <remarks>
            /// 行範囲：0 ~ 1<br/>
            /// 列範囲：10 ~ 9<br/>
            /// </remarks>
            public class ListTest7 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                private static int RowMaxCapacity => 0;
                private static int RowMinCapacity => 1;
                private static int ColumnMaxCapacity => 9;
                private static int ColumnMinCapacity => 10;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column)
                    => MakeDefaultValueItem(row, column);
            }

            /// <summary>
            /// 異常設定リストその５（defaultValue == null）
            /// </summary>
            /// <remarks>
            /// 行範囲：0 ~ 5<br/>
            /// 列範囲：0 ~ 10<br/>
            /// </remarks>
            public class ListTest8 : RestrictedCapacityTwoDimensionalList<string>, IListTest
            {
                private static int RowMaxCapacity => 0;
                private static int RowMinCapacity => 1;
                private static int ColumnMaxCapacity => 9;
                private static int ColumnMinCapacity => 10;

                public override int GetMaxRowCapacity() => RowMaxCapacity;

                public override int GetMinRowCapacity() => RowMinCapacity;

                public override int GetMaxColumnCapacity() => ColumnMaxCapacity;

                public override int GetMinColumnCapacity() => ColumnMinCapacity;

                protected override string MakeDefaultItem(int row, int column) => null;
            }

            #endregion

            public static ListTest1 MakeListFoeMethodTest(int initRowLength, int initColumnLength,
                out Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>
                    twoDimensionListChangingEventArgsList,
                out Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>
                    twoDimensionListChangedEventArgsList,
                out Dictionary<string, int> propertyChangedEventCalledCount)
            {
                if (initRowLength == 0 && initColumnLength != 0) Assert.Ignore();

                var initList = MakeStringList(initRowLength, initColumnLength);
                var result = new ListTest1(initList);

                twoDimensionListChangingEventArgsList = MakeTwoDimensionalListChangeEventArgsDic();
                result.TwoDimensionListChanging +=
                    MakeTwoDimensionalListChangeEventArgs(true, twoDimensionListChangingEventArgsList);

                twoDimensionListChangedEventArgsList = MakeTwoDimensionalListChangeEventArgsDic();
                result.TwoDimensionListChanged +=
                    MakeTwoDimensionalListChangeEventArgs(false, twoDimensionListChangedEventArgsList);

                propertyChangedEventCalledCount = MakePropertyChangedArgsDic();
                result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedEventCalledCount);

                return result;
            }

            #region for Event

            private static Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>
                MakeTwoDimensionalListChangeEventArgsDic()
                => new Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>>
                {
                    {
                        nameof(TwoDimensionalCollectionChangeAction.Add),
                        new List<TwoDimensionalCollectionChangeEventArgs<string>>()
                    },
                    {
                        nameof(TwoDimensionalCollectionChangeAction.Replace),
                        new List<TwoDimensionalCollectionChangeEventArgs<string>>()
                    },
                    {
                        nameof(TwoDimensionalCollectionChangeAction.Move),
                        new List<TwoDimensionalCollectionChangeEventArgs<string>>()
                    },
                    {
                        nameof(TwoDimensionalCollectionChangeAction.Remove),
                        new List<TwoDimensionalCollectionChangeEventArgs<string>>()
                    },
                    {
                        nameof(TwoDimensionalCollectionChangeAction.Reset),
                        new List<TwoDimensionalCollectionChangeEventArgs<string>>()
                    },
                };

            private static EventHandler<TwoDimensionalCollectionChangeEventArgs<string>>
                MakeTwoDimensionalListChangeEventArgs(bool isBefore,
                    Dictionary<string, List<TwoDimensionalCollectionChangeEventArgs<string>>> resultDic)
                => (sender, args) =>
                {
                    resultDic[args.Action.ToString()].Add(args);
                    logger.Debug($"TwoDimensionalList{(isBefore ? "Changing" : "Changed")} Event Raise. ");
                    logger.DebugObjectToJson(args);
                };

            private static Dictionary<string, int> MakePropertyChangedArgsDic() => new Dictionary<string, int>
            {
                {nameof(ITwoDimensionalList<string>.RowCount), 0},
                {nameof(ITwoDimensionalList<string>.ColumnCount), 0},
                {ListConstant.IndexerName, 0},
            };

            private static PropertyChangedEventHandler MakePropertyChangedEventHandler(
                Dictionary<string, int> resultDic) =>
                (sender, args) =>
                {
                    resultDic[args.PropertyName] += 1;
                    logger.DebugObjectToJson(args, nameof(args));
                };

            #endregion

            /// <summary>
            /// 引数で与えるリストのnull種別
            /// </summary>
            public class ListType : Commons.TypeSafeEnum<ListType>
            {
                /// <summary>通常（null要素なし、列要素数のばらつきなし）</summary>
                public static ListType Normal;

                /// <summary>リスト自身がnull</summary>
                public static ListType SelfNull;

                /// <summary>n行要素ullあり（行数2以上の場合に有効）</summary>
                public static ListType RowHasNull;

                /// <summary>列要素nullあり（列数2以上の場合に有効）</summary>
                public static ListType ColumnHasNull;

                /// <summary>列要素数ばらつきあり（列数2以上の場合に有効）</summary>
                public static ListType ColumnSizeDifference;

                static ListType()
                {
                    Normal = new ListType(nameof(Normal));
                    SelfNull = new ListType(nameof(SelfNull));
                    RowHasNull = new ListType(nameof(RowHasNull));
                    ColumnHasNull = new ListType(nameof(ColumnHasNull));
                    ColumnSizeDifference = new ListType(nameof(ColumnSizeDifference));
                }

                private ListType(string id) : base(id)
                {
                }

                public static ListType FromId(string id)
                {
                    return AllItems.First(x => x.Id.Equals(id));
                }

                public string[] GetLine(int itemCount)
                {
                    if (this == SelfNull) return null;

                    var funcMakeItem = (Func<int, string>) (i =>
                        this == RowHasNull && i % 2 == 0
                            ? (string) null
                            : MakeDefaultValueItemForAddItem(0, i));

                    return Enumerable.Range(0, itemCount)
                        .Select(funcMakeItem)
                        .ToArray();
                }

                public string[][] GetMultiLine(int rowCount, int columnCount)
                {
                    if (this == SelfNull) return null;

                    var funcMakeItem = (Func<int, int, string>) ((i, j) =>
                        this == ColumnHasNull && j % 2 == 1
                            ? (string) null
                            : MakeDefaultValueItemForAddItem(i, j));
                    var funcMakeRow = (Func<int, int, string[]>) ((i, colCount) =>
                        this == RowHasNull && i % 2 == 1
                            ? null
                            : Enumerable.Range(0, this == ColumnSizeDifference && i == 1
                                    ? colCount - 1
                                    : colCount)
                                .Select(j => funcMakeItem(i, j))
                                .ToArray());

                    return Enumerable.Range(0, rowCount)
                        .Select(i => funcMakeRow(i, columnCount))
                        .ToArray();
                }
            }
        }

        /// <summary>
        /// 共通検証処理
        /// </summary>
        private static class CommonAssertion
        {
            /// <summary>
            /// 二次元配列の行数および列数が正しいことを検証する。
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="rowCount">意図する行数</param>
            /// <param name="columnCount">意図する列数</param>
            public static void ArraySizeEqual(string[][] target, int rowCount, int columnCount)
            {
                Assert.AreEqual(target.Length, rowCount);

                if (rowCount == 0) return;

                target.ForEach(line =>
                    Assert.AreEqual(line.Length, columnCount));
            }

            /// <summary>
            /// 二次元配列の行数および列数が正しいことを検証する。
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="src">比較対象二次元配列</param>
            public static void ArraySizeEqual(string[][] target, string[][] src)
            {
                Assert.AreEqual(target.Length, src.Length);

                if (target.Length == 0) return;

                var columnCount = src[0].Length;

                target.ForEach(line =>
                    Assert.AreEqual(line.Length, columnCount));
            }

            /// <summary>
            /// 二次元リストの行数及び列数が正しいことを検証する。
            /// </summary>
            /// <param name="target">検証対象</param>
            /// <param name="rowCount">意図する行数</param>
            /// <param name="columnCount">意図する列数</param>
            public static void SizeEqual(TestClass.IListTest target, int rowCount, int columnCount)
            {
                Assert.AreEqual(target.RowCount, rowCount);
                Assert.AreEqual(target.ColumnCount, columnCount);
            }
        }
    }
}
