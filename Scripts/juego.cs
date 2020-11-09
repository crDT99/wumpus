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
    private bool OroExiste = false;
    private int[] wumpus = { 0, 0 };
    private int[] oro = { 0, 0 };
    //[SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;


    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual1;
    [SerializeField] private PathfindingVisual pathfindingVisual2;
    [SerializeField] private PathfindingVisual pathfindingVisual3;
    [SerializeField] private PathfindingVisual pathfindingVisual4;
    [SerializeField] private PathfindingVisual pathfindingVisual5;
    private Wumpus_World MundoW;


    private void Start()
    {
        MundoW = new Wumpus_World(20, 10);

        pathfindingVisual.SetGrid(MundoW.GetGrilla());
        pathfindingVisual1.SetGrid(MundoW.GetGrilla());
        pathfindingVisual2.SetGrid(MundoW.GetGrilla());
        pathfindingVisual3.SetGrid(MundoW.GetGrilla());
        pathfindingVisual4.SetGrid(MundoW.GetGrilla());
        pathfindingVisual5.SetGrid(MundoW.GetGrilla());
    }

    private void Update()
    {


        if (EditorType == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                MundoW.GetGrilla().GetXY(mouseWordlPosition, out int x, out int y);
                if (MundoW.GetNodo(x, y).Grilla_Real != 1)
                {
                    MundoW.GetNodo(x, y).CrearReal(1);
                    Debug.Log("pozo hecho");

                    foreach (CasillaNodo NodoValido in MundoW.EncontrarVecindad(MundoW.GetNodo(x, y)))
                    {
                        NodoValido.CrearReal(3);
                    }


                }
                else
                {
                    MundoW.GetNodo(x, y).CrearReal(0);
                    foreach (CasillaNodo NodoValido in MundoW.EncontrarVecindad(MundoW.GetNodo(x, y)))
                    {
                        NodoValido.CrearReal(0);
                    }
                    Debug.Log("retirado");
                }
                MundoW.GetGrilla();

            }

        }
        if (EditorType == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                MundoW.GetGrilla().GetXY(mouseWordlPosition, out int x, out int y);
                if (MundoW.GetNodo(x, y).Grilla_Real != 2)
                {

                    if (WumpusExiste == false)
                    {
                        WumpusExiste = true;
                    }
                    else
                    {
                        MundoW.GetNodo(wumpus[0], wumpus[1]).CrearReal(0);
                        foreach (CasillaNodo NodoValido in MundoW.EncontrarVecindad(MundoW.GetNodo(wumpus[0], wumpus[1])))
                        {
                            NodoValido.CrearReal(0);
                        }
                    }
                        MundoW.GetNodo(x, y).CrearReal(2);
                        wumpus[0] = x;
                        wumpus[1] = y;
                        foreach (CasillaNodo NodoValido in MundoW.EncontrarVecindad(MundoW.GetNodo(x, y)))
                        {
                            NodoValido.CrearReal(4);
                        }           
                        Debug.Log("wumpus hecho");


                }
                else
                {
                    MundoW.GetNodo(x, y).CrearReal(0);
                    foreach (CasillaNodo NodoValido in MundoW.EncontrarVecindad(MundoW.GetNodo(x, y)))
                    {
                        NodoValido.CrearReal(0);
                    }
                    Debug.Log("wumpus retirado");
                    WumpusExiste = false;
                }
                MundoW.GetGrilla();
              
            }

        }

        if (EditorType == 2)
        {
          
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWordlPosition = UtilsClass.GetMouseWorldPosition();
                MundoW.GetGrilla().GetXY(mouseWordlPosition, out int x, out int y);
                if (MundoW.GetNodo(x, y).Grilla_Real != 5)
                {

                  if (OroExiste == false)
                            {
                                OroExiste = true;
                            }
                            else
                            {
                                MundoW.GetNodo(oro[0], oro[1]).CrearReal(0);
                                foreach (CasillaNodo NodoValido in MundoW.EncontrarVecindad(MundoW.GetNodo(oro[0], oro[1])))
                                        {
                                    NodoValido.CrearReal(0);
                                }
                            }
                    MundoW.GetNodo(x, y).CrearReal(5);
                    oro[0] = x;
                    oro[1] = y;
                    Debug.Log("oro hecho");
                }
                else
                {
                    MundoW.GetNodo(x, y).CrearReal(0);
                    OroExiste = false;
                    Debug.Log("oro retirado");
                }
                MundoW.GetGrilla();
               
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
        MundoW.ResolverCaminito(origenx, origeny);
    }

}
