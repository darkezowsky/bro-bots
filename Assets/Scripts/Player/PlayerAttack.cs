using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackSequence());
            Debug.Log("HEY, I'M ATTACKING HERE");
            //animation
        }
    }

    private IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(0.25f);
        //particle
        //attack
    }
}
