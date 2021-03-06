﻿using System.Collections;
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

    /*Vector2[,] squarePosition = new Vector2[6, 12] {
                    {new Vector2(75,173),new Vector2(100,210),new Vector2(125,237),new Vector2(150,275),new Vector2(175,313),new Vector2(200,350),new Vector2(225,387),new Vector2(250,425),new Vector2(275,462),new Vector2(300,500),new Vector2(325,537),new Vector2(350,575)},
                    {new Vector2(185,125),new Vector2(200,150),new Vector2(225,200),new Vector2(250,250),new Vector2(275,287),new Vector2(300,325),new Vector2(325,375),new Vector2(350,425),new Vector2(367,462),new Vector2(375,500),new Vector2(383,525),new Vector2(400,550)},
                    {new Vector2(312,75),new Vector2(325,100),new Vector2(337,137),new Vector2(350,175),new Vector2(367,212),new Vector2(375,250),new Vector2(387,287),new Vector2(400,325),new Vector2(412,375),new Vector2(425,425),new Vector2(437,475),new Vector2(450,525)},
                    {new Vector2(662,75),new Vector2(650,100),new Vector2(637,137),new Vector2(625,175),new Vector2(612,212),new Vector2(600,250),new Vector2(587,287),new Vector2(575,325),new Vector2(562,375),new Vector2(550,425),new Vector2(537,475),new Vector2(525,525)},
                    {new Vector2(785,125),new Vector2(775,150),new Vector2(750,200),new Vector2(725,250),new Vector2(712,287),new Vector2(700,325),new Vector2(675,375),new Vector2(650,425),new Vector2(637,462),new Vector2(625,500),new Vector2(612,525),new Vector2(600,550)},
                    {new Vector2(900,173),new Vector2(875,210),new Vector2(850,237),new Vector2(825,275),new Vector2(800,313),new Vector2(775,350),new Vector2(750,387),new Vector2(725,425),new Vector2(712,462),new Vector2(700,500),new Vector2(687,537),new Vector2(675,575)},
    };*/
    Vector2[,] squarePosition = new Vector2[6, 12] {
                    {new Vector2(100,50),new Vector2(100,100),new Vector2(100,150),new Vector2(100,200),new Vector2(100,250),new Vector2(100,300),new Vector2(100,350),new Vector2(100,400),new Vector2(100,450),new Vector2(100,500),new Vector2(100,550),new Vector2(100,600)},
                    {new Vector2(200,50),new Vector2(200,100),new Vector2(200,150),new Vector2(200,200),new Vector2(200,250),new Vector2(200,300),new Vector2(200,350),new Vector2(200,400),new Vector2(200,450),new Vector2(200,500),new Vector2(200,550),new Vector2(200,600)},
                    {new Vector2(300,50),new Vector2(300,100),new Vector2(300,150),new Vector2(300,200),new Vector2(300,250),new Vector2(300,300),new Vector2(300,350),new Vector2(300,400),new Vector2(300,450),new Vector2(300,500),new Vector2(300,550),new Vector2(300,600)},
                    {new Vector2(400,50),new Vector2(400,100),new Vector2(400,150),new Vector2(400,200),new Vector2(400,250),new Vector2(400,300),new Vector2(400,350),new Vector2(400,400),new Vector2(400,450),new Vector2(400,500),new Vector2(400,550),new Vector2(400,600)},
                    {new Vector2(500,50),new Vector2(500,100),new Vector2(500,150),new Vector2(500,200),new Vector2(500,250),new Vector2(500,300),new Vector2(500,350),new Vector2(500,400),new Vector2(500,450),new Vector2(500,500),new Vector2(500,550),new Vector2(500,600)},
                    {new Vector2(600,50),new Vector2(600,100),new Vector2(600,150),new Vector2(600,200),new Vector2(600,250),new Vector2(600,300),new Vector2(600,350),new Vector2(600,400),new Vector2(600,450),new Vector2(600,500),new Vector2(600,550),new Vector2(600,600)},
    };
    Vector2[,] ennemies = new Vector2[6, 12];
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
    static Texture2D background, inspectorColor;
    static Texture2D[] rowColor = new Texture2D[6];

    private bool doOnce;
             
    public  void Init (in Patterns pattern)
        {

        this.pattern = pattern;
          var win =  EditorWindow.GetWindow(typeof(PaternEditorWIndow));
          background = UsualFunction.MakeTex(2, 2, Color.black);
          inspectorColor = UsualFunction.MakeTex(10000, 10000, Color.gray);
       
        //attention c'est dégeu mais j'ai la flemme
        rowColor[0] = UsualFunction.MakeTex(100, 100, Color.green);
        rowColor[1] = UsualFunction.MakeTex(100, 100, Color.red);
        rowColor[2] = UsualFunction.MakeTex(100, 100, new Color(0.27f, 0.20f, 0.15f));
        rowColor[3] = UsualFunction.MakeTex(100, 100, Color.yellow);
        rowColor[4] = UsualFunction.MakeTex(100, 100, Color.blue);
        rowColor[5] = UsualFunction.MakeTex(100, 100, new Color(0.35f,0.25f,0.5f ));
            
                win.Show();
                //win.minSize = new Vector2(x,y);
                //win.maxSize = new Vector2(x,y);
        }


    private void OnEnable()
    {
        doOnce = false;
    }



    private void OnGUI()
    {
        if (!doOnce)
        {
            foreach (var enemy in pattern.ennemiesPositionForEditor)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int x = 0; x < 12; x++)
                    {
                        if (enemy == squarePosition[i, x])
                        {
                            ennemies[i, x] = squarePosition[i, x];
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
        Repaint();
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
        var _rectangle= EditorGUILayout.BeginVertical(GUILayout.MaxWidth(inspectorWidth - 10/*for lisibility*/), GUILayout.MinHeight(500));
        GUI.Box(_rectangle,inspectorColor);

        scrollInspectorBar = EditorGUILayout.BeginScrollView(scrollInspectorBar);


        EditorGUILayout.Space(20);
        UsualEditorFunction.DrawButtonWithActionLayout("Tester", EditorStyles.miniButton, () => Play());
        pattern.difficulty = EditorGUILayout.IntField("Niveau de difficulté",pattern.difficulty); ;

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        GUI.EndGroup();
    }
    private void DrawSquare()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int x = 0; x < 12; x++)
            {
                var _rect = new Rect(squarePosition[i,x], doteSize);
                GUI.Box(_rect, rowColor[i]);
                if (ennemies[i, x] == squarePosition[i, x])
                {
                    var _rectEnnemy = new Rect(squarePosition[i, x], doteSize);                    
                    GUI.Box(_rectEnnemy, inspectorColor);
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
        for (int i = 0; i < 6; i++)
        {
            for (int x = 0; x < 12; x++)
            {
                if (mousePosition.x < squarePosition[i,x].x + doteSize.x
                   && mousePosition.x > squarePosition[i, x].x
                   && mousePosition.y < squarePosition[i, x].y + doteSize.y
                   && mousePosition.y > squarePosition[i, x].y )
                {
                    if (Event.current.type == EventType.MouseDown)
                    {

                        if (ennemies[i,x] != squarePosition[i,x])
                        {
                            pattern.AddEnnemy(squarePosition[i, x], i,x);
                            ennemies[i, x] = squarePosition[i, x];                            
                        }
                        else
                        {
                            pattern.RemoveEnnemy(squarePosition[i, x],i,x);
                            ennemies[i, x] = Vector2.zero;

                        }
                        break;
                    }
                }
            }
            

        } 
        
            
        
    }
}