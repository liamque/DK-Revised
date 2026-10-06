using UnityEngine;

public class HammerHitbox : MonoBehaviour
{
    public Player player;        // set in inspector
    public Transform playerTrans;
    private new Rigidbody2D rigidbody;

    public Vector3 playerPos;
    public float playerFacing;

    private void Update()
    {
        playerPos = playerTrans.position;
        playerFacing = playerTrans.rotation.y;

        if (player.hammerDown == true) {
            if (playerFacing == 0) {
                transform.position = new Vector3 (playerPos.x + 0.95f, playerPos.y, playerPos.z);
            } else {
                transform.position = new Vector3 (playerPos.x - 0.95f, playerPos.y, playerPos.z);
            }
        } else
        {
            transform.position = new Vector3 (playerPos.x, playerPos.y + 0.9f, playerPos.z);
        }
        
    }
}
