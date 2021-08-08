using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SamUsual.Editor;
using SamUsual.Common;
using NUnit.Framework;

public class PaternEditorWIndow : EditorWindow
{

    private readonly int inspectorWidth = 250;
    public int sizeBetNode = 50;

    private Rect inspectorRect;

    private Vector2 scrollInspectorBar;

    public Vector2Int
                    doteSize = new Vector2Int(50, 50),
                    tempSize = Vector2Int.one * 100;
    //Fields
    Patterns pattern;


    Vector3[,] squarePosition = new Vector3[7, 12] {
                    {new Vector3(100,50),new Vector3(100,100),new Vector3(100,150),new Vector3(100,200),new Vector3(100,250),new Vector3(100,300),new Vector3(100,350),new Vector3(100,400),new Vector3(100,450),new Vector3(100,500),new Vector3(100,550),new Vector3(100,600)},
                    {new Vector3(200,50),new Vector3(200,100),new Vector3(200,150),new Vector3(200,200),new Vector3(200,250),new Vector3(200,300),new Vector3(200,350),new Vector3(200,400),new Vector3(200,450),new Vector3(200,500),new Vector3(200,550),new Vector3(200,600)},
                    {new Vector3(300,50),new Vector3(300,100),new Vector3(300,150),new Vector3(300,200),new Vector3(300,250),new Vector3(300,300),new Vector3(300,350),new Vector3(300,400),new Vector3(300,450),new Vector3(300,500),new Vector3(300,550),new Vector3(300,600)},
                    {new Vector3(400,50),new Vector3(400,100),new Vector3(400,150),new Vector3(400,200),new Vector3(400,250),new Vector3(400,300),new Vector3(400,350),new Vector3(400,400),new Vector3(400,450),new Vector3(400,500),new Vector3(400,550),new Vector3(400,600)},
                    {new Vector3(500,50),new Vector3(500,100),new Vector3(500,150),new Vector3(500,200),new Vector3(500,250),new Vector3(500,300),new Vector3(500,350),new Vector3(500,400),new Vector3(500,450),new Vector3(500,500),new Vector3(500,550),new Vector3(500,600)},
                    {new Vector3(600,50),new Vector3(600,100),new Vector3(600,150),new Vector3(600,200),new Vector3(600,250),new Vector3(600,300),new Vector3(600,350),new Vector3(600,400),new Vector3(600,450),new Vector3(600,500),new Vector3(600,550),new Vector3(600,600)},
                    {new Vector3(1000,50),new Vector3(1000,100),new Vector3(1000,150),new Vector3(1000,200),new Vector3(1000,250),new Vector3(1000,300),new Vector3(1000,350),new Vector3(1000,400),new Vector3(1000,450),new Vector3(1000,500),new Vector3(1000,550),new Vector3(1000,600)},
    };
    Vector3[,] ennemies = new Vector3[7, 12];

    Vector3 positionCliked;
    private Vector2Int xClicked;
    private Rect ViewRect
    {
        get
        {
            return new Rect(Vector2.zero, ViewRectSize);
        }
    }
    private Vector2 ViewRectSize
        => tempSize * (doteSize + Vector2Int.one * sizeBetNode) + Vector2Int.one * sizeBetNode;


    private Vector2 mousePosition;
    static Texture2D background, inspectorColor, normalColor, teleportColor, tankColor,bossColor,groupedColor, redTexture;
    static Texture2D[] rowColor = new Texture2D[7];
   
    private bool doOnce;

    private GUIStyle grosBoutonRouge;
    public void Init(in Patterns pattern)
    {

        this.pattern = pattern;
        var win = EditorWindow.GetWindow(typeof(PaternEditorWIndow));
        background = UsualFunction.MakeTex(2, 2, Color.black);
        inspectorColor = UsualFunction.MakeTex(500, 1000, Color.gray);

        //attention c'est dégeu mais j'ai la flemme
        rowColor[0] = UsualFunction.MakeTex(100, 100, Color.green);
        rowColor[1] = UsualFunction.MakeTex(100, 100, Color.red);
        rowColor[2] = UsualFunction.MakeTex(100, 100, new Color(0.27f, 0.20f, 0.15f));
        rowColor[3] = UsualFunction.MakeTex(100, 100, Color.yellow);
        rowColor[4] = UsualFunction.MakeTex(100, 100, Color.blue);
        rowColor[5] = UsualFunction.MakeTex(100, 100, new Color(0.35f, 0.25f, 0.5f));
        rowColor[6] = UsualFunction.MakeTex(100, 100, Color.white);
       
        normalColor = UsualFunction.MakeTex(50, 50, Color.cyan);
        teleportColor = UsualFunction.MakeTex(50, 50, new Color(0.2f,0.5f,0.8f));
        tankColor = UsualFunction.MakeTex(50, 50, new Color(0.8f, 0.2f, 0.5f));
        groupedColor = UsualFunction.MakeTex(50, 50, new Color(0.8f, 0.5f, 0.2f));
        bossColor = UsualFunction.MakeTex(50, 50, new Color(0.2f, 0.8f, 0.5f));
        redTexture = UsualFunction.MakeTex(50, 50, new Color(0.8f,0f,0f,1f));


        win.Show();

    }


