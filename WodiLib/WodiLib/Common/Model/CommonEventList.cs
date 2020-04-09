// ========================================
// Project Name : WodiLib
// File Name    : CommonEventList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベントリスト
    /// </summary>
    [Serializable]
    public class CommonEventList : RestrictedCapacityCollection<CommonEvent>,
        IReadOnlyCommonEventList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト最大数</summary>
        public static int MaxCapacity => 10000;

        /// <summary>リスト最小数</summary>
        public static int MinCapacity => 1;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventList()
        {
        }

        /// <summary>
        /// コンストラクタ（初期配列指定）
        /// </summary>
        /// <param name="items">[NotNull] 初期リスト</param>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="InvalidOperationException">
        ///     itemsの要素数がMinLength未満の場合、
        ///     またはitemsの要素数がMaxLengthを超える場合
        /// </exception>
        public CommonEventList(IEnumerable<CommonEvent> items) : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 容量最大値を返す。
        /// </summary>
        /// <returns>容量最大値</returns>
        public override int GetMaxCapacity() => MaxCapacity;

        /// <inheritdoc />
        /// <summary>
        /// 容量最小値を返す。
        /// </summary>
        /// <returns>容量最小値</returns>
        public override int GetMinCapacity() => MinCapacity;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 指定したコモンイベントのイベントコードリストを取得する。
        /// </summary>
        /// <returns>イベントコードリスト</returns>
        public IReadOnlyList<string> GetEventCodeStringList(CommonEventId commonEventId)
        {
            var max = Count;
            const int min = 0;
            if (commonEventId < min || max < commonEventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(commonEventId), min, max, commonEventId));

            var targetCommonEvent = this[commonEventId];

            return targetCommonEvent.GetEventCodeStringList();
        }

        /// <summary>
        /// 指定したコモンイベントのイベントコマンド文字列情報を返す。
        /// </summary>
        /// <param name="commonEventId">[Range(0, Count - 1)] コモンイベントID</param>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列情報リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">commonEventIdが指定範囲外の場合</exception>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        public IReadOnlyList<EventCommandSentenceInfo> GetCommonEventEventCommandSentenceInfoList(
            CommonEventId commonEventId, EventCommandSentenceResolver resolver,
            EventCommandSentenceResolveDesc desc)
        {
            var max = Count;
            const int min = 0;
            if (commonEventId < min || max < commonEventId)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(commonEventId), min, max, commonEventId));

            var targetCommonEvent = this[commonEventId];

            return targetCommonEvent.MakeEventCommandSentenceInfoList(resolver, desc);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override CommonEvent MakeDefaultItem(int index) => new CommonEvent();

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

            // コモンイベント数
            result.AddRange(Count.ToBytes(Endian.Woditor));

            // コモンイベントリスト
            foreach (var commonEvent in Items)
            {
                result.AddRange(commonEvent.ToBinary());
            }

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Serializable
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected CommonEventList(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}