using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;

using System.Security.Permissions;
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace MoooreSlugcatsRemix
{
    
    [BepInPlugin(MOD_ID, "Mooore Slugcats Remix", "1.0.1")]
    public class MoooreSlugcatsRemix : BaseUnityPlugin
    {
        public const string MOD_ID = "stupidcat.moooreslugcatsremix";
        public static MoooreSlugcatsRemix INSTANCE;

        public void OnEnable()
        {
            INSTANCE = this;

            On.RainWorld.OnModsInit += (orig, self) =>
            {
                orig(self);

                MachineConnector.SetRegisteredOI(MOD_ID, new MSOptions());

                On.Room.Update += RoomOnUpdate;
                On.Player.NewRoom += PlayerOnNewRoom;
            };
        }

        private void PlayerOnNewRoom(On.Player.orig_NewRoom orig, Player self, Room newroom)
        {
            if (currentRoom != newroom.abstractRoom.index)
            {
                previousRoom = currentRoom;
                currentRoom = newroom.abstractRoom.index;
            }
        }

        private bool pressed = false;
        private int previousRoom = -1;
        private int currentRoom = -1;

        private void RoomOnUpdate(On.Room.orig_Update orig, Room self)
        {
            orig(self);
            
            if ((Input.GetKey(MSOptions.SPAWN_KEY.Value) && !pressed) 
                || (Input.GetKey(MSOptions.SPAWN_KEY.Value) && Input.GetKey(MSOptions.MULTI_SPAWN_MODIFIER.Value))
                || (Input.GetKey(MSOptions.MULTI_SPAWN_KEY.Value)))
            {
                pressed = true;
                if (!(self.game.IsStorySession && self.abstractRoom.connections.Length != 0)
                    && !(self.game.IsArenaSession && self.exitAndDenIndex.Length != 0))
                {
                    return;
                }

                var existingState = self.world.game.session.Players[0].state as PlayerState;


                var room = existingState.creature.Room;

                var template = StaticWorld.GetCreatureTemplate("Slugcat");
                var entityID = new EntityID(-1, 1);

                Logger.LogInfo("player pos: " + existingState.creature.pos.abstractNode);
                Logger.LogInfo("previous room: " + previousRoom);
                Logger.LogInfo("available rooms: " + string.Join(" ", room.connections));

                var coords = self.game.IsStorySession 
                    ? new WorldCoordinate(room.connections[0], 0, 0, 0) 
                    : new WorldCoordinate(room.index, self.exitAndDenIndex[0].x, self.exitAndDenIndex[0].y, 0);

                var creatura = new AbstractCreature(self.world, template, (Creature) null, coords, entityID);


                creatura.state = (CreatureState) new PlayerState(creatura, 0, existingState.slugcatCharacter, false);

                var abstractCreatura = (AbstractWorldEntity) (object) creatura;

                Logger.LogInfo(existingState.creature.Room.index);

                Logger.LogInfo(room.index);

                if (self.game.IsStorySession)
                {
                    ((AbstractPhysicalObject)creatura).ChangeRooms(new WorldCoordinate(room.index, 0, 0, existingState.creature.pos.abstractNode));
                }
                else
                {
                    room.MoveEntityToDen(abstractCreatura);
                    room.MoveEntityOutOfDen(abstractCreatura);
                }
            }

            if (!Input.GetKey(KeyCode.L) && pressed)
            {
                pressed = false;
            }
        }
    }
}
