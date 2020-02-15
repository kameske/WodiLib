// ========================================
// Project Name : WodiLib
// File Name    : TileSetSettingReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Map;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// タイルセット設定読み込みクラス
    /// </summary>
    internal class TileSetSettingReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込み経過状態</summary>
        private FileReadStatus Status { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        public TileSetSettingReader(FileReadStatus status)
        {
            Status = status;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイルセット設定を読み込み、返す。
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        public TileSetSetting Read()
        {
            Logger.Debug(FileIOMessage.StartCommonRead(GetType(), ""));

            var result = ReadOneTileSetSetting(Status);

            Logger.Debug(FileIOMessage.EndCommonRead(GetType(), ""));

            return result;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// タイルセット設定一つ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">バイナリデータがファイル仕様と異なる場合</exception>
        private TileSetSetting ReadOneTileSetSetting(FileReadStatus status)
        {
            Logger.Debug(FileIOMessage.StartCommonRead(typeof(TileSetSettingReader), "タイルセット設定"));

            // 設定名
            ReadName(status, out var name);

            // 基本タイルセットファイル名
            ReadBaseTileSetFileName(status, out var baseTileSetFileName);

            // オートタイルファイル名リスト
            ReadAutoTileSetFileNameList(status, AutoTileFileNameList.MaxCapacity, out var autoTileFileNames);

            // セパレータ
            ReadSeparator(status);

            // タグ番号リスト
            ReadTagNumberList(status, out var tagNumbers);

            // セパレータ
            ReadSeparator(status);

            // タイル設定リスト
            ReadTilePathSettingList(status, out var tilePathSettings);

            var result = new TileSetSetting(new TileTagNumberList(tagNumbers),
                new TilePathSettingList(tilePathSettings),
                new AutoTileFileNameList(autoTileFileNames))
            {
                Name = name,
                BaseTileSetFileName = baseTileSetFileName
            };

            Logger.Debug(FileIOMessage.EndCommonRead(typeof(TileSetSettingReader), "タイルセット設定"));

            return result;
        }

        /// <summary>
        /// タイプ名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="name">結果格納インスタンス</param>
        private void ReadName(FileReadStatus status, out TileSetName name)
        {
            var read = status.ReadString();
            name = read.String;

            status.AddOffset(read.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                "設定名", name));
        }

        /// <summary>
        /// 基本タイルセットファイル名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="fileName">結果格納インスタンス</param>
        private void ReadBaseTileSetFileName(FileReadStatus status, out BaseTileSetFileName fileName)
        {
            var read = status.ReadString();
            fileName = read.String;

            status.AddOffset(read.ByteLength);

            Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                "基本タイルセットファイル名", fileName));
        }

        /// <summary>
        /// オートタイルファイル名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="listLength">オートタイルファイル名数</param>
        /// <param name="list">結果格納インスタンス</param>
        private void ReadAutoTileSetFileNameList(FileReadStatus status, int listLength,
            out List<AutoTileFileName> list)
        {
            list = new List<AutoTileFileName>();

            for (var i = 0; i < listLength; i++)
            {
                var read = status.ReadString();
                AutoTileFileName fileName = read.String;
                list.Add(fileName);

                status.AddOffset(read.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                    $"オートタイル{i}ファイル名", fileName));
            }
        }

        /// <summary>
        /// セパレータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        private void ReadSeparator(FileReadStatus status)
        {
            var read = status.ReadByte();

            if (read != TileSetSetting.DataSeparator)
                throw new InvalidOperationException(
                    $"データセパレータが正しく読み込めませんでした。（offset:{status.Offset}）");

            status.IncreaseByteOffset();

            Logger.Debug(FileIOMessage.CheckOk(typeof(TileSetFileReader),
                "データセパレータ"));
        }

        /// <summary>
        /// タグ番号リスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="list">結果格納インスタンス</param>
        private void ReadTagNumberList(FileReadStatus status, out List<TileTagNumber> list)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                "タイルタグ番号数", length));

            list = new List<TileTagNumber>();

            for (var i = 0; i < length; i++)
            {
                var tagNumber = status.ReadByte();
                status.IncreaseByteOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                    $"タイルタグ{i}番号", tagNumber));

                list.Add(tagNumber);
            }
        }

        /// <summary>
        /// タイル通行設定リスト
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="list">結果格納インスタンス</param>
        private void ReadTilePathSettingList(FileReadStatus status, out List<TilePathSetting> list)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                "タイル通行設定数", length));

            list = new List<TilePathSetting>();

            for (var i = 0; i < length; i++)
            {
                var tilePathSettingCode = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(TileSetSettingReader),
                    $"タイルパス設定{i}コード", tilePathSettingCode));

                list.Add(new TilePathSetting(tilePathSettingCode));
            }
        }
    }
}