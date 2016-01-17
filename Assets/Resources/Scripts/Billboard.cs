using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

    private Camera m_Camera;

    void Start()
    {
        m_Camera = Camera.main;

        if (m_Camera.orthographic)
            transform.LookAt(transform.position - m_Camera.transform.forward, m_Camera.transform.up);
        else
            transform.LookAt(m_Camera.transform.position, m_Camera.transform.up);
    }
}
