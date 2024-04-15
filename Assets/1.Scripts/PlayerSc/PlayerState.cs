using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Archer")]
public class PlayerState : ScriptableObject
{
   public string characterName = "πÃ¡§";
    public int hp = 100;
    public int str = 3;
    public int dex = 5;
    public float walkSpeed = 6f;
    public float runSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
