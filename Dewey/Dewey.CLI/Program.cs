using Dewey.CLI.Builds;
using Dewey.CLI.Deployments;
using Dewey.Manifest;
using Dewey.Manifest.Component;
using Dewey.Manifest.Repository;
using Dewey.Messaging;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Dewey.CLI
{
    delegate void ComponentAction(ComponentItem repoComponent, ComponentManifest componentManifest, XElement componentElement);

    class Program
    {
        private static ICommand command;

        static void Main(string[] args)
        {
            var eventAggregator = new EventAggregator();
            var commandProcessor = new CommandProcessor(eventAggregator);

            commandProcessor.RegisterHandler<LoadManifestFiles, ManifestLoadHandler>();
            commandProcessor.RegisterHandler<ListItems, ListItemsHandler>();

            if (args.Length < 1)
            {
                Console.WriteLine("No action paramerter specified.");
                return;
            }

            switch (args[0])
            {
                case "build":
                    command = BuildCommand.Create(args);
                    //componentAction = BuildComponent;
                    break;
                case "deploy":
                    //componentAction = DeployComponent;
                    break;
                case "list":
                    command = new ListItems();
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    return;
            }

            if (command == null)
                goto done;

            var manifestLoadResultErrorWriter = new ManifestLoadResultErrorWriter(eventAggregator);

            var commandHandler = commandProcessor.Execute(command);
            if (commandHandler == null)
            {
                Console.WriteLine("No command handler registered for command.");
            }

            done:
            Console.ResetColor();
            Console.WriteLine("Continue...");
            Console.ReadLine();
        }

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
