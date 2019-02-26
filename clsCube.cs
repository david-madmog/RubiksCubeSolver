using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    class Cube
    {
        // Set of allowable operations on cube
        public enum Turns { R, RP, L, LP, U, UP, D, DP, F, FP, B, BP }

        private Face[] Faces;

        public Cube()
        {
            Faces = new Face[6];
            for(int i=0; i<6; i++)
            {
                Faces[i] = new Face(i);
            }
        }

        public void SFPB(int i, System.Windows.Forms.PictureBox PB)
        {
            Faces[i].MyDrawBox = PB;
        }

        public void Turn(Turns Move)
        {
            int RowCol;
            int[] Tmp = new int[3];

            switch (Move)
            {
                case Turns.B:
                    RowCol = 0;
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Tmp = Faces[5].SetRow(RowCol, Tmp);
                    Tmp = Faces[4].SetRow(RowCol, Tmp);
                    Tmp = Faces[3].SetRow(RowCol, Tmp);
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Faces[1].Rotate(false);
                    break;
                case Turns.BP:
                    RowCol = 0;
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Tmp = Faces[3].SetRow(RowCol, Tmp);
                    Tmp = Faces[4].SetRow(RowCol, Tmp);
                    Tmp = Faces[5].SetRow(RowCol, Tmp);
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Faces[1].Rotate(true);
                    break;
                case Turns.FP:
                    RowCol = 2;
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Tmp = Faces[3].SetRow(RowCol, Tmp);
                    Tmp = Faces[4].SetRow(RowCol, Tmp);
                    Tmp = Faces[5].SetRow(RowCol, Tmp);
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Faces[0].Rotate(true);
                    break;
                case Turns.F:
                    RowCol = 2;
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Tmp = Faces[3].SetRow(RowCol, Tmp);
                    Tmp = Faces[4].SetRow(RowCol, Tmp);
                    Tmp = Faces[5].SetRow(RowCol, Tmp);
                    Tmp = Faces[2].SetRow(RowCol, Tmp);
                    Faces[0].Rotate(false);
                    break;
                case Turns.L:
                    RowCol = 0;
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[1].SetCol(RowCol, Tmp);
                    Tmp = Faces[3].SetCol(RowCol, Tmp);
                    Tmp = Faces[0].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    Faces[2].Rotate(false);
                    break;
                case Turns.LP:
                    RowCol = 0;
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[0].SetCol(RowCol, Tmp);
                    Tmp = Faces[3].SetCol(RowCol, Tmp);
                    Tmp = Faces[1].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    Faces[2].Rotate(true);
                    break;
                case Turns.RP:
                    RowCol = 2;
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[1].SetCol(RowCol, Tmp);
                    Tmp = Faces[3].SetCol(RowCol, Tmp);
                    Tmp = Faces[0].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    Faces[4].Rotate(true);
                    break;
                case Turns.R:
                    RowCol = 2;
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[0].SetCol(RowCol, Tmp);
                    Tmp = Faces[3].SetCol(RowCol, Tmp);
                    Tmp = Faces[1].SetCol(RowCol, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(RowCol, Tmp);
                    Faces[4].Rotate(false);
                    break;
                case Turns.U:
                    Tmp = Faces[2].SetCol(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[1].SetRow(2, Tmp);
                    Tmp = Faces[4].SetCol(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[0].SetRow(0, Tmp);
                    Tmp = Faces[2].SetCol(2, Tmp);
                    Faces[3].Rotate(false);
                    break;
                case Turns.UP:
                    RowCol = 0;
                    Tmp = Faces[2].SetCol(2, Tmp);
                    Tmp = Faces[0].SetRow(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[4].SetCol(0, Tmp);
                    Tmp = Faces[1].SetRow(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[2].SetCol(2, Tmp);
                    Faces[3].Rotate(true);
                    break;
                case Turns.DP:
                    Tmp = Faces[2].SetCol(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[1].SetRow(0, Tmp);
                    Tmp = Faces[4].SetCol(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[0].SetRow(2, Tmp);
                    Tmp = Faces[2].SetCol(0, Tmp);
                    Faces[5].Rotate(true);
                    break;
                case Turns.D:
                    RowCol = 0;
                    Tmp = Faces[2].SetCol(0, Tmp);
                    Tmp = Faces[0].SetRow(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[4].SetCol(2, Tmp);
                    Tmp = Faces[1].SetRow(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[2].SetCol(0, Tmp);
                    Faces[5].Rotate(false);
                    break;
                default:
                    break;
            }

            this.Draw();
            
        }

        public void TurnWholeCube(Turns Move) { }

        public void Randomise()
        {
            Random r = new Random();
            Turns T;
            int Max = Enum.GetNames(typeof(Turns)).Length;

            for (int i=0; i<10; i++)
            {
                T = (Turns)r.Next(Max);
                Turn(T);
            }
        }

        public void Draw()
        {
            foreach(Face F in Faces)
            {
                F.Draw();
            }
        }

        private void rev(int[] val)
        {
            int i = val[0];
            val[0] = val[2];
            val[2] = i;
        }

    }
}
