// ========================================
// Project Name : WodiLib
// File Name    : CommonEventSpecialArgCase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Common
{
    /// <summary>
    ///     コモンイベント特殊指定選択肢
    /// </summary>
    [CommonMultiValueObject]
    public partial record CommonEventSpecialArgCase
    {
        private readonly CommonEventSpecialArgCaseNumber _caseNumber = 0;
        private readonly CommonEventSpecialArgCaseDescription _description = "";

        /// <summary>選択肢番号</summary>
        public CommonEventSpecialArgCaseNumber CaseNumber
        {
            get => _caseNumber;
            init
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(CaseNumber));
                _caseNumber = value;
            }
        }

        /// <summary>選択肢内容</summary>
        public CommonEventSpecialArgCaseDescription Description
        {
            get => _description;
            init
            {
                ThrowHelper.ValidatePropertyNotNull(value is null, nameof(Description));
                _description = value;
            }
        }

        [Obsolete]
        public CommonEventSpecialArgCase(
            CommonEventSpecialArgCaseNumber caseNumber,
            CommonEventSpecialArgCaseDescription description
        )
        {
            ThrowHelper.ValidateArgumentNotNull(caseNumber is null, nameof(caseNumber));
            ThrowHelper.ValidateArgumentNotNull(description is null, nameof(description));
            _caseNumber = caseNumber;
            _description = description;
        }
    }
}
