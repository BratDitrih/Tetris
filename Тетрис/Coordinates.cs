using System;

namespace Тетрис
{
    /// <summary>
    /// Пользовательский класс координат
    /// </summary>
    public class Coordinates
    {
        /// <summary>
        /// Координата X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Координата Y
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="y"> Координата Y </param>
        /// <param name="x"> Координата X </param>
        public Coordinates(int y, int x)
        {
            X = x;
            Y = y;
        }
    }
}
