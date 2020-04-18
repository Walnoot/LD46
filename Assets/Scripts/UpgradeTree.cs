using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Tree", menuName = "Upgrades/Upgrade Tree", order = 1)]
public class UpgradeTree : ScriptableObject {
    public List<Upgrade> Upgrades;
}
