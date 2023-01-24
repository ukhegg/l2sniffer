using System.Reflection;

namespace L2sniffer.GameData;

public class ItemNameProvider : IItemNameProvider
{
    private IDictionary<uint, ItemName> _itemNames;

    public ItemNameProvider(string npcNameDatFile)
    {
        _itemNames = new Dictionary<uint, ItemName>();

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
                var itemName = new ItemName()
                {
                    Id = uint.Parse(parts[0]),
                    Name = parts[1],
                    AddName = parts[2],
                    Description = parts[3]
                };
                _itemNames[itemName.Id] = itemName;
            }
        }
    }

    public ItemName GetItem(uint id)
    {
        return _itemNames[id];
    }
}