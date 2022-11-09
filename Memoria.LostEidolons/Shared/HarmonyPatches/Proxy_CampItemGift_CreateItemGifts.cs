using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Memoria.LostEidolons.BeepInEx;
using Memoria.LostEidolons.Configuration;
using Memoria.LostEidolons.IL2CPP;
using Memoria.LostEidolons.UnityProxies;
using TriangleNet;
using UnhollowerBaseLib;
using UnityEngine;
using US.Client;
using US.DataStructure;
using Object = System.Object;

// ReSharper disable Unity.NoNullPropagation

namespace Memoria.LostEidolons.HarmonyPatches;

[HarmonyPatch(typeof(UIGOEnableDisableBinder), nameof(UIGOEnableDisableBinder.OnEnable))]
public static class UIGOEnableDisableBinder_OnEnable
{
    public static void Postfix(UIGOEnableDisableBinder __instance)
    {
        try
        {
            if (__instance.name != "ListDataItemGift(Clone)")
                return;
            
            CampActivitiesGiftsConfiguration gifts = ModComponent.Instance.Config.CampActivitiesGifts;
            Color? preferredGiftsColor = gifts.ColorizePreferredGifts ? gifts.PreferredGiftsColor : null;
            Color? commonGiftsColor = gifts.ColorizeCommonGifts ? gifts.CommonGiftsColor : null;
            if (preferredGiftsColor is null && commonGiftsColor is null)
                return;

            UiItemGiftListEntry entry = UiItemGiftListEntry.TryCreate(__instance.gameObject);
            ColorizeGiftItem(entry, preferredGiftsColor, commonGiftsColor);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }

    private static void ColorizeGiftItem(UiItemGiftListEntry entry, Color? preferredGiftsColor, Color? commonGiftsColor)
    {
        if (entry.IsPreferred())
        {
            if (preferredGiftsColor is not null)
                entry.Name.color = preferredGiftsColor.Value;
        }
        else if (commonGiftsColor is not null)
        {
            entry.Name.color = commonGiftsColor.Value;
        }
    }

    // private static void ColorizeGiftItems(UiItemGiftList giftList, Color? preferredGiftsColor, Color? commonGiftsColor)
    // {
    //     HashSet<String> preferredTypeIds = null;
    //     foreach (UiItemGiftListEntry entry in giftList.EnumerateEntries())
    //     {
    //         if (preferredTypeIds == null)
    //         {
    //             preferredTypeIds = entry.TargetUnit.PreferredItemGiftType.ToManaged().Select(type => type.ID).ToHashSet();
    //             ModComponent.Log.LogMessage($"{entry.TargetUnit.ID}'s preferred gifsts: {String.Join(", ", preferredTypeIds)}");
    //         }
    //
    //         if (preferredTypeIds.Contains(entry.Gift.ItemGift.PDItemGift.PDItemGiftType.ID))
    //         {
    //             ModComponent.Log.LogMessage($"{entry.Gift.ItemGift.PDItemGift.PDItemGiftType.ID} is preferred");
    //             if (preferredGiftsColor is not null)
    //                 entry.Name.color = preferredGiftsColor.Value;
    //         }
    //         else if (commonGiftsColor is not null)
    //         {
    //             ModComponent.Log.LogMessage($"{entry.Gift.ItemGift.PDItemGift.PDItemGiftType.ID} is not");
    //             entry.Name.color = commonGiftsColor.Value;
    //         }
    //     }
    // }
}