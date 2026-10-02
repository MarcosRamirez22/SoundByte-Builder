namespace SoundByte_Builder
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnClipperSave = new Button();
            btnClipperSaveAs = new Button();
            lblClipperVolume = new Label();
            txtClipperVolume = new TextBox();
            btnStart = new Button();
            btnStop = new Button();
            cmbRecordingMode = new ComboBox();
            label1 = new Label();
            lblHotkey = new Label();
            btnChangeHotkey = new Button();
            txtSaveFolder = new TextBox();
            lblSaveFolder = new Label();
            btnBrowseFolder = new Button();
            lblStatus = new Label();
            btnShowRecordings = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            cmbMicrophone = new ComboBox();
            chkIncludeMicrophone = new CheckBox();
            chkAudioOutput = new CheckBox();
            btnAudioOutputs = new Button();
            ctxAudioOutputs = new ContextMenuStrip(components);
            chkApplications = new CheckBox();
            btnApplications = new Button();
            ctxApplications = new ContextMenuStrip(components);
            tabMain = new TabControl();
            tabRecorder = new TabPage();
            tabAudioClipper = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            lblClipperSelection = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblClipperPosition = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnClipperStop = new Button();
            btnClipperPlayPause = new Button();
            btnClipperZoomIn = new Button();
            btnClipperZoomOut = new Button();
            btnClipperFit = new Button();
            lblClipperZoom = new Label();
            hsbClipperWaveform = new HScrollBar();
            trkClipperPosition = new SeekTrackBar();
            btnOpenAudio = new Button();
            txtClipperFile = new TextBox();
            lblClipperDetails = new Label();
            clipperWaveform = new WaveformControl();
            clipperToolTip = new ToolTip(components);
            clipperPlaybackTimer = new System.Windows.Forms.Timer(components);
            tableLayoutPanel2.SuspendLayout();
            tabMain.SuspendLayout();
            tabRecorder.SuspendLayout();
            tabAudioClipper.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            (trkClipperPosition).BeginInit();
            SuspendLayout();
            // 
            // btnClipperSave
            // 
            btnClipperSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClipperSave.Enabled = false;
            btnClipperSave.Location = new Point(343, 3);
            btnClipperSave.Name = "btnClipperSave";
            btnClipperSave.Size = new Size(85, 27);
            btnClipperSave.TabIndex = 18;
            btnClipperSave.Text = "Save";
            btnClipperSave.UseVisualStyleBackColor = true;
            btnClipperSave.Click += btnClipperSave_Click;
            // 
            // btnClipperSaveAs
            // 
            btnClipperSaveAs.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClipperSaveAs.Enabled = false;
            btnClipperSaveAs.Location = new Point(229, 3);
            btnClipperSaveAs.Name = "btnClipperSaveAs";
            btnClipperSaveAs.Size = new Size(90, 27);
            btnClipperSaveAs.TabIndex = 19;
            btnClipperSaveAs.Text = "Save As...";
            btnClipperSaveAs.UseVisualStyleBackColor = true;
            btnClipperSaveAs.Click += btnClipperSaveAs_Click;
            // 
            // lblClipperVolume
            // 
            lblClipperVolume.Location = new Point(283, 81);
            lblClipperVolume.Name = "lblClipperVolume";
            lblClipperVolume.Size = new Size(53, 20);
            lblClipperVolume.TabIndex = 16;
            lblClipperVolume.Text = "Volume:";
            // 
            // txtClipperVolume
            // 
            txtClipperVolume.AccessibleName = "Volume percentage";
            txtClipperVolume.Enabled = false;
            txtClipperVolume.Location = new Point(338, 78);
            txtClipperVolume.Name = "txtClipperVolume";
            txtClipperVolume.Size = new Size(68, 23);
            txtClipperVolume.TabIndex = 17;
            txtClipperVolume.Text = "100%";
            clipperToolTip.SetToolTip(txtClipperVolume, "Volume 0-200%. 100% is original volume. Type a percentage and press Enter.");
            txtClipperVolume.KeyDown += txtClipperVolume_KeyDown;
            txtClipperVolume.Leave += txtClipperVolume_Leave;
            // 
            // btnStart
            // 
            btnStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnStart.AutoSize = true;
            btnStart.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnStart.Location = new Point(3, 5);
            btnStart.MaximumSize = new Size(100, 23);
            btnStart.MinimumSize = new Size(50, 23);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(98, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start Recording";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Bottom;
            btnStop.AutoSize = true;
            btnStop.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnStop.Location = new Point(159, 5);
            btnStop.MaximumSize = new Size(100, 23);
            btnStop.MinimumSize = new Size(50, 23);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(98, 23);
            btnStop.TabIndex = 1;
            btnStop.Text = "Stop Recording";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // cmbRecordingMode
            // 
            cmbRecordingMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRecordingMode.FormattingEnabled = true;
            cmbRecordingMode.Location = new Point(135, 117);
            cmbRecordingMode.Name = "cmbRecordingMode";
            cmbRecordingMode.Size = new Size(103, 23);
            cmbRecordingMode.TabIndex = 4;
            cmbRecordingMode.SelectedIndexChanged += cmbRecordingMode_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 120);
            label1.Name = "label1";
            label1.Size = new Size(98, 15);
            label1.TabIndex = 5;
            label1.Text = "Recording Mode:";
            // 
            // lblHotkey
            // 
            lblHotkey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblHotkey.AutoSize = true;
            lblHotkey.Location = new Point(31, 162);
            lblHotkey.Name = "lblHotkey";
            lblHotkey.Size = new Size(105, 15);
            lblHotkey.TabIndex = 6;
            lblHotkey.Text = "Recording Hotkey:";
            // 
            // btnChangeHotkey
            // 
            btnChangeHotkey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnChangeHotkey.Location = new Point(205, 158);
            btnChangeHotkey.MaximumSize = new Size(200, 23);
            btnChangeHotkey.MinimumSize = new Size(100, 23);
            btnChangeHotkey.Name = "btnChangeHotkey";
            btnChangeHotkey.Size = new Size(115, 23);
            btnChangeHotkey.TabIndex = 8;
            btnChangeHotkey.Text = "Change Hotkey";
            btnChangeHotkey.UseVisualStyleBackColor = true;
            btnChangeHotkey.Click += btnChangeHotkey_Click;
            // 
            // txtSaveFolder
            // 
            txtSaveFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSaveFolder.Location = new Point(107, 190);
            txtSaveFolder.MaximumSize = new Size(600, 23);
            txtSaveFolder.MinimumSize = new Size(50, 23);
            txtSaveFolder.Name = "txtSaveFolder";
            txtSaveFolder.Size = new Size(358, 23);
            txtSaveFolder.TabIndex = 9;
            // 
            // lblSaveFolder
            // 
            lblSaveFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSaveFolder.AutoSize = true;
            lblSaveFolder.Location = new Point(31, 193);
            lblSaveFolder.Name = "lblSaveFolder";
            lblSaveFolder.Size = new Size(70, 15);
            lblSaveFolder.TabIndex = 10;
            lblSaveFolder.Text = "Save Folder:";
            // 
            // btnBrowseFolder
            // 
            btnBrowseFolder.Location = new Point(107, 219);
            btnBrowseFolder.MaximumSize = new Size(200, 23);
            btnBrowseFolder.MinimumSize = new Size(20, 23);
            btnBrowseFolder.Name = "btnBrowseFolder";
            btnBrowseFolder.Size = new Size(90, 23);
            btnBrowseFolder.TabIndex = 11;
            btnBrowseFolder.Text = "Browse...";
            btnBrowseFolder.UseVisualStyleBackColor = true;
            btnBrowseFolder.Click += btnBrowseFolder_Click;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(47, 389);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 15);
            lblStatus.TabIndex = 12;
            // 
            // btnShowRecordings
            // 
            btnShowRecordings.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnShowRecordings.AutoSize = true;
            btnShowRecordings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnShowRecordings.Location = new Point(308, 5);
            btnShowRecordings.MaximumSize = new Size(110, 23);
            btnShowRecordings.MinimumSize = new Size(50, 23);
            btnShowRecordings.Name = "btnShowRecordings";
            btnShowRecordings.Size = new Size(108, 23);
            btnShowRecordings.TabIndex = 13;
            btnShowRecordings.Text = "Show Recordings";
            btnShowRecordings.UseVisualStyleBackColor = true;
            btnShowRecordings.Click += btnShowRecordings_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(btnStart, 0, 0);
            tableLayoutPanel2.Controls.Add(btnStop, 1, 0);
            tableLayoutPanel2.Controls.Add(btnShowRecordings, 2, 0);
            tableLayoutPanel2.Location = new Point(31, 343);
            tableLayoutPanel2.MaximumSize = new Size(419, 31);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(419, 31);
            tableLayoutPanel2.TabIndex = 15;
            // 
            // cmbMicrophone
            // 
            cmbMicrophone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbMicrophone.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMicrophone.FormattingEnabled = true;
            cmbMicrophone.Location = new Point(135, 19);
            cmbMicrophone.MaximumSize = new Size(572, 0);
            cmbMicrophone.MinimumSize = new Size(20, 0);
            cmbMicrophone.Name = "cmbMicrophone";
            cmbMicrophone.Size = new Size(327, 23);
            cmbMicrophone.TabIndex = 17;
            // 
            // chkIncludeMicrophone
            // 
            chkIncludeMicrophone.AutoSize = true;
            chkIncludeMicrophone.Location = new Point(31, 23);
            chkIncludeMicrophone.Name = "chkIncludeMicrophone";
            chkIncludeMicrophone.Size = new Size(94, 19);
            chkIncludeMicrophone.TabIndex = 18;
            chkIncludeMicrophone.Text = "Microphone:";
            chkIncludeMicrophone.UseVisualStyleBackColor = true;
            chkIncludeMicrophone.CheckedChanged += cmbMicrophone_SelectedIndexChanged;
            chkIncludeMicrophone.CheckStateChanged += chkIncludeMicrophone_CheckedChanged;
            // 
            // chkAudioOutput
            // 
            chkAudioOutput.AutoSize = true;
            chkAudioOutput.Location = new Point(31, 53);
            chkAudioOutput.Name = "chkAudioOutput";
            chkAudioOutput.Size = new Size(102, 19);
            chkAudioOutput.TabIndex = 19;
            chkAudioOutput.Text = "Audio Output:";
            chkAudioOutput.UseVisualStyleBackColor = true;
            chkAudioOutput.CheckedChanged += chkAudioOutput_CheckedChanged;
            // 
            // btnAudioOutputs
            // 
            btnAudioOutputs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnAudioOutputs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnAudioOutputs.Location = new Point(135, 50);
            btnAudioOutputs.MaximumSize = new Size(572, 23);
            btnAudioOutputs.MinimumSize = new Size(20, 23);
            btnAudioOutputs.Name = "btnAudioOutputs";
            btnAudioOutputs.Size = new Size(330, 23);
            btnAudioOutputs.TabIndex = 20;
            btnAudioOutputs.Text = "Select Audio Outputs";
            btnAudioOutputs.UseVisualStyleBackColor = true;
            btnAudioOutputs.Click += btnAudioOutputs_Click;
            // 
            // ctxAudioOutputs
            // 
            ctxAudioOutputs.MaximumSize = new Size(0, 400);
            ctxAudioOutputs.Name = "ctxAudioOutputs";
            ctxAudioOutputs.Size = new Size(61, 4);
            ctxAudioOutputs.Closing += ctxAudioOutputs_Closing;
            // 
            // chkApplications
            // 
            chkApplications.AutoSize = true;
            chkApplications.Location = new Point(30, 82);
            chkApplications.Name = "chkApplications";
            chkApplications.Size = new Size(95, 19);
            chkApplications.TabIndex = 21;
            chkApplications.Text = "Applications:";
            chkApplications.UseVisualStyleBackColor = true;
            chkApplications.Click += chkApplications_CheckedChanged;
            // 
            // btnApplications
            // 
            btnApplications.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnApplications.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnApplications.Location = new Point(135, 79);
            btnApplications.MaximumSize = new Size(572, 23);
            btnApplications.MinimumSize = new Size(20, 23);
            btnApplications.Name = "btnApplications";
            btnApplications.Size = new Size(327, 23);
            btnApplications.TabIndex = 22;
            btnApplications.Text = "Select Applications";
            btnApplications.UseVisualStyleBackColor = true;
            btnApplications.Click += btnApplications_Click;
            // 
            // ctxApplications
            // 
            ctxApplications.MaximumSize = new Size(0, 400);
            ctxApplications.Name = "ctxApplications";
            ctxApplications.Size = new Size(61, 4);
            ctxApplications.Closing += ctxApplications_Closing;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabRecorder);
            tabMain.Controls.Add(tabAudioClipper);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 0);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(494, 447);
            tabMain.TabIndex = 0;
            // 
            // tabRecorder
            // 
            tabRecorder.Controls.Add(btnApplications);
            tabRecorder.Controls.Add(chkApplications);
            tabRecorder.Controls.Add(btnAudioOutputs);
            tabRecorder.Controls.Add(chkAudioOutput);
            tabRecorder.Controls.Add(chkIncludeMicrophone);
            tabRecorder.Controls.Add(cmbMicrophone);
            tabRecorder.Controls.Add(tableLayoutPanel2);
            tabRecorder.Controls.Add(lblStatus);
            tabRecorder.Controls.Add(btnBrowseFolder);
            tabRecorder.Controls.Add(lblSaveFolder);
            tabRecorder.Controls.Add(txtSaveFolder);
            tabRecorder.Controls.Add(btnChangeHotkey);
            tabRecorder.Controls.Add(lblHotkey);
            tabRecorder.Controls.Add(label1);
            tabRecorder.Controls.Add(cmbRecordingMode);
            tabRecorder.Location = new Point(4, 24);
            tabRecorder.Name = "tabRecorder";
            tabRecorder.Size = new Size(486, 419);
            tabRecorder.TabIndex = 0;
            tabRecorder.Text = "Recorder";
            tabRecorder.UseVisualStyleBackColor = true;
            // 
            // tabAudioClipper
            // 
            tabAudioClipper.Controls.Add(tableLayoutPanel4);
            tabAudioClipper.Controls.Add(lblClipperVolume);
            tabAudioClipper.Controls.Add(txtClipperVolume);
            tabAudioClipper.Controls.Add(tableLayoutPanel3);
            tabAudioClipper.Controls.Add(tableLayoutPanel1);
            tabAudioClipper.Controls.Add(btnClipperZoomIn);
            tabAudioClipper.Controls.Add(btnClipperZoomOut);
            tabAudioClipper.Controls.Add(btnClipperFit);
            tabAudioClipper.Controls.Add(lblClipperZoom);
            tabAudioClipper.Controls.Add(hsbClipperWaveform);
            tabAudioClipper.Controls.Add(trkClipperPosition);
            tabAudioClipper.Controls.Add(btnOpenAudio);
            tabAudioClipper.Controls.Add(txtClipperFile);
            tabAudioClipper.Controls.Add(lblClipperDetails);
            tabAudioClipper.Controls.Add(clipperWaveform);
            tabAudioClipper.Location = new Point(4, 24);
            tabAudioClipper.Name = "tabAudioClipper";
            tabAudioClipper.Size = new Size(486, 419);
            tabAudioClipper.TabIndex = 1;
            tabAudioClipper.Text = "Audio Clipper";
            tabAudioClipper.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Controls.Add(lblClipperSelection, 0, 0);
            tableLayoutPanel4.Controls.Add(btnClipperSave, 2, 0);
            tableLayoutPanel4.Controls.Add(btnClipperSaveAs, 1, 0);
            tableLayoutPanel4.Location = new Point(20, 369);
            tableLayoutPanel4.MinimumSize = new Size(0, 33);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(431, 33);
            tableLayoutPanel4.TabIndex = 20;
            // 
            // lblClipperSelection
            // 
            lblClipperSelection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblClipperSelection.Location = new Point(3, 0);
            lblClipperSelection.Name = "lblClipperSelection";
            lblClipperSelection.Size = new Size(209, 25);
            lblClipperSelection.TabIndex = 8;
            lblClipperSelection.Text = "Start: --   End: --   Duration: --";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(lblClipperPosition, 1, 0);
            tableLayoutPanel3.Location = new Point(20, 346);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(431, 21);
            tableLayoutPanel3.TabIndex = 15;
            // 
            // lblClipperPosition
            // 
            lblClipperPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblClipperPosition.Location = new Point(158, 0);
            lblClipperPosition.Name = "lblClipperPosition";
            lblClipperPosition.Size = new Size(114, 18);
            lblClipperPosition.TabIndex = 7;
            lblClipperPosition.Text = "00:00.00 / 00:00.00";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btnClipperStop, 2, 0);
            tableLayoutPanel1.Controls.Add(btnClipperPlayPause, 1, 0);
            tableLayoutPanel1.Location = new Point(20, 301);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(431, 45);
            tableLayoutPanel1.TabIndex = 14;
            // 
            // btnClipperStop
            // 
            btnClipperStop.AccessibleName = "Stop";
            btnClipperStop.Enabled = false;
            btnClipperStop.Font = new Font("Segoe UI Symbol", 16F);
            btnClipperStop.Location = new Point(218, 3);
            btnClipperStop.Name = "btnClipperStop";
            btnClipperStop.Size = new Size(44, 36);
            btnClipperStop.TabIndex = 6;
            btnClipperStop.Text = "■";
            clipperToolTip.SetToolTip(btnClipperStop, "Stop and return to Start");
            btnClipperStop.UseVisualStyleBackColor = true;
            btnClipperStop.Click += btnClipperStop_Click;
            // 
            // btnClipperPlayPause
            // 
            btnClipperPlayPause.AccessibleName = "Play";
            btnClipperPlayPause.Enabled = false;
            btnClipperPlayPause.Font = new Font("Segoe UI Symbol", 16F);
            btnClipperPlayPause.Location = new Point(168, 3);
            btnClipperPlayPause.Name = "btnClipperPlayPause";
            btnClipperPlayPause.Size = new Size(44, 36);
            btnClipperPlayPause.TabIndex = 5;
            btnClipperPlayPause.Text = "▶";
            clipperToolTip.SetToolTip(btnClipperPlayPause, "Play selected audio");
            btnClipperPlayPause.UseVisualStyleBackColor = true;
            btnClipperPlayPause.Click += btnClipperPlayPause_Click;
            // 
            // btnClipperZoomIn
            // 
            btnClipperZoomIn.Enabled = false;
            btnClipperZoomIn.Location = new Point(20, 75);
            btnClipperZoomIn.Name = "btnClipperZoomIn";
            btnClipperZoomIn.Size = new Size(75, 27);
            btnClipperZoomIn.TabIndex = 9;
            btnClipperZoomIn.Text = "Zoom In";
            btnClipperZoomIn.UseVisualStyleBackColor = true;
            btnClipperZoomIn.Click += btnClipperZoomIn_Click;
            // 
            // btnClipperZoomOut
            // 
            btnClipperZoomOut.Enabled = false;
            btnClipperZoomOut.Location = new Point(101, 75);
            btnClipperZoomOut.Name = "btnClipperZoomOut";
            btnClipperZoomOut.Size = new Size(75, 27);
            btnClipperZoomOut.TabIndex = 10;
            btnClipperZoomOut.Text = "Zoom Out";
            btnClipperZoomOut.UseVisualStyleBackColor = true;
            btnClipperZoomOut.Click += btnClipperZoomOut_Click;
            // 
            // btnClipperFit
            // 
            btnClipperFit.Enabled = false;
            btnClipperFit.Location = new Point(182, 75);
            btnClipperFit.Name = "btnClipperFit";
            btnClipperFit.Size = new Size(50, 27);
            btnClipperFit.TabIndex = 11;
            btnClipperFit.Text = "Fit";
            btnClipperFit.UseVisualStyleBackColor = true;
            btnClipperFit.Click += btnClipperFit_Click;
            // 
            // lblClipperZoom
            // 
            lblClipperZoom.Location = new Point(238, 81);
            lblClipperZoom.Name = "lblClipperZoom";
            lblClipperZoom.Size = new Size(45, 20);
            lblClipperZoom.TabIndex = 12;
            lblClipperZoom.Text = "1x";
            // 
            // hsbClipperWaveform
            // 
            hsbClipperWaveform.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            hsbClipperWaveform.Enabled = false;
            hsbClipperWaveform.LargeChange = 10000;
            hsbClipperWaveform.Location = new Point(20, 225);
            hsbClipperWaveform.Maximum = 9999;
            hsbClipperWaveform.Name = "hsbClipperWaveform";
            hsbClipperWaveform.Size = new Size(431, 17);
            hsbClipperWaveform.TabIndex = 13;
            hsbClipperWaveform.Scroll += hsbClipperWaveform_Scroll;
            // 
            // trkClipperPosition
            // 
            trkClipperPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trkClipperPosition.Enabled = false;
            trkClipperPosition.LargeChange = 1000;
            trkClipperPosition.Location = new Point(20, 250);
            trkClipperPosition.Maximum = 10000;
            trkClipperPosition.Name = "trkClipperPosition";
            trkClipperPosition.Size = new Size(431, 45);
            trkClipperPosition.SmallChange = 100;
            trkClipperPosition.TabIndex = 4;
            trkClipperPosition.TickStyle = TickStyle.None;
            trkClipperPosition.Scroll += trkClipperPosition_Scroll;
            // 
            // btnOpenAudio
            // 
            btnOpenAudio.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOpenAudio.Location = new Point(351, 20);
            btnOpenAudio.Name = "btnOpenAudio";
            btnOpenAudio.Size = new Size(100, 27);
            btnOpenAudio.TabIndex = 1;
            btnOpenAudio.Text = "Open Audio...";
            btnOpenAudio.UseVisualStyleBackColor = true;
            btnOpenAudio.Click += btnOpenAudio_Click;
            // 
            // txtClipperFile
            // 
            txtClipperFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtClipperFile.Location = new Point(20, 22);
            txtClipperFile.Name = "txtClipperFile";
            txtClipperFile.PlaceholderText = "No audio selected";
            txtClipperFile.ReadOnly = true;
            txtClipperFile.Size = new Size(321, 23);
            txtClipperFile.TabIndex = 0;
            // 
            // lblClipperDetails
            // 
            lblClipperDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblClipperDetails.Location = new Point(20, 50);
            lblClipperDetails.Name = "lblClipperDetails";
            lblClipperDetails.Size = new Size(431, 22);
            lblClipperDetails.TabIndex = 2;
            lblClipperDetails.Text = "WAV recordings supported";
            // 
            // clipperWaveform
            // 
            clipperWaveform.AccessibleName = "Recording waveform";
            clipperWaveform.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            clipperWaveform.BackColor = Color.WhiteSmoke;
            clipperWaveform.ForeColor = Color.SteelBlue;
            clipperWaveform.Location = new Point(20, 110);
            clipperWaveform.Name = "clipperWaveform";
            clipperWaveform.Size = new Size(431, 110);
            clipperWaveform.TabIndex = 3;
            clipperToolTip.SetToolTip(clipperWaveform, "Drag the horizontal line up/down for volume. Wheel: zoom. Shift + wheel: scroll.");
            clipperWaveform.SeekRequested += clipperWaveform_SeekRequested;
            clipperWaveform.SelectionChanged += clipperWaveform_SelectionChanged;
            clipperWaveform.VolumeChanged += clipperWaveform_VolumeChanged;
            clipperWaveform.ViewChanged += clipperWaveform_ViewChanged;
            // 
            // clipperPlaybackTimer
            // 
            clipperPlaybackTimer.Tick += clipperPlaybackTimer_Tick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(494, 447);
            Controls.Add(tabMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(465, 463);
            Name = "MainForm";
            Text = "SoundByte Builder";
            Load += MainForm_Load;
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tabMain.ResumeLayout(false);
            tabRecorder.ResumeLayout(false);
            tabRecorder.PerformLayout();
            tabAudioClipper.ResumeLayout(false);
            tabAudioClipper.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            (trkClipperPosition).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabMain;
        private TabPage tabRecorder;
        private TabPage tabAudioClipper;
        private WaveformControl clipperWaveform;
        private Button btnOpenAudio;
        private TextBox txtClipperFile;
        private Label lblClipperDetails;
        private Button btnClipperPlayPause;
        private Button btnClipperStop;
        private SeekTrackBar trkClipperPosition;
        private Label lblClipperPosition;
        private Label lblClipperSelection;
        private Button btnClipperZoomIn;
        private Button btnClipperZoomOut;
        private Button btnClipperFit;
        private Label lblClipperZoom;
        private HScrollBar hsbClipperWaveform;
        private ToolTip clipperToolTip;
        private Label lblClipperVolume;
        private TextBox txtClipperVolume;
        private Button btnClipperSave;
        private Button btnClipperSaveAs;
        private System.Windows.Forms.Timer clipperPlaybackTimer;
        private Button btnStart;
        private Button btnStop;
        private ComboBox cmbRecordingMode;
        private Label label1;
        private Label lblHotkey;
        private Button btnChangeHotkey;
        private TextBox txtSaveFolder;
        private Label lblSaveFolder;
        private Button btnBrowseFolder;
        private Label lblStatus;
        private Button btnShowRecordings;
        private TableLayoutPanel tableLayoutPanel2;
        private ComboBox cmbMicrophone;
        private CheckBox chkIncludeMicrophone;
        private CheckBox chkAudioOutput;
        private Button btnAudioOutputs;
        private ContextMenuStrip ctxAudioOutputs;
        private CheckBox chkApplications;
        private Button btnApplications;
        private ContextMenuStrip ctxApplications;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
    }
}
