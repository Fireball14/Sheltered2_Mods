using System.IO;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace RespecButton
{
    public class Patch
    {
        public static Button respecButton;
        
        [HarmonyPatch(typeof(SkillsPanel), "OnOpen")]
        [HarmonyPostfix]
        public static void OnOpen_Postfix(SkillsPanel __instance, Button ___m_purchaseButton)
        {
            if (___m_purchaseButton.transform.parent.GetComponentsInChildren<Text>().Any(textComponent => textComponent.text == "Respec") == true)
            {
                return;
            }
            respecButton = Object.Instantiate(___m_purchaseButton, ___m_purchaseButton.transform.parent);
            respecButton.interactable = true;
            respecButton.gameObject.SetActive(true);
            respecButton.GetComponentInChildren<Text>().text = "Respec";
            respecButton.onClick = new Button.ButtonClickedEvent();
            respecButton.onClick.AddListener(RespecSelectedCharacter);
        }
        
        static AccessTools.FieldRef<BaseStat, int> m_pointsSpent = AccessTools.FieldRefAccess<BaseStat, int>("m_pointsSpent");

        private static void RespecSelectedCharacter()
        {
            Member selectedMember = InteractionManager.instance.SelectedMember.member;
            selectedMember.baseStats.RefundSkillPoints(selectedMember.baseStats.Charisma.PointsSpent, BaseStats.StatType.Charisma);
            selectedMember.baseStats.RefundSkillPoints(selectedMember.baseStats.Dexterity.PointsSpent, BaseStats.StatType.Dexterity);
            selectedMember.baseStats.RefundSkillPoints(selectedMember.baseStats.Fortitude.PointsSpent, BaseStats.StatType.Fortitude);
            selectedMember.baseStats.RefundSkillPoints(selectedMember.baseStats.Intelligence.PointsSpent, BaseStats.StatType.Intelligence);
            selectedMember.baseStats.RefundSkillPoints(selectedMember.baseStats.Perception.PointsSpent, BaseStats.StatType.Perception);
            selectedMember.baseStats.RefundSkillPoints(selectedMember.baseStats.Strength.PointsSpent, BaseStats.StatType.Strength);
            m_pointsSpent(selectedMember.baseStats.Charisma) = 0;
            m_pointsSpent(selectedMember.baseStats.Dexterity) = 0;
            m_pointsSpent(selectedMember.baseStats.Fortitude) = 0;
            m_pointsSpent(selectedMember.baseStats.Intelligence) = 0;
            m_pointsSpent(selectedMember.baseStats.Perception) = 0;
            m_pointsSpent(selectedMember.baseStats.Strength) = 0;
            selectedMember.SetupProfession(new Profession(selectedMember.memberRH.baseCharacter));
            SkillsPanel.Instance.Close();
        }
    }
}