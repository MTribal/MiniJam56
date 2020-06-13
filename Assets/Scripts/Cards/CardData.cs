using UnityEngine;

[CreateAssetMenu(fileName = "NewCardData", menuName = "ScriptableObjects/CardData")]
public class CardData : ScriptableObject
{
    public int cost;

    public Sprite background;
    public Sprite image;
    public Sprite sprite;

    public Doughnut doughnut;
}
