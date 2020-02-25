// ========================================
// Project Name : WodiLib
// File Name    : TilePathOption.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     タイル通行設定オプション
    /// </summary>
    public class TilePathOption : TypeSafeEnum<TilePathOption>
    {
        /// <summary>キャラより上</summary>
        public static readonly TilePathOption AboveCharacter;

        /// <summary>後ろに隠れる</summary>
        public static readonly TilePathOption Hide;

        /// <summary>下半身半透明</summary>
        public static readonly TilePathOption TranslucentLowerBody;

        /// <summary>なし</summary>
        public static readonly TilePathOption Nothing;

        static TilePathOption()
        {
            AboveCharacter = new TilePathOption(nameof(AboveCharacter), 0x10);
            Hide = new TilePathOption(nameof(Hide), 0x01_00);
            TranslucentLowerBody = new TilePathOption(nameof(TranslucentLowerBody), 0x40);
            Nothing = new TilePathOption(nameof(Nothing), 0x00);
        }

        private TilePathOption(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>タイル通行設定オプション</summary>
        public int Code { get; }

        /// <summary>
        /// コード値からインスタンスを返す。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static TilePathOption FromCode(int code)
        {
            var searchedWithoutCodeZero = _FindAll().Where(x => x.Code != 0).FirstOrDefault(x => (x.Code & code) != 0);
            if (!(searchedWithoutCodeZero is null)) return searchedWithoutCodeZero;

            return _FindAll().First(x => x.Code == 0);
        }
    }
}