using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "ScriptableObjects/CardData")]
public class CardData : ScriptableObject
{
    [Tooltip("Card cost in granulates.")]
    public int cost;
    [Tooltip("Card cooldown in seconds.")]
    public float cooldown;

    [Tooltip("Card background.")]
    public Sprite background;
    [Tooltip("Card UI image.")]
    public Sprite image;
    [Tooltip("Card first sprite.")]
    public Sprite sprite;

    [Tooltip("Card prefab.")]
    public Doughnut doughnut;
}
