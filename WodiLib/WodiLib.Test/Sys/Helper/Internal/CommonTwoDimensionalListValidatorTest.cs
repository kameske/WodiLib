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
    public class CommonTwoDimensionalListValidatorTest
    {
        private static Logger logger = default!;

        private static class TestInstanceConfig
        {
            public const int InitRowLength = 3;
            public const int InitColumnLength = 5;
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
                }
            ),
            // expectedError
            initItemsType => initItemsType != TestItemGenerator.GenerateTwoDimArrayType.Fill
        );

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(TestItemGenerator.GenerateTwoDimArrayType initItemsType, bool expectedError)
        {
            // target に null を与えても Constructor でだけはエラーにならない
            var instance = new CommonTwoDimensionalListValidator<IEnumerable<string>, string>(default!);

            var items = TestItemGenerator.GenerateTwoDimArray(
                initItemsType,
                TestInstanceConfig.InitRowLength,
                TestInstanceConfig.InitColumnLength,
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
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength + 1,
                        TestInstanceConfig.InitColumnLength
                    )
                }
            ),
            // expectedError
            (rowIndex, rowsInfo) =>
                !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                || rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (rowsInfo.RowLength != 00 && rowsInfo.ColumnLength != TestInstanceConfig.InitColumnLength)
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
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength - 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength + 1
                    ),
                    new TestItemGenerator.RowsInfo(
                        TestItemGenerator.GenerateTwoDimArrayType.Fill,
                        TestInstanceConfig.InitRowLength + 1,
                        TestInstanceConfig.InitColumnLength
                    )
                }
            ),
            // expectedError
            (rowIndex, rowsInfo) =>
                !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                || rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
                || (rowsInfo.RowLength != 0 && rowsInfo.ColumnLength != TestInstanceConfig.InitColumnLength)
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
                    new NamedValue<int>("count", 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitRowLength - 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitRowLength)
                }
            ),
            // expectedError
            (rowIndex, count) => !rowIndex.Value.IsBetween(0, TestInstanceConfig.InitRowLength - 1)
                                 || !count.Value.IsBetween(0, TestInstanceConfig.InitRowLength)
                                 || rowIndex.Value + count.Value > TestInstanceConfig.InitRowLength
                                 || TestInstanceConfig.InitRowLength - count.Value < 0
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
                    new NamedValue<int>("count", 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitColumnLength - 1),
                    new NamedValue<int>("count", TestInstanceConfig.InitColumnLength)
                }
            ),
            // expectedError
            (columnIndex, count) => !columnIndex.Value.IsBetween(0, TestInstanceConfig.InitColumnLength - 1)
                                    || !count.Value.IsBetween(0, TestInstanceConfig.InitColumnLength)
                                    || columnIndex.Value + count.Value > TestInstanceConfig.InitColumnLength
                                    || TestInstanceConfig.InitColumnLength - count.Value < 0
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
                    new NamedValue<int>("rowLength", -1),
                    new NamedValue<int>("rowLength", 0),
                    new NamedValue<int>("rowLength", 1),
                    new NamedValue<int>("rowLength", TestInstanceConfig.InitRowLength),
                    new NamedValue<int>("rowLength", TestInstanceConfig.InitRowLength + 1)
                },
                // columnLength,
                new[]
                {
                    new NamedValue<int>("columnLength", -1),
                    new NamedValue<int>("columnLength", 0),
                    new NamedValue<int>("columnLength", 1),
                    new NamedValue<int>("columnLength", TestInstanceConfig.InitColumnLength),
                    new NamedValue<int>("columnLength", TestInstanceConfig.InitColumnLength + 1)
                }
            ),
            // expectedError
            (rowLength, columnLength) => rowLength.Value < 0
                                         || columnLength.Value < 0
                                         || (rowLength.Value == 0 && columnLength.Value != 0)
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
                        TestInstanceConfig.InitRowLength,
                        TestInstanceConfig.InitColumnLength
                    )
                }
            ),
            // expectedError
            rowsInfo => rowsInfo.Type != TestItemGenerator.GenerateTwoDimArrayType.Fill
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

        private static CommonTwoDimensionalListValidator<RowForTest, string> GetTestInstance()
            => (CommonTwoDimensionalListValidator<RowForTest, string>)TwoDimListForTest.Validator;

        private static readonly TwoDimensionalList<RowForTest, string> TwoDimListForTest =
            new(
                TestInstanceConfig.InitRowLength,
                TestInstanceConfig.InitColumnLength,
                new TwoDimensionalList<RowForTest, string>.Config(
                    items => new RowForTest(items),
                    (r, c) => $"{r}_{c}".ToString(),
                    (l, r) => l.Equals(r),
                    target => new CommonTwoDimensionalListValidator<RowForTest, string>(target)
                )
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

        #endregion
    }
}
