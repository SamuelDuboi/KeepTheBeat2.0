using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Main : Singleton<Main>
{
    [Header("Sound")]
    public AudioSource music;
    public AudioSource clap;
    public double bpm;
    [Range(0, 100)]
    public float pourcentageAllow;

    [Header("Spawn")]
    public GameObject[] positionEnd = new GameObject[6];
    public Patterns[] patterns = new Patterns[1];
    public List<Patterns> patterns1 = new List<Patterns>();
    public List<Patterns> patterns2 = new List<Patterns>();
    public List<Patterns> patterns3 = new List<Patterns>();
    private List<int> previousNumberPattern = new List<int>();
    public int numberOfPatternPerDifficulty = 5;

    private Vector2 previousEnnemy;
    private List<Vector2> previousEnnemyList;
    private int currentPattern;
    private int nodeNumber;
    private int emptyNode;
    public GameObject[] rowOn = new GameObject[6];
    private int patternNumber;


    public bool canShoot;
    private LineRenderer lineRenderer;
    [HideInInspector] public SpriteRenderer sprite;


    [Header("Special")]
    public Slider specialBar;
    [HideInInspector] public int specialCount;
    public int specialMaxValue;
    public GameObject specialSpawner;
    public bool isBulletTime;
    public float maxBulletTime;
    private float bulletTimeTimer;



    private void Start()
    {

        sprite = GetComponent<SpriteRenderer>();
        bpm = SoundDisplay.Instance.Getbmp();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        specialBar.maxValue = specialMaxValue;

        previousEnnemyList = new List<Vector2>();
        SetPatternsArray();

        foreach (Vector2 vector in patterns1[0].ennemiesPosition)
        {
            previousEnnemyList.Add(vector);
        }
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
                lineRenderer.SetPosition(1, _ennemy.transform.position);
                clap.Play();
                StartCoroutine(LaserFade());
                SoundDisplay.Instance.RemoveEnnemy(_ennemy);
                Destroy(_ennemy);
                Score.Instance.ScoreUp(_ennemy.GetComponent<EnnemyBehavior>().scoreValue);
                StartCoroutine(RowFade(rowOn[_ennemyParent]));
                if (SoundDisplay.Instance.ennemys.Count == 0)
                {
                    isBulletTime = false;
                }
            }
            else if (SoundDisplay.Instance.ennemys.Count > 0)
                isBulletTime = false;
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
                Debug.Log("t'es pas en rythme connard");
        }
    }

    #region shoots
    void Shoot(Vector2 position, GameObject rowOn)
    {
        //canShoot = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(position.x - transform.position.x, position.y - transform.position.y));
        StartCoroutine(RowFade(rowOn));
        if (hit.collider != null && hit.collider.gameObject.tag == "Ennemy")
        {
            lineRenderer.SetPosition(1, hit.transform.position);
            clap.Play();
            StartCoroutine(LaserFade());
            Score.Instance.ScoreUp(hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue);
            SoundDisplay.Instance.RemoveEnnemy(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);
        }
    }
    /// <summary>
    /// shoot the special
    /// </summary>
    /// <param name="position"></param>
    /// <param name="isSpecial"></param>
    void Shoot(Vector2 position, bool isSpecial)
    {
        canShoot = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(position.x - transform.position.x, position.y - transform.position.y));
        StartCoroutine(RowFade(isSpecial));
        if (specialCount == specialMaxValue)
        {
            StartCoroutine(BulletTime());
        }
        else if (hit.collider != null && hit.collider.gameObject.tag == "SpecialEnnemy")
        {
            lineRenderer.SetPosition(1, hit.transform.position);
            clap.Play();
            Score.Instance.ScoreUp(hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue);
            SoundDisplay.Instance.RemoveEnnemy(hit.collider.gameObject);
            Destroy(hit.collider.gameObject);

            specialCount++;
            specialBar.value = specialCount;
        }
    }

    IEnumerator LaserFade()
    {
        for (int i = 0; i < 100; i++)
        {
            lineRenderer.startColor -= new Color(0, 0, 0, 0.01f);
            lineRenderer.endColor -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.005f);

        }
        lineRenderer.startColor += new Color(0, 0, 0, 1);
        lineRenderer.endColor -= new Color(0, 0, 0, 1);
        lineRenderer.SetPosition(1, transform.position);
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
        }
        yield return new WaitForSeconds(0.3f);
        foreach (var row in rowOn)
        {
            row.SetActive(false);
        }
    }
    #endregion
    public void Spawn()
    {
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


            #endregion

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
                    positionEnd[(int)previousEnnemyList[numberForEnnemy[i]].x].GetComponent<Spawner>().Spwan();
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

    public void SpecialSpawn()
    {
        specialSpawner.GetComponent<Spawner>().Spwan(true);

    }

    IEnumerator BulletTime()
    {
        isBulletTime = true;
        specialCount = 0;
        specialBar.value = 0;
        SoundDisplay.Instance.speedModifier = 0;
        yield return new WaitWhile(() => isBulletTime == true);
        SoundDisplay.Instance.speedModifier = 1;

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
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

}