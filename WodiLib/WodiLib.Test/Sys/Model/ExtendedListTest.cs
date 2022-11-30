using System;
using System.Collections;
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
        private static Logger logger = default!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        #region Events

        [Test]
        public static void NotifyPropertyChangedTest()
        {
            var instance = InitInstance.GenerateInstanceForCore();

            var notifiedPropertyNameList = new List<string>();
            instance.PropertyChanged += (_, args) => notifiedPropertyNameList.Add(args.PropertyName);

            instance.SetCore(0, new StubModel("setTest"));

            // 通知が行われていること
            Assert.IsTrue(notifiedPropertyNameList.Count == 1);
            Assert.IsTrue(notifiedPropertyNameList[0] == ListConstant.IndexerName);
        }

        [Test]
        public static void NotifyPropertyChangedByItemTest()
        {
            var instance = InitInstance.GenerateInstanceForCore();

            var notifiedPropertyNameList = new List<string>();
            instance.PropertyChanged += (_, args) => notifiedPropertyNameList.Add(args.PropertyName);

            instance[0].StringValue = "update value";

            // 通知が行われていること
            Assert.IsTrue(notifiedPropertyNameList.Count == 1);
            Assert.IsTrue(notifiedPropertyNameList[0] == ListConstant.IndexerName);
        }

        /*
         * コレクション変更通知のテストは SimpleList で行う。
         * ExtendedList は内部的には SimpleList を呼び出す前提であるため。
         * 通知が正しく伝播していることを確認するために Set パターンだけ確認する
         */

        [Test]
        public static void NotifyCollectionChangeEventArgsTest_Get()
        {
            var instance = InitInstance.GenerateInstanceForCore();

            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();
            instance.CollectionChanged += (_, args) => collectionChangedEventArgsList.Add(args);

            instance.SetCore(0, new StubModel("setTest"));

            // 通知が行われていること
            Assert.IsTrue(collectionChangedEventArgsList.Count == 1);
            Assert.IsTrue(collectionChangedEventArgsList[0].Action == NotifyCollectionChangedAction.Replace);
        }

        #endregion

        #region Implementations of ExtendedList

        [Test]
        public static void MakeDefaultItemGetTest()
        {
            var instance = InitInstance.GenerateExtendedList();
            var getResult = instance.MakeDefaultItem;

            // 正しく取得されること
            Assert.NotNull(getResult);
        }

        [Test]
        public static void ValidatorGetTest()
        {
            var instance = InitInstance.GenerateExtendedList();
            var getResult = instance.Validator;

            // 正しく取得されること
            Assert.NotNull(getResult);
        }

        private static readonly object[] ConstructorTestCaseSource = TestItemGenerator.GenerateTestCaseSource(
            (
                // makeListDefaultItem
                new DelegateMakeListDefaultItem<string>?[]
                {
                    null,
                    i => i.ToString()
                },
                // validator
                new IWodiLibListValidator<string>?[]
                {
                    null,
                    new MockWodiLibListValidator<string>(),
                },
                // initItems
                new[]
                {
                    null,
                    3.Iterate(i => i.ToString()),
                }
            ),
            // expectedError
            (makeListDefaultItem, /*validator*/_, /*initItems*/_) =>
                makeListDefaultItem is null
        );

        [TestCaseSource(nameof(ConstructorTestCaseSource))]
        public static void ConstructorTest(
            DelegateMakeListDefaultItem<string>? makeListDefaultItem,
            IWodiLibListValidator<string>? validator,
            IEnumerable<string>? initItems,
            bool expectedError
        )
        {
            TestTemplate.Constructor(
                () => new ExtendedList<string>(makeListDefaultItem!, validator!, initItems),
                expectedThrowCreateNewInstance: expectedError,
                logger
            );
        }

        public enum ItemEqualsTestType
        {
            Null,
            ReferenceEquals,
            SameItems,
            DifferenceOneItem,
            Longer,
            Shorter,
            OtherInstanceInItem,
        }

        [TestCase(ItemEqualsTestType.Null, false)]
        [TestCase(ItemEqualsTestType.ReferenceEquals, true)]
        [TestCase(ItemEqualsTestType.SameItems, true)]
        [TestCase(ItemEqualsTestType.DifferenceOneItem, false)]
        [TestCase(ItemEqualsTestType.Longer, false)]
        [TestCase(ItemEqualsTestType.Shorter, false)]
        [TestCase(ItemEqualsTestType.OtherInstanceInItem, true)]
        public static void ItemEqualsTest(ItemEqualsTestType testType, bool expected)
        {
            var instance = InitInstance.GenerateExtendedList();

            var other = testType switch
            {
                ItemEqualsTestType.Null => null,
                ItemEqualsTestType.ReferenceEquals => instance,
                ItemEqualsTestType.SameItems => instance.DeepClone(),
                ItemEqualsTestType.DifferenceOneItem => ((Func<IExtendedList<StubModel>>)(() =>
                {
                    var result = InitInstance.GenerateExtendedList();
                    result[0] = new StubModel("___no match___");
                    return result;
                }))(),
                ItemEqualsTestType.Longer => ((Func<IExtendedList<StubModel>>)(() =>
                {
                    var result = InitInstance.GenerateExtendedList();
                    result.AddCore(new StubModel("add item"));
                    return result;
                }))(),
                ItemEqualsTestType.Shorter => ((Func<IExtendedList<StubModel>>)(() =>
                {
                    var result = InitInstance.GenerateExtendedList();
                    result.RemoveCore(InitInstance.InitLength - 1);
                    return result;
                }))(),
                ItemEqualsTestType.OtherInstanceInItem => new InitInstance.AnotherExtendedList(),
                _ => throw new ArgumentOutOfRangeException(nameof(testType), testType, null)
            };

            TestTemplate.ItemEquals(
                instance,
                other,
                expected,
                logger
            );
        }

        [Test]
        public static void DeepCloneTest()
        {
            var instance = InitInstance.GenerateExtendedList();

            TestTemplate.DeepClone(
                instance,
                logger
            );
        }

        #endregion

        #region Transport Methods or Properties

        [Test]
        public static void IndexerGetTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            var _ = instance[0];

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance,
                nameof(IWodiLibListValidator<StubModel>.Get)
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Get)
            );
        }

        [Test]
        public static void IndexerSetTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance[0] = new StubModel("x");

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance,
                nameof(IWodiLibListValidator<StubModel>.Set)
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Set)
            );
        }

        [Test]
        public static void CountGetTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            var _ = instance.Count;

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Count)
            );
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            using var _ = instance.GetEnumerator();

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.GetEnumerator)
            );
        }

        [Test]
        public static void GetRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            var _ = instance.GetRangeCore(0, 1);

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Get)
            );
        }

        [Test]
        public static void SetRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.SetRangeCore(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Set)
            );
        }

        [Test]
        public static void InsertRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.InsertRangeCore(0, new[] { new StubModel("new Value") });

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Insert)
            );
        }

        [Test]
        public static void OverwriteCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.OverwriteCore(0, InitInstance.InitItems);

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Overwrite)
            );
        }

        [Test]
        public static void MoveRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.MoveRangeCore(0, 1, 1);

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Move)
            );
        }

        [Test]
        public static void RemoveRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.RemoveRangeCore(0, 1);

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Remove)
            );
        }

        [Test]
        public static void AdjustRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.AdjustLengthCore(1);

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Adjust)
            );
        }

        [Test]
        public static void ResetCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.ResetCore(InitInstance.InitItems);

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Reset)
            );
        }

        [Test]
        public static void ClearRangeCoreTest()
        {
            var instance = InitInstance.GenerateInstanceForImplementations();
            instance.ClearCore();

            // 意図したとおり処理が転送されていること
            ExtendedListTestTemplate.AssertValidatorCalledMethodHistory(
                instance
            );
            ExtendedListTestTemplate.AssertSimpleListCalledMethodHistory(
                instance,
                nameof(ISimpleList<StubModel>.Clear)
            );
        }

        #endregion

        private static class InitInstance
        {
            public static readonly StubModel[] InitItems =
            {
                new("InitStr"),
                new("\t_"),
                new("初期文字列"),
                new("Init String"),
                new("string123"),
            };

            public static readonly DelegateMakeListDefaultItem<StubModel> MakeListDefaultItem = GenerateTestModel;

            public static int InitLength => InitItems.Length;

            public static TestListForCore GenerateInstanceForCore() => new();

            public static TestListForImplementations GenerateInstanceForImplementations()
            {
                var result = new TestListForImplementations();
                result.ValidatorMock.ClearCalledHistory();
                result.SimpleListMock.ClearCalledHistory();
                return result;
            }

            public static ExtendedList<StubModel> GenerateExtendedList(
                IEnumerable<StubModel>? initItems = null
            ) => new(
                GenerateTestModel,
                new MockWodiLibListValidator<StubModel>(),
                initItems ?? GetClonedInitItems()
            );

            public static StubModel GenerateTestModel(int index) => new($"{index}");

            private static StubModel[] GetClonedInitItems()
                => InitItems.Select(item => item.DeepClone()).ToArray();

            /// <summary>
            /// このリストは validator を設定していないため、テスト時には core メソッドを直接呼び出す必要がある。
            /// </summary>
            public class TestListForCore : ExtendedList<StubModel>
            {
                public TestListForCore() : base(MakeListDefaultItem, default!, GetClonedInitItems())
                {
                }
            }

            /// <summary>
            /// このリストは validator を設定している。
            /// </summary>
            public class TestListForImplementations : ExtendedList<StubModel>
            {
                public MockBase<IWodiLibListValidator<StubModel>> ValidatorMock
                    => (MockWodiLibListValidator<StubModel>)Validator!;

                public MockBase<ISimpleList<StubModel>> SimpleListMock => (MockSimpleList<StubModel>)Items;

                protected override ISimpleList<StubModel> Items { get; } =
                    new MockSimpleList<StubModel>(MakeListDefaultItem, InitItems);

                public TestListForImplementations(
                    IEnumerable<StubModel>? values = null
                ) : base(MakeListDefaultItem, new MockWodiLibListValidator<StubModel>(), values)
                {
                    Items = new MockSimpleList<StubModel>(MakeListDefaultItem, InitItems);
                }
            }

            /// <summary>
            /// <see cref="ExtendedList{T}.ItemEquals(IExtendedList{T}?)"/> メソッド用、
            /// <see cref="IExtendedList{T}"/> を実装した、 <see cref="ExtendedList{T}"/> とは関連のないクラス
            /// </summary>
            /// <remarks>
            /// 比較には <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource},IEnumerable{TSource})"/> を利用しているはずなので、
            /// <see cref="IEnumerable{T}"/> のメソッドのみ実装していれば良い
            /// </remarks>
            public class AnotherExtendedList : ModelBase<AnotherExtendedList>,
                IExtendedList<StubModel>
            {
                private SimpleList<StubModel> Items { get; }

                public AnotherExtendedList()
                {
                    Items = new SimpleList<StubModel>(MakeListDefaultItem, GetClonedInitItems());
                }

                #region IEnumerable<T>

                public IEnumerator<StubModel> GetEnumerator() => Items.GetEnumerator();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

                #endregion

                // @formatter:off
                #region NotUse

                IExtendedList<StubModel> IDeepCloneable<IExtendedList<StubModel>>.DeepClone() => throw new NotImplementedException();
                public override bool ItemEquals(AnotherExtendedList? other) => throw new NotImplementedException();
                public override AnotherExtendedList DeepClone() => throw new NotImplementedException();
                public bool ItemEquals(IExtendedList<StubModel>? other) => throw new NotImplementedException();
                public event NotifyCollectionChangedEventHandler? CollectionChanged;
                public StubModel this[int index]
                {
                    get => throw new NotImplementedException();
                    set => throw new NotImplementedException();
                }
                public int Count => throw new NotImplementedException();
                public Func<int, StubModel> MakeDefaultItem  => throw new NotImplementedException();
                public IWodiLibListValidator<StubModel> Validator  => throw new NotImplementedException();
                public IEnumerable<StubModel> GetRangeCore(int index, int count) => throw new NotImplementedException();
                public void SetRangeCore(int index, IEnumerable<StubModel> items) => throw new NotImplementedException();
                public void InsertRangeCore(int index, IEnumerable<StubModel> items) => throw new NotImplementedException();
                public void OverwriteCore(int index, IEnumerable<StubModel> items) => throw new NotImplementedException();
                public void MoveRangeCore(int oldIndex, int newIndex, int count) => throw new NotImplementedException();
                public void RemoveRangeCore(int index, int count) => throw new NotImplementedException();
                public void AdjustLengthCore(int length) => throw new NotImplementedException();
                public void ResetCore(IEnumerable<StubModel> items) => throw new NotImplementedException();
                public void ClearCore() => throw new NotImplementedException();
                public bool ItemEquals(IEnumerable<StubModel>? other) => throw new NotImplementedException();
                #endregion
                // @formatter:on
            }
        }

        private static class ExtendedListTestTemplate
        {
            public static void AssertValidatorCalledMethodHistory(
                InitInstance.TestListForImplementations list,
                params string[] expectedValidationCalled
            )
            {
                TestTemplateWithMock.AssertEqualsCalledMemberHistory(list.ValidatorMock, expectedValidationCalled);
            }

            public static void AssertSimpleListCalledMethodHistory(
                InitInstance.TestListForImplementations list,
                params string[] expectedSimpleListCalled
            )
            {
                TestTemplateWithMock.AssertEqualsCalledMemberHistory(list.SimpleListMock, expectedSimpleListCalled);
            }
        }
    }
}
