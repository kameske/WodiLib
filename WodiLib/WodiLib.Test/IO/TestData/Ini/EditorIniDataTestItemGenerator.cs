using System.Collections.Generic;
using System.IO;
using WodiLib.Ini;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class EditorIniDataTestItemGenerator
    {
        #region CreateEditorIniData

        /** ========================================
         *  EditorIniオブジェクト作成
         *  ======================================== */

        public static EditorIniData GenerateData0()
        {
            return new EditorIniData
            {
                StartFlag =  0,
                LastLoadFile = "MapData/Map000.mps",
                MainWindowPosition = (0, 0),
                MainWindowSize = (651, 322),
                MapChipWindowPosition = (0, 0),
                MapEventWindowPosition = (273, 230),
                MapEventWindowSize = (840, 410),
                MapEventInputWindowPosition = (0,0),
                CommonEventWindowPosition = (0, 0),
                CommonEventWindowSize = (800, 640),
                CommonEventInputWindowPosition = (0, 0),
                UserDbWindowPosition = (27, 54),
                ChangeableDbWindowPosition = (27, 54),
                SystemDbWindowPosition = (27, 54),
                DatabaseValueNumberDrawType = DatabaseValueNumberDrawType.FromCode("0"),
                EditTimeDrawType = EditTimeDrawType.On,
                EditTime=14,
                NotEditTime=0,
                IsShowDebugWindow = true,
                LayerTransparent = LaterTransparentType.FromCode("2"),
                EventLayerOpacity = EventLayerOpacityType.FromCode("1"),
                CommandColorType = CommandColorType.FromCode("0"),
                IsDrawBackgroundImage = true,
                NotCopyExtList = new ExtensionList(new Extension[]
                {
                    ".psd", ".sai", ".svg", ".xls", ".db", ".tmp",
                    ".bak", ".db", "dummy_file"
                }),
                CommandViewType = 0,
                BackupType = ProjectBackupType.FromCode("3"),
                ShortCutKeyList = new EventCommandShortCutKeyList(new[]
                {
                    EventCommandShortCutKey.One, EventCommandShortCutKey.Two, EventCommandShortCutKey.Three,
                    EventCommandShortCutKey.Four, EventCommandShortCutKey.Five, EventCommandShortCutKey.Six,
                    EventCommandShortCutKey.Seven, EventCommandShortCutKey.Eight, EventCommandShortCutKey.Nine,
                    EventCommandShortCutKey.A, EventCommandShortCutKey.B, EventCommandShortCutKey.C,
                    EventCommandShortCutKey.D, EventCommandShortCutKey.E, EventCommandShortCutKey.F,
                    EventCommandShortCutKey.G, EventCommandShortCutKey.H, EventCommandShortCutKey.I,
                    EventCommandShortCutKey.J, EventCommandShortCutKey.One, EventCommandShortCutKey.One,
                    EventCommandShortCutKey.One, EventCommandShortCutKey.One, EventCommandShortCutKey.One,
                    EventCommandShortCutKey.One, EventCommandShortCutKey.One, EventCommandShortCutKey.One,
                    EventCommandShortCutKey.One, EventCommandShortCutKey.One, EventCommandShortCutKey.One,
                }),
                CommandPositionList = new ShortCutPositionList(new ShortCutPosition[]
                {
                    1, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1,
                    17, 18, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0,
                }),
                IsUseExpertCommand = false,
            };
        }

        #endregion

        #region OutputTestFile

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => $@"{Path.GetTempPath()}WodiLibTest";

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            (@"Dir0\Editor.ini", TestResources.EditorIni0),
            (@"Dir1\Editor.ini", TestResources.EditorIni1),
        };

        /// <summary>
        /// テストデータファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestWorkRootDir.CreateDirectoryIfNeed();

            foreach (var (fileName, bytes) in TestFiles)
            {
                var filePath = MakeFileFullPath(fileName);
                Path.GetDirectoryName(filePath).CreateDirectoryIfNeed();

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        /// テストデータファイルを削除する。
        /// </summary>
        public static void DeleteFile()
        {
            foreach (var (fileName, _) in TestFiles)
            {
                var fileFullPath = MakeFileFullPath(fileName);
                if (!File.Exists(fileFullPath)) continue;
                try
                {
                    File.Delete(fileFullPath);
                }
                catch
                {
                    // 削除に失敗しても何もしない
                }
            }
        }

        private static string MakeFileFullPath(string fileName)
        {
            return $@"{TestWorkRootDir}\{fileName}";
        }

        #endregion
    }
}