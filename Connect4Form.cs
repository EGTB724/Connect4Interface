using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect4Interface
{
    public partial class Connect4Form : Form
    {
        public Connect4Form()
        {
            InitializeComponent();
        }

        private void Connect4Form_Load(object sender,EventArgs e) {

        }

        private void leaveTile(object sender, EventArgs e)
        {
            textBox1.Text = "Not covered";
        }

        private void enterTile(object sender, EventArgs e)
        {
            PictureBox tile = sender as PictureBox;

            if (tile == null)
            {
                return;
            }

            textBox1.Text = tile.Name;
        }

        private void clickPictureBox(object sender,EventArgs e) {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackgroundImage = Connect4Interface.Properties.Resources.yellowchip;
        }

        //This is Nathan
        //Test commit from Ethan
        //this is hopefully our final change
    }
}
