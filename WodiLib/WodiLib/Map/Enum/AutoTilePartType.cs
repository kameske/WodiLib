// ========================================
// Project Name : WodiLib
// File Name    : AutoTilePartType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     オートタイルパーツ種別
    /// </summary>
    public class AutoTilePartType : TypeSafeEnum<AutoTilePartType>
    {
        /// <summary>中央への接続</summary>
        public static readonly AutoTilePartType ConnectionCentral;

        /// <summary>縦方向への接続</summary>
        public static readonly AutoTilePartType ConnectionVertical;

        /// <summary>横方向への接続</summary>
        public static readonly AutoTilePartType ConnectionHorizontal;

        /// <summary>外向きの接続</summary>
        public static readonly AutoTilePartType ConnectionOutSide;

        /// <summary>周囲が塗りつぶされた状態</summary>
        public static readonly AutoTilePartType SurroundingFilled;

        /// <summary>コード値</summary>
        public int Code { get; }

        static AutoTilePartType()
        {
            ConnectionCentral = new AutoTilePartType(nameof(ConnectionCentral), 0);
            ConnectionVertical = new AutoTilePartType(nameof(ConnectionVertical), 1);
            ConnectionHorizontal = new AutoTilePartType(nameof(ConnectionHorizontal), 2);
            ConnectionOutSide = new AutoTilePartType(nameof(ConnectionOutSide), 3);
            SurroundingFilled = new AutoTilePartType(nameof(SurroundingFilled), 4);
        }

        private AutoTilePartType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     コード値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static AutoTilePartType FromCode(int code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
