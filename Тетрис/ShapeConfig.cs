using System.Collections.Generic;
using System.Drawing;

namespace Тетрис
{
    /// <summary>
    /// Класс, содержащий конфигурационные данные об фигурах
    /// </summary>
    public static class ShapeConfig
    {
        /// <summary>
        /// Возвращает матрицу фигуры в виде двумерного массива по ее названию
        /// </summary>
        static public Dictionary<ShapeType, List<int[,]>> Matrices = new Dictionary<ShapeType, List<int[,]>>()
        {
            {ShapeType.J, new List<int[,]>() {
                new int[,] {
                {0,1},
                {0,1},
                {1,1} },
                new int[,] {
                {1,0,0},
                {1,1,1} },
                new int[,] {
                {1,1},
                {1,0},
                {1,0} },
                new int[,] {
                {1,1,1},
                {0,0,1} }
            }},
            {ShapeType.I, new List<int[,]>() {
                new int[,] {
                {1},
                {1},
                {1},
                {1} },
                new int[,] {
                {1,1,1,1} }
            }},
            {ShapeType.O, new List<int[,]>() {
                new int[,] {
                {1,1},
                {1,1} }
            }},
            {ShapeType.L, new List<int[,]>() {
                new int[,] {
                {1,0},
                {1,0},
                {1,1} },
                new int[,] {
                {1,1,1},
                {1,0,0} },
                new int[,] {
                {1,1},
                {0,1},
                {0,1} },
                new int[,] {
                {0,0,1},
                {1,1,1} }
            }},
            {ShapeType.Z, new List<int[,]>() {
                new int[,] {
                {1,1,0},
                {0,1,1} },
                new int[,] {
                {0,1},
                {1,1},
                {1,0} }
            }},
            {ShapeType.T, new List<int[,]>() {
                new int[,] {
                {0,1,0},
                {1,1,1} },
                new int[,] {
                {1,0},
                {1,1},
                {1,0} },
                new int[,] {
                {1,1,1},
                {0,1,0} },
                new int[,] {
                {0,1},
                {1,1},
                {0,1} }
            }},
            {ShapeType.S, new List<int[,]>() {
                new int[,] {
                {0,1,1},
                {1,1,0} },
                new int[,] {
                {1,0},
                {1,1},
                {0,1} }
            }},
            {ShapeType.NewFigure, new List<int[,]>() {
                new int[,] {
                {1,0},
                {1,1} },
                new int[,] {
                {1,1},
                {1,0} },
                new int[,] {
                {1,1},
                {0,1} },
                new int[,] {
                {0,1},
                {1,1} }
            }}
        };
        /// <summary>
        /// Возвращает цвет для фигуры по ее названию
        /// </summary>
        static public Dictionary<ShapeType, Color> Colors = new Dictionary<ShapeType, Color>()
        {
            { ShapeType.J, Color.Blue },
            { ShapeType.I, Color.Aqua },
            { ShapeType.O, Color.Yellow },
            { ShapeType.L, Color.Orange },
            { ShapeType.Z, Color.Red },
            { ShapeType.T, Color.Purple },
            { ShapeType.S, Color.Green },
            { ShapeType.NewFigure, Color.Black}
        };
    }
}
