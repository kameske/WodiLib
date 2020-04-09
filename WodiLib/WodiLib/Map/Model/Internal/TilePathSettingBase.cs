// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定基底クラス
    /// </summary>
    [Serializable]
    internal abstract class TilePathSettingBase : ModelBase<TilePathSettingBase>, ITilePathSetting, ISerializable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>カウンター属性ONコード値</summary>
        protected readonly int CounterOnCode = 0x80;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイル通行許可
        /// </summary>
        public abstract TilePathPermission PathPermission { get; }

        /// <summary>
        /// タイル通行不可フラグ
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"部分的に通行不可"以外の場合</exception>
        public virtual TileImpassableFlags ImpassableFlags =>
            throw new PropertyAccessException(
                $"{nameof(PathPermission)}が{nameof(TilePathPermission.PartialDeny)}ではないため取得できません。");

        private TilePathOption pathOption = TilePathOption.Nothing;

        /// <summary>
        /// [NotNull] タイル通行設定オプション
        /// </summary>
        /// <exception cref="PropertyNullException">nullを設定した場合</exception>
        public TilePathOption PathOption
        {
            get => pathOption;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PathOption)));
                pathOption = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// [NotNull] タイル通行方向設定
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"、"部分的に通行不可"の場合</exception>
        public virtual TileCannotPassingFlags CannotPassingFlags =>
            throw new PropertyAccessException(
                $"{nameof(PathPermission)}が{nameof(TilePathPermission.PartialDeny)}または{nameof(TilePathPermission.Deny)}であるため取得できません。");

        private bool isCounter;

        /// <summary>
        /// カウンター属性
        /// </summary>
        public bool IsCounter
        {
            get => isCounter;
            set
            {
                isCounter = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        protected TilePathSettingBase()
        {
        }

        protected TilePathSettingBase(int code)
        {
            PathOption = TilePathOption.FromCode(code);
            if (PathOption is null)
            {
                // ここへは来ないはず
                throw new InvalidOperationException(
                    "タイル通行設定オプションが正しく取得できませんでした。");
            }

            IsCounter = (code & CounterOnCode) != 0;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public bool Equals(ITilePathSetting other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            if (!ChildEquals(other)) return false;

            return PathOption == other.PathOption
                   && IsCounter == other.IsCounter;
        }

        public override bool Equals(TilePathSettingBase other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return Equals(other);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Abstract Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 継承クラス独自のEquals判定メソッド
        /// </summary>
        protected abstract bool ChildEquals(ITilePathSetting other);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public abstract byte[] ToBinary();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(pathOption), pathOption.Code);
            info.AddValue(nameof(IsCounter), IsCounter);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected TilePathSettingBase(SerializationInfo info, StreamingContext context)
        {
            pathOption = TilePathOption.FromCode(info.GetByte(nameof(pathOption)));
            IsCounter = info.GetBoolean(nameof(IsCounter));
        }
    }
}