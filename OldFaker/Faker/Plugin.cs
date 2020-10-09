using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Faker
{
    class Plugin
    {
		public List<IGenerator> Plugins;
		private string pluginPath;

		public Plugin()
		{
			Plugins = new List<IGenerator>();
			pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
			Plugins.Clear();
			DirectoryInfo pluginDirectory = new DirectoryInfo(pluginPath);
			if (!pluginDirectory.Exists)
				pluginDirectory.Create();

			var pluginFiles = Directory.GetFiles(pluginPath, "*.dll");
			foreach (var file in pluginFiles)
			{
				Assembly asm = Assembly.LoadFrom(file);
				var types = asm.GetTypes();
				foreach (Type type in types)
				{
					if (Array.Exists(type.GetInterfaces(), inter => inter.Name == "IGenerator"))
					{
						IGenerator plugin = Activator.CreateInstance(type) as IGenerator;
						Plugins.Add(plugin);
					}
				}
			}
		}
	}
}
