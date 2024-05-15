using Assert = Xunit.Assert;
using Environment = P4.Interpreter.Environment;

namespace P4.Tests.Interpreter.EnvironmentTests;

public class Environment_VariableDeclare_Should
{
    public Environment testenvironment = new ();
    
    [Fact]
    public void EnvironmentNode_DeclareVariable_Success()
    {
        // Arrange
        String teststring = "teststring";
        

        // Act
        testenvironment.DeclareVariable(teststring);
        object result = testenvironment.Get(teststring);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void EnvironmentNode_DeclareVariableWithAdd_Success()
    {
        // Arrange
        String teststring = "teststring";
        object testvalue = "testvalue";
        
        // Act
        testenvironment.DeclareVariable(teststring);
        testenvironment.Add(teststring, testvalue);
        object achievedvalue = testenvironment.Get(teststring);
        
        // Assert
        Assert.Equal("testvalue",achievedvalue);
    }
    
    [Fact]
    public void EnvironmentNode_NotDeclareVariable_Throws()
    {
        // Arrange
        String teststring = "teststring";
        
        // In case of no Act
        //object result = testenvironment.Get(teststring);
        
        // Assert
        Assert.Throws<Exception>(() => testenvironment.Get(teststring));;
    }
}