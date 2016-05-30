using Dewey.CLI.Builds;
using Dewey.CLI.Deployments;
using Dewey.CLI.Writers;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
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

            var loadRepositoriesManifestFileResult = RepositoriesManifest.LoadRepositoriesManifestFile();

            var loadRepositoryItemResults = new List<LoadRepositoryItemResult>();
            if (loadRepositoriesManifestFileResult.RepositoriesManifest != null)
            {
                foreach (var repositoryItem in loadRepositoriesManifestFileResult.RepositoriesManifest.RepositoryItems)
                {
                    loadRepositoryItemResults.Add(RepositoryManifest.LoadRepositoryItem(repositoryItem, loadRepositoriesManifestFileResult.RepositoriesManifestFile.DirectoryName));
                }
            }

            var loadComponentItemResults = new List<LoadComponentItemResult>();
            foreach (var loadRepositoryItemResult in loadRepositoryItemResults)
            {
                if (loadRepositoryItemResult.RepositoryManifest != null)
                {
                    foreach (var componentItem in loadRepositoryItemResult.RepositoryManifest.ComponentItems)
                    {
                        if (componentItem != null)
                        {
                            loadComponentItemResults.Add(ComponentManifest.LoadComponentItem(componentItem, loadRepositoryItemResult.RepositoryManifestFile.DirectoryName));
                        }
                    }
                }
            }

            //var loadComponentItemResults = new List<LoadC>

            loadRepositoriesManifestFileResult.WriteErrors();
            loadRepositoryItemResults.WriteErrors();

            Console.ReadLine();
        }

        //private static void LoadRepository(RepositoryItem repositoryItem)
        //{
        //    var repositoryManifestFilePath = Path.Combine(repositoryItem.RelativeLocation, "repository.xml");
        //    if (!Directory.Exists(repositoryItem.RelativeLocation))
        //    {
        //        Console.WriteLine("Unable to find repository directory at location '{1}' for repository '{0}'.", repositoryItem.Name, repositoryItem.RelativeLocation);
        //    }
        //    else if (!File.Exists(repositoryManifestFilePath))
        //    {
        //        Console.WriteLine("Unable to find repository manifest file at path '{1}' for repository '{0}'.", repositoryItem.Name, repositoryManifestFilePath);
        //    }
        //    else
        //    {
        //        var repository = XElement.Load(repositoryManifestFilePath);
        //        Console.WriteLine("Loaded repository '{0}' manifest file.", repositoryItem.Name);

        //        var componentsElement = repository.Elements().FirstOrDefault(x => x.Name.LocalName == "components");
        //        if (componentsElement == null || !componentsElement.Elements().Any(x => x.Name.LocalName == "component"))
        //        {
        //            Console.WriteLine("No components found.");
        //        }
        //        else
        //        {
        //            var componentElements = componentsElement.Elements().Where(x => x.Name.LocalName == "component").ToList();
        //            Console.WriteLine("Found {0} component elements.", componentElements.Count);

        //            foreach (var component in componentElements)
        //            {
        //                var componentNameAtt = component.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
        //                if (componentNameAtt == null || string.IsNullOrWhiteSpace(componentNameAtt.Value))
        //                {
        //                    Console.WriteLine("Skipping component element without a valid name: {0}", component.ToString());
        //                }
        //                else
        //                {
        //                    var componentLocationAtt = component.Attributes().FirstOrDefault(x => x.Name.LocalName == "location");
        //                    if (componentLocationAtt == null || string.IsNullOrWhiteSpace(componentLocationAtt.Value))
        //                    {
        //                        Console.WriteLine("Component '{0}' has no location set.", componentNameAtt.Value);
        //                    }
        //                    else
        //                    {
        //                        var repoComponent = new RepositoryComponent(componentNameAtt.Value, Path.Combine(repositoryItem.RelativeLocation, componentLocationAtt.Value));
        //                        LoadComponent(repoComponent);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void LoadComponent(RepositoryComponent repoComponent)
        //{
        //    var componentManifestFilePath = Path.Combine(repoComponent.Location, "component.xml");
        //    if (!Directory.Exists(repoComponent.Location))
        //    {
        //        Console.WriteLine("Unable to find component directory at location '{1}' for component '{0}'.", repoComponent.Name, repoComponent.Location);
        //    }
        //    else if (!File.Exists(componentManifestFilePath))
        //    {
        //        Console.WriteLine("Unable to find component manifest file at path '{1}' for component '{0}'.", repoComponent.Name, componentManifestFilePath);
        //    }
        //    else
        //    {
        //        var componentElement = XElement.Load(componentManifestFilePath);
        //        Console.WriteLine("Loaded component '{0}' manifest file.", repoComponent.Name);

        //        var componentNameAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "name");
        //        if (componentNameAtt == null || string.IsNullOrWhiteSpace(componentNameAtt.Value))
        //        {
        //            Console.WriteLine("Skipping component element without a valid name: {0}", componentElement.ToString());
        //            return;
        //        }
        //        else if (componentNameAtt.Value != repoComponent.Name)
        //        {
        //            Console.WriteLine("Warning: Component name mismatch. Repository Manifest: '{0}', Component Manifest: '{1}'", repoComponent.Name, componentNameAtt.Value);
        //        }

        //        var componentTypeAtt = componentElement.Attributes().FirstOrDefault(x => x.Name.LocalName == "type");
        //        if (componentTypeAtt == null || string.IsNullOrWhiteSpace(componentTypeAtt.Value))
        //        {
        //            Console.WriteLine("Skipping component element without a valid type: {0}", componentElement.ToString());
        //            return;
        //        }

        //        var componentManifest = new ComponentManifest(componentNameAtt.Value, componentTypeAtt.Value);

        //        if (componentAction != null)
        //        {
        //            componentAction(repoComponent, componentManifest, componentElement);
        //        }
        //        else
        //        {
        //            Console.WriteLine("No Action.");
        //        }
        //    }
        //}

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
