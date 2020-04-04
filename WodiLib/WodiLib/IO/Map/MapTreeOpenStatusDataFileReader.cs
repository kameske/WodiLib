// ========================================
// Project Name : WodiLib
// File Name    : MapTreeOpenStatusDataFileReader.cs
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
    /// マップツリーデータ読み込みクラス
    /// </summary>
    public class
        MapTreeOpenStatusDataFileReader : WoditorFileReaderBase<MapTreeOpenStatusDataFilePath, MapTreeOpenStatusData>
    {
        private FileReadStatus ReadStatus { get; }

        private readonly object readLock = new object();

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public MapTreeOpenStatusDataFileReader(MapTreeOpenStatusDataFilePath filePath) : base(filePath)
        {
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
        public override MapTreeOpenStatusData ReadSync()
        {
            lock (readLock)
            {
                Logger.Info(FileIOMessage.StartFileRead(GetType()));

                var result = ReadData(ReadStatus);

                Logger.Info(FileIOMessage.EndFileRead(GetType()));

                return result;
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データ読み込みRoot
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>読み込んだデータインスタンス</returns>
        private MapTreeOpenStatusData ReadData(FileReadStatus status)
        {
            // ヘッダ
            ReadHeader(status);

            // ツリーノード
            ReadOpenStatusList(status, out var statuses);

            // フッタ
            ReadFooter(status);

            return new MapTreeOpenStatusData
            {
                StatusList = new MapTreeOpenStatusList(statuses)
            };
        }

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in MapTreeOpenStatusData.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(MapTreeOpenStatusDataFileReader),
                "ヘッダ"));
        }

        /// <summary>
        /// タイルセット設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="statuses">読み込み結果格納インスタンス</param>
        private void ReadOpenStatusList(FileReadStatus status, out List<MapTreeOpenState> statuses)
        {
            // ステータス数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MapTreeOpenStatusDataFileReader),
                "マップツリー開閉状態数", length));

            statuses = new List<MapTreeOpenState>();

            for (var i = 0; i < length; i++)
            {
                var code = status.ReadByte();
                status.IncreaseByteOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MapTreeOpenStatusDataFileReader),
                    $"マップツリー開閉状態{i}コード値", code));

                statuses.Add(MapTreeOpenState.FromCode(code));
            }
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルフッタが仕様と異なる場合</exception>
        private void ReadFooter(FileReadStatus status)
        {
            foreach (var b in MapTreeOpenStatusData.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルフッタがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(MapTreeOpenStatusDataFileReader),
                "フッタ"));
        }
    }
}