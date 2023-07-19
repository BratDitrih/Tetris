using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Тетрис
{
    /// <summary>
    /// Содержит данные о фигуре и хранит данные о следующей фигуре
    /// </summary>
    public class Shape : ICloneable
    {
        /// <summary>
        /// Проверяет возможно ли разместить фигуру в новое местоположение
        /// </summary>
        public static Func<HashSet<Coordinates>, bool, bool> IsAvailableToMove;
        /// <summary>
        /// Вызывается при изменении расположения фигуры
        /// </summary>
        public static event Action CoordinatesChanging;
        /// <summary>
        /// Вызывается после изменения расположения фигруы
        /// </summary>
        public static event Action CoordinatesChanged;
        /// <summary>
        /// Тип фигуры
        /// </summary>
        public ShapeType ShapeType { get; set; }
        /// <summary>
        /// Координаты всех блоков, определяющих фигуру
        /// </summary>
        public HashSet<Coordinates> BlocksCoordinates { get; set; } = new HashSet<Coordinates>();
        /// <summary>
        /// Текущая матрица фигуры
        /// </summary>
        public int[,] CurrentMatrix { get; set; }
        /// <summary>
        /// Данные о следующей фигуре
        /// </summary>
        public Shape NextShape { get; set; }
        private  List<int[,]> AllMatrices { get; set; }
        private int index = 0;
        private Coordinates CenterOfRotation;
        /// <summary>
        /// Создает случайную фигуру на основе данных из ShapeConfiguration
        /// </summary>
        public Shape()
        {
            System.Threading.Thread.Sleep(1);
            Random random = new Random();
            ShapeType = (ShapeType)random.Next(1, ShapeConfig.Matrices.Count() + 1);
            AllMatrices = ShapeConfig.Matrices[ShapeType];
            CurrentMatrix = AllMatrices.First();
            CenterOfRotation = new Coordinates(0, 4);
            BindCoordinates(0, 4);
        }
        /// <summary>
        /// Синхронизирует матрицу с игровым полем с помощью метода BindCoordinates и вызывает события изменения координат
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        public void BindMatrixWithCoordinates(int y, int x)
        {
            BindCoordinates(y, x);
            CoordinatesChanged?.Invoke();
        }
        private void BindCoordinates(int y, int x)
        {
            BlocksCoordinates?.Clear();
            for (int i = y; i < y + CurrentMatrix.GetLength(0); i++)
            {
                for (int j = x; j < x + CurrentMatrix.GetLength(1); j++)
                {
                    if (CurrentMatrix[i - y, j - x] != 0)
                    {
                        BlocksCoordinates.Add(new Coordinates(i, j));
                    }
                }
            }
        }
        /// <summary>
        /// Сохраняет данные о координатах фигуры в двумерный массив игрового поля
        /// </summary>
        /// <param name="map"> Игровое поле для сохранения </param>
        public void Save(int[,] map)
        {
            foreach (Coordinates point in BlocksCoordinates)
            {
                map[point.Y, point.X] = (int)ShapeType;
            }
        }

        private void TryMove(Action<Coordinates> pointAction, Action<Coordinates> centerOfRotationAction, bool needToSave)
        {
            Shape clonedShape = Clone() as Shape;
            foreach (Coordinates point in clonedShape.BlocksCoordinates)
            {
                pointAction?.Invoke(point);
            }
            if (IsAvailableToMove == null) return;
            CoordinatesChanging?.Invoke();
            if (IsAvailableToMove.Invoke(clonedShape.BlocksCoordinates, needToSave))
            {
                BlocksCoordinates = clonedShape.BlocksCoordinates;
                centerOfRotationAction?.Invoke(CenterOfRotation);
                CoordinatesChanged?.Invoke();
            }
        }

        private void TryRotate()
        {
            Shape clonedShape = Clone() as Shape;
            int index = clonedShape.index;
            if (index == AllMatrices.Count - 1)
            {
                clonedShape.CurrentMatrix = AllMatrices.First();
                index = 0;
            }
            else
            {
                clonedShape.CurrentMatrix = AllMatrices[++index];
            }
            CoordinatesChanging?.Invoke();
            clonedShape.BindCoordinates(CenterOfRotation.Y, CenterOfRotation.X);
            if (IsAvailableToMove(clonedShape.BlocksCoordinates, false))
            {
                this.BlocksCoordinates = clonedShape.BlocksCoordinates;
                this.index = index;
                this.CurrentMatrix = AllMatrices[index];
                BindMatrixWithCoordinates(CenterOfRotation.Y, CenterOfRotation.X);
            }
        }
        /// <summary>
        /// Ускоряет падение фигуры
        /// </summary>
        public void MoveDown()
        {
            TryMove(point => point.Y++, centerOfRotation => centerOfRotation.Y++, true);
        }
        /// <summary>
        /// Двигает фигуру вправо
        /// </summary>
        public void MoveRight()
        {
            TryMove(point => point.X++, centerOfRotation => centerOfRotation.X++, false);
        }
        /// <summary>
        /// Двигает фигуру влево
        /// </summary>
        public void MoveLeft()
        {
            TryMove(point => point.X--, centerOfRotation => centerOfRotation.X--, false);
        }
        /// <summary>
        /// Вращает фигуру по часовой стрелке
        /// </summary>
        public void Rotate()
        {
            TryRotate();
        }
        /// <summary>
        /// Клонирует фигуру
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Shape clone = new Shape();
            HashSet<Coordinates> clonedBlocksCoordinates = new HashSet<Coordinates>();
            foreach (Coordinates coord in BlocksCoordinates)
            {
                clonedBlocksCoordinates.Add(new Coordinates(coord.Y, coord.X));
            }
            clone.BlocksCoordinates = clonedBlocksCoordinates;
            clone.index = index;
            return clone;
        }
    }
}
