using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Cash : NetworkBehaviour, IPooledObject
{
    public float lifeTime = 20f;
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
    [Server]
    void DestroySelf()
    {
        //NetworkServer.Destroy(gameObject);
        NetworkServer.UnSpawn(gameObject);
        gameObject.SetActive(false);
    }

    // ServerCallback because we don't want a warning if OnTriggerEnter is
    // called on the client
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        other.GetComponent<TankCash>().RpcAdd(value);

        RpcConsumed();

        // Destroy the shell.
        NetworkServer.UnSpawn(gameObject);
        gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcConsumed()
    {
        if (!isLocalPlayer) return;

        // Play the explosion sound effect.
        m_CashAudio.Play();
        gameObject.SetActive(false);
    }
}
