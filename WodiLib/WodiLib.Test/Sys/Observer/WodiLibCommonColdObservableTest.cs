using System;
using System.Reactive.Disposables;
using System.Threading;
using NUnit.Framework;
using WodiLib.Sys.Observer;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class WodiLibCommonColdObservableTest
    {
        [Test]
        public static void SubscribeTest_1()
        {
            var variable = 0;
            var taskSetInt = 10;
            var onNextCalled = false;
            var onErrorCalled = false;
            var onCompletedCalled = false;
            var onNextItem = false;

            var _ = WLObservable<bool>.Create(s =>
            {
                variable = taskSetInt;
                s.OnNext(true);
                s.OnCompleted();
                return Disposable.Empty;
            }).Subscribe(
                    b =>
                    {
                        onNextCalled = true;
                        onNextItem = b;
                    },
                    e => onErrorCalled = true,
                    () => onCompletedCalled = true);
            // onComplete, onErrorの発生を最大1秒待つ
            for (var i = 0; i < 1000; i++)
            {
                if (onErrorCalled || onCompletedCalled) break;
                Thread.Sleep(10);
            }
            
            // 各値が意図した結果となっていること
            Assert.AreEqual(variable, taskSetInt);
            Assert.IsTrue(onNextCalled);
            Assert.IsFalse(onErrorCalled);
            Assert.IsTrue(onCompletedCalled);
            Assert.AreEqual(onNextItem, true);
        }
        
        [Test]
        public static void SubscribeTest_2()
        {
            var onNextCalled = false;
            var onErrorCalled = false;
            var onCompletedCalled = false;

            var _ = WLObservable<bool>.Create(s =>
                {
                    s.OnError(new InvalidOperationException());
                    s.OnNext(true);
                    return Disposable.Empty;
                })
                .Subscribe(
                    b => onNextCalled = true,
                    e => onErrorCalled = true,
                    () => onCompletedCalled = true);
            
            // onComplete, onErrorの発生を最大1秒待つ
            for (var i = 0; i < 2000; i++)
            {
                if (onErrorCalled || onCompletedCalled) break;
                Thread.Sleep(10);
            }
            
            // 各値が意図した結果となっていること
            Assert.IsFalse(onNextCalled);
            Assert.IsTrue(onErrorCalled);
            Assert.IsFalse(onCompletedCalled);
        }
    }
}