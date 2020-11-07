using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wumpus_World
{
    public static Wumpus_World Instance { get; private set; }

    private Grilla<CasillaNodo> grilla;

    private List<CasillaNodo> CaminosPosiblesSeguros;
    private List<CasillaNodo> CaminosVisitados;


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

        CaminosPosiblesSeguros = new List<CasillaNodo> { StartNode }; // casillas seguras posibles
        CaminosVisitados = new List<CasillaNodo>();

    
        PathfindingDebugStepVisual.Instance.ClearSnapshots();
        PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, CaminosPosiblesSeguros, CaminosVisitados);




        while (CaminosPosiblesSeguros.Count > 0) // mientras que haya casillas seguras
        {

            EvaluarVecindadEfectos(CaminosPosiblesSeguros);



            CasillaNodo NodoActual = EncontrarValorMenor(CaminosPosibles, type);
            if (NodoActual == endNode)
            {

                PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, CaminosPosibles, CaminosVisitados);
                PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grilla, CaminoCalculado(endNode));
                return CaminoCalculado(endNode);
            }
            CaminosPosibles.Remove(NodoActual);
            CaminosVisitados.Add(NodoActual);

            foreach (CasillaNodo nodoVecino in ValorarVecindad(NodoActual))
            {
                if (CaminosVisitados.Contains(nodoVecino)) continue;
                if (!nodoVecino.casillaValida)
                {
                    CaminosVisitados.Add(nodoVecino);
                    continue;
                }
                int CostoGIdeal = NodoActual.CostoG + CalcularDistanciaEntre(NodoActual, nodoVecino);
                if (CostoGIdeal < nodoVecino.CostoG)
                {
                    nodoVecino.Padre = NodoActual;
                    nodoVecino.CostoG = CostoGIdeal;
                    nodoVecino.CostoH = CalcularDistanciaEntre(nodoVecino, endNode);
                    nodoVecino.CalcularCostoF();
                    if (!CaminosPosibles.Contains(nodoVecino))
                    {
                        CaminosPosibles.Add(nodoVecino);
                    }
                }
                PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, CaminosPosibles, CaminosVisitados);
            }
        }

        //una vez se acaben los caminos posibles y no hay solucion
        return null;



    }




    private List<CasillaNodo> EncontrarVecindad(CasillaNodo nodoactual) //retorna la vecindad de la casilla nodoactual
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
