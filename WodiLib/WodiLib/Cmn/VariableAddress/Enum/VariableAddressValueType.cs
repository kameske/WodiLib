// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressValueType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    /// 各情報アドレス情報種別
    /// </summary>
    public class VariableAddressValueType : TypeSafeEnum<VariableAddressValueType>
    {
        /// <summary>数値</summary>
        public static readonly VariableAddressValueType Numeric;

        /// <summary>文字列</summary>
        public static readonly VariableAddressValueType String;

        /// <summary>両方</summary>
        public static readonly VariableAddressValueType Both;

        static VariableAddressValueType()
        {
            Numeric = new VariableAddressValueType(nameof(Numeric), 0x01,
                "[{0}]★エラー！整数変数ではありません！！");
            String = new VariableAddressValueType(nameof(String), 0x02,
                "[{0}]★エラー！文字変数ではありません！！");
            Both = new VariableAddressValueType(nameof(Both), 0x03,
                "[{0}]★エラー！該当する変数がありません");
        }

        private VariableAddressValueType(string id, byte typeFlag, string eventCommandStringErrorFormat) : base(id)
        {
            TypeFlag = typeFlag;
            EventCommandStringErrorFormat = eventCommandStringErrorFormat;
        }

        private byte TypeFlag { get; }

        /// <summary>イベントコマンド文フォーマット</summary>
        public string EventCommandStringErrorFormat { get; }

        /// <summary>
        /// エラー時のイベントコマンド文用文字列を生成する。
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>イベントコマンド文字列</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string MakeEventCommandErrorSentence(int value)
        {
            return string.Format(EventCommandStringErrorFormat, value);
        }

        /// <summary>
        /// 自身のタイプ種別に指定したタイプ種別が適合するか判定する。
        /// </summary>
        /// <param name="target">[Nullable] 判定対象</param>
        /// <returns>targetがnullの場合false、適合する場合true。</returns>
        /// <remarks>
        ///     自身がBothの場合、targetがBoth, Numeric, String いずれの場合もtrue。
        ///     自身がNumericの場合、targetがNumericの場合のみtrue。
        ///     自身がStringの場合、targetがStringの場合のみtrue。
        /// </remarks>
        public bool CheckTypeInclude(VariableAddressValueType target)
        {
            if (target is null) return false;
            return (TypeFlag & target.TypeFlag) != 0;
        }
    }
}