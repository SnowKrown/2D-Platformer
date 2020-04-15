using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHp = 3;
    [SerializeField]
    private GameObject blood = default;

    private int currentHp = 0;

    private void Awake()
    {
        if (blood !=  null) 
            blood.SetActive(false);

        currentHp = maxHp;
    }

    public void MakeDamage ()
    {
        if (currentHp > 0)
        {
            currentHp--;

            if (currentHp == 0)
            {
                blood.transform.SetParent(null);
                blood.transform.SetPositionAndRotation(transform.position, blood.transform.rotation);
                blood.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
