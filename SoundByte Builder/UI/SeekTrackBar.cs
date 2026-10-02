using System.Runtime.InteropServices;

namespace SoundByte_Builder
{
    public sealed class SeekTrackBar : TrackBar
    {
        private bool seeking;
        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRectangle { public int Left, Top, Right, Bottom; }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr handle, int message, IntPtr parameter, ref NativeRectangle rectangle);

        private void SeekAt(int x)
        {
            NativeRectangle channel = default, thumb = default;
            SendMessage(Handle, 0x041A, IntPtr.Zero, ref channel); // TBM_GETCHANNELRECT
            SendMessage(Handle, 0x0419, IntPtr.Zero, ref thumb); // TBM_GETTHUMBRECT
            int halfThumb = (thumb.Right - thumb.Left) / 2;
            int left = channel.Left + halfThumb;
            int right = channel.Right - halfThumb;
            double fraction = Math.Clamp((double)(x - left) / Math.Max(1, right - left), 0, 1);
            Value = Minimum + (int)Math.Round(fraction * (Maximum - Minimum));
            OnScroll(EventArgs.Empty);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) { base.OnMouseDown(e); return; }
            Focus();
            seeking = true;
            Capture = true;
            SeekAt(e.X);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (seeking) SeekAt(e.X);
            else base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            seeking = false;
            Capture = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            if (!Capture) seeking = false;
            base.OnMouseCaptureChanged(e);
        }
    }
}
