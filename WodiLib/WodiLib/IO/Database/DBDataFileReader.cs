// ========================================
// Project Name : WodiLib
// File Name    : DBDataFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// DBプロジェクトデータファイル読み込みクラス
    /// </summary>
    internal class DBDataFileReader
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みファイルパス</summary>
        public string FilePath { get; }

        /// <summary>[Nullable] 読み込んだDBプロジェクトデータ</summary>
        public DBData Data { get; private set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DBDataFileReader(string filePath)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));
            if (filePath.IsEmpty())
                throw new ArgumentException(
                    ErrorMessage.NotEmpty(nameof(filePath)));

            FilePath = filePath;
            ReadStatus = new FileReadStatus(FilePath);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public DBData ReadSync()
        {
            Logger.Info(FileIOMessage.StartFileRead(GetType()));

            if (!(Data is null))
                throw new InvalidOperationException(
                    $"すでに読み込み完了しています。");

            Data = new DBData();

            // ヘッダチェック
            ReadHeader(ReadStatus);

            // DBデータ
            ReadDbData(ReadStatus, Data);

            Logger.Info(FileIOMessage.EndFileRead(GetType()));

            return Data;
        }

        /// <summary>
        /// ファイルを非同期的に読み込む
        /// </summary>
        /// <returns>読み込み成否</returns>
        /// <exception cref="InvalidOperationException">
        ///     すでにファイルを読み込んでいる場合、
        ///     またはファイルが正しく読み込めなかった場合
        /// </exception>
        public async Task<DBData> ReadAsync()
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
            foreach (var b in DBData.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）");
                }

                status.IncreaseByteOffset();
            }

            Logger.Debug(FileIOMessage.CheckOk(typeof(DBDataFileReader),
                "ヘッダ"));
        }

        private void ReadDbData(FileReadStatus status, DBData data)
        {
            // データ数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataFileReader),
                "データ数数", length));

            // DBデータ
            var dataDescList = new DatabaseDataDescList();

            for (var i = 0; i < length; i++)
            {
                var desc = new DatabaseDataDesc();

                // データ名
                var dataName = status.ReadString();
                status.AddOffset(dataName.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataFileReader),
                    "データ名", dataName.String));

                desc.DataName = dataName.String;

                // 数値項目
                ReadDbDataIntValues(status, out var intValues);
                desc.ItemValueList.AddRange(intValues);

                // 文字列項目
                ReadDbDataStringValues(status, out var stringValues);
                desc.ItemValueList.AddRange(stringValues);

                dataDescList.Overwrite(i, new List<DatabaseDataDesc> {desc});
            }

            data.DataDescList.Overwrite(0, dataDescList);
        }

        /// <summary>
        /// DBデータの数値項目
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="values">読み込み結果</param>
        private void ReadDbDataIntValues(FileReadStatus status, out IReadOnlyList<DBItemValue> values)
        {
            // 数値項目数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataFileReader),
                "数値項目数", length));

            var result = new List<DBItemValue>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadInt();
                status.IncreaseIntOffset();

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataFileReader),
                    $"  数値項目{i,2}", value));

                result.Add((DBValueInt) value);
            }

            values = result;
        }


        /// <summary>
        /// DBデータの文字列項目
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="values">読み込み結果</param>
        private void ReadDbDataStringValues(FileReadStatus status, out IReadOnlyList<DBItemValue> values)
        {
            // 数値項目数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataFileReader),
                "文字列項目数", length));

            var result = new List<DBItemValue>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadString();
                status.AddOffset(value.ByteLength);

                Logger.Debug(FileIOMessage.SuccessRead(typeof(DBDataFileReader),
                    $"  文字列項目{i,2}", value));

                result.Add((DBValueString) value.String);
            }

            values = result;
        }
    }
}