// ========================================
// Project Name : WodiLib
// File Name    : ModelBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WodiLib.Sys
{
    /// <summary>
    /// 各Modelクラス基底クラス
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public abstract class ModelBase<TChild> : IModelBase<TChild>, IEquatable<ModelBase<TChild>>
        where TChild : ModelBase<TChild>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event PropertyChangedEventHandler? _propertyChanged;

        /// <summary>
        /// プロパティ変更通知
        /// </summary>
        /// <remarks>
        ///     同じイベントを重複して登録することはできない。
        /// </remarks>
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (_propertyChanged != null
                    && _propertyChanged.GetInvocationList().Contains(value)) return;
                _propertyChanged += value;
            }
            remove => _propertyChanged -= value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>HashCode</summary>
        private int HashCode { get; } = new Random().Next();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public abstract bool Equals([AllowNull] TChild other);

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, this)) return true;
            if (ReferenceEquals(obj, null)) return false;

            if (obj is TChild equatable) return Equals(equatable);

            return false;
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ModelBase<TChild>? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return Equals((TChild) other);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// プロパティ変更イベント
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var arg = PropertyChangedEventArgsCache.GetInstance(propertyName);
            _propertyChanged?.Invoke(this, arg);
        }

        /// <summary>
        /// 指定した <see cref="INotifyPropertyChanged"/> を実装するインスタンスが通知した
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        /// 自身のイベントとして <see cref="PropertyChanged"/> イベントで通知する。
        /// </summary>
        /// <remarks>
        /// public ではないフィールド（処理転送先など）に対して設定することを前提としている。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="allowNotify">
        ///     伝播可否決定関数。第一引数は送信元、第二引数はイベント。<br/>
        ///     <see langword="null"/> の場合無条件に伝播する。
        /// </param>
        protected void PropagatePropertyChangeEvent(INotifyPropertyChanged target,
            Func<object, PropertyChangedEventArgs, bool>? allowNotify = null)
        {
            target.PropertyChanged += (sender, args) =>
            {
                if (!allowNotify?.Invoke(sender, args) ?? false) return;
                _propertyChanged?.Invoke(this, args);
            };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 等価演算子
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺が同じ参照を示す場合true</returns>
        public static bool operator ==(ModelBase<TChild>? left, ModelBase<TChild>? right)
        {
            return ReferenceEquals(left, right);
        }

        /// <summary>
        /// 等価演算子
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺が異なる参照の場合true</returns>
        public static bool operator !=(ModelBase<TChild>? left, ModelBase<TChild>? right)
        {
            return !(left == right);
        }
    }
}
