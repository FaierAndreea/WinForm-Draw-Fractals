namespace Fractals {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            using var g = Graphics.FromImage(pictureBox1.Image);
            Point[] points = {
                new Point(10,20),
                new Point(20,30),
                new Point(10,40),
            };
            g.DrawPolygon(Pens.Blue, points);
            pictureBox1.Refresh();
        }

        #region Draw_Circles
        private void button1_Click(object sender, EventArgs e) {
            using var g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(SystemColors.GradientActiveCaption);

            int initialRadius = 120;
            int centerX = 300;
            int centerY = 230;
            double factor = 0.45; 
            int recursionDepth = Convert.ToInt32(numericUpDown1.Value);

            DrawRecursionStage(centerX, centerY, initialRadius, g, 1);

            pictureBox1.Refresh();

            void DrawRecursionStage(int x, int y, int radius, Graphics g, int depth) {
                if (depth > recursionDepth)
                    return;

                DrawCircle(x, y, radius, g);

                if (recursionDepth > 1) {
                    int newRadius = (int)(radius * factor);
                    DrawRecursionStage(x - radius, y, newRadius, g, depth+1);
                    DrawRecursionStage(x, y - radius, newRadius, g, depth+1);
                    DrawRecursionStage(x + radius, y, newRadius, g, depth+1);
                    DrawRecursionStage(x, y + radius, newRadius, g, depth+1);
                }
            }

            void DrawCircle(int x, int y, int radius, Graphics g) {
                g.FillEllipse(Brushes.Black, x - radius, y - radius, 2 * radius, 2 * radius);
            }
        }
        #endregion

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) {

        }

        #region Draw_Triangles
        private void button2_Click(object sender, EventArgs e) {
            using var g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(SystemColors.GradientActiveCaption);

            float s60 = MathF.Sin((float)(60 * Math.PI / 180.0));
            float c60 = MathF.Cos((float)(60 * Math.PI / 180.0));
            PointF initial_p1 = new PointF(230, 300);
            PointF initial_p2 = new PointF(450, 300);
            PointF[] points = new PointF[3];

            int recursionDepth = Convert.ToInt32(numericUpDown1.Value);

            DrawRecursionTriangles(points, initial_p1, initial_p2, 1);

            pictureBox1.Refresh();

            void DrawRecursionTriangles(PointF[] points, PointF p1, PointF p2, int depth) {
                g.FillPolygon(Brushes.Black, points);

                if (depth > recursionDepth)
                    return;

                var p3 = FindThirdPoint(points, p1, p2, depth);

                PointF[] triangle = new PointF[] { p1, p2, p3 };

                DrawRecursionTriangles(triangle, new PointF(p1.X + (p2.X - p1.X) / 3, p1.Y + (p2.Y - p1.Y) / 3), new PointF(p1.X + 2 * (p2.X - p1.X) / 3, p1.Y + 2 * (p2.Y - p1.Y) / 3), depth+1);
                DrawRecursionTriangles(triangle, new PointF(p2.X + (p3.X - p2.X) / 3, p2.Y + (p3.Y - p2.Y) / 3), new PointF(p2.X + 2 * (p3.X - p2.X) / 3, p2.Y + 2 * (p3.Y - p2.Y) / 3), depth+1);
                DrawRecursionTriangles(triangle, new PointF(p1.X + (p3.X - p1.X) / 3, p1.Y + (p3.Y - p1.Y) / 3), new PointF(p1.X + 2 * (p3.X - p1.X) / 3, p1.Y + 2 * (p3.Y - p1.Y) / 3), depth+1);
            }

        }

        PointF FindThirdPoint(PointF[] points, PointF p1, PointF p2, int depth) {
            float s = MathF.Sin((float)(60 * Math.PI / 180.0));
            float c = MathF.Cos((float)(60 * Math.PI / 180.0));
            var point = new PointF(c * (p1.X - p2.X) - s * (p1.Y - p2.Y) + p2.X, s * (p1.X - p2.X) + c * (p1.Y - p2.Y) + p2.Y);

            if (depth == 1)
                return point;

            if (!PointInTriangle(points, point))
                return point;
            else {
                s = MathF.Sin((float)(-60 * Math.PI / 180.0));
                c = MathF.Cos((float)(-60 * Math.PI / 180.0));
                return new PointF(c * (p1.X - p2.X) - s * (p1.Y - p2.Y) + p2.X, s * (p1.X - p2.X) + c * (p1.Y - p2.Y) + p2.Y);
            }

        }
            
        bool PointInTriangle(PointF[] points, PointF p) {
            var s = (points[0].X - points[2].X) * (p.Y - points[2].Y) - (points[0].Y - points[2].Y) * (p.X - points[2].X);
            var t = (points[1].X - points[0].X) * (p.Y - points[0].Y) - (points[1].Y - points[0].Y) * (p.X - points[0].X);

            if ((s < 0) != (t < 0) && s != 0 && t != 0)
                return false;

            var d = (points[2].X - points[1].X) * (p.Y - points[1].Y) - (points[2].Y - points[1].Y) * (p.X - points[1].X);
            return d == 0 || (d < 0) == (s + t <= 0);
        }

        #endregion
    }
}