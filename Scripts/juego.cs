using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

public class juego : MonoBehaviour
{
    public int EditorType = 0;
    private int origeny = 0;
    private int origenx = 0;
    private bool WumpusExiste = false;
    private bool OroExiste = false, oroencontrado = false;
    private int[] wumpus = { 0, 0 };
    private int[] oro = { 0, 0 };

    private Grilla<CasillaNodo> grilla;
    private List<CasillaNodo> SegurosSinVisitar;
    private List<CasillaNodo> CaminosVisitados;
    private List<CasillaNodo> VecindadValida;
    private CasillaNodo NodoActual,StartNode,aux;
    //[SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;


    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual1;
    [SerializeField] private PathfindingVisual pathfindingVisual2;
    [SerializeField] private PathfindingVisual pathfindingVisual3;
    [SerializeField] private PathfindingVisual pathfindingVisual4;
    [SerializeField] private PathfindingVisual pathfindingVisual5;
    [SerializeField] private PathfindingVisualPlayer pathfindingVisualP;
    [SerializeField] private PathfindingVisualPlayer pathfindingVisualP1;
    [SerializeField] private PathfindingVisualPlayer pathfindingVisualP2;
    [SerializeField] private PathfindingVisualPlayer pathfindingVisualP3;
    private Wumpus_World MundoW;

    public void iniciargrilla(int width, int height)
    {

        grilla = new Grilla<CasillaNodo>(width, height, 50f, Vector3.zero, (Grilla<CasillaNodo> g, int x, int y) => new CasillaNodo(g, x, y)); // crea la grilla con el ancho y el alto especificados

    }

    public Grilla<CasillaNodo> GetGrilla()
    {
        return grilla;  // recupera la grilla actual

    }

    private void Start()
    {
       
        iniciargrilla(20, 10);
        //pathfindingDebugStepVisual.Setup(grilla);
        pathfindingVisual.SetGrid(grilla);
        pathfindingVisual1.SetGrid(grilla);
        pathfindingVisual2.SetGrid(grilla);
        pathfindingVisual3.SetGrid(grilla);
        pathfindingVisual4.SetGrid(grilla);
        pathfindingVisual5.SetGrid(grilla);
        pathfindingVisualP.SetGrid(grilla);
        pathfindingVisualP1.SetGrid(grilla);
        pathfindingVisualP2.SetGrid(grilla);
        pathfindingVisualP3.SetGrid(grilla);
        StartNode = grilla.GetGridObject(0, 0);
        VecindadValida = new List<CasillaNodo>(); // casillas de la vecindad que puede visitar
        SegurosSinVisitar = new List<CasillaNodo> { StartNode }; // casillas seguras posibles sin visitar
        CaminosVisitados = new List<CasillaNodo>();
        NodoActual = grilla.GetGridObject(0, 0);
        NodoActual.CrearPosible(-1);
      
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            
            EvaluarVecindadEfectos(NodoActual);
            if(oroencontrado == false)
            {

            
            if (SegurosSinVisitar.Count > 0) // mientras que haya casillas seguras sin visitar
            {
                Debug.Log("movimiento");
          
                SegurosSinVisitar.Remove(NodoActual);
                NodoActual.visitar(true);

               
                //VecindadValida = ValorarVecindad(NodoActual);

                foreach (CasillaNodo NodoValido in EncontrarVecindad(NodoActual)) // para cada vecino valido ....
                {
                    Debug.Log("foeach");
                    


                    if (NodoValido.Grilla_Agente == 0 )
                    {
                        Debug.Log("es seguro el paso");

                        if (NodoValido.Visitado)
                        { Debug.Log("ya esta visitado y es una mierda");
                            continue;
                        }
                        else
                        {
                            VecindadValida.Add(NodoValido);
                            SegurosSinVisitar.Add(NodoValido);

                            //Debug.Log("añade vecindad");
                            //if (!SegurosSinVisitar.Contains(NodoValido))
                            //{
                            //    Debug.Log("añade nodo");
                            //    SegurosSinVisitar.Add(NodoValido);
                            //}

                        }
                       
                    }
                    else
                    {
                        
                        Debug.Log("no entiendo");

                    }
                }


                if (VecindadValida.Count >0) // si la vecindad tiene un nodo seguro no visitado ....
                {
                    Debug.Log("se mueve");
                    NodoActual.CrearPosible(0);
                    CaminosVisitados.Add(NodoActual);
                    aux = NodoActual;
                    NodoActual = EvaluarVecindadMov(VecindadValida); // el nodo al que se mueve es el mejor que determina evaluarvecindad ... osea el primero que encuentra seguro en orden horario
                    NodoActual.Crearpadre(aux);
                    NodoActual.CrearPosible(-1);
                    
                }
                else
                {
                    Debug.Log("no hay");
                    NodoActual.CrearPosible(0);
                    //CaminosVisitados.Add(NodoActual);



                    NodoActual = NodoActual.Padre;
                    NodoActual.CrearPosible(-1);
                    

                }
                VecindadValida.Clear();
                

            } else
                {
                    Debug.Log("efesota");
                }

            }
            else
            {
                Debug.Log("Ganaste");
            }

        }
       


