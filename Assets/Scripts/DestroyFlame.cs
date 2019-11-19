using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFlame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeleteFlame", 2f);
    }

    // Update is called once per frame
    void DeleteFlame(){
        Destroy(gameObject);
    }
}
