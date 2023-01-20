using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public PortalScript ps;
    [SerializeField] int enemyCount;
    [SerializeField] bool stageComplete;

    void Start()
    {
        ps.SendMessage("ExitPortal");

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCount <= 0 && !stageComplete)
        {
            stageComplete = true;
            ps.SendMessage("Appear");
        }
    }

    void EnemyDie()
    {
        enemyCount--;
    }
}
