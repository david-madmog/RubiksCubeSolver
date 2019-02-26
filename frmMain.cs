using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RubiksCubeSolver
{
    public partial class frmMain : Form
    {
        Cube C;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            C = new Cube();

            C.SFPB(1, pictureBox1);
            C.SFPB(2, pictureBox2);
            C.SFPB(3, pictureBox3);
            C.SFPB(4, pictureBox4);
            C.SFPB(5, pictureBox5);
            C.SFPB(0, pictureBox6);

            this.Invalidate();
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            C.Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.B);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.FP);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.BP);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.F) ;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.LP);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.L);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.R);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.RP);

        }

        private void button17_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.U);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.UP);

        }

        private void button18_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.DP);

        }

        private void button15_Click(object sender, EventArgs e)
        {
            C.Turn(Cube.Turns.D);

        }

        private void button19_Click(object sender, EventArgs e)
        {
            C.Randomise();
        }
    }
}
