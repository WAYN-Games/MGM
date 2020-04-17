using UnityEngine;

public class Bilboard : MonoBehaviour
{
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if(camera == null) {
            camera= GetComponent<Canvas>().worldCamera;
        }
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(this.transform.position + camera.transform.forward);
    }
}
