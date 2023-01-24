namespace L2sniffer.GameData;

public interface IActionNameProvider
{
    ActionName GetActionName(uint id);

    bool TrygetActionName(uint id, out ActionName result);
}