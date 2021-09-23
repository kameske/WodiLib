using System;
using System.Collections.Generic;
using System.IO;
using WodiLib.Map;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO
{
    public static class TileSetFileDataTestItemGenerator
    {
        #region CreateTileSetFileData

        /**
         * ========================================
         * タイルセットデータオブジェクト作成
         * ========================================
         */
        public static TileSetFileData GenerateData0()
        {
            return new TileSetFileData
            {
                TileSetSetting = GenerateSettingData0()
            };
        }

        public static TileSetFileData GenerateData1()
        {
            return new TileSetFileData
            {
                TileSetSetting = GenerateSettingData1()
            };
        }

        #endregion

        #region CreateTileSetSetting

        public static TileSetSetting GenerateSettingData0()
        {
            return new TileSetSetting(new TileTagNumberList(
                    new Func<List<TileTagNumber>>(() =>
                    {
                        var result = new List<TileTagNumber> { 1 };
                        for (byte i = 1; i <= 15; i++)
                        {
                            result.Add(i);
                        }

                        for (var i = 0; i < 2008 - 16; i++)
                        {
                            result.Add(0);
                        }

                        return result;
                    })()), new TilePathSettingList(new Func<List<TilePathSetting>>(() =>
                {
                    var result = new List<TilePathSetting>
                    {
                        new(0x8f),
                        new(0x80),
                        new(0x98),
                        new(0x94),
                        new(0x01_81),
                        new(0x01_02),
                        new(0x02_0C),
                        new(0x02_03),
                        new(0x40),
                        new(0x40),
                        new(0x02_00),
                        new(0x0F),
                        new(0x02_10),
                        new(0x03_00),
                        new(0x02_00),
                        new(0x02_40),
                        new(0x28),
                        new(0x2A),
                        new(0x21),
                        new(0x26),
                        new(0x23),
                        new(0x00),
                        new(0x00),
                        new(0x80)
                    };

                    for (var i = 0; i < 2008 - 24; i++)
                    {
                        result.Add(new TilePathSetting(0x00));
                    }

                    return result;
                })()), new AutoTileFileNameList(new Func<List<AutoTileFileName>>(() =>
                {
                    var result = new List<AutoTileFileName>
                    {
                        "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "LastAutoTile"
                    };

                    return result;
                })())
            )
            {
                Name = "設定名",
                BaseTileSetFileName = "MapChip/test.png"
            };
        }

        public static TileSetSetting GenerateSettingData1()
        {
            return new TileSetSetting(new TileTagNumberList(
                new Func<List<TileTagNumber>>(() =>
                {
                    var result = new List<TileTagNumber>();
                    for (byte i = 0; i <= 15; i++)
                    {
                        result.Add(i);
                    }

                    for (var i = 0; i < 24 - 16; i++)
                    {
                        result.Add(0);
                    }

                    return result;
                })()), new TilePathSettingList(new Func<List<TilePathSetting>>(() =>
            {
                var result = new List<TilePathSetting>();
                for (var i = 0; i < 24; i++)
                {
                    result.Add(new TilePathSetting(0x00));
                }

                return result;
            })()), new AutoTileFileNameList(new Func<List<AutoTileFileName>>(() =>
            {
                var result = new List<AutoTileFileName>
                {
                    "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", ""
                };

                return result;
            })()))
            {
                Name = "aaa",
                BaseTileSetFileName = ""
            };
        }

        #endregion

        #region OutputTestFile

        /// <summary>テストディレクトリルート</summary>
        public static string TestWorkRootDir => $@"{Path.GetTempPath()}WodiLibTest";

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            (@"000_設定名.tile", TestResources.title000_設定名),
            (@"001_aaa.tile", TestResources.title001_aaa),
            (@"000_街.tile", TestResources.title000_街)
        };

        /// <summary>
        ///     テストデータファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestWorkRootDir.CreateDirectoryIfNeed();

            foreach (var (fileName, bytes) in TestFiles)
            {
                using (var fs = new FileStream(MakeFileFullPath(fileName), FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        /// <summary>
        ///     テストデータファイルを削除する。
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
