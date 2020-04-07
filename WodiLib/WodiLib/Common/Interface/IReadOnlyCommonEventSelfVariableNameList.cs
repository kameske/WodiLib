// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyCommonEventSelfVariableNameList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Common
{
    /// <summary>
    /// 読み取り専用コモンイベントリスト
    /// </summary>
    public interface IReadOnlyCommonEventSelfVariableNameList
        : IReadOnlyList<CommonEventSelfVariableName>
    {
    }
}