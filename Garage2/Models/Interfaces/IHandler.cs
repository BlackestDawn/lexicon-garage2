using System.Collections;

namespace Garage2.Models.Interfaces;

public interface IHandler
{
    public Func<Hashtable> GetUsageStats();
    public void Add();
    public void Remove();
    public bool TryAdd();
    public bool TryRemove();
}
