using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicID3Filler
{
    public partial class Form1 : Form
    {
        private TagLib.File f1;
        private int current = 0;

        private string[] paths;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                paths = openFileDialog1.FileNames;
                UpdateCounter();
                LoadCurrent();
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            saveCurrent();

            if (current < paths.Length - 1)
            {
                current++;
                UpdateCounter();
                LoadCurrent();
            }
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            saveCurrent();

            if (current > 0)
            {
                current--;
                UpdateCounter();
                LoadCurrent();
            }
        }

        private void autoFillButton_Click(object sender, EventArgs e)
        {
            string path = "";
            string[] splits = null;
            string[] splitter = new string[] { " - " };

            for (int i = 0; i < paths.Length; i++)
            {
                f1 = TagLib.File.Create(paths[i]);

                path = paths[i].Substring(paths[i].LastIndexOf('\\') + 1);
                path = path.Replace(".mp3", "");

                splits = path.Split(splitter, StringSplitOptions.None);
                if (splits.Length == 1)
                    f1.Tag.Title = splits[0];
                else
                {
                    f1.Tag.Title = splits[1];
                    f1.Tag.Artists = null;
                    f1.Tag.Artists = new string[] { splits[0] };
                }

                f1.Save();
            }
        }

        private void LoadCurrent()
        {
            f1 = TagLib.File.Create(paths[current]);
            nameTextBox.Text = paths[current].Substring(paths[current].LastIndexOf('\\') + 1);
            titleTextBox.Text = f1.Tag.Title;

            if (f1.Tag.Artists.Length > 0)
                artistTextBox.Text = f1.Tag.Artists[0];
            else
                artistTextBox.Text = "";
        }

        private void saveCurrent()
        {
            f1.Tag.Title = titleTextBox.Text;
            if (artistTextBox.Text != "")
            {
                f1.Tag.Artists = null;
                f1.Tag.Artists = new string[] {artistTextBox.Text};
            }
            f1.Save();
        }

        private void UpdateCounter()
        {
            counterLabel.Text = (current + 1) + "/" + paths.Length;
        }
    }
}
