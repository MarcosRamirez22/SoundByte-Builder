using System.ComponentModel;

namespace SoundByte_Builder
{
    public sealed class WaveformControl : Control
    {
        private float[] peaks = Array.Empty<float>();
        private TimeSpan duration;
        private double positionFraction;
        public event EventHandler<WaveformSeekEventArgs>? SeekRequested;
        public event EventHandler? SelectionChanged;
        private int draggingHandle;
        private double selectionStart;
        private double selectionEnd = 1;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double SelectionStartFraction => selectionStart;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double SelectionEndFraction => selectionEnd;

        private int volumePercent = 100;
        private bool draggingVolume;
        public event EventHandler? VolumeChanged;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VolumePercent
        {
            get => volumePercent;
            set
            {
                int next = Math.Clamp(value, 0, 200);
                if (next == volumePercent) return;
                volumePercent = next;
                Invalidate();
                VolumeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private int VolumeLineY => Plot.Bottom - (int)Math.Round(volumePercent / 200d * Plot.Height);
        private bool HitVolume(Point point) => point.X >= Plot.Left + 14 && point.X <= Plot.Right - 14 && Math.Abs(point.Y - VolumeLineY) <= 5;

        private double zoomFactor = 1;
        private double viewStart;
        public event EventHandler? ViewChanged;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ZoomFactor => zoomFactor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ViewStartFraction => viewStart;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ViewSpanFraction => 1 / zoomFactor;

        public void ZoomIn() => SetZoom(zoomFactor * 2);
        public void ZoomOut() => SetZoom(zoomFactor / 2);
        public void Fit() => SetZoom(1);

        private void SetZoom(double factor)
        {
            if (peaks.Length == 0) return;
            double center = IsVisible(positionFraction) ? positionFraction : viewStart + ViewSpanFraction / 2;
            zoomFactor = Math.Clamp(factor, 1, 8);
            viewStart = Math.Clamp(center - ViewSpanFraction / 2, 0, 1 - ViewSpanFraction);
            Invalidate();
            ViewChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ScrollTo(double fraction)
        {
            viewStart = Math.Clamp(fraction, 0, 1 - ViewSpanFraction);
            Invalidate();
            ViewChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool IsVisible(double fraction) => fraction >= viewStart && fraction <= viewStart + ViewSpanFraction;

        public WaveformControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            BackColor = Color.WhiteSmoke;
            ForeColor = Color.SteelBlue;
            AccessibleName = "Recording waveform";
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double PositionFraction
        {
            get => positionFraction;
            set { positionFraction = Math.Clamp(value, 0, 1); Invalidate(); }
        }

        public void SetAudio(float[] overview, TimeSpan totalTime)
        {
            peaks = overview;
            duration = totalTime;
            positionFraction = 0;
            selectionStart = 0;
            selectionEnd = 1;
            volumePercent = 100;
            draggingVolume = false;
            draggingHandle = 0;
            wheelRemainder = 0;
            zoomFactor = 1;
            viewStart = 0;
            Invalidate();
            ViewChanged?.Invoke(this, EventArgs.Empty);
        }

        private Rectangle Plot => new(8, 25, Math.Max(1, ClientSize.Width - 17), Math.Max(1, ClientSize.Height - 34));

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (ClientSize.Width < 20 || ClientSize.Height < 40) return;
            Rectangle plot = Plot;
            using Pen border = new(SystemColors.ControlDark);
            e.Graphics.DrawRectangle(border, 0, 0, Width - 1, Height - 1);
            if (peaks.Length == 0)
            {
                TextRenderer.DrawText(e.Graphics, "Open a WAV recording to view its waveform", Font,
                    ClientRectangle, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
                return;
            }
            using SolidBrush shade = new(Color.FromArgb(45, Color.SteelBlue));
            int shadeStart = Math.Clamp(XAt(selectionStart), plot.Left, plot.Right);
            int shadeEnd = Math.Clamp(XAt(selectionEnd), plot.Left, plot.Right);
            e.Graphics.FillRectangle(shade, shadeStart, plot.Top, Math.Max(0, shadeEnd - shadeStart), plot.Height);
            using Pen wave = new(ForeColor);
            float middle = plot.Top + plot.Height / 2f;
            e.Graphics.DrawLine(border, plot.Left, middle, plot.Right, middle);
            for (int x = 0; x < plot.Width; x++)
            {
                int first = Math.Clamp((int)((viewStart + (double)x / plot.Width * ViewSpanFraction) * peaks.Length), 0, peaks.Length - 1);
                int end = Math.Min(peaks.Length, Math.Max(first + 1, (int)((viewStart + (double)(x + 1) / plot.Width * ViewSpanFraction) * peaks.Length)));
                float peak = 0;
                for (int i = first; i < end; i++) peak = Math.Max(peak, peaks[i]);
                float height = Math.Min(1, peak * volumePercent / 100f) * plot.Height / 2f;
                e.Graphics.DrawLine(wave, plot.Left + x, middle - height, plot.Left + x, middle + height);
            }
            int divisions = Math.Max(1, plot.Width / 85);
            for (int i = 0; i <= divisions; i++)
            {
                double fraction = (double)i / divisions;
                TimeSpan time = TimeSpan.FromTicks((long)(duration.Ticks * (viewStart + fraction * ViewSpanFraction)));
                string label = time.TotalHours >= 1
                    ? $"{(int)time.TotalHours}:{time.Minutes:00}:{time.Seconds:00}"
                    : $"{(int)time.TotalMinutes}:{time.Seconds:00}.{time.Milliseconds / 10:00}";
                Size size = TextRenderer.MeasureText(label, Font);
                int x = plot.Left + (int)(fraction * plot.Width);
                int labelX = Math.Clamp(x - size.Width / 2, 1, Math.Max(1, Width - size.Width - 1));
                TextRenderer.DrawText(e.Graphics, label, Font, new Point(labelX, 3), ForeColor);
                e.Graphics.DrawLine(border, x, 21, x, 25);
            }
            using Pen volumeLine = new(Color.DarkSlateGray, 2);
            e.Graphics.DrawLine(volumeLine, plot.Left, VolumeLineY, plot.Right, VolumeLineY);
            DrawHandle(e.Graphics, selectionStart, Color.SeaGreen, true);
            DrawHandle(e.Graphics, selectionEnd, Color.DarkOrange, false);
            using Pen playhead = new(Color.Firebrick, 2);
            float playheadX = XAt(positionFraction);
            if (IsVisible(positionFraction)) e.Graphics.DrawLine(playhead, playheadX, plot.Top, playheadX, plot.Bottom);
        }

        private int XAt(double fraction) => Plot.Left + (int)Math.Round((fraction - viewStart) / ViewSpanFraction * (Plot.Width - 1));
        private double FractionAt(int x) => Math.Clamp(viewStart + Math.Clamp((double)(x - Plot.Left) / Math.Max(1, Plot.Width - 1), 0, 1) * ViewSpanFraction, 0, 1);

        private void DrawHandle(Graphics graphics, double fraction, Color color, bool isStart)
        {
            if (!IsVisible(fraction)) return;
            int x = XAt(fraction);
            using Pen line = new(color, 2);
            using SolidBrush brush = new(color);
            graphics.DrawLine(line, x, Plot.Top, x, Plot.Bottom);
            int left = isStart ? x : x - 12;
            graphics.FillRectangle(brush, left, Plot.Bottom - 16, 12, 16);
            TextRenderer.DrawText(graphics, isStart ? "S" : "E", Font,
                new Rectangle(left, Plot.Bottom - 16, 12, 16), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private int HitHandle(int x)
        {
            int startDistance = IsVisible(selectionStart) ? Math.Abs(x - XAt(selectionStart)) : int.MaxValue;
            int endDistance = IsVisible(selectionEnd) ? Math.Abs(x - XAt(selectionEnd)) : int.MaxValue;
            if (Math.Min(startDistance, endDistance) > 12) return 0;
            return startDistance <= endDistance ? 1 : 2;
        }

        private int wheelRemainder;
        private bool wheelScrollMode;

        public void ApplyMouseWheel(int delta, bool scrollMode)
        {
            if (peaks.Length == 0 || draggingHandle != 0 || draggingVolume) return;
            if (wheelScrollMode != scrollMode) wheelRemainder = 0;
            wheelScrollMode = scrollMode;
            wheelRemainder += delta;
            int steps = wheelRemainder / 120;
            wheelRemainder %= 120;
            if (steps == 0) return;
            if (scrollMode)
            {
                if (zoomFactor > 1) ScrollTo(viewStart - steps * ViewSpanFraction / 10);
            }
            else
            {
                int direction = Math.Sign(steps);
                for (int i = 0; i < Math.Min(Math.Abs(steps), 4); i++)
                    if (direction > 0) ZoomIn(); else ZoomOut();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            ApplyMouseWheel(e.Delta, (ModifierKeys & Keys.Shift) != 0);
            if (e is HandledMouseEventArgs handled) handled.Handled = true;
            base.OnMouseWheel(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            // Give the hovered waveform wheel input only while this app is active.
            if (peaks.Length > 0 && FindForm()?.ContainsFocus == true) Focus();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left || peaks.Length == 0) return;
            draggingHandle = HitHandle(e.X);
            if (draggingHandle == 0 && HitVolume(e.Location)) { draggingVolume = true; Capture = true; return; }
            if (draggingHandle != 0) { Capture = true; return; }
            SeekRequested?.Invoke(this, new WaveformSeekEventArgs(FractionAt(e.X)));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (peaks.Length == 0) return;
            if (draggingVolume)
            {
                VolumePercent = (int)Math.Round(Math.Clamp((double)(Plot.Bottom - e.Y) / Plot.Height, 0, 1) * 200);
                return;
            }
            if (draggingHandle == 0)
            {
                Cursor = HitHandle(e.X) != 0 ? Cursors.SizeWE : HitVolume(e.Location) ? Cursors.SizeNS : Cursors.Hand;
                return;
            }
            double minimumGap = Math.Min(1, .01 / Math.Max(.01, duration.TotalSeconds));
            if (draggingHandle == 1) selectionStart = Math.Clamp(FractionAt(e.X), 0, Math.Max(0, selectionEnd - minimumGap));
            else selectionEnd = Math.Clamp(FractionAt(e.X), Math.Min(1, selectionStart + minimumGap), 1);
            Invalidate();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            draggingHandle = 0;
            draggingVolume = false;
            Capture = false;
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
            if (!Capture) { draggingHandle = 0; draggingVolume = false; }
        }
    }

    public sealed class WaveformSeekEventArgs(double fraction) : EventArgs
    {
        public double Fraction { get; } = fraction;
    }
}
