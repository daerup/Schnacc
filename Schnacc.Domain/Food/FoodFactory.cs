namespace Schnacc.Domain.Food
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;

    public class FoodFactory
    {
        public readonly (int maxRow, int maxColumn) Boundaries;

        public FoodFactory((int maxRow, int maxColumn) boundaries)
        {
            if (boundaries.maxRow < 4)
            {
                boundaries.maxRow = 4;
            }
            if (boundaries.maxColumn< 4)
            {
                boundaries.maxColumn = 4;
            }

            this.Boundaries = boundaries;
        }

        public FoodFactory(Position lastPosition)
        {
            this.Boundaries = (lastPosition.Row, lastPosition.Column);
        }

        public Food CreateRandomFood()
        {
            List<Type> allFoodTypes = this.getAllFoodTypes();
            int randomRow = this.getRandomInt(1, this.Boundaries.maxRow);
            int randomColumn = this.getRandomInt(1, this.Boundaries.maxRow);
            return (Food)Activator.CreateInstance(allFoodTypes.ElementAt(this.getRandomInt(0, allFoodTypes.Count)), new Position(randomRow, randomColumn));
        }

        private List<Type> getAllFoodTypes()
        {
            Type derivedType = typeof(Food);
            return Assembly.GetAssembly(typeof(Food)).GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToList();
        }

        private int getRandomInt(int smallestPossibleNumber, int smallestOutOfRangeNumber)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[4];
                rng.GetBytes(buffer);
                int result = BitConverter.ToInt32(buffer, 0);
                return new Random(result).Next(smallestPossibleNumber, smallestOutOfRangeNumber);
            }
        }
    }
}