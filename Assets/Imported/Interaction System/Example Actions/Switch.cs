using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour , IAction
{
    /*ESPAÑOL
       *Solución por GameDevTraum
      * 
      * Artículo: https://gamedevtraum.com/gdt-short/sistema-de-interaccion-base-para-unity/
      * Página: https://gamedevtraum.com/es/
      * Canal: https://youtube.com/c/GameDevTraum
      * 
      * Visita la página para encontrar más soluciones, Assets y artículos
      * 
      * ESTE SCRIPT ES UNA ACCIÓN A MODO DE EJEMPLO
      * 
      * Noten que en la definición de la clase, a la derecha de MonoBehaviour se implementa la interfaz IAction,
      * esto nos fuerza a definir un método con el nombre Activate() dentro de este Script.
      * Ese método será ejecutado por el receptor de interacción cuando sea activado.
      * 
      * Dentro de Activate definimos la acción que se debe ejecutar al activar el interruptor. Este Script hace que el 
      * botón cambie de color al pulsarlo. Lo que hacemos es cambiar de estado una variable booleana y en función 
      * de ese estado le asignamos al botón un color u otro. 
      * 
     */
       
    /*ENGLISH
    *Solution by GameDevTraum
    * 
    * Article: https://gamedevtraum.com/gdt-short/basic-interaction-system-for-unity/
    * Website: https://gamedevtraum.com/en/
    * Channel: https://youtube.com/c/GameDevTraum
    * 
    * Visit the website to find more articles, solutions and assets
    * 
    * THIS SCRIPT IS AN EXAMPLE ACTION
    * 
    * Note that in the class definition, to the right of MonoBehaviour, the IAction interface is implemented,
    * this forces us to define a method with the name Activate() within this Script.
    * That method will be executed by the interaction receiver when it is activated.
    * 
    * Within Activate, we define the action to be executed when the switch is activated. This Script makes the 
    * button changes color when pressed. What we do is change the state of a boolean variable and depending 
    * on that state we assign the button one color or another. 
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
    * DIESES SKRIPT IST EINE BEISPIELAKTION
    * 
    * Beacht, dass in der Klassendefinition rechts neben MonoBehaviour die Schnittstelle IAction implementiert ist,
    * Dies zwingt uns, eine Methode mit dem Namen Activate() innerhalb dieses Skripts zu definieren.
    * Diese Methode wird vom InteractionReceiver ausgeführt, wenn sie aktiviert wird.
    *
    * Unter Aktivieren definieren wir die Aktion, die bei der Aktivierung des Schalters ausgeführt werden soll. 
    * Dieses Skript sorgt dafür, dass der Schalter beim Drücken die Farbe wechselt. Was wir tun, 
    * ist den Zustand einer booleschen Variablen zu ändern, und je nach diesem Zustand weisen wir der Schaltfläche 
    * die eine oder andere Farbe zu. 
    * 
    */

    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private Color inactiveColor;
    [SerializeField]
    private Material material;

    private bool activated;

    public void Start()
    {
        material.color = inactiveColor;

    }

    public void Activate()
    {
        activated = !activated;
        if (activated)
        {
            material.color = activeColor;
        }
        else
        {
            material.color = inactiveColor;
        }

    }

}
