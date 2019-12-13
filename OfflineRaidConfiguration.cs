using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Azurexx.OfflineRaid
{
    public class OfflineRaidConfiguration : IRocketPluginConfiguration
    {
        public bool structureDestroy = false;
        public bool barricadeDestroy = false;
        public bool vehicleDestroy = false;
        public bool vehicleLockpick = false;
        public bool vehicleTireDamage = false;
        public bool vehicleSiphon = false;
        public bool vehicleCarJack = false;

        public int structureDamageDivision = 3;
        public int barricadeDamageDivision = 3;
        public int vehicleDamageDivision = 3;
        public int vehicleSiphoningDivision = 3;

        public string structureDamageMessage = "";
        public string barricadeDamageMessage = "";
        public string vehicleDamageMessage = "";
        public string vehicleLockpickMessage = "";
        public string vehicleTireDamageMessage = "";
        public string vehicleSiphonMessage = "";
        public string vehicleCarJackMessage = "";

        public void LoadDefaults()
        {
            structureDestroy = false;
            barricadeDestroy = false;
            vehicleDestroy = false;
            vehicleLockpick = false;
            vehicleTireDamage = false;
            vehicleSiphon = false;
            vehicleCarJack = false;

            structureDamageDivision = 1;
            barricadeDamageDivision = 1;
            vehicleDamageDivision = 1;
            vehicleSiphoningDivision = 1;

            structureDamageMessage = "The owner of this structure is offline, you cannot damage this.";
            barricadeDamageMessage = "The owner of this barricade is offline, you cannot damage this.";
            vehicleDamageMessage = "The owner of this vehicle is offline, you cannot damage thise.";
            vehicleLockpickMessage = "The owner of this vehicle is offline, you cannot lockpick.";
            vehicleTireDamageMessage = "The owner of this vehicle is offline, you cannot damage this.";
            vehicleSiphonMessage = "The owner of this vehicle is offline, you cannot siphon this.";
            vehicleCarJackMessage = "The owner of this vehicle is offline, you cannot car jack this.";
        }
    }
}
