using NAudio.Wave;

namespace SoundByte_Builder.Audio
{
    // Limits reads at the audio-frame boundary, so preview cannot play past End.
    public sealed class SelectedWaveProvider : IWaveProvider
    {
        private readonly AudioFileReader reader;
        private long start;
        private long end;
        public WaveFormat WaveFormat => reader.WaveFormat;
        public TimeSpan Start => TimeSpan.FromSeconds((double)start / WaveFormat.AverageBytesPerSecond);
        public TimeSpan End => TimeSpan.FromSeconds((double)end / WaveFormat.AverageBytesPerSecond);

        public SelectedWaveProvider(AudioFileReader reader)
        {
            this.reader = reader;
            end = reader.Length;
        }

        public void SetRange(double startFraction, double endFraction)
        {
            long frames = reader.Length / WaveFormat.BlockAlign;
            long startFrame = Math.Clamp((long)(Math.Clamp(startFraction, 0, 1) * frames), 0, Math.Max(0, frames - 1));
            long endFrame = Math.Clamp((long)(Math.Clamp(endFraction, 0, 1) * frames), startFrame + 1, frames);
            start = startFrame * WaveFormat.BlockAlign;
            end = endFrame * WaveFormat.BlockAlign;
            reader.Position = start;
        }

        public void Rewind() => reader.Position = start;
        public bool AtEnd => reader.Position >= end;
        public void Seek(double fraction)
        {
            long position = (long)(reader.Length * Math.Clamp(fraction, 0, 1));
            position -= position % WaveFormat.BlockAlign;
            reader.Position = Math.Clamp(position, start, end);
        }

        public int Read(Span<byte> buffer)
        {
            if (reader.Position < start) reader.Position = start;
            int count = (int)Math.Min(buffer.Length, Math.Max(0, end - reader.Position));
            count -= count % WaveFormat.BlockAlign;
            return count == 0 ? 0 : reader.Read(buffer[..count]);
        }
    }
}
