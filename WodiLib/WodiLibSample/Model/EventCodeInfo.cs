using System.Drawing;

namespace WodiLibSample.Model
{
    /// <summary>
    /// イベントコード情報モデル
    /// </summary>
    internal class EventCodeInfo
    {
        /// <summary>
        /// 行番号
        /// </summary>
        public int LineNo { get; }

        /// <summary>
        /// コード文字列
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// コード配色
        /// </summary>
        public Color StringColor { get; }

        public EventCodeInfo(int lineNo, string code, Color stringColor)
        {
            LineNo = lineNo;
            Code = code;
            StringColor = stringColor;
        }
    }
}
