using System.Reflection;
using AutoFixture;

namespace Sellers;

[AttributeUsage(AttributeTargets.Parameter)]
public class ConnectionStringAttribute : Attribute, IParameterCustomizationSource, ICustomization
{
    public ConnectionStringAttribute(string connectionString) 
        => ConnectionString = connectionString;

    public string ConnectionString { get; }

    public ICustomization GetCustomization(ParameterInfo parameter) => this;

    public void Customize(IFixture fixture) 
        => fixture.Register(() => SellersServer.Create(this.ConnectionString));
}