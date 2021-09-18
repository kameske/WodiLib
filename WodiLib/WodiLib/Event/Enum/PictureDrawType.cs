// ========================================
// Project Name : WodiLib
// File Name    : PictureDrawType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Event
{
    /// <summary>
    ///     ピクチャ表示形式
    /// </summary>
    public class PictureDrawType : TypeSafeEnum<PictureDrawType>
    {
        /// <summary>通常</summary>
        public static readonly PictureDrawType Normal;

        /// <summary>加算</summary>
        public static readonly PictureDrawType Add;

        /// <summary>減算</summary>
        public static readonly PictureDrawType Sub;

        /// <summary>乗算</summary>
        public static readonly PictureDrawType Multi;

        static PictureDrawType()
        {
            Normal = new PictureDrawType("Normal", 0x00,
                "通常");
            Add = new PictureDrawType("Add", 0x01,
                "加算");
            Sub = new PictureDrawType("Sub", 0x02,
                "減算");
            Multi = new PictureDrawType("Multi", 0x03,
                "乗算");
        }

        /// <summary>表示形式コード</summary>
        public byte Code { get; }

        /// <summary>イベントコマンド文</summary>
        public object EventCommandSentence { get; }

        private PictureDrawType(string id, byte code,
            string eventCommandSentence) : base(id)
        {
            Code = code;
            EventCommandSentence = eventCommandSentence;
        }

        /// <summary>
        ///     Byte値からオブジェクトを取得する。
        /// </summary>
        /// <param name="code">種別コード</param>
        /// <returns>DBType</returns>
        public static PictureDrawType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
