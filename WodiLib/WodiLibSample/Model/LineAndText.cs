namespace WodiLibSample.Model
{
    /// <summary>
    /// 行数とテキストの組モデル
    /// </summary>
    internal class LineAndText
    {
        public int Line { get; }
        public string Text { get; }

        public LineAndText(int line, string text)
        {
            Line = line;
            Text = text;
        }
    }
}