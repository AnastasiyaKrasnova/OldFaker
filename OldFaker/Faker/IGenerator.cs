using System;

namespace Faker
{
    public interface IGenerator
    {
        Type[] PossibleTypes { get; }
        object Generate(Type type);
    }
}
