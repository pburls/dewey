namespace Dewey.Build
{
    public interface IBuildCommandCache
    {
        bool IsComponentAlreadyBuilt(string component);
    }
}
