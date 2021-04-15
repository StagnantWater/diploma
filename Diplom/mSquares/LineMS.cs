using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace mSquares
{
    class LineMS
    {
        PointF a;
        PointF b;
        PointF av;
        PointF bv;
        public LineMS(int x, int y, string index, int res)
        {
            switch (index[0])
            {
                case '0':
                    a = new PointF(x + 1, (float)(y + 0.5));
                    break;
                case '1':
                    a = new PointF((float)(x + 1.5), y + 1);
                    break;
                case '2':
                    a = new PointF(x + 1, (float)(y + 1.5));
                    break;
                case '3':
                    a = new PointF((float)(x+ 0.5), y + 1);
                    break;
            }

            switch (index[1])
            {
                case '0':
                    b = new PointF(x + 1, (float)(y + 0.5));
                    break;
                case '1':
                    b = new PointF((float)(x + 1.5), y + 1);
                    break;
                case '2':
                    b = new PointF(x + 1, (float)(y + 1.5));
                    break;
                case '3':
                    b = new PointF((float)(x + 0.5), y + 1);
                    break;
            }
            av = new PointF(a.X * res + 15, a.Y * res + 15);
            bv = new PointF(b.X * res + 15, b.Y * res + 15);
        }

        public PointF Av { get => av; }
        public PointF Bv { get => bv; }
        public PointF A { get => a; }
        public PointF B { get => b; }
        
    }
}

