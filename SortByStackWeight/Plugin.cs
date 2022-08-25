using BepInEx;
using HarmonyLib;

namespace SortByStackWeight
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Sheltered2.exe")]

    public class Plugin : BaseUnityPlugin
    {
        public void Awake()
        {
            Harmony harmony = new (PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(Patch));
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}
