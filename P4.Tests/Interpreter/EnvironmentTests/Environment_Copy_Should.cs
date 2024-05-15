using Assert = Xunit.Assert;
using Environment = P4.Interpreter.Environment;
namespace P4.Tests.Interpreter.EnvironmentTests;

public class Environment_Copy_Should
{
    public Environment testenvironment = new ();
    
    [Fact]
    public void Environment_CopyNotEqual_Success()
    { 
        // Arrange
        
        // Act
        var copiedenvironment = testenvironment.Copy();

        // Assert
        Assert.NotEqual(copiedenvironment,testenvironment);
    }
    [Fact]
    public void Environment_CopyValueEqual_Success()
    { 
        // Arrange
        string testvariablename = "testvariablename";
        object testvalue = 42;
        
        // Act
        testenvironment.DeclareVariable(testvariablename);
        testenvironment.Add(testvariablename,testvalue);
        var copiedenvironment = testenvironment.Copy();
        var copiedvalue = copiedenvironment.Get(testvariablename);
        
        // Assert
        Assert.Equal(copiedvalue,testvalue);
    }
    
    [Fact]
    public void Environment_CopyWithDeclareInCopiedEnvironment_Success()
    { 
        // Arrange
        string testvariablename = "testvariablename";
        string testvariablename2 = "testvariablename2";
        object testvalue = 42;
        
        // Act
        testenvironment.DeclareVariable(testvariablename);
        testenvironment.Add(testvariablename,testvalue);
        
        var copiedenvironment = testenvironment.Copy();
        copiedenvironment.DeclareVariable(testvariablename2);
        var copiedvalue = copiedenvironment.Get(testvariablename2);
        
        // Assert
        Assert.Equal(copiedvalue,null);
    }
    
    [Fact]
    public void Environment_CopyWithDeclareInCopiedEnvironmentNotFoundInTestEnvironment_Throws()
    { 
        // Arrange
        string testvariablename = "testvariablename";
        string testvariablename2 = "testvariablename2";
        object testvalue = 42;
        
        // Act
        testenvironment.DeclareVariable(testvariablename);
        testenvironment.Add(testvariablename,testvalue);
        
        var copiedenvironment = testenvironment.Copy();
        copiedenvironment.DeclareVariable(testvariablename2);
        
        // Assert
        Assert.Throws<Exception>(() => testenvironment.Get(testvariablename2));
    }
}