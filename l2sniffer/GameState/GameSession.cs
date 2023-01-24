using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;

namespace L2sniffer.GameState;

public class GameSession
{
    private Player _player;
    private IGameObjectsProvider _gameObjectsProvider;
    private IGameObjectsRegistry _objectsRegistry;

    public GameSession(IGameObjectsProvider gameObjectsProvider,
                       IGameObjectsRegistry objectsRegistry)
    {
        _gameObjectsProvider = gameObjectsProvider;
        _objectsRegistry = objectsRegistry;
    }

    public Player Player
    {
        get => _player;
        private set => _player = value;
    }

    public void OnCharSelected(SelectedCharInfo selectedCharInfo)
    {
        Console.WriteLine(
            $"char selected:{selectedCharInfo.Name}, lvl {selectedCharInfo.Level}, class {selectedCharInfo.Class}");
        Player = _gameObjectsProvider.CreatePlayer(selectedCharInfo);
        _objectsRegistry.RegisterObject(Player);
    }

    public void UpdateCharacter(MorphedCharacterInfo characterInfo)
    {
        if (_objectsRegistry.TryGetObject(characterInfo.ObjectId, out var gameObject))
        {
            gameObject.Update(characterInfo);
            return;
        }

        _objectsRegistry.RegisterObject(characterInfo.IsNpcInfo()
                                            ? _gameObjectsProvider.CreateNpc(characterInfo)
                                            : _gameObjectsProvider.CreateMorphed(characterInfo));
    }

    public void UpdateCharacter(OtherUserInfo otherUserInfo)
    {
        if (_objectsRegistry.TryGetObject(otherUserInfo.ObjectId, out var gameObject))
        {
            gameObject.IfOtherPlayer(player => player.Update(otherUserInfo));
            return;
        }

        var otherPlayer = _gameObjectsProvider.CreateOtherPlayer(otherUserInfo);
        Console.WriteLine(
            $"    Found new player {otherPlayer.Name} at {otherUserInfo.Coordinates}, race {otherPlayer.PlayerInfo.Race}, class {otherUserInfo.ClassOrBaseClassId}");

        _objectsRegistry.RegisterObject(otherPlayer);
    }

    public void HandleReplica(CreatureReplica replica)
    {
        Console.WriteLine($"    {replica.ObjectId} say to {replica.CharName}:{replica.Text}");
    }

    public void MoveToLocation(GameObjectId objectId, Coordinates3d current, Coordinates3d dst)
    {
        if (!_objectsRegistry.TryGetObject(objectId, out var gameObject))
        {
            Console.WriteLine($"unknwon object id #{objectId}");
            return;
        }

        gameObject.MoveToLocation(current, dst);
    }

    public void UpdateStatus(CharStatusUpdate statusUpdates)
    {
        if (!_objectsRegistry.TryGetObject(statusUpdates.ObjectId, out var go))
        {
            Console.WriteLine($"unknwon object ID {statusUpdates.ObjectId}");
            return;
        }

        go.Update(statusUpdates);
    }

    public void UpdateUserInfo(UserInfo packetInfo)
    {
        Player.Update(packetInfo);
        _objectsRegistry.RegisterObject(packetInfo.BaseInfo.ObjectId, Player);
    }

    public void HandleAction(GameObjectId playerId, uint actionId)
    {
        if (!_objectsRegistry.TryGetObject(playerId, out var go))
        {
            Console.WriteLine($"    unknown player id {playerId}");
            return;
        }

        go.HandleAction(actionId);
    }

    public void HandleAttack(AttackInfo packetAttackInfo)
    {
        if (!_objectsRegistry.TryGetObject(packetAttackInfo.AttackerId, out var go))
        {
            Console.WriteLine($"    unknown player id {packetAttackInfo.AttackerId}");
            return;
        }

        go.HandleAttack(packetAttackInfo);
    }

    public void DeleteObject(GameObjectId packetObjectId)
    {
        _objectsRegistry.DeleteObject(packetObjectId);
    }
}