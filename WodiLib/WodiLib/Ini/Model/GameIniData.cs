using WodiLib.Sys;

namespace WodiLib.Ini
{
    /// <summary>
    /// Game.iniデータクラス
    /// </summary>
    public class GameIniData
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Const
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>BGM再生コード値</summary>
        private const int UseBgmCode = 2;

        /// <summary>SE再生コード値</summary>
        private const int UseSeCode = 1;

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Public Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// Game.exe起動済み種別コード
        /// </summary>
        public int StartCode { get; set; }

        /// <summary>
        /// ソフトウェア表示モードフラグ
        /// </summary>
        public bool IsSoftGraphicMode { get; set; }

        /// <summary>
        /// ウィンドウモードフラグ
        /// </summary>
        public bool IsWindowMode { get; set; }

        /// <summary>
        /// BGM再生フラグ
        /// </summary>
        public bool IsPlayBgm { get; set; }

        /// <summary>
        /// SE再生フラグ
        /// </summary>
        public bool IsPlaySe { get; set; }

        private FrameSkipType frameSkipType = Ini.FrameSkipType.HighSpec;

        /// <summary>
        /// [NotNull] フレームスキップ種別
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public FrameSkipType FrameSkipType
        {
            get => frameSkipType;
            set
            {
                if(value == null) throw new PropertyNullException(
                    ErrorMessage.NotNull(nameof(FrameSkipType)));

                frameSkipType = value;
            }
        }

        private ProxyAddress proxyAddress = "";

        /// <summary>
        /// [NotNull] プロキシアドレス
        /// </summary>
        /// <exception cref="PropertyNullException">nullをセットした場合</exception>
        public ProxyAddress ProxyAddress
        {
            get => proxyAddress;
            set
            {
                if(value == null) throw new PropertyNullException(
                    ErrorMessage.NotNull(nameof(ProxyAddress)));

                proxyAddress = value;
            }
        }

        /// <summary>
        /// プロキシポート
        /// </summary>
        public ProxyPort ProxyPort { get; set; }

        /// <summary>
        /// スクリーンショット許可フラグ
        /// </summary>
        public bool CanTakeScreenShot { get; set; }

        /// <summary>
        /// 【Ver2.20以降】F12リセット許可フラグ
        /// </summary>
        public bool CanReset { get; set; }

        /// <summary>
        /// 【Ver2.20以降】起動ディスプレイ番号
        /// </summary>
        public DisplayNumber DisplayNumber { get; set; }

        /// <summary>
        /// 【Ver2.22以降】旧DirectXバージョン利用フラグ
        /// </summary>
        public bool IsUseOldDirectX { get; set; }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameIniData()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="data">Iniデータ</param>
        internal GameIniData(GameIniRootData data)
        {
            StartCode = data.Start.TryToInt() ?? 0;
            IsSoftGraphicMode = data.SoftModeFlag.TryToInt() == 1;
            IsWindowMode = data.WindowModeFlag.TryToInt() == 1;
            var seAndBgm = data.SEandBGM.TryToInt() ?? 0;
            IsPlayBgm = (seAndBgm & UseBgmCode) != 0;
            IsPlaySe = (seAndBgm & UseSeCode) != 0;
            try
            {
                FrameSkipType = FrameSkipType.FromCode(data.FrameSkip);
            }
            catch
            {
                FrameSkipType = FrameSkipType.HighSpec;
            }
            ProxyAddress = data.Proxy ?? "";
            ProxyPort = data.ProxyPort.TryToInt() ?? ProxyPort.Empty;
            CanTakeScreenShot = data.ScreenShotFlag.TryToInt() == 1;
            CanReset = data.F12_Reset.TryToInt() == 1;
            DisplayNumber = data.Display_Number.TryToInt() ?? 0;
            IsUseOldDirectX = data.Display_Number.TryToInt() == 1;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Internal Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// 自身のプロパティ値をGameIniRootDataインスタンスに変換する。
        /// </summary>
        /// <returns>GameIniRootDataインスタンス</returns>
        internal GameIniRootData ToIniRootData()
        {
            return new GameIniRootData
            {
                Start = StartCode.ToString(),
                SoftModeFlag = IsSoftGraphicMode.ToIntString(),
                WindowModeFlag = IsWindowMode.ToIntString(),
                SEandBGM = (( IsPlayBgm ? UseBgmCode : 0 )
                    + (IsPlaySe ? UseSeCode : 0)).ToString(),
                FrameSkip = FrameSkipType.Code,
                Proxy = ProxyAddress.ToString(),
                ProxyPort = ProxyPort.ToString(),
                ScreenShotFlag = CanTakeScreenShot.ToIntString(),
                F12_Reset = CanReset.ToIntString(),
                Display_Number = DisplayNumber.ToString(),
                Old_DirectX_Use = IsUseOldDirectX.ToIntString(),
            };
        }
    }
}