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
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// マップファイル読み込みクラス
    /// </summary>
    internal class MpsFileReader
    {
        /// <summary>読み込みファイルパス</summary>
        public MpsFilePath FilePath { get; }

        /// <summary>[Nullable] 読み込んだマップデータ</summary>
        public MapData MapData { get; private set; }

        private FileReadStatus ReadStatus { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MpsFileReader(MpsFilePath filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            FilePath = filePath;
            ReadStatus = new FileReadStatus(FilePath);
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
            if (!(MapData is null))
                throw new InvalidOperationException(
                    "すでに読み込み完了しています。");

            Logger.Info(FileIOMessage.StartFileRead(GetType()));

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

            Logger.Info(FileIOMessage.EndFileRead(GetType()));

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
        public async Task<MapData> ReadAsync()
        {
            return await Task.Run(ReadSync);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
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

            Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                "ヘッダ"));
        }

        /// <summary>
        /// ヘッダ文字列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private void ReadHeaderMemo(FileReadStatus status, MapData mapData)
        {
            var woditorString = status.ReadString();
            mapData.Memo = woditorString.String;
            status.AddOffset(woditorString.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "ヘッダ文字列", mapData.Memo));
        }

        /// <summary>
        /// タイルセットID
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private void ReadTileSetId(FileReadStatus status, MapData mapData)
        {
            mapData.TileSetId = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "タイルセットID", mapData.TileSetId));
        }

        /// <summary>
        /// マップサイズ横
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private void ReadMapSizeWidth(FileReadStatus status, MapData mapData)
        {
            mapData.UpdateMapSizeWidth(status.ReadInt());
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップサイズ横", mapData.MapSizeWidth));
        }

        /// <summary>
        /// マップサイズ縦
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private void ReadMapSizeHeight(FileReadStatus status, MapData mapData)
        {
            mapData.UpdateMapSizeHeight(status.ReadInt());
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップサイズ縦", mapData.MapSizeHeight));
        }

        /// <summary>
        /// マップイベント数
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        private int ReadMapEventLength(FileReadStatus status)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベント数", length));

            return length;
        }

        /// <summary>
        /// レイヤー1～3
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        private void ReadLayer(FileReadStatus status, MapData mapData)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(MpsFileReader),
                "レイヤー"));

            for (var layerIndex = 0; layerIndex < 3; layerIndex++)
            {
                Logger.Debug(FileIOMessage.StartCommonRead(typeof(MpsFileReader),
                    $"レイヤー{layerIndex}"));

                ReadOneLayer(status, mapData, layerIndex);

                Logger.Debug(FileIOMessage.EndCommonRead(typeof(MpsFileReader),
                    $"レイヤー{layerIndex}"));
            }

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(MpsFileReader),
                "レイヤー"));
        }

        /// <summary>
        /// 1レイヤー情報
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapData">データ格納マップデータインスタンス</param>
        /// <param name="layerNo">レイヤー番号</param>
        private void ReadOneLayer(FileReadStatus status, MapData mapData, int layerNo)
        {
            var chips = new List<List<MapChip>>();
            for (var x = 0; x < (int) mapData.MapSizeWidth; x++)
            {
                Logger.Debug(FileIOMessage.StartCommonRead(typeof(MpsFileReader),
                    $"列{x}"));

                ReadLayerOneLine(status, mapData.MapSizeHeight, chips);

                Logger.Debug(FileIOMessage.EndCommonRead(typeof(MpsFileReader),
                    "列{x}"));
            }

            var layer = new Layer
            {
                Chips = new MapChipList(chips)
            };
            mapData.SetLayer(layerNo, layer);
        }

        /// <summary>
        /// 1レイヤー列情報
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapSizeHeight">マップ高さ</param>
        /// <param name="chipList">格納先リスト</param>
        private void ReadLayerOneLine(FileReadStatus status, MapSizeHeight mapSizeHeight,
            ICollection<List<MapChip>> chipList)
        {
            var lineChips = new List<MapChip>();
            for (var y = 0; y < (int) mapSizeHeight; y++)
            {
                var chip = (MapChip) status.ReadInt();
                lineChips.Add(chip);
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    $"座標Y:{y} チップ番号", (int) chip));
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
        private void ReadMapEvent(int size, FileReadStatus status, MapData mapData)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(MpsFileReader),
                "マップイベント"));

            var mapEvents = new List<MapEvent>();
            var count = 0;
            while (true)
            {
                Logger.Debug(FileIOMessage.StartCommonRead(typeof(MpsFileReader),
                    $"マップイベント{count}"));

                // ヘッダチェック
                var validatedHeader = status.ReadByte() == MapEvent.Header[0];
                if (!validatedHeader) break;


                Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                    "ヘッダ"));

                // ヘッダ分オフセット加算
                status.AddOffset(MapEvent.Header.Length);

                var mapEvent = new MapEvent();

                // マップイベントID
                mapEvent.MapEventId = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    "マップイベントID", mapEvent.MapEventId));

                // イベント名
                var woditorString = status.ReadString();
                mapEvent.EventName = woditorString.String;
                status.AddOffset(woditorString.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    "イベント名", mapEvent.EventName));

                // X座標
                var posX = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    "X座標", posX));

                // Y座標
                var posY = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    "Y座標", posY));

                mapEvent.Position = (posX, posY);

                // イベントページ数
                var pageLength = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    "イベントページ数", pageLength));

                // 0パディングチェック
                var padding = status.ReadInt();
                status.IncreaseIntOffset();
                var isCorrectPadding = padding == 0;
                if (!isCorrectPadding)
                {
                    throw new InvalidOperationException(
                        $"マップイベントのパディングが異なります。（offset:{status.Offset}）");
                }

                Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                    "0パディング"));

                // マップイベントページ
                var mapEventPageList = new List<MapEventPage>();
                for (var i = 0; i < pageLength; i++)
                {
                    Logger.Debug(FileIOMessage.StartCommonRead(typeof(MpsFileReader),
                        $"マップイベントページ{i}"));

                    ReadMapEventOnePage(status, mapEventPageList);

                    Logger.Debug(FileIOMessage.EndCommonRead(typeof(MpsFileReader),
                        $"マップイベントページ{i}"));
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

                Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                    "イベントページ末尾"));

                mapEvents.Add(mapEvent);

                Logger.Debug(FileIOMessage.EndCommonRead(typeof(MpsFileReader),
                    $"マップイベント{count}"));

                count++;
            }

            if (count != size)
                throw new InvalidOperationException(
                    $"マップイベントデータの数が期待する数と異なります。(期待する数：{size}, 実際のイベント数：{count})");

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(MpsFileReader),
                "マップイベント"));

            mapData.MapEvents = new MapEventList(mapEvents);
        }

        /// <summary>
        /// マップイベントページ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="mapEventPages">格納先リスト</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private void ReadMapEventOnePage(FileReadStatus status, ICollection<MapEventPage> mapEventPages)
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

            Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                "マップイベントページヘッダ"));

            var graphicInfo = new MapEventPageGraphicInfo();

            // タイル画像ID
            var graphicTileId = (MapEventTileId) status.ReadInt();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページタイル画像ID", graphicTileId));

            if (graphicTileId != MapEventTileId.NotUse)
            {
                graphicInfo.IsGraphicTileChip = true;
                graphicInfo.GraphicTileId = graphicTileId;
            }

            status.IncreaseIntOffset();

            // キャラチップ名
            var charaChipString = status.ReadString();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページキャラチップ名", charaChipString.String));

            if (!graphicInfo.IsGraphicTileChip)
            {
                graphicInfo.CharaChipFilePath = charaChipString.String;
            }

            status.AddOffset(charaChipString.ByteLength);

            // 初期キャラ向き
            var initDirection = status.ReadByte();
            graphicInfo.InitDirection = CharaChipDirection.FromByte(initDirection);
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ初期キャラ向き", graphicInfo.InitDirection));

            // 初期アニメーション番号
            graphicInfo.InitAnimationId = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ初期アニメーション番号", graphicInfo.InitAnimationId));

            // キャラチップ透過度
            graphicInfo.CharaChipOpacity = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページキャラチップ透過度", graphicInfo.CharaChipOpacity));

            // キャラチップ表示形式
            graphicInfo.CharaChipDrawType = PictureDrawType.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページキャラチップ表示形式", graphicInfo.CharaChipDrawType));

            result.GraphicInfo = graphicInfo;

            var bootInfo = new MapEventPageBootInfo();

            // 起動条件
            bootInfo.MapEventBootType = MapEventBootType.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ起動条件", bootInfo.MapEventBootType));

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

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    $"マップイベントページ起動条件{i}演算子", conditions[i].Operation));
                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    $"マップイベントページ起動条件{i}使用フラグ", conditions[i].UseCondition));
            }

            // 条件1～4左辺
            for (var i = 0; i < 4; i++)
            {
                conditions[i].LeftSide = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    $"マップイベントページ起動条件{i}左辺", conditions[i].LeftSide));
            }

            // 条件1～4右辺
            for (var i = 0; i < 4; i++)
            {
                conditions[i].RightSide = status.ReadInt();
                status.IncreaseIntOffset();
                bootInfo.SetEventBootCondition(i, conditions[i]);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                    $"マップイベントページ起動条件{i}右辺", conditions[i].RightSide));
            }

            result.BootInfo = bootInfo;

            var moveRouteInfo = new MapEventPageMoveRouteInfo();

            // アニメ速度
            moveRouteInfo.AnimateSpeed = AnimateSpeed.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページアニメ速度", moveRouteInfo.AnimateSpeed));

            // 移動速度
            moveRouteInfo.MoveSpeed = MoveSpeed.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ移動速度", moveRouteInfo.MoveSpeed));

            // 移動頻度
            moveRouteInfo.MoveFrequency = MoveFrequency.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ移動頻度", moveRouteInfo.MoveFrequency));

            // 移動ルート
            moveRouteInfo.MoveType = MoveType.FromByte(status.ReadByte());
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ移動ルート種別", moveRouteInfo.MoveType));

            var option = new MapEventPageOption();

            // オプション
            var optionByte = status.ReadByte();
            option.SetOptionFlag(optionByte);
            status.IncreaseByteOffset();

            result.Option = option;

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページオプション", optionByte));

            // カスタム移動ルートフラグ
            var actionEntry = new ActionEntry();
            var customMoveRouteFlag = status.ReadByte();
            actionEntry.SetOptionFlag(customMoveRouteFlag);
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページカスタム移動ルートフラグ", customMoveRouteFlag));

            // 動作指定コマンド数
            actionEntry.CommandList = ReadCharaMoveCommand(status);

            moveRouteInfo.CustomMoveRoute = actionEntry;
            result.MoveRouteInfo = moveRouteInfo;

            // イベント行数
            var eventLength = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページイベント行数", eventLength));

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

            Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                "マップイベントページイベントコマンド終端"));

            // 影グラフィック番号
            result.ShadowGraphicId = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ影グラフィック番号", result.ShadowGraphicId));

            // 接触範囲拡張X
            var rangeWidth = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ接触範囲拡張X", rangeWidth));

            // 接触範囲拡張Y
            var rangeHeight = status.ReadByte();
            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MpsFileReader),
                "マップイベントページ接触範囲拡張Y", rangeHeight));

            result.HitExtendRange = (rangeWidth, rangeHeight);

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

            Logger.Debug(FileIOMessage.CheckOk(typeof(MpsFileReader),
                "マップイベントページ末尾チェック"));

            // 完了
            mapEventPages.Add(result);
        }

        /// <summary>
        /// 動作コマンドリスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private CharaMoveCommandList ReadCharaMoveCommand(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(EventCommandListReader),
                "マップイベントページ動作コマンドリスト"));

            // 動作コマンド数
            var commandLength = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(EventCommandListReader),
                "マップイベントページ動作コマンド数", commandLength));

            // 動作指定コマンド
            var reader = new CharaMoveCommandListReader(status, commandLength);
            var result = reader.Read();

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(EventCommandListReader),
                "マップイベントページ動作コマンドリスト"));

            return new CharaMoveCommandList(result);
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        private void ReadFooter(FileReadStatus status)
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

            Logger.Debug(FileIOMessage.CheckOk(typeof(EventCommandListReader),
                "フッタ"));
        }
    }
}