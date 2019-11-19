using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBox : MonoBehaviour
{
    [SerializeField] GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -3; x <= 5; x++)
        {
            Instantiate(box, new Vector3(x, transform.position.y + 1, 5), Quaternion.identity);
        }

        for (int x = -3; x <= 5; x += 2)
        {
            Instantiate(box, new Vector3(x, transform.position.y + 1, 4), Quaternion.identity);
        }


        for (int z = 3; z >= -4; z -= 2)
        {
            for (int x = -5; x <= 5; x++)
            {
                Instantiate(box, new Vector3(x, transform.position.y + 1, z), Quaternion.identity);
            }
        }

        for (int z = 2; z >= -3; z -= 2)
        {
            for (int x = -5; x <= 5; x += 2)
            {
                Instantiate(box, new Vector3(x, transform.position.y + 1, z), Quaternion.identity);
            }
        }

        for (int x = -5; x <= 3; x++)
        {
            Instantiate(box, new Vector3(x, transform.position.y + 1, -5), Quaternion.identity);
        }

        for (int x = -5; x <= 3; x += 2)
        {
            Instantiate(box, new Vector3(x, transform.position.y + 1, -4), Quaternion.identity);
        }

    }
}
