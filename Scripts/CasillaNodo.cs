using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    //se hace una clase CasillaNodo que representara cada espacio de la cuadricula en la grilla
public class CasillaNodo : MonoBehaviour
{
    private Grilla<CasillaNodo> grilla;
    public int x, y, Grilla_Real, Grilla_Agente;  // grilla real representa la grilla creada por el usuario, con los elementos de oro, pozo y wumpus en pociciones conocidas 0 = nada, 1 = pozo , 2 = wumpus , 3 = brisa , 4 = edor , 5 = oro
    //grilla_agente es la grilla que crea el agente basandose en operaciones logicas 0 = nada, 1 = pozo , 2 = wumpus
    public CasillaNodo Padre;  // apuntador al nodo padre del nodo actual
    public bool Visitado;


    public CasillaNodo(Grilla<CasillaNodo> grilla, int x, int y) //constructor de la clase nodo
    {
        this.grilla = grilla;
        this.x = x;
        this.y = y;
        this.Grilla_Real = 0;
        this.Grilla_Agente = -2;
        this.Padre = null;
        Visitado = false;
    }

    public void CrearReal(int tipo)
    {
        this.Grilla_Real = tipo;
        grilla.TriggerGridObjectChanged(x, y);
    }


    public void CrearPosible(int tipo)
    {
        this.Grilla_Agente = tipo;
        grilla.TriggerGridObjectChanged(x, y);
    }


    public override string ToString()
    {
        return x + "," + y;
    }

    public void Crearpadre(CasillaNodo padre)
    {
        this.Padre = padre;
        grilla.TriggerGridObjectChanged(x, y);
    }

    public void visitar(bool visitado)
    {
        this.Visitado = visitado;
        grilla.TriggerGridObjectChanged(x, y);
    }

}
