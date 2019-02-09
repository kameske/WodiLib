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
    public struct MapChip
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

        private int value;

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

        private int LeftUpAutoTileCode => value % (LeftUpTileCoefficient * 10) / LeftUpTileCoefficient;
        private int RightUpAutoTileCode => value % (RightUpTileCoefficient * 10) / RightUpTileCoefficient;
        private int LeftDownAutoTileCode => value % (LeftDownTileCoefficient * 10) / LeftDownTileCoefficient;
        private int RightDownAutoTileCode => value % (RightDownTileCoefficient * 10) / RightDownTileCoefficient;

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
        /// <exception cref="PropertyOutOfRangeException">0～1604444以外の値を設定した場合</exception>
        public MapChip(int value)
        {
            if (!IsMapChipNumber(value))
                throw new PropertyOutOfRangeException($"マップチップ番号は{StandardTileMin}～{AutoTileMax}の範囲で指定する必要があります。");
            IsAutoTile = IsAutoTileNumber(value);
            this.value = value;
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
            var result = new MapChip(src);
            return result;
        }

        /// <summary>
        /// MapChip -> int への明示的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static explicit operator int(MapChip src)
        {
            return src.value;
        }
    }
}