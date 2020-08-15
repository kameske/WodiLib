// ========================================
// Project Name : WodiLib
// File Name    : ISpecialDataSpecificationLoadFile.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    /// DB項目：データ内容の特殊設定＝「ファイル読み込み」の場合の
    /// 特殊設定内容インタフェース
    /// </summary>
    public interface ISpecialDataSpecificationLoadFile : IEquatable<ISpecialDataSpecificationLoadFile>
    {
        /// <summary>
        /// [NotNull] 初期フォルダ
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        DBSettingFolderName FolderName { get; set; }

        /// <summary>
        /// 保存時にフォルダ名省略フラグ
        /// </summary>
        /// <exception cref="PropertyException">特殊指定が「ファイル読み込み」以外の場合</exception>
        bool OmissionFolderNameFlag { get; set; }
    }
}
