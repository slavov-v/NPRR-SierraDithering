using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SierraDithering
{
    public partial class Form1 : Form
    {
        public Bitmap OriginalImage = null;
        public Bitmap NewImage = null;
        public bool BlackAndWhite = false;

        public Form1()
        {
            InitializeComponent();
            imgContainer.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public int applyThreshold(int value)
        {
            return value < 128 ? 0 : 255;
        }

        public int normalizeColorByte(int colorByte)
        {
            if (colorByte < 0) return 0;
            if (colorByte > 255) return 255;

            return colorByte;
        }

        public Color getFilteredColor(int x, int y, double divisor, int errorRed, int errorGreen, int errorBlue)
        {
            int red = normalizeColorByte(NewImage.GetPixel(x, y).R + (int)Math.Round(errorRed * divisor));
            int green = normalizeColorByte(NewImage.GetPixel(x, y).G + (int)Math.Round(errorGreen * divisor));
            int blue = normalizeColorByte(NewImage.GetPixel(x, y).B + (int)Math.Round(errorBlue * divisor));
            
            return Color.FromArgb(red, green, blue);

        }

        public void mutateIndexSafely(int x, int y, double divisor, int errorRed, int errorGreen, int errorBlue)
        {
            if ((x < NewImage.Width && x >= 0) && (y < NewImage.Height && y >= 0 ))
            {
                Color color = getFilteredColor(x, y, divisor, errorRed, errorGreen, errorBlue);
                NewImage.SetPixel(x, y, color);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select Image";
            fileDialog.ShowDialog();

            OriginalImage = new Bitmap(fileDialog.FileName);

            imgContainer.Image = OriginalImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewImage = new Bitmap(OriginalImage);

            if (OriginalImage == null)
            {
                MessageBox.Show("Load an image first");
                return;
            }

            for (int y = 0; y < NewImage.Height; y++)
            {
                for (int x = 0; x < NewImage.Width; x++)
                {
                    Color currentPixel = NewImage.GetPixel(x, y);

                    int redOld = currentPixel.R;
                    int greenOld = currentPixel.G;
                    int blueOld = currentPixel.B;

                    int redNew = 0;
                    int greenNew = 0;
                    int blueNew = 0;

                    if (!BlackAndWhite)
                    {
                        redNew = applyThreshold(redOld);
                        greenNew = applyThreshold(greenOld);
                        blueNew = applyThreshold(blueOld);
                    }
                    else
                    {
                        int gray = (int)(0.299 * currentPixel.R + 0.587 * currentPixel.G + 0.114 * currentPixel.B);
                        int newBit = gray < 128 ? 0 : 255;

                        redNew = newBit;
                        greenNew = newBit;
                        blueNew = newBit;
                    }

                    NewImage.SetPixel(x, y, Color.FromArgb(redNew, greenNew, blueNew));

                    int redError = redOld - redNew;
                    int greenError = greenOld - greenNew;
                    int blueError = blueOld - blueNew;

                    mutateIndexSafely(x + 1, y, (double)2 / (double)4, redError, greenError, blueError);
                    mutateIndexSafely(x - 1, y + 1, (double)1 / (double)4, redError, greenError, blueError);
                    mutateIndexSafely(x, y + 1, (double)1 / (double)4, redError, greenError, blueError);
                }
            }

            imgContainer.Image = NewImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(OriginalImage == null)
            {
                MessageBox.Show("No Image loaded");
                return;
            }

            imgContainer.Image = OriginalImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NewImage = new Bitmap(OriginalImage);

            if (OriginalImage == null)
            {
                MessageBox.Show("Load an image first");
                return;
            }

            for (int y = 0; y < NewImage.Height; y++)
            {
                for (int x = 0; x < NewImage.Width; x++)
                {
                    Color currentPixel = NewImage.GetPixel(x, y);

                    int redOld = currentPixel.R;
                    int greenOld = currentPixel.G;
                    int blueOld = currentPixel.B;

                    int redNew = 0;
                    int greenNew = 0;
                    int blueNew = 0;

                    if (!BlackAndWhite)
                    {
                        redNew = applyThreshold(redOld);
                        greenNew = applyThreshold(greenOld);
                        blueNew = applyThreshold(blueOld);
                    }
                    else
                    {
                        int gray = (int)(0.299 * currentPixel.R + 0.587 * currentPixel.G + 0.114 * currentPixel.B);
                        int newBit = gray < 128 ? 0 : 255;

                        redNew = newBit;
                        greenNew = newBit;
                        blueNew = newBit;
                    }

                    NewImage.SetPixel(x, y, Color.FromArgb(redNew, greenNew, blueNew));

                    int redError = redOld - redNew;
                    int greenError = greenOld - greenNew;
                    int blueError = blueOld - blueNew;

                    mutateIndexSafely(x + 1, y, (double)4 / (double)16, redError, greenError, blueError);
                    mutateIndexSafely(x + 2, y, (double)3 / (double)16, redError, greenError, blueError);
                    mutateIndexSafely(x - 2, y + 1, (double)1 / (double)16, redError, greenError, blueError);
                    mutateIndexSafely(x - 1, y + 1, (double)2 / (double)16, redError, greenError, blueError);
                    mutateIndexSafely(x, y + 1, (double)3 / (double)16, redError, greenError, blueError);
                    mutateIndexSafely(x + 1, y + 1, (double)2 / (double)16, redError, greenError, blueError);
                    mutateIndexSafely(x + 2, y + 1, (double)1 / (double)16, redError, greenError, blueError);
                }
            }

            imgContainer.Image = NewImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NewImage = new Bitmap(OriginalImage);

            if (OriginalImage == null)
            {
                MessageBox.Show("Load an image first");
                return;
            }

            for (int y = 0; y < NewImage.Height; y++)
            {
                for (int x = 0; x < NewImage.Width; x++)
                {
                    Color currentPixel = NewImage.GetPixel(x, y);

                    int redOld = currentPixel.R;
                    int greenOld = currentPixel.G;
                    int blueOld = currentPixel.B;

                    int redNew = 0;
                    int greenNew = 0;
                    int blueNew = 0;

                    if(!BlackAndWhite)
                    {
                        redNew = applyThreshold(redOld);
                        greenNew = applyThreshold(greenOld);
                        blueNew = applyThreshold(blueOld);
                    }else
                    {
                        int gray = (int)(0.299 * currentPixel.R + 0.587 * currentPixel.G + 0.114 * currentPixel.B);
                        int newBit = gray < 128 ? 0 : 255;

                        redNew = newBit;
                        greenNew = newBit;
                        blueNew = newBit;
                    }

                    NewImage.SetPixel(x, y, Color.FromArgb(redNew, greenNew, blueNew));

                    int redError = redOld - redNew;
                    int greenError = greenOld - greenNew;
                    int blueError = blueOld - blueNew;

                    mutateIndexSafely(x + 1, y, (double)5 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x + 2, y, (double)3 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x - 2, y + 1, (double)2 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x - 1, y + 1, (double)4 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x, y + 1, (double)5 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x + 1, y + 1, (double)4 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x + 2, y + 1, (double)2 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x - 1, y + 2, (double)2 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x, y + 2, (double)3 / (double)32, redError, greenError, blueError);
                    mutateIndexSafely(x + 1, y + 2, (double)2 / (double)32, redError, greenError, blueError);
                }
            }

            imgContainer.Image = NewImage;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BlackAndWhite = !BlackAndWhite;

            button6.Text = BlackAndWhite ? "Preserve colors" : "Do not preserve colors";

        }
    }
}
