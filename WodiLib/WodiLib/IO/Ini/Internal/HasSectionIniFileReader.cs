// ========================================
// Project Name : WodiLib
// File Name    : HasSectionIniFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WodiLib.Cmn;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    /// SectionのあるIniファイル読み込みクラス
    /// </summary>
    internal class HasSectionIniFileReader<TIniTarget>
        where TIniTarget : IIniTarget
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>読み込みファイルパス</summary>
        public FilePath FilePath { get; }

        /// <summary>読み込んだデータ</summary>
        public IReadOnlyCollection<TIniTarget> Data { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger Logger { get; } = WodiLibLogger.GetInstance();

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 読み込み完了フラグ
        /// </summary>
        private bool IsAlreadyRead { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <param name="targets">読み込みデータ格納用インスタンス</param>
        /// <exception cref="ArgumentNullException">
        ///     filePath, targetsがnullの場合、
        ///    またはtargetsにnull要素が含まれる場合
        /// </exception>
        public HasSectionIniFileReader(FilePath filePath, IReadOnlyCollection<TIniTarget> targets)
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
            var section = target.SectionName;

            var properties = GetAllPropertyInfo();

            foreach (var propertyInfo in properties)
            {
                var iniTargetAttr =
                    (IniTargetAttribute) propertyInfo.GetCustomAttribute(typeof(IniTargetAttribute), true);
                if (VersionConfig.IsUnderVersion(iniTargetAttr.SupportMinVersion))
                {
                    // サポート対象外の場合、ファイルからは取得できないため固定で空文字をセット
                    propertyInfo.SetValue(target, string.Empty);
                    continue;
                }

                var sb = new StringBuilder(256);
                IniFileHelper.GetPrivateProfileString(section ?? "", propertyInfo.Name,
                    "", sb, sb.Capacity, FilePath);

                var result = sb.ToString();

                Logger.Debug(FileIOMessage.SuccessRead(GetType(), $"プロパティ：{propertyInfo.Name}", result));

                propertyInfo.SetValue(target, sb.ToString());
            }
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Static Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private static IEnumerable<PropertyInfo> GetAllPropertyInfo()
        {
            return typeof(TIniTarget).GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(IniTargetAttribute), true).Any());
        }
    }
}