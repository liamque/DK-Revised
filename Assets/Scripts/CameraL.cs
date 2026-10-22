
using UnityEngine;

public class CameraL : MonoBehaviour
{
    private GameObject gameManage;

    private void Start()
    {
        gameManage = GameObject.Find("GameManager");
    }

    private void Update()
    {
        if (gameManage.GetComponent<GameManager>().LugiMode == false)
        {
            transform.position = new Vector3 (0, 0, -10);
            transform.rotation =  new Quaternion (0,0,0,1);
        } else
        {
            transform.position = new Vector3 (0, 0, 10);
            transform.rotation =  new Quaternion (0,180,0,1);
        }
    }
}
