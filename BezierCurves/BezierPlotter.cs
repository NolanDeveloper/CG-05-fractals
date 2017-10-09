using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BezierCurves
{
    public class CPoint
    {
        private PointF point = new PointF(0, 0);
        public float X { get { return point.X; } set { point.X = value; } }
        public float Y { get { return point.Y; } set { point.Y = value; } }
        public CPoint() { }
        public CPoint(PointF p) { point = p; }
        public static implicit operator CPoint(PointF p) { return new CPoint(p); }
        public static implicit operator PointF(CPoint p) { return p.point; }
    }

    public class BezierPlotter : UserControl
    {
        private float POINT_SIZE = 4;
        private float SELECTION_RADIUS = 12;

        private List<CPoint> points = new List<CPoint>();
        private CPoint selectedPoint;

        public BezierPlotter() : base()
        {
            var flags = ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint;
            SetStyle(flags, true);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (0 != (ModifierKeys & Keys.Control)) return;
            points.Add((PointF)e.Location);
            Invalidate();
        }

        private void MovePoint(PointF cursor)
        {
            if (null == selectedPoint) return;
            PointF previousPosition = selectedPoint;
            selectedPoint.X = cursor.X;
            selectedPoint.Y = cursor.Y;
            Invalidate();
        }

        private float ManhattanDistance(PointF a, PointF b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            return Math.Abs(dx) + Math.Abs(dy);
        }

        private float EuclideanDistance(PointF a, PointF b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private void SelectPoint(PointF cursor)
        {
            if (0 == points.Count) return;
            CPoint nearestPoint = points[0];
            var nearestManhattan = ManhattanDistance(cursor, nearestPoint);
            var nearestDistance = EuclideanDistance(cursor, nearestPoint);
            for (int i = 1; i < points.Count; ++i)
            {
                var p = points[i];
                var manhattan = ManhattanDistance(cursor, p);
                if (nearestManhattan < manhattan) continue; // Fast check -- true for most of the points
                var distance = EuclideanDistance(cursor, p);
                if (nearestDistance < distance) continue; // Slow check -- true only for really good candidates
                nearestPoint = p;
                nearestManhattan = manhattan;
                nearestDistance = distance;
            }
            var oldSelection = selectedPoint;
            selectedPoint = nearestDistance < SELECTION_RADIUS ? nearestPoint : null;
            if (oldSelection != selectedPoint)
                Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (0 != (ModifierKeys & Keys.Control) && 0 != (e.Button & MouseButtons.Left))
                MovePoint(e.Location);
            else
                SelectPoint(e.Location);
        }

        private PointF[] diamond = new PointF[4];

        private PointF Bezier(int i, int j, float t)
        {
            if (i + 1 == j)
                return points[i];
            var a = Bezier(i, j - 1, t);
            var b = Bezier(i + 1, j, t);
            return new PointF(
                (1 - t) * a.X + t * b.X,
                (1 - t) * a.Y + t * b.Y
            );
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (0 == points.Count) return;
            var g = e.Graphics;
            var s = POINT_SIZE;
            g.Clear(Color.White);
            for (int i = 0; i < points.Count - 1; ++i)
                g.DrawLine(Pens.Black, points[i], points[i + 1]);
            var prev = points[0];
            for (int i = 1; i < 100; ++i)
            {
                var next = Bezier(0, points.Count, i / 100.0f);
                g.DrawLine(Pens.Green, prev, next);
                prev = next;
            }
            foreach (var p in points)
            {
                diamond[0] = new PointF(p.X, p.Y - s);
                diamond[1] = new PointF(p.X + s, p.Y);
                diamond[2] = new PointF(p.X, p.Y + s);
                diamond[3] = new PointF(p.X - s, p.Y);
                if (p == selectedPoint)
                    g.FillPolygon(Brushes.Black, diamond);
                else
                    g.DrawPolygon(Pens.Black, diamond);
            }
        }
    }
}
