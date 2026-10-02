using NAudio.CoreAudioApi;
using NAudio.Wave;
using SoundByte_Builder.Audio;
using SoundByte_Builder.Input;
using SoundByte_Builder.Models;
using System.Diagnostics;
using System.Text.Json;
using SoundByte_Builder.Programs;

namespace SoundByte_Builder
{
    public partial class MainForm : Form
    {
        private readonly MMDeviceEnumerator deviceEnumerator = new();
        private readonly List<MMDevice> audioDevices = new();
        private readonly List<MMDevice> microphoneDevices = new();
        private readonly List<RunningApplication>runningApplications = new();

        private readonly RecordingManager recordingManager = new();

        private readonly AudioPlaybackManager clipperPlayback = new();
        private Exception? lastClipperPlaybackError;
        private readonly CancellationTokenSource waveformCancellation = new();

        private GlobalKeyboardHook? keyboardHook;

        private Keys recordingHotkey = Keys.F8;

        private bool hotkeyHeld = false;
        private bool waitingForHotkey = false;
        private bool ignoreHotkeyRelease = false;

        private bool isLoadingSettings = true;

        private bool audioOutputsMenuClosedByButton = false;

        private bool applicationsMenuClosedByButton = false;
        private bool isLoadingApplications = false;

        private bool isStartingRecording = false;

        private AppSettings appSettings = new();

