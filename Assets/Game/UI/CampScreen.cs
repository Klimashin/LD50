using System;
using UnityEngine;

public class CampScreen : UIScreen
{
    [SerializeField] private CampSystem _campSystem;

    public Action OnAllCharactersDead;
    public Action OnDayEnded;

}
