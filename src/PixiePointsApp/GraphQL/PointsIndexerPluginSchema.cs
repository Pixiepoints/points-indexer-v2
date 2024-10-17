using AeFinder.Sdk;

namespace PixiePointsApp.GraphQL;

public class PointsIndexerPluginSchema : AppSchema<Query>
{
    public PointsIndexerPluginSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}