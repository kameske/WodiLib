// ========================================
// Project Name : WodiLib
// File Name    : MpsFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WodiLib.Event;
using WodiLib.Event.EventCommand;
using WodiLib.Map;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// マップファイル読み込みクラス
    /// </summary>
    internal class MpsFileReader
    {
        /// <summary>読み込みファイルパス</summary>
        public string FilePath { get; }

        /// <summary>[Nullable] 読み込んだマップデータ</summary>
        public MapData MapData { get; private set; }

        private FileReadStatus ReadStatus { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MpsFileReader(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if(filePath.IsEmpty()) throw new ArgumentException(
                ErrorMessage.NotEmpty(nameof(filePath)));
            FilePath = filePath;
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public MapData ReadSync()
        {
            if (MapData != null) throw new InvalidOperationException(
                $"すでに読み込み完了しています。");

            ReadStatus = new FileReadStatus(FilePath);
            MapData = new MapData();

            // ヘッダチェック
            ReadHeader(ReadStatus);

            // ヘッダ文字列
            ReadHeaderMemo(ReadStatus, MapData);

            // タイルセットID
            ReadTileSetId(ReadStatus, MapData);

            // マップサイズ横
            ReadMapSizeWidth(ReadStatus, MapData);

            // マップサイズ縦
            ReadMapSizeHeight(ReadStatus, MapData);

            // マップイベント数
            var mapEventLength = ReadMapEventLength(ReadStatus);

            // レイヤー1～3
            ReadLayer(ReadStatus, MapData);

            // マップイベント
            ReadMapEvent(mapEventLength, ReadStatus, MapData);

            // ファイル末尾
            ReadFooter(ReadStatus);

            return MapData;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public async Task ReadAsync()
        {
            await Task.Run(() => ReadSync());
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private static void ReadHeader(FileReadStatus status)
        {
            foreach (var b in MapData.HeaderBytes)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }
                status.IncreaseByteOffset();
            }
        }

        /// <summary>
        /// ヘッダ文字列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private static void ReadHeaderMemo(FileReadStatus status, MapData mapData)
        {
            var woditorString = status.ReadString();
            mapData.Memo = woditorString.String;
            status.AddOffset(woditorString.ByteLength);
        }

        /// <summary>
        /// タイルセットID
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private static void ReadTileSetId(FileReadStatus status, MapData mapData)
        {
            mapData.TileSetId = status.ReadInt();
            status.IncreaseIntOffset();
        }

        /// <summary>
        /// マップサイズ横
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private static void ReadMapSizeWidth(FileReadStatus status, MapData mapData)
        {
            mapData.MapSizeWidth = status.ReadInt();
            status.IncreaseIntOffset();
        }

        /// <summary>
        /// マップサイズ縦
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private static void ReadMapSizeHeight(FileReadStatus status, MapData mapData)
        {
            mapData.MapSizeHeight = status.ReadInt();
            status.IncreaseIntOffset();
        }

        /// <summary>
        /// マップイベント数
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        private static int ReadMapEventLength(FileReadStatus status)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();
            return length;
        }

        /// <summary>
        /// レイヤー1～3
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private static void ReadLayer(FileReadStatus status, MapData mapData)
        {
            for (var layerIndex = 0; layerIndex < 3; layerIndex++)
            {
                ReadOneLayer(status, mapData, layerIndex);
            }
        }

        /// <summary>
        /// 1レイヤー情報
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        /// <param name="layerNo">レイヤー番号</param>
        private static void ReadOneLayer(FileReadStatus status, MapData mapData, int layerNo)
        {
            var chips = new List<IEnumerable<MapChip>>();
            for (var x = 0; x < mapData.MapSizeWidth; x++)
            {
                ReadLayerOneLine(status, mapData.MapSizeHeight, chips);
            }

            var layer = new Layer();
            layer.SetChips(chips);
            mapData.SetLayer(layerNo, layer);
        }

        /// <summary>
        /// 1レイヤー列情報
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapSizeHeight">マップ高さ</param>
        /// <param name="chipList">格納先リスト</param>
        private static void ReadLayerOneLine(FileReadStatus status, int mapSizeHeight, ICollection<IEnumerable<MapChip>> chipList)
        {
            var lineChips = new List<MapChip>();
            for (var y = 0; y < mapSizeHeight; y++)
            {
                var chip = (MapChip) status.ReadInt();
                lineChips.Add(chip);
                status.IncreaseIntOffset();
            }

            chipList.Add(lineChips);
        }

        /// <summary>
        /// マップイベント
        /// </summary>
        /// <param name="size">マップイベント数</param>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadMapEvent(int size, FileReadStatus status, MapData mapData)
        {
            var mapEvents = new List<MapEvent>();
            var count = 0;
            while (true)
            {
                // ヘッダチェック
                var validatedHeader = status.ReadByte() == MapEvent.Header[0];
                if (!validatedHeader) break;

                // ヘッダ分オフセット加算
                status.AddOffset(MapEvent.Header.Length);

                var mapEvent = new MapEvent();

                // マップイベントID
                mapEvent.MapEventId = status.ReadInt();
                status.IncreaseIntOffset();

                // イベント名
                var woditorString = status.ReadString();
                mapEvent.EventName = woditorString.String;
                status.AddOffset(woditorString.ByteLength);

                // X座標
                mapEvent.PositionX = status.ReadInt();
                status.IncreaseIntOffset();

                // Y座標
                mapEvent.PositionY = status.ReadInt();
                status.IncreaseIntOffset();

                // イベントページ数
                var pageLength = status.ReadInt();
                status.IncreaseIntOffset();

                // 0パディングチェック
                var padding = status.ReadInt();
                status.IncreaseIntOffset();
                var isCorrectPadding = padding == 0;
                if (!isCorrectPadding)
                {
                    throw new InvalidOperationException(
                        $"マップイベントのパディングが異なります。（offset:{status.Offset}）");
                }

                // マップイベントページ
                var mapEventPageList = new List<MapEventPage>();
                for (var i = 0; i < pageLength; i++)
                {
                    ReadMapEventOnePage(status, mapEventPageList);
                }

                mapEvent.MapEventPageList = new MapEventPageList(mapEventPageList);

                // イベントページ末尾チェック
                foreach (var b in MapEvent.Footer)
                {
                    if (status.ReadByte() != b)
                    {
                        throw new InvalidOperationException(
                            $"マップイベント末尾の値が異なります。（offset: {status.Offset}）");
                    }

                    status.IncreaseByteOffset();
                }

                mapEvents.Add(mapEvent);
                count++;
            }

            if (count != size)
                throw new InvalidOperationException(
                    $"マップイベントデータの数が期待する数と異なります。(期待する数：{size}, 実際のイベント数：{count})");

            mapData.MapEvents = new MapEventList(mapEvents);
        }

        /// <summary>
        /// マップイベントページ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapEventPages">格納先リスト</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadMapEventOnePage(FileReadStatus status, ICollection<MapEventPage> mapEventPages)
        {
            var result = new MapEventPage();

            // ヘッダチェック
            foreach (var b in MapEventPage.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"マップイベントページのヘッダが異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            var graphicInfo = new MapEventPageGraphicInfo();

            // タイル画像ID
            var graphicTileId = status.ReadInt();
            if (graphicTileId != MapEventPageGraphicInfo.GraphicNotUseTileId)
            {
                graphicInfo.IsGraphicTileChip = true;
                graphicInfo.GraphicTileId = graphicTileId;
            }

            status.IncreaseIntOffset();

            // キャラチップ名
            var charaChipString = status.ReadString();
            if (!graphicInfo.IsGraphicTileChip)
            {
                graphicInfo.CharaChipFileName = charaChipString.String;
            }

            status.AddOffset(charaChipString.ByteLength);

            // 初期キャラ向き
            graphicInfo.InitDirection = status.ReadByte();
            status.IncreaseByteOffset();

            // 初期アニメーション番号
            graphicInfo.InitAnimationId = status.ReadByte();
            status.IncreaseByteOffset();

            // キャラチップ透過度
            graphicInfo.CharaChipOpacity = status.ReadByte();
            status.IncreaseByteOffset();

            // キャラチップ表示形式
            graphicInfo.CharaChipDrawType = PictureDrawType.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            result.GraphicInfo = graphicInfo;

            var bootInfo = new MapEventPageBootInfo();

            // 起動条件
            bootInfo.MapEventBootType = MapEventBootType.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            // 条件1～4演算子 & 使用フラグ
            var conditions = new List<MapEventBootCondition>
            {
                new MapEventBootCondition(),
                new MapEventBootCondition(),
                new MapEventBootCondition(),
                new MapEventBootCondition(),
            };
            for (var i = 0; i < 4; i++)
            {
                conditions[i].Operation = CriteriaOperator.FromByte((byte) (status.ReadByte() & 0xF0));
                conditions[i].UseCondition = (byte) (status.ReadByte() & 0x0F) != 0;
                status.IncreaseByteOffset();
            }

            // 条件1～4左辺
            for (var i = 0; i < 4; i++)
            {
                conditions[i].LeftSide = status.ReadInt();
                status.IncreaseIntOffset();
            }

            // 条件1～4右辺
            for (var i = 0; i < 4; i++)
            {
                conditions[i].RightSide = status.ReadInt();
                status.IncreaseIntOffset();
                bootInfo.SetEventBootCondition(i, conditions[i]);
            }

            result.BootInfo = bootInfo;

            var moveRouteInfo = new MapEventPageMoveRouteInfo();

            // アニメ速度
            moveRouteInfo.AnimateSpeed = AnimateSpeed.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            // 移動速度
            moveRouteInfo.MoveSpeed = MoveSpeed.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            // 移動頻度
            moveRouteInfo.MoveFrequency = MoveFrequency.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            // 移動ルート
            moveRouteInfo.MoveType = MoveType.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            var option = new MapEventPageOption();

            // オプション
            option.SetOptionFlag(status.ReadByte());
            status.IncreaseByteOffset();

            result.Option = option;

            // カスタム移動ルートフラグ
            var actionEntry = new ActionEntry();
            actionEntry.SetOptionFlag(status.ReadByte());
            status.IncreaseByteOffset();

            // 動作指定コマンド数
            var eventCommandLength = status.ReadInt();
            status.IncreaseIntOffset();

            // 動作指定コマンド
            var charaMoveCommandList = new List<ICharaMoveCommand>();
            for (var i = 0; i < eventCommandLength; i++)
            {
                ReadCharaMoveCommand(status, charaMoveCommandList);
            }

            actionEntry.CommandList = charaMoveCommandList;

            moveRouteInfo.CustomMoveRoute = actionEntry;
            result.MoveRouteInfo = moveRouteInfo;

            // イベント行数
            var eventLength = status.ReadInt();
            status.IncreaseIntOffset();

            // イベントコマンド
            var eventCommandListReader = new EventCommandListReader(status, eventLength);

            result.EventCommands = eventCommandListReader.Read();

            // イベントコマンド終端チェック
            foreach (var b in EventCommandList.EndEventCommand)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"イベントコマンド後の値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            // 影グラフィック番号
            result.ShadowGraphicId = status.ReadByte();
            status.IncreaseByteOffset();

            // 接触範囲拡張X
            result.RangeWidth = status.ReadByte();
            status.IncreaseByteOffset();

            // 接触範囲拡張Y
            result.RangeHeight = status.ReadByte();
            status.IncreaseByteOffset();

            // イベントページ末尾チェック
            foreach (var b in MapEventPage.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"イベントページ末尾の値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            // 完了
            mapEventPages.Add(result);
        }

        /// <summary>
        /// 動作指定コマンド
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="commandList">データ格納先</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadCharaMoveCommand(FileReadStatus status, ICollection<ICharaMoveCommand> commandList)
        {
            // 動作指定コード
            var charaMoveCode = status.ReadByte();
            var charaMoveCommand = CharaMoveCommandFactory.CreateRaw(charaMoveCode);
            status.IncreaseByteOffset();

            // 変数の数
            var varLength = status.ReadByte();
            status.IncreaseByteOffset();

            // 変数
            for (var i = 0; i < varLength; i++)
            {
                var variable = status.ReadInt();
                charaMoveCommand.SetNumberValue(i, variable);
                status.IncreaseIntOffset();
            }

            // 終端コードチェック
            foreach (var b in Event.CharaMoveCommand.CharaMoveCommandBase.EndBlockCode)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"動作指定コマンド末尾の値が異なります。（offset: {status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            // 結果
            commandList.Add(charaMoveCommand);
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private static void ReadFooter(FileReadStatus status)
        {
            foreach (var b in MapData.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"フッタが正常に取得できませんでした。（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }
        }
    }
}