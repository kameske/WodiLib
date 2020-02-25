// ========================================
// Project Name : WodiLib
// File Name    : NonSectionIniFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// SectionのないIniファイル読み込みクラス
    /// </summary>
    internal class NonSectionIniFileReader<TIniTarget>
        where TIniTarget : IIniTarget
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みファイルパス</summary>
        public FilePath FilePath { get; }

        /// <summary>[Nullable] 読み込んだデータ</summary>
        public IReadOnlyCollection<TIniTarget> Data { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 読み込み完了フラグ
        /// </summary>
        private bool IsAlreadyRead { get; set; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">[NotNull] 読み込みファイルパス</param>
        /// <param name="targets">[NotNull] 読み込みデータ格納用インスタンス</param>
        /// <exception cref="ArgumentNullException">
        ///     filePath, targetsがnullの場合、
        ///    またはtargetsにnull要素が含まれる場合
        /// </exception>
        public NonSectionIniFileReader(FilePath filePath, IReadOnlyCollection<TIniTarget> targets)
        {
            if (filePath is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(filePath)));

            if (targets is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(targets)));
            if (targets.HasNullItem())
                throw new ArgumentNullException(
                    ErrorMessage.NotNullInList(nameof(targets)));

            FilePath = filePath;
            Data = targets;
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
        public IEnumerable<TIniTarget> ReadSync()
        {
            if (IsAlreadyRead)
                throw new InvalidOperationException(
                    "すでに読み込み完了しています。");

            Logger.Info(FileIOMessage.StartFileRead(GetType()));

            foreach (var target in Data)
            {
                ReadOneData(target);
            }

            Logger.Info(FileIOMessage.EndFileRead(GetType()));

            IsAlreadyRead = true;

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
        public async Task<IEnumerable<TIniTarget>> ReadAsync()
        {
            return await Task.Run(ReadSync);
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     ReadMethod
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private void ReadOneData(TIniTarget target)
        {
            var properties = GetAllPropertyInfo();

            foreach (var line in File.ReadLines(FilePath, Encoding.Default))
            {
                var iniProperty = new IniProperty(line);

                var targetProperty = properties.FirstOrDefault(x => x.Name.Equals(iniProperty.PropertyName));
                if (targetProperty == null) continue;

                targetProperty.SetValue(target, iniProperty.Value);
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static List<PropertyInfo> GetAllPropertyInfo()
        {
            return typeof(TIniTarget).GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(IniTargetAttribute), true).Any())
                .ToList();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Inner Class
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Ini設定文字列
        /// </summary>
        private class IniProperty
        {
            /// <summary>
            /// プロパティ名
            /// </summary>
            public string PropertyName { get; }

            /// <summary>
            /// 値
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="src"></param>
            public IniProperty(string src)
            {
                var trim = TrimComment(src);
                (PropertyName, Value) = GetPropertyNameAndValue(trim);
            }


            /// <summary>
            /// コメントを除去する
            /// </summary>
            /// <param name="line">対象文字列</param>
            /// <returns>コメントを除去した文字列</returns>
            private static string TrimComment(string line)
            {
                var regex = new Regex("#(.*)$");
                var replaced = regex.Replace(line, "");
                return replaced.Trim();
            }

            /// <summary>
            /// プロパティ名と値のペアを取得する。
            /// </summary>
            /// <param name="line">対象</param>
            /// <returns>プロパティ名と値のペア</returns>
            private static (string, string) GetPropertyNameAndValue(string line)
            {
                var splitStr = line.Split('=');
                return (splitStr[0], splitStr[1]);
            }
        }
    }
}