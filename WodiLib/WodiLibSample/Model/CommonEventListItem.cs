namespace WodiLibSample.Model
{
    /// <summary>
    /// コモンイベントリストアイテムモデル
    /// </summary>
    internal class CommonEventListItem
    {
        private LineAndText MainModel { get; }

        public int Value => MainModel.Line;

        public string DisplayText => $"{MainModel.Line:D4} | {MainModel.Text}";

        public CommonEventListItem(int line, string text)
        {
            MainModel = new LineAndText(line, text);
        }
    }
}
