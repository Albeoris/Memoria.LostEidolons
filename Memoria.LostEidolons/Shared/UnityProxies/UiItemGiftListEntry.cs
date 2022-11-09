using System;
using System.Linq;
using Memoria.LostEidolons.BeepInEx;
using UnityEngine;
using UnityEngine.UI;
using US.Client;
using US.DataStructure;

// ReSharper disable Unity.NoNullCoalescing

namespace Memoria.LostEidolons.UnityProxies;

public sealed class UiItemGiftListEntry
{
    public UITemplateData_ListDataItemGift Gift { get; }
    public PDUnit TargetUnit { get; }
    public Text Name { get; }

    private UiItemGiftListEntry(UITemplateData_ListDataItemGift gift, PDUnit targetUnit, Text name)
    {
        Gift = gift ?? throw new ArgumentNullException(nameof(gift));
        TargetUnit = targetUnit ?? throw new ArgumentNullException(nameof(gift));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Boolean IsPreferred()
    {
        PDItemGiftType[] preferredTypes = TargetUnit.PreferredItemGiftType.ToManaged();
        PDItemGiftType thisType = Gift.ItemGift.PDItemGift.PDItemGiftType;

        return preferredTypes.Any(type => type.ID == thisType.ID);
    }

    public static UiItemGiftListEntry Create(Transform child)
    {
        UIButtonMultiBinder binder = child.GetComponent<UIButtonMultiBinder>() ?? throw new ArgumentException($"Failed to get binder of {nameof(UiItemGiftListEntry)}");
        UIData data = binder.UIDataProxy ?? throw new ArgumentException($"Failed to get UIDataProxy of {nameof(UiItemGiftListEntry)}");
        UITemplateData_ListDataItemGift itemGift = data.TryCast<UITemplateData_ListDataItemGift>();
        PDUnit targetUnit = itemGift.Target.TargetUnit.PDUnit;

        Text itemName = child.transform.FindChild("Text_Name").GetComponent<Text>();
        
        return new UiItemGiftListEntry(itemGift, targetUnit, itemName);
    }
}