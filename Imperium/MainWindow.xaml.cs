using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Imperium
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[,] chessBoard = new string[9, 9];

        char colorMove;
        struct newMuve
        {
            public int X;
            public int Y;
            public string Fig;
            public bool check;
        }
        newMuve trueMove;

        
        private char checkWin()
        {
            if (chessBoard[4, 4] != "0")
            {
                if (chessBoard[4, 4][1] == 'K')
                {
                    MessageBox.Show('B' == chessBoard[4, 4][0] ?"1":"2", "Сообщение игроку", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return '0';
        }

        private Image getCellFigur(int x, int y)
        {
            Image[,] temp = new Image[9, 9]{{cell00, cell01, cell02, cell03, cell04, cell05, cell06, cell07, cell08},
                                            {cell10, cell11, cell12, cell13, cell14, cell15, cell16, cell17, cell18},
                                            {cell20, cell21, cell22, cell23, cell24, cell25, cell26, cell27, cell28},
                                            {cell30, cell31, cell32, cell33, cell34, cell35, cell36, cell37, cell38},
                                            {cell40, cell41, cell42, cell43, cell44, cell45, cell46, cell47, cell48},
                                            {cell50, cell51, cell52, cell53, cell54, cell55, cell56, cell57, cell58},
                                            {cell60, cell61, cell62, cell63, cell64, cell65, cell66, cell67, cell68},
                                            {cell70, cell71, cell72, cell73, cell74, cell75, cell76, cell77, cell78},
                                            {cell80, cell81, cell82, cell83, cell84, cell85, cell86, cell87, cell88}};
            return temp[x,y];
        }

        private Ellipse getCellEllipse(int x, int y)
        {
            Ellipse[,] temp = new Ellipse[9, 9]{{El00, El01, El02, El03, El04, El05, El06, El07, El08},
                                                {El10, El11, El12, El13, El14, El15, El16, El17, El18},
                                                {El20, El21, El22, El23, El24, El25, El26, El27, El28},
                                                {El30, El31, El32, El33, El34, El35, El36, El37, El38},
                                                {El40, El41, El42, El43, El44, El45, El46, El47, El48},
                                                {El50, El51, El52, El53, El54, El55, El56, El57, El58},
                                                {El60, El61, El62, El63, El64, El65, El66, El67, El68},
                                                {El70, El71, El72, El73, El74, El75, El76, El77, El78},
                                                {El80, El81, El82, El83, El84, El85, El86, El87, El88}};
            return temp[x, y];
        }

        private Ellipse getCellAtackEllipse(int x, int y)
        {
            Ellipse[,] temp = new Ellipse[9, 9]{{AEl00, AEl01, AEl02, AEl03, AEl04, AEl05, AEl06, AEl07, AEl08},
                                                {AEl10, AEl11, AEl12, AEl13, AEl14, AEl15, AEl16, AEl17, AEl18},
                                                {AEl20, AEl21, AEl22, AEl23, AEl24, AEl25, AEl26, AEl27, AEl28},
                                                {AEl30, AEl31, AEl32, AEl33, AEl34, AEl35, AEl36, AEl37, AEl38},
                                                {AEl40, AEl41, AEl42, AEl43, AEl44, AEl45, AEl46, AEl47, AEl48},
                                                {AEl50, AEl51, AEl52, AEl53, AEl54, AEl55, AEl56, AEl57, AEl58},
                                                {AEl60, AEl61, AEl62, AEl63, AEl64, AEl65, AEl66, AEl67, AEl68},
                                                {AEl70, AEl71, AEl72, AEl73, AEl74, AEl75, AEl76, AEl77, AEl78},
                                                {AEl80, AEl81, AEl82, AEl83, AEl84, AEl85, AEl86, AEl87, AEl88}};
            return temp[x, y];
        }

        private void choiceMarkCell(int x, int y, bool choice)
        {
            byte temp;
            if(choice == false) temp=0; else temp=128;
            if (chessBoard[x,y] != "0")
            {
                getCellAtackEllipse(x, y).Stroke = new SolidColorBrush(Color.FromArgb(temp, 0, 0, 0));
            }
            else
            {
                getCellEllipse(x, y).Fill = new SolidColorBrush(Color.FromArgb(temp, 0, 0, 0));
            }
        }


        private void resetBoard()
        {
            chessBoard = new string[9, 9]{{"RK", "RG", "RW", "0", "0", "0", "YW", "YG", "YK"},
                                          {"RG", "RW", "0", "0", "0", "0", "0", "YW", "YG"},
                                          {"RW", "0", "0", "0", "0", "0", "0", "0", "YW"},
                                          {"0", "0", "0", "0", "0", "0", "0", "0", "0"},
                                          {"0", "0", "0", "0", "0", "0", "0", "0", "0"},
                                          {"0", "0", "0", "0", "0", "0", "0", "0", "0"},
                                          {"BW", "0", "0", "0", "0", "0", "0", "0", "GW"},
                                          {"BG", "BW", "0", "0", "0", "0", "0", "GW", "GG"},
                                          {"BK", "BG", "BW", "0", "0", "0", "GW", "GG", "GK"}};
        }


        public MainWindow()
        {
            InitializeComponent();
            resetBoard();
            placeFigur();
            colorMove = 'B';
            trueMove.check = false;
        }

        private void checkMovingShow(int x,int y, string Fig)
        {
            if (Fig[1] == 'G')
            {
                if(x - 1 >= 0 && y - 1 >= 0 && chessBoard[x - 1, y-1][0] != colorMove)
                {
                    choiceMarkCell(x - 1, y - 1, true);
                }
                if (x - 1 >= 0 && y + 1 < 9 && chessBoard[x - 1, y+1][0] != colorMove)
                {
                    choiceMarkCell(x - 1, y + 1, true);
                }
                if (x + 1 < 9 && y - 1 >= 0 && chessBoard[x + 1, y-1][0] != colorMove)
                {
                    choiceMarkCell(x + 1, y - 1, true);
                }
                if (x + 1 < 9 && y + 1 < 9 && chessBoard[x + 1, y+1][0] != colorMove)
                {
                    choiceMarkCell(x + 1, y + 1, true);
                }
            }
            else
            {
                if(x-1 >= 0 && chessBoard[x-1,y][0] != colorMove)
                {
                    choiceMarkCell(x-1, y, true);
                }
                if(y-1 >= 0 && chessBoard[x, y - 1][0] != colorMove)
                {
                    choiceMarkCell(x, y-1, true);
                }
                if (x + 1 < 9 && chessBoard[x + 1, y][0] != colorMove)
                {
                    choiceMarkCell(x+1, y, true);
                }
                if(y + 1 < 9 && chessBoard[x, y + 1][0] != colorMove)
                {
                    choiceMarkCell(x,y+1, true);
                }
            }
        }

        private bool checkMoving(int x, int y, string Fig)
        {
            if (trueMove.Fig[1] == 'G')
            {
                if (((trueMove.X == x + 1 || trueMove.X == x - 1) && (trueMove.Y == y + 1 || trueMove.Y == y - 1)) && chessBoard[x, y][0] != colorMove)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if((((trueMove.X == x + 1 || trueMove.X == x - 1) && trueMove.Y == y) || (trueMove.Y == y + 1 || trueMove.Y == y - 1)&& trueMove.X == x) && chessBoard[x, y][0] != colorMove)
                {
                    return true;
                }else
                {
                    return false;
                }
            }
        }
        private void placeFigur()
        {
            Image cellImage = new Image();
            BitmapImage bitmap;
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++) 
                {
                    getCellEllipse(x, y).Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    getCellAtackEllipse(x, y).Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    if (chessBoard[x,y] != "0") 
                    {
                        if (chessBoard[x, y][1] == 'K')
                        {
                            bitmap = new BitmapImage(new Uri("Images/King.png", UriKind.Relative));
                        }
                        else
                        if (chessBoard[x, y][1] == 'W')
                        {
                            bitmap = new BitmapImage(new Uri("Images/Warrior.png", UriKind.Relative));
                        }
                        else
                        {
                            bitmap = new BitmapImage(new Uri("Images/Guard.png", UriKind.Relative));
                        }
                        getCellFigur(x, y).Visibility = Visibility.Visible;
                        getCellFigur(x, y).Source = bitmap;
                    }
                    else
                    {
                        getCellFigur(x, y).Visibility = Visibility.Collapsed;
                    }    
                }
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            Grid clickedGrid = (Grid)sender;

            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri("C:\\Students\\Project\\Imperium\\Imperium\\Images\\move.wav"));

            int column = (int)clickedGrid.GetValue(Grid.ColumnProperty);
            int row = (int)clickedGrid.GetValue(Grid.RowProperty);
            if(trueMove.check == false)
            {
                if (chessBoard[row, column] != "0" && chessBoard[row, column][0] == colorMove)
                {
                    placeFigur();
                    checkMovingShow(row, column, chessBoard[row, column]);
                    trueMove.check = true;
                    trueMove.X = row; trueMove.Y = column;
                    trueMove.Fig = chessBoard[row, column];
                }
            }
            else
            {
                if(checkMoving(row, column, chessBoard[row, column]) == true)
                {
                    chessBoard[row, column] = chessBoard[trueMove.X,trueMove.Y];
                    chessBoard[trueMove.X, trueMove.Y] = "0";
                    


                    mediaPlayer.Play();
                    if (colorMove == 'B') colorMove = 'R';
                    else if (colorMove == 'R') colorMove = 'Y';
                    else if (colorMove == 'Y') colorMove = 'G';
                    else colorMove = 'B';
                }
                trueMove.check = false;
                placeFigur();
                checkWin();
            }
            
            
        }

    }
}
