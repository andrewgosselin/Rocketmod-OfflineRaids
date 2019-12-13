using Rocket.API;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace Azurexx.OfflineRaid
{
    public class OfflineRaid : RocketPlugin<OfflineRaidConfiguration>
    {

        protected override void Load()
        {
            Logger.Log("[Azurexx] Offline Raid :: plugin initializing...");

            BarricadeManager.onDamageBarricadeRequested += onBarricadeDamage;
            StructureManager.onDamageStructureRequested += onStructureDamage;
            VehicleManager.onVehicleLockpicked += onVehicleLockpick;
            VehicleManager.onDamageVehicleRequested += onVehicleDamage;
            VehicleManager.onDamageTireRequested += onTireDamage;
            VehicleManager.onVehicleCarjacked += onVehicleCarjack;
            VehicleManager.onSiphonVehicleRequested += onVehicleSiphoning;

            Logger.Log("[Azurexx] Offline Raid :: plugin started!");
        }

        protected override void Unload()
        {
            BarricadeManager.onDamageBarricadeRequested -= onBarricadeDamage;
            StructureManager.onDamageStructureRequested -= onStructureDamage;
            VehicleManager.onVehicleLockpicked -= onVehicleLockpick;
            VehicleManager.onDamageVehicleRequested -= onVehicleDamage;
            VehicleManager.onDamageTireRequested -= onTireDamage;
            VehicleManager.onVehicleCarjacked -= onVehicleCarjack;
            VehicleManager.onSiphonVehicleRequested -= onVehicleSiphoning;

            Logger.Log("[Azurexx] Offline Raid :: plugin unloaded!");
        }

        private void onVehicleCarjack(InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow, ref Vector3 force, ref Vector3 torque)
        {
            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(vehicle.lockedOwner);
            UnturnedPlayer instigator = UnturnedPlayer.FromPlayer(instigatingPlayer);

            if (owner == null || instigator == null)
            {
                allow = true;
            }
            else
            {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    allow = true;
                }
                else
                {
                    if (Configuration.Instance.vehicleCarJack)
                    {
                        allow = true;
                    }
                    else
                    {
                        UnturnedChat.Say(instigator, Configuration.Instance.vehicleCarJackMessage);
                        allow = false;
                    }

                }
            }


        }

        private void onVehicleSiphoning(InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref ushort desiredAmount)
        {
            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(vehicle.lockedOwner);
            UnturnedPlayer instigator = UnturnedPlayer.FromPlayer(instigatingPlayer);
            if (owner == null || instigator == null)
            {
                shouldAllow = true;
            }
            else {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    shouldAllow = true;
                }
                else
                {
                    if (Configuration.Instance.vehicleSiphon)
                    {
                        ushort quotient = (ushort)(desiredAmount / Configuration.Instance.vehicleSiphoningDivision);
                        desiredAmount = quotient;
                        shouldAllow = true;
                    }
                    else
                    {
                        UnturnedChat.Say(instigator, Configuration.Instance.vehicleSiphonMessage);
                        shouldAllow = false;
                    }

                }
            }

        }

        private void onTireDamage(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(vehicle.lockedOwner);
            UnturnedPlayer instigator = UnturnedPlayer.FromCSteamID(instigatorSteamID);

            if (owner == null || instigator == null)
            {
                shouldAllow = true;
            }
            else {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    shouldAllow = true;
                }
                else
                {
                    if (Configuration.Instance.vehicleTireDamage)
                    {
                        shouldAllow = true;
                    }
                    else
                    {
                        UnturnedChat.Say(instigator, Configuration.Instance.vehicleTireDamageMessage);
                        shouldAllow = false;
                    }

                }
            }
        }

        private void onVehicleLockpick(InteractableVehicle vehicle, Player instigatingPlayer, ref bool allow)
        {
            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(vehicle.lockedOwner);
            UnturnedPlayer instigator = UnturnedPlayer.FromPlayer(instigatingPlayer);

            if (owner == null || instigator == null)
            {
                allow = true;
            } else
            {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    allow = true;
                }
                else
                {
                    if (Configuration.Instance.vehicleLockpick)
                    {
                        allow = true;
                    }
                    else
                    {
                        UnturnedChat.Say(instigator, Configuration.Instance.vehicleLockpickMessage);
                        instigator.GiveItem(1353, 1);
                        allow = false;
                    }

                }
            }


        }

        private void onVehicleDamage(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(vehicle.lockedOwner);
            UnturnedPlayer instigator = UnturnedPlayer.FromCSteamID(instigatorSteamID);

            if (owner == null || instigator == null)
            {
                shouldAllow = true;
            }
            else {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    shouldAllow = true;
                }
                else
                {
                    if (Configuration.Instance.vehicleDestroy)
                    {
                        ushort quotient = (ushort)(pendingTotalDamage / Configuration.Instance.vehicleDamageDivision);
                        pendingTotalDamage = quotient;
                        UnturnedChat.Say(instigator, "Damaged at a slower rate (REMOVE THIS MESSAGE AFTER TESTING)");
                        shouldAllow = true;
                    }
                    else
                    {

                        UnturnedChat.Say(instigator, Configuration.Instance.vehicleDamageMessage);
                        shouldAllow = false;
                    }
                }
            }
        }

        private void onStructureDamage(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            byte x;
            byte y;

            ushort index;

            StructureRegion s;

            StructureManager.tryGetInfo(structureTransform, out x, out y, out index, out s);


            var sdata = s.structures[index];


            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(new CSteamID(sdata.owner));
            UnturnedPlayer instigator = UnturnedPlayer.FromCSteamID(instigatorSteamID);

            if (owner == null || instigator == null)
            {
                shouldAllow = true;
            }
            else {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    shouldAllow = true;
                }
                else
                {
                    if (Configuration.Instance.structureDestroy)
                    {
                        ushort quotient = (ushort)(pendingTotalDamage / Configuration.Instance.structureDamageDivision);
                        pendingTotalDamage = quotient;
                        UnturnedChat.Say(instigator, "Damaged at a slower rate (REMOVE THIS MESSAGE AFTER TESTING)");
                        shouldAllow = true;
                    }
                    else
                    {
                        UnturnedChat.Say(instigator, Configuration.Instance.structureDamageMessage);
                        shouldAllow = false;
                    }
                }
            }


        }

        private void onBarricadeDamage(CSteamID instigatorSteamID, Transform barricadeTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            byte x;
            byte y;

            ushort plant;
            ushort index;

            BarricadeRegion r;

            

            BarricadeManager.tryGetInfo(barricadeTransform, out x, out y, out plant, out index, out r);


            var sdata = r.barricades[index];


            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID((CSteamID)sdata.owner);
            UnturnedPlayer instigator = UnturnedPlayer.FromCSteamID(instigatorSteamID);

            if (owner == null || instigator == null)
            {
                shouldAllow = true;
            }
            else {
                bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

                if (isOnline)
                {
                    shouldAllow = true;
                }
                else
                {
                    if (Configuration.Instance.barricadeDestroy)
                    {
                        ushort quotient = (ushort)(pendingTotalDamage / Configuration.Instance.barricadeDamageDivision);
                        pendingTotalDamage = quotient;
                        UnturnedChat.Say(instigator, "Damaged at a slower rate (REMOVE THIS MESSAGE AFTER TESTING)");
                        shouldAllow = true;
                    }
                    else
                    {
                        UnturnedChat.Say(instigator, Configuration.Instance.barricadeDamageMessage);
                        shouldAllow = false;
                    }

                }
            }
        }
    }
}