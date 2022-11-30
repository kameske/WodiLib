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
    public class RestrictedCapacityListTest
    {
        private static Logger logger = default!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region Events

        [Test]
        public static void AddCollectionChangedEventTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.CollectionChanged += (_, _) => { };

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(instance.CollectionChanged)
            );
        }

        [Test]
        public static void RemoveCollectionChangedEventTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.CollectionChanged -= default!;

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(instance.CollectionChanged)
            );
        }

        [Test]
        public static void PropagateNotifyPropertyChange_Item()
        {
            // 要素を変更したとき、 "Items[]" の変更通知が行われること

            var instance = InitInstance.GenerateForPropagateTest(5);
            var notifiedPropertyNames = new List<string>();

            instance.PropertyChanged += (_, args) => notifiedPropertyNames.Add(args.PropertyName);

            instance.SetCore(0, new StubModel("new Items"));

            Assert.IsTrue(notifiedPropertyNames.Count == 1);
            Assert.IsTrue(notifiedPropertyNames[0].Equals(ListConstant.IndexerName));
        }

        [Test]
        public static void PropagateNotifyPropertyChange_ItemProperty()
        {
            // 要素のPropertyChangedイベントが発火したとき、 "Items[]" の変更通知が行われること

            var instance = InitInstance.GenerateForPropagateTest(5);
            var notifiedPropertyNames = new List<string>();

            instance.PropertyChanged += (_, args) => notifiedPropertyNames.Add(args.PropertyName);

            instance[0].StringValue = "updated";

            Assert.IsTrue(notifiedPropertyNames.Count == 1);
            Assert.IsTrue(notifiedPropertyNames[0].Equals(ListConstant.IndexerName));
        }

        #endregion

        #region Implementations

        #region Constructor

        [Test]
        public static void ConstructorTest_NoArgs()
        {
            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestRestrictedCapacityList(),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期要素数が GetMinCapacity の結果と一致すること
            Assert.AreEqual(instance.Count, instance.GetMinCapacity());
        }

        [Test]
        public static void ConstructorTest_SpecifyLength()
        {
            const int length = 5;
            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestRestrictedCapacityList(length),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期要素数が length と一致すること
            Assert.AreEqual(length, instance.Count);
        }

        [Test]
        public static void ConstructorTest_SpecifyInitItems()
        {
            var initItems = 7.Iterate(i => new StubModel(i.ToString())).ToList();
            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestRestrictedCapacityList(initItems),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期要素が initItems と一致すること
            Assert.AreEqual(instance.Count, 7);
            instance.ForEach(
                (item, i) => { Assert.IsTrue(item.ItemEquals(initItems[i])); }
            );
        }

        [Test]
        public static void ConstructorTest_InjectPropertyItems()
        {
            var list = new MockExtendedList<StubModel>();

            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestRestrictedCapacityList(list),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // Items プロパティの実態がコンストラクタで与えた list であること
            Assert.IsTrue(instance.IsReferenceEqualsWithItems(list));
        }

        #endregion

        #region Indexer

        [Test]
        public static void IndexerGetTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            var _ = instance[0];

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                ListConstant.IndexerName
            );
        }

        [Test]
        public static void IndexerSetTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance[0] = new StubModel("new value");

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                ListConstant.IndexerName
            );
        }

        #endregion

        #region Property

        [Test]
        public static void CountTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            var _ = instance.Count;

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count)
            );
        }

        #endregion

        #region Method

        [Test]
        public static void GetRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            var _ = instance.GetRange(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Get)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.GetRangeCore)
            );
        }

        [Test]
        public static void SetRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.SetRange(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Set)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.SetRangeCore)
            );
        }

        [Test]
        public static void AddTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Add(new StubModel("new Value"));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void AddRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AddRange(new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Insert(0, new StubModel("new Value"));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.InsertRange(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void OverwriteTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Overwrite(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Overwrite)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.OverwriteCore)
            );
        }

        [Test]
        public static void MoveTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Move(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Move)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.MoveRangeCore)
            );
        }

        [Test]
        public static void MoveRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.MoveRange(0, 1, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Move)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.MoveRangeCore)
            );
        }

        [Test]
        public static void RemoveTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Remove(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Remove)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.RemoveRangeCore)
            );
        }

        [Test]
        public static void RemoveRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.RemoveRange(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Remove)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.RemoveRangeCore)
            );
        }

        [Test]
        public static void AdjustLengthTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AdjustLength(10);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.AdjustLength)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustLengthIfShortTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AdjustLengthIfShort(10);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.AdjustLength)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustLengthIfLongTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AdjustLengthIfLong(10);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.AdjustLength)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.AdjustLengthCore)
            );
        }

        [Test]
        public static void ResetTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Reset(new[] { new StubModel("reset value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Reset)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.ResetCore)
            );
        }

        [Test]
        public static void ClearTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Clear();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Clear)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.ClearCore)
            );
        }

        [Test]
        public static void ValidateGetTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateGet(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Get)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateGetRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateGetRange(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Get)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateSetTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateSet(0, new StubModel());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Set)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateSetRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateSetRange(0, new[] { new StubModel() });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Set)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateAddTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateAdd(new StubModel("new value"));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateAddRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateAddRange(new[] { new StubModel() });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateInsertTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateInsert(0, new StubModel("new value"));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateInsertRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateInsertRange(0, new[] { new StubModel() });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Insert)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateOverwriteTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateOverwrite(0, new[] { new StubModel() });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Overwrite)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateMoveTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateMove(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Move)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateMoveRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateMoveRange(0, 1, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Move)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateRemoveTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateRemove(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Remove)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateRemoveRangeTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateRemoveRange(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Remove)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateAdjustLengthTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateAdjustLength(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.AdjustLength)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateAdjustLengthIfShortTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateAdjustLengthIfShort(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.AdjustLength)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateAdjustLengthIfLongTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateAdjustLengthIfLong(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.AdjustLength)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateResetTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateReset(Array.Empty<StubModel>());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Reset)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void ValidateClearTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ValidateClear();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Clear)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Validator)
            );
        }

        [Test]
        public static void GetCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.GetCore(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.GetRangeCore)
            );
        }

        [Test]
        public static void GetRangeCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.GetRangeCore(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.GetRangeCore)
            );
        }

        [Test]
        public static void SetCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.SetCore(0, new StubModel());

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.SetRangeCore)
            );
        }


        [Test]
        public static void SetRangeCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.SetRangeCore(0, new[] { new StubModel() });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.SetRangeCore)
            );
        }

        [Test]
        public static void AddCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AddCore(new StubModel("new Value"));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void AddRangeCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AddRangeCore(new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.InsertCore(0, new StubModel("new Value"));

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void InsertRangeCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.InsertRangeCore(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.InsertRangeCore)
            );
        }

        [Test]
        public static void OverwriteCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.OverwriteCore(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.OverwriteCore)
            );
        }

        [Test]
        public static void MoveCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.MoveCore(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.MoveRangeCore)
            );
        }

        [Test]
        public static void MoveRangeCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.MoveRangeCore(0, 1, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.MoveRangeCore)
            );
        }

        [Test]
        public static void RemoveCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.RemoveCore(0);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.RemoveRangeCore)
            );
        }

        [Test]
        public static void RemoveRangeCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.RemoveRangeCore(0, 1);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.RemoveRangeCore)
            );
        }

        [Test]
        public static void AdjustLengthCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AdjustLengthCore(10);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustLengthIfShortCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AdjustLengthIfShortCore(10);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.AdjustLengthCore)
            );
        }

        [Test]
        public static void AdjustLengthIfLongCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.AdjustLengthIfLongCore(10);

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.AdjustLengthCore)
            );
        }

        [Test]
        public static void ResetCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ResetCore(new[] { new StubModel() });

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.ResetCore)
            );
        }

        [Test]
        public static void ClearCoreTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.ClearCore();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.ClearCore)
            );
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = InitInstance.GenerateForMembersTest();
            using var _ = instance.GetEnumerator();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.GetEnumerator)
            );
        }

        #endregion

        #endregion

        private static class InitInstance
        {
            #region For Transport Test

            public static TransportMembersTestRestrictedCapacityList GenerateForMembersTest() => new();

            /// <summary>
            /// 内部で使用する <see cref="IExtendedList{T}"/> をモックに差し替えただけ。
            /// </summary>
            public class TransportMembersTestRestrictedCapacityList :
                RestrictedCapacityList<StubModel, TransportMembersTestRestrictedCapacityList>
            {
                public MockExtendedList<StubModel> ExtendedListMock => (MockExtendedList<StubModel>)Items;

                private protected override IExtendedList<StubModel> Items { get; } = new MockExtendedList<StubModel>();

                public TransportMembersTestRestrictedCapacityList()
                {
                    Items = new MockExtendedList<StubModel>
                    {
                        Impl = new ExtendedList<StubModel>(
                            i => new StubModel(i.ToString()),
                            null,
                            5.Iterate(i => new StubModel(i.ToString()))
                        ),
                    };
                }

                // @formatter:off
                #region NotUse
                public override bool ItemEquals(TransportMembersTestRestrictedCapacityList? other) => false;
                protected override StubModel MakeDefaultItem(int index) => new (index.ToString());
                protected override IWodiLibListValidator<StubModel> GenerateValidatorForItems() => new MockWodiLibListValidator<StubModel>();
                public override int GetMaxCapacity() => int.MaxValue;
                public override int GetMinCapacity() => 0;
                public override IFixedLengthList<StubModel> AsFixedLengthList() => throw new NotImplementedException();
                public override IReadOnlyExtendedList<StubModel> AsReadOnlyList() => throw new NotImplementedException();
                public override TransportMembersTestRestrictedCapacityList DeepClone() => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }

            #endregion

            #region For Propagate Test

            public static PropagateTestRestrictedCapacityList GenerateForPropagateTest(int count = 0) => new(count);

            /// <summary>
            /// 内部で使用する <see cref="IExtendedList{T}"/> を差し替えないバージョン。
            /// <see cref="IExtendedList{T}"/> の通知が <see cref="RestrictedCapacityList{T,TImpl}"/> に伝播することを確認するためのクラス。
            /// またコンストラクタのテストにも使用する。
            /// </summary>
            public class PropagateTestRestrictedCapacityList :
                RestrictedCapacityList<StubModel, PropagateTestRestrictedCapacityList>
            {
                public PropagateTestRestrictedCapacityList()
                {
                }

                public PropagateTestRestrictedCapacityList(IExtendedList<StubModel> itemsImpl) : base(itemsImpl)
                {
                }

                public PropagateTestRestrictedCapacityList(int length) : base(length)
                {
                }

                public PropagateTestRestrictedCapacityList(IEnumerable<StubModel> initItems) : base(initItems)
                {
                }

                public bool IsReferenceEqualsWithItems(object? other)
                {
                    return ReferenceEquals(Items, other);
                }

                // @formatter:off
                #region NotUse
                public override bool ItemEquals(PropagateTestRestrictedCapacityList? other) => false;
                protected override StubModel MakeDefaultItem(int index) => new (index.ToString());
                protected override IWodiLibListValidator<StubModel> GenerateValidatorForItems() => new MockWodiLibListValidator<StubModel>();
                public override int GetMaxCapacity() => int.MaxValue;
                public override int GetMinCapacity() => 1;
                public override IFixedLengthList<StubModel> AsFixedLengthList() => throw new NotImplementedException();
                public override IReadOnlyExtendedList<StubModel> AsReadOnlyList() => throw new NotImplementedException();
                public override PropagateTestRestrictedCapacityList DeepClone() => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }

            #endregion
        }
    }
}
