using UnityEngine;

public class Bilboard : MonoBehaviour
{
    public Camera m_camera;

    // Start is called before the first frame update
    void Start()
    {
        if(m_camera == null) {
            m_camera= GetComponent<Canvas>().worldCamera;
        }
        if (m_camera == null)
        {
            m_camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(this.transform.position + m_camera.transform.forward);
    }
}
