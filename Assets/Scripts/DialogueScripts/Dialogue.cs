using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [Tooltip("Shows character portraits contained in this dialogue")]
    public Sprite[] portraits;
    [Tooltip("The lines the npcs say when the player talks to them")]
    public string[] lines;
}