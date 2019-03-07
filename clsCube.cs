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
        public enum Turns { R, RP, RC, L, LP, LC, U, UP, UC, D, DP, DC, F, FP, FC, B, BP, BC }

        private Face[] Faces;

        public Cube()
        {
            Faces = new Face[6];
            for(int i=0; i<6; i++)
            {
                Faces[i] = new Face(i);
            }
        }

#region DevSupportFunctions
        public void Randomise(int degree)
        {
            Random r = new Random();
            Turns T;
            int Max = Enum.GetNames(typeof(Turns)).Length;

            for (int i = 0; i < degree; i++)
            {
                T = (Turns)r.Next(Max);
                Turn(T);
            }
        }

        public void Reset()
        {
            for (int i = 0; i < 6; i++)
            {
                int[] CurrentRow = { i, i, i };
                for (int j = 0; j < 3; j++)
                {
                    Faces[i].SetRow(j, CurrentRow);
                }
            }
        }

#endregion

        public Face.FaceRender FaceRenderer(int i)
        {
            return Faces[i].Renderer;
        }

        public void Turn(Turns Move)
        {
            int[] Tmp = new int[3];

            switch (Move)
            {
                case Turns.BC:
                case Turns.FC:
                case Turns.LC:
                case Turns.RC:
                case Turns.UC:
                case Turns.DC:
                    TurnWholeCube(Move);
                    break; 
                case Turns.B:
                    Tmp = Faces[2].SetRow(0, Tmp);
                    Tmp = Faces[5].SetRow(0, Tmp);
                    Tmp = Faces[4].SetRow(0, Tmp);
                    Tmp = Faces[3].SetRow(0, Tmp);
                    Tmp = Faces[2].SetRow(0, Tmp);
                    Faces[1].Rotate(false);
                    break;
                case Turns.BP:
                    Tmp = Faces[2].SetRow(0, Tmp);
                    Tmp = Faces[3].SetRow(0, Tmp);
                    Tmp = Faces[4].SetRow(0, Tmp);
                    Tmp = Faces[5].SetRow(0, Tmp);
                    Tmp = Faces[2].SetRow(0, Tmp);
                    Faces[1].Rotate(true);
                    break;
                case Turns.FP:
                    Tmp = Faces[2].SetRow(2, Tmp);
                    Tmp = Faces[5].SetRow(2, Tmp);
                    Tmp = Faces[4].SetRow(2, Tmp);
                    Tmp = Faces[3].SetRow(2, Tmp);
                    Tmp = Faces[2].SetRow(2, Tmp);
                    Faces[0].Rotate(true);
                    break;
                case Turns.F:
                    Tmp = Faces[2].SetRow(2, Tmp);
                    Tmp = Faces[3].SetRow(2, Tmp);
                    Tmp = Faces[4].SetRow(2, Tmp);
                    Tmp = Faces[5].SetRow(2, Tmp);
                    Tmp = Faces[2].SetRow(2, Tmp);
                    Faces[0].Rotate(false);
                    break;
                case Turns.L:
                    Tmp = Faces[5].SetCol(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[1].SetCol(0, Tmp);
                    Tmp = Faces[3].SetCol(0, Tmp);
                    Tmp = Faces[0].SetCol(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(2, Tmp);
                    Faces[2].Rotate(false);
                    break;
                case Turns.LP:
                    Tmp = Faces[5].SetCol(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[0].SetCol(0, Tmp);
                    Tmp = Faces[3].SetCol(0, Tmp);
                    Tmp = Faces[1].SetCol(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(2, Tmp);
                    Faces[2].Rotate(true);
                    break;
                case Turns.RP:
                    Tmp = Faces[5].SetCol(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[1].SetCol(2, Tmp);
                    Tmp = Faces[3].SetCol(2, Tmp);
                    Tmp = Faces[0].SetCol(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(0, Tmp);
                    Faces[4].Rotate(true);
                    break;
                case Turns.R:
                    Tmp = Faces[5].SetCol(0, Tmp);
                    rev(Tmp);
                    Tmp = Faces[0].SetCol(2, Tmp);
                    Tmp = Faces[3].SetCol(2, Tmp);
                    Tmp = Faces[1].SetCol(2, Tmp);
                    rev(Tmp);
                    Tmp = Faces[5].SetCol(0, Tmp);
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

        public bool IsSolved()
        {
            foreach(Face F in Faces)
            {
                if (!F.IsSolved())
                {
                    return false;
                }
            }
            return true;
        }

        public void TurnWholeCube(Turns Move)
        {
            Face Tmp = new Face();

            switch (Move)
            {
                case Turns.B:
                case Turns.FP:
                case Turns.BC:
                    Tmp.CopyFrom(Faces[2]);
                    Faces[2].CopyFrom(Faces[5]);
                    Faces[5].CopyFrom(Faces[4]);
                    Faces[4].CopyFrom(Faces[3]);
                    Faces[3].CopyFrom(Tmp);
                    Faces[1].Rotate(true);
                    Faces[0].Rotate(false);
                    break;
                case Turns.BP:
                case Turns.F:
                case Turns.FC:
                    Tmp.CopyFrom(Faces[2]);
                    Faces[2].CopyFrom(Faces[3]);
                    Faces[3].CopyFrom(Faces[4]);
                    Faces[4].CopyFrom(Faces[5]);
                    Faces[5].CopyFrom(Tmp);
                    Faces[1].Rotate(false);
                    Faces[0].Rotate(true);
                    break;
                case Turns.LP:
                case Turns.R:
                case Turns.RC:
                    Tmp.CopyFrom(Faces[5]);
                    Faces[5].CopyFrom(Faces[1]);
                    Faces[5].Rotate(false);
                    Faces[5].Rotate(false);
                    Faces[1].CopyFrom(Faces[3]);
                    Faces[3].CopyFrom(Faces[0]);
                    Faces[0].CopyFrom(Tmp);
                    Faces[0].Rotate(false);
                    Faces[0].Rotate(false);
                    Faces[2].Rotate(true);
                    Faces[4].Rotate(false);
                    break;
                case Turns.L:
                case Turns.LC:
                case Turns.RP:
                    Tmp.CopyFrom(Faces[5]);            
                    Faces[5].CopyFrom(Faces[0]);
                    Faces[5].Rotate(false);
                    Faces[5].Rotate(false);
                    Faces[0].CopyFrom(Faces[3]);
                    Faces[3].CopyFrom(Faces[1]);
                    Faces[1].CopyFrom(Tmp);
                    Faces[1].Rotate(false);
                    Faces[1].Rotate(false);
                    Faces[2].Rotate(false);
                    Faces[4].Rotate(true);
                    break;
                case Turns.U:
                case Turns.UC:
                case Turns.DP:
                    Tmp.CopyFrom(Faces[2]);
                    Faces[2].CopyFrom(Faces[0]);
                    Faces[2].Rotate(false);
                    Faces[0].CopyFrom(Faces[4]);
                    Faces[0].Rotate(false);
                    Faces[4].CopyFrom(Faces[1]);
                    Faces[4].Rotate(false);
                    Faces[1].CopyFrom(Tmp);
                    Faces[1].Rotate(false);
                    Faces[3].Rotate(false);
                    Faces[5].Rotate(true);
                    break;
                case Turns.D:
                case Turns.DC:
                case Turns.UP:
                    Tmp.CopyFrom(Faces[2]);
                    Faces[2].CopyFrom(Faces[1]);
                    Faces[2].Rotate(true);
                    Faces[1].CopyFrom(Faces[4]);
                    Faces[1].Rotate(true);
                    Faces[4].CopyFrom(Faces[0]);
                    Faces[4].Rotate(true);
                    Faces[0].CopyFrom(Tmp);
                    Faces[0].Rotate(true);
                    Faces[3].Rotate(true);
                    Faces[5].Rotate(false);
                    break;
                default:
                    break;
            }

            this.Draw();

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
