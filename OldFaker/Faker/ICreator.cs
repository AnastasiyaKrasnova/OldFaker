using System;

namespace Faker
{
    interface ICreator
    {
        object CreateInstance(Type type);
    }
}
