using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Ini;
using WodiLib.IO;
using WodiLib.Map;
using WodiLib.Project;
using WodiLibSample.Component;
using WodiLibSample.Model;
using WodiLibSample.Resources;

namespace WodiLibSample
{
    public partial class Form1 : Form
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Property
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        private WoditorProject project;

        private WoditorProject Project
        {
            get => project;
            set
            {
                project = value;

                UpdateBtnMapUpdateEnabled(value != null);
                UpdateBtnCmnUpdateEnabled(value != null);

                if (value == null) return;

                LoadCommonEventList();
                LoadMapList();

                {       // プロジェクト内のイベントすべてに対してイベントコマンド文を取得する
                    {       // Map
                        foreach (var dataDesc in value.GetDatabaseDataDescList(DBKind.System, 0))
                        {
                            string mapFilePath = dataDesc.ItemValueList[0].StringValue;
                            mapFilePath = $@"Data\{mapFilePath}";
                            var mapData = value.ReadMpsFileSync(mapFilePath);

                            foreach (var mapEvent in mapData.MapEvents)
                            {
                                for(var page = 0; page < mapEvent.PageValue; page++)
                                {
                                    var sentenceInfo = value.GetMapEventEventCommandSentenceInfoListSync(mapFilePath, mapEvent.MapEventId,
                                        page);
                                }
                            }
                        }
                    }
                    {       // Common
                        foreach (var commonEvent in value.CommonEventList)
                        {
                            var sentenceInfo = value.GetCommonEventEventCommandSentenceInfoListSync(commonEvent.Id);
                        }
                    }
                    Console.WriteLine("すべてのマップ/コモンイベントをイベントコマンド文に変換できました。");
                }
            }
        }

        private MapData selectedMapData { get; set; }

        private MapData SelectedMapData
        {
            get => selectedMapData;
            set
            {
                selectedMapData = value;

                UpdateBtnMapEventUpdateEnabled(value != null);

                if (value == null) return;

                LoadMapEventList();
            }
        }

        private MpsFilePath SelectedMapFilePath { get; set; }

        private MapEventId? SelectedMapEventId { get; set; }

        private CommonEventId? SelectedCommonEventId { get; set; }

        private List<CommonEventListItem> commonEventList = new List<CommonEventListItem>();

        private List<CommonEventListItem> CommonEventList
        {
            get => commonEventList;
            set
            {
                commonEventList = value;
                UpdateCommonEventList();
            }
        }

        private bool ShowEventCodeFlag { get; set; }


        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Constructor
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        public Form1()
        {
            InitializeComponent();
            InitializeComponentState();
        }

