// ========================================
// Project Name : WodiLib
// File Name    : MapTreeOpenStatusList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// マップツリー開閉状態リストクラス
    /// </summary>
    [Serializable]
    public class MapTreeOpenStatusList : RestrictedCapacityCollection<MapTreeOpenState>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static readonly int MaxCapacity = MapTreeNodeList.MaxCapacity;

        /// <summary>リスト最小数</summary>
        public static readonly int MinCapacity = MapTreeNodeList.MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapTreeOpenStatusList()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">初期リスト</param>
        /// <exception cref="TypeInitializationException">派生クラスの設定値が不正な場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     listがnullの場合、
        ///     またはlist中にnullが含まれる場合
        /// </exception>
        /// <exception cref="InvalidOperationException">listの要素数が不適切な場合</exception>
        public MapTreeOpenStatusList(IReadOnlyCollection<MapTreeOpenState> list) : base(list)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
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
        protected override MapTreeOpenState MakeDefaultItem(int index)
            => MapTreeOpenState.Open;

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

            foreach (var status in this)
                result.Add(status.Code);

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// オブジェクトをシリアル化するために必要なデータを設定する。
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(MakeSerializeKeyNameForItem(-1), Items.Count);
            for (var i = 0; i < Items.Count; i++)
            {
                info.AddValue(MakeSerializeKeyNameForItem(i), Items[i].Code);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected MapTreeOpenStatusList(SerializationInfo info, StreamingContext context) : base(
            ((Func<List<MapTreeOpenState>>) (() =>
            {
                var count = info.GetInt32(MakeSerializeKeyNameForItem(-1));
                var result = new List<MapTreeOpenState>();
                for (var i = 0; i < count; i++)
                {
                    result.Add(MapTreeOpenState.FromCode(info.GetByte(MakeSerializeKeyNameForItem(i))));
                }

                return result;
            }))())
        {
        }

        /// <summary>
        /// シリアライズ時のリスト要素用キー名を作成する。
        /// </summary>
        /// <param name="index">配列Index</param>
        /// <returns>キー名</returns>
        private static string MakeSerializeKeyNameForItem(int index) => $"ITEM_{index}";
    }
}