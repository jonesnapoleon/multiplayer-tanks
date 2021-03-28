using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingCashManager : MonoBehaviour
{    
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.

    private RacingCash Cash;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


    public void Setup ()
    {
        // Get references to the components.
        Cash = m_Instance.GetComponent<RacingCash> ();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas> ().gameObject;

        // Get all of the renderers of the tank.
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();      
    }
}
