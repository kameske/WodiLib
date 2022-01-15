// ========================================
// Project Name : WodiLib
// File Name    : ChoiceCaseList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Event.EventCommand
{
    /// <summary>
    /// 選択肢リスト
    /// </summary>
    public partial class ChoiceCaseList : FixedLengthList<string, ChoiceCaseList>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constant
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>リスト数</summary>
        public static int Capacity => 12;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private int caseValue = 1;

        /// <summary>
        /// [Range(1, 12)] 選択肢数
        /// </summary>
        /// <exception cref="PropertyOutOfRangeException">1～12以外の値を設定した場合</exception>
        public int CaseValue
        {
            get => caseValue;
            set
            {
                if (value < 1 || 12 < value)
                    throw new PropertyOutOfRangeException(
                        ErrorMessage.OutOfRange(nameof(CaseValue), 1, 12, value)
                    );
                caseValue = value;
            }
        }

        /// <summary>
        /// 選択肢番号を指定して文字列を取得する。
        /// </summary>
        /// <param name="index">[Range(0, CaseValue - 1)] 選択肢番号</param>
        /// <returns>選択肢番号に対応した文字列</returns>
        /// <exception cref="ArgumentOutOfRangeException">0～選択肢最大番号以外の値を設定した場合</exception>
        [Obsolete("インデクサを通じて値を取得してください。 Ver2.4で削除します。")]
        public string Get(int index)
        {
            if (index < 0 || CaseValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, CaseValue - 1, index)
                );
            return this[index];
        }

        /// <summary>
        /// 選択肢番号を指定して内容を更新する。
        /// </summary>
        /// <param name="index">[Range(0, CaseValue - 1)] 選択肢番号</param>
        /// <param name="src">[NotNull] 更新文字列</param>
        /// <exception cref="ArgumentOutOfRangeException">0～選択肢最大番号以外の値を設定した場合</exception>
        /// <exception cref="ArgumentNullException">srcがnullの場合</exception>
        [Obsolete("インデクサを通じて値を設定してください。 Ver2.4で削除します。")]
        public void Set(int index, string src)
        {
            if (index < 0 || CaseValue <= index)
                throw new ArgumentOutOfRangeException(
                    ErrorMessage.OutOfRange(nameof(index), 0, CaseValue - 1, index)
                );
            if (src is null)
                throw new ArgumentNullException(
                    ErrorMessage.NotNull(nameof(src))
                );
            this[index] = src;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChoiceCaseList() : base(Enumerable.Repeat("", 12))
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">初期リスト</param>
        /// <exception cref="ArgumentNullException">itemsがnullの場合</exception>
        /// <exception cref="InvalidOperationException">itemsの要素数が12以外の場合</exception>
        [Obsolete]
        public ChoiceCaseList(IEnumerable<string> items) : base(items)
        {
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Protected Override Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <inheritdoc />
        protected override int GetCapacity() => Capacity;

        /// <inheritdoc />
        protected override string MakeDefaultItem(int index) => "";

        /// <inheritdoc />
        protected override IWodiLibListValidator<string> MakeValidator()
        {
            return new CustomValidator(this);
        }
    }
}
