using System.Reflection;

namespace L2sniffer.GameData;

public class ActionName
{
    public uint Tag;
    public uint Id;
    public int Type;
    public int Category;
    public int[] C;
    public string Cmd;
    public string Icon;
    public string Name;
    public string Description;
}

public class ActionNameProvider : IActionNameProvider
{
    private IDictionary<uint, ActionName> _actions;

    public ActionNameProvider(string datFileName)
    {
        _actions = new Dictionary<uint, ActionName>();
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "L2sniffer.GameData.Data." + datFileName;
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (var reader = new StreamReader(stream))
        {
            var isFirst = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (isFirst)
                {
                    isFirst = false;
                    continue; 
                }

                var parts = line.Split('\t');
                var cat2Count = int.Parse(parts[4]);
                var itemName = new ActionName()
                {
                    Tag = uint.Parse(parts[0]),
                    Id = uint.Parse(parts[1]),
                    Type = int.Parse(parts[2]),
                    Category = int.Parse(parts[3]),
                    C = new int[cat2Count],
                    Cmd = parts[^1],
                    Icon = parts[^3],
                    Name = parts[^4],
                    Description = parts[^2]
                };
                for (var i = 0; i < itemName.C.Length; ++i)
                {
                    if (parts[i + 5] == string.Empty) continue;
                    else itemName.C[i] = int.Parse(parts[i + 5]);
                }

                _actions[itemName.Id] = itemName;
            }
        }
    }

    public ActionName GetActionName(uint id)
    {
        return _actions[id];
    }

    public bool TrygetActionName(uint id, out ActionName result)
    {
        return _actions.TryGetValue(id, out result);
    }
}