using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wumpus_World
{
    public static Wumpus_World Instance { get; private set; }

    private Grilla<CasillaNodo> grilla;

    private List<CasillaNodo> SegurosSinVisitar;
    private List<CasillaNodo> CaminosVisitados;
    private List<CasillaNodo> VecindadValida;


    public Wumpus_World(int width, int height)
    {

        grilla = new Grilla<CasillaNodo>(width, height, 50f, Vector3.zero, (Grilla<CasillaNodo> g, int x, int y) => new CasillaNodo(g, x, y)); // crea la grilla con el ancho y el alto especificados

    }

    public Grilla<CasillaNodo> GetGrilla()
    {
        return grilla;  // recupera la grilla actual

    }


    public void ResolverCaminito(int startX, int startY)
    {
        CasillaNodo StartNode = grilla.GetGridObject(startX, startY);
        CasillaNodo NodoActual; // es la pocicion del agente
        VecindadValida = new List<CasillaNodo> { StartNode }; // casillas de la vecindad que puede visitar
        SegurosSinVisitar = new List<CasillaNodo> { StartNode }; // casillas seguras posibles sin visitar
        CaminosVisitados = new List<CasillaNodo> { StartNode };


        //PathfindingDebugStepVisual.Instance.ClearSnapshots();
        //PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, SegurosSinVisitar, CaminosVisitados);

        NodoActual = StartNode;

        while (SegurosSinVisitar.Count > 0) // mientras que haya casillas seguras sin visitar
        {

           SegurosSinVisitar.Remove(NodoActual);
            CaminosVisitados.Add(NodoActual);

            EvaluarVecindadEfectos(NodoActual);
            //VecindadValida = ValorarVecindad(NodoActual);

            foreach (CasillaNodo NodoValido in EncontrarVecindad(NodoActual)) // para cada vecino valido ....
            {

                if (CaminosVisitados.Contains(NodoValido)) continue;  //si el vecino ya fue visitado, ignorelo


                if (NodoValido.Grilla_Agente == 0 )
                {   
                    VecindadValida.Add(NodoValido);

                    if (!SegurosSinVisitar.Contains(NodoValido))
                    {
                       SegurosSinVisitar.Add(NodoValido);                     
                    }

                    continue;
                }
            }


            if (EvaluarVecindadMov(VecindadValida) != null) // si la vecindad tiene un nodo seguro no visitado ....
            {
                 NodoActual = EvaluarVecindadMov(VecindadValida); // el nodo al que se mueve es el mejor que determina evaluarvecindad ... osea el primero que encuentra seguro en orden horario

            }else
            {
                NodoActual = NodoActual.Padre;


            }

         
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode,SegurosSinVisitar, CaminosVisitados);
        }
    }

      


   public List<CasillaNodo> EncontrarVecindad(CasillaNodo nodoactual) //retorna la vecindad de la casilla nodoactual
    {
        List<CasillaNodo> ListaDeVecinos = new List<CasillaNodo>();
        // Arriba
        if (nodoactual.y + 1 < grilla.GetHeight()) ListaDeVecinos.Add(GetNodo(nodoactual.x, nodoactual.y + 1)); // si no se sale abajo añada el nodo de abajo a ListaDeVecinos 
        //Derecha
        if (nodoactual.x + 1 < grilla.GetWidth())  ListaDeVecinos.Add(GetNodo(nodoactual.x + 1, nodoactual.y)); // si no se sale a la derecha añada el nodo de la derecha a ListaDeVecinos 
        // Abajo 
        if (nodoactual.y - 1 >= 0) ListaDeVecinos.Add(GetNodo(nodoactual.x, nodoactual.y - 1)); // si no se sale a la arriba añada el nodo de arriba a ListaDeVecinos 
        //Izquierda
        if (nodoactual.x - 1 >= 0)   ListaDeVecinos.Add(GetNodo(nodoactual.x - 1, nodoactual.y)); // si no se sale a la izquierda añada el nodo de la izquierda a ListaDeVecinos 

        return ListaDeVecinos;

    }


    private CasillaNodo EvaluarVecindadMov(List<CasillaNodo> ListaNodos)
    {

        CasillaNodo CasillaMejor;

            for (int i = 0; i < ListaNodos.Count; i++)
            {
                    if(ListaNodos[i].Grilla_Agente == 0)
                    {
                        CasillaMejor = ListaNodos[i];
                        return CasillaMejor;
                    }
                   
            }
            return  null;
    }


    private void EvaluarVecindadEfectos(CasillaNodo nodoactual)
    {

       if(nodoactual.Grilla_Real == 0) // si en la real no hay nada -> la vecindad es segura
        {
            foreach (CasillaNodo nodoVecino in EncontrarVecindad(nodoactual))
            {
                nodoactual.Grilla_Agente = 0;
            }
        }

        if (nodoactual.Grilla_Real == 1) // si en la real hay un pozo c muere
        {
            // c muere
        }

        if (nodoactual.Grilla_Real == 2) // si en la real hay un wumpus c muere
        {
            // c muere
        }

        if (nodoactual.Grilla_Real == 3) // si en la real hay brisa, la vecindad es pozo posible
        {
            foreach (CasillaNodo nodoVecino in EncontrarVecindad(nodoactual))
            {
                if(nodoVecino != nodoactual.Padre)
                {
                    nodoactual.Grilla_Agente = 1;
                }
            }
        }

        if (nodoactual.Grilla_Real == 4) // si en la real hay edor, la vecindad es wumpus posible
        {
            foreach (CasillaNodo nodoVecino in EncontrarVecindad(nodoactual))
            {
                if (nodoVecino != nodoactual.Padre)
                {
                    nodoactual.Grilla_Agente = 2;
                }
            }
        }

        if (nodoactual.Grilla_Real == 5) // si en la real hay oro, somos ricos gg 100% real 
        {
            // gana
        }
    }

    public CasillaNodo GetNodo(int x, int y)
    {
        return grilla.GetGridObject(x, y);
    }



}
