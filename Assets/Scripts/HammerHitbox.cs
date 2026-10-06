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

        if (playerFacing == 0) {
            transform.position = new Vector3 (playerPos.x + 0.7f, playerPos.y + 0f, playerPos.z);
        } else {
            transform.position = new Vector3 (playerPos.x - 0.7f, playerPos.y - 0f, playerPos.z);
        }
        
    }
}
