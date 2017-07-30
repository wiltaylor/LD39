using Assets.Scripts;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float Force;
	
	public void Shoot ()
	{

	    var bullet = LevelManager.Instance.BulletPool.GetInstance();

	    bullet.transform.position = transform.position;
	    bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);

	}
}
