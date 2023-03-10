using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
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
    * ¿PARA QUÉ SIRVE ESTE SCRIPT?
    * 
    * Los Scripts que hagamos para resolver alguna acción y que serán activados por los interruptores
    * deben implementar esta interfaz llamada IAction, de esa forma deberán tener definido un método Activate que
    * se ejecutará cuando activemos el interruptor.
    * Dentro del método Activate resolvemos el comportamiento de la acción.
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
    * WHAT IS THIS SCRIPT FOR?
    * 
    * The scripts we make to solve some action and that will be activated by the switches
    * must implement this interface called IAction, so they must have defined an Activate method that
    * will run when we activate the switch.
    * Within the Activate method we solve the behavior of the action.
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
    * WOZU DIENT DIESES SKRIPT?
    * 
    * Die Skripte, die wir zur Lösung einer Aktion erstellen und die von den Schaltern aktiviert werden,   
    * müssen diese Schnittstelle namens IAction implementieren, d.h. sie müssen eine Activate-Methode definiert haben,
    * die ausgeführt wird, wenn wir den Schalter aktivieren. 
    * Innerhalb der Activate-Methode lösen wir das Verhalten der Aktion.
    * 
    */

    void Activate();

    
}