    private void OnEnable()
    {
        doOnce = false;
        grosBoutonRouge = EditorStyles.miniButtonMid;
        grosBoutonRouge.normal.background = redTexture;
        grosBoutonRouge.active.background = redTexture;
        grosBoutonRouge.onActive.background = redTexture;
        grosBoutonRouge.onNormal.background = redTexture;
    }



    private void OnGUI()
    {
        if (!doOnce)
        {

            for (int x = 0; x < 12; x++)
            {
                int _cpt = 0;
                for (int i = 0; i < 7; i++)
                {
                    foreach (var enemy in pattern.ennemiesPositionForEditor)
                    {
                        if (enemy.x == squarePosition[i, x].x && enemy.y == squarePosition[i,x].y)
                        {
                            _cpt++;
                            squarePosition[i, x].z = enemy.z;
                            ennemies[i, x] = squarePosition[i, x];

                        }
                        if (_cpt == 5)
                        {
                            ennemies[6, x] = ennemies[i, x] + Vector3.right * 500;
                            ennemies[6, x].z = 1;
                            squarePosition[6, x].z = 1;
                            Debug.Log(ennemies[6, x]);
                            Debug.Log(squarePosition[6, x]);
                            _cpt = 0;
                        }

                    }
                }
            }
            doOnce = true;
        }
        DrawGridCanevas();
        DrawInspector();
        DrawSquare();
        OnMousCLick();

        EditorUtility.SetDirty(pattern);
       // Repaint();
    }

    private void DrawGridCanevas()
    {


        //Draw background
        GUI.DrawTexture(ViewRect, background, ScaleMode.StretchToFill);

        //Scroll vieuw avec un arbre static pour ne pas se perdre avec les deplacement

        UsualEditorFunction.DrawGrid(25f, .25f, Color.white, ViewRect);
        UsualEditorFunction.DrawGrid(50f, .5f, Color.white, ViewRect);
        //UsualEditorFunction.DrawGrid(nodeSize.x, nodeSize.y, 50, .5f, Color.white, ViewRect);

    }

