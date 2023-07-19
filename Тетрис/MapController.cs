using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Тетрис
{
    /// <summary>
    /// Содержит логику работы игры
    /// </summary>
    public class MapController
    {
        private int[,] map;
        private int cellSize = 30;
        private int timeInterval = 300;
        private int height = 20;
        private int width = 10;
        private int score = 0;
        private int lines = 0;
        private Shape currentShape;
        private Form1 mainForm;
        private Timer timer;
        private bool IsPause = false;
        /// <summary>
        /// Возвращает размер игровой клетки
        /// </summary>
        public int CellSize => cellSize;
        /// <summary>
        /// Возвращает высоту игрового поля
        /// </summary>
        public int Height => height;
        /// <summary>
        /// Возвращает ширину игрового поля
        /// </summary>
        public int Width => width;
        /// <summary>
        /// Конструктор, подпсисывающийся на события, связанные с изменением состояния фигуры. После создания объекта запускается таймер игры.
        /// </summary>
        /// <param name="mainForm"> Форма, в которой происходит игра </param>
        public MapController(Form1 mainForm)
        {
            map = new int[height, width];
            this.mainForm = mainForm;
            Shape.CoordinatesChanging += Shape_CoordinatesChanging;
            Shape.CoordinatesChanged += Shape_CoordinatesChanged;
            Shape.IsAvailableToMove = IsAvailableToMove;

            timer = new Timer();
            timer.Interval = timeInterval;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Shape_CoordinatesChanging()
        {
            ResetArea();
        }

        private void Shape_CoordinatesChanged()
        {
            BindShapeWithMap();
            mainForm.Invalidate();
        }

        private bool IsAvailableToMove(HashSet<Coordinates> nextPoition, bool needToSave)
        {
            foreach (Coordinates point in nextPoition)
            {
                if (IsInside(point) == false || map[point.Y, point.X] != 0)
                {
                    if (needToSave)
                    {
                        currentShape.Save(map);
                        RemoveFilledLines();
                        GenerateNewShape();
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Добавляет матрицу фигуры в игровое поле, представляющее собой двумерный массив
        /// </summary>
        public void BindShapeWithMap()
        {
            foreach (Coordinates point in currentShape.BlocksCoordinates)
            {
                if (IsInside(point))
                    map[point.Y, point.X] = (int)currentShape.ShapeType;
                else return;
            }
        }

        private void RemoveFilledLines()
        {
            int removedLines = 0;
            for (int row = 0; row < height; row++)
            {
                int counter = 0;
                for (int column = 0; column < width; column++)
                {
                    if (map[row, column] != 0)
                    {
                        counter++;
                    }
                }
                if (counter == width)
                {
                    removedLines++;
                    for (int newRow = row; newRow > 0; newRow--)
                    {
                        for (int column = 0; column < width; column++)
                        {
                            map[newRow, column] = map[newRow - 1, column];
                        }
                    }
                    if (timeInterval > 10) timeInterval -= 10;
                }
            }
            score += removedLines * 10;
            lines += removedLines;
            if (score > mainForm.BestScore) mainForm.BestScore = score;
            ChangeLabels();
        }

        private void ChangeLabels()
        {
            Label Score = (Label)mainForm.Controls.Find("Score", false).First();
            Score.Text = "Счет: " + score.ToString();
            Label Lines = (Label)mainForm.Controls.Find("Lines", false).First();
            Lines.Text = "Линии: " + lines.ToString();
            Label BestScoreLabel = (Label)mainForm.Controls.Find("BestScoreLabel", false).First();
            BestScoreLabel.Text = "Рекорд: " + mainForm.BestScore.ToString();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentShape.MoveDown();
        }
        /// <summary>
        /// Управление фигурой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    currentShape.MoveRight();
                    break;
                case Keys.Left:
                    currentShape.MoveLeft();
                    break;
                case Keys.Up:
                    currentShape.Rotate();
                    break;
                case Keys.Down:
                    timer.Interval = 15;
                    break;
                case Keys.Space:
                    if (IsPause == false)
                    {
                        timer.Stop();
                        IsPause = true;
                    }
                    else
                    {
                        timer.Start();
                        IsPause = false;
                    }
                    break;
            }
        }
        /// <summary>
        /// возвращает истину, если координаты находится внутри ли игрвого поля
        /// </summary>
        /// <param name="point"> Координаты для проверки </param>
        /// <returns></returns>
        public bool IsInside(Coordinates point)
        {
            if (point.Y >= 0 && point.Y < height && point.X >= 0 && point.X < width) return true;
            return false;
        }
        /// <summary>
        /// Рисует границы игрового поля
        /// </summary>
        /// <param name="g"></param>
        public void DrawGrid(Graphics g)
        {
            for (int row = 0; row <= height; row++)
            {
                g.DrawLine(Pens.Black, new Point(0, row * cellSize), new Point(cellSize * width, row * cellSize));
            }
            for (int column = 0; column <= width; column++)
            {
                g.DrawLine(Pens.Black, new Point(column * cellSize, 0), new Point(column * cellSize, cellSize * height));
            }
        }
        /// <summary>
        /// Рисует фигуры с учетом их цвета
        /// </summary>
        /// <param name="g"></param>
        public void DrawMap(Graphics g)
        {
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    if (map[row, column] != 0)
                    {
                        Rectangle currentCell = new Rectangle(column * cellSize, row * cellSize, cellSize, cellSize);
                        ShapeType shapeType = (ShapeType)map[row, column];
                        Color color = ShapeConfig.Colors[shapeType];
                        g.FillRectangle(new SolidBrush(color), currentCell);
                        g.DrawRectangle(Pens.Black, currentCell);
                    }
                }
            }
        }
        /// <summary>
        /// Сбрасывает все данные об игровом поле
        /// </summary>
        public void ResetArea()
        {
            foreach (Coordinates point in currentShape.BlocksCoordinates)
            {
                map[point.Y, point.X] = 0;
            }
        }
        /// <summary>
        /// Создает новую фигуру, если она первая за игру, либо генерирует ту, которая сохранена в памяти как следующая фигруа.
        /// Затем сохраняются данные для следущей фигруы.
        /// </summary>
        public void GenerateNewShape()
        {
            timer.Interval = timeInterval;
            if (currentShape is null) currentShape = new Shape();
            else currentShape = currentShape.NextShape;
            currentShape.NextShape = new Shape();
            if (IsAvailableToMove(currentShape.BlocksCoordinates, false))
                currentShape.BindMatrixWithCoordinates(0, 4);
            else
            {
                timer.Stop();
                MessageBox.Show($"Конец игры\nВаш счет: {score}");
            }
        }
        /// <summary>
        /// Отображает следующую фигуру, которая появится после текущей.
        /// </summary>
        /// <param name="g"></param>
        public void ShowNextShape(Graphics g)
        {
            foreach (Coordinates point in currentShape.NextShape.BlocksCoordinates)
            {
                Rectangle currentCell = new Rectangle(300 + point.X * cellSize, 100 + point.Y * cellSize, cellSize, cellSize);
                g.FillRectangle(new SolidBrush(ShapeConfig.Colors[currentShape.NextShape.ShapeType]), currentCell);
                g.DrawRectangle(Pens.Black, currentCell);
            }
        }
    }
}
