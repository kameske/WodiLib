// ========================================
// Project Name : WodiLib
// File Name    : MapChip.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(0, 1604444)] マップチップ
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 1604444)]
    public partial class MapChip
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>タイル番号最小値</summary>
        public static int StandardTileMin => Const_StandardTileMin;

        /// <summary>タイル番号最小値（定数）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public const int Const_StandardTileMin = 0;

        /// <summary>タイル番号最大値</summary>
        public static int StandardTileMax => Const_StandardTileMax;

        /// <summary>タイル番号最大値（定数）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public const int Const_StandardTileMax = 99999;

        /// <summary>オートタイル番号最小値</summary>
        public static int AutoTileMin => Const_AutoTileMin;

        /// <summary>オートタイル番号最小値（定数）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public const int Const_AutoTileMin = 100000;

        /// <summary>オートタイル番号最大値</summary>
        public static int AutoTileMax => Const_AutoTileMax;

        /// <summary>オートタイル番号最大値（定数）</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public const int Const_AutoTileMax = 1604444;

        /// <summary>デフォルト値</summary>
        public static MapChip Default { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     マップチップ番号として適切かどうか判定し、返す。
        /// </summary>
        /// <param name="value">判定値</param>
        /// <returns>適切な場合、true</returns>
        public static bool IsMapChipNumber(int value)
        {
            if (IsStandardTileNumber(value)) return true;
            if (IsAutoTileNumber(value)) return true;
            return false;
        }

        /// <summary>
        ///     通常（オートタイルではない）のマップチップ番号として適切かどうか判定し、返す。
        /// </summary>
        /// <param name="value">判定値</param>
        /// <returns>適切な場合、true</returns>
        public static bool IsStandardTileNumber(int value)
        {
            return StandardTileMin <= value && value <= StandardTileMax;
        }

        /// <summary>
        ///     オートタイルのマップチップ番号として適切かどうか判定し、返す。
        /// </summary>
        /// <param name="value">判定値</param>
        /// <returns>適切な場合、true</returns>
        public static bool IsAutoTileNumber(int value)
        {
            return AutoTileMin <= value && value <= AutoTileMax;
        }


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static int LeftUpTileCoefficient => 1000;
        private static int RightUpTileCoefficient => 100;
        private static int LeftDownTileCoefficient => 10;
        private static int RightDownTileCoefficient => 1;

        /// <summary>デフォルト値</summary>
        private static readonly int DefaultValue = 104444;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>オートタイルフラグ</summary>
        public bool IsAutoTile { get; private set; }

        /// <summary>左上オートタイルパーツ種別</summary>
        /// <exception cref="PropertyAccessException">オートタイルではない場合</exception>
        public AutoTilePartType LeftUpAutoTile
        {
            get
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                return AutoTilePartType.FromCode(LeftUpAutoTileCode);
            }
        }

        /// <summary>右上オートタイルパーツ種別</summary>
        /// <exception cref="PropertyAccessException">オートタイルではない場合</exception>
        public AutoTilePartType RightUpAutoTile
        {
            get
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                return AutoTilePartType.FromCode(RightUpAutoTileCode);
            }
        }

        /// <summary>左下オートタイルパーツ種別</summary>
        /// <exception cref="PropertyAccessException">オートタイルではない場合</exception>
        public AutoTilePartType LeftDownAutoTile
        {
            get
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                return AutoTilePartType.FromCode(LeftDownAutoTileCode);
            }
        }

        /// <summary>右下オートタイルパーツ種別</summary>
        /// <exception cref="PropertyAccessException">オートタイルではない場合</exception>
        public AutoTilePartType RightDownAutoTile
        {
            get
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                return AutoTilePartType.FromCode(RightDownAutoTileCode);
            }
        }

        private int LeftUpAutoTileCode => RawValue % (LeftUpTileCoefficient * 10) / LeftUpTileCoefficient;
        private int RightUpAutoTileCode => RawValue % (RightUpTileCoefficient * 10) / RightUpTileCoefficient;
        private int LeftDownAutoTileCode => RawValue % (LeftDownTileCoefficient * 10) / LeftDownTileCoefficient;
        private int RightDownAutoTileCode => RawValue % (RightDownTileCoefficient * 10) / RightDownTileCoefficient;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     staticインスタンス生成コンストラクタ
        /// </summary>
        static MapChip()
        {
            Default = new MapChip(DefaultValue);
        }

        partial void DoConstructorExpansion(int value)
        {
            if (!IsMapChipNumber(value))
                throw new ArgumentOutOfRangeException(ErrorMessage.OutOfRange(nameof(value), StandardTileMin,
                    AutoTileMax, value));
            IsAutoTile = IsAutoTileNumber(value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian) => RawValue.ToBytes(endian);
    }
}