        if (EditorType == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                    grilla.GetXY(mouseWordlPosition, out int x, out int y);
                    if (grilla.GetGridObject(x, y).Grilla_Real != 1)
                    {
                        grilla.GetGridObject(x, y).CrearReal(1);
                        Debug.Log("pozo hecho");

                        foreach (CasillaNodo NodoValido in EncontrarVecindad(grilla.GetGridObject(x, y)))
                        {
                            NodoValido.CrearReal(3);
                        }


                    }
                    else
                    {
                        grilla.GetGridObject(x, y).CrearReal(0);
                        foreach (CasillaNodo NodoValido in EncontrarVecindad(grilla.GetGridObject(x, y)))
                        {
                            NodoValido.CrearReal(0);
                        }
                        Debug.Log("retirado");
                    }


                }

            }
            if (EditorType == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                    grilla.GetXY(mouseWordlPosition, out int x, out int y);
                    if (grilla.GetGridObject(x, y).Grilla_Real != 2)
                    {

                        if (WumpusExiste == false)
                        {
                            WumpusExiste = true;
                        }
                        else
                        {
                            grilla.GetGridObject(wumpus[0], wumpus[1]).CrearReal(0);
                            foreach (CasillaNodo NodoValido in EncontrarVecindad(grilla.GetGridObject(wumpus[0], wumpus[1])))
                            {
                                NodoValido.CrearReal(0);
                            }
                        }
                        grilla.GetGridObject(x, y).CrearReal(2);
                        wumpus[0] = x;
                        wumpus[1] = y;
                        foreach (CasillaNodo NodoValido in EncontrarVecindad(grilla.GetGridObject(x, y)))
                        {
                            NodoValido.CrearReal(4);
                        }
                        Debug.Log("wumpus hecho");


                    }
                    else
                    {
                        grilla.GetGridObject(x, y).CrearReal(0);
                        foreach (CasillaNodo NodoValido in EncontrarVecindad(grilla.GetGridObject(x, y)))
                        {
                            NodoValido.CrearReal(0);
                        }
                        Debug.Log("wumpus retirado");
                        WumpusExiste = false;
                    }


                }

            }

            if (EditorType == 2)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                    grilla.GetXY(mouseWordlPosition, out int x, out int y);
                    if (grilla.GetGridObject(x, y).Grilla_Real != 5)
                    {

                        if (OroExiste == false)
                        {
                            OroExiste = true;
                        }
                        else
                        {
                            grilla.GetGridObject(oro[0], oro[1]).CrearReal(0);
                            foreach (CasillaNodo NodoValido in EncontrarVecindad(grilla.GetGridObject(oro[0], oro[1])))
                            {
                                NodoValido.CrearReal(0);
                            }
                        }
                        grilla.GetGridObject(x, y).CrearReal(5);
                        oro[0] = x;
                        oro[1] = y;
                        Debug.Log("oro hecho");
                    }
                    else
                    {
                        grilla.GetGridObject(x, y).CrearReal(0);
                        OroExiste = false;
                        Debug.Log("oro retirado");
                    }


                }

            }
       



    }



    public void Set_Process(int val)
    {
        if (val == 0)
        {
            EditorType = 0;
        }

        if (val == 1)
        {
            EditorType = 1;
        }
        if (val == 2)
        {
            EditorType = 2;
        }
    }



    public void inicio()
    {
        if (!WumpusExiste)
        {
            int RandomXW = Random.Range(5 , 20);
            int RandomXW = Random.Range(5, 20);


        }
         private bool OroExiste


    }

    




    public List<CasillaNodo> EncontrarVecindad(CasillaNodo nodoactual) //retorna la vecindad de la casilla nodoactual
    {
        List<CasillaNodo> ListaDeVecinos = new List<CasillaNodo>();
        // Arriba
        if (nodoactual.y + 1 < grilla.GetHeight()) ListaDeVecinos.Add(GetNodo(nodoactual.x, nodoactual.y + 1)); // si no se sale abajo añada el nodo de abajo a ListaDeVecinos 
        //Derecha
        if (nodoactual.x + 1 < grilla.GetWidth()) ListaDeVecinos.Add(GetNodo(nodoactual.x + 1, nodoactual.y)); // si no se sale a la derecha añada el nodo de la derecha a ListaDeVecinos 
        // Abajo 
        if (nodoactual.y - 1 >= 0) ListaDeVecinos.Add(GetNodo(nodoactual.x, nodoactual.y - 1)); // si no se sale a la arriba añada el nodo de arriba a ListaDeVecinos 
        //Izquierda
        if (nodoactual.x - 1 >= 0) ListaDeVecinos.Add(GetNodo(nodoactual.x - 1, nodoactual.y)); // si no se sale a la izquierda añada el nodo de la izquierda a ListaDeVecinos 

        return ListaDeVecinos;

    }


    private CasillaNodo EvaluarVecindadMov(List<CasillaNodo> ListaNodos)
    {

        CasillaNodo CasillaMejor;

        for (int i = 0; i < ListaNodos.Count; i++)
        {
            if (ListaNodos[i].Grilla_Agente == 0)
            {
                Debug.Log("sig_segura");
                CasillaMejor = ListaNodos[i];
                return CasillaMejor;
            }

        }
        return null;
    }


    private void EvaluarVecindadEfectos(CasillaNodo nodoactual)
    {

        if (nodoactual.Grilla_Real == 0) // si en la real no hay nada -> la vecindad es segura
        {
            
            foreach (CasillaNodo nodoVecino in EncontrarVecindad(nodoactual))
            {
                nodoVecino.CrearPosible(0);
                Debug.Log("vecindad segura");
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

                if(nodoVecino.Grilla_Agente != 0)
                {
                    nodoVecino.CrearPosible(1);
                }
                
         
            }
        }

        if (nodoactual.Grilla_Real == 4) // si en la real hay edor, la vecindad es wumpus posible
        {
            foreach (CasillaNodo nodoVecino in EncontrarVecindad(nodoactual))
            {
                if (nodoVecino.Grilla_Agente != 0)
                {
                    nodoVecino.CrearPosible(2);
                }
            }
        }

        if (nodoactual.Grilla_Real == 5) // si en la real hay oro, somos ricos gg 100% real 
        {
            oroencontrado = true;
        }
    }

    public CasillaNodo GetNodo(int x, int y)
    {
        return grilla.GetGridObject(x, y);
    }

}
