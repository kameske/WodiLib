// ========================================
// Project Name : WodiLib
// File Name    : MapEventPage.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Event.EventCommand;
using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    ///     マップイベント1ページ毎の情報実装クラス
    /// </summary>
    public class MapEventPage : IWodiLibObject
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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(GraphicInfo)));
                graphicInfo = value;
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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(BootInfo)));
                bootInfo = value;
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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(MoveRouteInfo)));
                moveRouteInfo = value;
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
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(Option)));
                option = value;
            }
        }

        /// <summary>接触範囲拡張X</summary>
        public byte RangeWidth { get; set; }

        /// <summary>接触範囲拡張Y</summary>
        public byte RangeHeight { get; set; }

        /// <summary>影グラフィック番号</summary>
        public byte ShadowGraphicId { get; set; }

        private EventCommandList eventCommands = new EventCommandList(new[] {new Blank()});

        /// <summary>[NotNull] イベントコマンド</summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public EventCommandList EventCommands
        {
            get => eventCommands;
            set
            {
                if (value == null)
                    throw new PropertyNullException(
                        ErrorMessage.NotNull(nameof(EventCommands)));
                eventCommands = value;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
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
            result.Add(RangeWidth);
            // 接触範囲拡張Y
            result.Add(RangeHeight);
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