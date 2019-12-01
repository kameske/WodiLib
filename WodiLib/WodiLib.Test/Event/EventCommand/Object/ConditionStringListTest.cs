using NUnit.Framework;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Event.EventCommand
{
    [TestFixture]
    public class ConditionStringListTest
    {
        [Test]
        public static void GetCapacityTest()
        {
            var instance = new ConditionStringList();
            var maxCapacity = instance.GetCapacity();

            // 取得した値が容量最大値と一致すること
            Assert.AreEqual(maxCapacity, ConditionStringList.Capacity);
        }

        private static readonly object[] SearchUseNumberVariableForRightSideMaxTestCaseSource =
        {
            new object[] {1, false, false, false, false, 1},
            new object[] {1, true, false, false, false, 0},
            new object[] {2, false, false, false, false, 2},
            new object[] {2, true, false, false, false, 2},
            new object[] {2, false, true, false, false, 1},
            new object[] {2, true, true, false, false, 0},
            new object[] {3, false, false, false, false, 3},
            new object[] {3, true, false, false, false, 3},
            new object[] {3, false, true, false, false, 3},
            new object[] {3, false, false, true, false, 2},
            new object[] {3, true, true, false, false, 3},
            new object[] {3, true, false, true, false, 2},
            new object[] {3, false, true, true, false, 1},
            new object[] {3, true, true, true, false, 0},
            new object[] {4, false, false, false, false, 4},
            new object[] {4, true, false, false, false, 4},
            new object[] {4, false, true, false, false, 4},
            new object[] {4, false, false, true, false, 4},
            new object[] {4, false, false, false, true, 3},
            new object[] {4, true, true, false, false, 4},
            new object[] {4, true, false, true, false, 4},
            new object[] {4, true, false, false, true, 3},
            new object[] {4, false, true, true, false, 4},
            new object[] {4, false, true, false, true, 3},
            new object[] {4, false, false, true, true, 2},
            new object[] {4, true, true, true, false, 4},
            new object[] {4, true, true, false, true, 3},
            new object[] {4, true, false, true, true, 2},
            new object[] {4, false, true, true, true, 1},
            new object[] {4, true, true, true, true, 0},
        };

        [TestCaseSource(nameof(SearchUseNumberVariableForRightSideMaxTestCaseSource))]
        public static void SearchUseNumberVariableForRightSideMaxTest(int conditionValue, bool is1Str, bool is2Str,
            bool is3Str, bool is4Str, int result)
        {
            var instance = new ConditionStringList(new[]
            {
                new ConditionStringDesc
                {
                    Condition = StringConditionalOperator.Equal,
                    LeftSide = 0,
                    RightSide = is1Str ? (IntOrStr) "a" : 0,
                    IsUseNumberVariable = !is1Str
                },
                new ConditionStringDesc
                {
                    Condition = StringConditionalOperator.Equal,
                    LeftSide = 0,
                    RightSide = is2Str ? (IntOrStr) "a" : 0,
                    IsUseNumberVariable = !is2Str
                },
                new ConditionStringDesc
                {
                    Condition = StringConditionalOperator.Equal,
                    LeftSide = 0,
                    RightSide = is3Str ? (IntOrStr) "a" : 0,
                    IsUseNumberVariable = !is3Str
                },
                new ConditionStringDesc
                {
                    Condition = StringConditionalOperator.Equal,
                    LeftSide = 0,
                    RightSide = is4Str ? (IntOrStr) "a" : 0,
                    IsUseNumberVariable = !is4Str
                },
                new ConditionStringDesc(), new ConditionStringDesc(),new ConditionStringDesc(),
                new ConditionStringDesc(),new ConditionStringDesc(),new ConditionStringDesc(),
                new ConditionStringDesc(),new ConditionStringDesc(),new ConditionStringDesc(),
                new ConditionStringDesc(),new ConditionStringDesc(),
            });
            // 条件数を正しく
            instance.ConditionValue = conditionValue;

            var max = instance.SearchUseNumberVariableForRightSideMax();
            Assert.AreEqual(max, result);
        }

        [Test]
        public static void SerializeTest()
        {
            var target = new ConditionStringList {[0] = {LeftSide = 2200023}};
            var clone = DeepCloner.DeepClone(target);
            Assert.IsTrue(clone.Equals(target));
        }
    }
}