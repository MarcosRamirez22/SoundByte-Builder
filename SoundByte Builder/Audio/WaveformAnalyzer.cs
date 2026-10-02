using NAudio.Wave;

namespace SoundByte_Builder.Audio
{
    public static class WaveformAnalyzer
    {
        // Keep a compact overview, regardless of the recording's length.
        public static float[] Analyze(string path, CancellationToken cancellationToken = default)
        {
            using AudioFileReader reader = new(path);
            const int bucketCount = 4096;
            long sampleCount = reader.Length / sizeof(float);
            if (sampleCount <= 0) throw new InvalidDataException("The recording contains no audio.");
            float[] peaks = new float[bucketCount];
            float[] buffer = new float[8192];
            long sampleIndex = 0;
            int count;
            while ((count = reader.Read(buffer.AsSpan())) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < count; i++, sampleIndex++)
                {
                    int bucket = (int)Math.Min(bucketCount - 1, sampleIndex * bucketCount / sampleCount);
                    float value = Math.Abs(buffer[i]);
                    if (float.IsFinite(value)) peaks[bucket] = Math.Max(peaks[bucket], Math.Min(value, 1));
                }
            }
            return peaks;
        }
    }
}
