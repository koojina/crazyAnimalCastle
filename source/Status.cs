using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public GameObject Effect;
    public GameObject Effect2;
    public GameObject meshobj;

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.CompareTag("Player_RedFBubble") || gameObject.CompareTag("Player_RedIBubble") || gameObject.CompareTag("Player_RedPBubble") || gameObject.CompareTag("Player_RedWBubble"))
        {
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player_Blue"))
            {
                StartCoroutine(Explosion());
            }
        }
        else if (gameObject.CompareTag("Player_BlueFBubble") || gameObject.CompareTag("Player_BlueIBubble") || gameObject.CompareTag("Player_BluePBubble") || gameObject.CompareTag("Player_BlueWBubble"))
        {
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player_Red"))
            {
                StartCoroutine(Explosion());
            }
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            StartCoroutine(ExplosionLand());
        }
    }

    IEnumerator Explosion()
    {
        meshobj.SetActive(false);
        Effect.SetActive(true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator ExplosionLand()
    {
        meshobj.SetActive(false);
        Effect2.SetActive(true);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}


