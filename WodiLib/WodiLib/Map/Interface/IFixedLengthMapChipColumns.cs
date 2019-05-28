using WodiLib.Sys;

namespace WodiLib.Map
{
    /// <summary>
    /// 長さ固定マップチップリスト
    /// </summary>
    public interface IFixedLengthMapChipColumns : IFixedLengthCollection<MapChip>
    {
        /// <summary>
        /// マップチップ情報を初期化する。
        /// </summary>
        void InitializeChips();

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <returns>バイナリデータ</returns>
        byte[] ToBinary();
    }
}