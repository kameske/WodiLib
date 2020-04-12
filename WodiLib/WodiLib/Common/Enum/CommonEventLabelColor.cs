// ========================================
// Project Name : WodiLib
// File Name    : CommonEventLabelColor.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using Commons;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベントラベル色
    /// </summary>
    public class CommonEventLabelColor : TypeSafeEnum<CommonEventLabelColor>
    {
        /// <summary>黒</summary>
        public static readonly CommonEventLabelColor Black;

        /// <summary>赤</summary>
        public static readonly CommonEventLabelColor Red;

        /// <summary>青</summary>
        public static readonly CommonEventLabelColor Blue;

        /// <summary>緑</summary>
        public static readonly CommonEventLabelColor Green;

        /// <summary>紫</summary>
        public static readonly CommonEventLabelColor Purple;

        /// <summary>黄</summary>
        public static readonly CommonEventLabelColor Yellow;

        /// <summary>灰</summary>
        public static readonly CommonEventLabelColor Gray;

        static CommonEventLabelColor()
        {
            Black = new CommonEventLabelColor(nameof(Black), 0);
            Red = new CommonEventLabelColor(nameof(Red), 1);
            Blue = new CommonEventLabelColor(nameof(Blue), 2);
            Green = new CommonEventLabelColor(nameof(Green), 3);
            Purple = new CommonEventLabelColor(nameof(Purple), 4);
            Yellow = new CommonEventLabelColor(nameof(Yellow), 5);
            Gray = new CommonEventLabelColor(nameof(Gray), 6);
        }

        /// <summary>色コード</summary>
        public int Code { get; }

        private CommonEventLabelColor(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>
        /// コード値からインスタンスを返す。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        public static CommonEventLabelColor FromInt(int code)
        {
            return AllItems.First(x => x.Code == code);
        }
    }
}