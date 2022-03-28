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
        //Global variables
        int[,] board;
        PictureBox[,] formBoard;
        string turn;

        public Connect4Form()
        {
            InitializeComponent();
        }

        private void Connect4Form_Load(object sender, EventArgs e)
        {
            //Initialize global variables on load
            board = new int[,] { {0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0},
                                    {0, 0, 0, 0, 0, 0, 0}  };


            formBoard = new PictureBox[,] { { p00, p01, p02, p03, p04, p05, p06 },
                                            { p10, p11, p12, p13, p14, p15, p16 },
                                            { p20, p21, p22, p23, p24, p25, p26 },
                                            { p30, p31, p32, p33, p34, p35, p36 },
                                            { p40, p41, p42, p43, p44, p45, p46 },
                                            { p50, p51, p52, p53, p54, p55, p56 } };

            turn = "red";
        }

        private void leaveTile(object sender, EventArgs e)
        {
            PictureBox tile = sender as PictureBox;

            if (tile == null)
            {
                return;
            }

            //Check if background image is the highlight
            if ((string)tile.Tag == "highlight") {
                tile.BackgroundImage = null;
                tile.Tag = "";
            }
            
            return;
        }

        private void enterTile(object sender, EventArgs e)
        {
            PictureBox tile = sender as PictureBox;

            if (tile == null)
            {
                return;
            }

            if (tile.BackgroundImage == null)
            {
                if (isValid(tile.Name))
                {
                    tile.BackgroundImage = Properties.Resources.highlight;
                    tile.Tag = "highlight";
                }
            }

            return;

        }

        private void clickTile(object sender, EventArgs e)
        {
            PictureBox tile = sender as PictureBox;

            if (tile == null)
            {
                return;
            }

            if (turn == "red")
            {
                if ((string)tile.Tag == "highlight") 
                {
                    placePiece(tile.Name);
                    tile.BackgroundImage = Properties.Resources.redchip;
                    tile.Tag = "red";
                    turn = "yellow";
                    return;
                }
                
            }
            else if (turn == "yellow")
            {
                if ((string)tile.Tag == "highlight")
                {
                    placePiece(tile.Name);
                    tile.BackgroundImage = Properties.Resources.yellowchip;
                    tile.Tag = "yellow";
                    turn = "red";
                    return;
                }
            }
        }

        //Takes in the name of the tile and returns whether it would be valid to play there
        private bool isValid(string tileName)
        {
            int row = (int)(tileName[1] - '0');
            int col = (int)(tileName[2] - '0');

            //Make sure the space isn't already occupied
            if (board[row, col] != 0) 
            {
                return false;
            }

            //Moves on the bottom are valid
            if (row == 5) 
            {
                return true;
            }

            //If not on bottom then check if there's a piece below
            if (board[row + 1, col] != 0) {
                return true;
            }

            return false;
        }

        //Takes in the name of the tile and adds the value to array 
        private void placePiece(string tileName) {
            int row = (int)(tileName[1] - '0');
            int col = (int)(tileName[2] - '0');

            board[row, col] = 1;
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