    private void DrawInspector()
    {
        inspectorRect = new Rect(position.width - inspectorWidth, 0, inspectorWidth, position.height - 20/*for better scrolling navigation*/);
        GUI.BeginGroup(inspectorRect);
        var _rectangle = EditorGUILayout.BeginVertical(GUILayout.MaxWidth(inspectorWidth/*for lisibility*/), GUILayout.MinHeight(500));
        GUI.Box(_rectangle, inspectorColor);

        scrollInspectorBar = EditorGUILayout.BeginScrollView(scrollInspectorBar);


        EditorGUILayout.Space(20);
        UsualEditorFunction.DrawButtonWithActionLayout("Tester", EditorStyles.miniButton, () => Play());
        pattern.difficulty = EditorGUILayout.IntField("Niveau de difficulté", pattern.difficulty); ;
        EditorGUILayout.HelpBox("Espèce d'abrutie de link pas plus de deux \n notes sur la meme ligne", MessageType.Warning);

        #region colorDecription
        EditorGUILayout.BeginHorizontal();
            GUILayout.Box(normalColor);
            EditorGUILayout.LabelField(": normal");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Box(teleportColor);
        EditorGUILayout.LabelField(": teleporte");
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Box(tankColor);
        EditorGUILayout.LabelField(": tank");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Box(groupedColor);
        EditorGUILayout.Space();
        GUILayout.Box(groupedColor);
        EditorGUILayout.LabelField(": co-lié");
        EditorGUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.Space();

        if(GUILayout.Button("Gros bouton rouge", grosBoutonRouge)) { GrosBoutonRouge(); }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        GUI.EndGroup();
    }
    private void DrawSquare()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int x = 0; x < 12; x++)
            {
                var _rect = new Rect(squarePosition[i, x], doteSize);
                GUI.Box(_rect, rowColor[i]);
                if (ennemies[i, x] == squarePosition[i, x])
                {

                    var _rectEnnemy = new Rect(squarePosition[i, x], doteSize);
                    switch (squarePosition[i,x].z)
                    {
                        case 1:
                             GUI.Box(_rectEnnemy,normalColor);
                            break;
                        case 2:
                            GUI.Box(_rectEnnemy, teleportColor);
                            break;
                        case 3:
                            GUI.Box(_rectEnnemy, tankColor);
                            break;
                        case 4:
                            GUI.Box(_rectEnnemy, groupedColor);
                            break;
                        default:
                            GUI.Box(_rectEnnemy, inspectorColor);
                            break;
                    }      
                }
            }
        }
    }

    private void Play()
    {

    }

    private void OnMousCLick()
    {
        mousePosition = Event.current.mousePosition;
        for (int i = 0; i < 7; i++)
        {
            for (int x = 0; x < 12; x++)
            {

                if (mousePosition.x < squarePosition[i, x].x + doteSize.x
                   && mousePosition.x > squarePosition[i, x].x
                   && mousePosition.y < squarePosition[i, x].y + doteSize.y
                   && mousePosition.y > squarePosition[i, x].y)
                {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        if (i == 6)
                        {
                            if (ennemies[i, x] != squarePosition[i, x])
                            {
                                for (int z = 0; z < 6; z++)
                                {
                                    if (ennemies[z, x] != squarePosition[z, x])
                                    {
                                        squarePosition[z, x].z++;
                                        pattern.AddEnnemy(squarePosition[z, x], z, x);
                                        ennemies[z, x] = squarePosition[z, x];

                                    }
                                }
                                squarePosition[i, x].z++;
                                ennemies[i, x] = squarePosition[i, x];
                            }
                            /*else if(ennemies[i,x].z < 3)
                            {
                                for (int z = 0; z < 6; z++)
                                {
                                    if (ennemies[z, x] != squarePosition[z, x])
                                    {
                                        pattern.AddEnnemy(squarePosition[z, x], z, x);
                                        squarePosition[z, x].z++;
                                        ennemies[z, x] = squarePosition[z, x];

                                    }
                                }
                                ennemies[i, x] = squarePosition[i, x];
                            }*/
                            else
                            {
                                for (int z = 0; z < 6; z++)
                                {
                                    pattern.RemoveEnnemy(squarePosition[z, x], z, x);
                                    ennemies[z, x] = Vector3.zero;
                                    squarePosition[z, x].z = 0;
                                }
                                ennemies[i, x] = Vector3.zero;
                                squarePosition[i, x].z = 0;
                            }
                        }
                        else
                        {
                            
                            if (ennemies[i, x] != squarePosition[i, x])
                            {
                                if (squarePosition[i, x].x == 200 && squarePosition[i, x].z == 1 || squarePosition[i, x].x == 500 && squarePosition[i, x].z == 1)
                                {
                                    squarePosition[i, x].z += 2;
                                }
                                else
                                    squarePosition[i, x].z++;

                                pattern.AddEnnemy(squarePosition[i, x], i, x);
                                ennemies[i, x] = squarePosition[i, x];
                                positionCliked = ennemies[i, x];
                                xClicked = new Vector2Int(i, x);
                            }
                            else if(ennemies[i, x].z <3)
                            {
                                pattern.RemoveEnnemy(squarePosition[i, x], i, x);
                                if (squarePosition[i, x].x == 200 && squarePosition[i, x].z == 1 || squarePosition[i, x].x == 500 && squarePosition[i, x].z == 1)
                                {
                                    squarePosition[i, x].z += 2;
                                }
                                else
                                    squarePosition[i, x].z++;
                                pattern.AddEnnemy(squarePosition[i, x], i, x);
                                ennemies[i, x] = squarePosition[i, x];
                                positionCliked = ennemies[i, x];
                                xClicked = new Vector2Int(i, x);
                            }
                            else if (ennemies[i, x].z == 4)
                            {
                                for (int z = 0; z < 6; z++)
                                {
                                    if(ennemies[z,x].z == 4)
                                    {
                                        squarePosition[z, x].z = 4;
                                        pattern.RemoveEnnemy(squarePosition[z, x], z, x);
                                        squarePosition[z, x].z = 0;
                                    }
                                }

                                    
                            }
                            else 
                            {
                                pattern.RemoveEnnemy(squarePosition[i, x], i, x);
                                squarePosition[i, x].z = 0;
                            }
                           
                            
                        }
                        break;
                    }
                    else if(Event.current.type == EventType.MouseUp)
                    {
                        if(positionCliked != Vector3.zero &&squarePosition[i,x].y == positionCliked.y && positionCliked.x != squarePosition[i,x].x)
                        {
                            pattern.RemoveEnnemy(squarePosition[xClicked.x, xClicked.y], xClicked.x, xClicked.y);
                            squarePosition[i, x].z = 4;
                            squarePosition[xClicked.x, xClicked.y].z = 4;
                            ennemies[i, x] = squarePosition[i, x];
                            ennemies[xClicked.x, xClicked.y].z = squarePosition[i, x].z;
                            pattern.AddEnnemy(squarePosition[i, x], i, x);
                            pattern.AddEnnemy(squarePosition[xClicked.x, xClicked.y], xClicked.x, xClicked.y);
                        }
                        else
                        {
                            positionCliked = Vector3.zero;
                        }
                    }
                }
            }

        }
    }

    private void GrosBoutonRouge()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int x = 0; x < 12; x++)
            {
                squarePosition[i, x].z = 0;
                ennemies[i, x] = Vector3.zero;
                pattern.RemoveAll();
            }
        }
    }
}