using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    /*ESPAÑOL
     *Solución por GameDevTraum
    * 
    * Artículo: https://gamedevtraum.com/gdt-short/sistema-de-interaccion-base-para-unity/
    * Página: https://gamedevtraum.com/es/
    * Canal: https://youtube.com/c/GameDevTraum
    * 
    * Visita la página para encontrar más soluciones, Assets y artículos
   */

    /*ENGLISH
    *Solution by GameDevTraum
    * 
    * Article: https://gamedevtraum.com/gdt-short/basic-interaction-system-for-unity/
    * Website: https://gamedevtraum.com/en/
    * Channel: https://youtube.com/c/GameDevTraum
    * 
    * Visit the website to find more articles, solutions and assets
    */

    /*DEUTSCH
    *Lösung von GameDevTraum
    * 
    * Artikel: https://gamedevtraum.com/gdt-short/grundlegendes-interaktionssystem-fuer-unity/
    * Webseite: https://gamedevtraum.com/de/
    * Kanal: https://youtube.com/c/GameDevTraum
    * 
    * Besuch die Website, um weitere Artikel, Lösungen und Hilfsmittel zu finden. 
    *
    */

    [SerializeField]
    private float messageTime;

    [SerializeField]
    private GameObject messageGameObject;

    [SerializeField]
    private GameObject messageTextGameObject;

    private Text messageText;

    void Start()
    {
        messageText = messageTextGameObject.GetComponent<Text>();
        messageGameObject.SetActive(false);

    }
    
    void FixedUpdate()
    {
        clearMessage();
    }

    public void showMessage(string message)
    {
        messageText.text = message;
        messageGameObject.SetActive(true);
    }

    public void clearMessage()
    {
        messageGameObject.SetActive(false);
    }
}
