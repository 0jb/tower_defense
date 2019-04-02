using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TD.Gameplay.Units;
using TD.Gameplay.Path;

[ExecuteInEditMode]
public class InstantiateEnemy : MonoBehaviour
{
    public Transform Enemy;
    public PathCell TargetPlace;

    public bool InstanceNow;

    private void Update()
    {
        if(InstanceNow)
        {
            Transform newEnemy = Instantiate(Enemy);
            newEnemy.GetComponent<FollowPath>().OnInstantiate(TargetPlace);
            InstanceNow = false;
        }
    }
}
