using System;
using System.Collections.Generic;
using UnityEngine;

namespace Memoria.LostEidolons.UnityProxies;

public sealed class UiItemGiftList
{
    private readonly Transform _root;

    private UiItemGiftList(Transform root)
    {
        _root = root ? root : throw new ArgumentNullException(nameof(root));
    }

    public static UiItemGiftList FindSingleton()
    {
        Transform viewport = GameObject.Find("Hud_Camp/Panel_CampItemGift/Contents/ItemGiftList/List/Viewport/Content")?.transform;
        if (viewport is null)
            return null;

        return new UiItemGiftList(viewport);
    }

    public IEnumerable<UiItemGiftListEntry> EnumerateEntries()
    {
        Int32 count = _root.GetChildCount();
        for (Int32 i = 0; i < count; i++)
        {
            Transform child = _root.GetChild(i);
            yield return UiItemGiftListEntry.Create(child);
        }
    }
}