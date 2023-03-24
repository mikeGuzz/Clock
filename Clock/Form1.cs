using System.CodeDom;
using System.Drawing.Drawing2D;

namespace Clock
{
    public partial class Form1 : Form
    {
        private Pen markupPen = new Pen(Color.DarkGray, 2.5f);
        private Pen outlinePen = new Pen(Color.Black, 3);
        private Pen secondLinePen = new Pen(Color.Black, 2f);
        private Pen minuteLinePen = new Pen(Color.Black, 2f);
        private Pen hourLinePen = new Pen(Color.DarkRed, 3f);
        private DateTime time;

        public Form1()
        {
            InitializeComponent();

            time = DateTime.Now;

            timer1.Interval = 1000;
            timer1.Start();

            TopMost = true;
            MinimumSize = Size;

            markupPen.StartCap = LineCap.Triangle;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var offset = 75;
            var width = Math.Min(ClientSize.Width, ClientSize.Height) - offset;
            var rect = new Rectangle(ClientRectangle.X + ClientSize.Width / 2 - width / 2, ClientRectangle.Y + ClientSize.Height / 2 - width / 2, width, width);//width
            var g = e.Graphics;
            var center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var t = new Matrix())
            {
                for (int i = 0; i < 12; i++)
                {
                    t.RotateAt(360f * (i / 12f), center);
                    g.Transform = t;
                    g.DrawLine(markupPen, new Point(center.X, rect.Y + 20), new Point(center.X, rect.Y));
                    t.Reset();
                }
            }
            g.ResetTransform();
            g.DrawEllipse(outlinePen, rect);

            using (var t = new Matrix())
            {
                t.RotateAt(360f * (time.Hour / 24f), center);
                g.Transform = t;
                g.DrawLine(hourLinePen, new Point(center.X, center.Y + 25), new Point(center.X, center.Y - (width / 2 - 50)));
                t.Reset();

                t.RotateAt(360f * (time.Minute / 60f), center);
                g.Transform = t;
                g.DrawLine(minuteLinePen, new Point(center.X, center.Y + 25), new Point(center.X, center.Y - (width / 2 - 20)));
                t.Reset();

                t.RotateAt(360f * (time.Second / 60f), center);
                g.Transform = t;
                g.DrawLine(secondLinePen, new Point(center.X, center.Y + 35), new Point(center.X, center.Y - (width / 2 - 20)));
                g.ResetTransform();
            }

            var centerRext = new Rectangle(center.X - 5, center.Y - 5, 10, 10);
            g.FillEllipse(Brushes.Black, centerRext);
            g.DrawEllipse(outlinePen, centerRext);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time = time.AddSeconds(1);
            Invalidate();
        }
    }
}