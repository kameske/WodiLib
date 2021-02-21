// ========================================
// Project Name : WodiLib
// File Name    : IWodiLibConfig.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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
        ///     新規作成した <see cref="IReadOnlyModelBase{TChild}"/> の
        ///     <see cref="IReadOnlyModelBase{TChild}.IsNotifyBeforePropertyChange"/> 初期価値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyModelBase{TChild}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see langword="false"/> が設定されている。
        ///     </para>
        /// </remarks>
        public bool DefaultNotifyBeforePropertyChangeFlag { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyModelBase{TChild}"/> の
        ///     <see cref="IReadOnlyModelBase{TChild}.IsNotifyAfterPropertyChange"/> 初期価値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyModelBase{TChild}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see langword="true"/> が設定されている。
        ///     </para>
        /// </remarks>
        public bool DefaultNotifyAfterPropertyChangeFlag { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyExtendedList{T}"/> の
        ///     <see cref="IReadOnlyExtendedList{T}.IsNotifyBeforeCollectionChange"/> 初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyExtendedList{T}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see langword="false"/> が設定されている。
        ///     </para>
        /// </remarks>
        public bool DefaultNotifyBeforeCollectionChangeFlag { get; set; }

        /// <summary>
        ///     新規作成した <see cref="IReadOnlyExtendedList{TChild}"/> の
        ///     <see cref="IReadOnlyExtendedList{T}.IsNotifyAfterCollectionChange"/>、
        ///     および <see cref="IReadOnlyTwoDimensionalList{TChild}"/> の
        ///     <see cref="IReadOnlyTwoDimensionalList{T}.IsNotifyAfterCollectionChange"/>初期化値
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="IReadOnlyExtendedList{T}"/> を実装するすべてのクラスが影響を受ける。<br/>
        ///         各種モデルクラスおよびリストクラスが該当。
        ///     </para>
        ///     <para>
        ///         デフォルトでは <see langword="false"/> が設定されている。
        ///     </para>
        /// </remarks>
        public bool DefaultNotifyAfterCollectionChangeFlag { get; set; }
    }
}
