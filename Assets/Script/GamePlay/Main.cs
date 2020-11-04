using AudioHelm;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main : Singleton<Main>
{
    [Header("GameObject References")]
    public GameObject player;
    public TextMeshProUGUI currentPatternName;

    [Header("Sound")]
    public AudioSource clap;
    public GameObject deathBossSound;

    [Header("Spawn")]
    public GameObject[] positionEnd = new GameObject[6];
    public Patterns[] patterns = new Patterns[1];
    [HideInInspector] public List<Patterns> patterns1 = new List<Patterns>();
    [HideInInspector] public List<Patterns> patterns2 = new List<Patterns>();
    [HideInInspector] public List<Patterns> patterns3 = new List<Patterns>();
    [HideInInspector] private List<int> previousNumberPattern = new List<int>();

    public GameObject[] ennemysArray = new GameObject[5];
    public int numberOfPatternPerDifficulty = 5;

    private Vector3 previousEnnemy;
    private List<Vector3> previousEnnemyList;
    private int currentPattern;
    private int nodeNumber;
    private int emptyNode;
    public GameObject[] rowOn = new GameObject[6];
    private int patternNumber;


    public bool canShoot;
    public LineRenderer[] lineRenderer = new LineRenderer[2];
    [HideInInspector] public SpriteRenderer sprite;


    [Header("Special")]
    public Slider specialBarG;
    public Slider specialBarD;
     public int specialCount;
    public int specialMaxValue;
    public GameObject specialSpawner;
    public bool isBulletTime;
    public float maxBulletTime;
    private float bulletTimeTimer;


    [Header("Explosion")]
    public GameObject[] explosion = new GameObject [6];
    public GameObject explosionSpecial;
    public GameObject powerSupplies;

    private int doOnceCPT;


    [Header("MiniBoss")]
    private bool isBoss;
    [HideInInspector] public bool canShootMiniBoss;
    public GameObject miniBoss;
    private float miniBossDamage;
    public float miniBossTime;
    public float miniBossMaxScore;
    public int miniBossMinScore;
    private float miniBossLife;


    [Header("Boss")]
    [HideInInspector] public bool canShootBoss;
    private int phaseNumber;
    public GameObject boss;
    public GameObject[] laserBeams = new GameObject[3];
    public GameObject bigExplosion;
    private float bossLife;
    private GameObject currentBeam;
    public GameObject laserHit;
    private GameObject laserHitRef;

    //textBoss
    public TextMeshProUGUI bossHit;
    private float textTimer;

    public GameObject firstSphere;
    public GameObject SecondSphere;
    private GameObject[] spheres = new GameObject[2];

    private void Start()
    {
        
        sprite = GetComponent<SpriteRenderer>();

        lineRenderer[0].SetPosition(0, transform.position);
        lineRenderer[1].SetPosition(0, transform.position);
        lineRenderer[0].SetPosition(1, transform.position);
        lineRenderer[1].SetPosition(1, transform.position);
        specialBarG.maxValue = specialMaxValue;

        previousEnnemyList = new List<Vector3>();
        SetPatternsArray();

        foreach (Vector3 vector in patterns1[0].ennemiesPosition)
        {
            previousEnnemyList.Add(vector);
        }
        
        
        //debug to know wich pattern is playing
        currentPatternName.text = "current pattern : " +  patterns1[0].name;

        previousEnnemy = Vector2.one * 12;
    }



    private void SetPatternsArray()
    {
        foreach (var pattern in patterns)
        {
            int _difficulty = pattern.difficulty;
            switch (_difficulty)
            {
                case 1:
                    patterns1.Add(pattern);
                    break;
                case 2:
                    patterns2.Add(pattern);
                    break;

                case 3:
                    patterns3.Add(pattern);
                    break;
                default:
                    break;
            }
        }
    }
    private void Update()
    {
        specialBarD.value = specialBarG.value;
        specialBarD.maxValue = specialBarG.maxValue;
         if(canShootBoss || canShootMiniBoss)
        {
            
            textTimer += Time.deltaTime;
            if (textTimer >= 0.05f && textTimer<=0.01f)
            {
                bossHit.fontSize = bossHit.fontSize * 1.2f;
            }
            else if(textTimer>= 0.01f)
            {
                textTimer = 0;
                bossHit.fontSize = bossHit.fontSize * 0.8f;
            }
        }


        if (isBulletTime)
        {
            bulletTimeTimer += Time.deltaTime;
            if (bulletTimeTimer >= maxBulletTime)
            {
                isBulletTime = false;
                bulletTimeTimer = 0;
            }
            if (Input.anyKeyDown && SoundDisplay.Instance.ennemys.Count > 0)
            {

                GameObject _ennemy = SoundDisplay.Instance.ennemys[SoundDisplay.Instance.ennemys.Count - 1];
                int _ennemyParent = 0;
                for (int i = 0; i < positionEnd.Length; i++)
                {
                    if (positionEnd[i] == _ennemy.transform.parent.gameObject)
                    {
                        _ennemyParent = i;
                        break;
                    }
                }
                lineRenderer[0].SetPosition(1, _ennemy.transform.position);
                clap.Play();
                StartCoroutine(LaserFade(0,100));
                if (_ennemy.tag == "LinkedEnnemy")
                {
                    _ennemy.GetComponent<LinkedEnnemy>().DestroyAll();
                }
                else
                    SoundDisplay.Instance.RemoveEnnemy(_ennemy);
                Score.Instance.ScoreUp(_ennemy.GetComponent<EnnemyBehavior>().scoreValue);
                Destroy(_ennemy);
                StartCoroutine(RowFade(rowOn[_ennemyParent]));
                if (SoundDisplay.Instance.ennemys.Count == 0)
                {

                    isBulletTime = false;
                }
            }
            else if (SoundDisplay.Instance.ennemys.Count == 0)
            {
                isBulletTime = false;
            }
        }
        else if (canShootMiniBoss)
        {
            bossHit.color = new Color(miniBossDamage / miniBossLife, 0, 0,1);
 
            RaycastHit hit;
            Physics.Raycast(SoundDisplay.Instance.heart.transform.position, Vector3.forward * 1000, out hit ,1000,1 <<19);
            if (hit.collider != null)
            {
                laserHitRef.transform.position = hit.point;
            }
            else
                Debug.Log("yo");
            if(Input.GetKeyDown(KeyCode.E) )
            {                
                StartCoroutine(RowFade(rowOn[2]));
                positionEnd[2].GetComponent<Spawner>().Shoot();
                miniBossDamage++;
                bossHit.text = miniBossDamage.ToString();
                bossHit.fontSize = 150+miniBossDamage*0.5f ;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RowFade(rowOn[3]));
                positionEnd[3].GetComponent<Spawner>().Shoot();
                miniBossDamage++;
                bossHit.text = miniBossDamage.ToString();
                bossHit.fontSize = 150 + miniBossDamage * 0.5f;
            }
           if( miniBossDamage> miniBossLife / 3)
            {
                Destroy(currentBeam);
                spheres[0].transform.localScale = new Vector3(0.035f,0.035f,0.035f);
                currentBeam = Instantiate(laserBeams[1], SoundDisplay.Instance.heart.transform.position, Quaternion.identity);
            }
           else if (miniBossDamage > miniBossLife / 3 * 2)
            {
                Destroy(currentBeam);
                spheres[0].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
                spheres[1] = Instantiate(SecondSphere, SoundDisplay.Instance.heart.transform);
                currentBeam = Instantiate(laserBeams[2], SoundDisplay.Instance.heart.transform.position, Quaternion.identity);
            }

        }
        else if (canShootBoss)
        {
            bossHit.color = new Color(miniBossDamage / bossLife, 0, 1);
            RaycastHit hit;
            Physics.Raycast(SoundDisplay.Instance.heart.transform.position, Vector3.forward * 1000, out hit, 1000, 1 << 19);
            if (hit.collider != null)
            {
                laserHitRef.transform.position = hit.point;
            }
            else
                Debug.Log("yo");
            switch (phaseNumber)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        positionEnd[2].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        positionEnd[3].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        positionEnd[1].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    else if (Input.GetKeyDown(KeyCode.T))
                    {
                        positionEnd[4].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        positionEnd[0].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        positionEnd[2].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        positionEnd[1].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        positionEnd[3].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    break;
                case 4:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        positionEnd[2].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    else if (Input.GetKeyDown(KeyCode.H))
                    {
                        positionEnd[5].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    break;
                case 5:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        positionEnd[0].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    else if (Input.GetKeyDown(KeyCode.T))
                    {
                        positionEnd[4].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                        bossHit.text = miniBossDamage.ToString();
                        bossHit.fontSize = 150 + miniBossDamage * 0.5f;
                    }
                    break;
                case 6:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        positionEnd[2].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[2]));
                        miniBossDamage++;
                    }
                    else if (Input.GetKeyDown(KeyCode.R))
                    {
                        positionEnd[3].GetComponent<Spawner>().Shoot();
                        StartCoroutine(RowFade(rowOn[3]));
                        miniBossDamage++;
                    }
                    break;

                default:
                    break;
            }
            if (miniBossDamage > bossLife / 3 && miniBossDamage<bossLife/3*2)
            {
                Destroy(currentBeam);
                spheres[0].transform.localScale = new Vector3(0.035f,0.035f,0.035f);
                currentBeam = Instantiate(laserBeams[1], SoundDisplay.Instance.heart.transform.position, Quaternion.identity);
            }
            else if (miniBossDamage > bossLife / 3 * 2)
            {
                Destroy(currentBeam);
                spheres[1] = Instantiate(SecondSphere, SoundDisplay.Instance.heart.transform);
                spheres[0].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
                currentBeam = Instantiate(laserBeams[2], SoundDisplay.Instance.heart.transform.position, Quaternion.identity);
            }

            Debug.Log(miniBossDamage);
        }
        else if (Input.anyKeyDown)
        {
            if (canShoot)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                    Shoot(positionEnd[0].transform.position, 0);
                else if (Input.GetKeyDown(KeyCode.Z))
                    Shoot(positionEnd[1].transform.position, 1);
                else if (Input.GetKeyDown(KeyCode.E))
                    Shoot(positionEnd[2].transform.position, 2);
                else if (Input.GetKeyDown(KeyCode.R))
                    Shoot(positionEnd[3].transform.position, 3);
                else if (Input.GetKeyDown(KeyCode.T))
                    Shoot(positionEnd[4].transform.position, 4);
                else if (Input.GetKeyDown(KeyCode.H))
                    Shoot(positionEnd[5].transform.position, 5);
                else if (Input.GetKeyDown(KeyCode.Space))
                    Shoot(specialSpawner.transform.position, true);
            }
            else
                Score.Instance.ScoreDown(1000);
        }
    }

    #region shoots
    void Shoot(Vector3 position, int _rowOn)
    {
        doOnceCPT++;
        var _cpt = doOnceCPT;
        if(doOnceCPT >=2)
        {
            doOnceCPT = 0;
            canShoot = false;
        }
        RaycastHit hit;
        Physics.Raycast(new Vector3(position.x, position.y-2.5f, position.z-13), new Vector3(0,2.5f,15),out hit );
        StartCoroutine(RowFade(rowOn[_rowOn]));
        if (hit.collider != null )
        {
            if(hit.collider.gameObject.tag == "Ennemy" || hit.collider.gameObject.tag == "TPEnnemy")
            {
                lineRenderer[_cpt - 1].SetPosition(1, hit.transform.position);
                clap.Play();
                hit.collider.gameObject.GetComponent<EnnemyBehavior>().Destroyed(2-doOnceCPT);
                StartCoroutine(LaserFade(_cpt - 1,100));
               //  LEDSManager.Instance.LightUp(_rowOn);

            }
            else if(hit.collider.gameObject.tag == "LinkedEnnemy")
            {
                lineRenderer[_cpt - 1].SetPosition(1, hit.transform.position);
                clap.Play();
                StartCoroutine(LaserFade(_cpt - 1,100));
                //LEDSManager.Instance.LightUp(_rowOn);
                hit.collider.GetComponentInParent<LinkedEnnemy>().Hitted();
             
            }
        }
    }
    /// <summary>
    /// shoot the special
    /// </summary>
    /// <param name="position"></param>
    /// <param name="isSpecial"></param>
    void Shoot(Vector3 position, bool isSpecial)
    {
        canShoot = false;
        RaycastHit hit;
        Physics.Raycast(transform.position, new Vector3(position.x - transform.position.x, position.y - transform.position.y, position.z - transform.position.z), out hit);
        StartCoroutine(RowFade(isSpecial));
        if (specialCount >= specialMaxValue)
        {
            StartCoroutine(BulletTime());
        }
        else if (hit.collider != null && hit.collider.gameObject.tag == "SpecialEnnemy")
        {
            lineRenderer[0].SetPosition(1, hit.transform.position);
            clap.Play();
            Instantiate(explosionSpecial, hit.collider.transform.position, Quaternion.identity);
            Instantiate(powerSupplies, hit.collider.transform.position, Quaternion.identity);
            StartCoroutine(LaserFade(0,100));
            Score.Instance.ScoreUp(hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue);
            GameObject poptext = Instantiate(hit.collider.gameObject.GetComponent<EnnemyBehavior>().popTextScore, hit.collider.gameObject.GetComponent<EnnemyBehavior>().poptextPosition.transform.position, Quaternion.identity);
            poptext.GetComponent<TextMeshPro>().text = hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue.ToString();
            SoundDisplay.Instance.RemoveEnnemy(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
    }

    IEnumerator LaserFade(int cpt, int timer)
    {
        for (int i = 0; i < timer * 60/AudioHelmClock.Instance.bpm; i++)
        {
            lineRenderer[cpt].startColor -= new Color(0, 0, 0, 0.01f);
            lineRenderer[cpt].endColor -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.005f);

        }
        lineRenderer[cpt].startColor += new Color(0, 0, 0, 1);
        lineRenderer[cpt].endColor -= new Color(0, 0, 0, 1);
        lineRenderer[cpt].SetPosition(1, transform.position);
    }
    IEnumerator RowFade(GameObject rowSprite)
    {
        rowSprite.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        rowSprite.SetActive(false);
    }
    /// <summary>
    /// row fade for special
    /// </summary>
    /// <param name="isSpecial"></param>
    /// <returns></returns>
    IEnumerator RowFade(bool isSpecial)
    {
        foreach (var row in rowOn)
        {
            row.SetActive(true);
            row.GetComponent<AudioSource>().Play();
        }
        yield return new WaitForSeconds(0.3f);
        foreach (var row in rowOn)
        {
            row.SetActive(false);
        }
    }
    #endregion

    private bool breakSpawn;
    public void Spawn()
    {
        if (!isBoss) 
        {

            breakSpawn = false;
            float biggest = 0;
            List<int> numberForEnnemy = new List<int>();

            //go to the next pattern if not already done

            if (nodeNumber == 12)
            {
                nodeNumber = 0;
                #region patternDifficulty
                var _currentPattern = new List<Patterns>();
                if (patternNumber >= patterns.Length - 1)
                    return;
                else if (patternNumber < numberOfPatternPerDifficulty - 1)
                    _currentPattern = patterns1;
                else if (patternNumber >= numberOfPatternPerDifficulty - 1 && patternNumber < (numberOfPatternPerDifficulty - 1) * 2)
                {
                    _currentPattern = patterns2;
                }
                else if (patternNumber < patterns.Length - 1)
                    _currentPattern = patterns3;
                else
                {
                    Debug.Log("c'est fini");
                    return;
                }

                if (_currentPattern.Count == 0)
                    return;

                #endregion
                // number of pattern befor miniBoss
                if (patternNumber == 5)
                {
                    StartCoroutine(MiniBoss());
                }
                //number of pattern befor boss
                else if( patternNumber == 19)
                {
                    StartCoroutine(Boss());
                }

                previousNumberPattern.Add(currentPattern);
                currentPattern = Random.Range(0, _currentPattern.Count);
                foreach (var number in previousNumberPattern)
                {
                    while (currentPattern == number)
                        currentPattern = Random.Range(0, _currentPattern.Count);

                }

                patternNumber++;
                if (patternNumber == numberOfPatternPerDifficulty - 1 || patternNumber == (numberOfPatternPerDifficulty - 1) * 2)
                {
                    previousNumberPattern.Clear();
                }
                foreach (var vecor in _currentPattern[currentPattern].ennemiesPosition)
                {
                    previousEnnemyList.Add(vecor);
                }

                //debug to know wich pattern is playing
                currentPatternName.text = "current pattern : " + _currentPattern[currentPattern].name;


                for (int i = 0; i < previousEnnemyList.Count; i++)
                {
                    if (previousEnnemyList[i].y > biggest)
                    {
                        biggest = previousEnnemyList[i].y;
                    }

                }

                previousEnnemy.y = 12;
            }

            for (int i = 0; i < previousEnnemyList.Count; i++)
            {
                if (previousEnnemyList[i].y > biggest)
                {
                    biggest = previousEnnemyList[i].y;
                }

            }
            for (int i = 0; i < previousEnnemyList.Count; i++)
            {
                if (previousEnnemyList[i].y == biggest)
                {
                    numberForEnnemy.Add(i);
                }

            }

            if (biggest + emptyNode == previousEnnemy.y - 1)
            {
                emptyNode = 0;
                if (numberForEnnemy.Count < 6)
                {
                    for (int i = 0; i < numberForEnnemy.Count; i++)
                    {
                        var _ennemy = previousEnnemyList[numberForEnnemy[i]].z;

                        switch (_ennemy)
                        {
                            case 1:
                                positionEnd[(int)previousEnnemyList[numberForEnnemy[i]].x].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                                break;
                            case 2:
                                TpSpawn(previousEnnemyList[numberForEnnemy[i]].x, (int)previousEnnemyList[numberForEnnemy[i]].x);
                                break;
                            case 3:
                                positionEnd[(int)previousEnnemyList[numberForEnnemy[i]].x].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                                break;
                            case 4:
                                LinkedSpawn(previousEnnemyList[numberForEnnemy[i]].x, numberForEnnemy, previousEnnemyList[numberForEnnemy[i]], (int)previousEnnemyList[numberForEnnemy[i]].x);
                                break;

                            default:
                                Debug.Log(_ennemy);
                                break;
                        }
                        if (breakSpawn)
                            break;
                        previousEnnemy = previousEnnemyList[numberForEnnemy[i]];
                    }
                }
                else
                {
                    for (int i = 0; i < numberForEnnemy.Count; i++)
                    {
                        previousEnnemy = previousEnnemyList[numberForEnnemy[i]];
                    }
                    SpecialSpawn();
                }
                for (int i = 0; i < numberForEnnemy.Count; i++)
                {

                    previousEnnemyList.RemoveAt(numberForEnnemy[i] - i);
                }
            }
            else
                emptyNode++;

            nodeNumber++;
        }
    }

    public void SpecialSpawn()
    {
        specialSpawner.GetComponent<Spawner>().Spwan(true);

    }

    private void TpSpawn(float x, int listEnnemy)
    {
        if (x == 0 || x == 3)
        {

            positionEnd[listEnnemy].GetComponent<Spawner>().Spwan(ennemysArray[1], positionEnd[listEnnemy + 2].GetComponent<Spawner>().positions, positionEnd[listEnnemy + 2].GetComponent<Spawner>().currentColor);
        }
        else if (x == 2 || x == 5)
        {
            positionEnd[listEnnemy].GetComponent<Spawner>().Spwan(ennemysArray[1], positionEnd[listEnnemy - 2].GetComponent<Spawner>().positions, positionEnd[listEnnemy - 2].GetComponent<Spawner>().currentColor);
        }
        else 
            Debug.Log(x);
    }
    private void LinkedSpawn(float x, List<int> listEnnemy, Vector3 current, int currentInt)
    {
        int secondSpawn =1000;
        for (int i = 0; i < listEnnemy.Count; i++)
        {
            if(previousEnnemyList[ listEnnemy[i]].z == 4 && previousEnnemyList[listEnnemy[i]].y == current.y)
            {
                secondSpawn = i;
            }
            
        }
        if (secondSpawn != 1000)
        {
            specialSpawner.GetComponent<Spawner>().Spwan(ennemysArray[2], positionEnd[currentInt].GetComponent<Spawner>(), positionEnd[(int)previousEnnemyList[listEnnemy[secondSpawn]].x].GetComponent<Spawner>());
            breakSpawn = true;
        }
    }
    IEnumerator BulletTime()
    {
        isBulletTime = true;
        specialCount = 0;
        specialBarG.value = 0;
        SoundDisplay.Instance.speedModifier = 0;
        SoundDisplay.Instance.bpmVisuelD.fillAmount = 0;
        SoundDisplay.Instance.bpmVisuelG.fillAmount = 0;
        SoundDisplay.Instance.cantMove = true;
        yield return new WaitWhile(() => isBulletTime == true);
        SoundDisplay.Instance.speedModifier = 1;
        SoundDisplay.Instance.cantMove = false;

    }

    public void CanShoot()
    {
        if (SoundDisplay.Instance.doOnce)
        {
            canShoot = true;
        }
      
    }

    public void CantShoot()
    {
        canShoot = false;
        doOnceCPT = 0;
        SoundDisplay.Instance.doOnce = true;
    }


    IEnumerator MiniBoss()
    {
        isBoss = true;
        miniBossDamage = 0;
        yield return new WaitUntil(() => SoundDisplay.Instance.ennemys.Count == 0f);
        yield return new WaitForSeconds(2f);
        //instantiate the mini boss in the middle of the scene
        SoundManager.Instance.BossEntry();
        var _miniBoss = Instantiate(miniBoss, Vector3.forward * 1000, Quaternion.identity);
        miniBossLife = _miniBoss.GetComponent<MiniBossMovement>().life;
        laserHitRef = Instantiate(laserHit, Vector3.back*1000, Quaternion.identity);

        yield return new WaitForSeconds(4.5f);
        for (int i = 0; i < positionEnd.Length; i++)
        {
            if (i == 2 || i == 3)
            {
                positionEnd[i].GetComponent<Spawner>().LeaveOne();
            }
            else
                positionEnd[i].GetComponent<Spawner>().DesaapppearAll();

        }
        yield return new WaitUntil(() => canShootMiniBoss == true);
        textTimer = 0;
        bossHit.enabled = true;
        spheres[0] = Instantiate(firstSphere, SoundDisplay.Instance.heart.transform);
        SoundDisplay.Instance.isBoss = true;
        currentBeam = Instantiate(laserBeams[0], SoundDisplay.Instance.heart.transform.position, Quaternion.identity);
        

    }
    IEnumerator Boss()
    {
        isBoss = true;
        miniBossDamage = 0;
        yield return new WaitUntil(() => SoundDisplay.Instance.ennemys.Count == 0f);
        yield return new WaitForSeconds(2f);
        //instantiate the mini boss in the middle of the scene
        SoundManager.Instance.BossEntry();
        var _Boss = Instantiate(boss, Vector3.forward * 1000, Quaternion.identity);
        bossLife = _Boss.GetComponent<BossMovemenet>().life;
        laserHitRef = Instantiate(laserHit, Vector3.back*10000, Quaternion.identity);
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < positionEnd.Length; i++)
        {
            if (i == 2 || i == 3)
            {
                positionEnd[i].GetComponent<Spawner>().LeaveOne();
            }
            else
                positionEnd[i].GetComponent<Spawner>().DesaapppearAll();

        }
        yield return new WaitUntil(() => canShootBoss == true);
        textTimer = 0;
        bossHit.enabled = true;
        SoundDisplay.Instance.isBoss = true;
        currentBeam = Instantiate(laserBeams[0], SoundDisplay.Instance.heart.transform.position, Quaternion.identity);
        spheres[0] = Instantiate(firstSphere, SoundDisplay.Instance.heart.transform);
        canShootBoss = true;


    }
    public void BossPhaseUp()
    {
        phaseNumber++;
        switch (phaseNumber)
        {      
            case 1:
                positionEnd[1].GetComponent<Spawner>().AppeartOne();
                positionEnd[4].GetComponent<Spawner>().AppeartOne();
                positionEnd[2].GetComponent<Spawner>().DesaapppearAll();
                positionEnd[3].GetComponent<Spawner>().DesaapppearAll();
                break;
            case 2:
                positionEnd[0].GetComponent<Spawner>().AppeartOne();
                positionEnd[2].GetComponent<Spawner>().AppeartOne();
                positionEnd[1].GetComponent<Spawner>().DesaapppearAll();
                positionEnd[4].GetComponent<Spawner>().DesaapppearAll();
                break;
            case 3:
                positionEnd[1].GetComponent<Spawner>().AppeartOne();
                positionEnd[3].GetComponent<Spawner>().AppeartOne();
                positionEnd[2].GetComponent<Spawner>().DesaapppearAll();
                positionEnd[0].GetComponent<Spawner>().DesaapppearAll();
                break;
            case 4:
                positionEnd[2].GetComponent<Spawner>().AppeartOne();
                positionEnd[5].GetComponent<Spawner>().AppeartOne();
                positionEnd[1].GetComponent<Spawner>().DesaapppearAll();
                positionEnd[3].GetComponent<Spawner>().DesaapppearAll();
                break;
            case 5:
                positionEnd[0].GetComponent<Spawner>().AppeartOne();
                positionEnd[4].GetComponent<Spawner>().AppeartOne();
                positionEnd[2].GetComponent<Spawner>().DesaapppearAll();
                positionEnd[5].GetComponent<Spawner>().DesaapppearAll();
                break;
            case 6:
                positionEnd[2].GetComponent<Spawner>().AppeartOne();
                positionEnd[3].GetComponent<Spawner>().AppeartOne();
                positionEnd[0].GetComponent<Spawner>().DesaapppearAll();
                positionEnd[4].GetComponent<Spawner>().DesaapppearAll();
                break;
            default:
                break;
        }
        
    }
    IEnumerator StartAfterMiniBoss()
    {
        GameObject DeadSound = Instantiate(deathBossSound, transform.position, Quaternion.identity);
        Destroy(DeadSound, 8f);
        // wait for the player to calm down
        SoundDisplay.Instance.isBoss = false;
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < positionEnd.Length; i++)
        {
            positionEnd[i].GetComponent<Spawner>().ApppearAll();

        }
        yield return new WaitForSeconds(1f);
        bossHit.text = string.Empty;
        bossHit.fontSize = 150;
        bossHit.enabled = false;
        isBoss = false;
        
        EnvironnementManager.Instance.LunchPhase();
    }


    public void BossOverTest(int life, GameObject miniBoss)
    {
        if (miniBossDamage >= life)
        {
            canShootMiniBoss = false;
            Score.Instance.ScoreUp((int)miniBossDamage * 100);
            canShootMiniBoss = false;
            Destroy(currentBeam);
            Destroy(laserHitRef);
            
            foreach (GameObject sphere in spheres)
            {
                Destroy(sphere);
            }
            Instantiate(bigExplosion, miniBoss.transform.position, Quaternion.identity);
            SoundManager.Instance.UpdateVolume(Score.Instance.scorMultiplier);
            Destroy(miniBoss);
           
            if (phaseNumber <= 1)
            {
                StartCoroutine(StartAfterMiniBoss());
            }
            else
            {
                Score.Instance.EndScene(true);
            }

            
        }
        else
        {
            Score.Instance.EndScene(false);
        }
    }
}



public abstract class Singleton<T> : MonoBehaviour where T : Component
{

    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    private static T instance;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
     /*   else
        {
            Debug.Log("yo");
            Destroy(gameObject);
        }*/
    }

    #endregion

}