Soundbyte Builder

Soundbyte Builder is a Windows audio recorder and WAV clip editor. Record audio from outputs, microphones, and running applications, then use the Audio Clipper to trim recordings and adjust their volume.

The project started as a soundboard recording utility and has expanded into a multi-source recorder with a built-in clip editor.

## Features

### Recorder

- Record from one or more audio output devices
- Record from a microphone or specific running applications
- Record multiple applications at the same time
- Mix multiple recording sources into a single WAV file
- Remember selected applications, audio devices, and microphone settings
- Show saved applications as offline when they are not running
- Use global recording hotkeys with Hold to Record or Press to Toggle modes
- Choose a custom recording folder and quickly open saved recordings
- Automatically convert incompatible multichannel recordings
- Apply multichannel volume compensation for sources with more than two channels

### Audio Clipper

- Open WAV recordings in a separate tab within the main window
- View a waveform, time ruler, and moving playhead
- Preview audio with Play/Pause and Stop controls
- Click the waveform or seek slider to move through the recording
- Select a clip using draggable Start and End handles
- See the selection's Start, End, and Duration
- Preview only the selected region
- Zoom from 1x to 8x and scroll horizontally through the waveform
- Adjust volume from 0% to 200% using a draggable horizontal waveform line or percentage field
- See the waveform height change with the selected volume
- Export the selection and volume with Save or Save As

## Recording Sources

### Audio Outputs

Record everything playing through selected Windows audio output devices. Multiple output devices can be selected at the same time.

### Applications

Record audio directly from selected running applications, shown using friendly names when possible.

Previously selected applications are remembered between launches. Applications that are not running appear as `Application Name (Offline)` and are skipped until they are running again.

### Microphone

Record a microphone by itself or combine it with audio outputs and applications.

## Source Combinations

Supported combinations include:

- Audio output only, including multiple outputs
- Microphone only
- Application only, including multiple applications
- Audio output + microphone
- Application + microphone
- Audio output + application
- Multiple outputs + multiple applications + microphone

### Duplicate Audio

If an application is selected directly and its audio is also captured through a selected output device, it may be recorded twice.

For example, selecting both Discord and the headphones playing Discord's audio can make that audio louder in the final mix.

## Using the Audio Clipper

1. Switch to the **Audio Clipper** tab and select **Open Audio**.
2. Open a WAV recording.
3. Drag the green **Start** and orange **End** handles to select a region.
4. Use **Play/Pause** to preview the selection. **Stop** returns to the Start handle.
5. Adjust volume by dragging the horizontal line up or down, or enter a percentage in the **Volume** field and press Enter. **100%** is the original volume; **0%** mutes the clip.
6. Select **Save As** to create another WAV, or **Save** to replace the opened recording after confirmation.

### Waveform Navigation

- **Click the waveform:** seek to that position within the selected region
- **Zoom In / Zoom Out:** change waveform magnification
- **Fit:** show the whole recording
- **Mouse wheel over the waveform:** zoom in or out
- **Shift + mouse wheel:** scroll horizontally while zoomed in
- **Horizontal scrollbar:** move through the zoomed recording

Dragging a trim handle stops playback and updates the selection. Zooming and scrolling preserve the trim selection.

### Saving Clips

**Save As** exports the selected region and volume to another WAV while keeping the current recording open, so you can create multiple clips. Choosing an existing destination prompts before overwriting it.

**Save** asks before replacing the opened recording. After saving, the edited file reloads with the whole clip selected and volume reset to 100%, so the edits are not applied twice.

Exports are completed in a temporary file before the destination is replaced. Editing and previewing do not change the source recording; replacing it with Save is permanent.

## Audio Format

Mixed recordings use:

- 48 kHz sample rate
- Stereo
- 32-bit floating-point audio

Single-source recordings may retain their original sample rate. Sources with more than two channels are converted to stereo before final output.

Audio Clipper exports use **32-bit floating-point WAV**, preserving the opened recording's sample rate and channel count. Volume boosts are limited to the valid output amplitude range during export.

## System Requirements

- Windows 10 version 2004 (build 19041) or newer
- 64-bit Windows recommended

Application-specific recording relies on Windows process loopback capture. Self-contained releases include the required .NET runtime; framework-dependent builds require the .NET 9 Desktop Runtime.

## Download

Download the Windows release assets from [GitHub Releases](https://github.com/MarcosRamirez22/SoundByte-Builder/releases).

The current project produces `Soundboard Recorder.exe`. Published asset names may differ.

This release is unsigned, so Windows may display an unknown-publisher or SmartScreen warning. Code signing is planned for a future release.

## Building From Source

### Requirements

- Visual Studio with .NET 9 support
- .NET desktop development workload
- .NET 9 SDK

NAudio 3.1.0 is restored through NuGet.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/MarcosRamirez22/SoundByte-Builder.git
   ```

2. Open `SoundByte Builder/SoundByte Builder.csproj` in Visual Studio.
3. Restore NuGet packages.
4. Build and run the project.

The UI layout is maintained in WinForms Designer files. Open `UI/MainForm.cs` with **View Designer** to adjust the layout.

## Project Structure

```text
SoundByte-Builder
├── LICENSE.txt
├── README.md
├── SoundByte Builder.sln
└── SoundByte Builder
    ├── Audio
    │   ├── AudioClipExporter.cs
    │   ├── AudioConverter.cs
    │   ├── AudioMixer.cs
    │   ├── AudioPlaybackManager.cs
    │   ├── RecordingManager.cs
    │   ├── SelectedWaveProvider.cs
    │   └── WaveformAnalyzer.cs
    ├── Input
    │   └── GlobalKeyboardHook.cs
    ├── Models
    │   ├── AppSettings.cs
    │   ├── RunningApplication.cs
    │   └── SavedApplication.cs
    ├── Programs
    │   └── ApplicationDiscovery.cs
    ├── UI
    │   ├── MainForm.cs
    │   ├── MainForm.Designer.cs
    │   ├── MainForm.resx
    │   ├── SeekTrackBar.cs
    │   └── WaveformControl.cs
    ├── Program.cs
    └── SoundByte Builder.csproj
```

Audio processing lives in `Audio`, UI controls and layout in `UI`, keyboard handling in `Input`, and application discovery in `Programs`.

## Planned Features

Possible future improvements include:

- UI color, icon, and visual polish
- Per-source recording volume controls
- More advanced microphone controls
- Improved multi-source mixing controls
- Additional application recording options
- Additional audio editing tools

## Current Status

Soundbyte Builder is in active development. Features and behavior may change between releases.

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt) for details.
