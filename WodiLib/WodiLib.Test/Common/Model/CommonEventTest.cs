using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Common;
using WodiLib.Event;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Common
{
    [TestFixture]
    public class CommonEventTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        private static readonly object[] EventBootConditionTestCaseSource =
        {
            new object[] {new CommonEventBootCondition(), false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(EventBootConditionTestCaseSource))]
        public static void EventBootConditionTest(CommonEventBootCondition bootCondition, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.BootCondition = bootCondition;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var condition = instance.BootCondition;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(condition == bootCondition);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (isError)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.BootCondition)));
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void NumberArgsLengthTest(int length, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.NumberArgsLength = length;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var len = instance.NumberArgsLength;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(len == length);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.NumberArgsLength)));
            }
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(4, false)]
        [TestCase(5, true)]
        public static void StrArgsLengthTest(int length, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.StrArgsLength = length;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var len = instance.StrArgsLength;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(len == length);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.StrArgsLength)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void NameTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var name = isNull ? null : (CommonEventName) "test";

            var errorOccured = false;
            try
            {
                instance.Name = name;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var n = instance.Name;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(n == name);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.Name)));
            }
        }

        private static readonly object[] EventCommandsTestCaseSource =
        {
            new object[] {new EventCommandList(new List<IEventCommand> {new Blank()}), false},
            // new object[] {new EventCommandList(new List<IEventCommand>()), true}, イベント0行のEventCommandListは作成不可能
            new object[] {null, true},
        };

        [TestCaseSource(nameof(EventCommandsTestCaseSource))]
        public static void EventCommandsTest(EventCommandList list, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.EventCommands = list;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.EventCommands;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(ReferenceEquals(setValue, list));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.EventCommands)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void DescriptionTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var str = isNull ? null : (CommonEventDescription) "test";

            var errorOccured = false;
            try
            {
                instance.Description = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.Description;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue?.Equals(str));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.Description)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void MemoTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var str = isNull ? null : (CommonEventMemo) "test";

            var errorOccured = false;
            try
            {
                instance.Memo = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.Memo;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue?.Equals(str));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.Memo)));
            }
        }

        private static readonly object[] LabelColorTestCaseSource =
        {
            new object[] {CommonEventLabelColor.Black, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(LabelColorTestCaseSource))]
        public static void LabelColorTest(CommonEventLabelColor color, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.LabelColor = color;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.LabelColor;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue == color);
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.LabelColor)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void FooterStringTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var str = isNull ? null : (CommonEventFooterString) "test";

            var errorOccured = false;
            try
            {
                instance.FooterString = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.FooterString;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(ReferenceEquals(setValue, str));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.FooterString)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void ReturnValueDescriptionTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var str = isNull ? null : (CommonEventResultDescription) "test";

            var errorOccured = false;
            try
            {
                instance.ReturnValueDescription = str;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (!errorOccured)
            {
                var setValue = instance.ReturnValueDescription;

                // セットした値と取得した値が一致すること
                Assert.IsTrue(setValue?.Equals(str));
            }

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.ReturnValueDescription)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void SelfVariableNameListTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var nameList = isNull ? null : new CommonEventSelfVariableNameList();

            var errorOccured = false;
            try
            {
                instance.SelfVariableNameList = nameList;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedPropertyList.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedPropertyList.Count, 1);
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.SelfVariableNameList)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void UpdateSpecialNumberArgDescTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedSpecialNumberArgDescPropertyList = new List<string>();
            instance.NumberArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialNumberArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.NumberArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyCollection.Add(args);
            };
            var changedSpecialStringArgDescPropertyList = new List<string>();
            instance.StringArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialStringArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.StringArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyCollection.Add(args);
            };

            var index = (CommonEventNumberArgIndex) 1;
            var desc = isNull ? null : new CommonEventSpecialNumberArgDesc();

            var errorOccured = false;
            try
            {
                instance.UpdateSpecialNumberArgDesc(index, desc);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedSpecialNumberArgDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialNumberArgDescPropertyCollection.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedSpecialNumberArgDescPropertyList.Count, 1);
                Assert.IsTrue(changedSpecialNumberArgDescPropertyList[0]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialNumberArgDescPropertyCollection.Count, 1);
                Assert.AreEqual(changedSpecialNumberArgDescPropertyCollection[0].Action,
                    NotifyCollectionChangedAction.Replace);
            }

            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialStringArgDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialStringArgDescPropertyCollection.Count, 0);
        }

        [Test]
        public static void GetSpecialNumberArgDescTest()
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedSpecialNumberArgDescPropertyList = new List<string>();
            instance.NumberArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialNumberArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.NumberArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyCollection.Add(args);
            };
            var changedSpecialStringArgDescPropertyList = new List<string>();
            instance.StringArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialStringArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.StringArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyCollection.Add(args);
            };

            var index = (CommonEventNumberArgIndex) 0;

            var errorOccured = false;
            try
            {
                var _ = instance.GetSpecialNumberArgDesc(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialNumberArgDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialNumberArgDescPropertyCollection.Count, 0);
            Assert.AreEqual(changedSpecialStringArgDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialStringArgDescPropertyCollection.Count, 0);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void UpdateSpecialStringArgDescTest(bool isNull, bool isError)
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedSpecialNumberArgDescPropertyList = new List<string>();
            instance.NumberArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialNumberArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.NumberArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyCollection.Add(args);
            };
            var changedSpecialStringArgDescPropertyList = new List<string>();
            instance.StringArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialStringArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.StringArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyCollection.Add(args);
            };

            var index = (CommonEventStringArgIndex) 2;
            var desc = isNull ? null : new CommonEventSpecialStringArgDesc();

            var errorOccured = false;
            try
            {
                instance.UpdateSpecialStringArgDesc(index, desc);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 意図したとおりプロパティ変更通知が発火していること
            if (errorOccured)
            {
                Assert.AreEqual(changedSpecialStringArgDescPropertyList.Count, 0);
                Assert.AreEqual(changedSpecialStringArgDescPropertyCollection.Count, 0);
            }
            else
            {
                Assert.AreEqual(changedSpecialStringArgDescPropertyList.Count, 1);
                Assert.IsTrue(changedSpecialStringArgDescPropertyList[0]
                    .Equals(ListConstant.IndexerName));
                Assert.AreEqual(changedSpecialStringArgDescPropertyCollection.Count, 1);
                Assert.AreEqual(changedSpecialStringArgDescPropertyCollection[0].Action,
                    NotifyCollectionChangedAction.Replace);
            }

            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialNumberArgDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialNumberArgDescPropertyCollection.Count, 0);
        }

        [Test]
        public static void GetSpecialStringArgDescTest()
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };
            var changedSpecialNumberArgDescPropertyList = new List<string>();
            instance.NumberArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialNumberArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.NumberArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialNumberArgDescPropertyCollection.Add(args);
            };
            var changedSpecialStringArgDescPropertyList = new List<string>();
            instance.StringArgDescList.PropertyChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyList.Add(args.PropertyName);
            };
            var changedSpecialStringArgDescPropertyCollection = new List<NotifyCollectionChangedEventArgs>();
            instance.StringArgDescList.CollectionChanged += (sender, args) =>
            {
                changedSpecialStringArgDescPropertyCollection.Add(args);
            };

            var index = (CommonEventStringArgIndex) 1;

            var errorOccured = false;
            try
            {
                var _ = instance.GetSpecialStringArgDesc(index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialNumberArgDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialNumberArgDescPropertyCollection.Count, 0);
            Assert.AreEqual(changedSpecialStringArgDescPropertyList.Count, 0);
            Assert.AreEqual(changedSpecialStringArgDescPropertyCollection.Count, 0);
        }

        [Test]
        public static void SetReturnVariableIndexTest()
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var commonVarAddress = (CommonEventReturnVariableIndex) 10;

            var errorOccured = false;
            try
            {
                instance.SetReturnVariableIndex(commonVarAddress);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 2);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.ReturnVariableIndex)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(CommonEvent.IsReturnValue)));
        }

        [Test]
        public static void SetReturnValueNoneTest()
        {
            var instance = new CommonEvent();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.SetReturnValueNone();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.AreEqual(errorOccured, false);

            // 意図したとおりプロパティ変更通知が発火していること
            Assert.AreEqual(changedPropertyList.Count, 2);
            Assert.IsTrue(changedPropertyList[0].Equals(nameof(CommonEvent.ReturnVariableIndex)));
            Assert.IsTrue(changedPropertyList[1].Equals(nameof(CommonEvent.IsReturnValue)));
        }

        [Test]
        public static void EventCommandsOwnerTest()
        {
            var assignValue = new AssignValue();
            var assignValue2 = new AssignValue();
            var addValue = new AddValue();
            var addValue2 = new AddValue();

            // この時点で EventCommand の Owner が null であることを確認
            Assert.IsNull(assignValue.Owner);
            Assert.IsNull(assignValue2.Owner);
            Assert.IsNull(addValue.Owner);
            Assert.IsNull(addValue2.Owner);

            var commonEvent = new CommonEvent();

            var list = new EventCommandList();

            var moveRoute = new MoveRoute();

            var actionEntry = new ActionEntry();

            actionEntry.CommandList.Add(assignValue);
            actionEntry.CommandList.Add(addValue);

            // この時点で EventCommandList, MoveRoute, ActionEntry, EventCommand の Owner が null であることを確認
            Assert.IsNull(list.Owner);
            Assert.IsNull(moveRoute.Owner);
            Assert.IsNull(actionEntry.Owner);
            Assert.IsNull(assignValue.Owner);
            Assert.IsNull(assignValue2.Owner);
            Assert.IsNull(addValue.Owner);
            Assert.IsNull(addValue2.Owner);

            moveRoute.ActionEntry = actionEntry;
            list.Add(moveRoute);

            // この時点で EventCommand の Owner が null であることを確認
            Assert.IsNull(assignValue.Owner);
            Assert.IsNull(assignValue2.Owner);
            Assert.IsNull(addValue.Owner);
            Assert.IsNull(addValue2.Owner);

            commonEvent.EventCommands = list;

            // この時点で EventCommandList, MoveRoute, ActionEntry, セット済みのEventCommand の
            // Owner がセットされていることを確認
            Assert.AreEqual(list.Owner, TargetAddressOwner.CommonEvent);
            Assert.AreEqual(moveRoute.Owner, TargetAddressOwner.CommonEvent);
            Assert.AreEqual(assignValue.Owner, TargetAddressOwner.CommonEvent);
            Assert.AreEqual(addValue.Owner, TargetAddressOwner.CommonEvent);

            actionEntry.CommandList.Add(assignValue2);
            moveRoute.ActionEntry.CommandList.Add(addValue2);

            // EventCommand の Owner に適切な値が設定されること
            Assert.AreEqual(assignValue2.Owner, TargetAddressOwner.CommonEvent);
            Assert.AreEqual(addValue2.Owner, TargetAddressOwner.CommonEvent);

            // commonEvent をここまで開放したくないので無駄な処理を入れる
            commonEvent.Memo = "";
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new CommonEvent
            {
                Id = 20,
            };
            var changedPropertyList = new List<string>();
            target.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));

            // プロパティ変更通知が発火していないこと
            Assert.AreEqual(changedPropertyList.Count, 0);
        }
    }
}