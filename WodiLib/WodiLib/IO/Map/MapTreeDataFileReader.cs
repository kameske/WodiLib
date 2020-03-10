// ========================================
// Project Name : WodiLib
// File Name    : MapTreeDataFileReader.cs
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
    public class MapTreeDataFileReader : WoditorFileReaderBase<MapTreeDataFilePath, MapTreeData>
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
        public MapTreeDataFileReader(MapTreeDataFilePath filePath) : base(filePath)
        {
            ReadStatus = new FileReadStatus(FilePath);
        }

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public override MapTreeData ReadSync()
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
        private MapTreeData ReadData(FileReadStatus status)
        {
            // ヘッダ
            ReadHeader(status);

            // ツリーノード
            ReadTreeNodeList(status, out var nodes);

            // フッタ
            ReadFooter(status);

            return new MapTreeData
            {
                TreeNodeList = new MapTreeNodeList(nodes)
            };
        }

        /// <summary>
        /// ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in MapTreeData.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(MapTreeDataFileReader),
                "ヘッダ"));
        }

        /// <summary>
        /// タイルセット設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="nodes">読み込み結果格納インスタンス</param>
        private void ReadTreeNodeList(FileReadStatus status, out List<MapTreeNode> nodes)
        {
            // ノード数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(MapTreeDataFileReader), "マップツリーノード数", length));

            nodes = new List<MapTreeNode>();

            for (var i = 0; i < length; i++)
            {
                var parent = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MapTreeDataFileReader),
                    $"マップツリーノード{i} 親マップID", parent));

                var me = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(MapTreeDataFileReader),
                    $"マップツリーノード{i} 自身マップID", me));

                nodes.Add(new MapTreeNode(me, parent));
            }
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルフッタが仕様と異なる場合</exception>
        private void ReadFooter(FileReadStatus status)
        {
            foreach (var b in MapTreeData.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルフッタがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(MapTreeDataFileReader),
                "フッタ"));
        }
    }
}