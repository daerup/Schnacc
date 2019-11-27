namespace Schnacc.Domain.Food
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;

    public class FoodFactory
    {
        public Food CreateRandomFood()
        {
            List<Type> allFoodTypes = this.getAllFoodTypes();
            return (Food)Activator.CreateInstance(allFoodTypes.ElementAt(this.getRandomIndexOfSpectrum(allFoodTypes.Count)));
        }

        private List<Type> getAllFoodTypes()
        {
            Type derivedType = typeof(Food);
            return Assembly.GetAssembly(typeof(Food)).GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToList();
        }

        private int getRandomIndexOfSpectrum(int spectrum)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[4];
                rng.GetBytes(buffer);
                int result = BitConverter.ToInt32(buffer, 0);
                return new Random(result).Next(0, spectrum);
            }
        }
    }
}