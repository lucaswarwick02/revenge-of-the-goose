using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public bool CombatEnabled { get; private set; }
    
    public void EnableCombat () {
        CombatEnabled = true;
    }

    public void DisableCombat () {
        CombatEnabled = false;
    }
}
