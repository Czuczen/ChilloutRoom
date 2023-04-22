using System.Threading.Tasks;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus;

public static class BonusHubHelper
{
    private const string Pull = "Pull";
    private const string Put = "Put";
        
    
    public static async Task ProcessArtifact(BonusActivation bonusActivation, ExtendingModels.Models.Products.Bonus bonus, 
        PlantationStorage playerPlantationStorage, string artifactAction, IIgnoreChangeService ignoreChangeService)
    {
        if (artifactAction == Pull && bonus.IsActive)
        {
            if (playerPlantationStorage.Gold >= bonus.ArtifactPullCost)
            {
                playerPlantationStorage.Gold -= (decimal) bonus.ArtifactPullCost;
                bonus.IsActive = false;
                bonus.OwnedAmount++;
                await ignoreChangeService.Add(bonus);
                playerPlantationStorage.ArtifactSlotsInUse--;
                bonusActivation.SuccessfulActivation = true;
                bonusActivation.InfoMessage.Add("Artefakt został wyciągnięty");
                bonusActivation.InfoMessage.Add("Koszt: " + bonus.ArtifactPullCost + "$");
            }
            else
            {
                bonusActivation.SuccessfulActivation = false;
                bonusActivation.InfoMessage.Add("Za mało $");
            }
        }
        else if (artifactAction == Pull && !bonus.IsActive)
        {
            bonusActivation.SuccessfulActivation = false;
            bonusActivation.InfoMessage.Add("Artefakt został już wyciągnięty");
        }

        if (artifactAction == Put && !bonus.IsActive)
        {
            if (bonus.OwnedAmount < 1)
            {
                bonusActivation.SuccessfulActivation = false;
                bonusActivation.InfoMessage.Add("Nie posiadasz tego artefaktu");
            }
            else if (playerPlantationStorage.ArtifactSlotsInUse < playerPlantationStorage.UnlockedArtifactSlots)
            {
                if (playerPlantationStorage.Gold >= bonus.ArtifactPutCost)
                {
                    playerPlantationStorage.Gold -= (decimal) bonus.ArtifactPutCost;
                    bonus.IsActive = true;
                    bonus.OwnedAmount--;
                    await ignoreChangeService.Add(bonus);

                    playerPlantationStorage.ArtifactSlotsInUse++;
                    bonus.Usages++;
                    bonusActivation.SuccessfulActivation = true;
                    bonusActivation.InfoMessage.Add("Artefakt został włożony");
                    bonusActivation.InfoMessage.Add("Koszt: " + bonus.ArtifactPutCost + "$");
                }
                else
                {
                    bonusActivation.SuccessfulActivation = false;
                    bonusActivation.InfoMessage.Add("Za mało $");
                }
            }
            else
            {
                bonusActivation.SuccessfulActivation = false;
                bonusActivation.InfoMessage.Add("Dostępne sloty wykorzystane");
            }
        }
        else if (artifactAction == Put && bonus.IsActive)
        {
            bonusActivation.SuccessfulActivation = false;
            bonusActivation.InfoMessage.Add("Artefakt został już włożony");
        }
    }

    public static async Task ProcessBuff(BonusActivation bonusActivation, ExtendingModels.Models.Products.Bonus bonus, 
        PlantationStorage playerPlantationStorage, IIgnoreChangeService ignoreChangeService)
    {
        if (bonus.OwnedAmount < 1)
        {
            bonusActivation.SuccessfulActivation = false;
            bonusActivation.InfoMessage.Add("Nie posiadasz tego wzmocnienia");
        }
        else if (bonus.IsActive && (bool) !bonus.IsStackable)
        {
            bonusActivation.SuccessfulActivation = false;
            bonusActivation.InfoMessage.Add("Wzmocnienie zostało już aktywowane");
        }
        else if (bonus.IsActive && (bool) bonus.IsStackable)
        {
            bonus.RemainingActiveTime += bonus.ActiveTimePerUse;
            bonus.OwnedAmount--;
            await ignoreChangeService.Add(bonus);
            bonus.Usages++;
            bonusActivation.SuccessfulActivation = true;
            bonusActivation.InfoMessage.Add("Wzmocnienie zostało dodane");  
        }
        else if (!bonus.IsActive)
        {
            if (playerPlantationStorage.BuffSlotsInUse < playerPlantationStorage.UnlockedBuffsSlots)
            {
                bonus.IsActive = true;
                bonus.RemainingActiveTime = bonus.ActiveTimePerUse;
                bonus.OwnedAmount--;
                await ignoreChangeService.Add(bonus);
                bonus.Usages++;

                playerPlantationStorage.BuffSlotsInUse++;
                bonusActivation.SuccessfulActivation = true;
                bonusActivation.InfoMessage.Add("Wzmocnienie zostało aktywowane");    
            }
            else
            {
                bonusActivation.SuccessfulActivation = false;
                bonusActivation.InfoMessage.Add("Dostępne sloty wykorzystane");
            }
        }
        else if (bonus.IsActive)
        {    
            bonusActivation.SuccessfulActivation = false;
            bonusActivation.InfoMessage.Add("Wzmocnienie zostało już aktywowane");
        }
    }
}