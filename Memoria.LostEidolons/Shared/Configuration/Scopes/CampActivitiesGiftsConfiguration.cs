using System;
using Memoria.LostEidolons.Configuration.Color;
using UnityEngine;

namespace Memoria.LostEidolons.Configuration;

[ConfigScope("Camp.Activities.Gifts")]
public abstract partial class CampActivitiesGiftsConfiguration
{
    [ConfigEntry($"Colorize preferred gifts in the camp dialogues in different color. See [{nameof(PreferredGiftsColor)}]")]
    public virtual Boolean ColorizePreferredGifts => true;

    [ConfigEntry($"Colorize common gifts in the camp dialogues in different color. See [{nameof(CommonGiftsColor)}]")]
    public virtual Boolean ColorizeCommonGifts => true;

    [ConfigEntry($"Color of preferred gifts.")]
    [ConfigConverter(nameof(PreferredGiftsColorConverter))]
    public virtual UnityEngine.Color PreferredGiftsColor => new Color32(0, 255, 255, 255);

    [ConfigEntry($"Color of common gifts.")]
    [ConfigConverter(nameof(CommonGiftsColorConverter))]
    public virtual UnityEngine.Color CommonGiftsColor => new Color32(125, 125, 125, 255);

    protected IAcceptableValue<UnityEngine.Color> PreferredGiftsColorConverter { get; } = new AcceptableColor(nameof(PreferredGiftsColor));
    protected IAcceptableValue<UnityEngine.Color> CommonGiftsColorConverter { get; } = new AcceptableColor(nameof(CommonGiftsColor));

    public abstract void CopyFrom(CampActivitiesGiftsConfiguration configuration);
}