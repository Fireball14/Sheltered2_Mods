using HarmonyLib;

namespace LightningRodProtection
{
    public class Patch
    {
        [HarmonyPatch(typeof(Object_Powered), "OnLightningStrike")]
        [HarmonyPrefix]
        public static bool OnLightningStrike_Prefix(ref bool __result)
        {
            if (ObjectManager.instance.GetObjectsOfType(ObjectManager.ObjectType.LightningRod).Count > 0)
            {
                __result =  false;
            }
            __result = true;
            return false;
        }
    }
}