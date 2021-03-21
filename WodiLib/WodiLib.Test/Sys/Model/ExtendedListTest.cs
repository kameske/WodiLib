using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class ExtendedListTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region SingleAction

        private static readonly object[] NotifyCollectionChangeEventArgsTestCaseSource =
        {
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.None.Id},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Once.Id},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Simple.Id},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Single.Id},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Multi.Id},
            new object[] {NotifyCollectionChangeEventType.Once.Id, NotifyCollectionChangeEventType.None.Id},
            new object[] {NotifyCollectionChangeEventType.Simple.Id, NotifyCollectionChangeEventType.None.Id},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.None.Id},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.None.Id},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.Once.Id},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.Multi.Id},
        };

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Get(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int getIndex = 2;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var _ = instance[getIndex];

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (_, dic) =>
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_GetRange(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int getIndex = 2;
            const int count = 3;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var _ = instance.GetRange(getIndex, count);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (_, dic) =>
                {
                    // 通知が行われていないこと
                    Assert.IsTrue(dic.IsEmpty);
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Set(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int setIndex = 3;
            const string setValue = "new value";

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var oldValue = instance[setIndex];
            instance[setIndex] = setValue;

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Replace が一度通知されていること
                        dic.CheckReplaceEventArgs(1, false, setIndex, new[] {oldValue}, new[] {setValue});
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_SetRange(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int setIndex = 3;
            string[] setValues = {"new value", "NEW VALUE 2"};
            var setValueLength = setValues.Length;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var oldValues = instance.GetRange(setIndex, setValueLength).ToArray();
            instance.SetRange(setIndex, setValues);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Replace が一度通知されていること
                        dic.CheckReplaceEventArgs(setValueLength, false, setIndex, oldValues, setValues);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Replace が複数回通知されていること
                        dic.CheckReplaceEventArgs(setValueLength, true, setIndex, oldValues, setValues);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Add(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const string addValue = "new value";

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var beforeLength = instance.Count;
            instance.Add(addValue);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Add が一度通知されていること
                        dic.CheckAddEventArgs(1, false, beforeLength, new[] {addValue});
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_AddRange(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            string[] addValues = {"new value", "NEW VALUE 2"};
            var addValueLength = addValues.Length;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var beforeLength = instance.Count;
            instance.AddRange(addValues);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Add が一度通知されていること
                        dic.CheckAddEventArgs(addValueLength, false, beforeLength, addValues);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Add が複数回通知されていること
                        dic.CheckAddEventArgs(addValueLength, true, beforeLength, addValues);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Insert(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int insertIndex = 7;
            const string insertValue = "new value";

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            instance.Insert(insertIndex, insertValue);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Add が一度通知されていること
                        dic.CheckAddEventArgs(1, false, insertIndex, new[] {insertValue});
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_InsertRange(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int insertIndex = 7;
            string[] insertValues = {"new value", "NEW VALUE 2"};
            var insertValueLength = insertValues.Length;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            instance.InsertRange(insertIndex, insertValues);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Add が一度通知されていること
                        dic.CheckAddEventArgs(insertValueLength, false, insertIndex, insertValues);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Add が複数回通知されていること
                        dic.CheckAddEventArgs(insertValueLength, true, insertIndex, insertValues);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Move(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int oldIndex = 2;
            const int newIndex = 6;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var moveItem = instance[oldIndex];

            instance.Move(oldIndex, newIndex);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Move が一度通知されていること
                        dic.CheckMoveEventArgs(1, false, oldIndex, newIndex, new[] {moveItem});
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_MoveRange(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int oldIndex = 2;
            const int newIndex = 6;
            const int count = 3;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var moveItems = instance.GetRange(oldIndex, count).ToArray();

            instance.MoveRange(oldIndex, newIndex, count);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Move が一度通知されていること
                        dic.CheckMoveEventArgs(count, false, oldIndex, newIndex, moveItems);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Add が複数回通知されていること
                        dic.CheckMoveEventArgs(count, true, oldIndex, newIndex, moveItems);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Remove(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int removeIndex = 5;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var removeItem = instance[removeIndex];

            instance.RemoveAt(removeIndex);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Remove が一度通知されていること
                        dic.CheckRemoveEventArgs(1, false, removeIndex, new[] {removeItem});
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_RemoveRange(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            const int removeIndex = 5;
            const int count = 3;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var removeItems = instance.GetRange(removeIndex, count).ToArray();

            instance.RemoveRange(removeIndex, count);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Remove が一度通知されていること
                        dic.CheckRemoveEventArgs(count, false, removeIndex, removeItems);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Remove が複数回通知されていること
                        dic.CheckRemoveEventArgs(count, true, removeIndex, removeItems);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Reset(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            var resetItems = MakeStringList(5, i => $"new {i} string.").ToArray();

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var oldItems = instance.ToArray();

            instance.Reset(resetItems);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Reset が一度通知されていること
                        dic.CheckResetEventArgs(0, oldItems, resetItems);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgsTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Clear(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var oldItems = instance.ToArray();

            instance.Clear();

            var clearItems = instance.ToArray();

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Single
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Reset が一度通知されていること
                        dic.CheckResetEventArgs(0, oldItems, clearItems);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        #endregion

        #region MultiAction

        private static readonly object[] NotifyCollectionChangeEventArgs_OverwriteTestCaseSource =
        {
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.None.Id, 5, 2},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Once.Id, 5, 2},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Simple.Id, 5, 2},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Single.Id, 5, 2},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Multi.Id, 5, 2},
            new object[] {NotifyCollectionChangeEventType.Once.Id, NotifyCollectionChangeEventType.None.Id, 8, 5},
            new object[] {NotifyCollectionChangeEventType.Simple.Id, NotifyCollectionChangeEventType.None.Id, 8, 5},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.None.Id, 8, 5},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.None.Id, 8, 5},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.Once.Id, 8, 5},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.Multi.Id, 10, 2},
            new object[] {NotifyCollectionChangeEventType.Once.Id, NotifyCollectionChangeEventType.Simple.Id, 10, 2},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.None.Id, 10, 2},
        };

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgs_OverwriteTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_Overwrite(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId,
            int startIndex, int overwriteItemLength)
        {
            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            var overwriteValues = MakeStringList(overwriteItemLength, i => $"Overwrite item {i}.").ToArray();
            var insertValueLength = Math.Max(0, startIndex + overwriteItemLength - TestListInitLength);
            var replaceValueLength = overwriteItemLength - insertValueLength;
            var insertValues = overwriteValues.Skip(replaceValueLength).ToArray();
            var replaceNewValues = overwriteValues.Take(replaceValueLength).ToArray();
            var actionType = (insertValueLength != 0, replaceValueLength != 0) switch
            {
                (true, true) => OverwriteType.Multi,
                (true, false) => OverwriteType.Add,
                (false, true) => OverwriteType.Replace,
                _ => throw new Exception()
            };

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var replaceOldItems = instance.GetRange(startIndex, replaceValueLength).ToArray();

            instance.Overwrite(startIndex, overwriteValues);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                        return;
                    }

                    switch (actionType)
                    {
                        case OverwriteType.Add:
                            if (eventType == NotifyCollectionChangeEventType.Once
                                || eventType == NotifyCollectionChangeEventType.Single)
                            {
                                // Add が一度通知されていること
                                dic.CheckAddEventArgs(insertValueLength, false, startIndex, overwriteValues);
                            }
                            else if (eventType == NotifyCollectionChangeEventType.Simple
                                     || eventType == NotifyCollectionChangeEventType.Multi)
                            {
                                // Add が複数回通知されていること
                                dic.CheckAddEventArgs(insertValueLength, true, startIndex, overwriteValues);
                            }
                            else
                            {
                                Assert.Fail();
                            }

                            break;

                        case OverwriteType.Replace:
                            if (eventType == NotifyCollectionChangeEventType.Once
                                || eventType == NotifyCollectionChangeEventType.Single)
                            {
                                // Replace が一度通知されていること
                                dic.CheckReplaceEventArgs(replaceValueLength, false, startIndex, replaceOldItems,
                                    replaceNewValues);
                            }
                            else if (eventType == NotifyCollectionChangeEventType.Simple
                                     || eventType == NotifyCollectionChangeEventType.Multi)
                            {
                                // Replace が複数回通知されていること
                                dic.CheckReplaceEventArgs(replaceValueLength, true, startIndex, replaceOldItems,
                                    replaceNewValues);
                            }
                            else
                            {
                                Assert.Fail();
                            }

                            break;

                        case OverwriteType.Multi:
                            if (eventType == NotifyCollectionChangeEventType.Once
                                || eventType == NotifyCollectionChangeEventType.Simple)
                            {
                                // Reset が一度通知されていること
                                dic.CheckResetEventArgs(startIndex, replaceOldItems, overwriteValues);
                            }
                            else if (eventType == NotifyCollectionChangeEventType.Single)
                            {
                                // Add, Replace が一度ずつ通知されていること
                                dic.CheckOnlyReplaceEventArgs(replaceValueLength, false, startIndex, replaceOldItems,
                                    replaceNewValues);
                                dic.CheckOnlyAddEventArgs(insertValueLength, false, TestListInitLength, insertValues);
                                Assert.IsTrue(dic.IsMoveEventEmpty);
                                Assert.IsTrue(dic.IsRemoveEventEmpty);
                                Assert.IsTrue(dic.IsResetEventEmpty);
                            }
                            else if (eventType == NotifyCollectionChangeEventType.Multi)
                            {
                                // Add, Replace が複数回通知されていること
                                dic.CheckOnlyReplaceEventArgs(replaceValueLength, true, startIndex, replaceOldItems,
                                    replaceNewValues);
                                dic.CheckOnlyAddEventArgs(insertValueLength, true, TestListInitLength, insertValues);
                                Assert.IsTrue(dic.IsMoveEventEmpty);
                                Assert.IsTrue(dic.IsRemoveEventEmpty);
                                Assert.IsTrue(dic.IsResetEventEmpty);
                            }
                            else
                            {
                                Assert.Fail();
                            }

                            break;

                        default:
                            Assert.Fail();
                            break;
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        private static readonly object[] NotifyCollectionChangeEventArgs_AdjustLengthTestCaseSource =
        {
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.None.Id, 6},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Once.Id, 6},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Simple.Id, 6},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Single.Id, 6},
            new object[] {NotifyCollectionChangeEventType.None.Id, NotifyCollectionChangeEventType.Multi.Id, 6},
            new object[] {NotifyCollectionChangeEventType.Once.Id, NotifyCollectionChangeEventType.None.Id, 12},
            new object[] {NotifyCollectionChangeEventType.Simple.Id, NotifyCollectionChangeEventType.None.Id, 12},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.None.Id, 12},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.None.Id, 12},
            new object[] {NotifyCollectionChangeEventType.Single.Id, NotifyCollectionChangeEventType.Once.Id, 12},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.Multi.Id, 6},
            new object[] {NotifyCollectionChangeEventType.Multi.Id, NotifyCollectionChangeEventType.Multi.Id, 12},
        };

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgs_AdjustLengthTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_AdjustLength(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId,
            int length)
        {
            if (length > TestListInitLength)
            {
                NotifyCollectionChangeEventArgsTest_AdjustLengthIfShort(collectionChangingEventTypeId,
                    collectionChangedEventTypeId, length);
            }
            else if (length < TestListInitLength)
            {
                NotifyCollectionChangeEventArgsTest_AdjustLengthIfLong(collectionChangingEventTypeId,
                    collectionChangedEventTypeId, length);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgs_AdjustLengthTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_AdjustLengthIfShort(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId,
            int length)
        {
            if (length < TestListInitLength)
            {
                Assert.Pass();
                return;
            }

            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            var addItemLength = length - TestListInitLength;
            var addItems = MakeTestListMakeItemsItem(TestListInitLength, addItemLength).ToArray();

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            instance.AdjustLengthIfShort(length);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Add が一度通知されていること
                        dic.CheckAddEventArgs(addItemLength, false, TestListInitLength, addItems);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Add が複数回通知されていること
                        dic.CheckAddEventArgs(addItemLength, true, TestListInitLength, addItems);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        [TestCaseSource(nameof(NotifyCollectionChangeEventArgs_AdjustLengthTestCaseSource))]
        public static void NotifyCollectionChangeEventArgsTest_AdjustLengthIfLong(
            string collectionChangingEventTypeId, string collectionChangedEventTypeId,
            int length)
        {
            if (length > TestListInitLength)
            {
                Assert.Pass();
                return;
            }

            var collectionChangingEventType = NotifyCollectionChangeEventType.FromId(collectionChangingEventTypeId);
            var collectionChangedEventType = NotifyCollectionChangeEventType.FromId(collectionChangedEventTypeId);

            var removeItemLength = TestListInitLength - length;
            var removeStartIndex = length;

            var instance = MakeTestList(TestListInitLength, collectionChangingEventType, collectionChangedEventType,
                out var collectionChangingEventArgsList, out var collectionChangedEventArgsList);

            var removeItems = instance.TakeLast(removeItemLength).ToArray();

            instance.AdjustLengthIfLong(length);

            var checkAction = new Action<NotifyCollectionChangeEventType, NotifyCollectionChangedEventArgsDic>(
                (eventType, dic) =>
                {
                    if (eventType == NotifyCollectionChangeEventType.None)
                    {
                        // 通知が行われていないこと
                        Assert.IsTrue(dic.IsEmpty);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Once
                             || eventType == NotifyCollectionChangeEventType.Single)
                    {
                        // Remove が一度通知されていること
                        dic.CheckRemoveEventArgs(removeItemLength, false, removeStartIndex, removeItems);
                    }
                    else if (eventType == NotifyCollectionChangeEventType.Simple
                             || eventType == NotifyCollectionChangeEventType.Multi)
                    {
                        // Remove が複数回通知されていること
                        dic.CheckRemoveEventArgs(removeItemLength, true, removeStartIndex, removeItems);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                });

            checkAction(collectionChangingEventType, collectionChangingEventArgsList);
            checkAction(collectionChangedEventType, collectionChangedEventArgsList);
        }

        #endregion

        private static TestList MakeTestList(int initLength,
            NotifyCollectionChangeEventType notifyCollectionChangingEventType,
            NotifyCollectionChangeEventType notifyCollectionChangedEventType,
            out NotifyCollectionChangedEventArgsDic collectionChangingEventArgsList,
            out NotifyCollectionChangedEventArgsDic collectionChangedEventArgsList)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new TestList()
                : new TestList(initStringList);

            // Observerに購読させないよう、イベントObserver登録より前に通知フラグ設定
            result.NotifyCollectionChangingEventType = notifyCollectionChangingEventType;
            result.NotifyCollectionChangedEventType = notifyCollectionChangedEventType;

            result.FuncMakeItems = MakeTestListMakeItemsItem;

            collectionChangingEventArgsList = new NotifyCollectionChangedEventArgsDic();
            result.CollectionChanging += MakeCollectionChangeEventHandler(true, collectionChangingEventArgsList);

            collectionChangedEventArgsList = new NotifyCollectionChangedEventArgsDic();
            result.CollectionChanged += MakeCollectionChangeEventHandler(false, collectionChangedEventArgsList);

            return result;
        }

        #region MakeTestItems

        private static List<string> MakeStringList(int length, Func<int, string> funcMakeItem = null)
        {
            if (length < 0) return null;
            var result = new List<string>();
            for (var i = 0; i < length; i++)
            {
                result.Add((funcMakeItem ?? MakeDefaultItem)(i));
            }

            return result;
        }

        private static string MakeDefaultItem(int index)
            => index.ToString();

        /// <summary>
        /// <see cref="TestList"/> の <see cref="IExtendedList{T}.AdjustLengthIfShort"/> で不足要素を補う。
        /// </summary>
        private static IEnumerable<string> MakeTestListMakeItemsItem(int index, int count)
            => Enumerable.Range(index, count).Select(i => $"Adjusted {i} item.");

        /// <summary>
        /// CollectionChanging, CollectionChanged に登録するイベントハンドラを生成する
        /// </summary>
        /// <param name="isBefore">CollectionChanging にセットする場合true, CollectionChanged にセットする場合false</param>
        /// <param name="resultDic">発生したイベント引数を格納するDirectory</param>
        /// <returns>生成したインスタンス</returns>
        private static NotifyCollectionChangedEventHandler MakeCollectionChangeEventHandler(bool isBefore,
            NotifyCollectionChangedEventArgsDic resultDic)
            => (_, args) =>
            {
                var argsEx = (NotifyCollectionChangedEventArgsEx<string>) args;
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

        #endregion

        private class TestList : ExtendedList<string>
        {
            public TestList()
            {
            }

            public TestList(IEnumerable<string> values) : base(values)
            {
            }
        }

        private static readonly int TestListInitLength = 10;

        private enum OverwriteType
        {
            Replace,
            Add,
            Multi,
        }

        private class NotifyCollectionChangedEventArgsDic
        {
            private Dictionary<string, List<NotifyCollectionChangedEventArgsEx<string>>> Impl { get; }

            public NotifyCollectionChangedEventArgsDic()
            {
                Impl = new Dictionary<string, List<NotifyCollectionChangedEventArgsEx<string>>>();
                Clear();
            }

            public void Add(params NotifyCollectionChangedEventArgsEx<string>[] args)
            {
                args.ForEach(arg => { Impl[arg.Action.ToString()].Add(arg); });
            }

            #region 判定用

            /// <summary>
            /// いずれのイベント引数も格納されていない場合 true
            /// </summary>
            public bool IsEmpty => Impl.All(x => x.Value.Count == 0);

            public bool IsMoveEventEmpty => MoveEventArgs.Count == 0;
            public bool IsRemoveEventEmpty => RemoveEventArgs.Count == 0;
            public bool IsResetEventEmpty => ResetEventArgs.Count == 0;

            public void CheckReplaceEventArgs(int argsLength, bool isMultipart,
                int setIndex, string[] oldItems, string[] setItems)
            {
                CheckOnlyReplaceEventArgs(argsLength, isMultipart, setIndex, oldItems, setItems);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyReplaceEventArgs(int argsLength, bool isMultipart,
                int setIndex, string[] oldItems, string[] setItems)
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
                        Assert.IsTrue(arg.OldItems[0].Equals(oldItems[i]));
                        Assert.IsTrue(arg.NewStartingIndex == setIndex + i);
                        Assert.IsTrue(arg.NewItems?.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Equals(setItems[i]));
                    }
                }
                else
                {
                    // Replace が一度通知されていること
                    Assert.IsTrue(ReplaceEventArgs.Count == 1);
                    {
                        var arg = ReplaceEventArgs[0];
                        Assert.IsTrue(arg.OldStartingIndex == setIndex);
                        Assert.IsTrue(arg.OldItems?.Count == argsLength);
                        Assert.IsTrue(arg.OldItems.SequenceEqual(oldItems));
                        Assert.IsTrue(arg.NewStartingIndex == setIndex);
                        Assert.IsTrue(arg.NewItems?.Count == argsLength);
                        Assert.IsTrue(arg.NewItems.SequenceEqual(setItems));
                    }
                }
            }

            public void CheckAddEventArgs(int argsLength, bool isMultipart,
                int insertIndex, string[] insertItems)
            {
                CheckOnlyAddEventArgs(argsLength, isMultipart, insertIndex, insertItems);
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
                Assert.IsTrue(ResetEventArgs.Count == 0);
            }

            public void CheckOnlyAddEventArgs(int argsLength, bool isMultipart,
                int insertIndex, string[] insertItems)
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
                        Assert.IsTrue(arg.NewItems[0].Equals(insertItems[i]));
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
                        Assert.IsTrue(arg.NewItems.SequenceEqual(insertItems));
                    }
                }
            }

            public void CheckMoveEventArgs(int argsLenght, bool isMultipart,
                int oldIndex, int newIndex, string[] moveItems)
            {
                if (isMultipart)
                {
                    Assert.IsTrue(MoveEventArgs.Count == argsLenght);
                    for (var i = 0; i < argsLenght; i++)
                    {
                        var arg = MoveEventArgs[i];
                        Assert.IsTrue(arg.OldStartingIndex == oldIndex + i);
                        Assert.IsTrue(arg.OldItems?.Count == 1);
                        Assert.IsTrue(arg.OldItems[0].Equals(moveItems[i]));
                        Assert.IsTrue(arg.NewStartingIndex == newIndex + i);
                        Assert.IsTrue(arg.NewItems?.Count == 1);
                        Assert.IsTrue(arg.NewItems[0].Equals(moveItems[i]));
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
                        Assert.IsTrue(arg.OldItems.SequenceEqual(moveItems));
                        Assert.IsTrue(arg.NewStartingIndex == newIndex);
                        Assert.IsTrue(arg.NewItems?.Count == argsLenght);
                        Assert.IsTrue(arg.NewItems.SequenceEqual(moveItems));
                    }
                    Assert.IsTrue(ReplaceEventArgs.Count == 0);
                    Assert.IsTrue(AddEventArgs.Count == 0);
                    Assert.IsTrue(RemoveEventArgs.Count == 0);
                    Assert.IsTrue(ResetEventArgs.Count == 0);
                }
            }

            public void CheckRemoveEventArgs(int argsLength, bool isMultipart,
                int removeIndex, string[] removeItems)
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
                        Assert.IsTrue(arg.OldItems[0].Equals(removeItems[i]));
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
                        Assert.IsTrue(arg.OldItems.SequenceEqual(removeItems));
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
                string[] oldItems, string[] newItems)
            {
                // Remove が一度通知されていること
                Assert.IsTrue(ResetEventArgs.Count == 1);
                {
                    var arg = ResetEventArgs[0];
                    Assert.IsTrue(arg.OldStartingIndex == startIndex);
                    Assert.IsTrue(arg.OldItems?.Count == oldItems.Length);
                    Assert.IsTrue(arg.OldItems.SequenceEqual(oldItems));
                    Assert.IsTrue(arg.NewStartingIndex == startIndex);
                    Assert.IsTrue(arg.NewItems?.Count == newItems.Length);
                    Assert.IsTrue(arg.NewItems.SequenceEqual(newItems));
                }
                Assert.IsTrue(ReplaceEventArgs.Count == 0);
                Assert.IsTrue(AddEventArgs.Count == 0);
                Assert.IsTrue(MoveEventArgs.Count == 0);
                Assert.IsTrue(RemoveEventArgs.Count == 0);
            }

            #endregion

            private List<NotifyCollectionChangedEventArgsEx<string>> ReplaceEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Replace)];

            private List<NotifyCollectionChangedEventArgsEx<string>> AddEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Add)];

            private List<NotifyCollectionChangedEventArgsEx<string>> RemoveEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Remove)];

            private List<NotifyCollectionChangedEventArgsEx<string>> ResetEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Reset)];

            private List<NotifyCollectionChangedEventArgsEx<string>> MoveEventArgs =>
                Impl[nameof(NotifyCollectionChangedAction.Move)];

            private void Clear()
            {
                Impl.Clear();
                Impl.Add(nameof(NotifyCollectionChangedAction.Replace),
                    new List<NotifyCollectionChangedEventArgsEx<string>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Add),
                    new List<NotifyCollectionChangedEventArgsEx<string>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Remove),
                    new List<NotifyCollectionChangedEventArgsEx<string>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Reset),
                    new List<NotifyCollectionChangedEventArgsEx<string>>());
                Impl.Add(nameof(NotifyCollectionChangedAction.Move),
                    new List<NotifyCollectionChangedEventArgsEx<string>>());
            }
        }
    }
}
