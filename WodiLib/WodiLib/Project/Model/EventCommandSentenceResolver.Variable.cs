// ========================================
// Project Name : WodiLib
// File Name    : EventCommandSentenceResolver.Variable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Database;

namespace WodiLib.Project
{
    /// <summary>
    /// イベントコマンド文文字列解決クラス・変数
    /// </summary>
    internal class EventCommandSentenceResolver_Variable
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region SystemDatabase

        /// <summary>文字列変数のタイプID</summary>
        public TypeId StringVariableTypeId => 4;

        /// <summary>システム文字列変数のタイプID</summary>
        public TypeId SystemStringVariableTypeId => 5;

        /// <summary>システム変数のタイプID</summary>
        public TypeId SystemVariableTypeId => 6;

        /// <summary>通常変数のタイプID</summary>
        public TypeId NormalNumberVariableTypeId => 14;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>呼び出し元</summary>
        public EventCommandSentenceResolver Master { get; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="master">呼び出し元</param>
        public EventCommandSentenceResolver_Variable(
            EventCommandSentenceResolver master)
        {
            Master = master;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 文字列変数名を返す。
        /// </summary>
        /// <param name="index">文字列変数インデックス</param>
        /// <returns>文字列変数名（存在しない場合専用の文字列）</returns>
        public DataName GetStringVariableName(StringVariableIndex index)
        {
            return Master.GetDatabaseDataName(DBKind.System,
                StringVariableTypeId, index).Item2;
        }

        /// <summary>
        /// システム文字列変数名を返す。
        /// </summary>
        /// <param name="index">システム文字列変数インデックス</param>
        /// <returns>システム文字列変数名（存在しない場合専用の文字列）</returns>
        public DataName GetSystemStringVariableName(SystemStringVariableIndex index)
        {
            return Master.GetDatabaseDataName(DBKind.System,
                SystemStringVariableTypeId, index).Item2;
        }

        /// <summary>
        /// システム変数名を返す。
        /// </summary>
        /// <param name="index">システム変数インデックス</param>
        /// <returns>システム変数名（存在しない場合専用の文字列）</returns>
        public DataName GetSystemVariableName(SystemVariableIndex index)
        {
            return Master.GetDatabaseDataName(DBKind.System,
                SystemVariableTypeId, index).Item2;
        }

        /// <summary>
        /// 通常変数名を返す。
        /// </summary>
        /// <param name="index">通常変数インデックス</param>
        /// <returns>通常変数名（存在しない場合専用の文字列）</returns>
        public DataName GetNormalVariableName(NormalNumberVariableIndex index)
        {
            return Master.GetDatabaseDataName(DBKind.System,
                NormalNumberVariableTypeId, index).Item2;
        }

        /// <summary>
        /// 予備変数名を返す。
        /// </summary>
        /// <param name="number">予備変数番号</param>
        /// <param name="index">予備変数インデックス</param>
        /// <returns>予備変数名（存在しない場合専用の文字列）</returns>
        public DataName GetSpareVariableName(SpareNumberVariableNumber number,
            SpareNumberVariableIndex index)
        {
            return Master.GetDatabaseDataName(DBKind.System,
                NormalNumberVariableTypeId + number, index).Item2;
        }
    }
}