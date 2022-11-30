// ========================================
// Project Name : WodiLib
// File Name    : ModelBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WodiLib.Sys
{
    /// <summary>
    ///     各Modelクラス基底クラス
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract partial class ModelBase<TChild> : IModelBase<TChild>,
        IEqualityComparable<ModelBase<TChild>>
        where TChild : ModelBase<TChild>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private event PropertyChangedEventHandler? _propertyChanged;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public WodiLibContainerKeyName? ContainerKeyName { get; set; } = null;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructors
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ（通常）
        /// </summary>
        protected ModelBase()
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc/>
        public abstract bool ItemEquals(TChild? other);

        /// <inheritdoc/>
        public bool ItemEquals(ModelBase<TChild>? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return ItemEquals((TChild)other);
        }

        /// <inheritdoc/>
        public virtual bool ItemEquals(object? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ItemEquals(other as ModelBase<TChild>);
        }

        /// <inheritdoc/>
        public virtual TChild DeepClone()
        {
            // TODO: ビルドエラー回避のため一時的に virtual 宣言。あとで abstract 宣言する。
            throw new NotImplementedException();
        }
        // public abstract TChild DeepClone();

        /// <inheritdoc/>
        object IDeepCloneable.DeepClone()
            => DeepClone();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// フィールドに要素をセットする。
        /// 値が変化する場合、プロパティ変更通知イベントを発火させる。
        /// </summary>
        /// <remarks>
        /// 値が変更しているかどうかは
        ///     <ul>
        ///     <li><typeparamref name="T"/> が <see cref="IEqualityComparable"/> を実装していれば <see cref="IEqualityComparable.ItemEquals"/> による比較により決定する</li>
        ///     <li><typeparamref name="T"/> が <see cref="IEqualityComparable"/> を実装していなければ <see cref="EqualityComparer{T}.Default"/> による比較により決定する</li>
        ///     </ul>
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        protected void SetField<T>(ref T source, T value, [CallerMemberName] string propertyName = "")
        {
            var isNotDifference = EqualsHelper.NullableEquals(source, value);

            if (isNotDifference) return;

            source = value;
            NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        ///     プロパティ変更後イベント
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var arg = PropertyChangedEventArgsCache.GetInstance(propertyName);
            _propertyChanged?.Invoke(this, arg);
        }

        /// <summary>
        ///     <see cref="INotifyPropertyChanged"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <remarks>
        ///     <see langword="public"/> ではないフィールド（処理転送先など）に対して設定することを前提としている。<br/>
        ///     また、このメソッドで登録したインスタンスは自身と同じライフタイムであることを前提としている。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は無条件に通知される。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        protected void PropagatePropertyChangeEvent(INotifyPropertyChanged target)
        {
            target.PropertyChanged += (_, args) => { _propertyChanged?.Invoke(this, args); };
        }

        /// <summary>
        ///     <see cref="INotifyPropertyChanged"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <remarks>
        ///     注意点は <see cref="PropagatePropertyChangeEvent(INotifyPropertyChanged)"/> 参照。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は
        ///     <paramref name="allowNotifyPropertyList"/> に含まれるプロパティ名のみ通知される。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="allowNotifyPropertyList">プロパティ通知許可リスト</param>
        protected void PropagatePropertyChangeEvent(
            INotifyPropertyChanged target,
            IEnumerable<string> allowNotifyPropertyList
        )
        {
            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender,
                    args,
                    allowNotifyPropertyList
                );

                if (notifyArgs is null) return;

                _propertyChanged?.Invoke(this, notifyArgs);
            };
        }

        /// <summary>
        ///     <see cref="INotifyPropertyChanged"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <remarks>
        ///     注意点は <see cref="PropagatePropertyChangeEvent(INotifyPropertyChanged)"/> 参照。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は
        ///     <paramref name="filterNotifyPropertyName"/> を通して返却されたプロパティ名で通知される。
        ///     <paramref name="filterNotifyPropertyName"/> で <see langword="null"/> が返却された場合は通知しない。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="filterNotifyPropertyName">プロパティ通知フィルタデリゲート</param>
        protected void PropagatePropertyChangeEvent(
            INotifyPropertyChanged target,
            PropertyChangeNotificationHelper.FilterNotifyPropertyName? filterNotifyPropertyName
        )
        {
            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender,
                    args,
                    filterNotifyPropertyName
                );

                if (notifyArgs is null) return;

                _propertyChanged?.Invoke(this, notifyArgs);
            };
        }

        /// <summary>
        ///     <see cref="INotifyPropertyChanged"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <remarks>
        ///     注意点は <see cref="PropagatePropertyChangeEvent(INotifyPropertyChanged)"/> 参照。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は
        ///     <paramref name="mapNotifyPropertyName"/> を通して返却されたプロパティ名で通知される。
        ///     複数のプロパティ名で通知させることも可能。
        ///     <paramref name="mapNotifyPropertyName"/> で <see langword="null"/> が返却された場合は通知しない。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="mapNotifyPropertyName">伝播プロパティ名 Mapper</param>
        protected void PropagatePropertyChangeEvent(
            INotifyPropertyChanged target,
            PropertyChangeNotificationHelper.MapNotifyPropertyName? mapNotifyPropertyName
        )
        {
            if (mapNotifyPropertyName is null)
            {
                PropagatePropertyChangeEvent(target);
                return;
            }

            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender,
                    args,
                    mapNotifyPropertyName
                );

                notifyArgs?.ForEach(arg => _propertyChanged?.Invoke(this, arg));
            };
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
    }
}
