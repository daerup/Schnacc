using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Schnacc.Domain.Snake;

namespace Schnacc.Domain.Food
{
    public class FoodFactory : IFoodFactory
    {
        public Food CreateRandomFoodBetweenBoundaries(Position boundaries)
        {
            List<Type> allFoodTypes = this.GetAllFoodTypes();
            int randomRow = this.GetRandomInt(0, boundaries.Row);
            int randomColumn = this.GetRandomInt(0, boundaries.Column);
            return (Food)Activator.CreateInstance(allFoodTypes.ElementAt(this.GetRandomInt(0, allFoodTypes.Count)), new Position(randomRow, randomColumn));
        }

        private List<Type> GetAllFoodTypes()
        {
            Type derivedType = typeof(Food);
            return Assembly.GetAssembly(typeof(Food)).GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToList();
        }

        private int GetRandomInt(int smallestPossibleNumber, int smallestOutOfRangeNumber)
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