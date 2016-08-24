using Dewey.Messaging;
using Dewey.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dewey.CLI
{
    public class CLICommandManager : IEventHandler<ModuleLoaded>
    {
        readonly Dictionary<string, ICLICommandProvider> _commandDictionary;

        public IEnumerable<string> CommandWords { get { return _commandDictionary.Keys; } }

        public CLICommandManager(IEventAggregator eventAggregator)
        {
            _commandDictionary = new Dictionary<string, ICLICommandProvider>();

            eventAggregator.Subscribe(this);
        }

        public ICommand CreateCommandForArgs(string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("The args array must contain at least one value.", "args");

            string commandWord = args[0];

            ICLICommandProvider commandProvider;
            if (!_commandDictionary.TryGetValue(commandWord, out commandProvider))
            {
                return null;
            }

            return commandProvider.CreateCommand(args);
        }

        public void Handle(ModuleLoaded moduleLoaded)
        {
            var cliCommandProviderType = typeof(ICLICommandProvider);

            var moduleAssemblyTypes = moduleLoaded.Module.GetType().Assembly.GetTypes();
            var commandProviderTypes = moduleAssemblyTypes.Where(p => cliCommandProviderType.IsAssignableFrom(p) && p != cliCommandProviderType).ToList();

            foreach (var commandProviderType in commandProviderTypes)
            {
                var commandProvider = (ICLICommandProvider)Activator.CreateInstance(commandProviderType);

                foreach (var commandWord in commandProvider.CommandWords)
                {
                    _commandDictionary.Add(commandWord, commandProvider);
                }
            }
        }
    }
}
