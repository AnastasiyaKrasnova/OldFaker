using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Faker
{
    class FakeCreator: ICreator
    {
		private ObjectCreator objectCreator = new ObjectCreator();
		private List<Type> parentTypes= new List<Type>();
		private Stack<Type> nesting = new Stack<Type>();
		private static bool f = true;

		public object CreateInstance(Type type)
		{
			bool isNoCycleRecurtion = false;

			if (nesting.Count == 0)
				isNoCycleRecurtion = true;
			else if (!parentTypes.Contains(nesting.Peek()))
				isNoCycleRecurtion = true;

			ConstructorInfo[] constructorsInfo = type.GetConstructors();
			if (constructorsInfo.Length == 0)
            {
				constructorsInfo=type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
			}
			if (constructorsInfo.Length > 0 && isNoCycleRecurtion)
			{

				int index = 0;
				object obj;

				if (nesting.Count != 0)
					parentTypes.Add(nesting.Peek());
				nesting.Push(type);
				constructorsInfo = ChooseConstructor(constructorsInfo);
				do
				{
					obj = TestConstructor(constructorsInfo[index]);
					if (obj == null) index++;
					else
					{
						SetPropertiesOutConstructor(obj, type);
						SetFieldsOutConstructor(obj, type);
					}
				}
				while (index < constructorsInfo.Length && obj == null);
				nesting.Pop();
				if (nesting.Count != 0)
					parentTypes.Remove(nesting.Peek());
				return obj;
			}
			else return null;
				
		}

		private object TestConstructor(ConstructorInfo constructor)
		{
			ParameterInfo[] parametersInfo = constructor.GetParameters();
			object[] parameters = new object[parametersInfo.Length];
			int i = 0;
			foreach (var parameterInfo in parametersInfo)
			{
				parameters[i] = objectCreator.CreateInstance(parameterInfo.ParameterType)?? CreateInstance(parameterInfo.ParameterType);
				i++;
			}
            try
            {
				object obj = constructor.Invoke(parameters);
				return obj;
			}
            catch(Exception e)
            {
				Console.WriteLine(e.Message);
				return null;
			}
		}

		private ConstructorInfo[] ChooseConstructor(ConstructorInfo[] constr)
        {
			ConstructorInfo temp;
			for (int i = 0; i < constr.Length - 1; i++)
			{
				for (int j = i + 1; j < constr.Length; j++)
				{
					if (constr[i].GetParameters().Length < constr[j].GetParameters().Length)
					{
						temp = constr[i];
						constr[i] = constr[j];
						constr[j] = temp;
					}
				}
			}
			return constr;
		}

		private void SetPropertiesOutConstructor(object obj, Type type)
		{
			PropertyInfo[] propertyInfo = type.GetProperties();
			for (int i = 0; i < propertyInfo.Length; i++)
			{
				if (propertyInfo[i].CanWrite)
				{
					if (propertyInfo[i].GetValue(obj) == null)
                    {
						object value = objectCreator.CreateInstance(propertyInfo[i].PropertyType) ?? CreateInstance(propertyInfo[i].PropertyType);
						propertyInfo[i].SetValue(obj, value);
					}
					else if (propertyInfo[i].GetValue(obj).Equals(GetDefaultValue(propertyInfo[i].PropertyType)))
					{
						object value = objectCreator.CreateInstance(propertyInfo[i].PropertyType) ?? CreateInstance(propertyInfo[i].PropertyType);
						propertyInfo[i].SetValue(obj, value);
					}
					else Console.WriteLine("Property {0} already initialized", propertyInfo[i].Name);

				}
			}
		}

		private void SetFieldsOutConstructor(object obj, Type type)
		{
			FieldInfo[] fieldInfo = type.GetFields();
			for (int i = 0; i < fieldInfo.Length; i++)
			{
				if (!(fieldInfo[i].IsInitOnly || fieldInfo[i].IsStatic))
				{
					if (fieldInfo[i].GetValue(obj) == null)
					{
						object value = objectCreator.CreateInstance(fieldInfo[i].FieldType) ?? CreateInstance(fieldInfo[i].FieldType);
						fieldInfo[i].SetValue(obj, value);
					}
					else if (fieldInfo[i].GetValue(obj).Equals(GetDefaultValue(fieldInfo[i].FieldType)))
					{
						object value = objectCreator.CreateInstance(fieldInfo[i].FieldType) ?? CreateInstance(fieldInfo[i].FieldType);
						fieldInfo[i].SetValue(obj, value);
					}
					else Console.WriteLine("Field {0} already initialized", fieldInfo[i].Name);
				}
			}
		}

		private static object GetDefaultValue(Type t)
		{
			if (t.IsValueType)
				return Activator.CreateInstance(t);
			else
				return null;
		}
	}
}
