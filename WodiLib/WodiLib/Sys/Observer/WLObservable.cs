// ========================================
// Project Name : WodiLib
// File Name    : WLObservable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Observer
{
    /// <inheritdoc />
    /// <summary>
    /// WodiLib用標準Observable
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    internal class WLObservable<TResult> : IObservable<TResult>
    {
        private readonly Func<IObserver<TResult>, IDisposable> func;

        private IObserver<TResult> myObserver;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="func">[NotNull] 実行内容</param>
        private WLObservable(Func<IObserver<TResult>, IDisposable> func)
        {
            if (func == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(func)));
            this.func = func;
        }

        public static IObservable<TResult> Create(Func<IObserver<TResult>, IDisposable> subscribe)
        {
            if (subscribe == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(subscribe)));
            return new WLObservable<TResult>(subscribe);
        }

        /// <inheritdoc />
        /// <summary>
        /// 購読を開始する。
        /// </summary>
        /// <param name="observer">[NotNull] Observer</param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<TResult> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(observer)));
            myObserver = observer;
            return func(myObserver);
        }

        public void ClearObserver()
        {
            myObserver = null;
        }
    }
}