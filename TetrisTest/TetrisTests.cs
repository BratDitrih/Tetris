using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Тетрис;

namespace TetrisTest
{
    [TestClass]
    public class TetrisTests
    {
        private MapController controller = new MapController(null);
        [TestMethod]
        public void CheckCorrectCoordinates()
        {
            bool result = controller.IsInside(new Coordinates(5, 8));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckWrongCoordinates()
        {
            bool result = controller.IsInside(new Coordinates(20, 30));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CloningTest()
        {
            Shape firstShape = new Shape();
            Shape secondShape = firstShape.Clone() as Shape;

            HashSet<Coordinates> firstShapeBlocks = firstShape.BlocksCoordinates;

            foreach (Coordinates point in firstShapeBlocks)
            {
                point.X++;
                point.Y++;
            }

            HashSet<Coordinates> secondShapeBlocks = secondShape.BlocksCoordinates;

            bool success = true;

            for (int i = 0; i < firstShapeBlocks.Count; i++)
            {
                Coordinates firstPoint = firstShapeBlocks.ElementAt(i);
                Coordinates secondPoint = secondShapeBlocks.ElementAt(i);
                if (firstPoint.X == secondPoint.X || firstPoint.Y == secondPoint.Y)
                {
                    success = false;
                }
            }

            Assert.IsTrue(success);
        }
    }
}
