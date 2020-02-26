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
    /// マップツリーノード
    /// </summary>
    [Serializable]
    public readonly struct MapTreeNode : IEquatable<MapTreeNode>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自身のマップ番号
        /// </summary>
        public MapId MyMapId { get; }

        /// <summary>
        /// 親のマップイベントID
        /// </summary>
        public MapId ParentMapId { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="me">自身のマップID</param>
        /// <param name="parent">親のマップID</param>
        /// <exception cref="ArgumentException">
        ///     自身のマップIDにMapId.MapTreeRootを指定した場合、
        ///     またはmeとparentが同じ値の場合
        /// </exception>
        public MapTreeNode(MapId me, MapId parent)
        {
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
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Parent: {ParentMapId}, Me: {MyMapId}";
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MapTreeNode other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (MyMapId.GetHashCode() * 397) ^ ParentMapId.GetHashCode();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// byte配列に変換する。
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

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(MapTreeNode other)
        {
            return MyMapId == other.MyMapId
                   && ParentMapId == other.ParentMapId;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Operator
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ==
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺と右辺の</returns>
        public static bool operator ==(MapTreeNode left, MapTreeNode right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// !=
        /// </summary>
        /// <param name="left">左辺</param>
        /// <param name="right">右辺</param>
        /// <returns>左辺!=右辺の場合true</returns>
        public static bool operator !=(MapTreeNode left, MapTreeNode right)
        {
            return !(left == right);
        }
    }
}