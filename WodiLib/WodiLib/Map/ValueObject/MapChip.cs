// ========================================
// Project Name : WodiLib
// File Name    : MapChip.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     [Range(0, 1604444)] マップチップ
    /// </summary>
    [Serializable]
    public readonly struct MapChip : IConvertibleInt, IEquatable<MapChip>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>タイル番号最小値</summary>
        public static int StandardTileMin => 0;

        /// <summary>タイル番号最大値</summary>
        public static int StandardTileMax => 99999;

        /// <summary>オートタイル番号最小値</summary>
        public static int AutoTileMin => 100000;

        /// <summary>オートタイル番号最大値</summary>
        public static int AutoTileMax => 1604444;

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

        private int Value { get; }

        /// <summary>オートタイルフラグ</summary>
        public bool IsAutoTile { get; }

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

        private int LeftUpAutoTileCode => Value % (LeftUpTileCoefficient * 10) / LeftUpTileCoefficient;
        private int RightUpAutoTileCode => Value % (RightUpTileCoefficient * 10) / RightUpTileCoefficient;
        private int LeftDownAutoTileCode => Value % (LeftDownTileCoefficient * 10) / LeftDownTileCoefficient;
        private int RightDownAutoTileCode => Value % (RightDownTileCoefficient * 10) / RightDownTileCoefficient;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// staticインスタンス生成コンストラクタ
        /// </summary>
        static MapChip()
        {
            Default = new MapChip(DefaultValue);
        }

        /// <summary>
        /// コンストラクタ（マップチップ番号指定版）
        /// </summary>
        /// <param name="value">[Range(0, 1604444)] マップチップ番号</param>
        /// <exception cref="ArgumentOutOfRangeException">0～1604444以外の値を設定した場合</exception>
        public MapChip(int value)
        {
            if (!IsMapChipNumber(value))
                throw new ArgumentOutOfRangeException(ErrorMessage.OutOfRange(nameof(value), StandardTileMin,
                    AutoTileMax, value));
            Value = value;
            IsAutoTile = IsAutoTileNumber(value);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is MapChip other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public int ToInt() => this;

        /// <summary>
        /// int に変換する。
        /// </summary>
        /// <returns>int値</returns>
        public int ToInt32() => this;

        /// <summary>
        /// byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian) => Value.ToBytes(endian);

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(MapChip other)
        {
            return Value == other.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Implicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> MapChip への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator MapChip(int src)
        {
            var result = new MapChip(src);
            return result;
        }

        /// <summary>
        /// MapChip -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator int(MapChip src)
        {
            return src.Value;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺の</returns>
        public static bool operator ==(MapChip left, MapChip right)
        {
            return left.Value == right.Value;
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(MapChip left, MapChip right)
        {
            return !(left == right);
        }
    }
}
