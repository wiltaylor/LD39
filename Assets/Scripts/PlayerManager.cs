using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;

    public float PowerLeft;
    public float MaxPower;
    public float JumpLeft;
    public float MaxJump;
    public float AmmoLeft;
    public float MaxAmmo;
    

    public PlayerManager()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if(Instance == this)
            Instance = null;
    }
}
