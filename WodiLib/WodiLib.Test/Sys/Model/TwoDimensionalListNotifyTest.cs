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
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Enabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Once,
                NotifyTwoDimensionalListChangeEventType.Once, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Enabled, NotifyPropertyChangeEventType.Enabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Simple,
                NotifyTwoDimensionalListChangeEventType.Single, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Single,
                NotifyTwoDimensionalListChangeEventType.Multi_All, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.Multi,
                NotifyTwoDimensionalListChangeEventType.Multi_Row, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Once, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.Multi_Column, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Simple, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.Multi_Line, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Single, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.Simple_All, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Multi, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.Simple_Row, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Single, NotifyCollectionChangeEventType.Once,
                NotifyTwoDimensionalListChangeEventType.Simple_Column,
                NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.Multi, NotifyCollectionChangeEventType.Multi,
                NotifyTwoDimensionalListChangeEventType.Simple_Line, NotifyTwoDimensionalListChangeEventType.None
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Once
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Single
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Multi_All
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Multi_Row
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Multi_Column
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Multi_Line
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Simple_All
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Simple_Row
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.Simple_Column
            },
            new object[]
            {
                NotifyPropertyChangeEventType.Disabled, NotifyPropertyChangeEventType.Disabled,
                NotifyCollectionChangeEventType.None, NotifyCollectionChangeEventType.None,
                NotifyTwoDimensionalListChangeEventType.None, NotifyTwoDimensionalListChangeEventType.Simple_Line
            },
        };

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void GetTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

            const int getRow = 2;
            const int getColumn = 4;

            {
                var _ = instance[getRow, getColumn];
            }

            {
                var checkNotifyPropertyChange = new Action<Dictionary<string, int>>(dic =>
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

            {
                var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangedEventArgsDic>(dic =>
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                });
                checkNotifyTwoDimListChange(twoDimensionalListChangingEventArgsDic);
                checkNotifyTwoDimListChange(twoDimensionalListChangedEventArgsDic);
            }
        }

        #region Set

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int rowIdx = 3;
            const int colIdx = 5;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnOne,
                rowIdx, colIdx, Direction.None,
                (instance, setItems) => instance[rowIdx, colIdx] = setItems[0].First());
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int rowIdx = 3;
            const int colIdx = 0;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                rowIdx, colIdx, Direction.Row,
                (instance, setItems) => instance.SetRow(rowIdx, setItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int rowIdx = 1;
            const int colIdx = 0;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                rowIdx, colIdx, Direction.Row,
                (instance, setItems) => instance.SetRow(rowIdx, setItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int rowIdx = 0;
            const int colIdx = 3;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                rowIdx, colIdx, Direction.Column,
                (instance, setItems) => instance.SetColumn(colIdx, setItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void SetColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int rowIdx = 0;
            const int colIdx = 3;

            SetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                rowIdx, colIdx, Direction.Column,
                (instance, setItems) => instance.SetColumn(colIdx, setItems));
        }

        private static void SetTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            TestDoubleEnumerableInstanceType type,
            int rowIdx, int colIdx, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

            var setItems = MakeSetColumn(type, execDirection);
            var needTranspose = NeedTranspose(execDirection, Direction.Row);
            var rowLength = needTranspose ? setItems.GetInnerArrayLength() : setItems.Length;
            var colLength = needTranspose ? setItems.Length : setItems.GetInnerArrayLength();
            var fixedOldItems = instance.GetItem(rowIdx, rowLength, colIdx, colLength)
                .ToTwoDimensionalArray();
            var oldItems = fixedOldItems.ToTransposedArrayIf(needTranspose);

            actionCore(instance, TestTools.ConvertIEnumerableArray(setItems));

            Set_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic);
            Set_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                fixedOldItems, setItems.ToTransposedArrayIf(execDirection == Direction.Column), rowIdx);
            Set_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
                oldItems, setItems, rowIdx, colIdx, execDirection);
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
                    Assert.IsTrue(dic.Keys.Count == 1);
                    Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

        private static void Set_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            TestRecord[][] fixedOldItems, TestRecord[][] setItems,
            int rowIdx, int colIdx, Direction execDirection)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    dic.CheckReplaceEventArgs(execDirection, eventType.GroupingType,
                        rowIdx, colIdx, fixedOldItems, setItems);
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
        }

        #endregion

        #region Add/Insert

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                TestTools.InitRowLength, Direction.Row,
                (instance, addItems) => instance.AddRow(addItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                TestTools.InitRowLength, Direction.Row,
                (instance, addItems) => instance.AddRow(addItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                TestTools.InitColumnLength, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                TestTools.InitColumnLength, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                insertIndex, Direction.Row,
                (instance, insertItems) => instance.InsertRow(insertIndex, insertItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                insertIndex, Direction.Row,
                (instance, insertItems) => instance.InsertRow(insertIndex, insertItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                insertIndex, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(insertIndex, insertItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int insertIndex = 1;

            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                insertIndex, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(insertIndex, insertItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic, 0, Direction.Row,
                (instance, addItems) => instance.AddRow(addItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddRowRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, 0, Direction.Row,
                (instance, addItems) => instance.AddRow(addItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne, 0, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AddColumnRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic, 0, Direction.Column,
                (instance, addItems) => instance.AddColumn(addItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                0, Direction.Row,
                (instance, insertItems) => instance.InsertRow(0, insertItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertRowRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                0, Direction.Row,
                (instance, insertItems) => instance.InsertRow(0, insertItems));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                0, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(0, insertItems[0]));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void InsertColumnRangeFromEmptyTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            InsertTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                true, TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                0, Direction.Column,
                (instance, insertItems) => instance.InsertColumn(0, insertItems));
        }

        private static void InsertTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            bool isEmptyFrom, TestDoubleEnumerableInstanceType type,
            int insertIndex, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                isEmptyFrom,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

            var insertItems = MakeSetColumn(type, execDirection);
            var insertColumnLength = execDirection == Direction.Column
                ? insertItems.Length
                : insertItems.GetInnerArrayLength();
            var oldItems = instance.ToTwoDimensionalArray();
            var fixedOldItems = oldItems.ToTransposedArrayIf(execDirection == Direction.Column);

            actionCore(instance, TestTools.ConvertIEnumerableArray(insertItems));

            Insert_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                isEmptyFrom, insertColumnLength,
                propertyChangingCountDic, propertyChangedCountDic, execDirection);
            Insert_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                oldItems, isEmptyFrom, insertItems, insertIndex, execDirection);
            Insert_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
                fixedOldItems, insertItems, insertIndex, execDirection);
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
                                Assert.IsTrue(dic.Keys.Count == 5);
                                Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                            }
                            else
                            {
                                // 列数 == 0 の場合、行数のみ変化
                                Assert.IsTrue(dic.Keys.Count == 4);
                                Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                                Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                            }
                        }
                        else
                        {
                            // 元々空リストではない場合
                            Assert.IsTrue(dic.Keys.Count == 3);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                        }
                    }
                    else
                    {
                        if (isEmptyFrom)
                        {
                            // 空リストに追加した場合
                            Assert.IsTrue(dic.Keys.Count == 5);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                        }
                        else
                        {
                            // 元々空リストではない場合
                            Assert.IsTrue(dic.Keys.Count == 3);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                        }
                    }
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

        private static void Insert_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            TestRecord[][] oldItems, TestRecord[][] insertItems, int insertIndex, Direction execDirection)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    // 処理方向と通知方向が一致するなら add が、一致しないなら replace が呼ばれている
                    var isMadeAddArgs = TestTools.IsEqualExecDirectionAndNotifyDirection(execDirection, eventType);

                    if (isMadeAddArgs)
                    {
                        dic.CheckAddEventArgs(execDirection, eventType.GroupingType,
                            insertIndex, insertItems);
                    }
                    else
                    {
                        var crossDirection = execDirection == Direction.Column
                            ? Direction.Row
                            : Direction.Column;
                        var setRowIndex = crossDirection != Direction.Column ? insertIndex : 0;
                        var setColumnIndex = crossDirection == Direction.Column ? insertIndex : 0;
                        var fixedOldItems = oldItems.ToTransposedArray();
                        var fixedInsertItems = insertItems.ToTransposedArray();
                        var setItems = LinqExtension.Zip(fixedOldItems, fixedInsertItems)
                            .Select(zip =>
                            {
                                var (oldArray, insertArray) = zip;
                                var result = oldArray.ToList();
                                result.InsertRange(insertIndex, insertArray);
                                return result.ToArray();
                            }).ToArray();
                        dic.CheckReplaceEventArgs(crossDirection, eventType.GroupingType, setRowIndex, setColumnIndex,
                            fixedOldItems, setItems);
                    }
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
        }

        #endregion

        #region Move

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int oldIndex = 2;
            const int newIndex = 3;
            const int count = 1;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                oldIndex, newIndex, count, Direction.Row,
                (instance, _) => instance.MoveRow(oldIndex, newIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int oldIndex = 0;
            const int newIndex = 1;
            const int count = 2;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                oldIndex, newIndex, count, Direction.Row,
                (instance, _) => instance.MoveRow(oldIndex, newIndex, count));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int oldIndex = 2;
            const int newIndex = 3;
            const int count = 1;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                oldIndex, newIndex, count, Direction.Column,
                (instance, _) => instance.MoveColumn(oldIndex, newIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void MoveColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int oldIndex = 1;
            const int newIndex = 3;
            const int count = 2;

            MoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                oldIndex, newIndex, count, Direction.Column,
                (instance, _) => instance.MoveColumn(oldIndex, newIndex, count));
        }

        private static void MoveTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            int oldIndex, int newIndex, int count, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

            var oldItems = instance.ToTwoDimensionalArray();
            var moveItems = (execDirection == Direction.Row
                ? instance.GetRow(oldIndex, count)
                : instance.GetColumn(oldIndex, count)).ToTwoDimensionalArray();

            actionCore(instance, null);

            Move_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic);
            Move_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                oldItems, oldIndex, newIndex, count, execDirection);
            Move_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
                oldIndex, newIndex, moveItems, execDirection);
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
                    Assert.IsTrue(dic.Keys.Count == 1);
                    Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

        private static void Move_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            int oldIndex, int newIndex, TestRecord[][] moveItems, Direction execDirection)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    dic.CheckMoveEventArgs(execDirection, eventType.GroupingType,
                        oldIndex, newIndex, moveItems);
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
        }

        #endregion

        #region Remove

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int removeIndex = 3;
            const int count = 1;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                removeIndex, count, Direction.Row,
                (instance, _) => instance.RemoveRow(removeIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveRowRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int removeIndex = 1;
            const int count = 2;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                removeIndex, count, Direction.Row,
                (instance, _) => instance.RemoveRow(removeIndex, count));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int removeIndex = 3;
            const int count = 1;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                removeIndex, count, Direction.Column,
                (instance, _) => instance.RemoveColumn(removeIndex));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void RemoveColumnRangeTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int removeIndex = 1;
            const int count = 2;

            RemoveTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                removeIndex, count, Direction.Column,
                (instance, _) => instance.RemoveColumn(removeIndex, count));
        }

        private static void RemoveTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            int removeIndex, int count, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

            var target = instance.ToTwoDimensionalArray();
            var fixedOldItems = target.ToTransposedArrayIf(NeedTranspose(execDirection, Direction.Row));
            var removeItems = (execDirection == Direction.Row
                    ? instance.GetRow(removeIndex, count)
                    : instance.GetColumn(removeIndex, count)
                ).ToTwoDimensionalArray();

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
            Remove_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
                fixedOldItems, removeItems, removeIndex, execDirection);
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
                            Assert.IsTrue(dic.Keys.Count == 4);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                        }
                        else
                        {
                            Assert.IsTrue(dic.Keys.Count == 3);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                        }
                    }
                    else
                    {
                        if (isEmptyFromNotEmpty)
                        {
                            Assert.IsTrue(dic.Keys.Count == 4);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                        }
                        else
                        {
                            Assert.IsTrue(dic.Keys.Count == 3);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                        }
                    }
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

        private static void Remove_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            TestRecord[][] fixedOldItems, TestRecord[][] removeItems, int index, Direction execDirection)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    // 処理方向と通知方向が一致するなら Remove が、一致しないなら replace が呼ばれている
                    var isMadeRemoveArgs = TestTools.IsEqualExecDirectionAndNotifyDirection(execDirection, eventType);

                    if (isMadeRemoveArgs)
                    {
                        dic.CheckRemoveEventArgs(execDirection, eventType.GroupingType,
                            index, removeItems);
                    }
                    else
                    {
                        var crossDirection = execDirection == Direction.Column
                            ? Direction.Row
                            : Direction.Column;
                        var setRowIndex = crossDirection != Direction.Column ? index : 0;
                        var setColumnIndex = crossDirection == Direction.Column ? index : 0;
                        var removeLength = removeItems.Length;
                        var setItems = fixedOldItems.Take(index)
                            .Concat(fixedOldItems.TakeLast(fixedOldItems.Length - (index + removeLength)))
                            .ToArray();
                        dic.CheckReplaceEventArgs(crossDirection, eventType.GroupingType, setRowIndex, setColumnIndex,
                            fixedOldItems.ToTransposedArray(), setItems.ToTransposedArray());
                    }
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
        }

        #endregion

        #region Reset

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void ResetTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            var resetItems = MakeSetColumn(TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnOne, Direction.Row);

            ResetTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                resetItems,
                (instance, _) => instance.Reset(resetItems));
        }

        private static void ResetTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            TestRecord[][] resetItems, ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

            var target = instance.ToTwoDimensionalArray();

            actionCore(instance, null);

            Reset_CheckNotify_PropertyChangeTest(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                propertyChangingCountDic, propertyChangedCountDic);
            Reset_CheckNotify_CollectionChangeTest(
                collectionChangingEventType, collectionChangedEventType,
                collectionChangingEventArgsDic, collectionChangedEventArgsDic,
                target, resetItems, 0);
            Reset_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
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
                    Assert.IsTrue(dic.Keys.Count == 4);
                    Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                    Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                    Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                    Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

        private static void Reset_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            TestRecord[][] target, TestRecord[][] resetItems, int startIndex)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    var notifyDirection = TestTools.NotifyDirectionFrom(eventType.GroupingType, Direction.Row);
                    var needTranspose = NeedTranspose(notifyDirection, Direction.Row);
                    var fixedResetItems = resetItems.ToTransposedArrayIf(needTranspose);
                    var fixedTarget = target.ToTransposedArrayIf(needTranspose);

                    dic.CheckResetEventArgs(startIndex, fixedTarget,
                        fixedResetItems, notifyDirection);
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
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
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int startIndex = TestTools.InitRowLength - 2;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowOne_ColumnBasic,
                startIndex, Direction.Row,
                (instance, items) => instance.OverwriteRow(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteRow_AddTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int startIndex = TestTools.InitRowLength;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                startIndex, Direction.Row,
                (instance, items) => instance.OverwriteRow(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteRow_BothTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int startIndex = TestTools.InitRowLength - 1;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowTwo_ColumnBasic,
                startIndex, Direction.Row,
                (instance, items) => instance.OverwriteRow(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteColumn_ReplaceTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int startIndex = TestTools.InitColumnLength - 2;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnOne,
                startIndex, Direction.Column,
                (instance, items) => instance.OverwriteColumn(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteColumn_AddTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int startIndex = TestTools.InitColumnLength;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                startIndex, Direction.Column,
                (instance, items) => instance.OverwriteColumn(startIndex, items));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void OverwriteColumn_BothTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int startIndex = TestTools.InitColumnLength - 1;

            OverwriteTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestDoubleEnumerableInstanceType.NotNull_RowBasic_ColumnTwo,
                startIndex, Direction.Column,
                (instance, items) => instance.OverwriteColumn(startIndex, items));
        }

        private static void OverwriteTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            TestDoubleEnumerableInstanceType type, int startIndex,
            Direction execDirection, ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

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
            Overwrite_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
                target, startIndex, overwriteItems, execDirection);
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
                        Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                    }

                    if (isChangeColumnLength)
                    {
                        count += 1;
                        Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                    }

                    if (isChangeRowLength || isChangeColumnLength)
                    {
                        count += 1;
                        Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                    }

                    if (isNotEmptyFromEmpty)
                    {
                        count += 1;
                        Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                    }

                    Assert.IsTrue(dic.Keys.Count == count);
                    Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

        private static void Overwrite_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            TestRecord[][] target, int startIndex, TestRecord[][] overwriteItems,
            Direction execDirection)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                }
                else
                {
                    var needTranspose = NeedTranspose(execDirection, Direction.Row);
                    var notifyDirection = TestTools.NotifyDirectionFrom(eventType.GroupingType, execDirection);

                    if (execDirection == notifyDirection)
                    {
                        // 処理方向と通知方向が同じ場合

                        var fixedTarget = target.ToTransposedArrayIf(needTranspose);

                        var replaceLength = Math.Min(fixedTarget.Length - startIndex, overwriteItems.Length);
                        var addLength = overwriteItems.Length - replaceLength;

                        var replaceOldItems = fixedTarget.Skip(startIndex).Take(replaceLength).ToArray();
                        var replaceNewItems = overwriteItems.Take(replaceLength).ToArray();
                        var addItems = overwriteItems.Skip(replaceLength).ToArray();

                        var isReplaced = replaceLength > 0;
                        var isAdded = addLength > 0;

                        var isBoth = isReplaced && isAdded;

                        if (eventType.IsMultiAction || !eventType.IsMultiAction && !isBoth)
                        {
                            // replace または add どちらかのみ発生、
                            // またはどちらも発生した場合にそれぞれのアクションの通知を行う場合

                            // replace イベント通知
                            if (isReplaced)
                            {
                                var rowIdx = execDirection == Direction.Row
                                    ? startIndex
                                    : 0;
                                var columnIdx = execDirection == Direction.Row
                                    ? 0
                                    : startIndex;

                                dic.CheckOnlyReplaceEventArgs(execDirection, eventType.GroupingType,
                                    rowIdx, columnIdx, replaceOldItems, replaceNewItems);
                            }
                            else
                            {
                                Assert.IsTrue(dic.IsReplaceEventEmpty);
                            }

                            // add イベント通知
                            if (isAdded)
                            {
                                var addIndex = startIndex + replaceLength;

                                dic.CheckOnlyAddEventArgs(execDirection, eventType.GroupingType,
                                    addIndex, addItems);
                            }
                            else
                            {
                                Assert.IsTrue(dic.IsAddEventEmpty);
                            }

                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsRemoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);

                            // return; // IDEの警告抑制のためコメントアウト
                        }
                        else
                        {
                            // replace, add どちらも発生、
                            // かつ複数アクション発生時に一括通知を行う場合

                            // reset イベント通知
                            dic.CheckResetEventArgs(startIndex, replaceOldItems,
                                overwriteItems, notifyDirection);

                            Assert.IsTrue(dic.IsReplaceEventEmpty);
                            Assert.IsTrue(dic.IsAddEventEmpty);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsRemoveEventEmpty);

                            // return; // IDEの警告抑制のためコメントアウト
                        }
                    }
                    else
                    {
                        // 処理方向と通知方向が異なる場合

                        var fixedOldItems = target.ToTransposedArrayIf(!needTranspose);
                        var oldItemsForNotify =
                            target.ToTransposedArrayIf(NeedTranspose(notifyDirection, Direction.Column));

                        var replaceLength = Math.Min(
                            oldItemsForNotify.Length - startIndex,
                            overwriteItems.Length);
                        var addLength = overwriteItems.Length - replaceLength;

                        // replace イベント通知
                        if (replaceLength > 0)
                        {
                            var notifyOldItems = fixedOldItems.Select(line => line.Skip(startIndex).Take(replaceLength))
                                .ToTwoDimensionalArray();

                            var notifyNewItems = overwriteItems.Take(replaceLength)
                                .ToTransposedArray();

                            var rowIdx = execDirection == Direction.Row
                                ? startIndex
                                : 0;
                            var columnIdx = execDirection == Direction.Row
                                ? 0
                                : startIndex;

                            var checkDirection = execDirection == Direction.Row
                                ? Direction.Column
                                : Direction.Row;
                            dic.CheckOnlyReplaceEventArgs(checkDirection, eventType.GroupingType,
                                rowIdx, columnIdx, notifyOldItems, notifyNewItems);
                        }
                        else
                        {
                            Assert.IsTrue(dic.IsReplaceEventEmpty);
                        }

                        // Addイベント通知
                        if (addLength > 0)
                        {
                            var insertItems = overwriteItems.Skip(replaceLength)
                                .ToArray();
                            dic.CheckOnlyAddEventArgs(execDirection, eventType.GroupingType,
                                startIndex + replaceLength, insertItems);
                        }
                        else
                        {
                            Assert.IsTrue(dic.IsAddEventEmpty);
                        }

                        Assert.IsTrue(dic.IsMoveEventEmpty);
                        Assert.IsTrue(dic.IsRemoveEventEmpty);
                        Assert.IsTrue(dic.IsResetEventEmpty);
                    }
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
        }

        #endregion

        #region Adjust

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    rowLength, columnLength, Direction.None,
                    (instance, _) => instance.AdjustLength(rowLength, columnLength));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthIfLongTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    resultRowLength, resultColumnLength, Direction.None,
                    (instance, _) => instance.AdjustLengthIfLong(rowLength, columnLength));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthIfShortTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    resultRowLength, resultColumnLength, Direction.None,
                    (instance, _) => instance.AdjustLengthIfShort(rowLength, columnLength));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthRowTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int length = 3;

            AdjustLengthTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                length, TestTools.InitColumnLength, Direction.Row,
                (instance, _) => instance.AdjustRowLength(length));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthRowIfLongTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    resultLength, TestTools.InitColumnLength, Direction.Row,
                    (instance, _) => instance.AdjustRowLengthIfLong(length));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthRowIfShortTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    resultLength, TestTools.InitColumnLength, Direction.Row,
                    (instance, _) => instance.AdjustRowLengthIfShort(length));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthColumnTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
        {
            const int length = 3;

            AdjustLengthTestCore(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                TestTools.InitRowLength, length, Direction.Column,
                (instance, _) => instance.AdjustColumnLength(length));
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthColumnIfLongTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    TestTools.InitRowLength, resultLength, Direction.Column,
                    (instance, _) => instance.AdjustColumnLengthIfLong(length));
            });
        }

        [TestCaseSource(nameof(NotifyEventArgsTestCaseSource))]
        public static void AdjustLengthColumnIfShortTest(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType)
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
                    notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                    TestTools.InitRowLength, resultLength, Direction.Column,
                    (instance, _) => instance.AdjustColumnLengthIfShort(length));
            });
        }

        private static void AdjustLengthTestCore(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType collectionChangingEventType,
            NotifyCollectionChangeEventType collectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            int adjustRowLength, int adjustColumnLength, Direction execDirection,
            ActionCore actionCore)
        {
            var instance = MakeList(
                notifyPropertyChangingEventType, notifyPropertyChangedEventType,
                collectionChangingEventType, collectionChangedEventType,
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                false,
                out var propertyChangingCountDic, out var propertyChangedCountDic,
                out var collectionChangingEventArgsDic, out var collectionChangedEventArgsDic,
                out var twoDimensionalListChangingEventArgsDic, out var twoDimensionalListChangedEventArgsDic);

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
            AdjustLength_CheckNotify_TwoDimListChangeTest(
                notifyTwoDimensionalListChangingEventType, notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangingEventArgsDic, twoDimensionalListChangedEventArgsDic,
                target, adjustRowLength, adjustColumnLength, execDirection);
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
                            Assert.IsTrue(dic.Keys.Count == 5);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                            break;
                        case (true, false, true):
                            Assert.IsTrue(dic.Keys.Count == 4);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            break;
                        case (true, true, false):
                            Assert.IsTrue(dic.Keys.Count == 4);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.IsEmpty)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            break;
                        case (true, false, false):
                            Assert.IsTrue(dic.Keys.Count == 3);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.RowCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            break;
                        case (false, _, true):
                            Assert.IsTrue(dic.Keys.Count == 3);
                            Assert.IsTrue(dic[ListConstant.IndexerName] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.ColumnCount)] == 1);
                            Assert.IsTrue(dic[nameof(TwoDimensionalList<TestRecord>.AllCount)] == 1);
                            break;
                        case (false, _, false):
                            Assert.IsTrue(dic.Keys.Count == 0);
                            break;
                    }
                }
                else if (eventType == NotifyPropertyChangeEventType.Disabled)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.Keys.Count == 0);
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

                var columnManipulationType = DetermineListManipulationType(oldColumnLength, adjustColumnLength);

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
            });
            checkNotifyCollectionChange(collectionChangingEventType, collectionChangingEventArgsDic);
            checkNotifyCollectionChange(collectionChangedEventType, collectionChangedEventArgsDic);
        }

        private static void AdjustLength_CheckNotify_TwoDimListChangeTest(
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsDic,
            NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsDic,
            TestRecord[][] target, int adjustRowLength, int adjustColumnLength,
            Direction execDirection)
        {
            var checkNotifyTwoDimListChange = new Action<NotifyTwoDimensionalListChangeEventType,
                NotifyTwoDimensionalListChangedEventArgsDic>((eventType, dic) =>
            {
                if (eventType == NotifyTwoDimensionalListChangeEventType.None)
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                    return;
                }

                var oldRowLength = target.Length;
                var oldColumnLength = target.GetInnerArrayLength();

                var rowManipulationType = DetermineListManipulationType(oldRowLength, adjustRowLength);
                var columnManipulationType = DetermineListManipulationType(oldColumnLength, adjustColumnLength);

                var notifyDirection = TestTools.NotifyDirectionFrom(eventType.GroupingType, execDirection);

                switch (rowManipulationType, columnManipulationType)
                {
                    case (ListManipulationType.Add, ListManipulationType.Add):
                    {
                        var addRowLength = adjustRowLength - oldRowLength;
                        var addColumnLength = adjustColumnLength - oldColumnLength;

                        if (!eventType.IsMultiAction)
                        {
                            var newItems = target.Select((line, rowIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, addColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .Concat(Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                    => Enumerable.Range(0, adjustColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx)).ToArray())
                                ).ToArray();
                            dic.CheckResetEventArgs(0, target, newItems, notifyDirection);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                                 || execDirection == Direction.Row && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var replacedItems = target.Select((line, rowIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, addColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .ToArray();
                            var addItems = Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                    => Enumerable.Range(0, adjustColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx)).ToArray())
                                .ToArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Row,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyAddEventArgs(Direction.Row, eventType.GroupingType,
                                oldRowLength, addItems);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsRemoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var transposedTarget = target.ToTransposedArray();

                            var replacedItems = transposedTarget.Select((line, colIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .ToArray();
                            var addItems = Enumerable.Range(oldColumnLength, addColumnLength).Select(colIdx
                                    => Enumerable.Range(0, adjustRowLength).Select(rowIdx
                                        => TestTools.MakeItem(rowIdx, colIdx)).ToArray())
                                .ToArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Column,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyAddEventArgs(Direction.Column, eventType.GroupingType,
                                oldColumnLength, addItems);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var addRowItems = Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                => Enumerable.Range(0, oldColumnLength + addColumnLength).Select(colIdx
                                    => TestTools.MakeItem(rowIdx, colIdx)).ToArray()).ToArray();
                            var addColumnItems = Enumerable.Range(oldColumnLength, addColumnLength).Select(colIdx
                                => Enumerable.Range(0, oldRowLength + addRowLength).Select(rowIdx
                                    => TestTools.MakeItem(rowIdx, colIdx)).ToArray()).ToArray();

                            dic.CheckAddEventArgsForAdjustLengthBoth(execDirection,
                                oldRowLength, oldColumnLength, addRowItems, addColumnItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.Add, ListManipulationType.None):
                    {
                        var addRowLength = adjustRowLength - oldRowLength;

                        if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                            || execDirection == Direction.Row && (
                                eventType == NotifyTwoDimensionalListChangeEventType.Single
                                || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line)
                            || eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var addItems = Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                => Enumerable.Range(0, oldColumnLength).Select(colIdx
                                    => TestTools.MakeItem(rowIdx, colIdx)).ToArray()).ToArray();

                            dic.CheckAddEventArgs(Direction.Row, eventType.GroupingType,
                                oldRowLength, addItems);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var transposedTarget = target.ToTransposedArray();

                            var replacedItems = transposedTarget.Select((line, colIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, adjustRowLength).Select(rowIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .ToArray();

                            dic.CheckReplaceEventArgs(Direction.Column,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.Add, ListManipulationType.Remove):
                    {
                        var addRowLength = adjustRowLength - oldRowLength;
                        var removeColumnLength = oldColumnLength - adjustColumnLength;

                        if (!eventType.IsMultiAction)
                        {
                            var newItems = target.Select(line => line.Take(adjustColumnLength).ToArray())
                                .Concat(Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                    => Enumerable.Range(0, adjustColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx)).ToArray())
                                ).ToArray();
                            dic.CheckResetEventArgs(0, target, newItems, notifyDirection);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                                 || execDirection == Direction.Row && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var replacedItems = target.Select(line
                                    => line.Take(adjustColumnLength).ToArray())
                                .ToArray();
                            var addItems = Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                    => Enumerable.Range(0, adjustColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx)).ToArray())
                                .ToArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Row,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyAddEventArgs(Direction.Row, eventType.GroupingType,
                                oldRowLength, addItems);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsRemoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var transposedTarget = target.ToTransposedArray();

                            var replacedItems = transposedTarget.Take(adjustColumnLength).Select((line, colIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .ToArray();
                            var removeItems = transposedTarget.Skip(adjustColumnLength)
                                .ToArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Column,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyRemoveEventArgs(Direction.Column, eventType.GroupingType,
                                adjustColumnLength, removeItems);
                            Assert.IsTrue(dic.IsAddEventEmpty);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var addRowItems = Enumerable.Range(oldRowLength, addRowLength).Select(rowIdx
                                => Enumerable.Range(0, adjustColumnLength).Select(colIdx
                                    => TestTools.MakeItem(rowIdx, colIdx)).ToArray()).ToArray();
                            var removeColumnItems = Enumerable.Range(adjustColumnLength, removeColumnLength)
                                .Select(colIdx => Enumerable.Range(0, adjustRowLength).Select(rowIdx
                                    => TestTools.MakeItem(rowIdx, colIdx)).ToArray()).ToArray();

                            dic.CheckOnlyAddEventArgs(Direction.Row, eventType.GroupingType,
                                oldRowLength, addRowItems);
                            dic.CheckOnlyRemoveEventArgs(Direction.Row,
                                NotifyTwoDimensionalListChangeEventGroupingType.All,
                                adjustColumnLength, removeColumnItems);
                            Assert.IsTrue(dic.IsReplaceEventEmpty);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.None, ListManipulationType.Add):
                    {
                        var addColumnLength = adjustColumnLength - oldColumnLength;

                        if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                            || execDirection == Direction.Row && (
                                eventType == NotifyTwoDimensionalListChangeEventType.Single
                                || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var replacedItems = target.Select((line, rowIdx)
                                    => line.Concat(Enumerable.Range(oldColumnLength, adjustColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .ToTwoDimensionalArray();

                            dic.CheckReplaceEventArgs(Direction.Row,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line)
                                 || eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var addItems = Enumerable.Range(oldColumnLength, addColumnLength).Select(colIdx
                                => Enumerable.Range(0, oldRowLength).Select(rowIdx
                                    => TestTools.MakeItem(rowIdx, colIdx)).ToArray()).ToArray();

                            dic.CheckAddEventArgs(Direction.Column, eventType.GroupingType,
                                oldColumnLength, addItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.None, ListManipulationType.Remove):
                    {
                        var removeColumnLength = oldColumnLength - adjustColumnLength;

                        if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                            || execDirection == Direction.Row && (
                                eventType == NotifyTwoDimensionalListChangeEventType.Single
                                || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var replacedItems = target.Select(line
                                    => line.Take(adjustColumnLength))
                                .ToTwoDimensionalArray();

                            dic.CheckReplaceEventArgs(Direction.Row,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line)
                                 || eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var removeItems = target.Range(adjustColumnLength, removeColumnLength)
                                .ToTransposedArray();

                            dic.CheckRemoveEventArgs(Direction.Column, eventType.GroupingType,
                                adjustColumnLength, removeItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.Remove, ListManipulationType.Add):
                    {
                        var addColumnLength = adjustColumnLength - oldColumnLength;

                        if (!eventType.IsMultiAction)
                        {
                            var newItems = target.Select((line, rowIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, addColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))))
                                .Take(adjustRowLength).ToTwoDimensionalArray();
                            dic.CheckResetEventArgs(0, target, newItems, notifyDirection);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                                 || execDirection == Direction.Row && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var replacedItems = target.Select((line, rowIdx)
                                    => line.Concat(Enumerable.Range(oldRowLength, addColumnLength).Select(colIdx
                                        => TestTools.MakeItem(rowIdx, colIdx))).ToArray())
                                .ToArray();
                            var removeItems = target.Skip(adjustRowLength).ToArray();

                            dic.CheckOnlyReplaceEventArgs(execDirection,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyRemoveEventArgs(execDirection, eventType.GroupingType,
                                oldRowLength, removeItems);
                            Assert.IsTrue(dic.IsAddEventEmpty);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var transposedTarget = target.ToTransposedArray();

                            var replacedItems = transposedTarget.Select(line => line.Take(adjustRowLength))
                                .ToTwoDimensionalArray();
                            var addItems = Enumerable.Range(oldColumnLength, addColumnLength).Select(colIdx
                                    => Enumerable.Range(0, adjustRowLength).Select(rowIdx
                                        => TestTools.MakeItem(rowIdx, colIdx)))
                                .ToTwoDimensionalArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Column,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyAddEventArgs(Direction.Column, eventType.GroupingType,
                                oldColumnLength, addItems);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsRemoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var removeRowItems = target.Range(adjustRowLength, oldRowLength)
                                .ToTwoDimensionalArray();
                            var addColumnItems = Enumerable.Range(0, adjustRowLength).Select(rowIdx
                                => Enumerable.Range(oldColumnLength, addColumnLength).Select(colIdx
                                    => TestTools.MakeItem(rowIdx, colIdx))).ToTransposedArray();

                            dic.CheckOnlyAddEventArgs(Direction.Column, eventType.GroupingType,
                                oldColumnLength, addColumnItems);
                            dic.CheckOnlyRemoveEventArgs(Direction.Row, eventType.GroupingType,
                                adjustRowLength, removeRowItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.Remove, ListManipulationType.None):
                    {
                        if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                            || execDirection == Direction.Row && (
                                eventType == NotifyTwoDimensionalListChangeEventType.Single
                                || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line)
                            || eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var removeItems = target.Skip(adjustRowLength).ToArray();

                            dic.CheckRemoveEventArgs(Direction.Row, eventType.GroupingType,
                                oldRowLength, removeItems);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var transposedTarget = target.ToTransposedArray();

                            var replacedItems = transposedTarget.Select(line
                                    => line.Take(adjustRowLength))
                                .ToTwoDimensionalArray();

                            dic.CheckReplaceEventArgs(Direction.Column,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                    case (ListManipulationType.Remove, ListManipulationType.Remove):
                    {
                        if (!eventType.IsMultiAction)
                        {
                            var newItems = target.Take(adjustRowLength)
                                .Select(line => line.Take(adjustColumnLength))
                                .ToTwoDimensionalArray();
                            dic.CheckResetEventArgs(0, target, newItems, notifyDirection);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Row
                                 || execDirection == Direction.Row && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var replacedItems = target.Select(line
                                    => line.Take(adjustRowLength))
                                .ToTwoDimensionalArray();
                            var removeItems = target.Skip(adjustRowLength)
                                .ToArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Row,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyRemoveEventArgs(Direction.Row, eventType.GroupingType,
                                adjustRowLength, removeItems);
                            Assert.IsTrue(dic.IsAddEventEmpty);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_Column
                                 || execDirection == Direction.Column && (
                                     eventType == NotifyTwoDimensionalListChangeEventType.Single
                                     || eventType == NotifyTwoDimensionalListChangeEventType.Multi_Line))
                        {
                            var transposedTarget = target.ToTransposedArray();

                            var replacedItems = transposedTarget.Take(adjustColumnLength)
                                .Select(line => line.Take(adjustRowLength))
                                .ToTwoDimensionalArray();
                            var removeItems = transposedTarget.Skip(adjustColumnLength)
                                .ToArray();

                            dic.CheckOnlyReplaceEventArgs(Direction.Column,
                                eventType.GroupingType, 0, 0, target, replacedItems);
                            dic.CheckOnlyRemoveEventArgs(Direction.Column, eventType.GroupingType,
                                adjustColumnLength, removeItems);
                            Assert.IsTrue(dic.IsAddEventEmpty);
                            Assert.IsTrue(dic.IsMoveEventEmpty);
                            Assert.IsTrue(dic.IsResetEventEmpty);
                        }
                        else if (eventType == NotifyTwoDimensionalListChangeEventType.Multi_All)
                        {
                            var removeRowItems = target.Skip(adjustRowLength).ToArray();
                            var removeColumnItems = target.Skip(adjustRowLength)
                                .Select(line => line.Take(adjustColumnLength))
                                .ToTransposedArray();

                            dic.CheckRemoveEventArgsForAdjustLengthBoth(execDirection, adjustRowLength,
                                adjustColumnLength, removeRowItems, removeColumnItems);
                        }
                        else
                        {
                            Assert.Fail();
                        }

                        break;
                    }
                }
            });
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangingEventType,
                twoDimensionalListChangingEventArgsDic);
            checkNotifyTwoDimListChange(notifyTwoDimensionalListChangedEventType,
                twoDimensionalListChangedEventArgsDic);
        }

        #endregion

        #endregion

        #region TestTools

        private static TestRecord[][] MakeSetColumn(TestDoubleEnumerableInstanceType type, Direction execDirection)
        {
            return TestTools.MakeTestRecordList(type, execDirection == Direction.Column, TestTools.MakeInsertItem)
                ?.ToTwoDimensionalArray();
        }

        private static TwoDimensionalList<TestRecord> MakeList(
            NotifyPropertyChangeEventType notifyPropertyChangingEventType,
            NotifyPropertyChangeEventType notifyPropertyChangedEventType,
            NotifyCollectionChangeEventType notifyCollectionChangingEventType,
            NotifyCollectionChangeEventType notifyCollectionChangedEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangingEventType,
            NotifyTwoDimensionalListChangeEventType notifyTwoDimensionalListChangedEventType,
            bool isEmptyInstance,
            out Dictionary<string, int> propertyChangingCountDic,
            out Dictionary<string, int> propertyChangedCountDic,
            out NotifyCollectionChangedEventArgsDic collectionChangingEventArgsList,
            out NotifyCollectionChangedEventArgsDic collectionChangedEventArgsList,
            out NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangingEventArgsList,
            out NotifyTwoDimensionalListChangedEventArgsDic twoDimensionalListChangedEventArgsList)
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
            result.NotifyTwoDimensionalListChangingEventType = notifyTwoDimensionalListChangingEventType;
            result.NotifyTwoDimensionalListChangedEventType = notifyTwoDimensionalListChangedEventType;

            propertyChangingCountDic = new Dictionary<string, int>();
            result.PropertyChanging += MakePropertyChangingEventHandler(propertyChangingCountDic);

            propertyChangedCountDic = new Dictionary<string, int>();
            result.PropertyChanged += MakePropertyChangedEventHandler(propertyChangedCountDic);

            collectionChangingEventArgsList = new NotifyCollectionChangedEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = new NotifyCollectionChangedEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            twoDimensionalListChangingEventArgsList = new NotifyTwoDimensionalListChangedEventArgsDic();
            result.TwoDimensionalListChanging +=
                MakeTwoDimensionalListChangeEventHandler(true, twoDimensionalListChangingEventArgsList);

            twoDimensionalListChangedEventArgsList = new NotifyTwoDimensionalListChangedEventArgsDic();
            result.TwoDimensionalListChanged +=
                MakeTwoDimensionalListChangeEventHandler(false, twoDimensionalListChangedEventArgsList);

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

        private static NotifyCollectionChangedEventHandler MakeCollectionChangeEventHandler(bool isBefore,
            NotifyCollectionChangedEventArgsDic resultDic)
            => (_, args) =>
            {
                var argsEx = (NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>) args;
                resultDic.Add(argsEx);
                logger.Debug($"Collection{(isBefore ? "Changing" : "Changed")} Event Raise. ");
                logger.Debug($"{nameof(argsEx)}: {{");
                logger.Debug($"    {nameof(argsEx.Action)}: {argsEx.Action}");
                logger.Debug($"    {nameof(argsEx.OldStartingIndex)}: {argsEx.OldStartingIndex}");
                logger.Debug($"    {nameof(argsEx.OldItems)}: {argsEx.OldItems}");
                logger.Debug($"    {nameof(argsEx.NewStartingIndex)}: {argsEx.NewStartingIndex}");
                logger.Debug($"    {nameof(argsEx.NewItems)}: {argsEx.NewItems}");
                logger.Debug("}");
            };

        private static EventHandler<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>
            MakeTwoDimensionalListChangeEventHandler(bool isBefore,
                NotifyTwoDimensionalListChangedEventArgsDic resultDic)
            => (_, args) =>
            {
                resultDic.Add(args);
                logger.Debug($"Collection{(isBefore ? "Changing" : "Changed")} Event Raise. ");
                logger.Debug($"{nameof(args)}: {{");
                logger.Debug($"    {nameof(args.Action)}: {args.Action}");
                logger.Debug($"    {nameof(args.Direction)}: {args.Direction}");
                logger.Debug($"    {nameof(args.OldStartRow)}: {args.OldStartRow}");
                logger.Debug($"    {nameof(args.OldStartColumn)}: {args.OldStartColumn}");
                logger.Debug($"    {nameof(args.OldItems)}: {args.OldItems}");
                logger.Debug($"    {nameof(args.NewStartRow)}: {args.NewStartRow}");
                logger.Debug($"    {nameof(args.NewStartColumn)}: {args.NewStartColumn}");
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

        private delegate void ActionCore(TwoDimensionalList<TestRecord> instance,
            IEnumerable<TestRecord>[] operationItems);

        private class NotifyCollectionChangedEventArgsDic
        {
            private Dictionary<string, List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>> Impl
            {
                get;
            }

            public NotifyCollectionChangedEventArgsDic()
            {
                Impl = new Dictionary<string, List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>>();
                Clear();
            }

            public void Add(params NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>[] args)
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
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyReplaceEventArgs(int argsLength, bool isMultipart,
                int setIndex, TestRecord[][] oldItems, TestRecord[][] setItems)
            {
                if (isMultipart)
                {
                    // Replace が複数回通知されていること
                    Assert.IsTrue(ReplaceEventArgs.Count == argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = ReplaceEventArgs[i];
                        Assert.IsTrue(arg.OldStartingIndex == setIndex + i);
                        Assert.IsTrue(arg.OldItems?.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(oldItems[i], testRecordComparer));
                        Assert.IsTrue(arg.NewStartingIndex == setIndex + i);
                        Assert.IsTrue(arg.NewItems?.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].SequenceEqual(setItems[i], testRecordComparer));
                    }
                }
                else
                {
                    // Replace が1度通知されていること
                    Assert.IsTrue(ReplaceEventArgs.Count == 1);
                    {
                        var arg = ReplaceEventArgs[0];
                        Assert.IsTrue(arg.OldStartingIndex == setIndex);
                        Assert.IsTrue(arg.OldItems?.Count == argsLength);
                        Assert.IsTrue(arg.OldItems.ItemEquals(oldItems, itemListComparer));
                        Assert.IsTrue(arg.NewStartingIndex == setIndex);
                        Assert.IsTrue(arg.NewItems?.Count == argsLength);
                        Assert.IsTrue(arg.NewItems.ItemEquals(setItems, itemListComparer));
                    }
                }
            }

            public void CheckAddEventArgs(int argsLength, bool isMultipart,
                int insertIndex, TestRecord[][] insertItems)
            {
                CheckOnlyAddEventArgs(argsLength, isMultipart, insertIndex, insertItems);
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyAddEventArgs(int argsLength, bool isMultipart,
                int insertIndex, TestRecord[][] insertItems)
            {
                if (isMultipart)
                {
                    // Add が複数回通知されていること
                    Assert.IsTrue(AddEventArgs.Count == argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = AddEventArgs[i];
                        Assert.IsTrue(arg.OldStartingIndex == -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.IsTrue(arg.NewStartingIndex == insertIndex + i);
                        Assert.IsTrue(arg.NewItems?.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].SequenceEqual(insertItems[i], testRecordComparer));
                    }
                }
                else
                {
                    // Add が一度通知されていること
                    Assert.IsTrue(AddEventArgs.Count == 1);
                    {
                        var arg = AddEventArgs[0];
                        Assert.IsTrue(arg.OldStartingIndex == -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.IsTrue(arg.NewStartingIndex == insertIndex);
                        Assert.IsTrue(arg.NewItems?.Count == argsLength);
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
                    Assert.IsTrue(RemoveEventArgs.Count == argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = RemoveEventArgs[i];
                        Assert.IsTrue(arg.OldStartingIndex == removeIndex + i);
                        Assert.IsTrue(arg.OldItems?.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(removeItems[i], testRecordComparer));
                        Assert.IsTrue(arg.NewStartingIndex == -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }
                }
                else
                {
                    // Remove が一度通知されていること
                    Assert.IsTrue(RemoveEventArgs.Count == 1);
                    {
                        var arg = RemoveEventArgs[0];
                        Assert.IsTrue(arg.OldStartingIndex == removeIndex);
                        Assert.IsTrue(arg.OldItems?.Count == argsLength);
                        Assert.IsTrue(arg.OldItems.ItemEquals(removeItems, itemListComparer));
                        Assert.IsTrue(arg.NewStartingIndex == -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }
                }
            }

            public void CheckMoveEventArgs(int argsLenght, bool isMultipart,
                int oldIndex, int newIndex, TestRecord[][] moveItems)
            {
                if (isMultipart)
                {
                    Assert.IsTrue(MoveEventArgs.Count == argsLenght);
                    for (var i = 0; i < argsLenght; i++)
                    {
                        var arg = MoveEventArgs[i];
                        Assert.IsTrue(arg.OldStartingIndex == oldIndex + i);
                        Assert.IsTrue(arg.OldItems?.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(moveItems[i], testRecordComparer));
                        Assert.IsTrue(arg.NewStartingIndex == newIndex + i);
                        Assert.IsTrue(arg.NewItems?.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].SequenceEqual(moveItems[i], testRecordComparer));
                    }

                    Assert.IsTrue(ReplaceEventArgs.Count == 0);
                    Assert.IsTrue(AddEventArgs.Count == 0);
                    Assert.IsTrue(RemoveEventArgs.Count == 0);
                    Assert.IsTrue(ResetEventArgs.Count == 0);
                }
                else
                {
                    Assert.IsTrue(MoveEventArgs.Count == 1);
                    {
                        var arg = MoveEventArgs[0];
                        Assert.IsTrue(arg.OldStartingIndex == oldIndex);
                        Assert.IsTrue(arg.OldItems?.Count == argsLenght);
                        Assert.IsTrue(arg.OldItems.ItemEquals(moveItems, itemListComparer));
                        Assert.IsTrue(arg.NewStartingIndex == newIndex);
                        Assert.IsTrue(arg.NewItems?.Count == argsLenght);
                        Assert.IsTrue(arg.NewItems.ItemEquals(moveItems, itemListComparer));
                    }
                    Assert.IsTrue(ReplaceEventArgs.Count == 0);
                    Assert.IsTrue(AddEventArgs.Count == 0);
                    Assert.IsTrue(RemoveEventArgs.Count == 0);
                    Assert.IsTrue(ResetEventArgs.Count == 0);
                }
            }

            public void CheckRemoveEventArgs(int argsLength, bool isMultipart,
                int removeIndex, TestRecord[][] removeItems)
            {
                if (isMultipart)
                {
                    // Remove が複数回通知されていること
                    Assert.IsTrue(RemoveEventArgs.Count == argsLength);
                    for (var i = 0; i < argsLength; i++)
                    {
                        var arg = RemoveEventArgs[i];
                        Assert.IsTrue(arg.OldStartingIndex == removeIndex + i);
                        Assert.IsTrue(arg.OldItems?.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].SequenceEqual(removeItems[i], testRecordComparer));
                        Assert.IsTrue(arg.NewStartingIndex == -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }

                    Assert.IsTrue(ReplaceEventArgs.Count == 0);
                    Assert.IsTrue(AddEventArgs.Count == 0);
                    Assert.IsTrue(MoveEventArgs.Count == 0);
                    Assert.IsTrue(ResetEventArgs.Count == 0);
                }
                else
                {
                    // Remove が一度通知されていること
                    Assert.IsTrue(RemoveEventArgs.Count == 1);
                    {
                        var arg = RemoveEventArgs[0];
                        Assert.IsTrue(arg.OldStartingIndex == removeIndex);
                        Assert.IsTrue(arg.OldItems?.Count == argsLength);
                        Assert.IsTrue(arg.OldItems.ItemEquals(removeItems, itemListComparer));
                        Assert.IsTrue(arg.NewStartingIndex == -1);
                        Assert.IsTrue(arg.NewItems is null);
                    }
                    Assert.IsTrue(ReplaceEventArgs.Count == 0);
                    Assert.IsTrue(AddEventArgs.Count == 0);
                    Assert.IsTrue(MoveEventArgs.Count == 0);
                    Assert.IsTrue(ResetEventArgs.Count == 0);
                }
            }

            public void CheckResetEventArgs(int startIndex,
                TestRecord[][] oldItems, TestRecord[][] newItems)
            {
                // Reset が一度通知されていること
                Assert.IsTrue(ResetEventArgs.Count == 1);
                {
                    var arg = ResetEventArgs[0];
                    Assert.IsTrue(arg.OldStartingIndex == startIndex);
                    Assert.IsTrue(arg.OldItems?.Count == oldItems.Length);
                    Assert.IsTrue(arg.OldItems.ItemEquals(oldItems, itemListComparer));
                    Assert.IsTrue(arg.NewStartingIndex == startIndex);
                    Assert.IsTrue(arg.NewItems?.Count == newItems.Length);
                    Assert.IsTrue(arg.NewItems.ItemEquals(newItems, itemListComparer));
                }
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
            }

            #endregion

            private List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>> ReplaceEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Replace)];

            private List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>> AddEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Add)];

            private List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>> RemoveEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Remove)];

            private List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>> ResetEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Reset)];

            private List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>> MoveEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Move)];

            private void Clear()
            {
                Impl.Clear();
                Impl.Add(nameof(NotifyCollectionChangedAction.Replace),
                    new List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Add),
                    new List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Remove),
                    new List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Reset),
                    new List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Move),
                    new List<NotifyCollectionChangedEventArgsEx<IReadOnlyList<TestRecord>>>());
            }
        }

        private class NotifyTwoDimensionalListChangedEventArgsDic
        {
            private Dictionary<string, List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>> Impl { get; }

            public NotifyTwoDimensionalListChangedEventArgsDic()
            {
                Impl = new Dictionary<string, List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>>();
                Clear();
            }

            public void Add(params TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>[] args)
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

            public void CheckReplaceEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int rowIndex, int columnIndex, TestRecord[][] oldItems, TestRecord[][] setItems)
            {
                CheckOnlyReplaceEventArgs(execDirection, groupingType, rowIndex,
                    columnIndex, oldItems, setItems);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyReplaceEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int rowIndex, int columnIndex, TestRecord[][] oldItems, TestRecord[][] setItems)
            {
                var needTranspose = NeedTranspose(Direction.Row, execDirection);

                var oldItemOuterLength = oldItems.Length;
                var oldItemInnerLength = oldItems.GetInnerArrayLength();
                var oldItemRowLength = needTranspose ? oldItemInnerLength : oldItemOuterLength;
                var oldItemColumnLength = needTranspose ? oldItemOuterLength : oldItemInnerLength;

                var setItemOuterLength = setItems.Length;
                var setItemInnerLength = setItems.GetInnerArrayLength();
                var setItemRowLength = needTranspose ? setItemInnerLength : setItemOuterLength;
                var setItemColumnLength = needTranspose ? setItemOuterLength : setItemInnerLength;

                var fixedOldItems = oldItems.ToTransposedArrayIf(needTranspose);
                var fixedSetItems = setItems.ToTransposedArrayIf(needTranspose);

                if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.None)
                {
                    // Replace が複数回通知されていること
                    Assert.IsTrue(ReplaceEventArgs.Count == setItemRowLength * setItemColumnLength);
                    for (var i = 0; i < setItemRowLength; i++)
                    for (var j = 0; j < setItemColumnLength; j++)
                    {
                        var arg = ReplaceEventArgs[i * setItemColumnLength + j];
                        Assert.IsTrue(arg.Direction == Direction.None);
                        Assert.IsTrue(arg.OldStartRow == rowIndex + i);
                        Assert.IsTrue(arg.OldStartColumn == columnIndex + j);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0][0].Equals(fixedOldItems[i][j]));
                        Assert.IsTrue(arg.NewStartRow == rowIndex + i);
                        Assert.IsTrue(arg.NewStartColumn == columnIndex + j);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0][0].Equals(fixedSetItems[i][j]));
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Row
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection == Direction.Row)
                {
                    // Replace が変更した行数回通知されていること

                    Assert.IsTrue(ReplaceEventArgs.Count == setItemRowLength);
                    Assert.IsTrue(setItemRowLength == oldItemRowLength);
                    for (var i = 0; i < setItemRowLength; i++)
                    {
                        var arg = ReplaceEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Row);
                        Assert.IsTrue(arg.OldStartRow == rowIndex + i);
                        Assert.IsTrue(arg.OldStartColumn == columnIndex);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Count == oldItemColumnLength);
                        Assert.IsTrue(arg.NewStartRow == rowIndex + i);
                        Assert.IsTrue(arg.NewStartColumn == columnIndex);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Count == setItemColumnLength);

                        for (var j = 0; j < oldItemColumnLength; j++)
                        {
                            Assert.IsTrue(arg.OldItems[0][j].Equals(fixedOldItems[i][j]));
                        }

                        for (var j = 0; j < setItemColumnLength; j++)
                        {
                            Assert.IsTrue(arg.NewItems[0][j].Equals(fixedSetItems[i][j]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Column
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection == Direction.Column)
                {
                    // Replace が変更した列数回通知されていること
                    Assert.IsTrue(ReplaceEventArgs.Count == setItemColumnLength);
                    Assert.IsTrue(setItemColumnLength == oldItemColumnLength);
                    for (var j = 0; j < setItemColumnLength; j++)
                    {
                        var arg = ReplaceEventArgs[j];
                        Assert.IsTrue(arg.Direction == Direction.Column);
                        Assert.IsTrue(arg.OldStartRow == rowIndex);
                        Assert.IsTrue(arg.OldStartColumn == columnIndex + j);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Count == oldItemRowLength);
                        Assert.IsTrue(arg.NewStartRow == rowIndex);
                        Assert.IsTrue(arg.NewStartColumn == columnIndex + j);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Count == setItemRowLength);

                        for (var i = 0; i < oldItemRowLength; i++)
                        {
                            Assert.IsTrue(arg.OldItems[0][i].Equals(fixedOldItems[i][j]));
                        }

                        for (var i = 0; i < setItemRowLength; i++)
                        {
                            Assert.IsTrue(arg.NewItems[0][i].Equals(fixedSetItems[i][j]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.All
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection == Direction.None)
                {
                    // Replace が一度通知されていること

                    Assert.IsTrue(ReplaceEventArgs.Count == 1);
                    {
                        var arg = ReplaceEventArgs[0];
                        Assert.IsTrue(arg.Direction == execDirection);
                        Assert.IsTrue(arg.OldStartRow == rowIndex);
                        Assert.IsTrue(arg.OldStartColumn == columnIndex);
                        Assert.IsTrue(arg.OldItems!.Count == setItemOuterLength);
                        Assert.IsTrue(arg.OldItems[0].Count == setItemInnerLength);
                        Assert.IsTrue(arg.NewStartRow == rowIndex);
                        Assert.IsTrue(arg.NewStartColumn == columnIndex);
                        Assert.IsTrue(arg.NewItems!.Count == setItemOuterLength);
                        Assert.IsTrue(arg.NewItems[0].Count == setItemInnerLength);

                        var itemsNeedTranspose = NeedTranspose(execDirection, arg.Direction);
                        var origOldItems = oldItems.ToTransposedArrayIf(itemsNeedTranspose);

                        for (var i = 0; i < arg.OldItems.Count; i++)
                        for (var j = 0; j < arg.OldItems[0].Count; j++)
                        {
                            Assert.IsTrue(arg.OldItems[i][j].Equals(origOldItems[i][j]));
                            Assert.IsTrue(arg.NewItems[i][j].Equals(setItems[i][j]));
                        }
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }

            public void CheckAddEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int insertIndex, TestRecord[][] insertItems)
            {
                CheckOnlyAddEventArgs(execDirection, groupingType, insertIndex, insertItems);
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyAddEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int insertIndex, TestRecord[][] insertItems)
            {
                var insertItemsOuterLength = insertItems.Length;
                var insertItemsInnerLength = insertItems.GetInnerArrayLength();
                var insertItemsRowLength = execDirection != Direction.Column
                    ? insertItemsOuterLength
                    : insertItemsInnerLength;
                var insertItemsColumnLength = execDirection != Direction.Column
                    ? insertItemsInnerLength
                    : insertItemsOuterLength;

                if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.None)
                {
                    // Add が複数回通知されていること
                    Assert.IsTrue(AddEventArgs.Count == insertItemsRowLength * insertItemsColumnLength);

                    for (var i = 0; i < insertItemsRowLength; i++)
                    for (var j = 0; j < insertItemsColumnLength; j++)
                    {
                        var arg = AddEventArgs[i * insertItemsColumnLength + j];
                        Assert.IsTrue(arg.Direction == Direction.None);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.IsTrue(arg.OldStartRow == -1);
                        Assert.IsTrue(arg.OldStartColumn == -1);
                        Assert.IsTrue(arg.NewItems!.Count == 1);

                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.NewStartRow == insertIndex + i);
                            Assert.IsTrue(arg.NewStartColumn == j);
                            Assert.IsTrue(arg.NewItems[0][0].Equals(insertItems[i][j]));
                        }
                        else
                        {
                            Assert.IsTrue(arg.NewStartRow == i);
                            Assert.IsTrue(arg.NewStartColumn == insertIndex + j);
                            Assert.IsTrue(arg.NewItems[0][0].Equals(insertItems[j][i]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Row
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection != Direction.Column)
                {
                    // Add が追加した行数回通知されていること

                    Assert.IsTrue(AddEventArgs.Count == insertItemsRowLength);
                    for (var i = 0; i < insertItemsRowLength; i++)
                    {
                        var arg = AddEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Row);
                        Assert.IsTrue(arg.OldStartRow == -1);
                        Assert.IsTrue(arg.OldStartColumn == -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Count == insertItemsColumnLength);
                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.NewStartRow == insertIndex + i);
                            Assert.IsTrue(arg.NewStartColumn == 0);
                        }
                        else
                        {
                            Assert.IsTrue(arg.NewStartRow == i);
                            Assert.IsTrue(arg.NewStartColumn == insertIndex);
                        }

                        for (var j = 0; j < insertItemsColumnLength; j++)
                        {
                            Assert.IsTrue(execDirection == Direction.Row
                                ? arg.NewItems[0][j].Equals(insertItems[i][j])
                                : arg.NewItems[0][j].Equals(insertItems[j][i]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Column
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection == Direction.Column)
                {
                    // Add が追加した列数回通知されていること

                    Assert.IsTrue(AddEventArgs.Count == insertItemsColumnLength);
                    for (var i = 0; i < insertItemsColumnLength; i++)
                    {
                        var arg = AddEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Column);
                        Assert.IsTrue(arg.OldStartRow == -1);
                        Assert.IsTrue(arg.OldStartColumn == -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Count == insertItemsRowLength);
                        if (execDirection == Direction.Column)
                        {
                            Assert.IsTrue(arg.NewStartRow == 0);
                            Assert.IsTrue(arg.NewStartColumn == insertIndex + i);
                        }
                        else
                        {
                            Assert.IsTrue(arg.NewStartRow == insertIndex);
                            Assert.IsTrue(arg.NewStartColumn == i);
                        }

                        for (var j = 0; j < insertItemsRowLength; j++)
                        {
                            Assert.IsTrue(execDirection == Direction.Column
                                ? arg.NewItems[0][j].Equals(insertItems[i][j])
                                : arg.NewItems[0][j].Equals(insertItems[j][i]));
                        }
                    }
                }
                else
                {
                    // Add が一度通知されていること

                    Assert.IsTrue(AddEventArgs.Count == 1);
                    {
                        var arg = AddEventArgs[0];
                        Assert.IsTrue(arg.Direction == execDirection);
                        Assert.IsTrue(arg.OldStartRow == -1);
                        Assert.IsTrue(arg.OldStartColumn == -1);
                        Assert.IsTrue(arg.OldItems is null);
                        Assert.IsTrue(arg.NewItems!.Count == insertItemsOuterLength);
                        Assert.IsTrue(arg.NewItems[0].Count == insertItemsInnerLength);

                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.NewStartRow == insertIndex);
                            Assert.IsTrue(arg.NewStartColumn == 0);
                        }
                        else
                        {
                            Assert.IsTrue(arg.NewStartRow == 0);
                            Assert.IsTrue(arg.NewStartColumn == insertIndex);
                        }

                        for (var i = 0; i < insertItemsOuterLength; i++)
                        for (var j = 0; j < insertItemsInnerLength; j++)
                        {
                            Assert.IsTrue(arg.NewItems[i][j].Equals(insertItems[i][j]));
                        }
                    }
                }
            }

            public void CheckAddEventArgsForAdjustLengthBoth(
                Direction execDirection,
                int oldRowLength, int oldColumnLength,
                TestRecord[][] addRowItems, TestRecord[][] addColumnItems)
            {
                var addRowLength = addRowItems.Length;
                var addColumnLength = addColumnItems.Length;
                var addedRowLength = oldRowLength + addRowLength;
                var addedColumnLength = oldColumnLength + addColumnLength;

                var addItemLength = oldRowLength * addColumnLength
                                    + addRowLength * addedColumnLength;

                Assert.IsTrue(AddEventArgs.Count == addItemLength);

                if (execDirection == Direction.Row)
                {
                    var i = 0;
                    {
                        var fixedColumnItems = addColumnItems.ToTransposedArray()
                            .Take(oldRowLength).ToArray();

                        for (var rIdx = 0; rIdx < oldRowLength; rIdx++)
                        for (var cOffset = 0; cOffset < addColumnLength; cOffset++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Add);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == rIdx);
                            Assert.IsTrue(args.OldStartColumn == oldColumnLength + cOffset);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == fixedColumnItems[rIdx][cOffset]);
                        }
                    }
                    {
                        for (var rOffset = 0; rOffset < addRowLength; rOffset++)
                        for (var cIdx = 0; cIdx < addedColumnLength; cIdx++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Add);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == oldRowLength + rOffset);
                            Assert.IsTrue(args.OldStartColumn == cIdx);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == addRowItems[rOffset][cIdx]);
                        }
                    }
                }
                else
                {
                    var i = 0;
                    {
                        var rowItems = addRowItems.Take(oldColumnLength).ToArray();

                        for (var cIdx = 0; cIdx < oldColumnLength; cIdx++)
                        for (var rOffset = 0; rOffset < addRowLength; rOffset++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Add);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == rOffset);
                            Assert.IsTrue(args.OldStartColumn == oldColumnLength + cIdx);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == rowItems[rOffset][cIdx]);
                        }
                    }
                    {
                        for (var cOffset = 0; cOffset < addColumnLength; cOffset++)
                        for (var rIdx = 0; rIdx < addedRowLength; rIdx++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Add);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == oldRowLength + rIdx);
                            Assert.IsTrue(args.OldStartColumn == cOffset);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == addColumnItems[cOffset][rIdx]);
                        }
                    }
                }
            }

            public void CheckMoveEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int oldIndex, int newIndex, TestRecord[][] moveItems)
            {
                if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.None)
                {
                    var needTranspose = NeedTranspose(execDirection, Direction.Row);
                    var fixedMoveItems = moveItems.ToTransposedArrayIf(needTranspose);
                    var moveItemRowLength = fixedMoveItems.Length;
                    var moveItemColumnLength = fixedMoveItems.GetInnerArrayLength();

                    // Move が複数回通知されていること
                    Assert.IsTrue(MoveEventArgs.Count == moveItemRowLength * moveItemColumnLength);
                    for (var i = 0; i < moveItemRowLength; i++)
                    for (var j = 0; j < moveItemColumnLength; j++)
                    {
                        var arg = MoveEventArgs[i * moveItemColumnLength + j];
                        Assert.IsTrue(arg.Direction == Direction.None);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0][0].Equals(fixedMoveItems[i][j]));
                        Assert.IsTrue(arg.NewItems[0][0].Equals(fixedMoveItems[i][j]));

                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.OldStartRow == oldIndex + i);
                            Assert.IsTrue(arg.OldStartColumn == j);
                            Assert.IsTrue(arg.NewStartRow == newIndex + i);
                            Assert.IsTrue(arg.NewStartColumn == j);
                        }
                        else
                        {
                            Assert.IsTrue(arg.OldStartRow == i);
                            Assert.IsTrue(arg.OldStartColumn == oldIndex + j);
                            Assert.IsTrue(arg.NewStartRow == i);
                            Assert.IsTrue(arg.NewStartColumn == newIndex + j);
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Row
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection != Direction.Column)
                {
                    // Move が移動した行数回通知されていること

                    var needTranspose = NeedTranspose(execDirection, Direction.Row);
                    var fixedMoveItems = moveItems.ToTransposedArrayIf(needTranspose);
                    var moveItemRowLength = fixedMoveItems.Length;
                    var moveItemColumnLength = fixedMoveItems.GetInnerArrayLength();

                    Assert.IsTrue(MoveEventArgs.Count == moveItemRowLength);
                    for (var i = 0; i < moveItemRowLength; i++)
                    {
                        var arg = MoveEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Row);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Count == moveItemColumnLength);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Count == moveItemColumnLength);
                        Assert.IsTrue(arg.OldStartRow == oldIndex + i);
                        Assert.IsTrue(arg.OldStartColumn == 0);
                        Assert.IsTrue(arg.NewStartRow == newIndex + i);
                        Assert.IsTrue(arg.NewStartColumn == 0);

                        for (var j = 0; j < moveItemColumnLength; j++)
                        {
                            Assert.IsTrue(arg.OldItems[0][j].Equals(fixedMoveItems[i][j]));
                            Assert.IsTrue(arg.NewItems[0][j].Equals(fixedMoveItems[i][j]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Column
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection == Direction.Column)
                {
                    // Move が追加した列数回通知されていること

                    var needTranspose = NeedTranspose(execDirection, Direction.Column);
                    var fixedMoveItems = moveItems.ToTransposedArrayIf(needTranspose);
                    var moveItemRowLength = fixedMoveItems.GetInnerArrayLength();
                    var moveItemColumnLength = fixedMoveItems.Length;

                    Assert.IsTrue(MoveEventArgs.Count == moveItemColumnLength);
                    for (var i = 0; i < moveItemColumnLength; i++)
                    {
                        var arg = MoveEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Column);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Count == moveItemRowLength);
                        Assert.IsTrue(arg.NewItems!.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Count == moveItemRowLength);
                        Assert.IsTrue(arg.OldStartRow == 0);
                        Assert.IsTrue(arg.OldStartColumn == oldIndex + i);
                        Assert.IsTrue(arg.NewStartRow == 0);
                        Assert.IsTrue(arg.NewStartColumn == newIndex + i);

                        for (var j = 0; j < moveItemRowLength; j++)
                        {
                            Assert.IsTrue(arg.OldItems[0][j].Equals(fixedMoveItems[i][j]));
                            Assert.IsTrue(arg.NewItems[0][j].Equals(fixedMoveItems[i][j]));
                        }
                    }
                }
                else
                {
                    // Move が一度通知されていること

                    var moveItemOuterLength = moveItems.Length;
                    var moveItemInnerLength = moveItems.GetInnerArrayLength();

                    Assert.IsTrue(MoveEventArgs.Count == 1);
                    {
                        var arg = MoveEventArgs[0];
                        Assert.IsTrue(arg.Direction == execDirection);
                        Assert.IsTrue(arg.OldItems!.Count == moveItemOuterLength);
                        Assert.IsTrue(arg.OldItems[0].Count == moveItemInnerLength);
                        Assert.IsTrue(arg.NewItems!.Count == moveItemOuterLength);
                        Assert.IsTrue(arg.NewItems[0].Count == moveItemInnerLength);

                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.OldStartRow == oldIndex);
                            Assert.IsTrue(arg.OldStartColumn == 0);
                            Assert.IsTrue(arg.NewStartRow == newIndex);
                            Assert.IsTrue(arg.NewStartColumn == 0);
                        }
                        else
                        {
                            Assert.IsTrue(arg.OldStartRow == 0);
                            Assert.IsTrue(arg.OldStartColumn == oldIndex);
                            Assert.IsTrue(arg.NewStartRow == 0);
                            Assert.IsTrue(arg.NewStartColumn == newIndex);
                        }

                        for (var i = 0; i < moveItemOuterLength; i++)
                        for (var j = 0; j < moveItemInnerLength; j++)
                        {
                            Assert.IsTrue(arg.NewItems[i][j].Equals(moveItems[i][j]));
                        }
                    }
                }

                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckRemoveEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int removeIndex, TestRecord[][] removeItems)
            {
                CheckOnlyRemoveEventArgs(execDirection, groupingType, removeIndex, removeItems);
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyRemoveEventArgs(Direction execDirection,
                NotifyTwoDimensionalListChangeEventGroupingType groupingType,
                int removeIndex, TestRecord[][] removeItems)
            {
                var removeItemsOuterLength = removeItems.Length;
                var removeItemsInnerLength = removeItems.GetInnerArrayLength();
                var removeItemsRowLength = execDirection != Direction.Column
                    ? removeItemsOuterLength
                    : removeItemsInnerLength;
                var removeItemsColumnLength = execDirection != Direction.Column
                    ? removeItemsInnerLength
                    : removeItemsOuterLength;

                if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.None)
                {
                    // Remove が複数回通知されていること
                    Assert.IsTrue(RemoveEventArgs.Count == removeItemsRowLength * removeItemsColumnLength);
                    for (var i = 0; i < removeItemsRowLength; i++)
                    for (var j = 0; j < removeItemsColumnLength; j++)
                    {
                        var arg = RemoveEventArgs[i * removeItemsColumnLength + j];
                        Assert.IsTrue(arg.Direction == Direction.None);
                        Assert.IsTrue(arg.NewItems is null);
                        Assert.IsTrue(arg.NewStartRow == -1);
                        Assert.IsTrue(arg.NewStartColumn == -1);
                        Assert.IsTrue(arg.OldItems!.Count == 1);

                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.OldStartRow == removeIndex + i);
                            Assert.IsTrue(arg.OldStartColumn == j);
                            Assert.IsTrue(arg.OldItems[0][0].Equals(removeItems[i][j]));
                        }
                        else
                        {
                            Assert.IsTrue(arg.OldStartRow == i);
                            Assert.IsTrue(arg.OldStartColumn == removeIndex + j);
                            Assert.IsTrue(arg.OldItems[0][0].Equals(removeItems[j][i]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Row
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection != Direction.Column)
                {
                    // Remove が追加した行数回通知されていること

                    Assert.IsTrue(RemoveEventArgs.Count == removeItemsRowLength);
                    for (var i = 0; i < removeItemsRowLength; i++)
                    {
                        var arg = RemoveEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Row);
                        Assert.IsTrue(arg.NewStartRow == -1);
                        Assert.IsTrue(arg.NewStartColumn == -1);
                        Assert.IsTrue(arg.NewItems is null);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Count == removeItemsColumnLength);
                        if (execDirection == Direction.Row)

                        {
                            Assert.IsTrue(arg.OldStartRow == removeIndex + i);
                            Assert.IsTrue(arg.OldStartColumn == 0);
                        }
                        else
                        {
                            Assert.IsTrue(arg.OldStartRow == 0);
                            Assert.IsTrue(arg.OldStartColumn == removeIndex + i);
                        }

                        for (var j = 0; j < removeItemsColumnLength; j++)
                        {
                            Assert.IsTrue(execDirection == Direction.Row
                                ? arg.OldItems[0][j].Equals(removeItems[i][j])
                                : arg.OldItems[0][j].Equals(removeItems[j][i]));
                        }
                    }
                }
                else if (groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Column
                         || groupingType == NotifyTwoDimensionalListChangeEventGroupingType.Direct &&
                         execDirection == Direction.Column)
                {
                    // Remove が追加した列数回通知されていること

                    Assert.IsTrue(RemoveEventArgs.Count == removeItemsColumnLength);
                    for (var i = 0; i < removeItemsColumnLength; i++)
                    {
                        var arg = RemoveEventArgs[i];
                        Assert.IsTrue(arg.Direction == Direction.Column);
                        Assert.IsTrue(arg.NewStartRow == -1);
                        Assert.IsTrue(arg.NewStartColumn == -1);
                        Assert.IsTrue(arg.NewItems is null);
                        Assert.IsTrue(arg.OldItems!.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Count == removeItemsRowLength);

                        if (execDirection == Direction.Column)
                        {
                            Assert.IsTrue(arg.OldStartRow == 0);
                            Assert.IsTrue(arg.OldStartColumn == removeIndex + i);
                        }
                        else
                        {
                            Assert.IsTrue(arg.OldStartRow == removeIndex + i);
                            Assert.IsTrue(arg.OldStartColumn == 0);
                        }

                        for (var j = 0; j < removeItemsRowLength; j++)
                        {
                            Assert.IsTrue(execDirection == Direction.Column
                                ? arg.OldItems[0][j].Equals(removeItems[i][j])
                                : arg.OldItems[0][j].Equals(removeItems[j][i]));
                        }
                    }
                }
                else
                {
                    // Remove が一度通知されていること

                    Assert.IsTrue(RemoveEventArgs.Count == 1);
                    {
                        var arg = RemoveEventArgs[0];
                        Assert.IsTrue(arg.Direction == execDirection);
                        Assert.IsTrue(arg.NewStartRow == -1);
                        Assert.IsTrue(arg.NewStartColumn == -1);
                        Assert.IsTrue(arg.NewItems is null);
                        Assert.IsTrue(arg.OldItems!.Count == removeItemsOuterLength);
                        Assert.IsTrue(arg.OldItems[0].Count == removeItemsInnerLength);

                        if (execDirection == Direction.Row)
                        {
                            Assert.IsTrue(arg.OldStartRow == removeIndex);
                            Assert.IsTrue(arg.OldStartColumn == 0);
                        }
                        else
                        {
                            Assert.IsTrue(arg.OldStartRow == 0);
                            Assert.IsTrue(arg.OldStartColumn == removeIndex);
                        }

                        for (var i = 0; i < removeItemsOuterLength; i++)
                        for (var j = 0; j < removeItemsInnerLength; j++)
                        {
                            Assert.IsTrue(arg.OldItems[i][j].Equals(removeItems[i][j]));
                        }
                    }
                }
            }

            public void CheckRemoveEventArgsForAdjustLengthBoth(
                Direction execDirection,
                int adjustRowLength, int adjustColumnLength,
                TestRecord[][] removeRowItems, TestRecord[][] removeColumnItems)
            {
                var removeRowLength = removeRowItems.Length;
                var removeColumnLength = removeColumnItems.Length;
                var oldRowLength = adjustRowLength + removeRowLength;
                var oldColumnLength = adjustColumnLength + removeColumnLength;

                var removeItemLength = adjustRowLength * removeColumnLength
                                       + removeRowLength * oldColumnLength;

                Assert.IsTrue(AddEventArgs.Count == removeItemLength);

                if (execDirection == Direction.Row)
                {
                    var i = 0;
                    {
                        var fixedColumnItems = removeColumnItems.ToTransposedArray()
                            .Take(adjustRowLength).ToArray();

                        for (var rIdx = 0; rIdx < adjustRowLength; rIdx++)
                        for (var cOffset = 0; cOffset < removeColumnLength; cOffset++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Remove);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == rIdx);
                            Assert.IsTrue(args.OldStartColumn == adjustColumnLength + cOffset);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == fixedColumnItems[rIdx][cOffset]);
                        }
                    }
                    {
                        for (var rOffset = 0; rOffset < removeRowLength; rOffset++)
                        for (var cIdx = 0; cIdx < oldColumnLength; cIdx++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Remove);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == adjustRowLength + rOffset);
                            Assert.IsTrue(args.OldStartColumn == cIdx);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == removeRowItems[rOffset][cIdx]);
                        }
                    }
                }
                else
                {
                    var i = 0;
                    {
                        var rowItems = removeRowItems.Take(adjustColumnLength).ToArray();

                        for (var cIdx = 0; cIdx < removeColumnLength; cIdx++)
                        for (var rOffset = 0; rOffset < adjustRowLength; rOffset++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Add);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == rOffset);
                            Assert.IsTrue(args.OldStartColumn == adjustColumnLength + cIdx);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == rowItems[rOffset][cIdx]);
                        }
                    }
                    {
                        for (var cOffset = 0; cOffset < removeColumnLength; cOffset++)
                        for (var rIdx = 0; rIdx < oldRowLength; rIdx++)
                        {
                            var args = AddEventArgs[i++];
                            Assert.IsTrue(args.Action == TwoDimensionalCollectionChangeAction.Add);
                            Assert.IsTrue(args.Direction == Direction.None);
                            Assert.IsTrue(args.OldStartRow == -1);
                            Assert.IsTrue(args.OldStartColumn == -1);
                            Assert.IsTrue(args.OldItems is null);
                            Assert.IsTrue(args.OldStartRow == oldRowLength + rIdx);
                            Assert.IsTrue(args.OldStartColumn == cOffset);
                            Assert.IsTrue(args.OldItems!.Count == 1);
                            Assert.IsTrue(args.OldItems[0].Count == 1);
                            Assert.IsTrue(args.OldItems[0][0] == removeColumnItems[cOffset][rIdx]);
                        }
                    }
                }
            }

            public void CheckResetEventArgs(int startIndex,
                TestRecord[][] oldItems, TestRecord[][] newItems, Direction notifyDirection)
            {
                // Reset が一度通知されていること
                Assert.IsTrue(ResetEventArgs.Count == 1);

                {
                    var oldItemsOuterLength = oldItems.Length;
                    var oldItemsInnerLength = oldItems.GetInnerArrayLength();
                    var newItemsOuterLength = newItems.Length;
                    var newItemsInnerLength = newItems.GetInnerArrayLength();

                    var arg = ResetEventArgs[0];

                    if (notifyDirection == Direction.Row)
                    {
                        Assert.IsTrue(arg.Direction == Direction.Row);
                        Assert.IsTrue(arg.OldStartRow == startIndex);
                        Assert.IsTrue(arg.OldStartColumn == 0);
                        Assert.IsTrue(arg.NewStartRow == startIndex);
                        Assert.IsTrue(arg.NewStartColumn == 0);
                        Assert.IsTrue(arg.OldItems!.Count == oldItemsOuterLength);
                        Assert.IsTrue(arg.OldItems[0].Count == oldItemsInnerLength);

                        for (var i = 0; i < oldItemsOuterLength; i++)
                        for (var j = 0; j < oldItemsInnerLength; j++)
                        {
                            Assert.IsTrue(arg.OldItems[i][j].Equals(oldItems[i][j]));
                        }

                        Assert.IsTrue(arg.NewItems!.Count == newItemsOuterLength);
                        Assert.IsTrue(arg.NewItems[0].Count == newItemsInnerLength);

                        for (var i = 0; i < newItemsOuterLength; i++)
                        for (var j = 0; j < newItemsInnerLength; j++)
                        {
                            Assert.IsTrue(arg.NewItems[i][j].Equals(newItems[i][j]));
                        }
                    }
                    else
                    {
                        // notifyDirection == .Column

                        Assert.IsTrue(arg.Direction == Direction.Column);
                        Assert.IsTrue(arg.OldStartRow == 0);
                        Assert.IsTrue(arg.OldStartColumn == startIndex);
                        Assert.IsTrue(arg.NewStartRow == 0);
                        Assert.IsTrue(arg.NewStartColumn == startIndex);
                        Assert.IsTrue(arg.OldItems!.Count == oldItemsOuterLength);
                        Assert.IsTrue(arg.OldItems[0].Count == oldItemsInnerLength);
                        for (var i = 0; i < oldItemsInnerLength; i++)
                        for (var j = 0; j < oldItemsOuterLength; j++)
                        {
                            Assert.IsTrue(arg.OldItems[j][i].Equals(oldItems[j][i]));
                        }

                        Assert.IsTrue(arg.NewItems!.Count == newItemsOuterLength);
                        Assert.IsTrue(arg.NewItems[0].Count == newItemsInnerLength);

                        for (var i = 0; i < newItemsInnerLength; i++)
                        for (var j = 0; j < newItemsOuterLength; j++)
                        {
                            Assert.IsTrue(arg.NewItems[j][i].Equals(newItems[j][i]));
                        }
                    }
                }
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
            }

            #endregion

            private List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>> ReplaceEventArgs =>
                Impl[nameof(TwoDimensionalCollectionChangeAction.Replace)];

            private List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>> AddEventArgs =>
                Impl[nameof(TwoDimensionalCollectionChangeAction.Add)];

            private List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>> RemoveEventArgs =>
                Impl[nameof(TwoDimensionalCollectionChangeAction.Remove)];

            private List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>> ResetEventArgs =>
                Impl[nameof(TwoDimensionalCollectionChangeAction.Reset)];

            private List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>> MoveEventArgs =>
                Impl[nameof(TwoDimensionalCollectionChangeAction.Move)];

            private void Clear()
            {
                Impl.Clear();
                Impl.Add(nameof(TwoDimensionalCollectionChangeAction.Replace),
                    new List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>());
                Impl.Add(nameof(TwoDimensionalCollectionChangeAction.Add),
                    new List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>());
                Impl.Add(nameof(TwoDimensionalCollectionChangeAction.Remove),
                    new List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>());
                Impl.Add(nameof(TwoDimensionalCollectionChangeAction.Reset),
                    new List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>());
                Impl.Add(nameof(TwoDimensionalCollectionChangeAction.Move),
                    new List<TwoDimensionalCollectionChangeEventInternalArgs<TestRecord>>());
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
