using HarmonyLib;

namespace GreenhousePlus
{
    public class Patch
    {
        static AccessTools.FieldRef<Object_Planter, float> m_minTemperature = AccessTools.FieldRefAccess<Object_Planter, float>("m_minTemperature");
        static AccessTools.FieldRef<Object_Planter, float> m_maxTemperature = AccessTools.FieldRefAccess<Object_Planter, float>("m_maxTemperature");
        
        [HarmonyPatch(typeof(Object_Planter), "PlantSeed")]
        [HarmonyPostfix]
        public static void PlantSeed_Postfix(Object_Planter __instance, ObjectManager.ObjectType  ___m_objectType)
        {
            float minTemperatureMod = 0;
            float maxTemperatureMod = 0;
            switch (___m_objectType)
            {
                case ObjectManager.ObjectType.Greenhouse:
                    minTemperatureMod = -5;
                    maxTemperatureMod = 5;
                    break;
                case ObjectManager.ObjectType.LargeGreenhouse:
                    minTemperatureMod = -10;
                    maxTemperatureMod = 10;
                    break;
                case ObjectManager.ObjectType.HugeGreenhouse:
                    minTemperatureMod = -15;
                    maxTemperatureMod = 15;
                    break;
            }
            m_minTemperature(__instance) += minTemperatureMod;
            m_maxTemperature(__instance) += maxTemperatureMod;
        }
    }
}