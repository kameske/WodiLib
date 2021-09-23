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
using WodiLib.Sys.Cmn;

namespace WodiLib.Sys
{
    /// <summary>
    ///     各Modelクラス基底クラス
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract partial class ModelBase<TChild> : IModelBase<TChild>, IEqualityComparable<ModelBase<TChild>>
        where TChild : ModelBase<TChild>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event PropertyChangingEventHandler? _propertyChanging;

        /// <inheritdoc/>
        public virtual event PropertyChangingEventHandler PropertyChanging
        {
            add
            {
                if (_propertyChanging != null
                    && _propertyChanging.GetInvocationList().Contains(value)) return;
                _propertyChanging += value;
            }
            remove => _propertyChanging -= value;
        }

        [field: NonSerialized] private event PropertyChangedEventHandler? _propertyChanged;

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

        private NotifyPropertyChangeEventType notifyPropertyChangingEventType
            = WodiLibConfig.GetDefaultNotifyBeforePropertyChangeEventType();

        /// <inheritdoc cref="IReadOnlyModelBase{TChild}.NotifyPropertyChangingEventType"/>
        public virtual NotifyPropertyChangeEventType NotifyPropertyChangingEventType
        {
            get => notifyPropertyChangingEventType;
            set
            {
                NotifyPropertyChanging();
                notifyPropertyChangingEventType = value;
                Propagators.ForEach(item => item.NotifyPropertyChangingEventType = value);
                NotifyPropertyChanged();
            }
        }

        private NotifyPropertyChangeEventType isNotifyAfterPropertyChange
            = WodiLibConfig.GetDefaultNotifyAfterPropertyChangeEventType();

        /// <inheritdoc cref="IReadOnlyModelBase{T}.NotifyPropertyChangedEventType"/>
        public virtual NotifyPropertyChangeEventType NotifyPropertyChangedEventType
        {
            get => isNotifyAfterPropertyChange;
            set
            {
                NotifyPropertyChanging();
                isNotifyAfterPropertyChange = value;
                Propagators.ForEach(item => item.NotifyPropertyChangedEventType = value);
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     プロパティ変更通知伝播元リスト
        /// </summary>
        private List<INotifiablePropertyChange> Propagators { get; }
            = new();

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
        public bool ItemEquals(object? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other as ModelBase<TChild>);
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
        ///     プロパティ変更前イベント
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
        {
            if (!NotifyPropertyChangingEventType.IsNotify) return;

            var arg = PropertyChangingEventArgsCache.GetInstance(propertyName);
            _propertyChanging?.Invoke(this, arg);
        }

        /// <summary>
        ///     プロパティ変更後イベント
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (!NotifyPropertyChangedEventType.IsNotify) return;

            var arg = PropertyChangedEventArgsCache.GetInstance(propertyName);
            _propertyChanged?.Invoke(this, arg);
        }

        /// <summary>
        ///     <see cref="INotifiablePropertyChange"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントおよびを
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。<br/>
        ///     反対に自分自身に設定された通知フラグを対象インスタンスに反映する。
        /// </summary>
        /// <remarks>
        ///     <see langword="public"/> ではないフィールド（処理転送先など）に対して設定することを前提としている。<br/>
        ///     また、このメソッドで登録したインスタンスは自身と同じライフタイムであることを前提としている。<br/>
        ///     ただし、<paramref name="target"/> が通知した <see cref="INotifiablePropertyChange"/> の
        ///     プロパティ変更通知に関するフラグが変更された際の通知は通知されない。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は無条件に通知される。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        protected void PropagatePropertyChangeEvent(INotifiablePropertyChange target)
        {
            target.PropertyChanging += (_, args) => { _propertyChanging?.Invoke(this, args); };

            target.PropertyChanged += (_, args) => { _propertyChanged?.Invoke(this, args); };

            Propagators.Add(target);
        }

        /// <summary>
        ///     <see cref="INotifiablePropertyChange"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントおよびを
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。<br/>
        ///     反対に自分自身に設定された通知フラグを対象インスタンスに反映する。
        /// </summary>
        /// <remarks>
        ///     注意点は <see cref="PropagatePropertyChangeEvent(INotifiablePropertyChange)"/> 参照。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は
        ///     <paramref name="allowNotifyPropertyList"/> に含まれるプロパティ名のみ通知される。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="allowNotifyPropertyList">プロパティ通知許可リスト</param>
        protected void PropagatePropertyChangeEvent(INotifiablePropertyChange target,
            IEnumerable<string> allowNotifyPropertyList)
        {
            target.PropertyChanging += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangingEventArgs(
                    sender, args, allowNotifyPropertyList);

                if (notifyArgs is null) return;

                _propertyChanging?.Invoke(this, notifyArgs);
            };

            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender, args, allowNotifyPropertyList);

