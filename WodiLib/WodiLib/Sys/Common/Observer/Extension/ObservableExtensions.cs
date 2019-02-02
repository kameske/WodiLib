// ========================================
// Project Name : WodiLib
// File Name    : ObservableExtensions.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Observer.Extension
{
    /// <summary>
    /// Observable拡張クラス
    /// </summary>
    internal static class ObservableExtensions
    {
        /// <summary>
        /// Observableを購読する（WodiLib独自処理）
        /// </summary>
        /// <param name="source">[NotNull] 対象Observable</param>
        /// <param name="onNext">onNextイベント</param>
        /// <param name="onError">onErrorイベント</param>
        /// <param name="onComplete">onCompleteイベント</param>
        /// <typeparam name="T">通知クラス型</typeparam>
        /// <returns>Disposable</returns>
        /// <exception cref="ArgumentNullException">sourceがnullの場合</exception>
        public static IDisposable Subscribe<T>(this IObservable<T> source,
            Action<T> onNext = null,
            Action<Exception> onError = null,
            Action onComplete = null)
        {
            if (source == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(source)));
            return source.Subscribe(new AnonymousObserver<T>(onNext, onError, onComplete));
        }

        /// <summary>
        /// 無名Observer
        /// </summary>
        /// <typeparam name="T">通知型</typeparam>
        public class AnonymousObserver<T> : IObserver<T>
        {
            private readonly Action<T> onNextAction;
            private readonly Action<Exception> onErrorAction;
            private readonly Action onCompletedAction;

            /// <inheritdoc />
            public AnonymousObserver(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
            {
                onNextAction = onNext ?? (_ => { });
                onErrorAction = onError ?? (exception => { });
                onCompletedAction = onCompleted ?? (() => { });
            }

            /// <inheritdoc />
            public void OnCompleted()
            {
                onCompletedAction();
            }

            /// <inheritdoc />
            public void OnError(Exception error)
            {
                onErrorAction(error);
            }

            /// <inheritdoc />
            public void OnNext(T value)
            {
                onNextAction(value);
            }
        }
    }
}