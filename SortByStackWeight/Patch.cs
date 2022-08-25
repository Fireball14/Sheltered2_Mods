using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.UI;

namespace SortByStackWeight
{
    public class Patch
    {
        private static bool _sortByStackWeightEnabled = true; 
        
        [HarmonyPatch(typeof(ItemGrid), nameof(ItemGrid.SortByCurrentSortType))]
        [HarmonyPrefix]
        public static bool SortByCurrentSortType_Prefix(List<ItemStack> ___m_displayItems)
        {
            if (_sortByStackWeightEnabled == true)
            {
                ___m_displayItems.Sort((a, b)
                    => (b.def.weight*b.amount).CompareTo(a.def.weight*a.amount));
                return false;    
            }
            return true;
        }
        
        [HarmonyPatch(typeof(ItemGrid), nameof(ItemGrid.SetUpSortOptions))]
        [HarmonyPostfix]
        public static void SetUpSortOptions_Postfix(bool initial, Dropdown ___m_sortDropDown)
        {
            if (initial == true)
            {
                _sortByStackWeightEnabled = false;
            }
            ___m_sortDropDown.AddOptions(new List<string> { "Stack Weight" });
            if (_sortByStackWeightEnabled == true)
            {
                ___m_sortDropDown.SetValueWithoutNotify(___m_sortDropDown.options.Count-1);
            }
        }
        
        [HarmonyPatch(typeof(ItemGrid), nameof(ItemGrid.OnSortSelected))]
        [HarmonyPrefix]
        public static bool OnSortSelected_Prefix(Dropdown ___m_sortDropDown, List<string> ___m_sortOptions,
            ItemGrid.FilterChangeCallback ___m_filterChangeCallback, ItemGrid __instance)
        {
            if (___m_sortDropDown.value == ___m_sortOptions.Count)
            {
                _sortByStackWeightEnabled = true;
                __instance.SnapGridScrollToTop();
                __instance.RefreshGrid();
                ___m_filterChangeCallback?.Invoke();
                return false;
            }
            _sortByStackWeightEnabled = false;
            return true;
        }
    }
}