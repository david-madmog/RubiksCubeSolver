using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RubiksCubeSolver
{
    class Face
    {
        private int[,] Cells;
        public System.Windows.Forms.PictureBox MyDrawBox;
        private Color[] CellColours =
        {
            Color.Blue, Color.White, Color.Orange, Color.Green, Color.Red, Color.Yellow
        };

        public Face(int Colour)
        {
            Cells = new int[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    Cells[i, j] = Colour;
                }
        }

        public void Draw()
        {
            if (!(MyDrawBox is null))
            {
                Graphics G = MyDrawBox.CreateGraphics();
                int X = MyDrawBox.Width / 3;
                int Y = MyDrawBox.Height / 3;
                Pen P = Pens.Black;

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        Brush B = new SolidBrush(CellColours[Cells[j, i]]);
                        G.FillRectangle(B, X * i, Y * j, X, Y);
                        G.DrawRectangle(P, X * i, Y * j, X, Y);
                    }
            }
        }

        public int[] SetRow(int Row, int[] val)
        {
            int[] CurrentRow = new int[3];

            for (int i = 0; i < 3; i++)
            {
                CurrentRow[i] = Cells[Row, i];
                Cells[Row, i] = val[i];
            }

            return CurrentRow;
        }

        public int[] SetCol(int Col, int[] val)
        {
            int[] CurrentCol = new int[3];

            for (int i = 0; i < 3; i++)
            {
                CurrentCol[i] = Cells[i, Col];
                Cells[i, Col] = val[i];
            }

            return CurrentCol;
        }

        public void Rotate(bool bAC)
        {
            int i;
            if (bAC)
            {
                i = Cells[0, 0];
                Cells[0, 0] = Cells[0, 2];
                Cells[0, 2] = Cells[2, 2];
                Cells[2, 2] = Cells[2, 0];
                Cells[2, 0] = i;

                i = Cells[1, 0];
                Cells[1, 0] = Cells[0, 1];
                Cells[0, 1] = Cells[1, 2];
                Cells[1, 2] = Cells[2, 1];
                Cells[2, 1] = i;
            }
            else
            {
                i = Cells[0, 0];
                Cells[0, 0] = Cells[2, 0];
                Cells[2, 0] = Cells[2, 2];
                Cells[2, 2] = Cells[0, 2];
                Cells[0, 2] = i;

                i = Cells[1, 0];
                Cells[1, 0] = Cells[2, 1];
                Cells[2, 1] = Cells[1, 2];
                Cells[1, 2] = Cells[0, 1];
                Cells[0, 1] = i;
            }
        }
    }

}
