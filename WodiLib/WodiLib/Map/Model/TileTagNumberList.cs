// ========================================
// Project Name : WodiLib
// File Name    : TileTagNumberList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Map
{
    /// <summary>
    /// タイルタグ番号リストクラス
    /// </summary>
    public class TileTagNumberList : RestrictedCapacityList<TileTagNumber, TileTagNumberList>,
        IFixedLengthTileTagNumberList, IEquatable<TileTagNumberList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static readonly int MaxCapacity = TileId.MaxValue;

        /// <summary>リスト最小数</summary>
        public static readonly int MinCapacity = 24;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TileTagNumberList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     itemsがnullの場合、
        ///     またはitems中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が不適切な場合</exception>
        public TileTagNumberList(IEnumerable<TileTagNumber> items) : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_T/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override TileTagNumber MakeDefaultItem(int index)
            => new TileTagNumber();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public int GetCapacity() => Count;

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(TileTagNumberList? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return ItemEquals(other);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool ItemEquals(IReadOnlyFixedLengthList<TileTagNumber>? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return this.SequenceEqual(other);
        }

        /// <inheritdoc />
        public bool ItemEquals(IFixedLengthList<TileTagNumber>? other)
            => Equals((IEnumerable<TileTagNumber>?) other);

        IReadOnlyFixedLengthList<TileTagNumber> IDeepCloneable<IReadOnlyFixedLengthList<TileTagNumber>>.DeepClone()
        {
            throw new NotImplementedException();
        }

        IFixedLengthList<TileTagNumber> IDeepCloneable<IFixedLengthList<TileTagNumber>>.DeepClone()
        {
            throw new NotImplementedException();
        }

        IFixedLengthList<TileTagNumber> IFixedLengthList<TileTagNumber>.DeepCloneWith(
            IEnumerable<KeyValuePair<int, TileTagNumber>>? values)
        {
            throw new NotImplementedException();
        }

        IFixedLengthList<TileTagNumber> IFixedLengthList<TileTagNumber>.DeepCloneWith(int? length,
            IEnumerable<KeyValuePair<int, TileTagNumber>>? values)
        {
            throw new NotImplementedException();
        }

        IReadOnlyFixedLengthList<TileTagNumber> IReadOnlyFixedLengthList<TileTagNumber>.DeepCloneWith(
            IEnumerable<KeyValuePair<int, TileTagNumber>>? values)
        {
            throw new NotImplementedException();
        }

        IReadOnlyFixedLengthList<TileTagNumber> IReadOnlyFixedLengthList<TileTagNumber>.DeepCloneWith(int? length,
            IEnumerable<KeyValuePair<int, TileTagNumber>>? values)
        {
            throw new NotImplementedException();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            result.AddRange(Count.ToWoditorIntBytes());

            foreach (var tagId in this)
                result.Add(tagId);

            return result.ToArray();
        }
    }
}