        private void InitializeComponentState()
        {
            btnMapUpdate.Enabled = false;
            btnMapEventUpdate.Enabled = false;

            btnCmnUpdate.Enabled = false;
            btnChangeSentenceCode.Enabled = false;

            cmbEventCodeColor.SelectedIndex = 0;
            cmbEventCodeColor.Enabled = false;

            lblState.Text = string.Empty;
        }

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Event
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        /// <summary>
        /// フォームロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            lstCommonEvent.DataSource = CommonEventList;
        }

        #region ProjectInfo

        /// <summary>
        /// サンプルプロジェクト読み込みボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnOpenSampleProj_Click(object sender, EventArgs e)
        {
            // Resourceに含まれるサンプルプロジェクトを解凍して読み込む。
            var sampleProjectRootDir = await UnzipSampleProject();
            await LoadProject(sampleProjectRootDir);
        }

        /// <summary>
        /// フォルダ参照ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnOpenDirDialog_Click(object sender, EventArgs e)
        {
            // 選択されたフォルダをルートとしてプロジェクトを読み込む
            var dialog = new FolderOpenDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var selectedDir = dialog.Path;
                try
                {
                    await LoadProject(selectedDir);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show(
                        Form1Resource.ErrorProjectLoad,
                        Form1Resource.DialogTitle_Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Map

        /// <summary>
        /// マップ一覧ダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LstMap_DoubleClick(object sender, EventArgs e)
        {
            UpdateStateMessage(Form1Resource.LoadingMapFile);

            // 選択した情報をクリアする
            txtShow.Text = string.Empty;
            SelectedMapData = null;
            SelectedMapEventId = null;
            SelectedCommonEventId = null;

            // 選択したマップの情報を取得する
            var selectedMapItem = (DatabaseDataDesc) lstMap.SelectedItem;
            // マップファイルパスを取得する
            var mpsFileName = selectedMapItem.ItemValueList[0].StringValue.ToString();
            if (!mpsFileName.StartsWith(@"Data\"))
            {
                mpsFileName = $@"Data\{mpsFileName}";
            }

            SelectedMapFilePath = mpsFileName;

            // マップファイルを読み込む
            SelectedMapData = await Project.ReadMpsFileAsync(mpsFileName, false);

            ClearStateMessage();
        }

        /// <summary>
        /// マップ一覧更新ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnMapUpdate_Click(object sender, EventArgs e)
        {
            UpdateStateMessage(Form1Resource.LoadingMapList);

            // 選択した情報をクリアする
            txtShow.Text = string.Empty;
            SelectedMapData = null;
            SelectedMapFilePath = null;
            SelectedMapEventId = null;
            SelectedCommonEventId = null;

            if (Project == null) return;

            // システムDBを再読込する
            await Project.ReadSystemDatabaseAsync();
            LoadMapList();

            ClearStateMessage();
        }

        #endregion

        #region MapEvent

        /// <summary>
        /// マップイベント一覧ダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LstMapEvent_DoubleClick(object sender, EventArgs e)
        {
            UpdateStateMessage(Form1Resource.LoadingMapEvent);

            // 選択したマップイベントの情報を取得する
            SelectedMapEventId = ((MapEvent)lstMapEvent.SelectedItem).MapEventId;

            await SetSelectedMapEventString();

            ClearStateMessage();
        }

        /// <summary>
        /// マップイベント一覧更新ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnMapEventUpdate_Click(object sender, EventArgs e)
        {
            UpdateStateMessage(Form1Resource.LoadingMapEventList);

            // 選択した情報をクリアする
            txtShow.Text = string.Empty;
            SelectedMapEventId = null;
            ClearGrpSelectedEvent();

            if (Project == null) return;

            // コモンイベントデータを再読込する
            await Project.ReadCommonEventDataAsync();
            LoadCommonEventList();

            ClearStateMessage();

        }

        #endregion

        #region CommonEvent

        /// <summary>
        /// コモンイベントリストダブルクリック時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LstCommonEvent_DoubleClick(object sender, EventArgs e)
        {
            // 選択した情報をクリアする
            txtShow.Text = string.Empty;
            SelectedMapData = null;
            SelectedMapFilePath = null;
            SelectedMapEventId = null;
            SelectedCommonEventId = null;

            // 選択されたコモンイベントのイベントコマンド文またはイベントコードを表示する
            await SetSelectedCommonEventString();
        }

        /// <summary>
        /// コモンイベントリスト更新ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnCmnUpdate_Click(object sender, EventArgs e)
        {
            UpdateStateMessage(Form1Resource.LoadingCommonEventList);

            // 選択した情報をクリアする
            txtShow.Text = string.Empty;
            SelectedCommonEventId = null;

            if (Project == null) return;

            // コモンイベントデータを再読込する
            await Project.ReadCommonEventDataAsync();
            LoadCommonEventList();

            ClearStateMessage();
        }

        #endregion

        #region Output

        /// <summary>
        /// イベントコマンド文 / イベントコード切り替えボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnChangeSentenceCode_Click(object sender, EventArgs e)
        {
            // 表示フラグ切り替え
            ShowEventCodeFlag = !ShowEventCodeFlag;

            // 表示内容更新
            if (SelectedMapEventId != null)
            {
                await SetSelectedMapEventString();
            }
            else
            {
                await SetSelectedCommonEventString();
            }
        }

        /// <summary>
        /// イベントコマンド配色切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CmbEventCodeColor_SelectedValueChanged(object sender, EventArgs e)
        {
            // イベントコマンド文が表示されていないときは表示内容の更新をしない
            if (SelectedMapEventId == null && SelectedCommonEventId == null) return;
            if (!ShowEventCodeFlag) return;

            // 表示内容更新
            if (SelectedMapEventId != null)
            {
                await SetSelectedMapEventString();
            }
            else
            {
                await SetSelectedCommonEventString();
            }
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     UI
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region ProjectInfo

        /// <summary>
        /// 読み込んだプロジェクトパスを更新する。
        /// </summary>
        /// <param name="dir"></param>
        private void UpdateProjectDirText(string dir)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateProjectDirText(dir)));
                return;
            }
            txtProjectDir.Text = dir;
        }

        #endregion

        #region Map

        /// <summary>
        /// マップ一覧を更新する。
        /// </summary>
        private void UpdateMapList()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)UpdateMapList);
                return;
            }

            var mapList = Project.GetDatabaseDataDescList(DBKind.System, 0);

            // マップ情報リストをListBoxに表示
            lstMap.DataSource = mapList.ToList(); // IList を得るためにあえてList<T>型に変換
            lstMap.DisplayMember = nameof(DatabaseDataDesc.DataName);
            lstMap.ValueMember = nameof(DatabaseDataDesc.ItemValueList);

            // マップイベント一覧をクリア
            lstMapEvent.ClearSelected();
            lstMapEvent.DataSource = null;
        }

        /// <summary>
        /// マップ一覧更新ボタン有効/無効をセットする。
        /// </summary>
        /// <param name="enabled"></param>
        private void UpdateBtnMapUpdateEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateBtnMapUpdateEnabled(enabled)));
                return;
            }
            btnMapUpdate.Enabled = enabled;
        }

        #endregion

        #region MapEvent

        /// <summary>
        /// マップイベント一覧を更新する。
        /// </summary>
        private void UpdateMapEventList()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)UpdateMapEventList);
                return;
            }

            lstMapEvent.DataSource = SelectedMapData.MapEvents.ToList();
            lstMapEvent.DisplayMember = nameof(MapEvent.EventName);
            lstMapEvent.ValueMember = nameof(MapEvent.MapEventId);
        }

        /// <summary>
        /// マップイベント一覧更新ボタン有効/無効をセットする。
        /// </summary>
        /// <param name="enabled"></param>
        private void UpdateBtnMapEventUpdateEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateBtnMapEventUpdateEnabled(enabled)));
                return;
            }
            btnMapEventUpdate.Enabled = enabled;
        }

        #endregion

        #region CommonEvent

        /// <summary>
        /// コモンイベント一覧を更新する。
        /// </summary>
        private void UpdateCommonEventList()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)UpdateCommonEventList);
                return;
            }

            lstCommonEvent.DataSource = CommonEventList;
            lstCommonEvent.DisplayMember = nameof(CommonEventListItem.DisplayText);
            lstCommonEvent.ValueMember = nameof(CommonEventListItem.Value);
        }

        /// <summary>
        /// コモンイベント一覧更新ボタン有効/無効をセットする。
        /// </summary>
        /// <param name="enabled"></param>
        private void UpdateBtnCmnUpdateEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateBtnCmnUpdateEnabled(enabled)));
                return;
            }
            btnCmnUpdate.Enabled = enabled;
        }

        #endregion

        #region SelectedEventInfo

        private void UpdateGrpSelectedEvent(CommonEvent commonEvent)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateGrpSelectedEvent(commonEvent)));
                return;
            }

            ClearGrpSelectedEvent();

            var component = new CommonEventSettingView();
            component.Location = new Point(10, 16);
            component.SetCommonEvent(commonEvent);
            grpSelectEvent.Controls.Add(component);
        }

        private void ClearGrpSelectedEvent()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)ClearGrpSelectedEvent);
                return;
            }

            grpSelectEvent.Controls.Clear();
        }

        #endregion

        #region Output

        /// <summary>
        /// イベントコマンド文/コード切り替えボタン有効/無効をセットする。
        /// </summary>
        /// <param name="enabled"></param>
        private void UpdateBtnChangeSentenceCodeEnabled(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateBtnChangeSentenceCodeEnabled(enabled)));
                return;
            }
            btnChangeSentenceCode.Enabled = enabled;
        }

        /// <summary>
        /// 表示テキストを更新する。
        /// </summary>
        /// <param name="text">表示文字列</param>
        private void UpdateTxtShowText(string text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateTxtShowText(text)));
                return;
            }

            txtShow.Text = text;
        }

        /// <summary>
        /// 表示テキストを更新する。
        /// </summary>
        /// <param name="infoList">表示コード情報リスト</param>
        private void UpdateTxtShowText(IEnumerable<EventCodeInfo> infoList)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateTxtShowText(infoList)));
                return;
            }

            txtShow.Text = string.Empty;

            foreach (var info in infoList)
            {
                txtShow.SelectionColor = Color.Black;
                txtShow.AppendText($"{info.LineNo:D4} | ");
                txtShow.SelectionColor = info.StringColor;
                txtShow.AppendText(info.Code);
                txtShow.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// コマンド配色コンボボックス有効/無効をセットする。
        /// </summary>
        /// <param name="enable"></param>
        private void UpdateCmbEventCodeColorEnable(bool enable)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateCmbEventCodeColorEnable(enable)));
                return;
            }

            cmbEventCodeColor.Enabled = enable;
        }

        #endregion

        #region State

        /// <summary>
        /// 状態メッセージを更新する。
        /// </summary>
        /// <param name="message"></param>
        private void UpdateStateMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => UpdateStateMessage(message)));
                return;
            }
            lblState.Text = message;
        }

        /// <summary>
        /// 状態メッセージをクリアする。
        /// </summary>
        private void ClearStateMessage()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(ClearStateMessage));
                return;
            }
            UpdateStateMessage(string.Empty);
        }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        //     Private Method
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        // -------------------- Zip File --------------------

        /// <summary>
        /// SampleResourceに登録済みのテスト用プロジェクトファイル（ZIP）を
        /// Tmpフォルダに解凍する。
        /// </summary>
        /// <returns>解凍したフォルダ</returns>
        private static async Task<string> UnzipSampleProject()
        {
            return await Task.Run(() =>
            {
                // サンプルプロジェクトはテストプロジェクトで使用するフォルダと同じフォルダに出力
                var outputDir = $@"{Path.GetTempPath()}WodiLibTest";
                var zipFileFullPath = $@"{outputDir}\TestProject_Ver224.zip";
                var zipData = SampleResource.TestProject_Ver224;

                using (var fs = new FileStream(zipFileFullPath, FileMode.Create))
                {
                    fs.Write(zipData, 0, zipData.Length);
                }

                try
                {
                    using (var zip = ZipFile.Open(zipFileFullPath, ZipArchiveMode.Read))
                    {
                        zip.ExtractToDirectory(outputDir);
                    }
                }
                catch
                {
                    // すでに存在する場合は何もしない
                }

                File.Delete(zipFileFullPath);

                return zipFileFullPath.Replace(".zip", "");
            });
        }

        // -------------------- Project Data --------------------

        private async Task LoadProject(string projectRootDir)
        {
            UpdateStateMessage(Form1Resource.ProjectLoading);
            await Task.Run(() => { Project = new WoditorProject(projectRootDir); });
            UpdateProjectDirText(projectRootDir);
            ClearStateMessage();
        }

        /// <summary>
        /// 読み込んだプロジェクトからマップ一覧を取得し、画面にセットする。
        /// </summary>
        private void LoadMapList()
        {
            UpdateStateMessage(Form1Resource.LoadingCommonEventList);

            UpdateMapList();

            ClearStateMessage();
        }

        /// <summary>
        /// 選択したマップデータからイベント一覧を取得し、画面にセットする。
        /// </summary>
        private void LoadMapEventList()
        {
            UpdateStateMessage(Form1Resource.LoadingMapEventList);

            UpdateMapEventList();

            ClearStateMessage();
        }

        /// <summary>
        /// 読み込んだプロジェクトからコモンイベントの情報を取得し、画面にセットする。
        /// </summary>
        private void LoadCommonEventList()
        {
            UpdateStateMessage(Form1Resource.LoadingCommonEventList);

            CommonEventList = Project.CommonEventList
                .Select((x, idx) => new CommonEventListItem(idx, x.Name))
                .ToList();

            ClearStateMessage();
        }

        /// <summary>
        /// 選択したマップイベントの文字列をセットする。
        /// </summary>
        /// <returns></returns>
        private async Task SetSelectedMapEventString()
        {
            if (SelectedMapEventId == null) throw new InvalidOperationException();
            if (ShowEventCodeFlag) await SetMapEventSentence(SelectedMapEventId.Value);
            else await SetMapEventCode(SelectedMapEventId.Value);

            UpdateBtnChangeSentenceCodeEnabled(true);
        }

        /// <summary>
        /// マップイベントのイベントコマンド文を表示する。
        /// </summary>
        /// <param name="id">マップイベントID</param>
        private async Task SetMapEventSentence(int id)
        {
            UpdateStateMessage(string.Format(Form1Resource.LoadingMapEventCommandSentence, id));

            var typeCode = cmbEventCodeColor.SelectedIndex != 2
                ? CommandColorType.FromCode(cmbEventCodeColor.SelectedIndex.ToString())
                : CommandColorType.Old;

            var codeInfos = await Task.Run(() =>
            {
                return Project.GetMapEventEventCommandSentenceInfoListSync(SelectedMapFilePath, id, 0)
                    .Select((x, idx) =>
                    {
                        var color = x.GetCommandColor(typeCode);
                        // sentence中に改行コードを含んでいる可能性があるので、除去して表示。
                        //   なお、WodiLibが改行コードを含んだ状態でコマンド文を出力する動作は正しい。
                        var sentence = x.Sentence.ToString()
                            .Replace("\r", "")
                            .Replace("\n", "");
                        return new EventCodeInfo(idx, sentence, color);
                    });
            });
            UpdateTxtShowText(codeInfos);
            UpdateCmbEventCodeColorEnable(true);

            ClearStateMessage();
        }

        /// <summary>
        /// マップイベントのイベントコマンドコードを表示する。
        /// </summary>
        /// <param name="id">マップイベントID</param>
        private async Task SetMapEventCode(int id)
        {
            UpdateStateMessage(string.Format(Form1Resource.LoadingMapEventCode, id));

            UpdateCmbEventCodeColorEnable(false);

            var sentences = await Task.Run(() =>
            {
                return SelectedMapData.MapEvents[id].MapEventPageList[0].EventCommands
                    .Select((x, idx) => $"{idx:D4} | {x.ToEventCodeString()}");
            });
            UpdateTxtShowText(string.Join(Environment.NewLine, sentences));

            ClearStateMessage();
        }

        /// <summary>
        /// 選択したコモンイベントの文字列をセットする。
        /// </summary>
        private async Task SetSelectedCommonEventString()
        {
            var id = ((CommonEventListItem)lstCommonEvent.SelectedItem).Value;

            SelectedCommonEventId = id;

            if (ShowEventCodeFlag) await SetCommonEventSentence(id);
            else await SetCommonEventCode(id);

            var selectedCommonEvent = Project.GetCommonEvent(id);
            UpdateGrpSelectedEvent(selectedCommonEvent);

            UpdateBtnChangeSentenceCodeEnabled(true);
        }

        /// <summary>
        /// コモンイベントのイベントコマンド文を表示する。
        /// </summary>
        /// <param name="id">コモンイベントID</param>
        private async Task SetCommonEventSentence(int id)
        {
            UpdateStateMessage(string.Format(Form1Resource.LoadingCommonEventCommandSentence, id));

            var typeCode = cmbEventCodeColor.SelectedIndex != 2
                ? CommandColorType.FromCode(cmbEventCodeColor.SelectedIndex.ToString())
                : CommandColorType.Old;

            var codeInfos = await Task.Run(() =>
            {
                return Project.GetCommonEventEventCommandSentenceInfoListSync(id)
                    .Select((x, idx) =>
                    {
                        var color = x.GetCommandColor(typeCode);
                        // sentence中に改行コードを含んでいる可能性があるので、除去して表示。
                        //   なお、WodiLibが改行コードを含んだ状態でコマンド文を出力する動作は正しい。
                        var sentence = x.Sentence.ToString()
                            .Replace("\r", "")
                            .Replace("\n", "");
                        return new EventCodeInfo(idx, sentence, color);
                    });
            });
            UpdateTxtShowText(codeInfos);
            UpdateCmbEventCodeColorEnable(true);

            ClearStateMessage();
        }

        /// <summary>
        /// コモンイベントのイベントコマンドコードを表示する。
        /// </summary>
        /// <param name="id">コモンイベントID</param>
        private async Task SetCommonEventCode(int id)
        {
            UpdateStateMessage(string.Format(Form1Resource.LoadingCommonEventCode, id));

            UpdateCmbEventCodeColorEnable(false);

            var sentences = await Task.Run(() =>
            {
                return Project.GetEventCodeStringList(id)
                    .Select((x, idx) => $"{idx:D4} | {x}");
            });
            UpdateTxtShowText(string.Join(Environment.NewLine, sentences));

            ClearStateMessage();
        }
    }
}
