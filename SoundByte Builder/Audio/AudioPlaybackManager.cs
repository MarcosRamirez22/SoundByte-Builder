using NAudio.Wave;

namespace SoundByte_Builder.Audio
{
    public sealed class AudioPlaybackManager : IDisposable
    {
        private AudioFileReader? reader;
        private WaveOut? output;
        private SelectedWaveProvider? selection;
        public TimeSpan SelectionStart => selection?.Start ?? TimeSpan.Zero;
        public TimeSpan SelectionEnd => selection?.End ?? TimeSpan.Zero;
        public bool HasAudio => reader != null;
        public bool IsPlaying => output?.PlaybackState == PlaybackState.Playing;
        public TimeSpan Duration => reader?.TotalTime ?? TimeSpan.Zero;
        public TimeSpan Position => reader?.CurrentTime ?? TimeSpan.Zero;
        public Exception? PlaybackError { get; private set; }

        private float volume = 1;
        public float Volume
        {
            get => volume;
            set
            {
                if (!float.IsFinite(value)) throw new ArgumentOutOfRangeException(nameof(value));
                volume = Math.Clamp(value, 0, 2);
                if (reader != null) reader.Volume = volume;
            }
        }

        public void Open(string path)
        {
            AudioFileReader? nextReader = null;
            WaveOut? nextOutput = null;
            SelectedWaveProvider? nextSelection = null;
            try
            {
                nextReader = new AudioFileReader(path);
                if (nextReader.Length == 0 || nextReader.TotalTime <= TimeSpan.Zero)
                    throw new InvalidDataException("The WAV file contains no usable audio.");
                nextOutput = new WaveOut();
                nextSelection = new SelectedWaveProvider(nextReader);
                nextOutput.Init(nextSelection);
            }
            catch
            {
                nextOutput?.Dispose();
                nextReader?.Dispose();
                throw;
            }
            Dispose();
            reader = nextReader;
            volume = 1;
            reader.Volume = volume;
            output = nextOutput;
            selection = nextSelection;
            PlaybackError = null;
            output.PlaybackStopped += Output_PlaybackStopped;
        }

        private void Output_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (ReferenceEquals(sender, output)) PlaybackError = e.Exception;
        }

        public void TogglePlayback()
        {
            if (reader == null || output == null) return;
            if (IsPlaying) output.Pause();
            else
            {
                if (selection!.AtEnd) selection.Rewind();
                PlaybackError = null;
                output.Play();
            }
        }

        public void Stop()
        {
            output?.Stop();
            selection?.Rewind();
        }

        public void Seek(double fraction)
        {
            if (reader == null || output == null) return;
            bool resume = IsPlaying;
            output.Stop();
            selection!.Seek(fraction);
            if (resume && !selection!.AtEnd) output.Play();
        }

        public void SetSelection(double startFraction, double endFraction)
        {
            if (output == null || selection == null) return;
            output.Stop();
            selection.SetRange(startFraction, endFraction);
        }

        public void Dispose()
        {
            if (output != null)
            {
                output.PlaybackStopped -= Output_PlaybackStopped;
                output.Dispose();
                output = null;
            }
            reader?.Dispose();
            reader = null;
            selection = null;
        }
    }
}
