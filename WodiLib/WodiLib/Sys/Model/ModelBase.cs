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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using WodiLib.Sys.Cmn;

namespace WodiLib.Sys
{
    /// <summary>
    /// 各Modelクラス基底クラス
    /// </summary>
    /// <typeparam name="TChild">Model実装クラス型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Serializable]
    public abstract partial class ModelBase<TChild> : IModelBase<TChild>, IEqualityComparable<ModelBase<TChild>>
        where TChild : ModelBase<TChild>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /* マルチスレッドを考慮して、イベントハンドラ本体の実装は自動実装に任せる。 */
        [field: NonSerialized] private event PropertyChangingEventHandler? _propertyChanging;

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        private bool isNotifyBeforePropertyChange
            = WodiLibConfig.GetDefaultNotifyBeforePropertyChangeFlag();

        /// <inheritdoc cref="IReadOnlyModelBase{TChild}.IsNotifyBeforePropertyChange" />
        public virtual bool IsNotifyBeforePropertyChange
        {
            get => isNotifyBeforePropertyChange;
            set
            {
                NotifyPropertyChanging();

                isNotifyBeforePropertyChange = value;

                Propagators.ForEach(item => item.IsNotifyBeforePropertyChange = value);

                NotifyPropertyChanged();
            }
        }

        private bool isNotifyAfterPropertyChange
            = WodiLibConfig.GetDefaultNotifyAfterPropertyChangeFlag();

        /// <inheritdoc cref="IReadOnlyModelBase{TChild}.IsNotifyAfterPropertyChange" />
        public virtual bool IsNotifyAfterPropertyChange
        {
            get => isNotifyAfterPropertyChange;
            set
            {
                NotifyPropertyChanging();

                isNotifyAfterPropertyChange = value;

                Propagators.ForEach(item => item.IsNotifyAfterPropertyChange = value);

                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// プロパティ変更通知伝播元リスト
        /// </summary>
        private List<INotifyPropertyChange> Propagators { get; }
            = new();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public abstract bool ItemEquals([AllowNull] TChild other);

        /// <inheritdoc />
        public bool ItemEquals(ModelBase<TChild>? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return Equals((TChild) other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// プロパティ変更前イベント
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
        {
            if (!IsNotifyBeforePropertyChange) return;

            var arg = PropertyChangingEventArgsCache.GetInstance(propertyName);
            _propertyChanging?.Invoke(this, arg);
        }

        /// <summary>
        /// プロパティ変更後イベント
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (!IsNotifyAfterPropertyChange) return;

            var arg = PropertyChangedEventArgsCache.GetInstance(propertyName);
            _propertyChanged?.Invoke(this, arg);
        }

        /// <summary>
        /// <see cref="INotifyPropertyChange"/> を実装するインスタンスが通知した
        /// <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントおよびを
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        /// 自身のイベントとして通知する。<br/>
        /// 反対に自分自身に設定された通知フラグを対象インスタンスに反映する。
        /// </summary>
        /// <remarks>
        /// <see langword="public"/> ではないフィールド（処理転送先など）に対して設定することを前提としている。<br/>
        /// また、このメソッドで登録したインスタンスは自身と同じライフタイムであることを前提としている。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="filterNotifyPropertyName">
        ///     プロパティ通知フィルタデリゲート。<br/>
        ///     <see langword="null"/> の場合無条件に伝播する。
        /// </param>
        protected void PropagatePropertyChangeEvent(INotifyPropertyChange target,
            PropertyChangeNotificationHelper.FilterNotifyPropertyName? filterNotifyPropertyName = null)
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
        /// <see cref="INotifyPropertyChange"/> を実装するインスタンスが通知した
        /// <see cref="INotifyPropertyChanging.PropertyChanging"/> イベントおよびを
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> イベントを
        /// 自身のイベントとして通知する。<br/>
        /// 反対に自分自身に設定された通知フラグを対象インスタンスに反映する。
        /// </summary>
        /// <remarks>
        /// <see langword="public"/> ではないフィールド（処理転送先など）に対して設定することを前提としている。<br/>
        /// また、このメソッドで登録したインスタンスは自身と同じライフタイムであることを前提としている。
        /// </remarks>
        /// <param name="target">イベント伝播元</param>
        /// <param name="mapNotifyPropertyName">
        ///     伝播プロパティ名 Mapper。<br/>
        ///     関数が <see langword="null"/> の場合無条件に伝播する。
        /// </param>
        protected void PropagatePropertyChangeEvent(INotifyPropertyChange target,
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

                if (notifyArgs is null) return;

                _propertyChanging?.Invoke(this, notifyArgs);
            };

            target.PropertyChanged += (sender, args) =>
            {
                var notifyArgs = PropertyChangeNotificationHelper.GetPropertyChangedEventArgs(
                    sender, args, mapNotifyPropertyName);

                if (notifyArgs is null) return;

                _propertyChanged?.Invoke(this, notifyArgs);
            };

            Propagators.Add(target);
        }
    }
}
