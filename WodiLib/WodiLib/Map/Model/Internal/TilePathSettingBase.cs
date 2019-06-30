// ========================================
// Project Name : WodiLib
// File Name    : TilePathSettingBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// タイル通行許可設定基底クラス
    /// </summary>
    internal abstract class TilePathSettingBase : ITilePathSetting
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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(PathOption)));
                pathOption = value;
            }
        }

        /// <summary>
        /// [NotNull] タイル通行方向設定
        /// </summary>
        /// <exception cref="PropertyAccessException">タイル通行許可が"通行不可"、"部分的に通行不可"の場合</exception>
        public virtual TileCannotPassingFlags CannotPassingFlags =>
            throw new PropertyAccessException(
                $"{nameof(PathPermission)}が{nameof(TilePathPermission.PartialDeny)}または{nameof(TilePathPermission.Deny)}であるため取得できません。");

        /// <summary>
        /// カウンター属性
        /// </summary>
        public bool IsCounter { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        protected TilePathSettingBase()
        {
        }

        protected TilePathSettingBase(int code)
        {
            PathOption = TilePathOption.FromCode(code);
            if (PathOption == null)
            {
                // ここへは来ないはず
                throw new InvalidOperationException(
                    "タイル通行設定オプションが正しく取得できませんでした。");
            }

            IsCounter = (code & CounterOnCode) != 0;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public abstract byte[] ToBinary();
    }
}