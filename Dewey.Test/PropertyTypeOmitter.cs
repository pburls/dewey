using Ploeh.AutoFixture.Kernel;
using System;
using System.Reflection;

namespace Dewey.Test
{
    public class PropertyTypeOmitter : ISpecimenBuilder
    {
        private readonly Type type;

        public PropertyTypeOmitter(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.type = type;
        }

        public Type Type
        {
            get { return this.type; }
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo != null && propInfo.PropertyType == type)
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
