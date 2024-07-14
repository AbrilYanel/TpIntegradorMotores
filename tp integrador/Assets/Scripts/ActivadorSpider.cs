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

            // Iterar sobre cada ara�a y activar el script ScriptAra�a
            foreach (GameObject spider in spiders)
            {
                ScriptAra�a spiderScript = spider.GetComponent<ScriptAra�a>();
                if (spiderScript != null)
                {
                    spiderScript.ActivateSpider(collision.transform); // Pasar el jugador colisionado como par�metro
                    Debug.Log("Activated spider: " + spider.name);
                }
                else
                {
                    Debug.LogWarning("Spider does not have ScriptAra�a attached: " + spider.name);
                }
            }

            // Opcional: Desactivar el activador despu�s de usarlo
            // gameObject.SetActive(false);
        }
    }
}
