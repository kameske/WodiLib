// ========================================
// Project Name : WodiLib
// File Name    : HasSectionIniFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// Sectionのあるiniファイル書き出しクラス
    /// </summary>
    /// <typeparam name="TIniTarget">書き出しデータクラス</typeparam>
    internal class HasSectionIniFileWriter<TIniTarget>
        where TIniTarget : IIniTarget
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>書き出しファイルパス</summary>
        public FilePath FilePath { get; }

        /// <summary>書き出すデータ</summary>
        public IEnumerable<TIniTarget> Data { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNullOrEmpty] 書き出しファイルパス</param>
        /// <param name="outputData">[NotNull] 書き出しデータ</param>
        /// <exception cref="ArgumentNullException">outputData, filePathがnullの場合</exception>
        public HasSectionIniFileWriter(FilePath filePath, IEnumerable<TIniTarget> outputData)
        {
            if (outputData is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(outputData)));
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            Data = outputData;
            FilePath = filePath;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// データを同期的に書き出す。
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public void WriteSync()
        {
            Logger.Info(FileIOMessage.StartFileWrite(GetType()));

            foreach (var target in Data)
            {
                WriteOneData(target);
            }

            Logger.Info(FileIOMessage.EndFileWrite(GetType()));
        }


        /// <summary>
        /// データを非同期的に書き出す。
        /// </summary>
        /// <returns>書き出しTask</returns>
        /// <exception cref="ArgumentException">
        ///     ファイル名が正しくない場合、
        ///     またはpathが非ファイルデバイスを参照している場合
        /// </exception>
        public async Task WriteAsync()
        {
            await Task.Run(WriteSync);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 対象データひとつ分のデータを書き出す。
        /// </summary>
        /// <param name="target">対象データ</param>
        private void WriteOneData(TIniTarget target)
        {
            var section = target.SectionName;
            var keyAndValueTuples = GetAllPropertyAndValueTuples(target);

            foreach (var (key, value) in keyAndValueTuples)
            {
                IniFileHelper.WritePrivateProfileString(section, key,
                    value, FilePath);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static IEnumerable<(string, string)> GetAllPropertyAndValueTuples(TIniTarget target)
        {
            return target.GetType().GetProperties()
                .Where(x =>
                {
                    var iniTargetAttr = (IniTargetAttribute) x.GetCustomAttribute(typeof(IniTargetAttribute), true);
                    if (iniTargetAttr is null) return false;

                    // サポート対象外の場合、出力対象に含めない
                    return VersionConfig.IsGreaterVersion(iniTargetAttr.SupportMinVersion);
                })
                .Select(propertyInfo => (propertyInfo.Name, (string) propertyInfo.GetValue(target)));
        }
    }
}