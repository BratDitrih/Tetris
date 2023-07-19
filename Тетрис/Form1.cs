using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Тетрис
{
    /// <summary>
    /// Класс формы
    /// </summary>
    public partial class Form1 : Form
    {
        private bool IsStarted = false;
        private int indent = 50;
        MapController controller;
        /// <summary>
        /// Рекордный счет, который сохраняется в конце игры, если был побит
        /// </summary>
        public int BestScore { get; set; }
        /// <summary>
        /// Конструктор формы. Задает размеры и контроллер игры MapController
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(650, 800);
            controller = new MapController(this);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(indent, indent);
            if (IsStarted == false)
            {
                controller.GenerateNewShape();
                IsStarted = true;
            }
            controller.DrawGrid(g);
            controller.DrawMap(g);
            controller.ShowNextShape(g);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            controller.KeyUp(sender, e);
        }

        private void CreateLabels()
        {
            int distance = 2 * indent + controller.CellSize * controller.Width;
            Label BestScoreLabel = CreateLabel($"Рекорд: {BestScore}", new Point(distance, this.Height / 3 + 20), new Size(200, 50));
            BestScoreLabel.Name = "BestScoreLabel";
            Label Score = CreateLabel("Счет: 0", new Point(BestScoreLabel.Location.X, BestScoreLabel.Location.Y + BestScoreLabel.Height), new Size(BestScoreLabel.Width, BestScoreLabel.Height));
            Score.Name = "Score";
            Label Lines = CreateLabel("Линии: 0", new Point(Score.Location.X, Score.Location.Y + Score.Height), new Size(Score.Width, Score.Height));
            Lines.Name = "Lines";
            Label NextShape = CreateLabel("Следующая фигура:", new Point(Score.Location.X, indent), new Size(Score.Width, Score.Height));
        }

        private Label CreateLabel(string text, Point point, Size size)
        {
            Label label = new Label();
            label.Location = point;
            label.Size = size;
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(label);
            return label;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int loadedScore = 0;
            if (File.Exists("BestScore.txt"))
            {
                int.TryParse(File.ReadAllText("BestScore.txt"), out loadedScore);
            }
            else File.Create("BestScore.txt");
            BestScore = loadedScore;
            CreateLabels();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            File.WriteAllText("BestScore.txt", BestScore.ToString());
        }
    }
}
