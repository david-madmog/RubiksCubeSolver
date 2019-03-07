using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    partial class Face
    {
        private int[,] Cells;
        public FaceRender Renderer = new FaceRender();

        public Face(int Colour)
        {
            Cells = new int[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    Cells[i, j] = Colour;
                }
        }

        public Face()
        {
            Cells = new int[3, 3];
        }

        public void Draw()
        {
            if (!(Renderer is null))
            {
                Renderer.Draw(this);
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

        public void CopyFrom(Face F)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    Cells[i, j] = F.Cells[i,j];
                }

        }

        public bool IsSolved()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j] != Cells[0,0])
                        return false;
                }
            return true;
        }

    }

}
