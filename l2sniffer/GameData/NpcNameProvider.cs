using System.Reflection;

namespace L2sniffer.GameData;

public class NpcNameProvider : INpcNameProvider
{
    private IDictionary<uint, NpcName> _npcNames;

    public NpcNameProvider(string npcNameDatFile)
    {
        _npcNames = new Dictionary<uint, NpcName>();

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "L2sniffer.GameData.Data." + npcNameDatFile;
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
                var npcName = new NpcName()
                {
                    Id = uint.Parse(parts[0]),
                    Name = parts[1].Substring(2, parts[1].Length - 4),
                    Description = parts[2]
                };
                _npcNames[npcName.Id] = npcName;
            }
        }
    }

    public bool TryGetNpcName(uint id, out NpcName name)
    {
        return _npcNames.TryGetValue(id, out name);
    }

    public NpcName GetNpcName(uint id)
    {
        return _npcNames[id];
    }
}