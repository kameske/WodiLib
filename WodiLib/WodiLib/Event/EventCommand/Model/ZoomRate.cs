// ========================================
// Project Name : WodiLib
// File Name    : ZoomRate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 拡大率
    /// </summary>
    [Serializable]
    public class ZoomRate : IEquatable<ZoomRate>, ISerializable
    {
        /// <summary>パターン同値フラグ値</summary>
        private static readonly int SameValue = new byte[] {0xC0, 0xBD, 0xF0, 0xFF}.ToInt32(Endian.Environment);

        private static readonly string ErrorMessage_Difference = $"縦横別々フラグ=trueのためアクセスできません。";
        private static readonly string ErrorMessage_NotDifference = $"縦横別々フラグ=falseのためアクセスできません。";

        private const string EventCommandSentenceFormatSame = "同値";
        private const string EventCommandSentenceFormatDifference = "{0}x{1}％";
        private const string EventCommandSentenceFormatEtc = "{0}％";

        private ZoomRateType zoomRateType = ZoomRateType.Normal;

        /// <summary>
        /// [NotNull] 拡大率種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public ZoomRateType ZoomRateType
        {
            get => zoomRateType;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(ZoomRateType)));
                zoomRateType = value;
            }
        }

        /// <summary>縦横別々フラグ</summary>
        public bool IsDifference => ZoomRateType == ZoomRateType.Different
                                    || ZoomRateType == ZoomRateType.OnlyDepth
                                    || ZoomRateType == ZoomRateType.OnlyWidth;

        /// <summary>同値フラグ
        /// <para>Rateプロパティに影響する</para></summary>
        public bool IsSame => ZoomRateType == ZoomRateType.Same;

        /// <summary>拡大率
        /// <para>変更すると同値フラグも変化する</para></summary>
        /// <exception cref="PropertyAccessException">縦横別々フラグ＝trueのときにアクセスした場合</exception>
        public int Rate
        {
            get
            {
                if (IsDifference) throw new PropertyAccessException(ErrorMessage_Difference);
                if (IsSame) return SameValue;
                return rates[0];
            }
            set
            {
                if (IsDifference) throw new PropertyAccessException(ErrorMessage_Difference);
                rates[0] = value;
            }
        }

        /// <summary>拡大率横</summary>
        /// <exception cref="PropertyAccessException">縦横別々フラグ＝falseのときにアクセスした場合</exception>
        public int RateWidth
        {
            get
            {
                if (!IsDifference) throw new PropertyAccessException(ErrorMessage_NotDifference);
                return rates[0];
            }
            set
            {
                if (!IsDifference) throw new PropertyAccessException(ErrorMessage_NotDifference);
                rates[0] = value;
            }
        }

        /// <summary>拡大率縦</summary>
        /// <exception cref="PropertyAccessException">縦横別々フラグ＝falseのときにアクセスした場合</exception>
        public int RateHeight
        {
            get
            {
                if (!IsDifference) throw new PropertyAccessException(ErrorMessage_NotDifference);
                return rates[1];
            }
            set
            {
                if (!IsDifference) throw new PropertyAccessException(ErrorMessage_NotDifference);
                rates[1] = value;
            }
        }

        /// <summary>
        /// 拡大率にデフォルト値が設定されているかを返す。
        /// </summary>
        public bool IsDefaultRate
        {
            get
            {
                if (IsDifference)
                {
                    return rates[0] == DefaultRate && rates[1] == DefaultRate;
                }

                return rates[1] == DefaultRate;
            }
        }

        private readonly int[] rates = {DefaultRate, DefaultRate};

        private static readonly int DefaultRate = 100;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ZoomRate()
        {
        }

        /// <summary>
        /// イベントコマンド文字列を取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="type">[NotNull] イベント種別</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        public string GetEventCommandSentence(EventCommandSentenceResolver resolver, EventCommandSentenceType type,
            EventCommandSentenceResolveDesc desc)
        {
            if (IsSame) return EventCommandSentenceFormatSame;

            if (IsDifference)
            {
                var widthStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                    RateWidth, type, desc);
                var heightStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                    RateHeight, type, desc);

                return string.Format(EventCommandSentenceFormatDifference,
                    widthStr, heightStr);
            }

            var rateStr = resolver.GetNumericVariableAddressStringIfVariableAddress(
                Rate, type, desc);

            return string.Format(EventCommandSentenceFormatEtc, rateStr);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public bool Equals(ZoomRate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (zoomRateType != other.zoomRateType) return false;

            if (!IsDifference) return Rate == other.Rate;

            return RateWidth == other.RateWidth
                   && RateHeight == other.RateHeight;
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
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ZoomRateType), ZoomRateType.Code);
            info.AddValue(nameof(Rate), Rate);
            info.AddValue(nameof(rates), rates);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="info">デシリアライズ情報</param>
        /// <param name="context">コンテキスト</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ZoomRate(SerializationInfo info, StreamingContext context)
        {
            ZoomRateType = ZoomRateType.FromByte(info.GetByte(nameof(ZoomRateType)));
            Rate = info.GetInt32(nameof(Rate));
            rates = info.GetValue<int[]>(nameof(rates));
        }
    }
}