using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Connect4Interface
{
    public partial class Connect4Form : Form
    {
        //Global variables
        //Used to represent the board numerically
        int[,] board;

        //Used to represent the board visually (with pictures)
        PictureBox[,] formBoard;

        //Indicates whose turn is is
        string turn;

        //Number of rows and columns in a traditional connect 4 board
        const int NUM_ROWS = 6;
        const int NUM_COLS = 7;

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

            //Start with the game board disabled
            disableAllTiles();
        }

        private void StartOverButton_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    //Any time you add an event handler, it's smart to subtract it first
                    //This will prevent multiple of the same event handlers piling up on the same object
                    formBoard[row, col].Click -= new EventHandler(clickTile);
                    formBoard[row, col].MouseEnter -= new EventHandler(enterTile);
                    formBoard[row, col].MouseLeave -= new EventHandler(leaveTile);
                    formBoard[row, col].Click += new EventHandler(clickTile);
                    formBoard[row, col].MouseEnter += new EventHandler(enterTile);
                    formBoard[row, col].MouseLeave += new EventHandler(leaveTile);
                    formBoard[row, col].BackgroundImage = null;
                    formBoard[row, col].Tag = null;
                    board[row, col] = 0;
                }
            }

            StartOverButton.Enabled = false;
            turnIndicator.BackgroundImage = Properties.Resources.redchip;
            turn = "red";
            redTurn();
        }

        private void ResetGameButton_Click(object sender, EventArgs e)
        {
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    formBoard[row, col].Click -= new EventHandler(clickTile);
                    formBoard[row, col].MouseEnter -= new EventHandler(enterTile);
                    formBoard[row, col].MouseLeave -= new EventHandler(leaveTile);
                    formBoard[row, col].BackgroundImage = null;
                    formBoard[row, col].Tag = null;
                    board[row, col] = 0;
                }
            }

            turnIndicator.BackgroundImage = null;
            StartOverButton.Enabled = true;
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
                if (isValid(tile.Name)) 
                {
                    //Place the piece on both boards
                    placePiece(tile.Name);
                    tile.BackgroundImage = Properties.Resources.redchip;
                    tile.Tag = "red";

                    //Check if the player has won
                    if (hasWon())
                    {
                        MessageBox.Show("Red player won!!");
                        disableAllTiles();
                        return;
                    }
                    else
                    {

                        //Change the turn
                        turnIndicator.BackgroundImage = Properties.Resources.yellowchip;
                        turn = "yellow";
                        flipBoard();
                        yellowTurn();
                        return;
                    }
                }
                
            }
            else if (turn == "yellow")
            {
                if (isValid(tile.Name))
                {
                    //Place the piece on both boards
                    placePiece(tile.Name);
                    tile.BackgroundImage = Properties.Resources.yellowchip;
                    tile.Tag = "yellow";

                    //Check if the player has won
                    if (hasWon())
                    {
                        MessageBox.Show("Yellow player won!!");
                        disableAllTiles();
                        return;
                    }
                    else
                    {

                        //Change the turn
                        turnIndicator.BackgroundImage = Properties.Resources.redchip;
                        turn = "red";
                        flipBoard();
                        redTurn();
                        return;
                    }
                }
            }
        }

        private void yellowTurn() 
        {
            if (YellowPlayerLabel.Text == "Human")
            {
                enableAllTiles();
            }
            else {
                //If we are here, a computer is playing
                disableAllTiles();

                //We need to output the current board state 
                outputBoard();

                //Call the executable
                Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = RedPlayerLabel.Text;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                //Take in the move played by the computer
                string text = System.IO.File.ReadAllText("move.txt");

                int row = (int)(text[0] - '0');
                int col = (int)(text[2] - '0');

                board[row, col] = 1;
                formBoard[row,col].BackgroundImage = Properties.Resources.yellowchip;
                formBoard[row,col].Tag = "yellow";

                //Check if the player has won
                if (hasWon())
                {
                    MessageBox.Show("Yellow player won!!");
                    disableAllTiles();
                    return;
                }

                //Change the turn
                turn = "red";
                flipBoard();
                redTurn();
                return;
            }
        }

        private void redTurn() 
        {
            if (RedPlayerLabel.Text == "Human")
            {
                enableAllTiles();
            }
            else
            {
                //If we are here, a computer is playing
                disableAllTiles();

                //We need to output the current board state 
                outputBoard();

                //Call the executable
                Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = RedPlayerLabel.Text;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                //Take in the move played by the computer
                string text = System.IO.File.ReadAllText("move.txt");

                int row = (int)(text[0] - '0');
                int col = (int)(text[2] - '0');

                board[row, col] = 1;
                formBoard[row, col].BackgroundImage = Properties.Resources.redchip;
                formBoard[row, col].Tag = "red";

                //Check if the player has won
                if (hasWon())
                {
                    MessageBox.Show("Red player won!!");
                    disableAllTiles();
                    return;
                }

                //Change the turn
                turn = "yellow";
                flipBoard();
                yellowTurn();
                return;
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
        private void placePiece(string tileName) 
        {
            int row = (int)(tileName[1] - '0');
            int col = (int)(tileName[2] - '0');

            board[row, col] = 1;
        }

        //Check the board to see if there is any line of 4 1's
        private bool hasWon()
        {
            // horizontalCheck 
            for (int j = 0; j < NUM_COLS - 3; j++)
            {
                for (int i = 0; i < NUM_ROWS; i++)
                {
                    if (board[i, j] == 1 && board[i,j + 1] == 1 && board[i,j + 2] == 1 && board[i,j + 3] == 1)
                    {
                        return true;
                    }
                }
            }
            // verticalCheck
            for (int i = 0; i < NUM_ROWS - 3; i++)
            {
                for (int j = 0; j < NUM_COLS; j++)
                {
                    if (board[i,j] == 1 && board[i + 1,j] == 1 && board[i + 2,j] == 1 && board[i + 3,j] == 1)
                    {
                        return true;
                    }
                }
            }
            // ascendingDiagonalCheck 
            for (int i = 3; i < NUM_ROWS; i++)
            {
                for (int j = 0; j < NUM_COLS - 3; j++)
                {
                    if (board[i,j] == 1 && board[i - 1,j + 1] == 1 && board[i - 2,j + 2] == 1 && board[i - 3,j + 3] == 1)
                        return true;
                }
            }
            // descendingDiagonalCheck
            for (int i = 3; i < NUM_ROWS; i++)
            {
                for (int j = 3; j < NUM_COLS; j++)
                {
                    if (board[i,j] == 1 && board[i - 1,j - 1] == 1 && board[i - 2,j - 2] == 1 && board[i - 3,j - 3] == 1)
                        return true;
                }
            }
            return false;
        }

        //Flips the array (swaps 1 and -1)
        private void flipBoard() 
        {
            for (int row = 0; row < NUM_ROWS; row++) {
                for (int col = 0; col < NUM_COLS; col++) {
                    if (board[row, col] == 1)
                    {
                        board[row, col] = -1;
                    }
                    else if (board[row, col] == -1) 
                    {
                        board[row, col] = 1;
                    }
                }
            }
        }

        private void disableAllTiles() 
        {
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    formBoard[row, col].Click -= new EventHandler(clickTile);
                    formBoard[row, col].MouseEnter -= new EventHandler(enterTile);
                    formBoard[row, col].MouseLeave -= new EventHandler(leaveTile);
                }
            }
        }

        private void enableAllTiles() 
        {
            for (int row = 0; row < NUM_ROWS; row++)
            {
                for (int col = 0; col < NUM_COLS; col++)
                {
                    formBoard[row, col].Click -= new EventHandler(clickTile);
                    formBoard[row, col].MouseEnter -= new EventHandler(enterTile);
                    formBoard[row, col].MouseLeave -= new EventHandler(leaveTile);
                    formBoard[row, col].Click += new EventHandler(clickTile);
                    formBoard[row, col].MouseEnter += new EventHandler(enterTile);
                    formBoard[row, col].MouseLeave += new EventHandler(leaveTile);
                }
            }
        }

        private void YellowComputerButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Executables|*.exe"
            };

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                //Returns the path + filename 
                string filename = openFileDialog1.FileName;
                //MessageBox.Show(filename);
                //Process.Start(filename);
                YellowPlayerLabel.Text = filename;
            }
        }

        private void YellowHumanButton_Click(object sender, EventArgs e)
        {
            YellowPlayerLabel.Text = "Human";
        }

        private void RedComputerButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog()
            {
                Filter = "Executables|*.exe"
            };

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Returns the path + filename 
                string filename = openFileDialog1.FileName;
                //MessageBox.Show(filename);
                //Process.Start(filename);
                RedPlayerLabel.Text = filename;
            }
        }

        private void RedHumanButton_Click(object sender, EventArgs e)
        {
            RedPlayerLabel.Text = "Human";
        }

        private void outputBoard() 
        {
            string outputFile = "board.txt";
            string boardString = "";

            string path = Directory.GetCurrentDirectory();

            for (int row = 0; row < NUM_ROWS; row++) 
            {
                for (int col = 0; col < NUM_COLS; col++) 
                {
                    //O is max player
                    //X is min player
                    //. is an open spot
                    if (board[row, col] == 1)
                    {
                        boardString += "O";
                    }
                    else if (board[row, col] == -1)
                    {
                        boardString += "X";
                    }
                    else if (board[row, col] == 0) 
                    {
                        boardString += ".";
                    }
                }
            }

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(outputFile))
                {
                    File.Delete(outputFile);
                }

                //Create the new file
                using (StreamWriter sw = File.CreateText(outputFile))
                {
                    sw.WriteLine(boardString);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }






        //This is Nathan
        //Test commit from Ethan
        //this is hopefully our final change
    }
}
