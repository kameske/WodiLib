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
    public class ReadOnlyExtendedListTest
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
        public static void PropagateNotifyPropertyChange_ItemProperty()
        {
            // 要素のPropertyChangedイベントが発火したとき、 "Items[]" の変更通知が行われること

            var instance = InitInstance.GenerateForPropagateTest(5);
            var notifiedPropertyNames = new List<string>();

            instance.PropertyChanged += (_, args) => notifiedPropertyNames.Add(args.PropertyName);

            instance[0].StringValue = "updated";

            Assert.AreEqual(1, notifiedPropertyNames.Count);
            Assert.IsTrue(notifiedPropertyNames[0].Equals(ListConstant.IndexerName));
        }

        #endregion

        #region Implementations

        #region Constructor

        [Test]
        public static void ConstructorTest_SpecifyInitItems()
        {
            var initItems = 7.Iterate(i => new StubModel(i.ToString())).ToList();
            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestReadOnlyExtendedList(initItems),
                expectedThrowCreateNewInstance: false,
                logger
            );

            // 初期要素が initItems と一致すること
            Assert.AreEqual(7, instance.Count);
            instance.ForEach(
                (item, i) => { Assert.IsTrue(item.ItemEquals(initItems[i])); }
            );
        }

        [Test]
        public static void ConstructorTest_InjectPropertyItems()
        {
            var list = new MockExtendedList<StubModel>();

            var instance = TestTemplate.Constructor(
                () => new InitInstance.PropagateTestReadOnlyExtendedList(list),
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

        #endregion

        #endregion

        private static class InitInstance
        {
            #region For Transport Test

            public static TransportMembersTestReadOnlyExtendedList GenerateForMembersTest()
            {
                var result = new TransportMembersTestReadOnlyExtendedList();
                result.ExtendedListMock.ClearCalledHistory();
                return result;
            }

            /// <summary>
            /// 内部で使用する <see cref="IExtendedList{T}"/> をモックに差し替えただけ。
            /// </summary>
            public class TransportMembersTestReadOnlyExtendedList :
                ReadOnlyExtendedList<StubModel, TransportMembersTestReadOnlyExtendedList>
            {
                public MockExtendedList<StubModel> ExtendedListMock => (MockExtendedList<StubModel>)Items;

                private protected override IExtendedList<StubModel> Items { get; } = new MockExtendedList<StubModel>();

                public TransportMembersTestReadOnlyExtendedList() : base(Array.Empty<StubModel>())
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
                public override bool ItemEquals(TransportMembersTestReadOnlyExtendedList? other) => false;
                protected override IWodiLibListValidator<StubModel> GenerateValidatorForItems() => new MockWodiLibListValidator<StubModel>();
                public override TransportMembersTestReadOnlyExtendedList DeepClone() => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }

            #endregion

            #region For Propagate Test

            public static PropagateTestReadOnlyExtendedList GenerateForPropagateTest(int count = 0)
                => new(count.Iterate(i => new StubModel(i.ToString())));

            /// <summary>
            /// 内部で使用する <see cref="IExtendedList{T}"/> を差し替えないバージョン。
            /// <see cref="IExtendedList{T}"/> の通知が <see cref="ReadOnlyExtendedList{T,TImpl}"/> に伝播することを確認するためのクラス。
            /// またコンストラクタのテストにも使用する。
            /// </summary>
            public class PropagateTestReadOnlyExtendedList :
                ReadOnlyExtendedList<StubModel, PropagateTestReadOnlyExtendedList>
            {
                public PropagateTestReadOnlyExtendedList(IExtendedList<StubModel> itemsImpl) : base(itemsImpl)
                {
                }

                public PropagateTestReadOnlyExtendedList(IEnumerable<StubModel> initItems) : base(initItems)
                {
                }

                public bool IsReferenceEqualsWithItems(object? other)
                {
                    return ReferenceEquals(Items, other);
                }

                // @formatter:off
                #region NotUse
                public override bool ItemEquals(PropagateTestReadOnlyExtendedList? other) => false;
                protected override IWodiLibListValidator<StubModel> GenerateValidatorForItems() => new MockWodiLibListValidator<StubModel>();
                public override PropagateTestReadOnlyExtendedList DeepClone() => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }

            #endregion
        }
    }
}
