// ========================================
// Project Name : WodiLib
// File Name    : EventLayerOpacityType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// マップ編集時の下レイヤーの暗さ種別
    /// </summary>
    public class EventLayerOpacityType : TypeSafeEnum<EventLayerOpacityType>
    {
        /// <summary>100%</summary>
        public static readonly EventLayerOpacityType Completely;

        /// <summary>75%</summary>
        public static readonly EventLayerOpacityType ThreeQuoter;

        /// <summary>50%</summary>
        public static readonly EventLayerOpacityType Half;

        /// <summary>25%</summary>
        public static readonly EventLayerOpacityType Quoter;

        /// <summary>表示しない</summary>
        public static readonly EventLayerOpacityType Not;

        static EventLayerOpacityType()
        {
            Completely = new EventLayerOpacityType(nameof(Completely), "4");
            ThreeQuoter = new EventLayerOpacityType(nameof(ThreeQuoter), "3");
            Half = new EventLayerOpacityType(nameof(Half), "2");
            Quoter = new EventLayerOpacityType(nameof(Quoter), "1");
            Not = new EventLayerOpacityType(nameof(Not), "0");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        public EventLayerOpacityType(string id, string code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public string Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">[NotNull] コード</param>
        /// <returns>EventLayerOpacityType</returns>
        /// <exception cref="ArgumentNullException">code が null の場合</exception>
        /// <exception cref="ArgumentException">存在しない値の場合</exception>
        public static EventLayerOpacityType FromCode(string code)
        {
            if (code is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(code)));

            try
            {
                return _FindFirst(x => x.Code == code);
            }
            catch
            {
                var exception = new ArgumentException($"{nameof(EventLayerOpacityType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        ///     対象コードが存在しない場合はデフォルト値を返す。
        /// </summary>
        /// <param name="code">[Nullable] コード</param>
        /// <returns>EventCommandShortCutKey</returns>
        public static EventLayerOpacityType FromCodeOrDefault(string code)
        {
            if (code is null || code.Equals(string.Empty)) return Quoter;
            return FromCode(code);
        }
    }
}