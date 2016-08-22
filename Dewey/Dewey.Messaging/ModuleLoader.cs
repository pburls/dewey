//using SimpleInjector;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;

//namespace Dewey.Messaging
//{
//    public class ModuleLoader
//    {
//        readonly Dictionary<Type, IModule> _moduleDictionary;

//        public ModuleLoader(Container container)
//        {
//            var moduleInterface = typeof(IModule);
//            Console.WriteLine(moduleInterface.AssemblyQualifiedName);
//            _moduleDictionary = new Dictionary<Type, IModule>();

//            string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//            string modulesDirectory = Path.Combine(executingDirectory, "modules");

//            var assemblies = new List<Assembly>();
//            foreach (string assemblyFilePath in Directory.EnumerateFiles(modulesDirectory, "*.dll", SearchOption.AllDirectories))
//            {
//                var assembly = Assembly.LoadFrom(assemblyFilePath);
//                assemblies.Add(assembly);
//            }

//            Console.WriteLine("Found {0} assemblies.", assemblies.Count);

//            var allTypes = assemblies.SelectMany(s => s.GetTypes()).ToList();
//            var modTypes = allTypes.Where(x => x.Name == "Module").ToList();

//            var aModType = modTypes.FirstOrDefault();
//            if (aModType != null)
//            {
//                var interfaceTypes = aModType.GetInterfaces();
//                foreach (var intType in interfaceTypes)
//                {
//                    Console.Write(intType.AssemblyQualifiedName);
//                    Console.WriteLine(intType == moduleInterface);
//                }
//            }

//            Console.WriteLine("Found {0} types with name 'module'.", modTypes.Count);

//            var moduleTypes = assemblies.SelectMany(s => s.GetTypes())
//                .Where(p => moduleInterface.IsAssignableFrom(p) && p != moduleInterface).ToList();

//            Console.WriteLine("Found {0} module types.", moduleTypes.Count);

//            foreach (var moduleType in moduleTypes)
//            {
//                var module = (IModule)container.GetInstance(moduleType);

//                _moduleDictionary.Add(moduleType, module);
//            }
//        }
//    }
//}
