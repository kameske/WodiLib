using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class FixedLengthListTest
    {
        private static WodiLibLogger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupWodiLibLoggerForDebug();
            logger = WodiLibLogger.GetInstance();
        }

        [TestCase(TestClassType.Type1, false)]
        [TestCase(TestClassType.Type2, true)]
        [TestCase(TestClassType.Type3, true)]
        public static void ConstructorTest1(TestClassType testType, bool isError)
        {
            var initLength = 0;

            var errorOccured = false;

            AbsCollectionTest instance = null;
            try
            {
                switch (testType)
                {
                    case TestClassType.Type1:
                        initLength = CollectionTest1.Capacity;
                        instance = new CollectionTest1();
                        break;
                    case TestClassType.Type2:
                        initLength = CollectionTest2.Capacity;
                        instance = new CollectionTest2();
                        break;
                    case TestClassType.Type3:
                        initLength = CollectionTest3.Capacity;
                        instance = new CollectionTest3();
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 初期要素が容量最小数と一致すること
            Assert.AreEqual(instance.Count, initLength);
        }

        [TestCase(TestClassType.Type1, -1, true)]
        [TestCase(TestClassType.Type1, 0, true)]
        [TestCase(TestClassType.Type1, 10, false)]
        [TestCase(TestClassType.Type1, 11, true)]
        [TestCase(TestClassType.Type2, -1, true)]
        [TestCase(TestClassType.Type2, 0, true)]
        [TestCase(TestClassType.Type2, 10, true)]
        [TestCase(TestClassType.Type2, 11, true)]
        [TestCase(TestClassType.Type3, -1, true)]
        [TestCase(TestClassType.Type3, 0, true)]
        [TestCase(TestClassType.Type3, 10, true)]
        [TestCase(TestClassType.Type3, 11, true)]
        public static void ConstructorTest2(TestClassType testType, int initLength, bool isError)
        {
            var errorOccured = false;

            var initList = MakeStringList(initLength);

            AbsCollectionTest instance = null;
            try
            {
                switch (testType)
                {
                    case TestClassType.Type1:
                        instance = new CollectionTest1(initList);
                        break;
                    case TestClassType.Type2:
                        instance = new CollectionTest2(initList);
                        break;
                    case TestClassType.Type3:
                        instance = new CollectionTest3(initList);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 要素数がセットした要素数と一致すること
            Assert.AreEqual(instance.Count, initLength);
        }

        [TestCase(-1, true)]
        [TestCase(0, false)]
        [TestCase(9, false)]
        [TestCase(10, true)]
        public static void IndexerGetTest(int index, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out var countDic, out var handlerCalledCount);
            var errorOccured = false;
            try
            {
                var _ = instance[index];
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが一度も呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);

            // 各イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);
        }

        [TestCase(-1, "abc", true)]
        [TestCase(0, "abc", false)]
        [TestCase(9, "abc", false)]
        [TestCase(10, "abc", true)]
        [TestCase(-1, null, true)]
        [TestCase(0, null, true)]
        [TestCase(9, null, true)]
        [TestCase(10, null, true)]
        public static void IndexerSetTest(int index, string setItem, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out var countDic, out var handlerCalledCount);

            var errorOccured = false;
            try
            {
                instance[index] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], isError ? 0 : 1);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString],
                isError ? 0 : 1);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            if (errorOccured) return;

            for (var i = 0; i < 10; i++)
            {
                if (i != index)
                {
                    // 初期値が変化していないこと
                    Assert.AreEqual(instance[i], i.ToString());
                }
                else
                {
                    // セットした値が反映されていること
                    Assert.AreEqual(instance[i], setItem);
                }
            }
        }

        // Count プロパティのテストは ConstructorTest1, ConstructorTest2 が兼ねる

        [TestCase(-1, -1, true)]
        [TestCase(-1, 0, true)]
        [TestCase(-1, 7, true)]
        [TestCase(-1, 8, true)]
        [TestCase(0, -1, true)]
        [TestCase(0, 0, false)]
        [TestCase(0, 10, false)]
        [TestCase(0, 11, true)]
        [TestCase(9, -1, true)]
        [TestCase(9, 0, false)]
        [TestCase(9, 1, false)]
        [TestCase(9, 2, true)]
        [TestCase(10, -1, true)]
        [TestCase(10, 0, true)]
        [TestCase(10, 1, true)]
        public static void GetRangeTest(int index, int count, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out var countDic, out var handlerCalledCount);
            var errorOccured = false;
            IEnumerable<string> result = null;

            try
            {
                result = instance.GetRange(index, count);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 各Virtualメソッドが一度も呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            if (errorOccured || result == null) return;

            var resultArray = result as string[] ?? result.ToArray();

            // 取得した要素数が一致すること
            Assert.AreEqual(resultArray.Length, count);

            // 取得した要素が取得元と一致すること
            for (var i = 0; i < count; i++)
            {
                Assert.AreEqual(resultArray[i], instance[index + i]);
            }
        }

        [TestCase(10)]
        public static void ClearTest(int initLength)
        {
            var instance = MakeCollectionForMethodTest(initLength, out var countDic, out var handlerCalledCount);

            var capacity = instance.GetCapacity();

            var errorOccured = false;
            try
            {
                instance.Clear();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 各Virtualメソッドが意図した回数呼ばれていること
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 1);

            // 各有効イベントハンドラが意図した回数呼ばれていること
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 1);

            // 各無効イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);

            // 要素数が容量と一致すること
            Assert.AreEqual(instance.Count, capacity);

            // すべての要素がデフォルト要素と一致すること
            foreach (var t in instance)
            {
                Assert.AreEqual(t, "test");
            }
        }

        [TestCase(10, "1", true)]
        [TestCase(10, "11", false)]
        [TestCase(10, null, false)]
        public static void ContainsTest(int initLength, string item, bool result)
        {
            var instance = MakeCollectionForMethodTest(initLength, out var countDic, out var handlerCalledCount);
            var containsResult = false;

            var errorOccured = false;
            try
            {
                containsResult = instance.Contains(item);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(containsResult, result);

            // 各Virtualメソッドが呼ばれていないこと
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnSetItemCalled)], 0);
            Assert.AreEqual(countDic[nameof(CollectionTest1.OnClearItemsCalled)], 0);

            // 各イベントハンドラが一度も呼ばれていないこと
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString], 0);
            Assert.AreEqual(handlerCalledCount[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString], 0);
        }

        [TestCase("1", 1)]
        [TestCase("11", -1)]
        [TestCase(null, -1)]
        public static void IndexOfTest(string item, int result)
        {
            var instance = MakeCollectionForMethodTest(10, out _, out _);
            var indexOfResult = -1;

            var errorOccured = false;
            try
            {
                indexOfResult = instance.IndexOf(item);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // 取得した値が意図した値と一致すること
            Assert.AreEqual(indexOfResult, result);


            // 初期値が変化していないこと
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(instance[i], i.ToString());
            }
        }

        [TestCase(9, -1, true)]
        [TestCase(9, 0, true)]
        [TestCase(9, 1, true)]
        [TestCase(10, -1, true)]
        [TestCase(10, 0, false)]
        [TestCase(10, 1, true)]
        [TestCase(11, -1, true)]
        [TestCase(11, 0, false)]
        [TestCase(11, 1, false)]
        [TestCase(11, 2, true)]
        public static void CopyToTest(int arrayLength, int index, bool isError)
        {
            var instance = MakeCollectionForMethodTest(10, out _, out _);
            var copyArray = MakeStringArray(arrayLength);

            var errorOccured = false;
            try
            {
                instance.CopyTo(copyArray, index);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            // 初期値が変化していないこと
            for (var j = 0; j < 10; j++)
            {
                Assert.AreEqual(instance[j], j.ToString());
            }

            if (errorOccured) return;

            // 配列の要素（コピー領域より前）が変化していないこと
            var i = 0;
            for (; i < index; i++)
            {
                Assert.AreEqual(copyArray[i], (i * 100).ToString());
            }

            // 配列の要素（コピーした領域）がコピーした内容で上書きされていること
            for (; i < 10; i++)
            {
                Assert.AreEqual(copyArray[i + index], i.ToString());
            }
        }

        [Test]
        public static void GetEnumeratorTest()
        {
            var instance = MakeCollectionForMethodTest(10, out _, out _);
            // foreachを用いた処理で要素が正しく取得できること
            var i = 0;
            foreach (var value in instance)
            {
                Assert.AreEqual(value, i.ToString());
                i++;
            }
        }

        [Test]
        public static void SerializeTest()
        {
            var target = MakeCollectionForMethodTest(10, out _, out _);
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      テストデータ
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static List<string> MakeStringList(int length)
        {
            if (length < 0) return null;
            var result = new List<string>();
            for (var i = 0; i < length; i++)
            {
                result.Add(i.ToString());
            }

            return result;
        }

        private static string[] MakeStringArray(int length)
        {
            var result = new string[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = (i * 100).ToString();
            }

            return result;
        }

        private static CollectionTest1 MakeCollectionForMethodTest(int initLength,
            out Dictionary<string, int> methodCalledCount,
            out Dictionary<string, Dictionary<string, int>> handlerCalledCount)
        {
            var initStringList = MakeStringList(initLength);
            var result = initStringList == null
                ? new CollectionTest1()
                : new CollectionTest1(initStringList);

            methodCalledCount = new Dictionary<string, int>
            {
                {nameof(CollectionTest1.OnSetItemCalled), 0},
                {nameof(CollectionTest1.OnClearItemsCalled), 0},
            };

            var ints = methodCalledCount;
            result.OnSetItemCalled = () => ints[nameof(CollectionTest1.OnSetItemCalled)]++;
            result.OnClearItemsCalled = () => ints[nameof(CollectionTest1.OnClearItemsCalled)]++;

            handlerCalledCount = new Dictionary<string, Dictionary<string, int>>
            {
                {
                    nameof(CollectionTest1.OnSetItemCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
                {
                    nameof(CollectionTest1.OnClearItemsCalled),
                    new Dictionary<string, int>
                    {
                        {bool.TrueString, 0}, {bool.FalseString, 0}
                    }
                },
            };

            var hccs = handlerCalledCount;
            result.SetItemHandlerList.Add(
                new OnSetItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnSetItemCalled)][bool.TrueString]++; },
                    bool.TrueString, false
                )
            );
            result.SetItemHandlerList.Add(
                new OnSetItemHandler<string>(
                    (i, s) => { hccs[nameof(CollectionTest1.OnSetItemCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );
            result.ClearItemHandlerList.Add(
                new OnClearItemHandler<string>(
                    () => { hccs[nameof(CollectionTest1.OnClearItemsCalled)][bool.TrueString]++; }, bool.TrueString,
                    false
                )
            );
            result.ClearItemHandlerList.Add(
                new OnClearItemHandler<string>(
                    () => { hccs[nameof(CollectionTest1.OnClearItemsCalled)][bool.FalseString]++; }, bool.FalseString,
                    false, false
                )
            );

            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //      テスト用クラス
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public enum TestClassType
        {
            Type1,
            Type2,
            Type3
        }

        private abstract class AbsCollectionTest : FixedLengthList<string>
        {
            public AbsCollectionTest()
            {
            }

            public AbsCollectionTest(IReadOnlyCollection<string> list) : base(list)
            {
            }

            // FixedLengthList<T>継承クラスはデシリアライズ時に呼び出せるconstructor(SerializationInfo, StreamingContext)が必須
            protected AbsCollectionTest(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        private class CollectionTest1 : AbsCollectionTest
        {
            /**
             * 正常設定
             */

            public static int Capacity => 10;

            public static string Default => "test";

            [field: NonSerialized] public Action OnSetItemCalled { get; set; }
            [field: NonSerialized] public Action OnClearItemsCalled { get; set; }

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest1()
            {
            }

            public CollectionTest1(IReadOnlyCollection<string> list) : base(list)
            {
            }

            protected override void SetItem(int index, string item)
            {
                base.SetItem(index, item);
                OnSetItemCalled?.Invoke();
            }

            protected override void ClearItems()
            {
                base.ClearItems();
                OnClearItemsCalled?.Invoke();
            }

            protected CollectionTest1(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        private class CollectionTest2 : AbsCollectionTest
        {
            /*
             * 異常設定（Capacity < 0）
             */

            public static int Capacity => -2;
            public static string Default => "test";

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest2()
            {
            }

            public CollectionTest2(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }

        private class CollectionTest3 : AbsCollectionTest
        {
            /**
             * 異常設定（DefaultValue＝null）
             */
            public static int Capacity => 10;

            public static string Default => null;

            public override int GetCapacity() => Capacity;

            protected override string MakeDefaultItem(int index) => Default;

            public CollectionTest3()
            {
            }

            public CollectionTest3(IReadOnlyCollection<string> list) : base(list)
            {
            }
        }
    }
}