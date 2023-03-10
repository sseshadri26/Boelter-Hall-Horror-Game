using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour , IAction
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
      * Dentro de Activate definimos la acción que se debe ejecutar al activar el interruptor, en este caso
      * simplemente cambiamos de estado una variable booleana, pero esto hace que en el método FixedUpdate el portón
      * se abra o se cierre dependiendo del estado de esta variable.
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
    * Within Activate we define the action to be executed when the switch is activated, in this case
    * we simply change the state of a boolean variable, but this makes the gate in the FixedUpdate method
    * opens or closes depending on the state of this variable.
    * 
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
    * Innerhalb von Activate definieren wir die Aktion, die bei der Aktivierung des Schalters ausgeführt werden soll, in diesem Fall
    * wir ändern einfach den Status einer booleschen Variablen, aber das macht das Gate in der FixedUpdate-Methode
    * öffnet oder schließt sich je nach Zustand dieser Variable.
    * 
    */

    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private float velocity;
    private bool activated;
    private Vector3 moveDirection;

    void Start()
    {
        moveDirection = Vector3.Normalize(endPoint.position - startPoint.position);
    }

    void FixedUpdate()
    {
        if (activated)
        {
            if (Vector3.Distance(gameObject.transform.position, endPoint.position)>0.05f)
            {
                gameObject.transform.position += moveDirection * velocity/100;
            }

        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, startPoint.position) > 0.05f)
            {
                gameObject.transform.position -= moveDirection * velocity/100;

            }
        }
    }

    public void Activate()
    {
        activated = !activated;
        if (activated)
        {
            Debug.Log("Gate activated");
        }
        else
        {
            Debug.Log("Gate deactivated");

        }
    }

}
