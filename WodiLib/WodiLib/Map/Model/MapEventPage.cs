// ========================================
// Project Name : WodiLib
// File Name    : MapEventPage.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using WodiLib.Event.EventCommand;
using WodiLib.Project;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント1ページ毎の情報実装クラス
    /// </summary>
    [Serializable]
    public class MapEventPage : ModelBase<MapEventPage>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ヘッダ</summary>
        public static readonly byte[] Header = {0x79};

        /// <summary>フッタ</summary>
        public static readonly byte[] Footer = {0x7A};

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private MapEventPageGraphicInfo graphicInfo = new MapEventPageGraphicInfo();

        /// <summary>[NotNull] 画像情報</summary>
        /// <exception cref="PropertyNullException">nullをセットしようとした場合</exception>
        public MapEventPageGraphicInfo GraphicInfo
        {
            get => graphicInfo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(GraphicInfo)));
                graphicInfo = value;
                NotifyPropertyChanged();
            }
        }

        private MapEventPageBootInfo bootInfo = new MapEventPageBootInfo();

        /// <summary>[NotNull] 起動条件</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEventPageBootInfo BootInfo
        {
            get => bootInfo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(BootInfo)));
                bootInfo = value;
                NotifyPropertyChanged();
            }
        }

        private MapEventPageMoveRouteInfo moveRouteInfo = new MapEventPageMoveRouteInfo();

        /// <summary>[NotNull] 移動ルート</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEventPageMoveRouteInfo MoveRouteInfo
        {
            get => moveRouteInfo;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveRouteInfo)));
                moveRouteInfo = value;
                NotifyPropertyChanged();
            }
        }

        private MapEventPageOption option = new MapEventPageOption();

        /// <summary>[NotNull] オプション</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public MapEventPageOption Option
        {
            get => option;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Option)));
                option = value;
                NotifyPropertyChanged();
            }
        }

        private HitExtendRange hitExtendRange;

        /// <summary>接触範囲拡張</summary>
        public HitExtendRange HitExtendRange
        {
            get => hitExtendRange;
            set
            {
                hitExtendRange = value;
                NotifyPropertyChanged();
            }
        }

        private ShadowGraphicId shadowGraphicId;

        /// <summary>影グラフィック番号</summary>
        public ShadowGraphicId ShadowGraphicId
        {
            get => shadowGraphicId;
            set
            {
                shadowGraphicId = value;
                NotifyPropertyChanged();
            }
        }

        private EventCommandList eventCommands = new EventCommandList(new[] {new Blank()});

        /// <summary>[NotNull] イベントコマンド</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandList EventCommands
        {
            get => eventCommands;
            set
            {
                if (value is null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventCommands)));
                eventCommands = value;
                NotifyPropertyChanged();
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// イベントコマンド文字列情報リストを取得する。
        /// </summary>
        /// <param name="resolver">[NotNull] 名前解決クラスインスタンス</param>
        /// <param name="desc">[Nullable] 付加情報</param>
        /// <returns>イベントコマンド文字列</returns>
        /// <exception cref="ArgumentNullException">
        ///     resolver または type が null の場合、
        ///     または必要なときに desc が null の場合
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyList<EventCommandSentenceInfo> MakeEventCommandSentenceInfoList(
            EventCommandSentenceResolver resolver, EventCommandSentenceResolveDesc desc)
        {
            var sentenceType = EventCommandSentenceType.Map;

            return EventCommands.MakeEventCommandSentenceInfoList(resolver, sentenceType, desc);
        }

        /// <summary>
        /// 値を比較する。
        /// </summary>
        /// <param name="other">比較対象</param>
        /// <returns>一致する場合、true</returns>
        public override bool Equals(MapEventPage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return graphicInfo.Equals(other.graphicInfo)
                   && bootInfo.Equals(other.bootInfo)
                   && moveRouteInfo.Equals(other.moveRouteInfo)
                   && option.Equals(other.option)
                   && eventCommands.Equals(other.eventCommands)
                   && HitExtendRange.Equals(other.HitExtendRange)
                   && ShadowGraphicId.Equals(other.ShadowGraphicId);
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
            ValidateToBinary();

            var result = new List<byte>();
            // ヘッダ
            result.AddRange(Header);
            // キャラ画像情報
            result.AddRange(graphicInfo.ToBinary());
            // イベント起動条件
            result.AddRange(BootInfo.ToBinary());
            // 移動ルート
            result.AddRange(MoveRouteInfo.ToBinary());
            // オプション
            result.AddRange(Option.ToBinary());
            // 移動ルート詳細
            result.AddRange(MoveRouteInfo.ToCustomMoveRouteBinary());
            // イベントコマンド
            result.AddRange(EventCommands.ToBinary());
            // 影グラフィック番号
            result.Add(ShadowGraphicId);
            // 接触範囲拡張X
            result.Add(HitExtendRange.Width);
            // 接触範囲拡張Y
            result.Add(HitExtendRange.Height);
            // イベントページ終端
            result.AddRange(Footer);

            return result.ToArray();
        }

        private void ValidateToBinary()
        {
            MoveRouteInfo.ValidateMoveType();
        }
    }
}