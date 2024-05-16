namespace P4.Interpreter;

public class Environment
{
    private readonly SymbolTable<string, int> _locationTable;
    private readonly SymbolTable<int, object> _valueTable;
    private int _nextLocation;

    public Environment()
    {
        _locationTable = new SymbolTable<string, int>();
        _valueTable = new SymbolTable<int, object>();
        _nextLocation = 0;
    }

    private Environment(SymbolTable<string, int> locationTable, SymbolTable<int, object> valueTable, int nextLocation)
    {
        _locationTable = locationTable;
        _valueTable = valueTable;
        _nextLocation = nextLocation;
    }
    
    public void PushScope()
    {
        _locationTable.PushScope();
        _valueTable.PushScope();
    }

    public void PopScope()
    {
        _locationTable.PopScope();
        _valueTable.PopScope();
    }
    public Environment Copy()
    {
        return new Environment(_locationTable.Copy(), _valueTable.Copy(), _nextLocation);
    }   
    public void DeclareVariable(string name, object value)
    {
        int location = _nextLocation++;
        _locationTable.Add(name, location);
        _valueTable.Add(location, value);
    }
    public object Get(string name)
    {
        int location = _locationTable.Get(name);
        return _valueTable.Get(location);
    }
    public void Set(string name, object value)
    {
        int location = _locationTable.Get(name);
        _valueTable.Set(location, value);
    }
}
