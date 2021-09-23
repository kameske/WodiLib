// ========================================
// Project Name : WodiLib
// File Name    : MapTreeNode.cs
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
    ///     マップツリーノード
    /// </summary>
    public record MapTreeNode
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     自身のマップ番号
        /// </summary>
        public MapId MyMapId { get; }

        /// <summary>
        ///     親のマップイベントID
        /// </summary>
        public MapId ParentMapId { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="me">自身のマップID</param>
        /// <param name="parent">親のマップID</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="me"/> に <see cref="MapId.MapTreeRoot"/> を指定した場合、
        ///     または <paramref name="me"/> と <paramref name="parent"/> が同じ値の場合
        /// </exception>
        public MapTreeNode(MapId me, MapId parent)
        {
            ThrowHelper.ValidateArgumentNotNull(me is null, nameof(me));
            ThrowHelper.ValidateArgumentNotNull(parent is null, nameof(parent));

            if (me == MapId.MapTreeRoot)
                throw new ArgumentException(
                    ErrorMessage.Deny(nameof(me), $"{nameof(MapId)}.{nameof(MapId.MapTreeRoot)}"));
            if (me == parent)
                throw new ArgumentException(
                    ErrorMessage.NotEqual(nameof(me), nameof(parent)));

            MyMapId = me;
            ParentMapId = parent;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        ///     byte配列に変換する。
        /// </summary>
        /// <param name="endian">エンディアン</param>
        /// <returns>byte配列</returns>
        public IEnumerable<byte> ToBytes(Endian endian)
        {
            var result = new List<byte>();

            result.AddRange(ParentMapId.ToBytes(endian));

            result.AddRange(MyMapId.ToBytes(endian));

            return result;
        }
    }
}
