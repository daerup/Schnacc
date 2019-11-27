namespace Schnacc.Domain.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Xunit.Sdk;

    internal class repeatAttribute : DataAttribute
    {
        private readonly int count;

        public repeatAttribute(int count)
        {
            this.count = count;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return Enumerable.Repeat(new object[0], this.count);
        }
    }
}