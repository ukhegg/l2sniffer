using System.Net;
using L2sniffer.GameData;
using L2sniffer.GameState.GameObjects;
using L2sniffer.Packets.DataStructs;
using L2sniffer.Packets.GS;
using PartyInfo = L2sniffer.Packets.DataStructs.PartyInfo;

namespace L2sniffer.GameState;

public class GameSession
{
    private Player _player;
    private IGameObjectsProvider _gameObjectsProvider;
    private IGameObjectsRegistry _objectsRegistry;
    private ISkillInfoProvider _skillInfoProvider;
    private CharacterInfo[] _playerCharacters;

    public L2Shortcut[] Shortcuts { get; private set; }

    public IPEndPoint ServerEndpoint { get; private set; }
    public IPEndPoint ClientEndpoint { get; private set; }

    public GameSession(IPEndPoint serverEndpoint,
                       IPEndPoint clientEndpoint,
                       IGameObjectsProvider gameObjectsProvider,
                       IGameObjectsRegistry objectsRegistry,
                       ISkillInfoProvider skillInfoProvider)
    {
        _gameObjectsProvider = gameObjectsProvider;
        _objectsRegistry = objectsRegistry;
        _skillInfoProvider = skillInfoProvider;
        ServerEndpoint = serverEndpoint;
        ClientEndpoint = clientEndpoint;
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
        var selectedChar = _playerCharacters.First(info => info.Name == selectedCharInfo.Name);
        Player = _gameObjectsProvider.CreatePlayer(selectedChar, selectedCharInfo);
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
            ((OtherPlayer)gameObject).Update(otherUserInfo);
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

    public void ChangeMoveType(GameObjectId charId, MoveType MoveType)
    {
        _objectsRegistry.GetObject(charId).SetMoveType(MoveType);
    }

    public void ApplyMagicEffectsOnPlayer(MagicEffect[] packetMagicEffects, DateTime castTimestamp)
    {
        foreach (var magicEffect in packetMagicEffects)
        {
            var skill = _skillInfoProvider.GetSkillInfo(magicEffect.Skill);
            var playerMagicEffect = new PlayerMagicEffect(skill, castTimestamp, magicEffect.TimeDuration);
            Player.AddMagicEffect(playerMagicEffect);
        }
    }

    public void RegisterChars(CharacterInfo[] playerCharacters)
    {
        _playerCharacters = playerCharacters;
    }

    public void RegisterShortcuts(L2Shortcut[] Shortcuts)
    {
        this.Shortcuts = Shortcuts;
    }


    public void ChangeWaitType(ChangeWaitTypeData packetData)
    {
        _objectsRegistry.GetObject(packetData.ObjectId).ChangeWaitType(packetData.WaitType, packetData.Coordinates);
    }

    public void StopRotation(GameObjectId charId, int Degree)
    {
        _objectsRegistry.GetObject(charId).StopRotation(Degree);
    }

    public void SetPlayerTarget(GameObjectId targetId)
    {
        Player.SetTarget(_objectsRegistry.GetObject(targetId));
    }

    public void OnPartySpell(GameObjectId sourceCharId, MagicEffect[] packetEffects, DateTime castTime)
    {
        var sourceChar = _objectsRegistry.GetObject(sourceCharId);

        foreach (var partyEffect in packetEffects)
        {
            var skill = _skillInfoProvider.GetSkillInfo(partyEffect.Skill);
            var magicEffect = new PlayerMagicEffect(skill, castTime, partyEffect.TimeDuration);

            Player.AddMagicEffect(magicEffect);
            foreach (var partyMember in Player.Party.Members)
            {
                partyMember.AddMagicEffect(magicEffect);
            }
        }
    }

    public void SetPlayerParty(PartyInfo packetInfo)
    {
        var leader = _objectsRegistry.GetObject<HumanPlayer>(packetInfo.Leader);
        Player.Party = new GameObjects.PartyInfo(leader);

        foreach (var participant in packetInfo.Participants)
        {
            Player.Party.AddMember(_objectsRegistry.GetObject<HumanPlayer>(participant.PlayerId));
        }
    }

    public void UpdateParty(PartyMemberInfo packetParticipant)
    {
        Player.Party.Update(packetParticipant);
    }

    public void UpdatePosition(GameObjectId objectId, Coordinates3d position)
    {
        _objectsRegistry.GetObject(objectId).UpdatePosition(position);
    }

    public void ValidatePosition(GameObjectId character, Coordinates3d position, uint heading)
    {
        try
        {
            _objectsRegistry.GetObject(character).UpdatePosition(position, heading);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to validate char {character} position:{e.Message}");
        }
    }

    public void MoveToPawl(GameObjectId charId, GameObjectId targetId, uint distance, Coordinates3d targetCoordinates)
    {
        var movingChar = _objectsRegistry.GetObject(charId);
        var target = _objectsRegistry.GetObject(targetId);
        Console.WriteLine(
            $"    {movingChar.ObjectName} is moving to {target} at {targetCoordinates}, distance={distance}");
    }

    public void CurrentActionFailed()
    {
        Console.WriteLine($"    Palyers current action failed");
    }

    public void TargetSelected(GameObjectId charId, GameObjectId targetId, Coordinates3d targetCoordinates)
    {
        var character = _objectsRegistry.GetObject(charId);
        var target = _objectsRegistry.GetObject(targetId);
        character.SetTarget(target, targetCoordinates);
    }

    public void PlayerTargetUnselected(GameObjectId targetId, Coordinates3d targetCoordinates)
    {
        var target = _objectsRegistry.GetObject(targetId);
        Player.UnselectTarget(target, targetCoordinates);
    }

    public void MagicSkillLaunched(GameObjectId charId, GameObjectId targetId, MagicSkillId skillId, uint failedOrNot)
    {
        var character = _objectsRegistry.GetObject(charId);
        var target = _objectsRegistry.GetObject(targetId);
        var skill = _skillInfoProvider.GetSkillInfo(skillId);
        Console.WriteLine($"    {character.ObjectName} is casting {skill.Name} at {target.ObjectName}");
        int a = 09;
    }

    public void PlayerPartyAdd(PartyMemberInfo newMemberInfo)
    {
        var newMember = _objectsRegistry.GetObject<OtherPlayer>(newMemberInfo.PlayerId);
        newMember.Update(newMemberInfo);
        Player.Party.AddMember(newMember);
    }

    public void HandleMagicSkillUse(MagicSkillUseInfo packetSkillUseInfo, DateTime castTime)
    {
        var caster = _objectsRegistry.GetObject(packetSkillUseInfo.CharId);
        var target = _objectsRegistry.GetObject(packetSkillUseInfo.TargetId);
        var skillInfo = _skillInfoProvider.GetSkillInfo(packetSkillUseInfo.Skill);

        Console.WriteLine($"    {caster.ObjectId} is casting {skillInfo.Name} on {target.ObjectName}");
    }

    public void PlayerStartAutoAttack(GameObjectId targetId)
    {
        var target = _objectsRegistry.GetObject(targetId);
        Player.StartAutoAttack(target);
    }

    public void PlayerStopAutoAttack(GameObjectId targetId)
    {
        var target = _objectsRegistry.GetObject(targetId);
        Player.StopAutoAttack(target);
    }

    public void StopMove(GameObjectId objectId, Coordinates3d coordinates, uint heading)
    {
        var obj = _objectsRegistry.GetObject(objectId);
        obj.StopMove(coordinates, heading);
    }

    public void Die(GameObjectId charId, bool isSweepable)
    {
        var character = _objectsRegistry.GetObject(charId);
        character.SetDead(true);
    }

    public void ItemDropped(GameObjectId playerId,
                            GameObjectId objectId, uint itemId,
                            Coordinates3d coordinates, uint count,
                            uint isStackable)
    {
        var player = _objectsRegistry.GetObject(playerId);
        var item = _gameObjectsProvider.CreateEquipment(objectId, itemId, count);
        _objectsRegistry.RegisterObject(item);

        Console.WriteLine($"    {player.ObjectName} dropped {item} at {coordinates}");
    }

    public void PickupItem(GameObjectId playerId, GameObjectId itemId, Coordinates3d coordinates)
    {
        var player = _objectsRegistry.GetObject(playerId);
        var item = _objectsRegistry.GetObject<Equipment>(itemId);
        Console.WriteLine($"    {player.ObjectName} picked up {item} at {coordinates}");
    }

    public void HandleLogout()
    {
        Console.WriteLine($"    Logging out!");
    }
}