        private readonly string settingsFolder = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData
            ),
            "SoundByte Builder"
        );

        private string SettingsFilePath =>
            Path.Combine(
                settingsFolder,
                "settings.json"
            );

        public MainForm()
        {
            InitializeComponent();

            btnStop.Enabled = false;

            cmbRecordingMode.Items.Add("Hold to Record");
            cmbRecordingMode.Items.Add("Press to Toggle");

            recordingManager.StatusChanged +=
                RecordingManager_StatusChanged;

            recordingManager.RecordingSaved +=
                RecordingManager_RecordingSaved;

            recordingManager.RecordingFailed +=
                RecordingManager_RecordingFailed;

            LoadAudioDevices();
            LoadMicrophones();
            LoadSettings();
            LoadRunningApplications();

            UpdateHotkeyLabel();

            keyboardHook = new GlobalKeyboardHook();

            keyboardHook.KeyDown += GlobalKeyDown;
            keyboardHook.KeyUp += GlobalKeyUp;

            lblStatus.Text = "Ready";

            isLoadingSettings = false;
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json =
                        File.ReadAllText(SettingsFilePath);

                    AppSettings? loadedSettings =
                        JsonSerializer.Deserialize<AppSettings>(
                            json
                        );

                    if (loadedSettings != null)
                    {
                        appSettings = loadedSettings;
                    }
                }
            }
            catch
            {
                appSettings = new AppSettings();
            }

            if (appSettings.RecordingMode >= 0 &&
                appSettings.RecordingMode <
                cmbRecordingMode.Items.Count)
            {
                cmbRecordingMode.SelectedIndex =
                    appSettings.RecordingMode;
            }
            else
            {
                cmbRecordingMode.SelectedIndex = 0;
            }

            recordingHotkey =
                (Keys)appSettings.RecordingHotkey;

            if (!string.IsNullOrWhiteSpace(
                    appSettings.SaveFolder))
            {
                txtSaveFolder.Text =
                    appSettings.SaveFolder;
            }
            else
            {
                txtSaveFolder.Text =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.MyMusic
                        ),
                        "SoundByte Builder"
                    );
            }

            foreach (ToolStripMenuItem item in ctxAudioOutputs.Items)
            {
                item.Checked = false;
            }

            foreach (ToolStripMenuItem item in ctxAudioOutputs.Items)
            {
                if (item.Tag is string deviceId &&
                    appSettings.AudioDeviceIds.Contains(deviceId))
                {
                    item.Checked = true;
                }
            }

            chkAudioOutput.Checked =
                appSettings.IncludeAudioOutput;

            btnAudioOutputs.Enabled =
                chkAudioOutput.Checked &&
                audioDevices.Count > 0;

            UpdateAudioOutputsButtonText();

            bool microphoneFound = false;

            if (!string.IsNullOrWhiteSpace(
                    appSettings.MicrophoneDeviceId))
            {
                for (int i = 0;
                     i < microphoneDevices.Count;
                     i++)
                {
                    if (microphoneDevices[i].ID ==
                        appSettings.MicrophoneDeviceId)
                    {
                        cmbMicrophone.SelectedIndex = i;
                        microphoneFound = true;
                        break;
                    }
                }
            }

            chkApplications.Checked =
            appSettings.IncludeApplications;

            btnApplications.Enabled =
                chkApplications.Checked;

            if (!microphoneFound &&
                microphoneDevices.Count > 0)
            {
                cmbMicrophone.SelectedIndex = 0;
            }

            chkIncludeMicrophone.Checked =
                appSettings.IncludeMicrophone;

            cmbMicrophone.Enabled =
                chkIncludeMicrophone.Checked &&
                microphoneDevices.Count > 0;
        }

        private void SaveSettings()
        {
            if (isLoadingSettings)
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(
                    settingsFolder
                );

                appSettings.RecordingMode =
                    cmbRecordingMode.SelectedIndex;

                appSettings.RecordingHotkey =
                    (int)recordingHotkey;

                appSettings.SaveFolder =
                    txtSaveFolder.Text.Trim();

                appSettings.IncludeMicrophone =
                    chkIncludeMicrophone.Checked;

                appSettings.IncludeAudioOutput =
                    chkAudioOutput.Checked;

                appSettings.IncludeApplications =
                    chkApplications.Checked;

                appSettings.AudioDeviceIds.Clear();

                foreach (ToolStripMenuItem item in ctxAudioOutputs.Items)
                {
                    if (item.Checked &&
                        item.Tag is string deviceId)
                    {
                        appSettings.AudioDeviceIds.Add(
                            deviceId
                        );
                    }
                }

                if (cmbMicrophone.SelectedIndex >= 0 &&
                    cmbMicrophone.SelectedIndex <
                    microphoneDevices.Count)
                {
                    appSettings.MicrophoneDeviceId =
                        microphoneDevices[
                            cmbMicrophone.SelectedIndex
                        ].ID;
                }

                string json =
                    JsonSerializer.Serialize(
                        appSettings,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        }
                    );

                File.WriteAllText(
                    SettingsFilePath,
                    json
                );
            }
            catch (Exception ex)
            {
                lblStatus.Text =
                    $"Could not save settings: {ex.Message}";
            }
        }

        private void LoadAudioDevices()
        {
            ctxAudioOutputs.Items.Clear();
            audioDevices.Clear();

            var devices =
                deviceEnumerator.EnumerateAudioEndPoints(
                    DataFlow.Render,
                    DeviceState.Active
                );

            foreach (var device in devices)
            {
                audioDevices.Add(device);

                var item =
                    new ToolStripMenuItem(
                        device.FriendlyName
                    )
                    {
                        CheckOnClick = true,
                        Tag = device.ID
                    };

                item.CheckedChanged +=
                    AudioOutputItem_CheckedChanged;

                ctxAudioOutputs.Items.Add(item);
            }

            UpdateAudioOutputsButtonText();
        }

        private void LoadMicrophones()
        {
            cmbMicrophone.Items.Clear();
            microphoneDevices.Clear();

            var devices =
                deviceEnumerator.EnumerateAudioEndPoints(
                    DataFlow.Capture,
                    DeviceState.Active
                );

            foreach (var device in devices)
            {
                microphoneDevices.Add(device);

                cmbMicrophone.Items.Add(
                    device.FriendlyName
                );
            }

            if (microphoneDevices.Count > 0)
            {
                cmbMicrophone.SelectedIndex = 0;
            }
            else
            {
                cmbMicrophone.Items.Add(
                    "No microphones found"
                );

                cmbMicrophone.SelectedIndex = 0;

                cmbMicrophone.Enabled = false;
                chkIncludeMicrophone.Enabled = false;
            }
        }

        private void LoadRunningApplications()
        {
            isLoadingApplications = true;

            try
            {
                ctxApplications.Items.Clear();
                runningApplications.Clear();

                var discoveredApplications =
                    ApplicationDiscovery
                        .GetRunningApplications();

                runningApplications.AddRange(
                    discoveredApplications
                );

                var displayedKeys =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase
                    );

                foreach (var application in discoveredApplications)
                {
                    string key =
                        GetApplicationKey(
                            application.ProcessName,
                            application.ExecutablePath
                        );

                    if (!displayedKeys.Add(key))
                    {
                        continue;
                    }

                    SavedApplication? savedApplication =
                        appSettings.Applications
                            .FirstOrDefault(
                                saved =>
                                    GetApplicationKey(
                                        saved.ProcessName,
                                        saved.ExecutablePath
                                    ) == key
                            );

                    var applicationInfo =
                        new SavedApplication
                        {
                            ProcessName =
                                application.ProcessName,

                            DisplayName =
                                application.DisplayName,

                            ExecutablePath =
                                application.ExecutablePath
                        };

                    var item =
                        new ToolStripMenuItem(
                            application.DisplayName
                        )
                        {
                            CheckOnClick = true,
                            Checked =
                                savedApplication != null,

                            Tag =
                                applicationInfo
                        };

                    item.CheckedChanged +=
                        ApplicationItem_CheckedChanged;

                    ctxApplications.Items.Add(
                        item
                    );
                }

                foreach (SavedApplication savedApplication
                         in appSettings.Applications)
                {
                    string key =
                        GetApplicationKey(
                            savedApplication.ProcessName,
                            savedApplication.ExecutablePath
                        );

                    if (displayedKeys.Contains(key))
                    {
                        continue;
                    }

                    var item =
                        new ToolStripMenuItem(
                            $"{savedApplication.DisplayName} (Offline)"
                        )
                        {
                            CheckOnClick = true,
                            Checked = true,
                            Tag = savedApplication
                        };

                    item.CheckedChanged +=
                        ApplicationItem_CheckedChanged;

                    ctxApplications.Items.Add(
                        item
                    );
                }

                UpdateApplicationsButtonText();
            }
            finally
            {
                isLoadingApplications = false;
            }
        }

        private static string GetApplicationKey(
            string processName,
            string? executablePath)
        {
            if (!string.IsNullOrWhiteSpace(
                    executablePath))
            {
                return executablePath;
            }

            return processName;
        }

        private void ApplicationItem_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            if (isLoadingApplications ||
                sender is not ToolStripMenuItem item ||
                item.Tag is not SavedApplication application)
            {
                return;
            }

            string key =
                GetApplicationKey(
                    application.ProcessName,
                    application.ExecutablePath
                );

            SavedApplication? existing =
                appSettings.Applications
                    .FirstOrDefault(
                        saved =>
                            GetApplicationKey(
                                saved.ProcessName,
                                saved.ExecutablePath
                            ) == key
                    );

            if (item.Checked)
            {
                if (existing == null)
                {
                    appSettings.Applications.Add(
                        new SavedApplication
                        {
                            ProcessName =
                                application.ProcessName,

                            DisplayName =
                                application.DisplayName,

                            ExecutablePath =
                                application.ExecutablePath
                        }
                    );
                }
            }
            else
            {
                if (existing != null)
                {
                    appSettings.Applications.Remove(
                        existing
                    );
                }
            }

            UpdateApplicationsButtonText();
            SaveSettings();
        }

        private void AudioOutputItem_CheckedChanged(
            object? sender,
            EventArgs e)
        {
            UpdateAudioOutputsButtonText();
            SaveSettings();
        }

        private void btnAudioOutputs_Click(
            object sender,
            EventArgs e)
        {
            if (audioOutputsMenuClosedByButton)
            {
                audioOutputsMenuClosedByButton = false;
                return;
            }

            ctxAudioOutputs.Show(
                btnAudioOutputs,
                new Point(
                    0,
                    btnAudioOutputs.Height
                )
            );
        }

        private void chkAudioOutput_CheckedChanged(
            object sender,
            EventArgs e)
        {
            btnAudioOutputs.Enabled =
                chkAudioOutput.Checked &&
                audioDevices.Count > 0;

            SaveSettings();
        }

        private void btnApplications_Click(
            object sender,
            EventArgs e)
        {
            if (applicationsMenuClosedByButton)
            {
                applicationsMenuClosedByButton = false;
                return;
            }

            LoadRunningApplications();

            ctxApplications.Show(
                btnApplications,
                new Point(
                    0,
                    btnApplications.Height
                )
            );
        }

        private void ctxApplications_Closing(
            object sender,
            ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason ==
                ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
                return;
            }

            if (e.CloseReason ==
                ToolStripDropDownCloseReason.AppClicked)
            {
                Point mousePosition =
                    btnApplications.PointToClient(
                        Cursor.Position
                    );

                if (btnApplications
                    .ClientRectangle
                    .Contains(mousePosition))
                {
                    applicationsMenuClosedByButton = true;
                }
            }
        }

        private void ctxAudioOutputs_Closing(
            object sender,
            ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason ==
                ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
                return;
            }

            if (e.CloseReason ==
                ToolStripDropDownCloseReason.AppClicked)
            {
                Point mousePosition =
                    btnAudioOutputs.PointToClient(
                        Cursor.Position
                    );

                if (btnAudioOutputs.ClientRectangle.Contains(
                        mousePosition))
                {
                    audioOutputsMenuClosedByButton = true;
                }
            }
        }

        private void chkApplications_CheckedChanged(
            object sender,
            EventArgs e)
        {
            btnApplications.Enabled =
                chkApplications.Checked;

            SaveSettings();
        }

        private void cmbMicrophone_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            SaveSettings();
        }

        private void chkIncludeMicrophone_CheckedChanged(
            object sender,
            EventArgs e)
        {
            cmbMicrophone.Enabled =
                chkIncludeMicrophone.Checked &&
                microphoneDevices.Count > 0;

            SaveSettings();
        }

        private void UpdateAudioOutputsButtonText()
        {
            var selectedItems =
                ctxAudioOutputs.Items
                    .OfType<ToolStripMenuItem>()
                    .Where(item => item.Checked)
                    .ToList();

            if (selectedItems.Count == 0)
            {
                btnAudioOutputs.Text =
                    "Select Audio Outputs";
            }
            else if (selectedItems.Count == 1)
            {
                btnAudioOutputs.Text =
                    selectedItems[0].Text;
            }
            else
            {
                btnAudioOutputs.Text =
                    $"{selectedItems[0].Text} + {selectedItems.Count - 1} more";
            }
        }

        private void UpdateApplicationsButtonText()
        {
            var selectedItems =
                ctxApplications.Items
                    .OfType<ToolStripMenuItem>()
                    .Where(item => item.Checked)
                    .ToList();

            if (selectedItems.Count == 0)
            {
                btnApplications.Text =
                    "Select Applications";
            }
            else if (selectedItems.Count == 1)
            {
                btnApplications.Text =
                    selectedItems[0].Text;
            }
            else
            {
                btnApplications.Text =
                    $"{selectedItems[0].Text} + {selectedItems.Count - 1} more";
            }
        }

        private void UpdateHotkeyLabel()
        {
            if (waitingForHotkey)
            {
                lblHotkey.Text =
                    "Recording Hotkey: Press any key...";
            }
            else
            {
                lblHotkey.Text =
                    $"Recording Hotkey: {recordingHotkey}";
            }
        }

        private async Task StartRecording()
        {
            if (recordingManager.IsRecording ||
                isStartingRecording)
            {
                return;
            }

            if (!chkAudioOutput.Checked &&
                !chkIncludeMicrophone.Checked &&
                !chkApplications.Checked)
            {
                lblStatus.Text =
                    "Please select at least one recording source.";

                return;
            }

            List<RunningApplication> selectedApplications =
                chkApplications.Checked
                    ? ApplicationDiscovery.ResolveSavedApplications(
                        appSettings.Applications
                    )
                    : new List<RunningApplication>();

            bool recordApplications =
                chkApplications.Checked &&
                selectedApplications.Count > 0;

            if (!chkAudioOutput.Checked &&
                !chkIncludeMicrophone.Checked &&
                !recordApplications)
            {
                lblStatus.Text =
                    "None of the selected applications are currently running.";

                return;
            }

            string recordingsFolder =
                txtSaveFolder.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    recordingsFolder))
            {
                lblStatus.Text =
                    "Please choose a save folder.";

                return;
            }

            List<MMDevice> selectedOutputDevices =
                new();

            if (chkAudioOutput.Checked)
            {
                foreach (ToolStripMenuItem item in ctxAudioOutputs.Items)
                {
                    if (!item.Checked ||
                        item.Tag is not string deviceId)
                    {
                        continue;
                    }

                    MMDevice? device =
                        audioDevices.FirstOrDefault(
                            d => d.ID == deviceId
                        );

                    if (device != null)
                    {
                        selectedOutputDevices.Add(
                            device
                        );
                    }
                }

                if (selectedOutputDevices.Count == 0)
                {
                    lblStatus.Text =
                        "Please select at least one audio output device.";

                    return;
                }
            }

            MMDevice? microphoneDevice = null;

            if (chkIncludeMicrophone.Checked)
            {
                if (cmbMicrophone.SelectedIndex < 0 ||
                    cmbMicrophone.SelectedIndex >=
                    microphoneDevices.Count)
                {
                    lblStatus.Text =
                        "Please select a microphone.";

                    return;
                }

                microphoneDevice =
                    microphoneDevices[
                        cmbMicrophone.SelectedIndex
                    ];
            }

            isStartingRecording = true;
            SetRecordingControlsEnabled(false);

            try
            {
                await recordingManager.StartRecording(
                    selectedOutputDevices,
                    chkAudioOutput.Checked,
                    microphoneDevice,
                    chkIncludeMicrophone.Checked,
                    selectedApplications,
                    recordApplications,
                    recordingsFolder
                );
            }
            catch (Exception ex)
            {
                SetRecordingControlsEnabled(true);

                lblStatus.Text =
                    "Could not start recording.";

                MessageBox.Show(
                    $"Could not start recording:\n\n{ex.Message}",
                    "Recording Error"
                );
            }
            finally
            {
                isStartingRecording = false;
            }
        }

        private void StopRecording()
        {
            recordingManager.StopRecording();
        }

        private void RecordingManager_StatusChanged(
            string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() =>
                    RecordingManager_StatusChanged(
                        status
                    )
                );

                return;
            }

            lblStatus.Text = status;
        }

        private void RecordingManager_RecordingSaved(
            string filePath,
            int sampleRate,
            int channels)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() =>
                    RecordingManager_RecordingSaved(
                        filePath,
                        sampleRate,
                        channels
                    )
                );

                return;
            }

            SetRecordingControlsEnabled(
                true
            );

            lblStatus.Text =
                $"Saved: " +
                $"{Path.GetFileName(filePath)} " +
                $"({sampleRate} Hz, " +
                $"{channels} channel(s))";
        }

        private void RecordingManager_RecordingFailed(
            Exception exception)
        {
            if (InvokeRequired)
            {
                BeginInvoke(() =>
                    RecordingManager_RecordingFailed(
                        exception
                    )
                );

                return;
            }

            SetRecordingControlsEnabled(
                true
            );

            lblStatus.Text =
                "Recording failed.";

            MessageBox.Show(
                $"Recording failed:\n\n{exception.Message}",
                "Recording Error"
            );
        }

        private void SetRecordingControlsEnabled(
            bool enabled)
        {
            btnStart.Enabled = enabled;
            btnStop.Enabled = !enabled;

            cmbRecordingMode.Enabled = enabled;
            btnChangeHotkey.Enabled = enabled;

            txtSaveFolder.Enabled = enabled;
            btnBrowseFolder.Enabled = enabled;

            chkAudioOutput.Enabled =
                enabled &&
                audioDevices.Count > 0;

            btnAudioOutputs.Enabled =
                enabled &&
                chkAudioOutput.Checked &&
                audioDevices.Count > 0;

            chkIncludeMicrophone.Enabled =
                enabled &&
                microphoneDevices.Count > 0;

            cmbMicrophone.Enabled =
                enabled &&
                chkIncludeMicrophone.Checked &&
                microphoneDevices.Count > 0;

            chkApplications.Enabled =
                enabled;

            btnApplications.Enabled =
                enabled &&
                chkApplications.Checked;
        }

        private async void GlobalKeyDown(
            Keys key)
        {
            if (waitingForHotkey)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(() =>
                        SetNewHotkey(key)
                    );
                }
                else
                {
                    SetNewHotkey(key);
                }

                return;
            }

            if (key != recordingHotkey)
            {
                return;
            }

            if (hotkeyHeld)
            {
                return;
            }

            hotkeyHeld = true;

            if (InvokeRequired)
            {
                BeginInvoke(async () =>
                    await HandleHotkeyDown()
                );
            }
            else
            {
                await HandleHotkeyDown();
            }
        }

        private void GlobalKeyUp(
            Keys key)
        {
            if (key != recordingHotkey)
            {
                return;
            }

            if (ignoreHotkeyRelease)
            {
                ignoreHotkeyRelease = false;
                hotkeyHeld = false;

                return;
            }

            hotkeyHeld = false;

            if (InvokeRequired)
            {
                BeginInvoke(() =>
                    HandleHotkeyUp()
                );
            }
            else
            {
                HandleHotkeyUp();
            }
        }

        private async Task HandleHotkeyDown()
        {
            if (cmbRecordingMode.SelectedIndex == 0)
            {
                await StartRecording();
            }
            else
            {
                if (recordingManager.IsRecording)
                {
                    StopRecording();
                }
                else
                {
                    await StartRecording();
                }
            }
        }

        private void HandleHotkeyUp()
        {
            if (cmbRecordingMode.SelectedIndex == 0)
            {
                StopRecording();
            }
        }

        private void btnChangeHotkey_Click(
            object sender,
            EventArgs e)
        {
            waitingForHotkey = true;
            hotkeyHeld = false;

            UpdateHotkeyLabel();

            btnChangeHotkey.Enabled = false;
        }

        private void SetNewHotkey(
            Keys key)
        {
            recordingHotkey = key;

            waitingForHotkey = false;
            ignoreHotkeyRelease = true;

            UpdateHotkeyLabel();

            btnChangeHotkey.Enabled = true;

            SaveSettings();
        }

        private void btnBrowseFolder_Click(
            object sender,
            EventArgs e)
        {
            using FolderBrowserDialog folderDialog =
                new();

            folderDialog.Description =
                "Choose where recordings should be saved.";

            folderDialog.ShowNewFolderButton =
                true;

            if (Directory.Exists(
                    txtSaveFolder.Text))
            {
                folderDialog.SelectedPath =
                    txtSaveFolder.Text;
            }

            if (folderDialog.ShowDialog() ==
                DialogResult.OK)
            {
                txtSaveFolder.Text =
                    folderDialog.SelectedPath;

                lblStatus.Text =
                    "Save folder changed.";

                SaveSettings();
            }
        }

        private void btnShowRecordings_Click(
            object sender,
            EventArgs e)
        {
            string folderPath =
                txtSaveFolder.Text.Trim();

            if (string.IsNullOrWhiteSpace(
                    folderPath))
            {
                lblStatus.Text =
                    "No save folder selected.";

                return;
            }

            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(
                        folderPath
                    );
                }
                catch
                {
                    lblStatus.Text =
                        "Save folder does not exist.";

                    return;
                }
            }

            try
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = folderPath,
                        UseShellExecute = true
                    }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not open the recordings folder:\n\n{ex.Message}",
                    "Folder Error"
                );
            }
        }

        private void cmbRecordingMode_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            SaveSettings();
        }

        private async void btnStart_Click(
            object sender,
            EventArgs e)
        {
            await StartRecording();
        }

        private void btnStop_Click(
            object sender,
            EventArgs e)
        {
            StopRecording();
        }

        private async void btnOpenAudio_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Title = "Open a WAV recording",
                Filter = "WAV audio (*.wav)|*.wav",
                CheckFileExists = true,
                Multiselect = false
            };
            string recordingsFolder = txtSaveFolder.Text.Trim();
            if (Directory.Exists(recordingsFolder))
            {
                dialog.InitialDirectory = recordingsFolder;
            }
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            try
            {
                tabAudioClipper.Enabled = false;
                btnOpenAudio.Enabled = false;
                UseWaitCursor = true;
                float[] peaks = await Task.Run(() => WaveformAnalyzer.Analyze(dialog.FileName, waveformCancellation.Token));
                if (IsDisposed || Disposing) return;
                // Read metadata only and release the source file immediately.
                using WaveFileReader reader = new(dialog.FileName);
                if (reader.Length == 0 || reader.TotalTime <= TimeSpan.Zero)
                {
                    throw new InvalidDataException("The WAV file contains no usable audio.");
                }
                TimeSpan duration = reader.TotalTime;
                string durationText = duration.TotalHours >= 1
                    ? $"{(int)duration.TotalHours}:{duration.Minutes:00}:{duration.Seconds:00}.{duration.Milliseconds / 10:00}"
                    : $"{(int)duration.TotalMinutes:00}:{duration.Seconds:00}.{duration.Milliseconds / 10:00}";
                clipperPlayback.Open(dialog.FileName);
                txtClipperVolume.Text = "100%";
                clipperWaveform.SetAudio(peaks, duration);
                lastClipperPlaybackError = null;
                UpdateClipperPlaybackControls();
                clipperPlaybackTimer.Start();
                txtClipperFile.Text = dialog.FileName;
                lblClipperDetails.Text = $"Duration: {durationText}   |   {reader.WaveFormat.SampleRate:N0} Hz   |   {reader.WaveFormat.Channels} channel(s)";

            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing || waveformCancellation.IsCancellationRequested) return;
                // Retain the previous selection if the new file cannot be opened.
                MessageBox.Show(this,
                    $"Could not open this WAV recording:\n\n{ex.Message}",
                    "Open Audio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    tabAudioClipper.Enabled = true;
                    btnOpenAudio.Enabled = true;
                    UseWaitCursor = false;
                }
            }
        }

        private bool isSavingClip;

        private async void btnClipperSave_Click(object sender, EventArgs e)
        {
            if (!clipperPlayback.HasAudio || isSavingClip) return;
            if (MessageBox.Show(this,
                "Replace the opened recording with the selected clip and volume? This cannot be undone.",
                "Replace Recording", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            await SaveClipAsync(txtClipperFile.Text, true);
        }

        private async void btnClipperSaveAs_Click(object sender, EventArgs e)
        {
            if (!clipperPlayback.HasAudio || isSavingClip) return;
            using SaveFileDialog dialog = new()
            {
                Title = "Save audio clip as",
                Filter = "WAV audio (*.wav)|*.wav",
                DefaultExt = "wav",
                AddExtension = true,
                OverwritePrompt = true,
                InitialDirectory = Path.GetDirectoryName(txtClipperFile.Text),
                FileName = Path.GetFileNameWithoutExtension(txtClipperFile.Text) + "_clip.wav"
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            await SaveClipAsync(dialog.FileName, File.Exists(dialog.FileName));
        }

        private async Task SaveClipAsync(string destination, bool allowReplace)
        {
            CommitClipperVolume();
            string source = txtClipperFile.Text;
            bool replacingSource = string.Equals(Path.GetFullPath(source), Path.GetFullPath(destination), StringComparison.OrdinalIgnoreCase);
            TimeSpan start = clipperPlayback.SelectionStart;
            TimeSpan end = clipperPlayback.SelectionEnd;
            float volume = clipperPlayback.Volume;
            double oldStart = clipperWaveform.SelectionStartFraction, oldEnd = clipperWaveform.SelectionEndFraction;
            string? temporary = null;
            bool committed = false, releasedSource = false;
            isSavingClip = true;
            tabAudioClipper.Enabled = false;
            clipperPlaybackTimer.Stop();
            UseWaitCursor = true;
            try
            {
                clipperPlayback.Stop();
                temporary = await Task.Run(() => AudioClipExporter.Prepare(source, destination, start, end, volume));
                if (replacingSource)
                {
                    clipperPlayback.Dispose();
                    releasedSource = true;
                }
                await Task.Run(() => AudioClipExporter.Commit(temporary, destination, allowReplace));
                committed = true;
                if (replacingSource)
                {
                    float[] peaks = await Task.Run(() => WaveformAnalyzer.Analyze(source));
                    clipperPlayback.Open(source);
                    clipperWaveform.SetAudio(peaks, clipperPlayback.Duration);
                    txtClipperVolume.Text = "100%";
                    using WaveFileReader reader = new(source);
                    lblClipperDetails.Text = $"Duration: {FormatClipperTime(reader.TotalTime)}   |   {reader.WaveFormat.SampleRate:N0} Hz   |   {reader.WaveFormat.Channels} channel(s)";
                }
                MessageBox.Show(this, $"Saved clip to:\n{destination}", "Audio Clip Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (releasedSource && !committed)
                {
                    try
                    {
                        clipperPlayback.Open(source);
                        clipperPlayback.SetSelection(oldStart, oldEnd);
                        clipperPlayback.Volume = volume;
                    }
                    catch { /* The save error below remains the primary failure. */ }
                }
                MessageBox.Show(this,
                    committed ? $"The clip was saved, but the preview could not reopen:\n\n{ex.Message}" : $"Could not save the clip. The destination was not replaced.\n\n{ex.Message}",
                    "Save Audio Clip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Only the generated temporary output can be removed here.
                if (temporary != null && File.Exists(temporary))
                {
                    try { File.Delete(temporary); } catch { }
                }
                isSavingClip = false;
                tabAudioClipper.Enabled = true;
                UseWaitCursor = false;
                UpdateClipperPlaybackControls();
                if (clipperPlayback.HasAudio) clipperPlaybackTimer.Start();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isSavingClip)
            {
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }


        private static string FormatClipperTime(TimeSpan time)
        {
            return time.TotalHours >= 1
                ? $"{(int)time.TotalHours}:{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds / 10:00}"
                : $"{(int)time.TotalMinutes:00}:{time.Seconds:00}.{time.Milliseconds / 10:00}";
        }

        private void UpdateClipperPlaybackControls()
        {
            bool loaded = clipperPlayback.HasAudio;
            btnClipperSave.Enabled = loaded && !isSavingClip;
            btnClipperSaveAs.Enabled = loaded && !isSavingClip;
            btnClipperPlayPause.Enabled = loaded;
            btnClipperStop.Enabled = loaded;
            trkClipperPosition.Enabled = loaded;
            txtClipperVolume.Enabled = loaded;
            btnClipperPlayPause.Text = clipperPlayback.IsPlaying ? "\u23F8" : "\u25B6";
            btnClipperPlayPause.AccessibleName = clipperPlayback.IsPlaying ? "Pause" : "Play";
            clipperToolTip.SetToolTip(btnClipperPlayPause, clipperPlayback.IsPlaying ? "Pause selected audio" : "Play selected audio");
            UpdateClipperZoomControls();
            double duration = clipperPlayback.Duration.TotalSeconds;
            trkClipperPosition.Value = duration > 0
                ? Math.Clamp((int)(clipperPlayback.Position.TotalSeconds / duration * trkClipperPosition.Maximum), 0, trkClipperPosition.Maximum)
                : 0;
            clipperWaveform.PositionFraction = duration > 0 ? clipperPlayback.Position.TotalSeconds / duration : 0;
            lblClipperSelection.Text = loaded
                ? $"Start: {FormatClipperTime(clipperPlayback.SelectionStart)}   End: {FormatClipperTime(clipperPlayback.SelectionEnd)}   Duration: {FormatClipperTime(clipperPlayback.SelectionEnd - clipperPlayback.SelectionStart)}"
                : "Start: --   End: --   Duration: --";
            lblClipperPosition.Text = $"{FormatClipperTime(clipperPlayback.Position)} / {FormatClipperTime(clipperPlayback.Duration)}";
        }

        private void RunClipperPlaybackAction(Action action)
        {
            try { action(); }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Audio Playback", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            UpdateClipperPlaybackControls();
        }

        private void btnClipperPlayPause_Click(object sender, EventArgs e)
            => RunClipperPlaybackAction(clipperPlayback.TogglePlayback);

        private void btnClipperStop_Click(object sender, EventArgs e)
            => RunClipperPlaybackAction(clipperPlayback.Stop);

        private void trkClipperPosition_Scroll(object sender, EventArgs e)
            => RunClipperPlaybackAction(() => clipperPlayback.Seek((double)trkClipperPosition.Value / trkClipperPosition.Maximum));

        private void clipperWaveform_VolumeChanged(object? sender, EventArgs e)
        {
            clipperPlayback.Volume = clipperWaveform.VolumePercent / 100f;
            txtClipperVolume.Text = $"{clipperWaveform.VolumePercent}%";
        }

        private void CommitClipperVolume()
        {
            string input = txtClipperVolume.Text.Trim().TrimEnd('%').Trim();
            if (int.TryParse(input, out int value) && value >= 0 && value <= 200)
            {
                clipperWaveform.VolumePercent = value;
            }
            txtClipperVolume.Text = $"{clipperWaveform.VolumePercent}%";
        }

        private void txtClipperVolume_Leave(object? sender, EventArgs e) => CommitClipperVolume();
        private void txtClipperVolume_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommitClipperVolume();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                txtClipperVolume.Text = $"{clipperWaveform.VolumePercent}%";
                e.SuppressKeyPress = true;
            }
        }

        private void UpdateClipperZoomControls()
        {
            bool loaded = clipperPlayback.HasAudio;
            btnClipperSave.Enabled = loaded && !isSavingClip;
            btnClipperSaveAs.Enabled = loaded && !isSavingClip;
            btnClipperZoomIn.Enabled = loaded && clipperWaveform.ZoomFactor < 8;
            btnClipperZoomOut.Enabled = loaded && clipperWaveform.ZoomFactor > 1;
            btnClipperFit.Enabled = loaded && clipperWaveform.ZoomFactor > 1;
            lblClipperZoom.Text = $"{clipperWaveform.ZoomFactor:0}x";
            hsbClipperWaveform.LargeChange = Math.Clamp((int)Math.Round(clipperWaveform.ViewSpanFraction * 10000), 1, 10000);
            hsbClipperWaveform.SmallChange = Math.Max(1, hsbClipperWaveform.LargeChange / 10);
            hsbClipperWaveform.Value = Math.Clamp((int)Math.Round(clipperWaveform.ViewStartFraction * 10000), 0, 10000 - hsbClipperWaveform.LargeChange);
            hsbClipperWaveform.Enabled = loaded && clipperWaveform.ZoomFactor > 1;
        }

        private void btnClipperZoomIn_Click(object sender, EventArgs e) => clipperWaveform.ZoomIn();
        private void btnClipperZoomOut_Click(object sender, EventArgs e) => clipperWaveform.ZoomOut();
        private void btnClipperFit_Click(object sender, EventArgs e) => clipperWaveform.Fit();
        private void hsbClipperWaveform_Scroll(object sender, ScrollEventArgs e) => clipperWaveform.ScrollTo(e.NewValue / 10000d);
        private void clipperWaveform_ViewChanged(object? sender, EventArgs e) => UpdateClipperZoomControls();

        private void clipperWaveform_SelectionChanged(object? sender, EventArgs e)
            => RunClipperPlaybackAction(() => clipperPlayback.SetSelection(
                clipperWaveform.SelectionStartFraction, clipperWaveform.SelectionEndFraction));

        private void clipperWaveform_SeekRequested(object? sender, WaveformSeekEventArgs e)
            => RunClipperPlaybackAction(() => clipperPlayback.Seek(e.Fraction));

        private void clipperPlaybackTimer_Tick(object sender, EventArgs e)
        {
            UpdateClipperPlaybackControls();
            if (clipperPlayback.PlaybackError is Exception error && !ReferenceEquals(error, lastClipperPlaybackError))
            {
                lastClipperPlaybackError = error;
                MessageBox.Show(this, error.Message, "Audio Playback", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void MainForm_Load(
            object sender,
            EventArgs e)
        {
        }

        protected override void OnFormClosed(
            FormClosedEventArgs e)
        {
            SaveSettings();

            waveformCancellation.Cancel();
            clipperPlaybackTimer.Stop();
            clipperPlayback.Dispose();
            recordingManager.Dispose();

            keyboardHook?.Dispose();
            deviceEnumerator.Dispose();

            base.OnFormClosed(e);
        }
    }
}