using UnityEngine;
[System.Serializable]
public class CharacterAbility
{
    public PlayerAbility ability;
    public GameObject icon;
}
public class CharacterAbilityInfo : MonoBehaviour
{
    public CharacterAbility[] abilities;
}
