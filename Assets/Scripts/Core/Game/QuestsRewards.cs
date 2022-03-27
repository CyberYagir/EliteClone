using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "", menuName = "Game/Rewards", order = 1)]
public class QuestsRewards : ScriptableObject
{
    public Item creditItem;
    public List<Item> canBeRewarded;
    public List<Item> canBeTransfered;
    public List<Item> canBeMineral;
}
