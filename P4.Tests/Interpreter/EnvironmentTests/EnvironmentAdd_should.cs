using P4.Interpreter;
using Environment = P4.Interpreter.Environment;

namespace P4.Tests.Interpreter.EnvironmentTests;
using Xunit;

public class EnvironmentAdd_should
{
   private readonly Environment _environment = new();

   [Fact]
    public void Add_AddsValueToEnvironment_Success()
    {
        // Arrange
        var name = "testVar";
        var value = 1;
        
        
        // Act
        _environment.DeclareVariable(name, value);
    
        // Assert
        Assert.Equal(value, _environment.Get(name));
    }
   
    [Fact]
    public void Set_UpdatesValueToEnvironment_ThrowsException()
    {
        // Arrange
        var name = "testVar";
        var value = 1;
        
        // Act + Assert
        Assert.Throws<Exception>(() => _environment.Set(name, value));
    }
   
}