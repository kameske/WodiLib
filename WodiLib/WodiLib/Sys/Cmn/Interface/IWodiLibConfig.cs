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
        public WodiLibContainerKeyName KeyName { get; }

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
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> を設定しようとした場合。
        /// </exception>
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
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> を設定しようとした場合。
        /// </exception>
        public NotifyPropertyChangeEventType DefaultNotifyAfterPropertyChangeEventType { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyExtendedList{T}"/> の
        ///     <see cref="IReadOnlyExtendedList{T}.NotifyCollectionChangingEventType"/>、
        ///     および <see cref="IRestrictedCapacityList{T}"/> の
        ///     <see cref="IRestrictedCapacityList{T}.NotifyCollectionChangingEventType"/>、
        ///     および <see cref="IFixedLengthList{T}"/> の
        ///     <see cref="IFixedLengthList{T}.NotifyCollectionChangingEventType"/>、
        ///     および各二次元リストの NotifyCollectionChangingEventType 初期化値
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
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> を設定しようとした場合。
        /// </exception>
        public NotifyCollectionChangeEventType DefaultNotifyBeforeCollectionChangeEventType { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyExtendedList{T}"/> の
        ///     <see cref="IReadOnlyExtendedList{T}.NotifyCollectionChangedEventType"/>、
        ///     および <see cref="IRestrictedCapacityList{T}"/> の
        ///     <see cref="IRestrictedCapacityList{T}.NotifyCollectionChangedEventType"/>、
        ///     および <see cref="IFixedLengthList{T}"/> の
        ///     <see cref="IFixedLengthList{T}.NotifyCollectionChangedEventType"/>、
        ///     および各二次元リストの NotifyCollectionChangedEventType 初期化値
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
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> を設定しようとした場合。
        /// </exception>
        public NotifyCollectionChangeEventType DefaultNotifyAfterCollectionChangeEventType { get; set; }

        /// <summary>
        ///     新規作成した 二次元リスト の
        ///     NotifyTwoDimensionalListChangingEventType 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         デフォルトでは <see cref="NotifyTwoDimensionalListChangeEventType.None"/> が設定されている。
        ///     </para>
        /// </remarks>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> を設定しようとした場合。
        /// </exception>
        public NotifyTwoDimensionalListChangeEventType DefaultNotifyBeforeTwoDimensionalListChangeEventType
        {
            get;
            set;
        }

        /// <summary>
        ///     新規作成した 二次元リスト の
        ///     NotifyTwoDimensionalListChangedEventType 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         デフォルトでは <see cref="NotifyTwoDimensionalListChangeEventType.Single"/> が設定されている。
        ///     </para>
        /// </remarks>
        /// <exception cref="PropertyNullException">
        ///     <see langword="null"/> を設定しようとした場合。
        /// </exception>
        public NotifyTwoDimensionalListChangeEventType DefaultNotifyAfterTwoDimensionalListChangeEventType { get; set; }
    }
}
