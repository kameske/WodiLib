// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SourceFormatTargetHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.Templates.FromAttribute.MainSourceAddableTemplate;

namespace WodiLib.SourceGenerator.Core.Templates.FromAttribute
{
    /// <summary>
    ///     <see cref="SourceFormatTarget"/> 生成補助クラス（FromAttribute用）
    /// </summary>
    internal class SourceFormatTargetHelper
    {
        /// <summary>
        ///     プロパティ情報から文字列定数定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="values">プロパティ値</param>
        /// <param name="workState">内部ワーク状態</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant_String(
            PropertyInfo propertyInfo, PropertyValues values, WorkState workState,
            bool? isOverwrittenProperty = null)
            => new SourceFormatTargetHelper(propertyInfo, values, workState)
                .SourceFormatTargetsClassConstant(
                    isWrapDoubleQuoteValue: true,
                    isOverwrittenProperty: isOverwrittenProperty);

        /// <summary>
        ///     プロパティ情報から数値定数定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="values">プロパティ値</param>
        /// <param name="workState">内部ワーク状態</param>
        /// <param name="defaultValue"><see cref="values"/>にプロパティ値が存在しなかった場合のデフォルト値</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant_Numeric(
            PropertyInfo propertyInfo, PropertyValues values, WorkState workState,
            string defaultValue = "",
            bool? isOverwrittenProperty = null)
            => new SourceFormatTargetHelper(propertyInfo, values, workState).SourceFormatTargetsClassConstant(
                defaultValue,
                isOverwrittenProperty: isOverwrittenProperty);

        /// <summary>
        ///     プロパティ情報から列挙値定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="values">プロパティ値</param>
        /// <param name="workState">内部ワーク状態</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant_Enum(
            PropertyInfo propertyInfo, PropertyValues values, WorkState workState,
            bool? isOverwrittenProperty = null)
            => new SourceFormatTargetHelper(propertyInfo, values, workState).SourceFormatTargetsClassConstant(
                isCastValue: true, isOverwrittenProperty: isOverwrittenProperty);

        /// <summary>
        ///     プロパティ情報から定数定義コードを生成する。
        /// </summary>
        /// <param name="propertyInfo">プロパティ情報</param>
        /// <param name="values">プロパティ値</param>
        /// <param name="workState">内部ワーク状態</param>
        /// <param name="defaultValue"><see cref="values"/>にプロパティ値が存在しなかった場合のデフォルト値</param>
        /// <param name="isWrapDoubleQuoteValue">プロパティ値ダブルクォート付与フラグ</param>
        /// <param name="isCastValue">プロパティ値キャストフラグ</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public static SourceFormatTargetBlock SourceFormatTargetsClassConstant(
            PropertyInfo propertyInfo, PropertyValues values, WorkState workState,
            string defaultValue = "", bool isWrapDoubleQuoteValue = false,
            bool isCastValue = false, bool? isOverwrittenProperty = null
        )
            => new SourceFormatTargetHelper(propertyInfo, values, workState)
                .SourceFormatTargetsClassConstant(defaultValue, isWrapDoubleQuoteValue,
                    isCastValue, isOverwrittenProperty);

        private PropertyInfo PropertyInfo { get; }
        private PropertyValues Values { get; }
        private WorkState WorkState { get; }

        public SourceFormatTargetHelper(PropertyInfo propertyInfo, PropertyValues values, WorkState workState)
        {
            PropertyInfo = propertyInfo;
            Values = values;
            WorkState = workState;
        }

        /// <summary>
        ///     文字列定数定義コードを生成する。
        /// </summary>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public SourceFormatTargetBlock SourceFormatTargetsClassConstant_String(
            bool? isOverwrittenProperty = null)
            => SourceFormatTargetsClassConstant(
                isWrapDoubleQuoteValue: true,
                isOverwrittenProperty: isOverwrittenProperty);

        /// <summary>
        ///     数値定数定義コードを生成する。
        /// </summary>
        /// <param name="defaultValue">プロパティ値が存在しなかった場合のデフォルト値</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public SourceFormatTargetBlock SourceFormatTargetsClassConstant_Numeric(
            string defaultValue = "",
            bool? isOverwrittenProperty = null)
            => SourceFormatTargetsClassConstant(defaultValue,
                isOverwrittenProperty: isOverwrittenProperty);

        /// <summary>
        ///     プロパティ情報から列挙値定義コードを生成する。
        /// </summary>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public SourceFormatTargetBlock SourceFormatTargetsClassConstant_Enum(
            bool? isOverwrittenProperty = null)
            => SourceFormatTargetsClassConstant(
                isCastValue: true, isOverwrittenProperty: isOverwrittenProperty);

        /// <summary>
        ///     プロパティ情報から定数定義コードを生成する。
        /// </summary>
        /// <param name="defaultValue">プロパティ値が存在しなかった場合のデフォルト値</param>
        /// <param name="isWrapDoubleQuoteValue">プロパティ値ダブルクォート付与フラグ</param>
        /// <param name="isCastValue">プロパティ値キャストフラグ</param>
        /// <param name="isOverwrittenProperty">プロパティ上書きフラグ（<see langword="null"/>の場合自動判定する）</param>
        /// <returns>ソース文字列情報</returns>
        public SourceFormatTargetBlock SourceFormatTargetsClassConstant(
            string defaultValue = "", bool isWrapDoubleQuoteValue = false,
            bool isCastValue = false, bool? isOverwrittenProperty = null
        )
        {
            var propertyName = PropertyInfo.Name;

            var setValue = Values.GetOrDefault(propertyName, defaultValue);
            if (isCastValue)
            {
                setValue = $"({PropertyInfo.Type}) {setValue}";
            }

            if (isWrapDoubleQuoteValue)
            {
                setValue = $"@{setValue.ToWrappedDoubleQuote()}";
            }

            var hasNewModifier = isOverwrittenProperty ?? WorkState.IsPropertyOverwritten(propertyName);

            return ClassConstants.Generate(
                ClassConstants.ConstantInfo.FromPropertyInfo(
                    PropertyInfo, setValue, hasNewModifier
                )
            );
        }
    }
}
