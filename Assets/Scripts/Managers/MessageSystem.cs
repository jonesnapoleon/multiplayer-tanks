using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MessageSystem : NetworkBehaviour
{
    public Text m_MessageText;                  // Reference to the overlay Text to display winning text, etc.

    [ClientRpc]
    public void RpcUpdateMessage(string message)
    {
        m_MessageText.text = message;
    }
}
