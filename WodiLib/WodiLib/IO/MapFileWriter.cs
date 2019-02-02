// ========================================
// Project Name : WodiLib
// File Name    : MapFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WodiLib.Map;
using WodiLib.Sys.Observer;

namespace WodiLib.IO
{
    /// <summary>
    /// マップファイル書き出しクラス
    /// </summary>
    public class MapFileWriter
    {
        /// <summary>書き出しファイルパス</summary>
        public string FilePath { get; }

        /// <summary>書き出し成否</summary>
        public bool IsSuccessRead { get; private set; }

        /// <summary>書き出すマップデータ</summary>
        public MapData MapData { get; }

        /// <summary>エラーメッセージ（書き出し失敗時）</summary>
        public string ErrorMessage { get; private set; } = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="outputData">[NotNull] 書き出しマップデータ</param>
        /// <param name="filePath">[NotNull] 書き出しファイルパス</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MapFileWriter(MapData outputData, string filePath)
        {
            if (outputData == null)
                throw new ArgumentNullException(
                    Sys.ErrorMessage.NotNull(nameof(outputData)));
            if (filePath == null)
                throw new ArgumentNullException(
                    Sys.ErrorMessage.NotNull(nameof(filePath)));
            MapData = outputData;
            FilePath = filePath;
        }

        /// <summary>
        /// マップデータを同期的に書き出す。
        /// </summary>
        /// <returns>書き出し成否</returns>
        public bool WriteSync()
        {
            try
            {
                var bin = MapData.ToBinary().ToArray();
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    stream.Write(bin, 0, bin.Length);
                }

                IsSuccessRead = true;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"例外が発生しました。{ex.Message} (type: {ex.GetType()})";
                IsSuccessRead = false;
                return false;
            }
        }


        /// <summary>
        /// マップデータを非同期的に書き出す。
        /// </summary>
        /// <returns>書き出し成否</returns>
        public async Task<bool> WriteAsync()
        {
            return await Task.Run(() => WriteSync());
        }

        /// <summary>
        /// 書き出し処理のColdObservableを生成する。
        /// </summary>
        /// <returns>書き出し成否を通知するObservable</returns>
        public IObservable<bool> CreateObservable()
        {
            return WLObservable<bool>.Create(s =>
            {
                try
                {
                    var result = WriteSync();
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
    }
}