// ========================================
// Project Name : WodiLib
// File Name    : CommonEventArgType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     コモンイベント引数特殊指定タイプ
    /// </summary>
    public record CommonEventArgType : TypeSafeEnum<CommonEventArgType>
    {
        /// <summary>特殊な指定方法を使用しない</summary>
        public static readonly CommonEventArgType Normal;

        /// <summary>データベース参照</summary>
        public static readonly CommonEventArgType ReferDatabase;

        /// <summary>選択肢を手動生成</summary>
        public static readonly CommonEventArgType Manual;

        static CommonEventArgType()
        {
            Normal = new CommonEventArgType(nameof(Normal), 0x00);
            ReferDatabase = new CommonEventArgType(nameof(ReferDatabase), 0x01);
            Manual = new CommonEventArgType(nameof(Manual), 0x02);
        }

        private CommonEventArgType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>
        ///     コード値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static CommonEventArgType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }

        /// <inheritdoc/>
        public override string ToString()
            => base.ToString();
    }
}
