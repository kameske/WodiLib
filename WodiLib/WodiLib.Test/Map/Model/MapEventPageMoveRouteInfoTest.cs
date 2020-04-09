using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Event.CharaMoveCommand;
using WodiLib.Map;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class MapEventPageMoveRouteInfoTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void AnimateSpeedSetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageMoveRouteInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.AnimateSpeed = isNull ? null : AnimateSpeed.Middle;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageMoveRouteInfo.AnimateSpeed)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void MoveSpeedSetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageMoveRouteInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MoveSpeed = isNull ? null : MoveSpeed.Slow;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageMoveRouteInfo.MoveSpeed)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void MoveFrequencySetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageMoveRouteInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MoveFrequency = isNull ? null : MoveFrequency.Long;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageMoveRouteInfo.MoveFrequency)));
            }
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public static void MoveTypeSetTest(bool isNull, bool isError)
        {
            var instance = new MapEventPageMoveRouteInfo();
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.MoveType = isNull ? null : MoveType.Not;
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageMoveRouteInfo.MoveType)));
            }
        }

        [TestCase(false, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, false, false)]
        [TestCase(true, true, true)]
        public static void CustomMoveRouteSetTest(bool isRouteCustom, bool isSetNull, bool isError)
        {
            var instance = new MapEventPageMoveRouteInfo();
            instance.MoveType = isRouteCustom ? MoveType.Custom : MoveType.Not;
            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (sender, args) => { changedPropertyList.Add(args.PropertyName); };

            var errorOccured = false;
            try
            {
                instance.CustomMoveRoute = isSetNull ? null : new ActionEntry();
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
                Assert.IsTrue(changedPropertyList[0].Equals(nameof(MapEventPageMoveRouteInfo.CustomMoveRoute)));
            }
        }

        [Test]
        public static void CharaMoveCommandOwnerTest()
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

            var actionEntry = new ActionEntry();

            actionEntry.CommandList.Add(assignValue);
            actionEntry.CommandList.Add(addValue);

            // この時点で ActionEntry, EventCommand の Owner が null であることを確認
            Assert.IsNull(actionEntry.Owner);
            Assert.IsNull(assignValue.Owner);
            Assert.IsNull(assignValue2.Owner);
            Assert.IsNull(addValue.Owner);
            Assert.IsNull(addValue2.Owner);

            var instance = new MapEventPageMoveRouteInfo
            {
                CustomMoveRoute = actionEntry
            };

            // この時点で ActionEntry, セット済みのEventCommand の Owner がセットされていることを確認
            Assert.AreEqual(actionEntry.Owner, TargetAddressOwner.MapEvent);
            Assert.AreEqual(assignValue.Owner, TargetAddressOwner.MapEvent);
            Assert.AreEqual(addValue.Owner, TargetAddressOwner.MapEvent);
            Assert.IsNull(assignValue2.Owner);
            Assert.IsNull(addValue2.Owner);

            actionEntry.CommandList.Add(assignValue2);
            actionEntry.CommandList.Add(addValue2);

            // EventCommand の Owner に適切な値が設定されること
            Assert.AreEqual(assignValue2.Owner, TargetAddressOwner.MapEvent);
            Assert.AreEqual(addValue2.Owner, TargetAddressOwner.MapEvent);

            // instance をここまで開放したくないので無駄な処理を入れる
            instance.MoveSpeed = MoveSpeed.Fast;
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new MapEventPageMoveRouteInfo
            {
                MoveSpeed = MoveSpeed.Slowest,
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