// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSelfVariableNameList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Common
{
    /// <summary>
    /// コモンイベントセルフ変数名リスト
    /// </summary>
    [Serializable]
    public class CommonEventSelfVariableNameList :
        FixedLengthList<CommonEventSelfVariableName, CommonEventSelfVariableNameList>,
        IReadOnlyCommonEventSelfVariableNameList
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト数</summary>
        public static int Capacity => 100;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonEventSelfVariableNameList() : base(Capacity)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期要素</param>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が100以外の場合</exception>
        public CommonEventSelfVariableNameList(
            IEnumerable<CommonEventSelfVariableName> items)
            : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        /// <summary>
        /// 格納対象のデフォルトインスタンスを生成する。
        /// </summary>
        /// <param name="index">挿入インデックス</param>
        /// <returns>デフォルトインスタンス</returns>
        protected override CommonEventSelfVariableName MakeDefaultItem(int index) =>
            new CommonEventSelfVariableName("");

        /// <inheritdoc/>
        protected override IWodiLibListValidator<CommonEventSelfVariableName> MakeValidator()
            => new Validator(this);

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Common
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        public byte[] ToBinary()
        {
            var result = new List<byte>();

            foreach (var varName in this)
            {
                result.AddRange(varName.ToWoditorStringBytes());
            }

            return result.ToArray();
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Classes
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// このクラス専用のValidator
        /// </summary>
        protected class Validator : WodiLibListValidatorTemplate<CommonEventSelfVariableName>
        {
            /// <inheritdoc/>
            protected override IWodiLibListValidator<CommonEventSelfVariableName>? BaseValidator { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="target">対象</param>
            public Validator(CommonEventSelfVariableNameList target) : base(target)
            {
                BaseValidator = new FixedLengthListValidator<CommonEventSelfVariableName>(target);
            }

            /// <inheritdoc/>
            public override void Constructor(IReadOnlyList<CommonEventSelfVariableName> initItems)
            {
                BaseValidator?.Constructor(initItems);
                FixedLengthListValidationHelper.ItemCount(initItems.Count, Capacity);
            }
        }
    }
}
