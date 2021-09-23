// ========================================
// Project Name : WodiLib
// File Name    : CharaChipFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [NotNewLine] キャラチップファイルパス
    /// </summary>
    [StringValueObject(IsAllowNewLine = false, CastType = CastType.Implicit)]
    public partial class CharaChipFilePath
    {
    }
}