                if (notifyArgs is null) return;

                _propertyChanged?.Invoke(this, notifyArgs);
            };

            Propagators.Add(target);
        }

        /// <summary>
        ///     <see cref="INotifiablePropertyChange"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントおよびを
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。<br/>
        ///     反対に自分自身に設定された通知フラグを対象インスタンスに反映する。
        /// </summary>
        /// <remarks>
        ///     注意点は <see cref="PropagatePropertyChangeEvent(INotifiablePropertyChange)"/> 参照。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は
        ///     <paramref name="filterNotifyPropertyName"/> を通して返却されたプロパティ名で通知される。
        ///     <paramref name="filterNotifyPropertyName"/> で <see langword="null"/> が返却された場合は通知しない。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="filterNotifyPropertyName">プロパティ通知フィルタデリゲート</param>
        protected void PropagatePropertyChangeEvent(INotifiablePropertyChange target,
            PropertyChangeNotificationHelper.FilterNotifyPropertyName? filterNotifyPropertyName)
        {
            target.PropertyChanging += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangingEventArgs(
                    sender, args, filterNotifyPropertyName);

                if (notifyArgs is null) return;

                _propertyChanging?.Invoke(this, notifyArgs);
            };

            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender, args, filterNotifyPropertyName);

                if (notifyArgs is null) return;

                _propertyChanged?.Invoke(this, notifyArgs);
            };

            Propagators.Add(target);
        }

        /// <summary>
        ///     <see cref="INotifiablePropertyChange"/> を実装するインスタンスが通知した
        ///     <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントおよびを
        ///     <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        ///     自身のイベントとして通知する。<br/>
        ///     反対に自分自身に設定された通知フラグを対象インスタンスに反映する。
        /// </summary>
        /// <remarks>
        ///     注意点は <see cref="PropagatePropertyChangeEvent(INotifiablePropertyChange)"/> 参照。<br/>
        ///     このメソッドで登録された <paramref name="target"/> の通知は
        ///     <paramref name="mapNotifyPropertyName"/> を通して返却されたプロパティ名で通知される。
        ///     複数のプロパティ名で通知させることも可能。
        ///     <paramref name="mapNotifyPropertyName"/> で <see langword="null"/> が返却された場合は通知しない。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="mapNotifyPropertyName">伝播プロパティ名 Mapper</param>
        protected void PropagatePropertyChangeEvent(INotifiablePropertyChange target,
            PropertyChangeNotificationHelper.MapNotifyPropertyName? mapNotifyPropertyName)
        {
            if (mapNotifyPropertyName is null)
            {
                PropagatePropertyChangeEvent(target);
                return;
            }

            target.PropertyChanging += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangingEventArgs(
                    sender, args, mapNotifyPropertyName);

                notifyArgs?.ForEach(arg => _propertyChanging?.Invoke(this, arg));
            };

            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender, args, mapNotifyPropertyName);

                notifyArgs?.ForEach(arg => _propertyChanged?.Invoke(this, arg));
            };

            Propagators.Add(target);
        }
    }
}
