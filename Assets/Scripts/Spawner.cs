using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] type; //List of barrel prefabs
    public float minTime = 2f;
    public float maxTime = 4f;

    public bool firstBarrel = true;
    public int randomType;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (firstBarrel) {
            Instantiate(type[1], transform.position, Quaternion.identity);
            firstBarrel = false;
        } else
        {
            randomType = Random.Range(0,4);
            if (randomType < 2) {
                Instantiate(type[0], transform.position, Quaternion.identity);
            } else if (randomType == 2)
            {
                Instantiate(type[1], transform.position, Quaternion.identity);
            } else if (randomType == 3)
            {
                Instantiate(type[2], transform.position, Quaternion.identity);
            }
        }
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }
}
