namespace Dewey.Build
{
    public interface IBuildActionFactory
    {
        IBuildAction CreateBuildAction(string buildType);
    }
}