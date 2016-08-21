namespace Dewey.Test
{
    public class XmlElement
    {
        public string XmlText { get; set; }
        public string ScenarioName { get; set; }

        public override string ToString()
        {
            return ScenarioName;
        }
    }
}
