using Dewey.CLI.Builds;
using Dewey.CLI.Deployments;
using Dewey.CLI.Writers;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repositories;
using Dewey.Manifest.Repository;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.CLI
{
    delegate void ComponentAction(ComponentItem repoComponent, ComponentManifest componentManifest, XElement componentElement);

    class Program
    {
        private static ComponentAction componentAction;
        private static Action<LoadRepositoriesManifestResult> loadRepositoriesManifestResultAction;
        private static ICommand command;

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
                    command = BuildAction.Create(args);
                    //componentAction = BuildComponent;
                    break;
                case "deploy":
                    //componentAction = DeployComponent;
                    break;
                case "list":
                    command = ListItems.Create();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    return;
            }

            if (command == null)
                goto done;

            var loadRepositoriesManifestFileResult = RepositoriesManifest.LoadRepositoriesManifestFile();

            loadRepositoriesManifestFileResult.WriteErrors();

            command.Execute(loadRepositoriesManifestFileResult);

            done:
            Console.ResetColor();
            Console.WriteLine("Continue...");
            Console.ReadLine();
        }

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

        private static void BuildComponent(ComponentItem repoComponent, ComponentManifest componentManifest, XElement componentElement)
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

        private static void DeployComponent(ComponentItem repoComponent, ComponentManifest componentManifest, XElement componentElement)
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
