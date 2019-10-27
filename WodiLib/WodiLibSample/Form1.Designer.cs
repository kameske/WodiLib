namespace WodiLibSample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenSampleProj = new System.Windows.Forms.Button();
            this.btnOpenDirDialog = new System.Windows.Forms.Button();
            this.txtProjectDir = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstMap = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMapUpdate = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstMapEvent = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMapEventUpdate = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstCommonEvent = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCmnUpdate = new System.Windows.Forms.Button();
            this.btnChangeSentenceCode = new System.Windows.Forms.Button();
            this.txtShow = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblState = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmbEventCodeColor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grpSelectEvent = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOpenSampleProj);
            this.groupBox1.Controls.Add(this.btnOpenDirDialog);
            this.groupBox1.Controls.Add(this.txtProjectDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "読み込むプロジェクト";
            // 
            // btnOpenSampleProj
            // 
            this.btnOpenSampleProj.Location = new System.Drawing.Point(10, 46);
            this.btnOpenSampleProj.Name = "btnOpenSampleProj";
            this.btnOpenSampleProj.Size = new System.Drawing.Size(151, 23);
            this.btnOpenSampleProj.TabIndex = 1;
            this.btnOpenSampleProj.Text = "サンプルプロジェクトを開く";
            this.btnOpenSampleProj.UseVisualStyleBackColor = true;
            this.btnOpenSampleProj.Click += new System.EventHandler(this.BtnOpenSampleProj_Click);
            // 
            // btnOpenDirDialog
            // 
            this.btnOpenDirDialog.Location = new System.Drawing.Point(228, 46);
            this.btnOpenDirDialog.Name = "btnOpenDirDialog";
            this.btnOpenDirDialog.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDirDialog.TabIndex = 2;
            this.btnOpenDirDialog.Text = "参照...";
            this.btnOpenDirDialog.UseVisualStyleBackColor = true;
            this.btnOpenDirDialog.Click += new System.EventHandler(this.BtnOpenDirDialog_Click);
            // 
            // txtProjectDir
            // 
            this.txtProjectDir.Location = new System.Drawing.Point(10, 18);
            this.txtProjectDir.Name = "txtProjectDir";
            this.txtProjectDir.ReadOnly = true;
            this.txtProjectDir.Size = new System.Drawing.Size(293, 19);
            this.txtProjectDir.TabIndex = 0;
            this.txtProjectDir.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 93);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(309, 332);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(301, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "マップイベント";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(301, 306);
            this.splitContainer1.SplitterDistance = 152;
            this.splitContainer1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.lstMap);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnMapUpdate);
            this.groupBox2.Location = new System.Drawing.Point(0, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 146);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "マップ一覧";
            // 
            // lstMap
            // 
            this.lstMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstMap.FormattingEnabled = true;
            this.lstMap.ItemHeight = 12;
            this.lstMap.Location = new System.Drawing.Point(6, 18);
            this.lstMap.Name = "lstMap";
            this.lstMap.Size = new System.Drawing.Size(283, 88);
            this.lstMap.TabIndex = 3;
            this.lstMap.DoubleClick += new System.EventHandler(this.LstMap_DoubleClick);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(172, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "ダブルクリックでマップデータ読み込み";
            // 
            // btnMapUpdate
            // 
            this.btnMapUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMapUpdate.Location = new System.Drawing.Point(10, 120);
            this.btnMapUpdate.Name = "btnMapUpdate";
            this.btnMapUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnMapUpdate.TabIndex = 1;
            this.btnMapUpdate.Text = "リスト更新";
            this.btnMapUpdate.UseVisualStyleBackColor = true;
            this.btnMapUpdate.Click += new System.EventHandler(this.BtnMapUpdate_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.lstMapEvent);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnMapEventUpdate);
            this.groupBox3.Location = new System.Drawing.Point(0, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(295, 141);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "マップイベント一覧";
            // 
            // lstMapEvent
            // 
            this.lstMapEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstMapEvent.FormattingEnabled = true;
            this.lstMapEvent.ItemHeight = 12;
            this.lstMapEvent.Location = new System.Drawing.Point(6, 18);
            this.lstMapEvent.Name = "lstMapEvent";
            this.lstMapEvent.Size = new System.Drawing.Size(283, 88);
            this.lstMapEvent.TabIndex = 4;
            this.lstMapEvent.DoubleClick += new System.EventHandler(this.LstMapEvent_DoubleClick);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "ダブルクリックでマップデータ読み込み";
            // 
            // btnMapEventUpdate
            // 
            this.btnMapEventUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMapEventUpdate.Location = new System.Drawing.Point(10, 115);
            this.btnMapEventUpdate.Name = "btnMapEventUpdate";
            this.btnMapEventUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnMapEventUpdate.TabIndex = 1;
            this.btnMapEventUpdate.Text = "マップ再読込";
            this.btnMapEventUpdate.UseVisualStyleBackColor = true;
            this.btnMapEventUpdate.Click += new System.EventHandler(this.BtnMapEventUpdate_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstCommonEvent);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btnCmnUpdate);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(301, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "コモンイベント";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstCommonEvent
            // 
            this.lstCommonEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstCommonEvent.FormattingEnabled = true;
            this.lstCommonEvent.ItemHeight = 12;
            this.lstCommonEvent.Location = new System.Drawing.Point(6, 6);
            this.lstCommonEvent.Name = "lstCommonEvent";
            this.lstCommonEvent.Size = new System.Drawing.Size(283, 268);
            this.lstCommonEvent.TabIndex = 0;
            this.lstCommonEvent.DoubleClick += new System.EventHandler(this.LstCommonEvent_DoubleClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "ダブルクリックで右部にコマンドを表示";
            // 
            // btnCmnUpdate
            // 
            this.btnCmnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCmnUpdate.Location = new System.Drawing.Point(6, 280);
            this.btnCmnUpdate.Name = "btnCmnUpdate";
            this.btnCmnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnCmnUpdate.TabIndex = 1;
            this.btnCmnUpdate.Text = "リスト更新";
            this.btnCmnUpdate.UseVisualStyleBackColor = true;
            this.btnCmnUpdate.Click += new System.EventHandler(this.BtnCmnUpdate_Click);
            // 
            // btnChangeSentenceCode
            // 
            this.btnChangeSentenceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangeSentenceCode.Location = new System.Drawing.Point(704, 402);
            this.btnChangeSentenceCode.Name = "btnChangeSentenceCode";
            this.btnChangeSentenceCode.Size = new System.Drawing.Size(273, 23);
            this.btnChangeSentenceCode.TabIndex = 3;
            this.btnChangeSentenceCode.Text = "イベントコマンド文 / イベントコマンドコード  切り替え";
            this.btnChangeSentenceCode.UseVisualStyleBackColor = true;
            this.btnChangeSentenceCode.Click += new System.EventHandler(this.btnChangeSentenceCode_Click);
            // 
            // txtShow
            // 
            this.txtShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtShow.BackColor = System.Drawing.Color.White;
            this.txtShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShow.Location = new System.Drawing.Point(704, 12);
            this.txtShow.Name = "txtShow";
            this.txtShow.ReadOnly = true;
            this.txtShow.Size = new System.Drawing.Size(458, 384);
            this.txtShow.TabIndex = 2;
            this.txtShow.Text = "";
            this.txtShow.WordWrap = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 427);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1174, 23);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblState
            // 
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(53, 18);
            this.lblState.Text = "lblState";
            // 
            // cmbEventCodeColor
            // 
            this.cmbEventCodeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEventCodeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEventCodeColor.FormattingEnabled = true;
            this.cmbEventCodeColor.Items.AddRange(new object[] {
            "タイプ1",
            "タイプ2",
            "旧配色"});
            this.cmbEventCodeColor.Location = new System.Drawing.Point(1100, 403);
            this.cmbEventCodeColor.Name = "cmbEventCodeColor";
            this.cmbEventCodeColor.Size = new System.Drawing.Size(62, 20);
            this.cmbEventCodeColor.TabIndex = 5;
            this.cmbEventCodeColor.SelectedValueChanged += new System.EventHandler(this.CmbEventCodeColor_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(994, 406);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "イベントコマンド配色";
            // 
            // grpSelectEvent
            // 
            this.grpSelectEvent.Location = new System.Drawing.Point(327, 12);
            this.grpSelectEvent.Name = "grpSelectEvent";
            this.grpSelectEvent.Size = new System.Drawing.Size(371, 411);
            this.grpSelectEvent.TabIndex = 7;
            this.grpSelectEvent.TabStop = false;
            this.grpSelectEvent.Text = "選択イベント情報";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 450);
            this.Controls.Add(this.grpSelectEvent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbEventCodeColor);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtShow);
            this.Controls.Add(this.btnChangeSentenceCode);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "WodiLibSample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOpenSampleProj;
        private System.Windows.Forms.Button btnOpenDirDialog;
        private System.Windows.Forms.TextBox txtProjectDir;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lstCommonEvent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCmnUpdate;
        private System.Windows.Forms.Button btnChangeSentenceCode;
        private System.Windows.Forms.RichTextBox txtShow;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblState;
        private System.Windows.Forms.ComboBox cmbEventCodeColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMapUpdate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMapEventUpdate;
        private System.Windows.Forms.ListBox lstMap;
        private System.Windows.Forms.ListBox lstMapEvent;
        private System.Windows.Forms.GroupBox grpSelectEvent;
    }
}

