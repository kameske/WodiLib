using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestTools = WodiLib.Test.Sys.TwoDimensionalListTest_Tools;
using TestRecord = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord;
using TestSingleEnumerableInstanceType = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestSingleEnumerableInstanceType;
using TestDoubleEnumerableInstanceType = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestDoubleEnumerableInstanceType;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class TwoDimensionalListOperationResultTest
    {
        /*
         * 操作の引数検証クラスが正しく呼ばれること、
         * 各操作メソッドの実行結果が正しいことをテストする。
         *
         * 各メソッドの引数が正しいことを事前条件とする。
         * 引数が誤っている場合の動作や処理前後の通知の正しさは検証しない。
         * (TwoDimensionalListTest, TwoDimensionalListOperationValidateTest、TwoDimensionalListNotifyTestで確認)
         */

        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region Constructor

        [Test]
        public static void ConstructorTest_Require()
        {
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;
            var initItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                false, funcMakeDefaultItem);
            var validatorMock = new TwoDimensionalListValidatorMock();

            TwoDimensionalList<TestRecord> instance = null;

            try
            {
                instance = new TwoDimensionalList<TestRecord>(initItems, _ => validatorMock, funcMakeDefaultItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckConstructor(initItems, true);

            // 正しく初期化されていること
            AssertElementsConstructor(instance, initItems);
        }

        [Test]
        public static void ConstructorTest_A()
        {
            var validatorMock = new TwoDimensionalListValidatorMock();
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            TwoDimensionalList<TestRecord> instance = null;

            try
            {
                instance = new TwoDimensionalList<TestRecord>(
                    TestTools.InitRowLength, TestTools.InitColumnLength,
                    _ => validatorMock, funcMakeDefaultItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            var expectedInitItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                false, funcMakeDefaultItem);

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckConstructor(expectedInitItems, false);

            // 正しく初期化されていること
            AssertElementsConstructor(instance, expectedInitItems);
        }

        [Test]
        public static void ConstructorTest_B()
        {
            var validatorMock = new TwoDimensionalListValidatorMock();
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            TwoDimensionalList<TestRecord> instance = null;

            try
            {
                instance = new TwoDimensionalList<TestRecord>(_ => validatorMock, funcMakeDefaultItem);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            var expectedInitItems = Array.Empty<TestRecord[]>();

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckConstructor(expectedInitItems, false);

            // 正しく初期化されていること
            AssertElementsConstructor(instance, expectedInitItems);
        }

        private static void AssertElementsConstructor(TwoDimensionalList<TestRecord> instance, TestRecord[][] items)
        {
            var assumedCount = items.Length;
            var assumedItemCount = items.GetInnerArrayLength();
            Assert.IsTrue(instance.Count == assumedCount);
            Assert.IsTrue(instance.ItemCount == assumedItemCount);
            for (var i = 0; i < assumedCount; i++)
            for (var j = 0; j < assumedItemCount; j++)
            {
                Assert.IsTrue(instance[i, j].ItemEquals(items[i][j]),
                    $"instance[{i}, {j}] == items{i}][{j}] ({instance[i, j]}.ItemEquals({items[i][j]}))");
            }
        }

        #endregion

        #region Accessor

        [Test]
        public static void AccessorTest_Line_Get()
        {
            const int index = 1;

            IEnumerable<TestRecord> result = null;

            var instance = MakeInstance(out var validatorMock);

            try
            {
                result = instance[index];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGet(index, 1, 0, TestTools.InitColumnLength, Direction.Row);

            // 取得結果が正しいこと
            var resultArray = result.ToArray();
            AssertElementsGet(instance, index, 1, 0, TestTools.InitColumnLength, Direction.Row, new[] {resultArray});
        }

        [Test]
        public static void AccessorTest_Line_Set()
        {
            const int index = 1;
            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var setItems = TestTools.MakeTestRecordArray(TestSingleEnumerableInstanceType.NotNull_Basic, false,
                funcMakeDefaultItem);

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance[index] = setItems;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSet(index, 0, new[] {setItems}, Direction.Row, true);

            // 編集結果が正しいこと
            AssertElementsSetRow(index, 0, new[] {setItems}, Direction.Row, instance, oldItems);
        }

        [Test]
        public static void AccessorTest_Item_Get()
        {
            const int rowIndex = 1;
            const int columnIndex = 3;

            TestRecord result = null;

            var instance = MakeInstance(out var validatorMock);

            try
            {
                result = instance[rowIndex, columnIndex];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGet(rowIndex, 1, columnIndex, 1, Direction.Row);

            // 取得結果が正しいこと
            {
                var expectedItem = TestTools.MakeListDefaultItem(rowIndex, columnIndex);

                Assert.IsTrue(result.ItemEquals(expectedItem),
                    $"result.ItemEquals(expectedItem) ({result}.ItemEquals({expectedItem}))");
            }
        }

        [Test]
        public static void AccessorTest_Item_Set()
        {
            const int rowIndex = 1;
            const int columnIndex = 3;

            var setItem = TestTools.MakeItem(rowIndex, columnIndex, "Set", "Value");

            var instance = MakeInstance(out var validatorMock);

            try
            {
                instance[rowIndex, columnIndex] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSet(rowIndex, columnIndex, new[] {new[] {setItem}}, Direction.None, false);

            // 取得結果が正しいこと
            Assert.IsTrue(instance[rowIndex, columnIndex].ItemEquals(setItem),
                $"instance[{rowIndex}, {columnIndex}].ItemEquals(expectedItem) ({instance[rowIndex, columnIndex]}.ItemEquals({setItem}))");
        }

        #endregion

        #region CopyTo

        private static readonly object[] CopyToTest_ToArrayWithDirectionTestCaseSource =
        {
            new object[] {Direction.Row},
            new object[] {Direction.Column},
        };

        [TestCaseSource(nameof(CopyToTest_ToArrayWithDirectionTestCaseSource))]
        public static void CopyToTest_ToArrayWithDirection(Direction direction)
        {
            const int index = 1;
            const int beforeArrayLength = TestTools.InitRowLength * TestTools.InitColumnLength + index + 4;

            var array = new TestRecord[beforeArrayLength];
            var beforeArrayItems = array.ToArray();

            var instance = MakeInstance(out var validatorMock);

            try
            {
                instance.CopyTo(array, index, direction);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckCopyTo(array, index, direction);

            // 処理結果が正しいこと
            Assert.AreEqual(beforeArrayLength, array.Length);

            // コピー先の要素が正しいこと
            var offset = 0;
            // コピー先範囲より前：変化していないこと
            for (; offset < index; offset++)
            {
                Assert.AreEqual(beforeArrayItems[offset], array[offset]);
            }

            // コピー先範囲
            if (direction == Direction.Row)
            {
                for (var r = 0; r < instance.Count; r++)
                for (var c = 0; c < instance.ItemCount; c++)
                {
                    Assert.AreEqual(instance[r, c], array[offset]);
                    offset++;
                }
            }
            else // direction == Direction.Column
            {
                for (var c = 0; c < instance.ItemCount; c++)
                for (var r = 0; r < instance.Count; r++)
                {
                    Assert.AreEqual(instance[r, c], array[offset]);
                    offset++;
                }
            }

            // コピー先範囲より後：変化していないこと
            for (; offset < array.Length; offset++)
            {
                Assert.AreEqual(beforeArrayItems[offset], array[offset]);
            }
        }

        [Test]
        public static void CopyToTest_ToArray_RectangularArray()
        {
            const int row = 1;
            const int column = 2;
            const int arrayRowLength = TestTools.InitRowLength + row + 2;
            const int arrayColumnLength = TestTools.InitColumnLength + column + 3;

            Func<int, int, TestRecord> makeOriginalItem =
                (rowIdx, columnIdx) => TestTools.MakeItem(rowIdx, columnIdx, "copyTo", "originalItem");

            var array = new TestRecord[arrayRowLength, arrayColumnLength];
            for (var r = 0; r < arrayRowLength; r++)
            for (var c = 0; c < arrayRowLength; c++)
            {
                array[r, c] = makeOriginalItem(r, c);
            }

            var beforeArrayItems = new TestRecord[arrayRowLength, arrayColumnLength];
            for (var r = 0; r < arrayRowLength; r++)
            for (var c = 0; c < arrayRowLength; c++)
            {
                beforeArrayItems[r, c] = array[r, c];
            }

            var instance = MakeInstance(out var validatorMock);

            try
            {
                instance.CopyTo(array, row, column);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckCopyTo(array, row, column);

            // 処理結果が正しいこと

            // コピー先の要素が正しいこと
            AssertElementsCopyToTwoDimensionalArray(instance, row, column,
                (r, c) => array[r, c], (r, c) => beforeArrayItems[r, c],
                arrayRowLength, arrayColumnLength);
        }

        [Test]
        public static void CopyToTest_ToArray_JaggedArray()
        {
            const int row = 1;
            const int column = 2;

            var array = TestTools.MakeDoubleJaggedArray(false);

            var beforeArrayItems = array.ToTwoDimensionalArray();

            var instance = MakeInstance(out var validatorMock);

            try
            {
                instance.CopyTo(array, row, column);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckCopyTo(array, row, column);

            // 処理結果が正しいこと

            // コピー先の要素が正しいこと
            AssertElementsCopyToTwoDimensionalArray(instance, row, column,
                (r, c) => array[r][c], (r, c) => beforeArrayItems[r][c],
                array.Length, array.GetInnerArrayLength());
        }

        private static void AssertElementsCopyToTwoDimensionalArray(TwoDimensionalList<TestRecord> instance,
            int row, int column, Func<int, int, TestRecord> funcGetArrayItem,
            Func<int, int, TestRecord> funcGetBeforeArrayItem, int arrayRowLength, int arrayColumnLength)
        {
            var rOffset = 0;
            // コピー先範囲より前の行：変化していないこと
            for (; rOffset < row; rOffset++)
            for (var c = 0; c < arrayColumnLength; c++)
            {
                Assert.AreEqual(funcGetBeforeArrayItem(rOffset, c), funcGetArrayItem(rOffset, c));
            }

            // コピー先範囲行
            for (var r = 0; r < instance.Count; r++)
            {
                var cOffset = 0;
                // コピー先範囲より前の列：変化していないこと
                for (; cOffset < column; cOffset++)
                {
                    Assert.AreEqual(funcGetBeforeArrayItem(rOffset, cOffset), funcGetArrayItem(rOffset, cOffset));
                }

                // コピー先範囲
                for (var c = 0; c < instance.ItemCount; c++)
                {
                    Assert.AreEqual(instance[r, c], funcGetArrayItem(rOffset, cOffset));
                    cOffset++;
                }

                // コピー先範囲より後の列：変化していないこと
                for (; cOffset < arrayColumnLength; cOffset++)
                {
                    Assert.AreEqual(funcGetBeforeArrayItem(rOffset, cOffset), funcGetArrayItem(rOffset, cOffset));
                }

                rOffset++;
            }

            // コピー先範囲より後の行：変化していないこと
            for (; rOffset < arrayRowLength; rOffset++)
            for (var c = 0; c < arrayColumnLength; c++)
            {
                Assert.AreEqual(funcGetBeforeArrayItem(rOffset, c), funcGetArrayItem(rOffset, c));
            }
        }

        #endregion

        #region Get

        [Test]
        public static void GetRange_Row_Test()
        {
            const int index = 1;
            const int count = 3;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<IEnumerable<TestRecord>> result = null;

            try
            {
                result = instance.GetRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGet(index, count, 0, TestTools.InitColumnLength, Direction.Row);

            // 取得結果が正しいこと
            var resultArray = result.ToTwoDimensionalArray();
            AssertElementsGet(instance, index, count, 0, TestTools.InitColumnLength, Direction.Row, resultArray);
        }

        [Test]
        public static void GetItem_Test()
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<TestRecord> result = null;

            try
            {
                result = instance.GetItem(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGet(0, TestTools.InitRowLength, index, 1, Direction.Column);

            // 取得結果が正しいこと
            var resultArray = result.ToArray();
            AssertElementsGet(instance, 0, TestTools.InitRowLength, index, 1, Direction.Column, new[] {resultArray});
        }

        [Test]
        public static void GetItemRange_Test()
        {
            const int index = 1;
            const int count = 2;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<IEnumerable<TestRecord>> result = null;

            try
            {
                result = instance.GetItemRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGet(0, TestTools.InitRowLength, index, count, Direction.Column);

            // 取得結果が正しいこと
            var resultArray = result.ToTwoDimensionalArray();
            AssertElementsGet(instance, 0, TestTools.InitRowLength, index, count, Direction.Column, resultArray);
        }

        private static readonly object[] GetRange_Direction_TestCaseSource =
        {
            new object[] {Direction.Row},
            new object[] {Direction.Column},
        };

        [TestCaseSource(nameof(GetRange_Direction_TestCaseSource))]
        public static void GetRange_Direction_Test(Direction direction)
        {
            const int row = 1;
            const int count = 3;
            const int column = 2;
            const int itemCount = 4;

            var instance = MakeInstance(out var validatorMock);

            IEnumerable<IEnumerable<TestRecord>> result = null;

            try
            {
                result = instance.GetRange(row, count, column, itemCount, direction);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckGet(row, count, column, itemCount, direction);

            // 取得結果が正しいこと
            var resultArray = result.ToTwoDimensionalArray();
            AssertElementsGet(instance, row, count, column, itemCount, direction, resultArray);
        }

        private static void AssertElementsGet(TwoDimensionalList<TestRecord> instance,
            int row, int count, int column, int itemCount, Direction direction,
            TestRecord[][] resultArray)
        {
            if (direction == Direction.Row)
            {
                Assert.AreEqual(count, resultArray.Length);
                Assert.AreEqual(itemCount, resultArray.GetInnerArrayLength());
                for (var rOffset = 0; rOffset < count; rOffset++)
                for (var cOffset = 0; cOffset < itemCount; cOffset++)
                {
                    Assert.IsTrue(instance[rOffset + row, cOffset + column].ItemEquals(resultArray[rOffset][cOffset]));
                }
            }
            else // direction == Direction.Column
            {
                Assert.AreEqual(itemCount, resultArray.Length);
                Assert.AreEqual(count, resultArray.GetInnerArrayLength());
                for (var cOffset = 0; cOffset < itemCount; cOffset++)
                for (var rOffset = 0; rOffset < count; rOffset++)
                {
                    Assert.IsTrue(instance[rOffset + row, cOffset + column].ItemEquals(resultArray[cOffset][rOffset]));
                }
            }
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = MakeInstance(out var validatorMock);

            try
            {
                _ = instance.GetEnumerator();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が呼ばれていないこと
            validatorMock.CheckNotWork();

            // foreach で結果が正しく取得されること
            var r = 0;
            foreach (var line in instance)
            {
                var lineArray = line.ToArray();
                Assert.AreEqual(TestTools.InitColumnLength, lineArray.Length);
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(lineArray[c]));
                }

                r++;
            }
        }

        #endregion

        #region Set

        [Test]
        public static void SetRangeTest()
        {
            const int row = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            var setItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.SetRange(row, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSet(row, 0, setItems, Direction.Row, true);

            // 編集結果が正しいこと
            AssertElementsSetRow(row, 0, setItems, Direction.Row, instance, oldItems);
        }

        [Test]
        public static void SetItemTest()
        {
            const int index = 3;

            var setItems = TestTools.MakeTestRecordArray(TestSingleEnumerableInstanceType.NotNull_Basic,
                true, TestTools.MakeInitItem);

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.SetItem(index, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSet(0, index, new[] {setItems}, Direction.Column, true);

            // 編集結果が正しいこと
            AssertElementsSetRow(0, index, new[] {setItems}, Direction.Column, instance, oldItems);
        }

        [Test]
        public static void SetItemRangeTest()
        {
            const int index = 1;

            var setItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.SetItemRange(index, setItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSet(0, index, setItems, Direction.Column, true);

            // 編集結果が正しいこと
            AssertElementsSetRow(0, index, setItems, Direction.Column, instance, oldItems);
        }

        private static readonly object[] SetRange_Wide_TestCaseSource =
        {
            new object[] {Direction.Row},
            new object[] {Direction.Column},
        };

        [TestCaseSource(nameof(SetRange_Wide_TestCaseSource))]
        public static void SetRange_Wide_Test(Direction direction)
        {
            const int setItemRowLength = 2;
            const int setItemColumnLength = 1;

            const int row = TestTools.InitRowLength - setItemRowLength;
            const int column = TestTools.InitColumnLength - setItemColumnLength;

            var setItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne,
                direction == Direction.Column, TestTools.MakeInitItem);

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.SetRange(row, column, setItems, direction);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckSet(row, column, setItems, direction, false);

            // 編集結果が正しいこと
            AssertElementsSetRow(row, column, setItems, direction, instance, oldItems);
        }

        private static void AssertElementsSetRow(
            int row, int column, TestRecord[][] setItems, Direction setDirection,
            TwoDimensionalList<TestRecord> instance, TestRecord[][] oldItems)
        {
            Assert.AreEqual(TestTools.InitRowLength, instance.Count);
            Assert.AreEqual(TestTools.InitColumnLength, instance.ItemCount);

            var setItemRowLength =
                setDirection == Direction.Row
                    ? setItems.Length
                    : setItems.GetInnerArrayLength();
            var setItemColumnLength =
                setDirection == Direction.Row
                    ? setItems.GetInnerArrayLength()
                    : setItems.Length;

            var r = 0;
            // 編集行より前：要素が変化していないこと
            for (; r < row; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 編集行：編集した列が編集した内容に変化していること
            for (var rOffset = 0; rOffset < setItemRowLength; rOffset++)
            {
                // 編集列より前：要素が変化していないこと
                var c = 0;
                for (; c < column; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                for (var cOffset = 0; cOffset < setItemColumnLength; cOffset++)
                {
                    // 編集列：編集した内容に変化していること
                    if (setDirection == Direction.Row)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(setItems[rOffset][cOffset]),
                            $"instance[{r}, {c}].ItemEquals(setItems[{rOffset}][{cOffset}]) ({instance[r, c]}.ItemEquals({setItems[rOffset][cOffset]}))");
                    }
                    else
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(setItems[cOffset][rOffset]),
                            $"instance[{r}, {c}].ItemEquals(setItems[{rOffset}][{cOffset}]) ({instance[r, c]}.ItemEquals({setItems[cOffset][rOffset]}))");
                    }

                    c++;
                }

                // 編集列より後：要素が変化していないこと
                for (; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                r++;
            }

            // 編集行より後：要素が変化していないこと
            for (; r < TestTools.InitRowLength; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }
        }

        #endregion

        #region Add

        [Test]
        public static void AddTest()
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArray(TestSingleEnumerableInstanceType.NotNull_Basic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.Add(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(TestTools.InitRowLength, new[] {addItems}, Direction.Row);

            // 挿入結果が正しいこと
            AssertElementsAddRow(instance, oldItems, new[] {addItems});
        }

        [Test]
        public static void AddRangeTest()
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.AddRange(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(TestTools.InitRowLength, addItems, Direction.Row);

            // 挿入結果が正しいこと
            AssertElementsAddRow(instance, oldItems, addItems);
        }

        private static void AssertElementsAddRow(TwoDimensionalList<TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addRowLength = addItems.Length;

            Assert.IsTrue(instance.Count == TestTools.InitRowLength + addRowLength);
            Assert.IsTrue(instance.ItemCount == TestTools.InitColumnLength);

            var r = 0;

            // 追加行より前：値が変化していないこと
            for (; r < oldItems.Length; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 追加行：追加した値と一致すること
            var rOffset = 0;
            for (; rOffset < addRowLength; rOffset++)
            {
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[rOffset][c]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{rOffset}][{c}]) ({instance[r, c]}.ItemEquals({addItems[rOffset][c]}))");
                }

                r++;
            }
        }

        [Test]
        public static void AddItemTest()
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArray(TestSingleEnumerableInstanceType.NotNull_Basic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.AddItem(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(TestTools.InitColumnLength, new[] {addItems}, Direction.Column);

            // 挿入結果が正しいこと
            AssertElementsAddColumn(instance, oldItems, new[] {addItems});
        }

        [Test]
        public static void AddItemRangeTest()
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            try
            {
                instance.AddItemRange(addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(TestTools.InitColumnLength, addItems, Direction.Column);

            // 挿入結果が正しいこと
            AssertElementsAddColumn(instance, oldItems, addItems);
        }

        private static void AssertElementsAddColumn(TwoDimensionalList<TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addColumnLength = addItems.Length;

            Assert.IsTrue(instance.Count == TestTools.InitRowLength);
            Assert.IsTrue(instance.ItemCount == TestTools.InitColumnLength + addColumnLength);

            for (var r = 0; r < TestTools.InitRowLength; r++)
            {
                var c = 0;
                // 追加列より前：値が変化していないこと
                for (; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 追加行：追加した値と一致すること
                var cOffset = 0;
                for (; cOffset < addColumnLength; cOffset++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[cOffset][r]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{cOffset}][{r}]) ({instance[r, c]}.ItemEquals({addItems[cOffset][r]}))");
                    c++;
                }
            }
        }

        #endregion

        #region Insert

        [Test]
        public static void InsertTest()
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArray(TestSingleEnumerableInstanceType.NotNull_Basic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.Insert(index, addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(index, new[] {addItems}, Direction.Row);

            // 挿入結果が正しいこと
            AssertElementsInsertRow(index, instance, oldItems, new[] {addItems});
        }

        [Test]
        public static void InsertRangeTest()
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            try
            {
                instance.InsertRange(index, addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(index, addItems, Direction.Row);

            // 挿入結果が正しいこと
            AssertElementsInsertRow(index, instance, oldItems, addItems);
        }

        private static void AssertElementsInsertRow(int index, TwoDimensionalList<TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addRowLength = addItems.Length;

            Assert.IsTrue(instance.Count == TestTools.InitRowLength + addRowLength);
            Assert.IsTrue(instance.ItemCount == TestTools.InitColumnLength);

            var r = 0;

            // 挿入行より前：値が変化していないこと
            for (; r < index; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 挿入行：挿入した値と一致すること
            var rOffset = 0;
            for (; rOffset < addRowLength; rOffset++)
            {
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[rOffset][c]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{rOffset}][{c}]) ({instance[r, c]}.ItemEquals({addItems[rOffset][c]}))");
                }

                r++;
            }

            // 挿入列より後：値が変化していないこと
            for (; r < TestTools.InitRowLength + addRowLength; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r - rOffset][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r - rOffset][c]}))");
            }
        }

        [Test]
        public static void InsertItemTest()
        {
            const int index = 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArray(TestSingleEnumerableInstanceType.NotNull_Basic,
                true, TestTools.MakeInitItem);

            try
            {
                instance.InsertItem(index, addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(index, new[] {addItems}, Direction.Column);

            // 挿入結果が正しいこと
            AssertElementsInsertColumn(index, instance, oldItems, new[] {addItems});
        }

        [Test]
        public static void InsertItemRangeTest()
        {
            const int index = 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var addItems = TestTools.MakeTestRecordArrays(TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            try
            {
                instance.InsertItemRange(index, addItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckInsert(index, addItems, Direction.Column);

            // 挿入結果が正しいこと
            AssertElementsInsertColumn(index, instance, oldItems, addItems);
        }

        private static void AssertElementsInsertColumn(int index, TwoDimensionalList<TestRecord> instance,
            TestRecord[][] oldItems, TestRecord[][] addItems)
        {
            var addColumnLength = addItems.Length;

            Assert.IsTrue(instance.Count == TestTools.InitRowLength);
            Assert.IsTrue(instance.ItemCount == TestTools.InitColumnLength + addColumnLength);

            for (var r = 0; r < TestTools.InitRowLength; r++)
            {
                var c = 0;
                // 挿入列より前：値が変化していないこと
                for (; c < index; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 挿入行：挿入した値と一致すること
                var cOffset = 0;
                for (; cOffset < addColumnLength; cOffset++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(addItems[cOffset][r]),
                        $"instance[{r}, {c}].ItemEquals(addItems[{cOffset}][{r}]) ({instance[r, c]}.ItemEquals({addItems[cOffset][r]}))");
                    c++;
                }

                // 挿入列より後：値が変化していないこと
                for (; c < TestTools.InitColumnLength + addColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c - cOffset]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c - cOffset]}))");
                }
            }
        }

        #endregion

        #region Overwrite

        private const string OverwriteTestCase_ReplaceOnly = "ReplaceOnly";
        private const string OverwriteTestCase_ReplaceAdd = "ReplaceAdd";
        private const string OverwriteTestCase_AddOnly = "AddOnly";

        [TestCase(OverwriteTestCase_ReplaceOnly)]
        [TestCase(OverwriteTestCase_ReplaceAdd)]
        [TestCase(OverwriteTestCase_AddOnly)]
        public static void OverwriteTest(string testType)
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var overwriteItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                false, TestTools.MakeInitItem);

            var addRowLength = overwriteItems.Length;

            var index = testType switch
            {
                OverwriteTestCase_ReplaceOnly => TestTools.InitRowLength - addRowLength,
                OverwriteTestCase_ReplaceAdd => TestTools.InitRowLength - addRowLength / 2,
                OverwriteTestCase_AddOnly => TestTools.InitRowLength,
                _ => throw new Exception()
            };

            var answerAfterRowLength =
                TestTools.InitRowLength + Math.Max(index - TestTools.InitRowLength + addRowLength, 0);

            try
            {
                instance.Overwrite(index, overwriteItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckOverwrite(index, overwriteItems, Direction.Row);

            // 上書き結果が正しいこと
            {
                Assert.AreEqual(instance.Count, answerAfterRowLength);
                Assert.AreEqual(instance.ItemCount, TestTools.InitColumnLength);

                var r = 0;

                // 上書き行より前：値が変化していないこと
                for (; r < index; r++)
                for (var c = 0; c < TestTools.InitColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 上書き行：上書きした値と一致すること
                for (var rOffset = 0; rOffset < addRowLength; rOffset++)
                {
                    for (var c = 0; c < TestTools.InitColumnLength; c++)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(overwriteItems[rOffset][c]),
                            $"instance[{r}, {c}].ItemEquals(overwriteItems[{rOffset}][{c}]) ({instance[r, c]}.ItemEquals({overwriteItems[rOffset][c]}))");
                    }

                    r++;
                }
            }
        }

        [TestCase(OverwriteTestCase_ReplaceOnly)]
        [TestCase(OverwriteTestCase_ReplaceAdd)]
        [TestCase(OverwriteTestCase_AddOnly)]
        public static void OverwriteItemTest(string testType)
        {
            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var overwriteItems = TestTools.MakeTestRecordArrays(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                true, TestTools.MakeInitItem);

            var addColumnLength = overwriteItems.Length;

            var index = testType switch
            {
                OverwriteTestCase_ReplaceOnly => TestTools.InitColumnLength - addColumnLength,
                OverwriteTestCase_ReplaceAdd => TestTools.InitColumnLength - addColumnLength / 2,
                OverwriteTestCase_AddOnly => TestTools.InitColumnLength,
                _ => throw new Exception()
            };

            var answerAfterColumnLength =
                TestTools.InitColumnLength + Math.Max(index - TestTools.InitColumnLength + addColumnLength, 0);

            try
            {
                instance.OverwriteItem(index, overwriteItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckOverwrite(index, overwriteItems, Direction.Column);

            // 上書き結果が正しいこと
            {
                Assert.IsTrue(instance.Count == TestTools.InitRowLength);
                Assert.IsTrue(instance.ItemCount == answerAfterColumnLength);

                for (var r = 0; r < TestTools.InitRowLength; r++)
                {
                    var c = 0;

                    // 上書き列より前：値が変化していないこと
                    for (; c < index; c++)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                            $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                    }

                    // 上書き列：上書きした値と一致すること
                    for (var cOffset = 0; cOffset < addColumnLength; cOffset++)
                    {
                        Assert.IsTrue(instance[r, c].ItemEquals(overwriteItems[cOffset][r]),
                            $"instance[{r}, {c}].ItemEquals(overwriteItems[{cOffset}][{r}]) ({instance[r, c]}.ItemEquals({overwriteItems[cOffset][r]}))");
                        c++;
                    }
                }
            }
        }

        #endregion

        #region Move

        private const string MoveTestCase_OldGreaterThanNew = "OldIndex > NewIndex";
        private const string MoveTestCase_OldEqualsNew = "OldIndex == NewIndex";
        private const string MoveTestCase_OldLessThanNew = "OldIndex < NewIndex";

        #region Row

        [TestCase(MoveTestCase_OldGreaterThanNew)]
        [TestCase(MoveTestCase_OldEqualsNew)]
        [TestCase(MoveTestCase_OldLessThanNew)]
        public static void MoveTest(string testType)
        {
            var (oldIndex, newIndex) = testType switch
            {
                MoveTestCase_OldGreaterThanNew => (TestTools.InitRowLength - 1, 1),
                MoveTestCase_OldEqualsNew => (TestTools.InitRowLength / 2, TestTools.InitRowLength / 2),
                MoveTestCase_OldLessThanNew => (1, TestTools.InitRowLength - 1),
                _ => throw new Exception(),
            };

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.Move(oldIndex, newIndex);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckMove(oldIndex, newIndex, 1, Direction.Row);

            // 移動結果が正しいこと
            AssertElementsMove(instance, oldIndex, newIndex, 1, Direction.Row, oldItems);
        }

        [TestCase(MoveTestCase_OldGreaterThanNew)]
        [TestCase(MoveTestCase_OldEqualsNew)]
        [TestCase(MoveTestCase_OldLessThanNew)]
        public static void MoveRangeTest(string testType)
        {
            var (oldIndex, newIndex) = testType switch
            {
                MoveTestCase_OldGreaterThanNew => (TestTools.InitRowLength / 2 - 1, 0),
                MoveTestCase_OldEqualsNew => (TestTools.InitRowLength / 2 - 1, TestTools.InitRowLength / 2 - 1),
                MoveTestCase_OldLessThanNew => (0, TestTools.InitRowLength / 2 - 1),
                _ => throw new Exception(),
            };
            const int count = TestTools.InitRowLength / 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.MoveRange(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckMove(oldIndex, newIndex, count, Direction.Row);

            // 移動結果が正しいこと
            AssertElementsMove(instance, oldIndex, newIndex, count, Direction.Row, oldItems);
        }

        #endregion

        #region Column

        [TestCase(MoveTestCase_OldGreaterThanNew)]
        [TestCase(MoveTestCase_OldEqualsNew)]
        [TestCase(MoveTestCase_OldLessThanNew)]
        public static void MoveItemTest(string testType)
        {
            var (oldIndex, newIndex) = testType switch
            {
                MoveTestCase_OldGreaterThanNew => (TestTools.InitRowLength - 1, 1),
                MoveTestCase_OldEqualsNew => (TestTools.InitRowLength / 2, TestTools.InitRowLength / 2),
                MoveTestCase_OldLessThanNew => (1, TestTools.InitRowLength - 1),
                _ => throw new Exception(),
            };

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.MoveItem(oldIndex, newIndex);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckMove(oldIndex, newIndex, 1, Direction.Column);

            // 移動結果が正しいこと
            AssertElementsMove(instance, oldIndex, newIndex, 1, Direction.Column, oldItems);
        }

        [TestCase(MoveTestCase_OldGreaterThanNew)]
        [TestCase(MoveTestCase_OldEqualsNew)]
        [TestCase(MoveTestCase_OldLessThanNew)]
        public static void MoveItemRangeTest(string testType)
        {
            var (oldIndex, newIndex) = testType switch
            {
                MoveTestCase_OldGreaterThanNew => (TestTools.InitRowLength - 1, 1),
                MoveTestCase_OldEqualsNew => (TestTools.InitRowLength / 2, TestTools.InitRowLength / 2),
                MoveTestCase_OldLessThanNew => (1, TestTools.InitRowLength - 1),
                _ => throw new Exception(),
            };
            const int count = TestTools.InitRowLength / 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.MoveItemRange(oldIndex, newIndex, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckMove(oldIndex, newIndex, count, Direction.Column);

            // 移動結果が正しいこと
            AssertElementsMove(instance, oldIndex, newIndex, count, Direction.Column, oldItems);
        }

        #endregion

        private static void AssertElementsMove(TwoDimensionalList<TestRecord> instance,
            int oldIndex, int newIndex, int count, Direction direction, TestRecord[][] oldItems)
        {
            Assert.IsTrue(instance.Count == TestTools.InitRowLength);
            Assert.IsTrue(instance.ItemCount == TestTools.InitColumnLength);

            // 移動前後のインデックスMap
            var outer = Enumerable.Range(0, TestTools.InitRowLength).ToList();
            if (direction == Direction.Row)
            {
                var moveItems = outer.GetRange(oldIndex, count);
                outer.RemoveRange(oldIndex, count);
                outer.InsertRange(newIndex, moveItems);
            }

            var inner = Enumerable.Range(0, TestTools.InitColumnLength).ToList();
            if (direction == Direction.Column)
            {
                var moveItems = inner.GetRange(oldIndex, count);
                inner.RemoveRange(oldIndex, count);
                inner.InsertRange(newIndex, moveItems);
            }

            outer.ForEach((beforeRow, afterRow) =>
                inner.ForEach((beforeColumn, afterColumn) =>
                {
                    Assert.IsTrue(instance[afterRow, afterColumn].ItemEquals(oldItems[beforeRow][beforeColumn]),
                        $"instance[{afterRow}, {afterColumn}].ItemEquals(oldItems[{beforeRow}][{beforeColumn}]) " +
                        $"({instance[afterRow, afterColumn]}.ItemEquals({oldItems[beforeRow][beforeColumn]}))");
                })
            );
        }

        #endregion

        #region Remove

        #region Row

        [Test]
        public static void RemoveAtTest()
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.RemoveAt(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemove(index, 1, Direction.Row);

            // 移動結果が正しいこと
            AssertElementsRemoveRow(instance, index, 1, oldItems);
        }

        [Test]
        public static void RemoveTest()
        {
            const int itemRow = 1;

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();
            var removeItems = TestTools.MakeTestRecords(TestSingleEnumerableInstanceType.NotNull_Basic, false,
                (_, c) => funcMakeDefaultItem(itemRow, c));
            var comparer = EqualityComparerFactory.Create<TestRecord>();

            try
            {
                instance.Remove(removeItems, comparer);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemove(itemRow, 1, Direction.Row);

            // 除去結果が正しいこと
            AssertElementsRemoveRow(instance, itemRow, 1, oldItems);
        }

        [TestCase(0)]
        [TestCase(2)]
        public static void RemoveRangeTest(int removeCount)
        {
            const int index = 2;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.RemoveRange(index, removeCount);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemove(index, removeCount, Direction.Row);

            // 除去結果が正しいこと
            AssertElementsRemoveRow(instance, index, removeCount, oldItems);
        }

        private static void AssertElementsRemoveRow(TwoDimensionalList<TestRecord> instance,
            int index, int count, TestRecord[][] oldItems)
        {
            var answerRowLength = TestTools.InitRowLength - count;

            Assert.IsTrue(instance.Count == answerRowLength);
            Assert.IsTrue(instance.ItemCount == TestTools.InitColumnLength);

            // 初期要素（削除位置より前）が変化していないこと
            var r = 0;
            for (; r < index; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
            }

            // 初期要素（削除位置より後）が変化していないこと
            for (; r < answerRowLength; r++)
            for (var c = 0; c < TestTools.InitColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r + count][c]),
                    $"instance[{r}, {c}].ItemEquals(oldItems[{r + count}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r + count][c]}))");
            }
        }

        #endregion

        #region Column

        [Test]
        public static void RemoveItemTest()
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.RemoveItem(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemove(index, 1, Direction.Column);

            // 移動結果が正しいこと
            AssertElementsRemoveColumn(instance, index, 1, oldItems);
        }

        [TestCase(0)]
        [TestCase(2)]
        public static void RemoveItemRangeTest(int removeCount)
        {
            const int index = 1;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.RemoveItemRange(index, removeCount);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckRemove(index, removeCount, Direction.Column);

            // 移動結果が正しいこと
            AssertElementsRemoveColumn(instance, index, removeCount, oldItems);
        }

        private static void AssertElementsRemoveColumn(TwoDimensionalList<TestRecord> instance,
            int index, int count, TestRecord[][] oldItems)
        {
            var answerColumnLength = TestTools.InitColumnLength - count;

            Assert.IsTrue(instance.Count == TestTools.InitRowLength);
            Assert.IsTrue(instance.ItemCount == answerColumnLength);

            for (var r = 0; r < TestTools.InitRowLength; r++)
            {
                var c = 0;

                // 初期要素（削除位置より前）が変化していないこと
                for (; c < index; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                // 初期要素（削除位置より後）が変化していないこと
                for (; c < answerColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c + count]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c + count}]) ({instance[r, c]}.ItemEquals({oldItems[r][c + count]}))");
                }
            }
        }

        #endregion

        #endregion

        #region AdjustLength

        private const string AdjustLengthTestCase_LongLength = "LongLength";
        private const string AdjustLengthTestCase_SameLength = "SameLength";
        private const string AdjustLengthTestCase_ShortLength = "ShortLength";

        #region Both

        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_ShortLength)]
        public static void AdjustLengthTest(string rowLengthType, string columnLengthType)
        {
            var rowLength = rowLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };
            var columnLength = columnLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLength(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, rowLength, columnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_ShortLength)]
        public static void AdjustLengthIfShortTest(string rowLengthType, string columnLengthType)
        {
            var rowLength = rowLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };
            var columnLength = columnLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLengthIfShort(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Max(TestTools.InitRowLength, rowLength),
                Math.Max(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_LongLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_SameLength, AdjustLengthTestCase_ShortLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength, AdjustLengthTestCase_ShortLength)]
        public static void AdjustLengthIfLongTest(string rowLengthType, string columnLengthType)
        {
            var rowLength = rowLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };
            var columnLength = columnLengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLengthIfLong(rowLength, columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Min(TestTools.InitRowLength, rowLength),
                Math.Min(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        #endregion

        #region Row

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustRowLengthTest(string lengthType)
        {
            var rowLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLength(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, TestTools.InitColumnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, rowLength, TestTools.InitColumnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustRowLengthIfShortTest(string lengthType)
        {
            var rowLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLengthIfShort(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, TestTools.InitColumnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Max(TestTools.InitRowLength, rowLength),
                TestTools.InitColumnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustRowLengthIfLongTest(string lengthType)
        {
            var rowLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitRowLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitRowLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitRowLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustLengthIfLong(rowLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(rowLength, TestTools.InitColumnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, Math.Min(TestTools.InitRowLength, rowLength),
                TestTools.InitColumnLength, oldItems, funcMakeDefaultItem);
        }

        #endregion

        #region Column

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustColumnLengthTest(string lengthType)
        {
            var columnLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitColumnLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitColumnLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitColumnLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustItemLength(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(TestTools.InitRowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, TestTools.InitRowLength, columnLength, oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustColumnLengthIfShortTest(string lengthType)
        {
            var columnLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitColumnLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitColumnLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitColumnLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustItemLengthIfShort(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(TestTools.InitRowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, TestTools.InitRowLength,
                Math.Max(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        [TestCase(AdjustLengthTestCase_LongLength)]
        [TestCase(AdjustLengthTestCase_SameLength)]
        [TestCase(AdjustLengthTestCase_ShortLength)]
        public static void AdjustColumnLengthIfLongTest(string lengthType)
        {
            var columnLength = lengthType switch
            {
                AdjustLengthTestCase_LongLength => TestTools.InitColumnLength - 1,
                AdjustLengthTestCase_SameLength => TestTools.InitColumnLength,
                AdjustLengthTestCase_ShortLength => TestTools.InitColumnLength + 1,
                _ => throw new Exception(),
            };

            Func<int, int, TestRecord> funcMakeDefaultItem = TestTools.MakeListDefaultItem;

            var instance = MakeInstance(out var validatorMock);
            var oldItems = instance.ToTwoDimensionalArray();

            try
            {
                instance.AdjustItemLengthIfLong(columnLength);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckAdjustLength(TestTools.InitRowLength, columnLength);

            // 結果が正しいこと
            AssertElementsAdjustLength(instance, TestTools.InitRowLength,
                Math.Min(TestTools.InitColumnLength, columnLength), oldItems, funcMakeDefaultItem);
        }

        #endregion

        private static void AssertElementsAdjustLength(TwoDimensionalList<TestRecord> instance, int adjustRowLength,
            int adjustColumnLength, TestRecord[][] oldItems, Func<int, int, TestRecord> funcMakeDefaultItem)
        {
            var oldRowLength = oldItems.Length;
            var oldColumnLength = oldItems.GetInnerArrayLength();

            var notChangedRowLength = Math.Min(adjustRowLength, oldRowLength);
            var notChangedColumnLength = Math.Min(adjustColumnLength, oldColumnLength);

            var isRowAdded = adjustRowLength > oldRowLength;
            var isColumnAdded = adjustColumnLength > oldColumnLength;

            Assert.IsTrue(instance.Count == adjustRowLength);
            Assert.IsTrue(instance.ItemCount == adjustColumnLength);

            var r = 0;

            for (; r < notChangedRowLength; r++)
            {
                var c = 0;

                // 初期要素（調整位置より前）が変化していないこと
                for (; c < notChangedColumnLength; c++)
                {
                    Assert.IsTrue(instance[r, c].ItemEquals(oldItems[r][c]),
                        $"instance[{r}, {c}].ItemEquals(oldItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({oldItems[r][c]}))");
                }

                if (isColumnAdded)
                {
                    // 列に対して「追加」の場合、追加要素が正しいこと
                    for (; c < oldColumnLength; c++)
                    {
                        var answerItem = funcMakeDefaultItem(r, c);

                        Assert.IsTrue(instance[r, c].ItemEquals(answerItem),
                            $"instance[{r}, {c}].ItemEquals(answerItem) ({instance[r, c]}.ItemEquals({answerItem}))");
                    }
                }
                else
                {
                    // 列に対して「追加」以外の場合、すべての列要素に対して探索完了していること
                    Assert.AreEqual(notChangedColumnLength, c);
                }
            }

            if (isRowAdded)
            {
                // 行に対して「追加」の場合、追加要素が正しいこと
                for (var c = 0; c < adjustColumnLength; c++)
                {
                    var answerItem = funcMakeDefaultItem(r, c);

                    Assert.IsTrue(instance[r, c].ItemEquals(answerItem),
                        $"instance[{r}, {c}] == defaultItem({r}][{c}) ({instance[r, c]} == {answerItem})");
                }
            }
            else
            {
                // 行に対して「追加」以外の場合、すべての列要素に対して探索完了していること
                Assert.AreEqual(notChangedRowLength, r);
            }
        }

        #endregion

        #region Reset

        [Test]
        public static void ResetTest()
        {
            var instance = MakeInstance(out var validatorMock);

            var resetItems = TestTools.MakeTestRecordList(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                    false, TestTools.MakeInsertItem)
                .ToTwoDimensionalArray();

            try
            {
                instance.Reset(resetItems);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckReset(resetItems);

            // 結果が正しいこと
            AssertElementsReset(instance, resetItems);
        }

        [Test]
        public static void ClearTest()
        {
            var instance = MakeInstance(out var validatorMock);

            var expectedResetItems = Array.Empty<TestRecord[]>();

            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                Assert.Fail();
            }

            // 検証処理が正しく呼ばれていること
            validatorMock.CheckReset(expectedResetItems);

            // 結果が正しいこと
            AssertElementsReset(instance, expectedResetItems);
        }

        private static void AssertElementsReset(TwoDimensionalList<TestRecord> instance,
            TestRecord[][] resetItems)
        {
            var resetItemRowLength = resetItems.Length;
            var resetItemColumnLength = resetItems.GetInnerArrayLength();

            Assert.IsTrue(instance.Count == resetItemRowLength);
            Assert.IsTrue(instance.ItemCount == resetItemColumnLength);

            // すべての要素が初期化要素に置換されていること
            for (var r = 0; r < resetItemRowLength; r++)
            for (var c = 0; c < resetItemColumnLength; c++)
            {
                Assert.IsTrue(instance[r, c].ItemEquals(resetItems[r][c]),
                    $"instance[{r}, {c}].ItemEquals(resetItems[{r}][{c}]) ({instance[r, c]}.ItemEquals({resetItems[r][c]}))");
            }
        }

        #endregion

        #region TestTools

        private static TwoDimensionalList<TestRecord> MakeInstance(
            out TwoDimensionalListValidatorMock validatorMock)
        {
            validatorMock = new TwoDimensionalListValidatorMock(false);

            var result = TestTools.MakeTwoDimensionalList(
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                validatorMock, TestTools.MakeListDefaultItem);

            return result;
        }

        #endregion

        #region TestClass

        private class TestFixedLengthList : FixedLengthList<TestRecord, TestFixedLengthList>
        {
            private int Capacity { get; }

            public TestFixedLengthList(int initLength) : base(((Func<IEnumerable<TestRecord>>) (() =>
                Enumerable.Range(0, initLength).Select(i => new TestRecord(i.ToString()))))())
            {
                Capacity = initLength;
            }

            public override int GetCapacity() => Capacity;

            public override TestFixedLengthList DeepClone()
                => throw new NotSupportedException();

            protected override TestRecord MakeDefaultItem(int index)
                => new(index.ToString());
        }

        private class TwoDimensionalListValidatorMock : ITwoDimensionalListValidator<TestRecord>
        {
            private List<string> CalledMethodNames { get; } = new();
            private TestRecord[][] Items { get; set; }
            private TestRecord[][] InitItems { get; set; }
            private TestRecord[] Array_Array { get; set; }
            private TestRecord[,] Array_RectangularArray { get; set; }
            private TestRecord[][] Array_JaggedArray { get; set; }
            private int? Index { get; set; }
            private int? Row { get; set; }
            private int? Column { get; set; }
            private int? RowCount { get; set; }
            private int? ColumnCount { get; set; }
            private int? Count { get; set; }
            private int? OldIndex { get; set; }
            private int? NewIndex { get; set; }
            private int? RowLength { get; set; }
            private int? ColumnLength { get; set; }
            private bool? NeedFitItemsInnerSize { get; set; }
            private Direction Direction { get; set; }

            private bool IsRecordConstructor { get; }

            public TwoDimensionalListValidatorMock(bool isRecordConstructor = true)
            {
                IsRecordConstructor = isRecordConstructor;
            }

            #region ValidateHandler

            public void Constructor(TestRecord[][] initItems)
            {
                if (!IsRecordConstructor) return;

                CalledMethodNames.Add(nameof(Constructor));
                InitItems = initItems;
            }

            public void CopyTo(IReadOnlyList<TestRecord>[] array, int index)
            {
                CalledMethodNames.Add(nameof(CopyTo));
                Index = index;
            }

            public void CopyTo(IEnumerable<TestRecord>[] array, int index)
            {
                CalledMethodNames.Add(nameof(CopyTo));
                Index = index;
            }

            public void CopyTo(TestRecord[] array, int index, Direction direction)
            {
                CalledMethodNames.Add(nameof(CopyTo));
                Array_Array = array;
                Index = index;
                Direction = direction;
            }

            public void CopyTo(TestRecord[,] array, int row, int column)
            {
                CalledMethodNames.Add(nameof(CopyTo));
                Array_RectangularArray = array;
                Row = row;
                Column = column;
            }

            public void CopyTo(TestRecord[][] array, int row, int column)
            {
                CalledMethodNames.Add(nameof(CopyTo));
                Array_JaggedArray = array;
                Row = row;
                Column = column;
            }

            public void Get(int row, int rowCount, int column, int columnCount, Direction direction)
            {
                CalledMethodNames.Add(nameof(Get));
                Row = row;
                RowCount = rowCount;
                Column = column;
                ColumnCount = columnCount;
                Direction = direction;
            }

            public void Set(int row, int column, TestRecord[][] items, Direction direction, bool needFitItemsInnerSize)
            {
                CalledMethodNames.Add(nameof(Set));
                Row = row;
                Column = column;
                Items = items;
                Direction = direction;
                NeedFitItemsInnerSize = needFitItemsInnerSize;
            }

            public void Set(int row, int column, TestRecord[][] items)
            {
                CalledMethodNames.Add(nameof(Set));
                Row = row;
                Column = column;
                Items = items;
            }

            public void Insert(int index, TestRecord[][] items, Direction direction)
            {
                CalledMethodNames.Add(nameof(Insert));
                Index = index;
                Items = items;
                Direction = direction;
            }

            public void Overwrite(int index, TestRecord[][] items, Direction direction)
            {
                CalledMethodNames.Add(nameof(Overwrite));
                Index = index;
                Items = items;
                Direction = direction;
            }

            public void Move(int oldIndex, int newIndex, int count, Direction direction)
            {
                CalledMethodNames.Add(nameof(Move));
                OldIndex = oldIndex;
                NewIndex = newIndex;
                Count = count;
                Direction = direction;
            }

            public void Remove(int index, int count, Direction direction)
            {
                CalledMethodNames.Add(nameof(Remove));
                Index = index;
                Count = count;
                Direction = direction;
            }

            public void AdjustLength(int rowLength, int columnLength)
            {
                CalledMethodNames.Add(nameof(AdjustLength));
                RowLength = rowLength;
                ColumnLength = columnLength;
            }

            public void Reset(TestRecord[][] items)
            {
                CalledMethodNames.Add(nameof(Reset));
                Items = items;
            }

            public ITwoDimensionalListValidator<TestRecord> CreateAnotherFor(
                IReadOnlyTwoDimensionalList<TestRecord> target)
                => new TwoDimensionalListValidatorMock(IsRecordConstructor);

            #endregion

            #region Assersion

            // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

            public void CheckConstructor(IEnumerable<IEnumerable<TestRecord>> values, bool isValuesUseConstructor)
            {
                Assert.IsTrue(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Constructor)));

                var valuesArray = values.ToTwoDimensionalArray();

                Assert.IsTrue(isValuesUseConstructor
                    ? TestTools.IsAllItemReferenceEquals(InitItems, valuesArray)
                    : TestTools.IsAllItemEquals(InitItems, valuesArray));
                CheckNull(nameof(InitItems));
            }

            public void CheckCopyTo(TestRecord[] array, int index, Direction direction)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(CopyTo)));

                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Array_Array, array));
                Assert.AreEqual(index, Index);
                Assert.AreEqual(direction, Direction);
                CheckNull(
                    nameof(Array_Array),
                    nameof(Index),
                    nameof(Direction)
                );
            }

            public void CheckCopyTo(TestRecord[,] array, int row, int column)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(CopyTo)));

                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Array_RectangularArray, array));
                Assert.AreEqual(Row, row);
                Assert.AreEqual(Column, column);
                CheckNull(
                    nameof(Array_RectangularArray),
                    nameof(Row),
                    nameof(Column)
                );
            }

            public void CheckCopyTo(TestRecord[][] array, int row, int column)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(CopyTo)));

                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Array_JaggedArray, array));
                Assert.AreEqual(Row, row);
                Assert.AreEqual(Column, column);
                CheckNull(
                    nameof(Array_JaggedArray),
                    nameof(Row),
                    nameof(Column)
                );
            }

            public void CheckGet(int row, int rowCount, int column, int columnCount, Direction direction)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Get)));

                Assert.AreEqual(row, Row);
                Assert.AreEqual(rowCount, RowCount);
                Assert.AreEqual(column, Column);
                Assert.AreEqual(columnCount, ColumnCount);
                Assert.IsTrue(Direction == direction);
                CheckNull(
                    nameof(Row),
                    nameof(RowCount),
                    nameof(Column),
                    nameof(ColumnCount),
                    nameof(Direction)
                );
            }

            public void CheckSet(int row, int column, IEnumerable<IEnumerable<TestRecord>> items,
                Direction direction, bool needFitItemsInnerSize)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Set)));

                var itemArray = items.ToTwoDimensionalArray();

                Assert.AreEqual(row, Row);
                Assert.AreEqual(column, Column);
                Assert.IsTrue(TestTools.IsAllItemReferenceEquals(Items, itemArray));
                Assert.IsTrue(Direction == direction);
                Assert.AreEqual(needFitItemsInnerSize, NeedFitItemsInnerSize);
                CheckNull(
                    nameof(Row),
                    nameof(Column),
                    nameof(Items),
                    nameof(Direction),
                    nameof(NeedFitItemsInnerSize)
                );
            }

            public void CheckInsert(int index, IEnumerable<IEnumerable<TestRecord>> items, Direction direction)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Insert)));
                Assert.AreEqual(index, Index);
                CheckItemEquals(items);
                Assert.IsTrue(Direction == direction);
                CheckNull(
                    nameof(Index),
                    nameof(Items),
                    nameof(Direction)
                );
            }

            public void CheckOverwrite(int index, IEnumerable<IEnumerable<TestRecord>> items, Direction direction)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Overwrite)));
                Assert.IsTrue(Direction == direction);

                Assert.AreEqual(index, Index);
                CheckItemEquals(items);
                CheckNull(
                    nameof(Index),
                    nameof(Items),
                    nameof(Direction)
                );
            }

            public void CheckMove(int oldIndex, int newIndex, int count, Direction direction)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Move)));

                Assert.AreEqual(oldIndex, OldIndex);
                Assert.AreEqual(newIndex, NewIndex);
                Assert.AreEqual(count, Count);
                Assert.IsTrue(Direction == direction);
                CheckNull(
                    nameof(OldIndex),
                    nameof(NewIndex),
                    nameof(Count),
                    nameof(Direction)
                );
            }

            public void CheckRemove(int index, int count, Direction direction)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Remove)));

                Assert.AreEqual(index, Index);
                Assert.AreEqual(count, Count);
                Assert.IsTrue(Direction == direction);
                CheckNull(
                    nameof(Index),
                    nameof(Count),
                    nameof(Direction)
                );
            }

            public void CheckAdjustLength(int rowLength, int columnLength)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(AdjustLength)));

                Assert.AreEqual(rowLength, RowLength);
                Assert.AreEqual(columnLength, ColumnLength);
                CheckNull(nameof(RowLength), nameof(ColumnLength));
            }

            public void CheckReset(TestRecord[][] items)
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(1, CalledMethodNames.Count);
                Assert.IsTrue(CalledMethodNames[0].Equals(nameof(Reset)));

                CheckItemEquals(items);
                CheckNull(nameof(Items));
            }

            public void CheckNotWork()
            {
                Assert.IsFalse(IsRecordConstructor);

                Assert.AreEqual(0, CalledMethodNames.Count);
                CheckNull();
            }

            private void CheckNull(params string[] ignores)
            {
                var targets = new List<string>
                {
                    nameof(Items),
                    nameof(InitItems),
                    nameof(Row),
                    nameof(Column),
                    nameof(RowCount),
                    nameof(ColumnCount),
                    nameof(Count),
                    nameof(OldIndex),
                    nameof(NewIndex),
                    nameof(RowLength),
                    nameof(ColumnLength),
                };

                targets.RemoveAll(ignores.Contains);

                foreach (var target in targets)
                {
                    switch (target)
                    {
                        case nameof(Items):
                            Assert.IsNull(Items);
                            break;
                        case nameof(InitItems):
                            Assert.IsNull(InitItems);
                            break;
                        case nameof(Row):
                            Assert.IsNull(Row);
                            break;
                        case nameof(Column):
                            Assert.IsNull(Column);
                            break;
                        case nameof(RowCount):
                            Assert.IsNull(RowCount);
                            break;
                        case nameof(ColumnCount):
                            Assert.IsNull(ColumnCount);
                            break;
                        case nameof(Count):
                            Assert.IsNull(Count);
                            break;
                        case nameof(OldIndex):
                            Assert.IsNull(OldIndex);
                            break;
                        case nameof(NewIndex):
                            Assert.IsNull(NewIndex);
                            break;
                        case nameof(RowLength):
                            Assert.IsNull(RowLength);
                            break;
                        case nameof(ColumnLength):
                            Assert.IsNull(ColumnLength);
                            break;
                        default:
                            Assert.Fail(target);
                            break;
                    }
                }
            }

            private void CheckItemEquals(IEnumerable<IEnumerable<TestRecord>> items)
            {
                var itemArrays = items.ToTwoDimensionalArray();
                var insertItemLength = itemArrays.Length;
                var insertItemInnerLength = itemArrays.GetInnerArrayLength();

                Assert.AreEqual(Items.Length, insertItemLength);
                Assert.AreEqual(Items.GetInnerArrayLength(), insertItemInnerLength);

                for (var r = 0; r < insertItemLength; r++)
                for (var c = 0; c < insertItemInnerLength; c++)
                {
                    Assert.IsTrue(itemArrays[r][c].ItemEquals(Items[r][c]));
                }
            }

            // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Local

            #endregion
        }

        #endregion
    }
}
