using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] int powerExplosion = 1; // How many block destroy for one explosion
    [SerializeField] float timeExplosion = 2f;
    public float radiusExplosion = 2f;
    [SerializeField] GameObject flame;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Boom", timeExplosion);
    }

    void Boom()
    {
        Hit();
        Destroy(gameObject);
    }

    void Hit()
    {
        Vector3 pointExplosion;
        int countDestroyBlock = 0;
        bool dontDestroy;
        int[] coefNextPointArray = new int[] { 1, -1 };
        Vector3[] vectorNextPointArray = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, 1) };

        foreach (Vector3 vectorNextPoint in vectorNextPointArray)
        {
            foreach (int coefNextPoint in coefNextPointArray)
            {
                countDestroyBlock = 0;
                pointExplosion = transform.position;
                for (int j = 0; j < radiusExplosion; j++)
                {
                    dontDestroy = false;
                    pointExplosion += vectorNextPoint * coefNextPoint;
                    Collider[] intersecting = Physics.OverlapSphere(pointExplosion, 0.01f);

                    if(intersecting.Length == 0)  Instantiate(flame,pointExplosion, Quaternion.identity );

                    foreach (Collider collider in intersecting)
                    {
                        if (collider.gameObject.tag == "Box")
                        {
                            Destroy(collider.gameObject);
                            Instantiate(flame,pointExplosion, Quaternion.identity );
                            countDestroyBlock++;
                        }
                        else if(collider.tag == "Player"){
                           var player = collider.gameObject.GetComponent<Player>();
                           player.Die();
                        }
                        else if(collider.tag == "Bot"){
                           var bot = collider.gameObject.GetComponent<BasicBot>();
                           bot.BotDie();
                        }
                        else
                        {
                            dontDestroy = true;
                        }
                    }
                    if (dontDestroy || countDestroyBlock == powerExplosion) break;
                }
            }
        }
    }

}
