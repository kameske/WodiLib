// ========================================
// Project Name : WodiLib
// File Name    : IWodiLibConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys.Collections;

namespace WodiLib.Sys.Cmn
{
    /// <summary>
    ///     WodiLib 全体の設定クラス
    /// </summary>
    /// <remarks>
    ///     設定グループの新規作成や更新は <see cref="WodiLibConfig"/> の
    ///     各種 <see langword="static"/> メソッドを通じて行う。
    /// </remarks>
    public interface IWodiLibConfig
    {
        /// <summary>
        ///     設定キー名
        /// </summary>
        public string KeyName { get; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyModelBase{T}"/> の
        ///     <see cref="P:IReadOnlyModelBase{T}.NotifyPropertyChangingEventType"/> 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyModelBase{T}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see cref="NotifyPropertyChangeEventType.Disabled"/> が設定されている。
        ///     </para>
        /// </remarks>
        public NotifyPropertyChangeEventType DefaultNotifyBeforePropertyChangeEventType { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyModelBase{T}"/> の
        ///     <see cref="P:IReadOnlyModelBase{T}.NotifyPropertyChangedEventType"/> 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyModelBase{T}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see cref="NotifyPropertyChangeEventType.Enabled"/> が設定されている。
        ///     </para>
        /// </remarks>
        public NotifyPropertyChangeEventType DefaultNotifyAfterPropertyChangeEventType { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyExtendedList{T}"/> の
        ///     <see cref="P:IReadOnlyExtendedList{T}.NotifyCollectionChangingEventType"/>、
        ///     および <see cref="IRestrictedCapacityList{T}"/> の
        ///     <see cref="P:IRestrictedCapacityList{T}.NotifyCollectionChangingEventType"/>、
        ///     および <see cref="IFixedLengthList{T}"/> の
        ///     <see cref="P:IFixedLengthList{T}.NotifyCollectionChangingEventType"/>、
        ///     および <see cref="IReadOnlyTwoDimensionalList{T}"/> の
        ///     <see cref="P:IReadOnlyTwoDimensionalList{T}.NotifyCollectionChangingEventType"/> 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyExtendedList{T}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see cref="NotifyCollectionChangeEventType.None"/> が設定されている。
        ///     </para>
        /// </remarks>
        public NotifyCollectionChangeEventType DefaultNotifyBeforeCollectionChangeEventType { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyExtendedList{T}"/> の
        ///     <see cref="P:IReadOnlyExtendedList{T}.NotifyCollectionChangedEventType"/>、
        ///     および <see cref="IRestrictedCapacityList{T}"/> の
        ///     <see cref="P:IRestrictedCapacityList{T}.NotifyCollectionChangedEventType"/>、
        ///     および <see cref="IFixedLengthList{T}"/> の
        ///     <see cref="P:IFixedLengthList{T}.NotifyCollectionChangedEventType"/>、
        ///     および <see cref="IReadOnlyTwoDimensionalList{T}"/> の
        ///     <see cref="P:IReadOnlyTwoDimensionalList{T}.NotifyCollectionChangedEventType"/> 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyExtendedList{T}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see cref="NotifyCollectionChangeEventType.Single"/> が設定されている。
        ///     </para>
        /// </remarks>
        public NotifyCollectionChangeEventType DefaultNotifyAfterCollectionChangeEventType { get; set; }
    }
}
