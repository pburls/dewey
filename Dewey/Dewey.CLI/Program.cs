using Dewey.CLI.Builds;
using Dewey.CLI.Deployments;
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
    delegate void ComponentAction(RepositoryComponent repoComponent, ComponentManifest componentManifest, XElement componentElement);

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
                case "deploy":
                    componentAction = DeployComponent;
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
                                var repoComponent = new RepositoryComponent(componentNameAtt.Value, Path.Combine(repoLocation, componentLocationAtt.Value));
                                LoadComponent(repoComponent);
                            }
                        }
                    }
                }
            }
        }

        private static void LoadComponent(RepositoryComponent repoComponent)
        {
            var componentManifestFilePath = Path.Combine(repoComponent.Location, "component.xml");
            if (!Directory.Exists(repoComponent.Location))
            {
                Console.WriteLine("Unable to find component directory at location '{1}' for component '{0}'.", repoComponent.Name, repoComponent.Location);
            }
            else if (!File.Exists(componentManifestFilePath))
            {
                Console.WriteLine("Unable to find component manifest file at path '{1}' for component '{0}'.", repoComponent.Name, componentManifestFilePath);
            }
            else
            {
                var componentElement = XElement.Load(componentManifestFilePath);
                Console.WriteLine("Loaded component '{0}' manifest file.", repoComponent.Name);

                var componentNameAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
                if (componentNameAtt == null || string.IsNullOrWhiteSpace(componentNameAtt.Value))
                {
                    Console.WriteLine("Skipping component element without a valid name: {0}", componentElement.ToString());
                    return;
                }
                else if (componentNameAtt.Value != repoComponent.Name)
                {
                    Console.WriteLine("Warning: Component name mismatch. Repository Manifest: '{0}', Component Manifest: '{1}'", repoComponent.Name, componentNameAtt.Value);
                }

                var componentTypeAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                if (componentTypeAtt == null || string.IsNullOrWhiteSpace(componentTypeAtt.Value))
                {
                    Console.WriteLine("Skipping component element without a valid type: {0}", componentElement.ToString());
                    return;
                }

                var componentManifest = new ComponentManifest(componentNameAtt.Value, componentTypeAtt.Value);

                if (componentAction != null)
                {
                    componentAction(repoComponent, componentManifest, componentElement);
                }
                else
                {
                    Console.WriteLine("No Action.");
                }
            }
        }

        private static void BuildComponent(RepositoryComponent repoComponent, ComponentManifest componentManifest, XElement componentElement)
        {
            Console.WriteLine("**Build**");
            var buildsElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "builds");
            if (buildsElement == null || !buildsElement.Elements().Any(x => x.Name.LocalName == "build"))
            {
                Console.WriteLine("No builds found for component '{0}'.", repoComponent.Name);
            }
            else
            {
                var buildElements = buildsElement.Elements().Where(x => x.Name.LocalName == "build").ToList();
                Console.WriteLine("Found {0} builds for component '{1}'.", buildElements.Count, repoComponent.Name);

                foreach (var buildElement in buildElements)
                {
                    var buildTypeAtt = buildElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                    if (buildTypeAtt == null || string.IsNullOrWhiteSpace(buildTypeAtt.Value))
                    {
                        Console.WriteLine("Skipping build element without a valid type: {0}", buildElement.ToString());
                        continue;
                    }

                    try
                    {
                        var buildAction = BuildActionFactory.CreateBuildAction(buildTypeAtt.Value);
                        buildAction.Build(repoComponent, componentManifest, buildElement);
                    }
                    catch (Exception ex)
                    {
                        Output(ex);
                    }
                }
            }
        }

        private static void DeployComponent(RepositoryComponent repoComponent, ComponentManifest componentManifest, XElement componentElement)
        {
            Output("**Deploy**");
            var deploymentsElement = componentElement.Elements().FirstOrDefault(x => x.Name.LocalName == "deployments");
            if (deploymentsElement == null || !deploymentsElement.Elements().Any(x => x.Name.LocalName == "deployment"))
            {
                Console.WriteLine("No deployments found for component '{0}'.", componentManifest.Name);
            }
            else
            {
                var deploymentElements = deploymentsElement.Elements().Where(x => x.Name.LocalName == "deployment").ToList();
                Console.WriteLine("Found {0} deployment for component '{1}'.", deploymentElements.Count, componentManifest.Name);

                foreach (var deploymentElement in deploymentElements)
                {
                    var deploymentTypeAtt = deploymentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
                    if (deploymentTypeAtt == null || string.IsNullOrWhiteSpace(deploymentTypeAtt.Value))
                    {
                        Console.WriteLine("Skipping deployment element without a valid type: {0}", deploymentElement.ToString());
                        continue;
                    }

                    try
                    {
                        var deploymentAction = DeploymentActionFactory.CreateDeploymentAction(deploymentTypeAtt.Value);
                        deploymentAction.Deploy(repoComponent, componentManifest, deploymentElement);
                    }
                    catch (Exception ex)
                    {
                        Output(ex);
                    }
                }
            }
        }

        private static void Output(Exception ex)
        {
            Output(ex.Message);
        }

        private static void Output(string message)
        {
            Output(message, null);
        }

        private static void Output(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
