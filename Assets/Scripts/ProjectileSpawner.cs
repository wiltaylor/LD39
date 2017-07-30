using Assets.Scripts;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float Force;
    public GameObject Owner;
	
	public void Shoot ()
	{

	    var bullet = LevelManager.Instance.BulletPool.GetInstance();

	    bullet.transform.position = transform.position;
	    bullet.transform.rotation = transform.rotation;
	    bullet.GetComponent<BulletHandler>().Owner = Owner;
        bullet.SetActive(true);

	}
}
