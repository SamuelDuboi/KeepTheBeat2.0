using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AudioHelm;
using UnityEngine.UI;


public class MainTuto : Singleton<MainTuto>
{
    [Header("GameObject References")]
    public GameObject player;
    public TextMeshProUGUI currentPatternName;

    [Header("Sound")]
    public AudioSource clap;

    [Header("Spawn")]
    public GameObject[] positionEnd = new GameObject[6];
    public Patterns[] patterns = new Patterns[1];

    public GameObject[] ennemysArray = new GameObject[5];

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
    public GameObject[] explosion = new GameObject[6];
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


    public LEDSManager LEDSManager;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        lineRenderer[0].SetPosition(0, transform.position);
        lineRenderer[1].SetPosition(0, transform.position);
        lineRenderer[0].SetPosition(1, transform.position);
        lineRenderer[1].SetPosition(1, transform.position);
        specialBarG.maxValue = specialMaxValue;

        previousEnnemyList = new List<Vector3>();


        previousEnnemy = Vector2.one * 12;
    }



    private void Update()
    {
        specialBarD.value = specialBarG.value;
        specialBarD.maxValue = specialBarG.maxValue;
        if (canShootBoss || canShootMiniBoss)
        {

            textTimer += Time.deltaTime;
            if (textTimer >= 0.05f && textTimer <= 0.01f)
            {
                bossHit.fontSize = bossHit.fontSize * 1.2f;
            }
            else if (textTimer >= 0.01f)
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
            if (Input.anyKeyDown && SoundDisplqyTuto.Instance.ennemys.Count > 0)
            {

                GameObject _ennemy = SoundDisplqyTuto.Instance.ennemys[SoundDisplqyTuto.Instance.ennemys.Count - 1];
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
                StartCoroutine(LaserFade(0, 100));
                if (_ennemy.tag == "LinkedEnnemy")
                {
                    _ennemy.GetComponent<LinkedEnnemy>().DestroyAll(true);
                }
                else if (_ennemy.tag == "SpecialEnnemy")
                {
                    Score.Instance.ScoreUp(_ennemy.GetComponent<EnnemyBehavior>().scoreValue);
                    SoundDisplqyTuto.Instance.RemoveEnnemy(_ennemy);
                    Destroy(_ennemy);
                }
                else
                    _ennemy.GetComponent<EnnemyBehavior>().Destroyed(1, true);

                StartCoroutine(RowFade(rowOn[_ennemyParent]));
                if (SoundDisplqyTuto.Instance.ennemys.Count == 0)
                {

                    isBulletTime = false;
                }
            }
            else if (SoundDisplqyTuto.Instance.ennemys.Count == 0)
            {
                isBulletTime = false;
            }
        }
        else if (canShootMiniBoss)
        {
            bossHit.color = new Color(miniBossDamage / miniBossLife, 0, 0, 1);

            RaycastHit hit;
            Physics.Raycast(SoundDisplqyTuto.Instance.heart.transform.position, Vector3.forward * 1000, out hit, 1000, 1 << 19);
            if (hit.collider != null)
            {
                laserHitRef.transform.position = hit.point;
            }
            else
                Debug.Log("yo");
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(RowFade(rowOn[2]));
                positionEnd[2].GetComponent<Spawner>().Shoot();
                miniBossDamage++;
                bossHit.text = miniBossDamage.ToString();
                bossHit.fontSize = 150 + miniBossDamage * 0.5f;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(RowFade(rowOn[3]));
                positionEnd[3].GetComponent<Spawner>().Shoot();
                miniBossDamage++;
                bossHit.text = miniBossDamage.ToString();
                bossHit.fontSize = 150 + miniBossDamage * 0.5f;
            }
            if (miniBossDamage > miniBossLife / 3)
            {
                Destroy(currentBeam);
                spheres[0].transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
                currentBeam = Instantiate(laserBeams[1], SoundDisplqyTuto.Instance.heart.transform.position, Quaternion.identity);
            }
            else if (miniBossDamage > miniBossLife / 3 * 2)
            {
                Destroy(currentBeam);
                spheres[0].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
                spheres[1] = Instantiate(SecondSphere, SoundDisplqyTuto.Instance.heart.transform);
                currentBeam = Instantiate(laserBeams[2], SoundDisplqyTuto.Instance.heart.transform.position, Quaternion.identity);
            }

        }
        else if (canShootBoss)
        {
            bossHit.color = new Color(miniBossDamage / bossLife, 0, 1);
            RaycastHit hit;
            Physics.Raycast(SoundDisplqyTuto.Instance.heart.transform.position, Vector3.forward * 1000, out hit, 1000, 1 << 19);
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
            if (miniBossDamage > bossLife / 3 && miniBossDamage < bossLife / 3 * 2)
            {
                Destroy(currentBeam);
                spheres[0].transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
                currentBeam = Instantiate(laserBeams[1], SoundDisplqyTuto.Instance.heart.transform.position, Quaternion.identity);
            }
            else if (miniBossDamage > bossLife / 3 * 2)
            {
                Destroy(currentBeam);
                spheres[1] = Instantiate(SecondSphere, SoundDisplqyTuto.Instance.heart.transform);
                spheres[0].transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
                currentBeam = Instantiate(laserBeams[2], SoundDisplqyTuto.Instance.heart.transform.position, Quaternion.identity);
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
        LEDSManager.LightUp(_rowOn);
        var _cpt = doOnceCPT;
        if (doOnceCPT == 2)
        {
            doOnceCPT = 0;
            canShoot = false;
        }
        RaycastHit hit;
        Physics.Raycast(new Vector3(position.x, position.y - 2.5f, position.z - 13), new Vector3(0, 2.5f, 15), out hit);
        StartCoroutine(RowFade(rowOn[_rowOn]));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Ennemy" || hit.collider.gameObject.tag == "TPEnnemy")
            {
                lineRenderer[_cpt - 1].SetPosition(1, hit.transform.position);
                clap.Play();
                hit.collider.gameObject.GetComponent<EnnemyBehavior>().Destroyed(2 - doOnceCPT, true);
                StartCoroutine(LaserFade(_cpt - 1, 100));
                //  LEDSManager.Instance.LightUp(_rowOn);

            }
            else if (hit.collider.gameObject.tag == "LinkedEnnemy")
            {
                lineRenderer[_cpt - 1].SetPosition(1, hit.transform.position);
                clap.Play();
                StartCoroutine(LaserFade(_cpt - 1, 100));
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
        LEDSManager.LightUp(6);
        RaycastHit hit;
        Physics.Raycast(transform.position, new Vector3(position.x - transform.position.x, position.y - transform.position.y, position.z - transform.position.z), out hit);
        StartCoroutine(RowFade(isSpecial));
        if (specialCount >= specialMaxValue && canShootSpecial)
        {
            StartCoroutine(BulletTime());
        }
        else if (hit.collider != null && hit.collider.gameObject.tag == "SpecialEnnemy")
        {
            lineRenderer[0].SetPosition(1, hit.transform.position);
            clap.Play();
            Instantiate(explosionSpecial, hit.collider.transform.position, Quaternion.identity);
            Instantiate(powerSupplies, hit.collider.transform.position, Quaternion.identity);
            StartCoroutine(LaserFade(0, 100));
            Score.Instance.ScoreUp(hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue);
            SoundDisplqyTuto.Instance.RemoveEnnemy(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
    }

    IEnumerator LaserFade(int cpt, int timer)
    {
        for (int i = 0; i < timer * 60 / AudioHelmClock.Instance.bpm; i++)
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
    public int cptPhase;
    [HideInInspector] public bool cantSpwan;
    [HideInInspector] public float waveNumber;
    [HideInInspector] public bool canShootSpecial;
    public void Spawn()
    {
        if (!cantSpwan)
        {
            // one simple ennemmy, 2 rows, time is freez until the ennemy is destroy
            if (cptPhase == 0)
            {
               
                positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                SoundDisplqyTuto.Instance.text.NextText(0);
                cptPhase++;
                cantSpwan = true;
            }
            // 3 ennemys, 2 rows
            else if (cptPhase == 2)
            {
                   

                switch (waveNumber)
                {
                    case 0:
                        positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 2:
                        positionEnd[2].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 5:
                        cptPhase++;
                        waveNumber = -1;
                        positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        cantSpwan = true;
                        break;

                    default:
                        break;
                }
                waveNumber++;
            }
            // linked ennemy, 2 rows, time is freez until the ennemy is destroy
            else if (cptPhase == 4)
            {
                LinkedSpawn(2, 3);
                cptPhase++;
                SoundDisplqyTuto.Instance.text.NextText(1);
                cantSpwan = true;
            }
            //linked ennemy + normal, every rows
            else if (cptPhase == 6)
            {
                if (waveNumber == 0)
                {
                    SoundDisplqyTuto.Instance.text.NextText(2);

                    for (int i = 0; i < positionEnd.Length; i++)
                    {
                        if (i != 2 && i != 3)
                        {
                            positionEnd[i].GetComponent<Spawner>().ApppearAll();
                        }

                    }
                }

                switch (waveNumber)
                {
                    case 3:
                        LinkedSpawn(2, 4);
                        break;
                    case 6:
                        positionEnd[2].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 8:
                        positionEnd[0].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 10:
                        LinkedSpawn(2, 4);
                        break;
                    case 12:
                        positionEnd[1].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 14:
                        positionEnd[5].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;

                    case 16:
                        LinkedSpawn(1, 3);
                        break;
                    case 18:
                        LinkedSpawn(0, 1);
                        break;
                    case 19:
                        positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        cantSpwan = true;
                        cptPhase++;
                        break;
                    default:
                        break;
                }
                waveNumber++;
            }
            //tank, every rows , time is freez until the ennemy is destroy
            else if (cptPhase == 8)
            {
                positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                SoundDisplqyTuto.Instance.text.NextText(3);
                cptPhase++;
                cantSpwan = true;
            }
            //tank, linked ennemy, normals
            else if (cptPhase == 10)
            {
                SoundDisplqyTuto.Instance.text.ClearText();
                switch (waveNumber)
                {
                    case 0:
                        positionEnd[2].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        break;
                    case 5:
                        positionEnd[5].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        break;
                    case 9:
                        positionEnd[0].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 10:
                        LinkedSpawn(2, 4);
                        break;
                    case 13:
                        positionEnd[1].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;
                    case 15:
                        positionEnd[5].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        break;

                    case 18:
                        positionEnd[4].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        break;
                    case 20:
                        LinkedSpawn(0, 1);
                        break;
                    case 23:
                        positionEnd[2].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        cantSpwan = true;
                        cptPhase++;
                        waveNumber = -1;
                        break;
                    default:
                        break;
                }
                waveNumber++;
            }
            //wave, time is freez until the wave is destroy
            else if (cptPhase == 12)
            {
                specialSpawner.GetComponent<Spawner>().Spwan(true);
                SoundDisplqyTuto.Instance.text.NextText(4);
                cptPhase++;
                cantSpwan = true;
            }
            //multiple waves to charges the special 
            else if (cptPhase == 14)
            {
                if(waveNumber ==0)
                    SoundDisplqyTuto.Instance.text.NextText(5);
                switch (waveNumber)
                {
                    case 0:
                        specialSpawner.GetComponent<Spawner>().Spwan(true);
                        break;
                    case 2:
                        specialSpawner.GetComponent<Spawner>().Spwan(true);
                        break;
                    case 4:
                        specialSpawner.GetComponent<Spawner>().Spwan(true);
                        break;                    
                    case 6:
                        specialSpawner.GetComponent<Spawner>().Spwan(true);
                        cantSpwan = true;
                        cptPhase++;
                        waveNumber = -1;

                        break;
                    default:
                        break;
                }
                waveNumber++;
               

            }
            // full of ennemy to use special
            else if (cptPhase == 16)
            {
                if (waveNumber == 0)
                    SoundDisplqyTuto.Instance.text.NextText(6);
                switch (waveNumber)
                {
                    case 0:
                        positionEnd[2].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[4].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[0].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        positionEnd[5].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        LinkedSpawn(1, 3);
                        break;
                    case 1:
                        positionEnd[0].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[1].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[4].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        positionEnd[5].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        LinkedSpawn(2, 3);
                        break;
                    case 2:
                        positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[1].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[0].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        positionEnd[4].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        LinkedSpawn(2, 5);
                        break;
                    case 3:
                        positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[1].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[0].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        positionEnd[4].GetComponent<Spawner>().Spwan(ennemysArray[0]);
                        LinkedSpawn(2, 5);
                        break;
                    case 4:
                        positionEnd[1].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        positionEnd[4].GetComponent<Spawner>().Spwan(ennemysArray[3]);
                        LinkedSpawn(2, 5);
                        LinkedSpawn(0, 3);
                        cantSpwan = true;
                        cptPhase++;
                        waveNumber = -1;

                        break;

                    default:
                        break;
                }
                waveNumber++;

            }
        }


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
    private void LinkedSpawn(int spwan1, int spwan2)
    {
        specialSpawner.GetComponent<Spawner>().Spwan(ennemysArray[2], positionEnd[spwan1].GetComponent<Spawner>(), positionEnd[spwan2].GetComponent<Spawner>());


    }
    IEnumerator BulletTime()
    {
        
        isBulletTime = true;
        specialCount = 0;
        specialBarG.value = 0;
        PostProcessManager.post.ActivatePostProcess((int)postProcess.SpecialPower);
        SoundDisplqyTuto.Instance.speedModifier = 0;
        SoundDisplqyTuto.Instance.text.NextText(7);
        SoundDisplqyTuto.Instance.bpmVisuelD.fillAmount = 0;
        SoundDisplqyTuto.Instance.bpmVisuelG.fillAmount = 0;
        SoundDisplqyTuto.Instance.cantMove = true;
        yield return new WaitWhile(() => isBulletTime == true);
        PostProcessManager.post.DeactivatePostProcess();
        SoundDisplqyTuto.Instance.speedModifier = 1;
        SoundDisplqyTuto.Instance.cantMove = false;

    }

    public void CanShoot()
    {
        if (SoundDisplqyTuto.Instance.doOnce)
        {
            canShoot = true;
        }

    }

    public void CantShoot()
    {
        canShoot = false;
        doOnceCPT = 0;
        SoundDisplqyTuto.Instance.doOnce = true;
    }
}
