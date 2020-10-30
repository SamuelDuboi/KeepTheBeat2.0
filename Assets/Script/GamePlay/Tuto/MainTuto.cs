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
                SoundDisplqyTuto.Instance.RemoveEnnemy(_ennemy);
                Destroy(_ennemy);
                Score.Instance.ScoreUp(_ennemy.GetComponent<EnnemyBehavior>().scoreValue);
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
                hit.collider.gameObject.GetComponent<EnnemyBehavior>().Destroyed(2 - doOnceCPT);
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
    [HideInInspector] public int cptPhase;
    public void Spawn()
    {
        
        if(cptPhase == 0)
        {
            positionEnd[3].GetComponent<Spawner>().Spwan(ennemysArray[0]);
            cptPhase++;
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
    private void LinkedSpawn(float x, List<int> listEnnemy, Vector3 current, int currentInt)
    {
        int secondSpawn = 1000;
        for (int i = 0; i < listEnnemy.Count; i++)
        {
            if (previousEnnemyList[listEnnemy[i]].z == 4 && previousEnnemyList[listEnnemy[i]].y == current.y)
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
        SoundDisplqyTuto.Instance.speedModifier = 0;
        SoundDisplqyTuto.Instance.cantMove = true;
        yield return new WaitWhile(() => isBulletTime == true);
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
