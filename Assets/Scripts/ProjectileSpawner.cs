using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public float Force;
    public GameObject Prefab;
	
	public void Shoot ()
	{

	    var bullet = Instantiate(Prefab);

	    bullet.transform.position = transform.position;
	    bullet.transform.rotation = transform.rotation;

	}
}
