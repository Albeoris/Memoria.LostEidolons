using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Memoria.LostEidolons.BeepInEx;
using Memoria.LostEidolons.Configuration;
using Memoria.LostEidolons.IL2CPP;
using Memoria.LostEidolons.UnityProxies;
using UnhollowerBaseLib;
using UnityEngine;
using US.Client;
using US.DataStructure;
using Object = System.Object;

// ReSharper disable Unity.NoNullPropagation

namespace Memoria.LostEidolons.HarmonyPatches;

// ReSharper disable InconsistentNaming
[HarmonyPatch(typeof(Proxy_CampItemGift), nameof(Proxy_CampItemGift.CreateItemGifts))]
public static class Proxy_CampItemGift_CreateItemGifts
{
    public static void Postfix(Proxy_CampItemGift __instance, Boolean __result)
    {
        try
        {
            if (!__result)
                return;

            CampActivitiesGiftsConfiguration gifts = ModComponent.Instance.Config.CampActivitiesGifts;
            Color? preferredGiftsColor = gifts.ColorizePreferredGifts ? gifts.PreferredGiftsColor : null;
            Color? commonGiftsColor = gifts.ColorizeCommonGifts ? gifts.CommonGiftsColor : null;
            if (preferredGiftsColor is null && commonGiftsColor is null)
                return;

            UiItemGiftList giftList = UiItemGiftList.FindSingleton();
            if (giftList is null)
                return;

            ColorizeGiftItems(giftList, preferredGiftsColor, commonGiftsColor);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }

    private static void ColorizeGiftItems(UiItemGiftList giftList, Color? preferredGiftsColor, Color? commonGiftsColor)
    {
        HashSet<String> preferredTypeIds = null;
        foreach (UiItemGiftListEntry entry in giftList.EnumerateEntries())
        {
            preferredTypeIds ??= entry.TargetUnit.PreferredItemGiftType.ToManaged().Select(type => type.ID).ToHashSet();

            if (preferredTypeIds.Contains(entry.Gift.ItemGift.PDItemGift.PDItemGiftType.ID))
            {
                if (preferredGiftsColor is not null)
                    entry.Name.color = preferredGiftsColor.Value;
            }
            else if (commonGiftsColor is not null)
            {
                entry.Name.color = commonGiftsColor.Value;
            }
        }
    }
}