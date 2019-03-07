using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RubiksCubeSolver
{
    public partial class frmMain : Form
    {
        Cube C;

        private int MD_Face = -1, MD_Row, MD_Col;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            C.Draw();
            label2.Visible = C.IsSolved();
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            char C = e.KeyChar;
            string S;
            
            if (C == 's' || C == 'S')
            {
                SimpleInput InputForm = new SimpleInput();
                InputForm.ShowDialog();
                S = InputForm.InputResult;
                InputForm.Dispose();
                ProcessSeq(S);
            }

            if (char.IsUpper(C))
            {
                // it's upper case = Convert to "Prime"
                S = C.ToString() + "P";
            } else
            {
                S = C.ToString().ToUpper();
            }

            Rotator(S);
        }

#region Loader
        private void frmMain_Load(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Visible = false;
            LoadSnippets();

            C = new Cube();

            C.FaceRenderer(1).MyFlatDrawBox = pictureBox1;
            C.FaceRenderer(2).MyFlatDrawBox = pictureBox2;
            C.FaceRenderer(3).MyFlatDrawBox = pictureBox3;
            C.FaceRenderer(4).MyFlatDrawBox = pictureBox4;
            C.FaceRenderer(5).MyFlatDrawBox = pictureBox5;
            C.FaceRenderer(0).MyFlatDrawBox = pictureBox6;

            float X = pictureBox7.Width ;
            float Y = pictureBox7.Height ;

            // 3 = Top
            // 0 = front
            // 4 = right
            Matrix MatrixTop = new Matrix();
            MatrixTop.Translate(X * 0.325f, 0);
            MatrixTop.Scale(0.6f, 0.3f);
            MatrixTop.Shear(-0.5f, 0);
            C.FaceRenderer(3).MySkewDrawBox = pictureBox7;
            C.FaceRenderer(3).MySkewRegionMatrix = MatrixTop;

            Matrix MatrixFront = new Matrix();
            MatrixFront.Translate(0, Y * 0.305f);
            MatrixFront.Scale(0.6f, 0.6f);
            //            MatrixFront.Shear(-1f / 3f, 0);
            C.FaceRenderer(0).MySkewDrawBox = pictureBox7;
            C.FaceRenderer(0).MySkewRegionMatrix = MatrixFront;

            Matrix MatrixRight = new Matrix();
            MatrixRight.Translate(X * 0.617f, Y * 0.263f);
            MatrixRight.Scale(0.295f, 0.65f);
            MatrixRight.Shear(0, -0.455f);
            MatrixRight.RotateAt(90, new PointF(X / 2, Y / 2));
            C.FaceRenderer(4).MySkewDrawBox = pictureBox7;
            C.FaceRenderer(4).MySkewRegionMatrix = MatrixRight;

            //C.Draw();
            this.Invalidate();            
        }

        private void LoadSnippets()
        {
            LoadSingleSnippet("U U D D L L R R F F B B");
            LoadSingleSnippet("RP DP R D RP DP R D D F D D FP RP D D R");
            LoadSingleSnippet("L D LP DP L D LP D D FP D D F L D D LP");
            LoadSingleSnippet("B L U LP UP BP");
            LoadSingleSnippet("B U L UP LP BP");
            LoadSingleSnippet("R R U U DP L RP F F LP R D R R");
            LoadSingleSnippet("R R DP L RP F F LP R U U D R R");
            LoadSingleSnippet("LP U R UP L U RP UP");
            LoadSingleSnippet("L D LP DP L D LP DP");
        }

        private void LoadSingleSnippet(string Caption)
        {
            Button B = new Button();
            B.Text = Caption;
            B.TextAlign = ContentAlignment.MiddleLeft;
            B.Width = panel1.Width - 6;
            B.Location = new Point(3, (panel1.Controls.Count * (B.Height + 2)));
            //Hook our button up to our generic button handler
            B.Click += new System.EventHandler(this.SnippetButton_Click);
            panel1.Controls.Add(B);
        }

        #endregion

#region Cube manipulation
        private void Rotator(string Turn)
        {
            if (Enum.TryParse<Cube.Turns>(Turn, out Cube.Turns T))
            {
                C.Turn(T);
                label1.Text += " " + Turn;
                label2.Visible = C.IsSolved();
            }
        }

        private void ProcessSeq(string S)
        {
            string[] RotSeq = S.Split(' ');
            foreach (string Rot in RotSeq)
            {
                Rotator(Rot);
            }

        }

        private void net_MouseDown(object sender, MouseEventArgs e, int face)
        {
            PictureBox Box = (PictureBox)sender;

            MD_Face = face;
            MD_Col = (e.X * 3) / Box.Width;
            MD_Row = (e.Y * 3) / Box.Height;

            label1.Text += " MD:" + MD_Face.ToString() + " [" + MD_Col.ToString() + "," + MD_Row.ToString() + "]";
        }

        private void skew_MouseDown(object sender, MouseEventArgs e, int face)
        {
            PictureBox Box = (PictureBox)sender;
            Graphics G = Box.CreateGraphics();

            for(int i=0; i<6; i++)
            {
                Face.FaceRender FR = C.FaceRenderer(i);
                if (!(FR.MySkewRegionMatrix is null))
                {
                    // crete a region that represents the face, and see if the MD event is in that region 
                    Region R = new Region(Box.ClientRectangle);
                    R.Transform(FR.MySkewRegionMatrix);

                    if (R.IsVisible(e.Location))
                    {
                        MD_Face = face + i;
                        // TO DO
                        MD_Col = (int)((e.X * 3.0f) / R.GetBounds(G).Width);
                        MD_Row = (int)((e.Y * 3.0f) / R.GetBounds(G).Height);
                        break;
                    }
                }
            }
            label1.Text += " MD:" + MD_Face.ToString() + " [" + MD_Col.ToString() + "," + MD_Row.ToString() + "]";
        }


        // arrays describe the move to make depending on mouse drags 
        string[/*face*/,/*up/down*/,/*Col*/] FaceColMoves = {
                /*0*/ {{"LP", "RC", "R" },{"L", "LC", "RP" }},
                /*1*/ {{"LP", "RC", "R" },{"L", "LC", "RP" }},
                /*2*/ {{"DP", "UC", "U"},{"D", "DC", "UP"}},
                /*3*/ {{"LP", "RC", "R" },{"L", "LC", "RP" }},
                /*4*/ {{"UP", "DC", "D" },{"U", "UC", "DP" }},
                /*5*/ {{"RP", "LC", "L" },{"R", "RC", "LP" }}
            };

        string[/*face*/,/*Left/right*/,/*Col*/] FaceRowMoves = {
                /*0*/ {{"U", "UC", "DP" },{"UP", "DC", "D" }},
                /*1*/ {{"D", "DC", "UP" },{"DP", "UC", "U" }},
                /*2*/ {{"B", "FC", "FP"},{"BP", "BC", "F"}},
                /*3*/ {{"B", "FC", "FP"},{"BP", "BC", "F"}},
                /*4*/ {{"B", "FC", "FP"},{"BP", "BC", "F"}},
                /*5*/ {{"B", "FC", "FP"},{"BP", "BC", "F"}}
            };

        private void net_MouseUp(object sender, MouseEventArgs e, int face)
        {
            if (MD_Face == face)
            {
                PictureBox Box = (PictureBox)sender;
                int Col = (e.X * 3) / Box.Width;
                int Row = (e.Y * 3) / Box.Height;

                //now the logic to work out the direction of drag...
                if (Col == MD_Col)
                {
                    if (Row == MD_Row)
                    {
                        // Row and col same... insufficient movement 
                        label1.Text += "MU: Null";
                    }
                    else
                    {
                        // Col changed
                        int UD = 0;
                        if (Row > MD_Row)
                            UD = 1;
                        String Move = FaceColMoves[face, UD, Col];
                        label1.Text += " MU:C" + Col.ToString() + (UD==1?"Down":"Up");
                        Rotator(Move);
                    }
                } else
                {
                    if (Row == MD_Row)
                    {
                        int LR = 0;
                        if (Col> MD_Col)
                            LR = 1;
                        String Move = FaceRowMoves[face, LR, Row];
                        label1.Text += " MU:R" + Row.ToString() + (LR == 1 ? "Right" : "Left");
                        Rotator(Move);
                    }
                    else
                    {
                        // Rpw AND Col changed
                        label1.Text += "MU: Null";
                    }

                }

                //label1.Text += " MU:" + " [" + Col.ToString() + "," + Row.ToString() + "]";
                MD_Face = -1;
            }
        }

        private void skew_MouseUp(object sender, MouseEventArgs e, int face)
        {
            MD_Face = -1;
            /*
            if (MD_Face == face)
            {
                PictureBox Box = (PictureBox)sender;
                int Col = (e.X * 3) / Box.Width;
                int Row = (e.Y * 3) / Box.Height;

                //now the logic to work out the direction of drag...
                if (Col == MD_Col)
                {
                    if (Row == MD_Row)
                    {
                        // Row and col same... insufficient movement 
                        label1.Text += "MU: Null";
                    }
                    else
                    {
                        // Col changed
                        int UD = 0;
                        if (Row > MD_Row)
                            UD = 1;
                        String Move = FaceColMoves[face, UD, Col];
                        label1.Text += " MU:C" + Col.ToString() + (UD == 1 ? "Down" : "Up");
                        Rotator(Move);
                    }
                }
                else
                {
                    if (Row == MD_Row)
                    {
                        int LR = 0;
                        if (Col > MD_Col)
                            LR = 1;
                        String Move = FaceRowMoves[face, LR, Row];
                        label1.Text += " MU:R" + Row.ToString() + (LR == 1 ? "Right" : "Left");
                        Rotator(Move);
                    }
                    else
                    {
                        // Rpw AND Col changed
                        label1.Text += "MU: Null";
                    }

                }

                //label1.Text += " MU:" + " [" + Col.ToString() + "," + Row.ToString() + "]";
                MD_Face = -1;
            } */
        }

        #endregion

        #region SpinButtons
        private void button1_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Rotator(((Button)sender).Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            C.TurnWholeCube(Cube.Turns.F);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            C.TurnWholeCube(Cube.Turns.FP);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            C.TurnWholeCube(Cube.Turns.LP);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            C.TurnWholeCube(Cube.Turns.L);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            C.TurnWholeCube(Cube.Turns.DP);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            C.TurnWholeCube(Cube.Turns.D);
        }
#endregion

#region UtilityButtons
        private void SnippetButton_Click(object sender, EventArgs e)
        {
            ProcessSeq(((Button)sender).Text);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            // randomise button
            C.Randomise(10);
            label2.Visible = C.IsSolved();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            // reset button
            C.Reset();
            label1.Text = "";
            label2.Visible = C.IsSolved();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            // sequence button
            SimpleInput InputForm = new SimpleInput();
            InputForm.ShowDialog();
            string S = InputForm.InputResult;
            InputForm.Dispose();
            ProcessSeq(S);
        }

        #endregion

#region Mouse Events
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            net_MouseDown(sender, e, 2);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            net_MouseDown(sender, e, 1);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            net_MouseUp(sender, e, 1);
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            net_MouseDown(sender, e, 3);
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            net_MouseUp(sender, e, 3);
        }

        private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
        {
            net_MouseDown(sender, e, 4);
        }

        private void pictureBox4_MouseUp(object sender, MouseEventArgs e)
        {
            net_MouseUp(sender, e, 4);
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            net_MouseDown(sender, e, 5);
        }

        private void pictureBox5_MouseUp(object sender, MouseEventArgs e)
        {
            net_MouseUp(sender, e, 5);
        }

        private void pictureBox6_MouseUp(object sender, MouseEventArgs e)
        {
            net_MouseUp(sender, e, 0);
        }

        private void pictureBox7_MouseDown(object sender, MouseEventArgs e)
        {
            skew_MouseDown(sender, e, 100);
        }

        private void pictureBox7_MouseUp(object sender, MouseEventArgs e)
        {
            skew_MouseUp(sender, e, 100);
        }

        private void pictureBox6_MouseDown(object sender, MouseEventArgs e)
        {
            net_MouseDown(sender, e, 0);
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            net_MouseUp(sender, e, 2);
        }


#endregion
    }
}
