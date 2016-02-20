using Dewey.CLI.Builds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dewey.CLI
{
    public delegate void ComponentAction(string componentName, string componentLocation, XElement componentElement);

    class Program
    {
        private static ComponentAction componentAction;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No action paramerter specified.");
                return;
            }

            switch (args[0])
            {
                case "build":
                    componentAction = BuildComponent;
                    break;
                default:
                    break;
            }

            string repositoriesManifestFileName = "repositories.xml";
            if(!File.Exists(repositoriesManifestFileName))
            {
                Console.WriteLine("No repositories manifest file '{0}' found in current directory.", repositoriesManifestFileName);
            }

            var repositories = XElement.Load(repositoriesManifestFileName);
            Console.WriteLine("Loaded development repositories manifest file.");

            var repositoryElements = repositories.Elements().Where(x => x.Name.LocalName == "repository").ToList();
            Console.WriteLine("Found {0} repository elements.", repositoryElements.Count);

            foreach (var repo in repositoryElements)
            {
                var repoNameAtt = repo.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
                if (repoNameAtt == null || string.IsNullOrWhiteSpace(repoNameAtt.Value))
                {
                    Console.WriteLine("Skipping repository element without a valid name: {0}", repo.ToString());
                }
                else
                {
                    var repoLocationAtt = repo.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
                    if (repoLocationAtt == null || string.IsNullOrWhiteSpace(repoLocationAtt.Value))
                    {
                        Console.WriteLine("Repository '{0}' has no location set.", repoNameAtt.Value);
                    }
                    else
                    {
                        LoadRepository(repoNameAtt.Value, repoLocationAtt.Value);
                    }
                }
            }
        }

        private static void LoadRepository(string repoName, string repoLocation)
        {
            var repositoryManifestFilePath = Path.Combine(repoLocation, "repository.xml");
            if(!Directory.Exists(repoLocation))
            {
                Console.WriteLine("Unable to find repository directory at location '{1}' for repository '{0}'.", repoName, repoLocation);
            }
            else if (!File.Exists(repositoryManifestFilePath))
            {
                Console.WriteLine("Unable to find repository manifest file at path '{1}' for repository '{0}'.", repoName, repositoryManifestFilePath);
            }
            else
            {
                var repository = XElement.Load(repositoryManifestFilePath);
                Console.WriteLine("Loaded repository '{0}' manifest file.", repoName);

                var componentsElement = repository.Elements().FirstOrDefault(x => x.Name.LocalName == "components");
                if (componentsElement == null || !componentsElement.Elements().Any(x => x.Name.LocalName == "component"))
                {
                    Console.WriteLine("No components found.");
                }
                else
                {
                    var componentElements = componentsElement.Elements().Where(x => x.Name.LocalName == "component").ToList();
                    Console.WriteLine("Found {0} component elements.", componentElements.Count);

                    foreach (var component in componentElements)
                    {
                        var componentNameAtt = component.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
                        if (componentNameAtt == null || string.IsNullOrWhiteSpace(componentNameAtt.Value))
                        {
                            Console.WriteLine("Skipping component element without a valid name: {0}", component.ToString());
                        }
                        else
                        {
                            var componentLocationAtt = component.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
                            if (componentLocationAtt == null || string.IsNullOrWhiteSpace(componentLocationAtt.Value))
                            {
                                Console.WriteLine("Component '{0}' has no location set.", componentNameAtt.Value);
                            }
                            else
                            {
                                LoadComponent(componentNameAtt.Value, Path.Combine(repoLocation, componentLocationAtt.Value));
                            }
                        }
                    }
                }
            }
        }

        private static void LoadComponent(string componentName, string componentLocation)
        {
            var componentManifestFilePath = Path.Combine(componentLocation, "component.xml");
            if (!Directory.Exists(componentLocation))
            {
                Console.WriteLine("Unable to find component directory at location '{1}' for component '{0}'.", componentName, componentLocation);
            }
            else if (!File.Exists(componentManifestFilePath))
            {
                Console.WriteLine("Unable to find component manifest file at path '{1}' for component '{0}'.", componentName, componentManifestFilePath);
            }
            else
            {
                var componentElement = XElement.Load(componentManifestFilePath);
                Console.WriteLine("Loaded component '{0}' manifest file.", componentName);

                var componentNameAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
                if (componentNameAtt == null || string.IsNullOrWhiteSpace(componentNameAtt.Value))
                {
                    Console.WriteLine("Skipping component element without a valid name: {0}", componentElement.ToString());
                }
                else
                {
                    if (componentNameAtt.Value != componentName)
                    {
                        Console.WriteLine("Warning: Component name mismatch. Repository Manifest: '{0}', Component Manifest: '{1}'", componentName, componentNameAtt.Value);
                    }

                    if (componentAction != null)
                    {
                        componentAction(componentName, componentLocation, componentElement);
                    }
                    else
                    {
                        Console.WriteLine("No Action.");
                    }
                }
            }
        }

        private static void BuildComponent(string componentName, string componentLocation, XElement componentElement)
        {
            Console.WriteLine("**Build**");
            var buildsElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "builds");
            if (buildsElement == null || !buildsElement.Elements().Any(x => x.Name.LocalName == "build"))
            {
                Console.WriteLine("No builds found for component '{0}'.", componentName);
            }
            else
            {
                var buildElements = buildsElement.Elements().Where(x => x.Name.LocalName == "build").ToList();
                Console.WriteLine("Found {0} builds for component '{1}'.", buildElements.Count, componentName);

                foreach (var build in buildElements)
                {
                    var buildTypeAtt = build.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                    if (buildTypeAtt == null || string.IsNullOrWhiteSpace(buildTypeAtt.Value))
                    {
                        Console.WriteLine("Skipping build element without a valid type: {0}", build.ToString());
                        continue;
                    }

                    var buildTargetAtt = build.Attributes().FirstOrDefault(x => x.Name.LocalName == "target");
                    if (buildTargetAtt == null || string.IsNullOrWhiteSpace(buildTargetAtt.Value))
                    {
                        Console.WriteLine("Skipping build element without a valid target: {0}", build.ToString());
                        continue;
                    }

                    try
                    {
                        var buildAction = BuildActionFactory.CreateBuildAction(buildTypeAtt.Value);
                        string buildTargetPath = Path.Combine(componentLocation, buildTargetAtt.Value);
                        buildAction.Build(buildTargetPath);
                    }
                    catch (Exception ex)
                    {
                        Log(ex);
                    }
                }
            }
        }

        private static void Log(Exception ex)
        {
            Log(ex.Message);
        }

        private static void Log(string message)
        {
            Log(message, null);
        }

        private static void Log(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
