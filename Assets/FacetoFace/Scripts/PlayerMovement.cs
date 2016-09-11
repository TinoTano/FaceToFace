using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    public GameObject bulletPrefab;

    public override void OnStartLocalPlayer()
    {
        
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        float x = Input.GetAxis("Horizontal") * 0.1f;
        float y = Input.GetAxis("Vertical") * 0.1f;

        transform.Translate(x, y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        // This [Command] code is run on the server!

        // create the bullet object locally
        GameObject bullet = (GameObject)Instantiate(bulletPrefab,transform.position,Quaternion.identity);
        bullet.transform.parent = transform;
        bullet.GetComponent<Rigidbody2D>().velocity = -transform.right * 4;

        // spawn the bullet on the clients
        NetworkServer.Spawn(bullet);

        // when the bullet is destroyed on the server it will automaticaly be destroyed on clients
        Destroy(bullet, 2.0f);
    }
}
