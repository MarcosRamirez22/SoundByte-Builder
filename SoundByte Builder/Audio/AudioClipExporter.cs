using NAudio.Wave;
using System.Runtime.InteropServices;

namespace SoundByte_Builder.Audio
{
    public static class AudioClipExporter
    {
        // Finish a new WAV before touching the destination. The caller releases
        // playback's source handle before committing an overwrite.
        public static string Prepare(string source, string destination, TimeSpan start, TimeSpan end, float volume)
        {
            if (!float.IsFinite(volume) || volume < 0 || volume > 2) throw new ArgumentOutOfRangeException(nameof(volume));
            string directory = Path.GetDirectoryName(Path.GetFullPath(destination))!;
            string temporary = Path.Combine(directory, $".soundbyte-{Guid.NewGuid():N}.wav");
            try
            {
                using AudioFileReader reader = new(source);
                int channels = reader.WaveFormat.Channels;
                long totalFrames = reader.Length / reader.WaveFormat.BlockAlign;
                long first = (long)Math.Round(start.TotalSeconds * reader.WaveFormat.SampleRate);
                long last = (long)Math.Round(end.TotalSeconds * reader.WaveFormat.SampleRate);
                if (first < 0 || last > totalFrames || last <= first) throw new InvalidDataException("Select a non-empty region within the recording.");
                reader.Position = first * reader.WaveFormat.BlockAlign;
                reader.Volume = volume;
                using WaveFileWriter writer = new(temporary, reader.WaveFormat);
                float[] samples = new float[4096 * channels];
                long remaining = (last - first) * channels;
                while (remaining > 0)
                {
                    int requested = (int)Math.Min(samples.Length, remaining);
                    int count = reader.Read(samples.AsSpan(0, requested));
                    if (count <= 0) throw new EndOfStreamException("The recording ended before the selected region.");
                    for (int i = 0; i < count; i++) samples[i] = float.IsFinite(samples[i]) ? Math.Clamp(samples[i], -1, 1) : 0;
                    writer.Write(MemoryMarshal.AsBytes(samples.AsSpan(0, count)));
                    remaining -= count;
                }
                return temporary;
            }
            catch
            {
                if (File.Exists(temporary)) File.Delete(temporary);
                throw;
            }
        }

        public static void Commit(string temporary, string destination, bool allowReplace)
        {
            if (File.Exists(destination) && allowReplace) File.Replace(temporary, destination, null);
            else File.Move(temporary, destination); // Never silently overwrite an unapproved file.
        }
    }
}
