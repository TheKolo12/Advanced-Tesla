using Exiled.API.Enums;
using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace AdvancedTesla
{
    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("Configs of Advance Tesla")]
        public bool AdvanceTesla { get; set; } = true;
        public bool CrazyTesla { get; set; } = true;
        public float CrazyTeslaInterval { get; set; } = UnityEngine.Random.Range(350, 700);
        public string CassieContent { get; set; } = "Warning, Tesla Gates have been hacked , Move away from them immediately";
        public string CassieMessage { get; set; } = "Warning, Tesla Gates have been hacked , Move away from them immediately";

        public List<ItemType> RequiredItems { get; set; } = new List<ItemType>()
        {
            ItemType.KeycardMTFPrivate,
            ItemType.KeycardContainmentEngineer,
            ItemType.KeycardMTFOperative,
            ItemType.KeycardMTFCaptain,
            ItemType.KeycardContainmentEngineer,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardO5
        };
        public string Hint { get; set; } = $"You have been recognized as %Role%, The tesla has been deactivated";
    }
}
