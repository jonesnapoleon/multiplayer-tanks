using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RacingCash : MonoBehaviour
{
    public float lifeTime = 10f;
    public float rotation = 0.5f;
    public int value = 500;
    public AudioSource m_CashAudio;

    public void OnObjectSpawned()
    {
        Invoke(nameof(DestroySelf), lifeTime);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, rotation, 0));
    }

    // destroy for everyone on the server
    void DestroySelf()
    {
        //NetworkServer.Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        other.GetComponent<RacingTankCash>().Add(value);
        gameObject.SetActive(false);
    }

    public void RpcConsumed()
    {
        // Play the explosion sound effect.
        m_CashAudio.Play();
        gameObject.SetActive(false);
    }
}
