using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class RestrictedCapacityTwoDimensionalListValidatorTest
    {
        private static Logger logger = default!;

        private static class TestInstanceConfig
        {
            public const int InitRowLength = 3;
            public const int InitColumnLength = 5;
            public const int MaxRowCapacity = 6;
            public const int MinRowCapacity = 2;
            public const int MaxColumnCapacity = 7;
            public const int MinColumnCapacity = 4;
        }

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] ConstructorTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // initItemsType
                new[]
                {
                    TestItemGenerator.GenerateTwoDimArrayType.Null,
                    TestItemGenerator.GenerateTwoDimArrayType.Dig,
                    TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                    TestItemGenerator.GenerateTwoDimArrayType.Fill
                },
                // rowLength
                new[]
                {
                    TestInstanceConfig.MinRowCapacity - 1,
                    TestInstanceConfig.MinRowCapacity,
                    TestInstanceConfig.MaxRowCapacity,
                    TestInstanceConfig.MaxRowCapacity + 1
                },
                // columnLength
                new[]
                {
                    TestInstanceConfig.MinColumnCapacity - 1,
                    TestInstanceConfig.MinColumnCapacity,
                    TestInstanceConfig.MaxColumnCapacity,
                    TestInstanceConfig.MaxColumnCapacity + 1
                }
            ),
            // expectedError
            (initItemsType, rowLength, columnLength) => initItemsType != TestItemGenerator.GenerateTwoDimArrayType.Fill
                                                        || !rowLength.IsBetween(
                                                            TestInstanceConfig.MinRowCapacity,
                                                            TestInstanceConfig.MaxRowCapacity
                                                        )
                                                        || !columnLength.IsBetween(
                                                            TestInstanceConfig.MinColumnCapacity,
                                                            TestInstanceConfig.MaxColumnCapacity
                                                        )
        );

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(
            TestItemGenerator.GenerateTwoDimArrayType initItemsType,
            int rowLength,
            int columnLength,
            bool expectedError
        )
        {
            var dummyTarget = new RestrictedTwoDimTestForConstructor();
            var instance =
                new RestrictedCapacityTwoDimensionalListValidator<IEnumerable<string>, string>(dummyTarget, "行", "列");

            var items = TestItemGenerator.GenerateTwoDimArray(
                initItemsType,
                rowLength,
                columnLength,
                (r, c) => $"{r}_{c}"
            );
            var initItems = new NamedValue<IEnumerable<IEnumerable<string>>>("initItems", items);

            TestTemplate.PureMethod(
                instance,
                target => target.Constructor(initItems),
                expectedError,
                logger
            );
        }

        private static readonly object[] GetRowTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // rowCount
                new[]
                {
                    new NamedValue<int>("rowCount", -1),
                    new NamedValue<int>("rowCount", 0),
                    new NamedValue<int>("rowCount", TestInstanceConfig.InitRowLength),
                    new NamedValue<int>("rowCount", TestInstanceConfig.InitRowLength + 1)
                }
            ),
            // expectedError
            (rowIndex, rowCount) => !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                                    || !rowCount.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                                    || (rowIndex.Value + rowCount.Value) > TestInstanceConfig.InitRowLength
        );

        [TestCaseSource(nameof(GetRowTestCaseSource))]
        public static void GetRowTest(NamedValue<int> rowIndex, NamedValue<int> rowCount, bool expectedError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.GetRow(rowIndex, rowCount),
                expectedError,
                logger
            );
        }

        private static readonly object[] GetColumnTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength)
                },
                // rowCount
                new[]
                {
                    new NamedValue<int>("columnCount", -1),
                    new NamedValue<int>("columnCount", 0),
                    new NamedValue<int>("columnCount", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnCount", TestInstanceConfig.InitColumnLength + 1)
                }
            ),
            // expectedError
            (columnIndex, columnCount) => !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                                          || !columnCount.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                                          || (columnIndex.Value + columnCount.Value)
                                          > TestInstanceConfig.InitColumnLength
        );

        [TestCaseSource(nameof(GetColumnTestCaseSource))]
        public static void GetColumnTest(NamedValue<int> columnIndex, NamedValue<int> columnCount, bool expectedError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.GetColumn(columnIndex, columnCount),
                expectedError,
                logger
            );
        }

        private static readonly object[] GetItemTest1_TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // rowCount
                new[]
                {
                    new NamedValue<int>("rowCount", -1),
                    new NamedValue<int>("rowCount", 0),
                    new NamedValue<int>("rowCount", TestInstanceConfig.InitRowLength),
                    new NamedValue<int>("rowCount", TestInstanceConfig.InitRowLength + 1)
                },
                // columnIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength + 1)
                },
                // columnCount
                new[]
                {
                    new NamedValue<int>("columnCount", -1),
                    new NamedValue<int>("columnCount", 0),
                    new NamedValue<int>("columnCount", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnCount", TestInstanceConfig.InitColumnLength + 1)
                }
            ),
            // expectedError
            (rowIndex, rowCount, columnIndex, columnCount)
                => !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                   || !rowCount.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                   || (rowIndex.Value + rowCount.Value) > TestInstanceConfig.InitRowLength
                   || !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                   || !columnCount.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                   || (columnIndex.Value + columnCount.Value) > TestInstanceConfig.InitColumnLength
        );

        [TestCaseSource(nameof(GetItemTest1_TestCaseSource))]
        public static void GetItemTest1(
            NamedValue<int> rowIndex,
            NamedValue<int> rowCount,
            NamedValue<int> columnIndex,
            NamedValue<int> columnCount,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.GetItem(rowIndex, rowCount, columnIndex, columnCount),
                expectedError,
                logger
            );
        }

        private static readonly object[] GetItemTest2_TestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // columnIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength + 1)
                }
            ),
            // expectedError
            (rowIndex, columnIndex)
                => !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                   || !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
        );

        [TestCaseSource(nameof(GetItemTest2_TestCaseSource))]
        public static void GetItemTest2(NamedValue<int> rowIndex, NamedValue<int> columnIndex, bool expectedError)
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.GetItem(rowIndex, columnIndex),
                expectedError,
                logger
            );
        }

        private static readonly object[] SetRowTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // rowsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.NotEqualColumnLength,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        0,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        1,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength - 1,
                        TestInstanceConfig.InitColumnLength + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    )
                }
            ),
            // expectedError
            (rowIndex, rowsInfo) =>
                !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                || rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (rowsInfo.RowLength != 0 && rowsInfo.ColumnLength != TestInstanceConfig.InitColumnLength)
                || (rowIndex.Value + rowsInfo.RowLength) > TestInstanceConfig.InitRowLength
        );

        [TestCaseSource(nameof(SetRowTestCaseSource))]
        public static void SetRowTest(
            NamedValue<int> rowIndex,
            TestItemGenerator.RowsInfo rowsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var items = TestItemGenerator.GenerateTwoDimArray(rowsInfo, (r, c) => $"{r}_{c}");
            var _rows = ConvertRows(items);
            var rows = new NamedValue<IEnumerable<RowForTest>>("rows", _rows);

            TestTemplate.PureMethod(
                instance,
                target => target.SetRow(rowIndex, rows),
                expectedError,
                logger
            );
        }

        private static readonly object[] SetColumnTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // columnIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength)
                },
                // rowsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.NotEqualColumnLength,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        0,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        1,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength + 1
                    )
                }
            ),
            // expectedError
            (columnIndex, columnsInfo) =>
                !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                || columnsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (columnsInfo.RowLength != 0 && columnsInfo.ColumnLength != TestInstanceConfig.InitRowLength)
                || (columnIndex.Value + columnsInfo.RowLength) > TestInstanceConfig.InitColumnLength
        );

        [TestCaseSource(nameof(SetColumnTestCaseSource))]
        public static void SetColumnTest(
            NamedValue<int> columnIndex,
            TestItemGenerator.RowsInfo rowsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var _items = TestItemGenerator.GenerateTwoDimArray(rowsInfo, (r, c) => $"{r}_{c}");
            var items = new NamedValue<IEnumerable<IEnumerable<string>>>("items", _items);

            TestTemplate.PureMethod(
                instance,
                target => target.SetColumn(columnIndex, items),
                expectedError,
                logger
            );
        }

        private static readonly object[] SetItemTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // columnIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength)
                },
                // item
                new[]
                {
                    new NamedValue<string?>("item", null),
                    new NamedValue<string?>("item", "")
                }
            ),
            // expectedError
            (rowIndex, columnIndex, item) =>
                !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                || !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                || item.Value is null
        );

        [TestCaseSource(nameof(SetItemTestCaseSource))]
        public static void SetItemTest(
            NamedValue<int> rowIndex,
            NamedValue<int> columnIndex,
            NamedValue<string> item,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.SetItem(rowIndex, columnIndex, item),
                expectedError,
                logger
            );
        }

        private static readonly object[] InsertRowTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // rowsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.NotEqualColumnLength,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        0,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        1,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength + 1,
                        TestInstanceConfig.InitColumnLength
                    )
                }
            ),
            // expectedError
            (rowIndex, rowsInfo) =>
                !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                || rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (rowsInfo.RowLength != 0 && rowsInfo.ColumnLength != TestInstanceConfig.InitColumnLength)
                || rowsInfo.RowLength + TestInstanceConfig.InitRowLength > TestInstanceConfig.MaxRowCapacity
        );

        [TestCaseSource(nameof(InsertRowTestCaseSource))]
        public static void InsertRowTest(
            NamedValue<int> rowIndex,
            TestItemGenerator.RowsInfo rowsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var items = TestItemGenerator.GenerateTwoDimArray(rowsInfo, (r, c) => $"{r}_{c}");
            var _rows = ConvertRows(items);
            var rows = new NamedValue<IEnumerable<RowForTest>>("rows", _rows);

            TestTemplate.PureMethod(
                instance,
                target => target.InsertRow(rowIndex, rows),
                expectedError,
                logger
            );
        }

        private static readonly object[] InsertColumnTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // columnIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength + 1)
                },
                // columnsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        2,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        2,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        2,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.NotEqualColumnLength,
                        2,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        0,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        1,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxColumnCapacity - TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxColumnCapacity - TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxColumnCapacity - TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxColumnCapacity - TestInstanceConfig.InitColumnLength + 1,
                        TestInstanceConfig.InitRowLength + 1
                    )
                }
            ),
            // expectedError
            (columnIndex, columnsInfo) =>
                !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                || columnsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (columnsInfo.RowLength != 0 && columnsInfo.ColumnLength != TestInstanceConfig.InitRowLength)
                || columnsInfo.RowLength + TestInstanceConfig.InitColumnLength > TestInstanceConfig.MaxColumnCapacity
        );

        [TestCaseSource(nameof(InsertColumnTestCaseSource))]
        public static void InsertColumnTest(
            NamedValue<int> columnIndex,
            TestItemGenerator.RowsInfo columnsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var _items = TestItemGenerator.GenerateTwoDimArray(columnsInfo, (r, c) => $"{r}_{c}");
            var items = new NamedValue<IEnumerable<IEnumerable<string>>>("items", _items);

            TestTemplate.PureMethod(
                instance,
                target => target.InsertColumn(columnIndex, items),
                expectedError,
                logger
            );
        }

        private static readonly object[] OverwriteRowTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength + 1)
                },
                // rowsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        2,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        2,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        2,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.NotEqualColumnLength,
                        2,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        0,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        1,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity - TestInstanceConfig.InitRowLength + 1,
                        TestInstanceConfig.InitColumnLength
                    )
                }
            ),
            // expectedError
            (rowIndex, rowsInfo) =>
                !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                || rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (rowsInfo.RowLength != 0 && rowsInfo.ColumnLength != TestInstanceConfig.InitColumnLength)
                || (rowIndex.Value + rowsInfo.RowLength) > TestInstanceConfig.MaxRowCapacity
        );

        [TestCaseSource(nameof(OverwriteRowTestCaseSource))]
        public static void OverwriteRowTest(
            NamedValue<int> rowIndex,
            TestItemGenerator.RowsInfo rowsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var items = TestItemGenerator.GenerateTwoDimArray(rowsInfo, (r, c) => $"{r}_{c}");
            var _rows = ConvertRows(items);
            var rows = new NamedValue<IEnumerable<RowForTest>>("rows", _rows);

            TestTemplate.PureMethod(
                instance,
                target => target.OverwriteRow(rowIndex, rows),
                expectedError,
                logger
            );
        }

        private static readonly object[] OverwriteColumnTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // columnIndex
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength + 1)
                },
                // rowsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.NotEqualColumnLength,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        0,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        1,
                        0
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitColumnLength,
                        TestInstanceConfig.InitRowLength + 1
                    )
                }
            ),
            // expectedError
            (columnIndex, columnsInfo) =>
                !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                || columnsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (columnsInfo.RowLength != 0 && columnsInfo.ColumnLength != TestInstanceConfig.InitRowLength)
                || (columnIndex.Value + columnsInfo.RowLength) > TestInstanceConfig.MaxColumnCapacity
        );

        [TestCaseSource(nameof(OverwriteColumnTestCaseSource))]
        public static void OverwriteColumnTest(
            NamedValue<int> columnIndex,
            TestItemGenerator.RowsInfo rowsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var _items = TestItemGenerator.GenerateTwoDimArray(rowsInfo, (r, c) => $"{r}_{c}");
            var items = new NamedValue<IEnumerable<IEnumerable<string>>>("items", _items);

            TestTemplate.PureMethod(
                instance,
                target => target.OverwriteColumn(columnIndex, items),
                expectedError,
                logger
            );
        }

        private static readonly object[] MoveRowTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // oldRowIndex,
                new[]
                {
                    new NamedValue<int>("oldRowIndex", -1),
                    new NamedValue<int>("oldRowIndex", 0),
                    new NamedValue<int>("oldRowIndex", 1),
                    new NamedValue<int>("oldRowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("oldRowIndex", TestInstanceConfig.InitRowLength)
                },
                // newRowIndex,
                new[]
                {
                    new NamedValue<int>("newRowIndex", -1),
                    new NamedValue<int>("newRowIndex", 0),
                    new NamedValue<int>("newRowIndex", 1),
                    new NamedValue<int>("newRowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("newRowIndex", TestInstanceConfig.InitRowLength)
                },
                // count,
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitRowLength)
                }
            ),
            // expectedError
            (oldRowIndex, newRowIndex, count) => !oldRowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                                                 || !newRowIndex.Value.IsBetween(
                                                     0,
                                                     TestInstanceConfig.InitRowLength - 1
                                                 )
                                                 || !count.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                                                 || oldRowIndex.Value + count.Value > TestInstanceConfig.InitRowLength
                                                 || newRowIndex.Value + count.Value > TestInstanceConfig.InitRowLength
        );

        [TestCaseSource(nameof(MoveRowTestCaseSource))]
        public static void MoveRowTest(
            NamedValue<int> oldRowIndex,
            NamedValue<int> newRowIndex,
            NamedValue<int> count,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.MoveRow(oldRowIndex, newRowIndex, count),
                expectedError,
                logger
            );
        }

        private static readonly object[] MoveColumnTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // oldColumnIndex,
                new[]
                {
                    new NamedValue<int>("oldColumnIndex", -1),
                    new NamedValue<int>("oldColumnIndex", 0),
                    new NamedValue<int>("oldColumnIndex", 1),
                    new NamedValue<int>("oldColumnIndex", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("oldColumnIndex", TestInstanceConfig.InitColumnLength)
                },
                // newColumnIndex,
                new[]
                {
                    new NamedValue<int>("newColumnIndex", -1),
                    new NamedValue<int>("newColumnIndex", 0),
                    new NamedValue<int>("newColumnIndex", 1),
                    new NamedValue<int>("newColumnIndex", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("newColumnIndex", TestInstanceConfig.InitColumnLength)
                },
                // count,
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitColumnLength)
                }
            ),
            // expectedError
            (oldColumnIndex, newColumnIndex, count)
                => !oldColumnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                   || !newColumnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                   || !count.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                   || oldColumnIndex.Value + count.Value > TestInstanceConfig.InitColumnLength
                   || newColumnIndex.Value + count.Value > TestInstanceConfig.InitColumnLength
        );

        [TestCaseSource(nameof(MoveColumnTestCaseSource))]
        public static void MoveColumnTest(
            NamedValue<int> oldColumnIndex,
            NamedValue<int> newColumnIndex,
            NamedValue<int> count,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.MoveColumn(oldColumnIndex, newColumnIndex, count),
                expectedError,
                logger
            );
        }

        private static readonly object[] RemoveRowTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowIndex,
                new[]
                {
                    new NamedValue<int>("rowIndex", -1),
                    new NamedValue<int>("rowIndex", 0),
                    new NamedValue<int>("rowIndex", 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("rowIndex", TestInstanceConfig.InitRowLength)
                },
                // count,
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>("count", TestInstanceConfig.InitRowLength - TestInstanceConfig.MinRowCapacity),
                    new NamedValue<int>(
                        "count",
                        TestInstanceConfig.InitRowLength - TestInstanceConfig.MinRowCapacity + 1
                    )
                }
            ),
            // expectedError
            (rowIndex, count) => !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                                 || !count.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                                 || rowIndex.Value + count.Value > TestInstanceConfig.InitRowLength
                                 || TestInstanceConfig.InitRowLength - count.Value < TestInstanceConfig.MinRowCapacity
        );

        [TestCaseSource(nameof(RemoveRowTestCaseSource))]
        public static void RemoveRowTest(
            NamedValue<int> rowIndex,
            NamedValue<int> count,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.RemoveRow(rowIndex, count),
                expectedError,
                logger
            );
        }

        private static readonly object[] RemoveColumnTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // columnIndex,
                new[]
                {
                    new NamedValue<int>("columnIndex", -1),
                    new NamedValue<int>("columnIndex", 0),
                    new NamedValue<int>("columnIndex", 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("columnIndex", TestInstanceConfig.InitColumnLength)
                },
                // count,
                new[]
                {
                    new NamedValue<int>("count", -1),
                    new NamedValue<int>("count", 0),
                    new NamedValue<int>(
                        "count",
                        TestInstanceConfig.InitColumnLength - TestInstanceConfig.MinColumnCapacity
                    ),
                    new NamedValue<int>(
                        "count",
                        TestInstanceConfig.InitColumnLength - TestInstanceConfig.MinColumnCapacity + 1
                    )
                }
            ),
            // columnIndex
            (columnIndex, count) => !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                                    || !count.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                                    || columnIndex.Value + count.Value > TestInstanceConfig.InitColumnLength
                                    || TestInstanceConfig.InitColumnLength - count.Value
                                    < TestInstanceConfig.MinColumnCapacity
        );

        [TestCaseSource(nameof(RemoveColumnTestCaseSource))]
        public static void RemoveColumnTest(
            NamedValue<int> columnIndex,
            NamedValue<int> count,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.RemoveColumn(columnIndex, count),
                expectedError,
                logger
            );
        }

        private static readonly object[] AdjustLengthTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowLength,
                new[]
                {
                    new NamedValue<int>("rowLength", TestInstanceConfig.MinRowCapacity - 1),
                    new NamedValue<int>("rowLength", TestInstanceConfig.MinRowCapacity),
                    new NamedValue<int>("rowLength", TestInstanceConfig.MaxRowCapacity),
                    new NamedValue<int>("rowLength", TestInstanceConfig.MaxRowCapacity + 1)
                },
                // columnLength,
                new[]
                {
                    new NamedValue<int>("columnLength", TestInstanceConfig.MinColumnCapacity - 1),
                    new NamedValue<int>("columnLength", TestInstanceConfig.MinColumnCapacity),
                    new NamedValue<int>("columnLength", TestInstanceConfig.MaxColumnCapacity),
                    new NamedValue<int>("columnLength", TestInstanceConfig.MaxColumnCapacity + 1)
                }
            ),
            // expectedError
            (rowLength, columnLength) => !rowLength.Value.IsBetween(
                                             TestInstanceConfig.MinRowCapacity,
                                             TestInstanceConfig.MaxRowCapacity
                                         )
                                         || !columnLength.Value.IsBetween(
                                             TestInstanceConfig.MinColumnCapacity,
                                             TestInstanceConfig.MaxColumnCapacity
                                         )
        );

        [TestCaseSource(nameof(AdjustLengthTestCaseSource))]
        public static void AdjustLengthTest(
            NamedValue<int> rowLength,
            NamedValue<int> columnLength,
            bool expectedError
        )
        {
            var instance = GetTestInstance();

            TestTemplate.PureMethod(
                instance,
                target => target.AdjustLength(rowLength, columnLength),
                expectedError,
                logger
            );
        }

        private static readonly object[] ResetTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // rowsInfo
                new[]
                {
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Null,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Dig,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.HasNull,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MinRowCapacity - 1,
                        TestInstanceConfig.MinColumnCapacity
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MinRowCapacity,
                        TestInstanceConfig.MinColumnCapacity - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MinRowCapacity,
                        TestInstanceConfig.MinColumnCapacity
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MinRowCapacity,
                        TestInstanceConfig.MaxColumnCapacity
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MinRowCapacity,
                        TestInstanceConfig.MaxColumnCapacity + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity,
                        TestInstanceConfig.MaxColumnCapacity
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.MaxRowCapacity + 1,
                        TestInstanceConfig.MaxColumnCapacity
                    )
                }
            ),
            // expectedError
            rowsInfo => rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                        || !rowsInfo.RowLength.IsBetween(
                            TestInstanceConfig.MinRowCapacity,
                            TestInstanceConfig.MaxRowCapacity
                        )
                        || !rowsInfo.ColumnLength.IsBetween(
                            TestInstanceConfig.MinColumnCapacity,
                            TestInstanceConfig.MaxColumnCapacity
                        )
        );

        [TestCaseSource(nameof(ResetTestCaseSource))]
        public static void ResetTest(
            TestItemGenerator.RowsInfo rowsInfo,
            bool expectedError
        )
        {
            var instance = GetTestInstance();
            var items = TestItemGenerator.GenerateTwoDimArray(rowsInfo, (r, c) => $"{r}_{c}");
            var _initItems = ConvertRows(items);
            var initItems = new NamedValue<IEnumerable<RowForTest>>("initItems", _initItems);

            TestTemplate.PureMethod(
                instance,
                target => target.Reset(initItems),
                expectedError,
                logger
            );
        }

        #region For TestInstance

        private static RestrictedCapacityTwoDimensionalListValidator<RowForTest, string> GetTestInstance()
            => (RestrictedCapacityTwoDimensionalListValidator<RowForTest, string>)TwoDimListForTest.Validator!;

        private static readonly TwoDimensionalList<RowForTest, string> TwoDimListForTest =
            new(
                TestInstanceConfig.InitRowLength,
                TestInstanceConfig.InitColumnLength,
                new TwoDimensionalList<RowForTest, string>.Config(
                    items => new RowForTest(items),
                    (r, c) => $"{r}_{c}".ToString(),
                    (l, r) => l.Equals(r),
                    target => new RestrictedCapacityTwoDimensionalListValidator<RowForTest, string>(target, "行", "列")
                )
                {
                    MaxRowCapacity = TestInstanceConfig.MaxRowCapacity,
                    MinRowCapacity = TestInstanceConfig.MinRowCapacity,
                    MaxColumnCapacity = TestInstanceConfig.MaxColumnCapacity,
                    MinColumnCapacity = TestInstanceConfig.MinColumnCapacity
                }
            );

        private class RowForTest : ExtendedList<string>
        {
            public RowForTest(IEnumerable<string>? initItems = null) : base(
                i => i.ToString(),
                null,
                initItems
            )
            {
            }
        }

        private static IEnumerable<RowForTest> ConvertRows(IEnumerable<IEnumerable<string?>?>? items)
            => items?.Select(row => new RowForTest(row!)) ?? default!;

        private class RestrictedTwoDimTestForConstructor : TwoDimensionalList<RowForTest, string>,
            ITwoDimensionalList<IEnumerable<string>, string>
        {
            public new ITwoDimensionalListValidator<IEnumerable<string>, string>? Validator { get; }

            internal RestrictedTwoDimTestForConstructor() : base(
                TestItemGenerator.GenerateTwoDimArray(
                    TestItemGenerator.GenerateTwoDimArrayType.Fill,
                    TestInstanceConfig.InitRowLength,
                    TestInstanceConfig.InitColumnLength,
                    (r, c) => $"{r}_{c}"
                ),
                new Config(
                    items => new RowForTest(items),
                    (r, c) => $"{r}_{c}",
                    (l, r) => l == r,
                    _ => null
                )
            )
            {
                Validator = default;
            }

            public new int GetMaxRowCapacity() => TestInstanceConfig.MaxRowCapacity;
            public new int GetMinRowCapacity() => TestInstanceConfig.MinRowCapacity;
            public new int GetMaxColumnCapacity() => TestInstanceConfig.MaxColumnCapacity;
            public new int GetMinColumnCapacity() => TestInstanceConfig.MinColumnCapacity;

            // @formatter:off

            public bool ItemEquals(ITwoDimensionalList<IEnumerable<string>, string>? other) { throw new NotImplementedException(); }
            public new ITwoDimensionalList<IEnumerable<string>, string> DeepClone() { throw new NotImplementedException(); }
            public new IEnumerator<IEnumerable<string>> GetEnumerator() { throw new NotImplementedException(); }
            public new IEnumerable<string> this[int rowIndex]
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }
            public new string this[int rowIndex, int columnIndex]
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }
            public new IEnumerable<IEnumerable<string>> GetRowRangeCore(int rowIndex, int rowCount) { throw new NotImplementedException(); }
            public void SetRowRangeCore(int rowIndex, IEnumerable<IEnumerable<string>> items) { throw new NotImplementedException(); }
            public void AddRowRangeCore(IEnumerable<IEnumerable<string>> items) { throw new NotImplementedException(); }
            public void InsertRowRangeCore(int rowIndex, IEnumerable<IEnumerable<string>> items) { throw new NotImplementedException(); }
            public void OverwriteRowCore(int rowIndex, IEnumerable<IEnumerable<string>> items) { throw new NotImplementedException(); }
            public void ResetCore(IEnumerable<IEnumerable<string>> rows) { throw new NotImplementedException(); }
            // @formatter:on
        }

        #endregion
    }
}
