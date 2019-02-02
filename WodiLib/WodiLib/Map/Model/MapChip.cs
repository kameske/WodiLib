// ========================================
// Project Name : WodiLib
// File Name    : MapChip.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップチップ
    /// </summary>
    public class MapChip
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>タイル番号最小値</summary>
        public static int StandardTileMin => 0;

        /// <summary>タイル番号最大値</summary>
        public static int StandardTileMax => 99999;

        /// <summary>オートタイル番号最小値</summary>
        public static int AutoTileMin => 100000;

        /// <summary>オートタイル番号最大値</summary>
        public static int AutoTileMax => 1604444;


        private static int LeftUpTileCoefficient => 1000;
        private static int RightUpTileCoefficient => 100;
        private static int LeftDownTileCoefficient => 10;
        private static int RightDownTileCoefficient => 1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int value = 104444;

        /// <summary>[Range(0, 1604444)] チップ番号</summary>
        /// <exception cref="PropertyOutOfRangeException">0～1604444以外の値を設定した場合</exception>
        public int Value
        {
            get => value;
            set
            {
                if (!IsMapChipNumber(value))
                    throw new PropertyOutOfRangeException($"マップチップ番号は{StandardTileMin}～{AutoTileMax}の範囲で指定する必要があります。");
                this.value = value;
            }
        }

        /// <summary>オートタイルフラグ</summary>
        public bool IsAutoTile
        {
            get
            {
                // オートタイル最小値判定
                if (Value < AutoTileMin) return false;
                // オートタイル最大値判定
                if (Value > AutoTileMax) return false;
                return true;
            }
        }

        /// <summary>左上オートタイルパーツ種別</summary>
        /// <exception cref="PropertyAccessException">オートタイルではない場合</exception>
        public AutoTilePartType LeftUpAutoTile
        {
            get
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                return AutoTilePartType.FromCode(LeftUpAutoTileCode);
            }
            set
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                Value -= LeftUpAutoTileCode * LeftUpTileCoefficient;
                Value += value.Code * LeftUpTileCoefficient;
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
            set
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                Value -= RightUpAutoTileCode * RightUpTileCoefficient;
                Value += value.Code * RightUpTileCoefficient;
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
            set
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                Value -= LeftDownAutoTileCode * LeftDownTileCoefficient;
                Value += value.Code * LeftDownTileCoefficient;
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
            set
            {
                if (!IsAutoTile) throw new PropertyAccessException("オートタイルではないため情報は取得できません");
                Value -= RightDownAutoTileCode * RightDownTileCoefficient;
                Value += value.Code * RightDownTileCoefficient;
            }
        }

        private int LeftUpAutoTileCode => Value % (LeftUpTileCoefficient * 10) / LeftUpTileCoefficient;
        private int RightUpAutoTileCode => Value % (RightUpTileCoefficient * 10) / RightUpTileCoefficient;
        private int LeftDownAutoTileCode => Value % (LeftDownTileCoefficient * 10) / LeftDownTileCoefficient;
        private int RightDownAutoTileCode => Value % (RightDownTileCoefficient * 10) / RightDownTileCoefficient;

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
        //     Explicit
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// int -> MapChip への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator MapChip(int src)
        {
            var result = new MapChip {Value = src};
            return result;
        }
    }
}