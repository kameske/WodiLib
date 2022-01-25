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
using TestTools = WodiLib.Test.Sys.TwoDimensionalListTest_Tools;
using TestRecord = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord;
using TestTwoDimList = WodiLib.Sys.Collections.TwoDimensionalList<
    WodiLib.Sys.Collections.IFixedLengthList<WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord>,
    WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecordList,
    WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestRecord
>;
using TestDoubleEnumerableInstanceType = WodiLib.Test.Sys.TwoDimensionalListTest_Tools.TestDoubleEnumerableInstanceType;


namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class TwoDimensionalListNotifyTest
    {
        /*
         * 各メソッドの変更通知が正しいことを事前条件とする。
         * 引数が誤っている場合の動作や処理結果の正しさは確認しない。
         * (TwoDimensionalListTest, TwoDimensionalListOperationValidateTest, TwoDimensionalListOperationResultTestで確認)
         *
         * 通知が正しく行われることをテストする。
         */

        private static Logger logger;
        private static IEqualityComparer<TestRecord> testRecordComparer;
        private static IEqualityComparer<IEnumerable<TestRecord>> itemListComparer;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
            testRecordComparer = EqualityComparerFactory.Create<TestRecord>();
            itemListComparer = new TestTools.ItemListComparer(testRecordComparer);
        }

        #region SingleAction

        private static readonly object[] NotifyEventArgsTestCaseSource =
        {
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Enabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Enabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Once,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Enabled, NotifyPropertyChangeEventType.Enabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Simple,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Single,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Multi,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Once, NotifyCollectionChangeEventType.None,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Simple, NotifyCollectionChangeEventType.None,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Single, NotifyCollectionChangeEventType.None,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Multi, NotifyCollectionChangeEventType.None,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Single, NotifyCollectionChangeEventType.Once,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Multi, NotifyCollectionChangeEventType.Multi,
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
            },
        };

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void GetTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            const int getRow = 2;
            const int getColumn = 4;

            {
                var _ = instance[getRow, getColumn];
            }

            {
                var checkNotifyPropertyChange = new Action<Dictionary<string, int>>(dic =>
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                });
                checkNotifyPropertyChange(propertyChangingCountDic);
                checkNotifyPropertyChange(propertyChangedCountDic);
            }

            {
                var checkNotifyCollectionChange = new Action<NotifyCollectionChangedEventArgsDic>(dic =>
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                });
                checkNotifyCollectionChange(collectionChangingEventArgsDic);
                checkNotifyCollectionChange(collectionChangedEventArgsDic);
            }
        }

        #region Set

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int rowIdx = 3;
            const int colIdx = 5;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne,
                rowIdx, colIdx, Direction.None,
                (instance, setItems) => instance[rowIdx, colIdx] = setItems[0].First());
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int rowIdx = 3;
            const int colIdx = 0;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                rowIdx, colIdx, Direction.Row,
                (instance, setItems) => instance.SetRow(rowIdx, TestTools.MakeRowList(setItems[0])));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int rowIdx = 1;
            const int colIdx = 0;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                rowIdx, colIdx, Direction.Row,
                (instance, setItems) => instance.SetRow(rowIdx, TestTools.MakeRowList(setItems)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int rowIdx = 0;
            const int colIdx = 3;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                rowIdx, colIdx, Direction.Column,
                (instance, setItems) => instance.SetColumn(colIdx, setItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int rowIdx = 0;
            const int colIdx = 3;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                rowIdx, colIdx, Direction.Column,
                (instance, setItems) => instance.SetColumn(colIdx, setItems));
        }

        private static void SetTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            TestDoubleEnumerableInstanceType type,
            int rowIdx, int colIdx, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var setItems = MakeSetColumn(type, execDirection);
            var needTranspose = NeedTranspose(execDirection, Direction.Row);
            var rowLength = needTranspose ? setItems.GetInnerArrayLength() : setItems.Length;
            var fixedOldItems = instance.GetRow(rowIdx, rowLength)
                .ToTwoDimensionalArray();
            var fixedSetItems = setItems.ToTransposedArrayIf(execDirection == Direction.Column);
            var newItems = fixedOldItems.ToTwoDimensionalArray();
            fixedSetItems.ForEach((fixedSetRow, r) =>
                fixedSetRow.ForEach((setItem, c) => newItems[r][colIdx + c] = setItem)
            );

            actionCore(instance, TestTools.ConvertIEnumerableArray(setItems));

            Set_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic);
            Set_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                fixedOldItems, newItems, rowIdx);
        }

        private static void Set_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    Assert.AreEqual(dic.Keys.Count, 1);
                    Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void Set_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] fixedOldItems, TestRecord[][] fixedSetItems, int setIndex)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    var rowLength = fixedOldItems.Length;

                    // Replace が変更した行数だけ通知されていること
                    dic.CheckReplaceEventArgs(rowLength, eventType.IsMultipart, setIndex,
                        fixedOldItems, fixedSetItems);
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #region Add/Insert

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                TestTools.InitRowLength, Direction.Row,
                (instance, addItems) => instance.AddRow(TestTools.MakeRowList(addItems[0])));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                TestTools.InitRowLength, Direction.Row,
                (instance, addItems) => instance.AddRow(TestTools.MakeRowList(addItems)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                TestTools.InitColumnLength, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                TestTools.InitColumnLength, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                insertIndex, Direction.Row,
                (instance, insertItems) => instance.InsertRow(insertIndex, TestTools.MakeRowList(insertItems[0])));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                insertIndex, Direction.Row,
                (instance, insertItems) => instance.InsertRow(insertIndex, TestTools.MakeRowList(insertItems)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                insertIndex, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(insertIndex, insertItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                insertIndex, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(insertIndex, insertItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, 0, Direction.Row,
                (instance, addItems) => instance.AddRow(TestTools.MakeRowList(addItems[0])));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, 0, Direction.Row,
                (instance, addItems) => instance.AddRow(TestTools.MakeRowList(addItems)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, 0, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, 0, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                0, Direction.Row,
                (instance, insertItems) => instance.InsertRow(0, TestTools.MakeRowList(insertItems[0])));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                0, Direction.Row,
                (instance, insertItems) => instance.InsertRow(0, TestTools.MakeRowList(insertItems)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                0, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(0, insertItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                0, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(0, insertItems));
        }

        private static void InsertTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            bool isEmptyFrom, TestDoubleEnumerableInstanceType type,
            int insertIndex, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                isEmptyFrom,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var insertItems = MakeSetColumn(type, execDirection);
            var insertColumnLength = execDirection == Direction.Column
                ? insertItems.Length
                : insertItems.GetInnerArrayLength();
            var oldItems = instance.ToTwoDimensionalArray();

            actionCore(instance, TestTools.ConvertIEnumerableArray(insertItems));

            Insert_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                isEmptyFrom, insertColumnLength,
                propertyChangingCountDic, propertyChangedCountDic, execDirection);
            Insert_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                oldItems, isEmptyFrom, insertItems, insertIndex, execDirection);
        }

        private static void Insert_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            bool isEmptyFrom, int insertColumnLength,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic,
            Direction execDirection)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    // 通知が行われていること
                    if (execDirection == Direction.Row)
                    {
                        if (isEmptyFrom)
                        {
                            // 空リストに追加した場合
                            if (insertColumnLength != 0)
                            {
                                Assert.AreEqual(dic.Keys.Count, 5);
                                Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                            }
                            else
                            {
                                // 列数 == 0 の場合、行数のみ変化
                                Assert.AreEqual(dic.Keys.Count, 4);
                                Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                                Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                            }
                        }
                        else
                        {
                            // 元々空リストではない場合
                            Assert.AreEqual(dic.Keys.Count, 3);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                        }
                    }
                    else
                    {
                        if (isEmptyFrom)
                        {
                            // 空リストに追加した場合
                            Assert.AreEqual(dic.Keys.Count, 5);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                        }
                        else
                        {
                            // 元々空リストではない場合
                            Assert.AreEqual(dic.Keys.Count, 3);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                        }
                    }
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void Insert_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] fixedOldItems, bool isEmptyFrom,
            TestRecord[][] insertItems, int insertIndex, Direction execDirection)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    if (execDirection == Direction.Row)
                    {
                        var insertValueLength = insertItems.Length;

                        // Add が通知されていること
                        dic.CheckAddEventArgs(insertValueLength, eventType.IsMultipart,
                            insertIndex, insertItems);
                    }
                    else
                    {
                        if (isEmptyFrom)
                        {
                            // 空要素から変更された場合は Add が通知されること
                            var transposedInsertItems = insertItems.ToTransposedArray();
                            var insertValueLength = transposedInsertItems.Length;

                            // Add が通知されていること
                            dic.CheckAddEventArgs(insertValueLength, eventType.IsMultipart,
                                insertIndex, transposedInsertItems);
                        }
                        else
                        {
                            var fixedInsertItems = insertItems.ToTransposedArray();
                            var insertedItems = LinqExtension.Zip(fixedOldItems, fixedInsertItems)
                                .Select(zip =>
                                {
                                    var list = new List<TestRecord>(zip.Item1);
                                    list.InsertRange(insertIndex, zip.Item2);
                                    return list;
                                }).ToTwoDimensionalArray();
                            var rowLength = fixedOldItems.Length;
                            // Replace が通知されていること
                            dic.CheckReplaceEventArgs(rowLength, eventType.IsMultipart,
                                0, fixedOldItems, insertedItems);
                        }
                    }
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #region Move

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int oldIndex = 2;
            const int newIndex = 3;
            const int count = 1;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                oldIndex, newIndex, count, Direction.Row,
                (instance, _) => instance.MoveRow(oldIndex, newIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int oldIndex = 0;
            const int newIndex = 1;
            const int count = 2;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                oldIndex, newIndex, count, Direction.Row,
                (instance, _) => instance.MoveRow(oldIndex, newIndex, count));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int oldIndex = 2;
            const int newIndex = 3;
            const int count = 1;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                oldIndex, newIndex, count, Direction.Column,
                (instance, _) => instance.MoveColumn(oldIndex, newIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int oldIndex = 1;
            const int newIndex = 3;
            const int count = 2;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                oldIndex, newIndex, count, Direction.Column,
                (instance, _) => instance.MoveColumn(oldIndex, newIndex, count));
        }

        private static void MoveTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            int oldIndex, int newIndex, int count, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var oldItems = instance.ToTwoDimensionalArray();

            actionCore(instance, null);

            Move_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic);
            Move_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                oldItems, oldIndex, newIndex, count, execDirection);
        }

        private static void Move_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    Assert.AreEqual(dic.Keys.Count, 1);
                    Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void Move_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] oldItems,
            int oldIndex, int newIndex, int count, Direction execDirection)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    if (execDirection == Direction.Row)
                    {
                        var moveItems = oldItems.Range(oldIndex, count)
                            .ToArray();

                        // Move が通知されていること
                        dic.CheckMoveEventArgs(count, eventType.IsMultipart, oldIndex, newIndex, moveItems);
                    }
                    else
                    {
                        var movedItems = oldItems.Select(line =>
                        {
                            var list = new ExtendedList<TestRecord>(line);
                            list.MoveRange(oldIndex, newIndex, count);
                            return list.ToArray();
                        }).ToArray();

                        var rowLength = oldItems.Length;
                        // Replace が通知されていること
                        dic.CheckReplaceEventArgs(rowLength, eventType.IsMultipart,
                            0, oldItems, movedItems);
                    }
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #region Remove

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int removeIndex = 3;
            const int count = 1;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                removeIndex, count, Direction.Row,
                (instance, _) => instance.RemoveRow(removeIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int removeIndex = 1;
            const int count = 2;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                removeIndex, count, Direction.Row,
                (instance, _) => instance.RemoveRow(removeIndex, count));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int removeIndex = 3;
            const int count = 1;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                removeIndex, count, Direction.Column,
                (instance, _) => instance.RemoveColumn(removeIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int removeIndex = 1;
            const int count = 2;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                removeIndex, count, Direction.Column,
                (instance, _) => instance.RemoveColumn(removeIndex, count));
        }

        private static void RemoveTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            int removeIndex, int count, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var target = instance.ToTwoDimensionalArray();

            actionCore(instance, null);

            var isEmptyAfter = execDirection == Direction.Row
                               && removeIndex == 0
                               && count == TestTools.InitRowLength;

            Remove_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                isEmptyAfter,
                propertyChangingCountDic, propertyChangedCountDic, execDirection);
            Remove_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                target, removeIndex, count, execDirection);
        }

        private static void Remove_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            bool isEmptyFromNotEmpty,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic,
            Direction execDirection)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    // 通知が行われていること
                    if (execDirection == Direction.Row)
                    {
                        if (isEmptyFromNotEmpty)
                        {
                            Assert.AreEqual(dic.Keys.Count, 4);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                        }
                        else
                        {
                            Assert.AreEqual(dic.Keys.Count, 3);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                        }
                    }
                    else
                    {
                        if (isEmptyFromNotEmpty)
                        {
                            Assert.AreEqual(dic.Keys.Count, 4);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                        }
                        else
                        {
                            Assert.AreEqual(dic.Keys.Count, 3);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                        }
                    }
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void Remove_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] target,
            int index, int count, Direction execDirection)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    if (execDirection == Direction.Row)
                    {
                        var removeItems = target.Range(index, count).ToArray();

                        // Remove が通知されていること
                        dic.CheckRemoveEventArgs(count, eventType.IsMultipart, index, removeItems);
                    }
                    else
                    {
                        var removedItems = target
                            .Select(line =>
                            {
                                var list = new List<TestRecord>(line);
                                list.RemoveRange(index, count);
                                return list;
                            }).ToTwoDimensionalArray();
                        var rowLength = removedItems.Length;

                        // Replace が通知されていること
                        dic.CheckReplaceEventArgs(rowLength, eventType.IsMultipart,
                            0, target, removedItems);
                    }
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #region Reset

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void ResetTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var resetItems = MakeSetColumn(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, Direction.Row);

            ResetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                resetItems,
                (instance, _) => instance.Reset(TestTools.MakeRowList(resetItems)));
        }

        private static void ResetTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            TestRecord[][] resetItems, ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var target = instance.ToTwoDimensionalArray();

            actionCore(instance, null);

            Reset_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic);
            Reset_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                target, resetItems, 0);
        }

        private static void Reset_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    // 通知が行われていること
                    Assert.AreEqual(dic.Keys.Count, 4);
                    Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                    Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                    Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                    Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void Reset_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] target, TestRecord[][] resetItems,
            int startIndex)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    // Reset が通知されていること
                    dic.CheckResetEventArgs(startIndex, target, resetItems);
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #endregion

        #region MultiAction

        #region Overwrite

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteRow_ReplaceTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int startIndex = TestTools.InitRowLength - 2;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                startIndex, Direction.Row,
                (instance, items) => instance.OverwriteRow(startIndex, TestTools.MakeRowList(items)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteRow_AddTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int startIndex = TestTools.InitRowLength;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                startIndex, Direction.Row,
                (instance, items) => instance.OverwriteRow(startIndex, TestTools.MakeRowList(items)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteRow_BothTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int startIndex = TestTools.InitRowLength - 1;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                startIndex, Direction.Row,
                (instance, items) => instance.OverwriteRow(startIndex, TestTools.MakeRowList(items)));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteColumn_ReplaceTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int startIndex = TestTools.InitColumnLength - 2;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                startIndex, Direction.Column,
                (instance, items) => instance.OverwriteColumn(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteColumn_AddTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int startIndex = TestTools.InitColumnLength;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                startIndex, Direction.Column,
                (instance, items) => instance.OverwriteColumn(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteColumn_BothTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int startIndex = TestTools.InitColumnLength - 1;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                startIndex, Direction.Column,
                (instance, items) => instance.OverwriteColumn(startIndex, items));
        }

        private static void OverwriteTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            TestDoubleEnumerableInstanceType type, int startIndex,
            Direction execDirection, ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var overwriteItems = MakeSetColumn(type, execDirection);
            var overwriteOuterLength = overwriteItems.Length;
            var overwriteInnerLength = overwriteItems.GetInnerArrayLength();
            var overwriteRowLength = execDirection != Direction.Column ? overwriteOuterLength : overwriteInnerLength;
            var overwriteColumnLength = execDirection != Direction.Column ? overwriteInnerLength : overwriteOuterLength;

            var target = instance.ToTwoDimensionalArray();
            var beforeTargetLength = target.Length;
            var isNotEmptyFromEmpty = beforeTargetLength == 0 && overwriteRowLength > 0;

            actionCore(instance, TestTools.ConvertIEnumerableArray(overwriteItems));

            var isChangeRowLength = target.Length < overwriteRowLength
                + (execDirection != Direction.Column ? startIndex : 0);
            var isChangeColumnLength = target.GetInnerArrayLength() < overwriteColumnLength
                + (execDirection == Direction.Column ? startIndex : 0);

            Overwrite_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                isNotEmptyFromEmpty,
                propertyChangingCountDic, propertyChangedCountDic,
                isChangeRowLength, isChangeColumnLength);
            Overwrite_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                target, overwriteItems, startIndex, execDirection);
        }

        private static void Overwrite_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            bool isNotEmptyFromEmpty,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic,
            bool isChangeRowLength, bool isChangeColumnLength)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    // 通知が行われていること
                    var count = 1;
                    if (isChangeRowLength)
                    {
                        count += 1;
                        Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                    }

                    if (isChangeColumnLength)
                    {
                        count += 1;
                        Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                    }

                    if (isChangeRowLength || isChangeColumnLength)
                    {
                        count += 1;
                        Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                    }

                    if (isNotEmptyFromEmpty)
                    {
                        count += 1;
                        Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                    }

                    Assert.AreEqual(dic.Keys.Count, count);
                    Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void Overwrite_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] target, TestRecord[][] overwriteItems,
            int startIndex, Direction execDirection)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    var needTranspose = NeedTranspose(execDirection, Direction.Row);

                    var replaceLength = Math.Min(target.Length - startIndex, overwriteItems.Length);
                    var addLength = overwriteItems.Length - replaceLength;

                    var fixedOverwriteItems = overwriteItems.ToTransposedArrayIf(needTranspose);

                    var setItems = fixedOverwriteItems.Take(replaceLength).ToArray();
                    var addItems = fixedOverwriteItems.Skip(replaceLength).ToArray();

                    var isReplaced = replaceLength > 0;
                    var isAdded = addLength > 0;

                    if (execDirection == Direction.Row)
                    {
                        if (isReplaced && isAdded)
                        {
                            if (eventType == NotifyCollectionChangeEventType.Once
                                || eventType == NotifyCollectionChangeEventType.Simple)
                            {
                                var oldItems = target.Skip(startIndex).Take(replaceLength).ToArray();
                                // Reset が一度通知されていること
                                dic.CheckResetEventArgs(startIndex, oldItems, fixedOverwriteItems);
                            }
                            else
                            {
                                var replaceOldItems = target.Skip(startIndex).ToArray();
                                var addIndex = startIndex + replaceLength;

                                // Add, Replace が通知されていること
                                dic.CheckOnlyReplaceEventArgs(replaceLength, eventType.IsMultipart, startIndex,
                                    replaceOldItems,
                                    setItems);
                                dic.CheckOnlyAddEventArgs(addLength, eventType.IsMultipart, addIndex, addItems);
                                Assert.IsTrue(dic.IsMoveEventEmpty);
                                Assert.IsTrue(dic.IsRemoveEventEmpty);
                                Assert.IsTrue(dic.IsResetEventEmpty);
                            }
                        }
                        else if (isReplaced)
                        {
                            var replaceOldItems = target.Skip(startIndex)
                                .Take(overwriteItems.Length)
                                .ToArray();

                            // Replace が通知されていること
                            dic.CheckReplaceEventArgs(replaceLength, eventType.IsMultipart, startIndex, replaceOldItems,
                                setItems);
                        }
                        else
                        {
                            // isAdded == true
                            var addIndex = startIndex + replaceLength;

                            // Add が通知されていること
                            dic.CheckAddEventArgs(addLength, eventType.IsMultipart, addIndex, addItems);
                        }
                    }
                    else
                    {
                        // execDirection == Column
                        var fixedReplacedItems = target.Select((line, idx) =>
                        {
                            var list = new SimpleList<TestRecord>(line);
                            var overwriteParam =
                                OverwriteParam<TestRecord>.Factory.Create(list, startIndex, fixedOverwriteItems[idx]);
                            list.Overwrite(startIndex, overwriteParam);
                            return list.ToArray();
                        }).ToTwoDimensionalArray();

                        // Replace が通知されること
                        dic.CheckReplaceEventArgs(target.Length, eventType.IsMultipart, 0, target,
                            fixedReplacedItems);
                    }
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #region Adjust

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testRowLengths = new[]
            {
                TestTools.InitRowLength - 1,
                TestTools.InitRowLength,
                TestTools.InitRowLength + 1,
            };
            var testColumnLength = new[]
            {
                TestTools.InitColumnLength - 1,
                TestTools.InitColumnLength,
                TestTools.InitColumnLength + 1
            };
            var testLengths = testRowLengths.SelectMany(rowLength =>
                testColumnLength.Select(columnLength => (rowLength, columnLength)));

            testLengths.ForEach((lengthPair, _) =>
            {
                var (rowLength, columnLength) = lengthPair;

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    rowLength, columnLength,
                    (instance, _) => instance.AdjustLength(rowLength, columnLength));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthIfLongTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testRowLengths = new[]
            {
                TestTools.InitRowLength - 1,
                TestTools.InitRowLength,
                TestTools.InitRowLength + 1,
            };
            var testColumnLength = new[]
            {
                TestTools.InitColumnLength - 1,
                TestTools.InitColumnLength,
                TestTools.InitColumnLength + 1
            };
            var testLengths = testRowLengths.SelectMany(rowLength =>
                testColumnLength.Select(columnLength => (rowLength, columnLength)));

            testLengths.ForEach((lengthPair, _) =>
            {
                var (rowLength, columnLength) = lengthPair;
                var resultRowLength = Math.Min(rowLength, TestTools.InitRowLength);
                var resultColumnLength = Math.Min(columnLength, TestTools.InitColumnLength);

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    resultRowLength, resultColumnLength,
                    (instance, _) => instance.AdjustLengthIfLong(rowLength, columnLength));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthIfShortTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testRowLengths = new[]
            {
                TestTools.InitRowLength - 1,
                TestTools.InitRowLength,
                TestTools.InitRowLength + 1,
            };
            var testColumnLength = new[]
            {
                TestTools.InitColumnLength - 1,
                TestTools.InitColumnLength,
                TestTools.InitColumnLength + 1
            };
            var testLengths = testRowLengths.SelectMany(rowLength =>
                testColumnLength.Select(columnLength => (rowLength, columnLength)));

            testLengths.ForEach((lengthPair, _) =>
            {
                var (rowLength, columnLength) = lengthPair;
                var resultRowLength = Math.Max(rowLength, TestTools.InitRowLength);
                var resultColumnLength = Math.Max(columnLength, TestTools.InitColumnLength);

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    resultRowLength, resultColumnLength,
                    (instance, _) => instance.AdjustLengthIfShort(rowLength, columnLength));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int length = 3;

            AdjustLengthTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                length, TestTools.InitColumnLength,
                (instance, _) => instance.AdjustRowLength(length));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthRowIfLongTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testLengths = new[]
            {
                TestTools.InitRowLength - 1,
                TestTools.InitRowLength,
                TestTools.InitRowLength + 1,
            };

            testLengths.ForEach(length =>
            {
                var resultLength = Math.Min(length, TestTools.InitRowLength);

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    resultLength, TestTools.InitColumnLength,
                    (instance, _) => instance.AdjustRowLengthIfLong(length));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthRowIfShortTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testLengths = new[]
            {
                TestTools.InitRowLength - 1,
                TestTools.InitRowLength,
                TestTools.InitRowLength + 1,
            };

            testLengths.ForEach(length =>
            {
                var resultLength = Math.Max(length, TestTools.InitRowLength);

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    resultLength, TestTools.InitColumnLength,
                    (instance, _) => instance.AdjustRowLengthIfShort(length));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            const int length = 3;

            AdjustLengthTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                TestTools.InitRowLength, length,
                (instance, _) => instance.AdjustColumnLength(length));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthColumnIfLongTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testLengths = new[]
            {
                TestTools.InitColumnLength - 1,
                TestTools.InitColumnLength,
                TestTools.InitColumnLength + 1,
            };

            testLengths.ForEach(length =>
            {
                var resultLength = Math.Min(length, TestTools.InitColumnLength);

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    TestTools.InitRowLength, resultLength,
                    (instance, _) => instance.AdjustColumnLengthIfLong(length));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthColumnIfShortTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType)
        {
            var testLengths = new[]
            {
                TestTools.InitColumnLength - 1,
                TestTools.InitColumnLength,
                TestTools.InitColumnLength + 1,
            };

            testLengths.ForEach(length =>
            {
                var resultLength = Math.Max(length, TestTools.InitColumnLength);

                AdjustLengthTestCore(
                    notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                    collectionChangingEventType, collectionChangedEventType,
                    TestTools.InitRowLength, resultLength,
                    (instance, _) => instance.AdjustColumnLengthIfShort(length));
            });
        }

        private static void AdjustLengthTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            int adjustRowLength, int adjustColumnLength,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic);

            var oldTarget = instance.ToTwoDimensionalArray();

            actionCore(instance, null);
            var target = instance.ToTwoDimensionalArray();

            AdjustLength_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic, oldTarget, adjustRowLength,
                adjustColumnLength);
            AdjustLength_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                oldTarget, target, adjustRowLength, adjustColumnLength);
        }

        private static void AdjustLength_CheckNotify_PropertyChangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            Dictionary<string, int> propertyChangingCountDic,
            Dictionary<string, int> propertyChangedCountDic,
            TestRecord[][] oldItems, int adjustRowLength, int adjustColumnLength)
        {
            var checkNotifyPropertyChange = new Action<NotifyPropertyChangeEventType,
                Dictionary<string, int>>((eventType, dic) =>
            {
                if (eventType == NotifyPropertyChangeEventType.Enabled)
                {
                    var oldRowLength = oldItems.Length;
                    var oldColumnLength = oldItems.GetInnerArrayLength();

                    var isChangeRow = adjustRowLength != oldRowLength;
                    var isChangeColumn = adjustColumnLength != oldColumnLength;
                    var isEmptyBefore = oldRowLength == 0;
                    var isEmptyAfter = adjustRowLength == 0;
                    var isChangeEmptyState = isEmptyBefore != isEmptyAfter;

                    // 通知が行われていること
                    switch (isChangeRow, isChangeEmptyState, isChangeColumn)
                    {
                        case (true, true, true):
                            Assert.AreEqual(dic.Keys.Count, 5);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                            break;
                        case (true, false, true):
                            Assert.AreEqual(dic.Keys.Count, 4);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            break;
                        case (true, true, false):
                            Assert.AreEqual(dic.Keys.Count, 4);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.IsEmpty)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            break;
                        case (true, false, false):
                            Assert.AreEqual(dic.Keys.Count, 3);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.RowCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            break;
                        case (false, _, true):
                            Assert.AreEqual(dic.Keys.Count, 3);
                            Assert.AreEqual(dic[ListConstant.IndexerName], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.ColumnCount)], 1);
                            Assert.AreEqual(dic[nameof(TestTwoDimList.AllCount)], 1);
                            break;
                        case (false, _, false):
                            Assert.AreEqual(dic.Keys.Count, 0);
                            break;
                    }
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.AreEqual(dic.Keys.Count, 0);
                }
                else
                {
                    Assert.Fail();
                }
            });
            checkNotifyPropertyChange(notifyPropertyChangingEventType, propertyChangingCountDic);
            checkNotifyPropertyChange(notifyPropertyChangedEventType, propertyChangedCountDic);
        }

        private static void AdjustLength_CheckNotify_CollectionChangeTest(
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyCollectionChangedEventArgsDic collectionChangingEventArgsDic,
            NotifyCollectionChangedEventArgsDic collectionChangedEventArgsDic,
            TestRecord[][] oldTarget, TestRecord[][] target,
            int adjustRowLength, int adjustColumnLength)
        {
            var checkNotifyCollectionChange = new Action<NotifyCollectionChangeEventType,
                NotifyCollectionChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyCollectionChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                    return;
                }

                var oldRowLength = oldTarget.Length;
                var oldColumnLength = oldTarget.GetInnerArrayLength();

                // Add / Remove
                var rowManipulationType = DetermineListManipulationType(oldRowLength, adjustRowLength);
                var columnManipulationType = DetermineListManipulationType(oldColumnLength, adjustColumnLength);

                if (rowManipulationType == ListManipulationType.None &&
                    columnManipulationType == ListManipulationType.None)
                {
                    // 行数・列数が変化していない場合
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                    return;
                }

                var isChangeOneSide = rowManipulationType == ListManipulationType.None ^
                                      columnManipulationType == ListManipulationType.None;

                if (isChangeOneSide)
                {
                    // 片方のアクション（行のAdd/Remove or 列のReplace）しか起こっていない場合、
                    // 行変化と列変化それぞれ個別にコレクション変更通知が行われること

                    switch (rowManipulationType)
                    {
                        case ListManipulationType.Add:
                        {
                            {
                                // Add
                                var addItemLength = adjustRowLength - oldRowLength;
                                var addItems = target.Range(oldRowLength, addItemLength).ToArray();

                                dic.CheckOnlyAddEventArgs(addItemLength, eventType.IsMultipart, oldRowLength, addItems);
                            }

                            Assert.IsTrue(dic.IsRemoveEventEmpty);
                            break;
                        }

                        case ListManipulationType.None:
                        {
                            Assert.IsTrue(dic.IsAddEventEmpty);
                            Assert.IsTrue(dic.IsRemoveEventEmpty);
                            break;
                        }

                        case ListManipulationType.Remove:
                        {
                            {
                                // Remove
                                var removeItemLength = oldRowLength - adjustRowLength;
                                var removeItems = oldTarget.Range(adjustRowLength, removeItemLength).ToArray();

                                // Remove が通知されていること
                                dic.CheckOnlyRemoveEventArgs(removeItemLength, eventType.IsMultipart, adjustRowLength,
                                    removeItems);
                            }

                            Assert.IsTrue(dic.IsAddEventEmpty);
                            break;
                        }
                        default:
                            Assert.Fail();
                            break;
                    }

                    switch (columnManipulationType)
                    {
                        case ListManipulationType.Add:
                        case ListManipulationType.Remove:
                        {
                            var replaceLength = Math.Min(oldRowLength, adjustRowLength);
                            var replaceOldItems = oldTarget.Range(0, replaceLength)
                                .ToArray();
                            var replaceNewItems = target.Range(0, replaceLength)
                                .ToArray();

                            // Replace / Add が通知されていること
                            dic.CheckOnlyReplaceEventArgs(replaceLength, eventType.IsMultipart, 0, replaceOldItems,
                                replaceNewItems);
                            break;
                        }
                        case ListManipulationType.None:
                        {
                            Assert.IsTrue(dic.IsReplaceEventEmpty);
                            break;
                        }
                        default:
                            Assert.Fail();
                            break;
                    }

                    Assert.IsTrue(dic.IsMoveEventEmpty);
                    Assert.IsTrue(dic.IsResetEventEmpty);
                }
                else
                {
                    // 両方のアクション(行のAdd/Remove & 列のReplace)が起きている場合
                    // Reset通知が一度だけ行われること
                    dic.CheckResetEventArgs(0, oldTarget, target);
                }
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        #endregion

        #endregion

        #region InnerValueChange

        [TestCase(true, true, 1, 1)]
        [TestCase(true, false, 1, 0)]
        [TestCase(false, true, 0, 1)]
        [TestCase(false, false, 0, 0)]
        public static void NotifyRowItemChangeTest(
            bool isAddPropertyChangingEventHandler, bool isAddPropertyChangedEventHandler,
            int actualChangingNotifyCount, int actualChangedNotifyCount
        )
        {
            var instance = CreateNotifyChangeTestInstance();

            // イベント登録
            var notifyChangingProperties = new List<string>();
            var notifyChangedProperties = new List<string>();

            var propertyChangingEventHandler = CreatePropertyChangingEventHandler(notifyChangingProperties);
            var propertyChangedEventHandler = CreatePropertyChangedEventHandler(notifyChangedProperties);

            if (isAddPropertyChangingEventHandler)
            {
                instance.PropertyChanging += propertyChangingEventHandler;
            }

            if (isAddPropertyChangedEventHandler)
            {
                instance.PropertyChanged += propertyChangedEventHandler;
            }

            // 事前確認 PropertyChange が発火していないこと
            Assert.AreEqual(notifyChangingProperties.Count, 0);
            Assert.AreEqual(notifyChangedProperties.Count, 0);

            // 行データプロパティ変更
            instance.GetRow(1).TestValue = "updated";

            // PropertyChange が意図したとおり発火すること
            Assert.AreEqual(notifyChangingProperties.Count, actualChangingNotifyCount);
            Assert.AreEqual(notifyChangedProperties.Count, actualChangedNotifyCount);
        }

        [TestCase(true, true, 1, 1)]
        [TestCase(true, false, 1, 0)]
        [TestCase(false, true, 0, 1)]
        [TestCase(false, false, 0, 0)]
        public static void NotifyTableItemChangeTest(
            bool isAddPropertyChangingEventHandler, bool isAddPropertyChangedEventHandler,
            int actualChangingNotifyCount, int actualChangedNotifyCount
        )
        {
            var instance = CreateNotifyChangeTestInstance();

            // イベント登録
            var notifyChangingProperties = new List<string>();
            var notifyChangedProperties = new List<string>();

            var propertyChangingEventHandler = CreatePropertyChangingEventHandler(notifyChangingProperties);
            var propertyChangedEventHandler = CreatePropertyChangedEventHandler(notifyChangedProperties);

            if (isAddPropertyChangingEventHandler)
            {
                instance.PropertyChanging += propertyChangingEventHandler;
            }

            if (isAddPropertyChangedEventHandler)
            {
                instance.PropertyChanged += propertyChangedEventHandler;
            }

            // 事前確認 PropertyChange が発火していないこと
            Assert.AreEqual(notifyChangingProperties.Count, 0);
            Assert.AreEqual(notifyChangedProperties.Count, 0);

            // テーブルデータプロパティ変更
            instance.GetRow(1)[2].GrandchildValue = "updated";

            // PropertyChange が意図したとおり発火すること
            Assert.AreEqual(notifyChangingProperties.Count, actualChangingNotifyCount);
            Assert.AreEqual(notifyChangedProperties.Count, actualChangedNotifyCount);
        }

        private static TwoDimensionalList<TestRecordListForNotifyFromChild,
                TestRecordListForNotifyFromChild, NotifiableObject>
            CreateNotifyChangeTestInstance()
        {
            var rows = Enumerable.Range(0, 3)
                .Select(_ => Enumerable.Range(0, 4).Select(_ => new NotifiableObject())
                );

            var config =
                new TwoDimensionalList<TestRecordListForNotifyFromChild, TestRecordListForNotifyFromChild,
                    NotifiableObject>.Config(
                    items => new TestRecordListForNotifyFromChild(items),
                    row => new TestRecordListForNotifyFromChild(row),
                    (_, _) => new NotifiableObject(),
                    (l, r) => l == r,
                    _ => null
                );

            var instance = new TwoDimensionalList<TestRecordListForNotifyFromChild,
                TestRecordListForNotifyFromChild, NotifiableObject>(rows, config)
            {
                NotifyPropertyChangingEventType = NotifyPropertyChangeEventType.Enabled,
                NotifyPropertyChangedEventType = NotifyPropertyChangeEventType.Enabled,
                NotifyCollectionChangingEventType = NotifyCollectionChangeEventType.Multi,
                NotifyCollectionChangedEventType = NotifyCollectionChangeEventType.Multi
            };

            return instance;
        }

        private static PropertyChangingEventHandler CreatePropertyChangingEventHandler(
            ICollection<string> notifyChangingProperties)
            => (_, args) => notifyChangingProperties.Add(args.PropertyName);

        private static PropertyChangedEventHandler CreatePropertyChangedEventHandler(
            ICollection<string> notifyChangedProperties)
            => (_, args) => notifyChangedProperties.Add(args.PropertyName);

        private class TestRecordListForNotifyFromChild : ExtendedList<NotifiableObject>,
            IDeepCloneable<TestRecordListForNotifyFromChild>
        {
            public string TestValue
            {
                set
                {
                    NotifyPropertyChanging();
                    var _ = value;
                    NotifyPropertyChanged();
                }
            }

            public TestRecordListForNotifyFromChild(IEnumerable<NotifiableObject> items) : base(items)
            {
            }

            public new TestRecordListForNotifyFromChild DeepClone() => new(this);
        }

        private class NotifiableObject : ModelBase<NotifiableObject>
        {
            public string GrandchildValue
            {
                set
                {
                    NotifyPropertyChanging();
                    var _ = value;
                    NotifyPropertyChanged();
                }
            }

            public override bool ItemEquals(NotifiableObject other)
            {
                // not use
                throw new InvalidOperationException();
            }

            public override NotifiableObject DeepClone() => new();
        }

        #endregion

        #region TestTools

        private static TestRecord[][] MakeSetColumn(TestDoubleEnumerableInstanceType type, Direction execDirection)
        {
            return TestTools.MakeTestRecordList(type, execDirection == Direction.Column, TestTools.MakeInsertItem)
                ?.ToTwoDimensionalArray();
        }

        private static TestTwoDimList MakeList(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType notifyCollectionChangingEventType,
            NotifyCollectionChangeEventType notifyCollectionChangedEventType,
            bool isEmptyInstance,
            out Dictionary<string, int> propertyChangingCountDic,
            out Dictionary<string, int> propertyChangedCountDic,
            out NotifyCollectionChangedEventArgsDic collectionChangingEventArgsList,
            out NotifyCollectionChangedEventArgsDic collectionChangedEventArgsList)
        {
            var result = TestTools.MakeTwoDimensionalList(
                isEmptyInstance
                    ? TestDoubleEnumerableInstanceType.Empty
                    : TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnBasic,
                TestTools.MakeListDefaultItem);

            result.NotifyPropertyChangingEventType = notifyPropertyChangingEventType;
            result.NotifyPropertyChangedEventType = notifyPropertyChangedEventType;
            result.NotifyCollectionChangingEventType = notifyCollectionChangingEventType;
            result.NotifyCollectionChangedEventType = notifyCollectionChangedEventType;

            propertyChangingCountDic = new Dictionary<string, int>();
            result.PropertyChanging += MakePropertyChangingEventHandler(propertyChangingCountDic);

            propertyChangedCountDic = new Dictionary<string, int>();
            result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedCountDic);

            collectionChangingEventArgsList = new NotifyCollectionChangedEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = new NotifyCollectionChangedEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            return result;
        }

        private static PropertyChangingEventHandler MakePropertyChangingEventHandler(
            IDictionary<string, int> propertyCountDic)
            => (_, args) =>
            {
                var propName = args.PropertyName;
                if (propertyCountDic.ContainsKey(propName))
                {
                    propertyCountDic[propName]++;
                }
                else
                {
                    propertyCountDic[propName] = 1;
                }

                logger.Debug($"PropertyChanging Event Raise. ");
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                logger.Debug("}");
            };

        private static PropertyChangedEventHandler MakePropertyChangedEventHandler(
            IDictionary<string, int> propertyCountDic)
            => (_, args) =>
            {
                var propName = args.PropertyName;
                if (propertyCountDic.ContainsKey(propName))
                {
                    propertyCountDic[propName]++;
                }
                else
                {
                    propertyCountDic[propName] = 1;
                }

                logger.Debug($"PropertyChanged Event Raise. ");
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.PropertyName)}: {args.PropertyName}");
                logger.Debug("}");
            };

        private static EventHandler<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>
            MakeCollectionChangeEventHandler(bool isBefore,
                NotifyCollectionChangedEventArgsDic resultDic)
            => (_, args) =>
            {
                resultDic.Add(args);
                logger.Debug($"Collection{(isBefore ? "Changing" : "Changed")} Event Raise. ");
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.Action)}: {args.Action}");
                logger.Debug($"    {nameof(args.OldStartingIndex)}: {args.OldStartingIndex}");
                logger.Debug($"    {nameof(args.OldItems)}: {args.OldItems}");
                logger.Debug($"    {nameof(args.NewStartingIndex)}: {args.NewStartingIndex}");
                logger.Debug($"    {nameof(args.NewItems)}: {args.NewItems}");
                logger.Debug("}");
            };

        private static bool NeedTranspose(Direction direction1, Direction direction2)
            => (direction1 == Direction.Row) ^ (direction2 == Direction.Row);

        private static ListManipulationType DetermineListManipulationType(int oldLength, int newLength)
        {
            if (newLength == oldLength) return ListManipulationType.None;

            return newLength > oldLength
                ? ListManipulationType.Add
                : ListManipulationType.Remove;
        }

        private delegate void ActionCore(TestTwoDimList instance,
            IEnumerable<TestRecord>[] operationItems);

        private class NotifyCollectionChangedEventArgsDic
        {
            private Dictionary<string, List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>> Impl
            {
                get;
            }

            public NotifyCollectionChangedEventArgsDic()
            {
                Impl = new Dictionary<string, List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>>();
                Clear();
            }

            public void Add(params NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>[] args)
            {
                args.ForEach(arg => { Impl[arg.Action.ToString()].Add(arg); });
            }

            #region 判定用

            /// <summary>
            ///     いずれのイベント引数も格納されていない場合 true
            /// </summary>
            public bool IsEmpty => Impl.All(x => x.Value.Count == 0);

            public bool IsReplaceEventEmpty => ReplaceEventArgs.Count == 0;
            public bool IsAddEventEmpty => AddEventArgs.Count == 0;
            public bool IsMoveEventEmpty => MoveEventArgs.Count == 0;
            public bool IsRemoveEventEmpty => RemoveEventArgs.Count == 0;
            public bool IsResetEventEmpty => ResetEventArgs.Count == 0;

            public void CheckReplaceEventArgs(int argsLength, bool isMultipart,
                int setIndex, TestRecord[][] oldItems, TestRecord[][] setItems)
            {
                CheckOnlyReplaceEventArgs(argsLength, isMultipart, setIndex, oldItems, setItems);
                Assert.AreEqual(AddEventArgs.Count, 0);
                Assert.AreEqual(MoveEventArgs.Count, 0);
                Assert.AreEqual(RemoveEventArgs.Count, 0);
                Assert.AreEqual(ResetEventArgs.Count, 0);
            }

            public void CheckOnlyReplaceEventArgs(int argsLength, bool isMultipart,
                int setIndex, TestRecord[][] oldItems, TestRecord[][] setItems)
            {
                if (isMultipart)
                {
                    // Replace が複数回通知されていること
                    Assert.AreEqual(ReplaceEventArgs.Count, argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = ReplaceEventArgs[i];
                        Assert.AreEqual(arg.OldStartingIndex, setIndex + i);
                        Assert.AreEqual(arg.OldItems!.Count, 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(oldItems[i], testRecordComparer));
                        Assert.AreEqual(arg.NewStartingIndex, setIndex + i);
                        Assert.AreEqual(arg.NewItems!.Count, 1);
                        Assert.IsTrue(arg.NewItems[0].SequenceEqual(setItems[i], testRecordComparer));
                    }
                }
                else
                {
                    // Replace が1度通知されていること
                    Assert.AreEqual(ReplaceEventArgs.Count, 1);
                    {
                        var arg = ReplaceEventArgs[0];
                        Assert.AreEqual(arg.OldStartingIndex, setIndex);
                        Assert.AreEqual(arg.OldItems!.Count, argsLength);
                        Assert.IsTrue(arg.OldItems.ItemEquals(oldItems, itemListComparer));
                        Assert.AreEqual(arg.NewStartingIndex, setIndex);
                        Assert.AreEqual(arg.NewItems!.Count, argsLength);
                        Assert.IsTrue(arg.NewItems.ItemEquals(setItems, itemListComparer));
                    }
                }
            }

            public void CheckAddEventArgs(int argsLength, bool isMultipart,
                int insertIndex, TestRecord[][] insertItems)
            {
                CheckOnlyAddEventArgs(argsLength, isMultipart, insertIndex, insertItems);
                Assert.AreEqual(ReplaceEventArgs.Count, 0);
                Assert.AreEqual(MoveEventArgs.Count, 0);
                Assert.AreEqual(RemoveEventArgs.Count, 0);
                Assert.AreEqual(ResetEventArgs.Count, 0);
            }

            public void CheckOnlyAddEventArgs(int argsLength, bool isMultipart,
                int insertIndex, TestRecord[][] insertItems)
            {
                if (isMultipart)
                {
                    // Add が複数回通知されていること
                    Assert.AreEqual(AddEventArgs.Count, argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = AddEventArgs[i];
                        Assert.AreEqual(arg.OldStartingIndex, -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.AreEqual(arg.NewStartingIndex, insertIndex + i);
                        Assert.AreEqual(arg.NewItems!.Count, 1);
                        Assert.IsTrue(arg.NewItems[0].SequenceEqual(insertItems[i], testRecordComparer));
                    }
                }
                else
                {
                    // Add が一度通知されていること
                    Assert.AreEqual(AddEventArgs.Count, 1);
                    {
                        var arg = AddEventArgs[0];
                        Assert.AreEqual(arg.OldStartingIndex, -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.AreEqual(arg.NewStartingIndex, insertIndex);
                        Assert.AreEqual(arg.NewItems!.Count, argsLength);
                        Assert.IsTrue(arg.NewItems.ItemEquals(insertItems, itemListComparer));
                    }
                }
            }

            public void CheckOnlyRemoveEventArgs(int argsLength, bool isMultipart,
                int removeIndex, TestRecord[][] removeItems)
            {
                if (isMultipart)
                {
                    // Remove が複数回通知されていること
                    Assert.AreEqual(RemoveEventArgs.Count, argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = RemoveEventArgs[i];
                        Assert.AreEqual(arg.OldStartingIndex, removeIndex + i);
                        Assert.AreEqual(arg.OldItems!.Count, 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(removeItems[i], testRecordComparer));
                        Assert.AreEqual(arg.NewStartingIndex, -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }
                }
                else
                {
                    // Remove が一度通知されていること
                    Assert.AreEqual(RemoveEventArgs.Count, 1);
                    {
                        var arg = RemoveEventArgs[0];
                        Assert.AreEqual(arg.OldStartingIndex, removeIndex);
                        Assert.AreEqual(arg.OldItems!.Count, argsLength);
                        Assert.IsTrue(arg.OldItems.ItemEquals(removeItems, itemListComparer));
                        Assert.AreEqual(arg.NewStartingIndex, -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }
                }
            }

            public void CheckMoveEventArgs(int argsLenght, bool isMultipart,
                int oldIndex, int newIndex, TestRecord[][] moveItems)
            {
                if (isMultipart)
                {
                    Assert.AreEqual(MoveEventArgs.Count, argsLenght);
                    for (var i = 0; i < argsLenght; i++)
                    {
                        var arg = MoveEventArgs[i];
                        Assert.AreEqual(arg.OldStartingIndex, oldIndex + i);
                        Assert.AreEqual(arg.OldItems!.Count, 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(moveItems[i], testRecordComparer));
                        Assert.AreEqual(arg.NewStartingIndex, newIndex + i);
                        Assert.AreEqual(arg.NewItems!.Count, 1);
                        Assert.IsTrue(arg.NewItems[0].SequenceEqual(moveItems[i], testRecordComparer));
                    }

                    Assert.AreEqual(ReplaceEventArgs.Count, 0);
                    Assert.AreEqual(AddEventArgs.Count, 0);
                    Assert.AreEqual(RemoveEventArgs.Count, 0);
                    Assert.AreEqual(ResetEventArgs.Count, 0);
                }
                else
                {
                    Assert.AreEqual(MoveEventArgs.Count, 1);
                    {
                        var arg = MoveEventArgs[0];
                        Assert.AreEqual(arg.OldStartingIndex, oldIndex);
                        Assert.AreEqual(arg.OldItems!.Count, argsLenght);
                        Assert.IsTrue(arg.OldItems.ItemEquals(moveItems, itemListComparer));
                        Assert.AreEqual(arg.NewStartingIndex, newIndex);
                        Assert.AreEqual(arg.NewItems!.Count, argsLenght);
                        Assert.IsTrue(arg.NewItems.ItemEquals(moveItems, itemListComparer));
                    }
                    Assert.AreEqual(ReplaceEventArgs.Count, 0);
                    Assert.AreEqual(AddEventArgs.Count, 0);
                    Assert.AreEqual(RemoveEventArgs.Count, 0);
                    Assert.AreEqual(ResetEventArgs.Count, 0);
                }
            }

            public void CheckRemoveEventArgs(int argsLength, bool isMultipart,
                int removeIndex, TestRecord[][] removeItems)
            {
                if (isMultipart)
                {
                    // Remove が複数回通知されていること
                    Assert.AreEqual(RemoveEventArgs.Count, argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = RemoveEventArgs[i];
                        Assert.AreEqual(arg.OldStartingIndex, removeIndex + i);
                        Assert.AreEqual(arg.OldItems!.Count, 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(removeItems[i], testRecordComparer));
                        Assert.AreEqual(arg.NewStartingIndex, -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }

                    Assert.AreEqual(ReplaceEventArgs.Count, 0);
                    Assert.AreEqual(AddEventArgs.Count, 0);
                    Assert.AreEqual(MoveEventArgs.Count, 0);
                    Assert.AreEqual(ResetEventArgs.Count, 0);
                }
                else
                {
                    // Remove が一度通知されていること
                    Assert.AreEqual(RemoveEventArgs.Count, 1);
                    {
                        var arg = RemoveEventArgs[0];
                        Assert.AreEqual(arg.OldStartingIndex, removeIndex);
                        Assert.AreEqual(arg.OldItems!.Count, argsLength);
                        Assert.IsTrue(arg.OldItems.ItemEquals(removeItems, itemListComparer));
                        Assert.AreEqual(arg.NewStartingIndex, -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }
                    Assert.AreEqual(ReplaceEventArgs.Count, 0);
                    Assert.AreEqual(AddEventArgs.Count, 0);
                    Assert.AreEqual(MoveEventArgs.Count, 0);
                    Assert.AreEqual(ResetEventArgs.Count, 0);
                }
            }

            public void CheckResetEventArgs(int startIndex,
                TestRecord[][] oldItems, TestRecord[][] newItems)
            {
                // Reset が一度通知されていること
                Assert.AreEqual(ResetEventArgs.Count, 1);
                {
                    var arg = ResetEventArgs[0];
                    Assert.AreEqual(arg.OldStartingIndex, startIndex);
                    Assert.AreEqual(arg.OldItems!.Count, oldItems.Length);
                    Assert.IsTrue(arg.OldItems.ItemEquals(oldItems, itemListComparer));
                    Assert.AreEqual(arg.NewStartingIndex, startIndex);
                    Assert.AreEqual(arg.NewItems!.Count, newItems.Length);
                    Assert.IsTrue(arg.NewItems.ItemEquals(newItems, itemListComparer));
                }
                Assert.AreEqual(ReplaceEventArgs.Count, 0);
                Assert.AreEqual(AddEventArgs.Count, 0);
                Assert.AreEqual(MoveEventArgs.Count, 0);
                Assert.AreEqual(RemoveEventArgs.Count, 0);
            }

            #endregion

            private List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>> ReplaceEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Replace)];

            private List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>> AddEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Add)];

            private List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>> RemoveEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Remove)];

            private List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>> ResetEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Reset)];

            private List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>> MoveEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Move)];

            private void Clear()
            {
                Impl.Clear();
                Impl.Add(nameof(NotifyCollectionChangedAction.Replace),
                    new List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Add),
                    new List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Remove),
                    new List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Reset),
                    new List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Move),
                    new List<NotifyCollectionChangedEventArgsEx<IFixedLengthList<TestRecord>>>());
            }
        }

        private enum ListManipulationType
        {
            None,
            Add,
            Remove
        }

        #endregion
    }
}
