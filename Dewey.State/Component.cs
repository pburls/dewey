using Dewey.Manifest.Component;

namespace Dewey.State
{
    public class Component
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public Component(ComponentManifest component)
        {
            Name = component.Name;
            Type = component.Type;
        }
    }
}
