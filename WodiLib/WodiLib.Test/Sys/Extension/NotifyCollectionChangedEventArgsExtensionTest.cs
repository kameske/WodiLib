using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public static class NotifyCollectionChangedEventArgsExtensionTest
    {
        private static readonly object[] ExecuteByActionTestCaseSource =
        {
            new object[] { NotifyCollectionChangedEventArgsHelper.Insert(new[] { 1, 2 }, 5) },
            new object[] { NotifyCollectionChangedEventArgsHelper.Move(new[] { 1 }, 2, 0) },
            new object[] { NotifyCollectionChangedEventArgsHelper.Remove(new[] { 1, 2 }, 0) }
        };

        [TestCaseSource(nameof(ExecuteByActionTestCaseSource))]
        public static void ExecuteByActionTest(NotifyCollectionChangedEventArgs args)
        {
            var actionCountDic = new Dictionary<string, int>
            {
                { nameof(NotifyCollectionChangedAction.Replace), 0 },
                { nameof(NotifyCollectionChangedAction.Add), 0 },
                { nameof(NotifyCollectionChangedAction.Move), 0 },
                { nameof(NotifyCollectionChangedAction.Remove), 0 },
                { nameof(NotifyCollectionChangedAction.Reset), 0 }
            };
            args.ExecuteByAction<int>(
                (index, items, newItems) => actionCountDic[nameof(NotifyCollectionChangedAction.Replace)] += 1,
                (index, items) => actionCountDic[nameof(NotifyCollectionChangedAction.Add)] += 1,
                (index, newIndex, items) => actionCountDic[nameof(NotifyCollectionChangedAction.Move)] += 1,
                (index, items) => actionCountDic[nameof(NotifyCollectionChangedAction.Remove)] += 1,
                () => actionCountDic[nameof(NotifyCollectionChangedAction.Reset)] += 1
            );

            Assert.AreEqual(actionCountDic[nameof(NotifyCollectionChangedAction.Replace)],
                args.Action == NotifyCollectionChangedAction.Replace ? 1 : 0);
            Assert.AreEqual(actionCountDic[nameof(NotifyCollectionChangedAction.Add)],
                args.Action == NotifyCollectionChangedAction.Add ? 1 : 0);
            Assert.AreEqual(actionCountDic[nameof(NotifyCollectionChangedAction.Move)],
                args.Action == NotifyCollectionChangedAction.Move ? 1 : 0);
            Assert.AreEqual(actionCountDic[nameof(NotifyCollectionChangedAction.Remove)],
                args.Action == NotifyCollectionChangedAction.Remove ? 1 : 0);
            Assert.AreEqual(actionCountDic[nameof(NotifyCollectionChangedAction.Reset)],
                args.Action == NotifyCollectionChangedAction.Reset ? 1 : 0);
        }
    }
}
