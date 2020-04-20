using UnityEngine;

public class Bilboard : MonoBehaviour
{
    public Camera m_camera;

    // Start is called before the first frame update
    void Start()
    {
        CacheCamera();
    }

    private void CacheCamera()
    {
        if (m_camera == null)
        {
            m_camera = GetComponent<Canvas>().worldCamera;
        }
        if (m_camera == null)
        {
            m_camera = Camera.main;
        }
        if (m_camera == null)
        {
            m_camera = FindObjectOfType<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_camera == null)
        {
            CacheCamera();
            return;
        }

        transform.LookAt(transform.position + m_camera.transform.forward);
    }
}
