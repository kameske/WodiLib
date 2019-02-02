// ========================================
// Project Name : WodiLib
// File Name    : MapFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WodiLib.Event;
using WodiLib.Map;
using WodiLib.Sys;
using WodiLib.Sys.Observer;

namespace WodiLib.IO
{
    /// <summary>
    /// マップファイル読み込みクラス
    /// </summary>
    public class MapFileReader
    {
        /// <summary>読み込みファイルパス</summary>
        public string FilePath { get; }

        /// <summary>読み込み成否</summary>
        public bool IsSuccessRead { get; private set; }

        /// <summary>読み込んだマップデータ</summary>
        public MapData MapData { get; private set; }

        private readonly ErrorMessageInfo errorMessageInfo = new ErrorMessageInfo();

        /// <summary>エラーメッセージ（読み込み失敗時）</summary>
        public string ErrorMessage => errorMessageInfo.Message;

        /// <summary>読み込んだバイト配列</summary>
        private byte[] buf;

        /// <summary>読み込んだオフセット</summary>
        private long offset;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapFileReader(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(
                    Sys.ErrorMessage.NotNull(nameof(filePath)));
            FilePath = filePath;
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        public bool ReadSync()
        {
            if (MapData != null)
            {
                throw new InvalidOperationException($"すでに読み込みが完了しています");
            }

            IsSuccessRead = false;
            MapData = new MapData();

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"指定されたファイルが見つかりませんでした。 file: {FilePath}");
            var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            buf = new byte[stream.Length];
            stream.Read(buf, 0, buf.Length);

            offset = 0;

            // ヘッダチェック
            if (!ReadHeader()) return false;

            // ヘッダ文字列
            if (!ReadHeaderMemo()) return false;
            // タイルセットID
            if (!ReadTileSetId()) return false;
            // マップサイズ横
            if (!ReadMapSizeWidth()) return false;
            // マップサイズ縦
            if (!ReadMapSizeHeight()) return false;
            // マップイベント数
            if (!ReadMapEventLength(out var mapEventLength)) return false;
            // レイヤー1～3
            if (!ReadLayer()) return false;
            // マップイベント
            if (!ReadMapEvent(mapEventLength)) return false;
            // ファイル末尾
            if (!ReadFooter()) return false;

            IsSuccessRead = true;

            return true;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        public async Task<bool> ReadAsync()
        {
            return await Task.Run(() => ReadSync());
        }

        /// <summary>
        /// 読み込み処理のColdObservableを生成する。
        /// </summary>
        /// <returns>読み込み結果を通知するObservable</returns>
        public IObservable<bool> CreateObservable()
        {
            return WLObservable<bool>.Create(s =>
            {
                try
                {
                    var result = ReadSync();
                    s.OnNext(result);
                    s.OnCompleted();
                }
                catch (Exception ex)
                {
                    s.OnError(ex);
                }

                return EmptyDisposable.Instance;
            });
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadHeader()
        {
            try
            {
                foreach (var b in MapData.HeaderBytes)
                {
                    if (buf[offset] != b)
                    {
                        throw new InvalidOperationException($"ファイルヘッダがファイル仕様と異なります（offset:{offset}）");
                    }

                    offset++;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"ヘッダチェック中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// ヘッダ文字列
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadHeaderMemo()
        {
            try
            {
                var woditorString = new WoditorString(ref buf, offset);
                MapData.Memo = woditorString.String;
                offset += woditorString.ByteLength;
                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"ヘッダ文字列処理中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// タイルセットID
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadTileSetId()
        {
            try
            {
                MapData.TileSetId = buf.ToInt32(Endian.Woditor, offset);
                offset += 4;
                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"タイルセットID処理中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// マップサイズ横
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadMapSizeWidth()
        {
            try
            {
                MapData.MapSizeWidth = buf.ToInt32(Endian.Woditor, offset);
                offset += 4;
                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"マップサイズ横処理中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// マップサイズ縦
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadMapSizeHeight()
        {
            try
            {
                MapData.MapSizeHeight = buf.ToInt32(Endian.Woditor, offset);
                offset += 4;
                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"マップサイズ縦処理中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// 謎のパディング
        /// </summary>
        /// <param name="length">イベント数</param>
        /// <returns>読み込み成否</returns>
        private bool ReadMapEventLength(out int length)
        {
            try
            {
                length = buf.ToInt32(Endian.Woditor, offset);
                offset += 4;
                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"マップサイズ縦処理中にエラーが発生しました。（offset:{offset}）", ex);
                length = 0;
                return false;
            }
        }

        /// <summary>
        /// レイヤー1～3
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadLayer()
        {
            try
            {
                for (var layerIndex = 0; layerIndex < 3; layerIndex++)
                {
                    if (!ReadOneLayer(layerIndex)) return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"レイヤー処理中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// 1レイヤー分のデータ
        /// </summary>
        /// <param name="layerIndex">レイヤー番号</param>
        /// <returns>読み込み成否</returns>
        private bool ReadOneLayer(int layerIndex)
        {
            try
            {
                var chips = new List<List<MapChip>>();
                for (var x = 0; x < MapData.MapSizeWidth; x++)
                {
                    chips.Add(ReadLayerOneLine());
                }

                var layer = new Layer();
                layer.SetChips(chips);
                MapData.SetLayer(layerIndex, layer);
                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"レイヤー処理中にエラーが発生しました。（layerIndex: {layerIndex}, offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// 1行分のレイヤー情報
        /// </summary>
        /// <returns>読み込みデータ</returns>
        private List<MapChip> ReadLayerOneLine()
        {
            // 例外は上に投げる
            var lineChips = new List<MapChip>();
            for (var y = 0; y < MapData.MapSizeHeight; y++)
            {
                var chip = (MapChip) buf.ToInt32(Endian.Woditor, offset);
                lineChips.Add(chip);
                offset += 4;
            }

            return lineChips;
        }

        /// <summary>
        /// マップイベント
        /// </summary>
        /// <param name="size">マップイベント数</param>
        /// <returns>読み込み成否</returns>
        private bool ReadMapEvent(int size)
        {
            try
            {
                var mapEvents = new List<MapEvent>();
                var count = 0;
                while (true)
                {
                    // ヘッダチェック
                    if (buf[offset] != MapEvent.Header[0]) break;

                    var mapEvent = new MapEvent();

                    // ヘッダ
                    offset += MapEvent.Header.Length;

                    // マップイベントID
                    mapEvent.MapEventId = buf.ToInt32(Endian.Woditor, offset);
                    offset += 4;

                    // イベント名
                    var eventName = new WoditorString(ref buf, offset);
                    mapEvent.EventName = eventName.String;
                    offset += eventName.ByteLength;

                    // X座標
                    mapEvent.PositionX = buf.ToInt32(Endian.Woditor, offset);
                    offset += 4;

                    // Y座標
                    mapEvent.PositionY += buf.ToInt32(Endian.Woditor, offset);
                    offset += 4;

                    // イベントページ数
                    var eventPageValue = buf.ToInt32(Endian.Woditor, offset);
                    offset += 4;

                    // 0パディングチェック
                    var padding = buf.ToInt32(Endian.Woditor, offset);
                    if (padding != 0)
                    {
                        throw new InvalidOperationException($"マップイベントのパディングが異なります。（offset:{offset}）");
                    }

                    offset += 4;

                    // マップイベントページ
                    var mapEventPageList = new List<MapEventPage>();
                    for (var i = 0; i < eventPageValue; i++)
                    {
                        var onePage = ReadMapEventOnePage();
                        if (onePage == null)
                        {
                            return false;
                        }

                        mapEventPageList.Add(onePage);
                    }

                    mapEvent.MapEventPageList = new MapEventPageList(mapEventPageList);

                    // イベントページ末尾チェック
                    foreach (var b in MapEvent.Footer)
                    {
                        if (buf[offset] != b)
                        {
                            throw new InvalidOperationException($"マップイベント末尾の値が異なります。（offset: {offset}）");
                        }

                        offset++;
                    }

                    mapEvents.Add(mapEvent);
                    count++;
                }

                if (count != size)
                    throw new InvalidOperationException(
                        $"マップイベントデータの数が期待する数と異なります。(期待する数：{size}, 実際のイベント数：{count})");

                MapData.MapEvents = new MapEventList(mapEvents);

                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($"マップイベントデータチェック中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// マップイベントページ
        /// </summary>
        /// <returns>読み込んだ情報 エラー時、null</returns>
        private MapEventPage ReadMapEventOnePage()
        {
            // 例外は上にスローする
            MapEventPage result = new MapEventPage();

            // ヘッダチェック
            foreach (var b in MapEventPage.Header)
            {
                if (buf[offset] != b)
                {
                    throw new InvalidOperationException($"マップイベントページのヘッダが異なります。（offset: {offset}）");
                }

                offset++;
            }

            var graphicInfo = new MapEventPageGraphicInfo();

            // タイル画像ID
            var graphicTileId = buf.ToInt32(Endian.Woditor, offset);
            if (graphicTileId != MapEventPageGraphicInfo.GraphicNotUseTileId)
            {
                graphicInfo.IsGraphicTileChip = true;
                graphicInfo.GraphicTileId = graphicTileId;
            }

            offset += 4;

            // キャラチップ名
            var charaChipString = new WoditorString(ref buf, offset);
            if (!graphicInfo.IsGraphicTileChip)
            {
                graphicInfo.CharaChipFileName = charaChipString.String;
            }

            offset += charaChipString.ByteLength;

            // 初期キャラ向き
            graphicInfo.InitDirection = buf[offset];
            offset++;

            // 初期アニメーション番号
            graphicInfo.InitAnimationId = buf[offset];
            offset++;

            // キャラチップ透過度
            graphicInfo.CharaChipOpacity = buf[offset];
            offset += 1;

            // キャラチップ表示形式
            graphicInfo.CharaChipDrawType = PictureDrawType.FromByte(buf[offset]);
            offset += 1;

            result.GraphicInfo = graphicInfo;

            var bootInfo = new MapEventPageBootInfo();

            // 起動条件
            bootInfo.EventBootType = EventBootType.FromByte(buf[offset]);
            offset += 1;

            // 条件1～4演算子 & 使用フラグ
            var conditions = new List<EventBootCondition>
            {
                new EventBootCondition(),
                new EventBootCondition(),
                new EventBootCondition(),
                new EventBootCondition(),
            };
            for (var i = 0; i < 4; i++)
            {
                conditions[i].Operation = CriteriaOperator.FromByte((byte) (buf[offset] & 0xF0));
                conditions[i].UseCondition = (byte) (buf[offset] & 0x0F) != 0;
                offset++;
            }

            // 条件1～4左辺
            for (var i = 0; i < 4; i++)
            {
                conditions[i].LeftSide = buf.ToInt32(Endian.Woditor, offset);
                offset += 4;
            }

            // 条件1～4右辺
            for (var i = 0; i < 4; i++)
            {
                conditions[i].RightSide = buf.ToInt32(Endian.Woditor, offset);
                offset += 4;
                bootInfo.SetEventBootCondition(i, conditions[i]);
            }

            result.BootInfo = bootInfo;

            var moveRouteInfo = new MapEventPageMoveRouteInfo();

            // アニメ速度
            moveRouteInfo.AnimateSpeed = AnimateSpeed.FromByte(buf[offset]);
            offset += 1;

            // 移動速度
            moveRouteInfo.MoveSpeed = MoveSpeed.FromByte(buf[offset]);
            offset += 1;

            // 移動頻度
            moveRouteInfo.MoveFrequency = MoveFrequency.FromByte(buf[offset]);
            offset += 1;

            // 移動ルート
            moveRouteInfo.MoveType = MoveType.FromByte(buf[offset]);
            offset += 1;

            var option = new MapEventPageOption();

            // オプション
            option.SetOptionFlag(buf[offset]);
            offset += 1;

            result.Option = option;

            // カスタム移動ルートフラグ
            var actionEntry = new ActionEntry();
            actionEntry.SetOptionFlag(buf[offset]);
            offset += 1;

            // 動作指定コマンド数
            var eventCommandLength = buf.ToInt32(Endian.Woditor, offset);
            offset += 4;

            // 動作指定コマンド
            var charaMoveCommandList = new List<ICharaMoveCommand>();
            for (var i = 0; i < eventCommandLength; i++)
            {
                var readCharaMoveCommand = ReadCharaMoveCommand();
                if (readCharaMoveCommand == null) return null;
                charaMoveCommandList.Add(readCharaMoveCommand);
            }

            actionEntry.CommandList = charaMoveCommandList;

            moveRouteInfo.CustomMoveRoute = actionEntry;
            result.MoveRouteInfo = moveRouteInfo;

            // イベント行数
            var eventLength = buf.ToInt32(Endian.Woditor, offset);
            offset += 4;

            // イベントコマンド
            var eventCommandList = new List<IEventCommand>();
            for (var i = 0; i < eventLength; i++)
            {
                var eventCommand = ReadEventCommand();
                if (eventCommand == null) return null;
                eventCommandList.Add(eventCommand);
            }

            result.EventCommands = new EventCommandList(eventCommandList);

            // イベントコマンド終端チェック
            foreach (var b in EventCommandList.EndEventCommand)
            {
                if (buf[offset] != b)
                {
                    throw new InvalidOperationException($"イベントコマンド後の値が異なります。（offset: {offset}）");
                }

                offset++;
            }

            // 影グラフィック番号
            result.ShadowGraphicId = buf[offset];
            offset += 1;

            // 接触範囲拡張X
            result.RangeWidth = buf[offset];
            offset += 1;

            // 接触範囲拡張Y
            result.RangeHeight = buf[offset];
            offset += 1;

            // イベントページ末尾チェック
            foreach (var b in MapEventPage.Footer)
            {
                if (buf[offset] != b)
                {
                    throw new InvalidOperationException($"イベントページ末尾の値が異なります。（offset: {offset}）");
                }

                offset++;
            }

            // 完了
            return result;
        }

        /// <summary>
        /// 動作指定コマンドを返す。
        /// </summary>
        /// <returns>読み込みに失敗した場合、null</returns>
        private ICharaMoveCommand ReadCharaMoveCommand()
        {
            // 例外は上に投げる

            // 動作指定コード
            var charaMoveCode = buf[offset];
            var charaMoveCommand = CharaMoveCommandFactory.CreateRaw(charaMoveCode);
            offset += 1;

            // 変数の数
            var varLength = buf[offset];
            offset += 1;

            // 変数
            for (var i = 0; i < varLength; i++)
            {
                var variable = buf.ToInt32(Endian.Woditor, offset);
                charaMoveCommand.SetNumberValue(i, variable);
                offset += 4;
            }

            // 終端コードチェック
            foreach (var b in Event.CharaMoveCommand.CharaMoveCommandBase.EndBlockCode)
            {
                if (buf[offset] != b)
                {
                    throw new InvalidOperationException($"動作指定コマンド末尾の値が異なります。（offset: {offset}）");
                }

                offset++;
            }

            // 結果
            return charaMoveCommand;
        }

        /// <summary>
        /// イベントコマンドを読み込み、返す。
        /// </summary>
        /// <returns>読み込みに失敗した場合、false</returns>
        private IEventCommand ReadEventCommand()
        {
            // 数値変数の数
            var numVarLength = buf[offset];
            offset += 1;

            // 数値変数
            var numVarList = new List<int>();
            for (var i = 0; i < numVarLength; i++)
            {
                var numVar = buf.ToInt32(Endian.Woditor, offset);
                numVarList.Add(numVar);
                offset += 4;
            }

            // インデント
            var indent = buf[offset];
            offset += 1;

            // 文字データ数
            var strVarLength = buf[offset];
            offset += 1;

            // 文字列変数
            var strVarList = new List<string>();
            for (var i = 0; i < strVarLength; i++)
            {
                var woditorString = new WoditorString(ref buf, offset);
                strVarList.Add(woditorString.String);
                offset += woditorString.ByteLength;
            }

            // 動作指定フラグ
            var hasMoveCommand = buf[offset] != 0;
            offset += 1;

            // 動作指定コマンド
            ActionEntry actionEntry = null;
            if (hasMoveCommand)
            {
                actionEntry = ReadEventActionEntry();
                if (actionEntry == null)
                {
                    return null;
                }
            }

            // 結果
            var eventCommand = EventCommandFactory.CreateRaw(
                numVarLength, numVarList,
                indent,
                strVarLength, strVarList,
                actionEntry);
            return eventCommand;
        }

        /// <summary>
        /// イベントコマンドの動作指定コマンドを返す。
        /// </summary>
        /// <returns>読み込みに失敗した場合、false</returns>
        private ActionEntry ReadEventActionEntry()
        {
            // ヘッダチェック
            foreach (var b in ActionEntry.HeaderBytes)
            {
                if (buf[offset] != b)
                {
                    throw new InvalidOperationException($"イベントコマンド中の動作指定ヘッダの値が異なります。（offset: {offset}）");
                }

                offset++;
            }

            // 動作フラグ
            var actionEntry = new ActionEntry();
            actionEntry.SetOptionFlag(buf[offset]);
            offset += 1;

            // 動作コマンド数
            var commandLength = buf.ToInt32(Endian.Woditor, offset);
            offset += 4;

            // 動作指定コマンド
            var charaMoveCommandList = new List<ICharaMoveCommand>();
            for (var i = 0; i < commandLength; i++)
            {
                var readCharaMoveCommand = ReadCharaMoveCommand();
                if (readCharaMoveCommand == null) return null;
                charaMoveCommandList.Add(readCharaMoveCommand);
            }

            actionEntry.CommandList = charaMoveCommandList;

            return actionEntry;
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <returns>読み込み成否</returns>
        private bool ReadFooter()
        {
            try
            {
                foreach (var b in MapData.Footer)
                {
                    if (buf[offset] != b)
                    {
                        throw new InvalidOperationException($"フッタが正常に取得できませんでした。（offset:{offset}）");
                    }

                    offset++;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessageInfo.SetMessage($@"フッタ処理中にエラーが発生しました。（offset:{offset}）", ex);
                return false;
            }
        }

        /// <summary>
        /// エラーメッセージ管理クラス
        /// </summary>
        private class ErrorMessageInfo
        {
            public string Message { get; private set; } = "";

            public void SetMessage(string mainMessage, Exception exception)
            {
                var builder = new StringBuilder();
                builder.Append($"{mainMessage}{Environment.NewLine}");
                builder.Append($"    {exception.Message}{Environment.NewLine}");
                builder.Append($"    ExceptionClass: {exception.GetType()}{Environment.NewLine}");
                builder.Append($"    StackTrace: {exception.StackTrace}");
                Message = $"{builder}";
            }
        }
    }
}