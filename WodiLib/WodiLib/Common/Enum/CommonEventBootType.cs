// ========================================
// Project Name : WodiLib
// File Name    : CommonEventBootType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     コモンイベント起動条件
    /// </summary>
    public class CommonEventBootType : TypeSafeEnum<CommonEventBootType>
    {
        /// <summary>呼び出しのみ</summary>
        public static readonly CommonEventBootType OnlyCall;

        /// <summary>自動実行</summary>
        public static readonly CommonEventBootType Auto;

        /// <summary>並列実行</summary>
        public static readonly CommonEventBootType Parallel;

        /// <summary>並列実行（常時）</summary>
        public static readonly CommonEventBootType ParallelAlways;

        static CommonEventBootType()
        {
            OnlyCall = new CommonEventBootType(nameof(OnlyCall), 0x00);
            Auto = new CommonEventBootType(nameof(Auto), 0x01);
            Parallel = new CommonEventBootType(nameof(Parallel), 0x02);
            ParallelAlways = new CommonEventBootType(nameof(ParallelAlways), 0x03);
        }

        /// <summary>起動条件コード</summary>
        public byte Code { get; }

        private CommonEventBootType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        ///     コード値からインスタンスを返す。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static CommonEventBootType FromByte(byte code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}
