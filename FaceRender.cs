using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RubiksCubeSolver
{
    partial class Face
    {
        public class FaceRender
        {
            public System.Windows.Forms.PictureBox MyFlatDrawBox;

            public Matrix MySkewRegionMatrix;
            public System.Windows.Forms.PictureBox MySkewDrawBox;

            private Color[] CellColours =
            {
                Color.Blue, Color.White, Color.Orange, Color.Green, Color.Red, Color.Yellow
            };

            public void Draw(Face F)
            {
                if (!(MyFlatDrawBox is null))
                {
                    Graphics G = MyFlatDrawBox.CreateGraphics();
                    int X = MyFlatDrawBox.Width / 3;
                    int Y = MyFlatDrawBox.Height / 3;
                    Pen P = Pens.Black;

                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            Brush B = new SolidBrush(CellColours[F.Cells[j, i]]);
                            G.FillRectangle(B, X * i, Y * j, X, Y);
                            G.DrawRectangle(P, X * i, Y * j, X, Y);
                        }
                }

                if (!(MySkewRegionMatrix is null) && !(MySkewDrawBox is null))
                {
                    Graphics G = MySkewDrawBox.CreateGraphics();
                    Pen P = Pens.Black;

                    GraphicsPath Path = new GraphicsPath();
                    Path.AddLine(0, 0, MySkewDrawBox.Width, 0);
                    Path.AddLine(MySkewDrawBox.Width, 0, MySkewDrawBox.Width, MySkewDrawBox.Height);
                    Path.AddLine(MySkewDrawBox.Width, MySkewDrawBox.Height, 0, MySkewDrawBox.Height);
                    Path.AddLine(0, MySkewDrawBox.Height, 0, 0);

                    // Debugging - ping overall region
                    Brush BB = new SolidBrush(Color.Pink);
                    GraphicsPath FacePath = (GraphicsPath)Path.Clone();
                    FacePath.Transform(MySkewRegionMatrix);
                    G.FillRegion(BB, new Region(FacePath));
                    G.DrawPath(P, FacePath);
                    //                float SSPWidth = =MySkewRegionPath.

                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            Brush B = new SolidBrush(CellColours[F.Cells[j, i]]);
                            Matrix transformMatrix = new Matrix();
                            // The skew region matrix represents a transform from the whole area of the picture box
                            //      to a skewed, translated and scaled area for this face
                            transformMatrix.Multiply(MySkewRegionMatrix);

                            // We then further transform this to the third of the face for the individual cell
                            transformMatrix.Scale(1f / 3f, 1f / 3f);
                            transformMatrix.Translate((float)i * Path.GetBounds().Width,
                                (float)j * Path.GetBounds().Height);

                            GraphicsPath CellPath = (GraphicsPath)Path.Clone();
                            CellPath.Transform(transformMatrix);
                            G.FillRegion(B, new Region(CellPath));
                            G.DrawPath(P, CellPath);
                        }
                }
            }



        }

     }
}
