using AudioHelm;
using Microsoft.Win32.SafeHandles;
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
    private bool isMiniBoss;
    private bool canShootMiniBoss;
    public GameObject miniBoss;
    private int miniBossDamage;
    public float miniBossTime;
    public float miniBossMaxScore;
    public int miniBossMinScore;


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
                SoundDisplay.Instance.RemoveEnnemy(_ennemy);
                Destroy(_ennemy);
                Score.Instance.ScoreUp(_ennemy.GetComponent<EnnemyBehavior>().scoreValue);
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
            if(Input.GetKeyDown(KeyCode.E) )
            {
                lineRenderer[0].SetPosition(1, specialSpawner.transform.position);
                StartCoroutine(RowFade(rowOn[2]));
                StartCoroutine(LaserFade(0,20));
                miniBossDamage++;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                lineRenderer[1].SetPosition(1, specialSpawner.transform.position);
                StartCoroutine(RowFade(rowOn[3]));
                StartCoroutine(LaserFade(1,20));
                miniBossDamage++;
            }
           
        }
        else if (Input.anyKeyDown)
        {
            if (canShoot)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                    Shoot(positionEnd[0].transform.position, rowOn[0]);
                else if (Input.GetKeyDown(KeyCode.Z))
                    Shoot(positionEnd[1].transform.position, rowOn[1]);
                else if (Input.GetKeyDown(KeyCode.E))
                    Shoot(positionEnd[2].transform.position, rowOn[2]);
                else if (Input.GetKeyDown(KeyCode.R))
                    Shoot(positionEnd[3].transform.position, rowOn[3]);
                else if (Input.GetKeyDown(KeyCode.T))
                    Shoot(positionEnd[4].transform.position, rowOn[4]);
                else if (Input.GetKeyDown(KeyCode.H))
                    Shoot(positionEnd[5].transform.position, rowOn[5]);
                else if (Input.GetKeyDown(KeyCode.Space))
                    Shoot(specialSpawner.transform.position, true);
            }
            else
                Score.Instance.ScoreDown(1000);
        }
    }

    #region shoots
    void Shoot(Vector3 position, GameObject rowOn)
    {
        doOnceCPT++;
        var _cpt = doOnceCPT;
        if(doOnceCPT ==2)
        {
            doOnceCPT = 0;
            canShoot = false;
        }
        RaycastHit hit;
        Physics.Raycast(new Vector3(position.x, position.y-2.5f, position.z-13), new Vector3(0,2.5f,15),out hit );
        StartCoroutine(RowFade(rowOn));
        if (hit.collider != null )
        {
            if(hit.collider.gameObject.tag == "Ennemy")
            {
                lineRenderer[_cpt - 1].SetPosition(1, hit.transform.position);
                clap.Play();
                hit.collider.gameObject.GetComponent<EnnemyBehavior>().Destroyed();
                StartCoroutine(LaserFade(_cpt - 1,100));
            }
            else if(hit.collider.gameObject.tag == "LinkedEnnemy")
            {
                lineRenderer[_cpt - 1].SetPosition(1, hit.transform.position);
                clap.Play();
                StartCoroutine(LaserFade(_cpt - 1,100));

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
            SoundDisplay.Instance.RemoveEnnemy(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
    }

    IEnumerator LaserFade(int cpt, int timer)
    {
        for (int i = 0; i < timer * 60/AudioHelmClock.GetGlobalBpm(); i++)
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
        if (!isMiniBoss) 
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

                if (patternNumber == 9)
                {
                    StartCoroutine(MiniBoss());
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
            positionEnd[listEnnemy].GetComponent<Spawner>().Spwan(ennemysArray[1], positionEnd[listEnnemy + 2].GetComponent<Spawner>().positions);
        }
        else if (x == 2 || x == 5)
        {
            positionEnd[listEnnemy].GetComponent<Spawner>().Spwan(ennemysArray[1], positionEnd[listEnnemy - 2].GetComponent<Spawner>().positions);
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
        isMiniBoss = true;
        miniBossDamage = 0;
        yield return new WaitUntil(() => SoundDisplay.Instance.ennemys.Count == 0f);
        yield return new WaitForSeconds(2f);
        canShootMiniBoss = true;
        //instantiate the mini boss in the middle of the scene
        var _miniBoss = Instantiate(miniBoss, specialSpawner.transform);
        yield return new WaitForSeconds(miniBossTime);
        Destroy(_miniBoss);
        canShootMiniBoss = false;

        Score.Instance.ScoreUp(miniBossDamage * 100);

        // wait for the player to calm down
        yield return new WaitForSeconds(5f);
        isMiniBoss = false;


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