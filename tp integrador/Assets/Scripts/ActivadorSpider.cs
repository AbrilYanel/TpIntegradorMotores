using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorSpider : MonoBehaviour
{
    public List<GameObject> spiders;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with the SpiderActivator.");

            // Iterar sobre cada araña y activar el script ScriptAraña
            foreach (GameObject spider in spiders)
            {
                ScriptAraña spiderScript = spider.GetComponent<ScriptAraña>();
                if (spiderScript != null)
                {
                    spiderScript.ActivateSpider(collision.transform); // Pasar el jugador colisionado como parámetro
                    Debug.Log("Activated spider: " + spider.name);
                }
                else
                {
                    Debug.LogWarning("Spider does not have ScriptAraña attached: " + spider.name);
                }
            }

            // Opcional: Desactivar el activador después de usarlo
            // gameObject.SetActive(false);
        }
    }
}
