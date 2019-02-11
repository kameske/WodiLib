using NUnit.Framework;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.Test.Map
{
    [TestFixture]
    public class EventBootConditionTest
    {
        private static readonly object[] SetCriteriaOperatorTestCaseSource =
        {
            new object[] {CriteriaOperator.Above, false},
            new object[] {null, true},
        };

        [TestCaseSource(nameof(SetCriteriaOperatorTestCaseSource))]
        public static void SetCriteriaOperatorTest(CriteriaOperator operation, bool isError)
        {
            var instance = new MapEventBootCondition();
            var errorOccured = false;
            try
            {
                instance.Operation = operation;
            }
            catch (PropertyNullException)
            {
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }
    }
}