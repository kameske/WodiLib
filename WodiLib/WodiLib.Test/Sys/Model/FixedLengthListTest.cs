using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class FixedLengthListTest
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
                nameof(IExtendedList<StubModel>.CollectionChanged)
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
                nameof(IExtendedList<StubModel>.CollectionChanged)
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
        public static void ConstructorTest_InjectPropertyItems()
        {
            var list = new MockExtendedList<StubModel>();

            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestFixedLengthList(list),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // Items プロパティの実態がコンストラクタで与えた list であること
            Assert.IsTrue(instance.IsReferenceEqualsWithItems(list));
        }

        [Test]
        public static void ConstructorTest_SpecifyCount()
        {
            var count = 13;
            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestFixedLengthList(count),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 要素数が引数で与えた count と一致すること
            Assert.IsTrue(instance.Count == count);
            // 各要素が初期要素生成メソッドが返却した値で初期化されていること
            count.Range()
                .ForEach(
                    i => { Assert.IsTrue(instance[i].ItemEquals(instance.DefaultItemFactory(i))); }
                );
        }

        [TestCase(9, 10, true)]
        [TestCase(10, 10, false)]
        [TestCase(11, 10, true)]
        public static void ConstructorTest_InitItemsAndCapacity(int initItemLength, int capacity, bool isError)
        {
            TestTemplate.Constructor(
                () => new InitInstance.PropagateTestFixedLengthList(
                    initItemLength.Iterate(i => new StubModel(i.ToString())),
                    capacity
                ),
                expectedThrowCreateNewInstance: isError,
                logger
            );
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
        public static void ResetTest1()
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
        public static void ResetTest2()
        {
            var instance = InitInstance.GenerateForMembersTest();
            instance.Reset();

            // 意図したとおり処理が転送されていること
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock.ValidatorMock,
                nameof(instance.ExtendedListMock.ValidatorMock.Reset)
            );
            TestTemplateWithMock.AssertEqualsCalledMemberHistory(
                instance.ExtendedListMock,
                nameof(IExtendedList<StubModel>.Count),
                nameof(IExtendedList<StubModel>.Validator),
                nameof(IExtendedList<StubModel>.ResetCore)
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

            public static TransportMembersTestFixedLengthList GenerateForMembersTest()
            {
                var result = new TransportMembersTestFixedLengthList();
                result.ExtendedListMock.ClearCalledHistory();
                return result;
            }

            /// <summary>
            /// 内部で使用する <see cref="IExtendedList{T}"/> をモックに差し替えただけ。
            /// 本来継承先で実装すべきメソッドはいずれも未実装。
            /// </summary>
            public class TransportMembersTestFixedLengthList :
                FixedLengthList<StubModel, TransportMembersTestFixedLengthList>
            {
                public MockExtendedList<StubModel> ExtendedListMock => (MockExtendedList<StubModel>)Items;

                private protected override IExtendedList<StubModel> Items { get; } = new MockExtendedList<StubModel>();

                public TransportMembersTestFixedLengthList() : base(0)
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
                public override bool ItemEquals(TransportMembersTestFixedLengthList? other) => false;
                protected override StubModel MakeDefaultItem(int index) => new (index.ToString());
                protected override IWodiLibListValidator<StubModel> GenerateValidatorForItems() => new MockWodiLibListValidator<StubModel>();
                public override TransportMembersTestFixedLengthList DeepClone() => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }

            #endregion

            #region For Propagate Test

            public static PropagateTestFixedLengthList GenerateForPropagateTest(int count = 0) => new(count);

            /// <summary>
            /// 内部で使用する <see cref="IExtendedList{T}"/> を差し替えないバージョン。
            /// <see cref="IExtendedList{T}"/> の通知が <see cref="FixedLengthList{T,TImpl}"/> に伝播することを確認するためのクラス。
            /// また、要素数を引数とするコンストラクタのテスト用クラスを兼ねる。
            /// </summary>
            public class PropagateTestFixedLengthList : FixedLengthList<StubModel, PropagateTestFixedLengthList>
            {
                public Func<int, StubModel> DefaultItemFactory => MakeDefaultItem;

                public PropagateTestFixedLengthList(int count) : base(count)
                {
                }

                public PropagateTestFixedLengthList(IExtendedList<StubModel> itemsImpl) : base(itemsImpl)
                {
                }

                public PropagateTestFixedLengthList(IEnumerable<StubModel> itemsImpl, int capacity) : base(
                    itemsImpl,
                    capacity
                )
                {
                }

                public bool IsReferenceEqualsWithItems(object? other)
                {
                    return ReferenceEquals(Items, other);
                }

                // @formatter:off
                #region NotUse
                public override bool ItemEquals(PropagateTestFixedLengthList? other) => false;
                protected override StubModel MakeDefaultItem(int index) => new (index.ToString());
                protected override IWodiLibListValidator<StubModel> GenerateValidatorForItems() => new MockWodiLibListValidator<StubModel>();
                public override PropagateTestFixedLengthList DeepClone() => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }

            #endregion
        }
    }
}
