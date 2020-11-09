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


    //public CasillaNodo ResolverPasoCaminito(int startX, int startY)
    //{
    //    CasillaNodo StartNode = grilla.GetGridObject(startX, startY);
    //    CasillaNodo NodoActual; // es la pocicion del agente
    //    VecindadValida = new List<CasillaNodo> { StartNode }; // casillas de la vecindad que puede visitar
    //    SegurosSinVisitar = new List<CasillaNodo> { StartNode }; // casillas seguras posibles sin visitar
    //    CaminosVisitados = new List<CasillaNodo> { StartNode };


    //   // PathfindingDebugStepVisual.Instance.ClearSnapshots();
    //    //PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode, SegurosSinVisitar, CaminosVisitados);

    //    NodoActual = StartNode;

    //    while (SegurosSinVisitar.Count > 0) // mientras que haya casillas seguras sin visitar
    //    {

    //       SegurosSinVisitar.Remove(NodoActual);
    //        CaminosVisitados.Add(NodoActual);
    //        NodoActual.CrearReal(-1);

    //        EvaluarVecindadEfectos(NodoActual);
    //        //VecindadValida = ValorarVecindad(NodoActual);

    //        foreach (CasillaNodo NodoValido in EncontrarVecindad(NodoActual)) // para cada vecino valido ....
    //        {

    //            if (CaminosVisitados.Contains(NodoValido)) continue;  //si el vecino ya fue visitado, ignorelo


    //            if (NodoValido.Grilla_Agente == 0 )
    //            {   
    //                VecindadValida.Add(NodoValido);

    //                if (!SegurosSinVisitar.Contains(NodoValido))
    //                {
    //                   SegurosSinVisitar.Add(NodoValido);                     
    //                }

    //                continue;
    //            }
    //        }


    //        if (EvaluarVecindadMov(VecindadValida) != null) // si la vecindad tiene un nodo seguro no visitado ....
    //        {
    //             NodoActual = EvaluarVecindadMov(VecindadValida); // el nodo al que se mueve es el mejor que determina evaluarvecindad ... osea el primero que encuentra seguro en orden horario

    //        }else
    //        {
    //            NodoActual = NodoActual.Padre;


    //        }

         
    //           // PathfindingDebugStepVisual.Instance.TakeSnapshot(grilla, StartNode,SegurosSinVisitar, CaminosVisitados);
    //    }
    //}

      


    public CasillaNodo GetNodo(int x, int y)
    {
        return grilla.GetGridObject(x, y);
    }



}
