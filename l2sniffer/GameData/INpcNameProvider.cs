namespace L2sniffer.GameData;

public interface INpcNameProvider
{
    NpcName GetNpcName(uint id);

    bool TryGetNpcName(uint id, out NpcName name);
}