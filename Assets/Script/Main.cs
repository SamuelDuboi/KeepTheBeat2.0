using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : Singleton<Main>
{
	public AudioSource music;
	public AudioSource clap;

    public double bpm;
	public GameObject[] positionEnd = new GameObject[6];
	public  Patterns[] patterns = new Patterns[1];
	private Vector2 previousEnnemy;
	private List< Vector2> previousEnnemyList;
	private int currentPattern;
	private int emptyNode;
	public GameObject[] rowOn = new GameObject[6];
	[Range(0,100)]
    public float pourcentageAllow;
	 public bool canShoot;
	private LineRenderer lineRenderer;
	[HideInInspector] public SpriteRenderer sprite;

	//for special
	[HideInInspector] public int specialCount;
	public Slider specialBar;
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

        foreach (Vector2 vector in patterns[0].ennemiesPosition)
        {
			previousEnnemyList.Add(vector);
        }
		previousEnnemy = Vector2.one * 12;
    }

	private void Update()
	{
        if (isBulletTime)
        {
			bulletTimeTimer += Time.deltaTime;
			if(bulletTimeTimer>= maxBulletTime)
            {
				isBulletTime = false;
				bulletTimeTimer = 0;
            }
            if (Input.anyKeyDown)
            {
				GameObject _ennemy = SoundDisplay.Instance.ennemys[SoundDisplay.Instance.ennemys.Count - 1];
				int _ennemyParent =0;
                for (int i = 0; i < positionEnd.Length; i++)
                {
					if (positionEnd[i] == _ennemy.transform.parent.gameObject)
                    {
						_ennemyParent = i;
						break;
                    }
                }
				lineRenderer.SetPosition(1,_ennemy.transform.position);
				clap.Play();
				StartCoroutine(LaserFade());
				SoundDisplay.Instance.RemoveEnnemy(_ennemy);
				Destroy(_ennemy);
				Score.Instance.ScoreUp(_ennemy.GetComponent<EnnemyBehavior>().scoreValue);
				StartCoroutine(RowFade(rowOn[_ennemyParent]));
				if(SoundDisplay.Instance.ennemys.Count == 0)
                {
					isBulletTime = false;
                }
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
				Debug.Log("t'es pas en rythme connard");
		}
	}

  
	void Shoot(Vector2 position, GameObject rowOn)
    {
		canShoot = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(position.x - transform.position.x, position.y - transform.position.y));
		StartCoroutine(RowFade(rowOn));
		if(hit.collider != null && hit.collider.gameObject.tag == "Ennemy")
        {
			lineRenderer.SetPosition(1, hit.transform.position);
			clap.Play();
			StartCoroutine(LaserFade());
			Score.Instance.ScoreUp(hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue);
			SoundDisplay.Instance.RemoveEnnemy(hit.collider.gameObject);
			Destroy(hit.collider.gameObject);
        }
    }
	void Shoot(Vector2 position, bool isSpecial)
	{
		canShoot = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(position.x - transform.position.x, position.y - transform.position.y));
		StartCoroutine(RowFade(isSpecial));
		if(specialCount == specialMaxValue)
        {
			StartCoroutine(BulletTime());
        }
		else if (hit.collider != null && hit.collider.gameObject.tag == "SpecialEnnemy")
		{
			lineRenderer.SetPosition(1, hit.transform.position);
			clap.Play();
			Score.Instance.ScoreUp(hit.collider.gameObject.GetComponent<EnnemyBehavior>().scoreValue);
			SoundDisplay.Instance.specialEnnemy = null;
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
	IEnumerator RowFade( bool isSpecial)
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
	public void Spawn()
    {
		float biggest = 0;
		List<int> numberForEnnemy = new List<int>();

		if ( previousEnnemyList.Count == 0 )
        {
			if (currentPattern != patterns.Length)
			{
				currentPattern++;
				foreach (var vecor in patterns[currentPattern].ennemiesPosition)
				{
					previousEnnemyList.Add(vecor);
				}
			}
			else
				return;
			
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
            if (previousEnnemyList[i].y > biggest )
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
			for (int i = 0; i < numberForEnnemy.Count; i++)
			{
				positionEnd[(int)previousEnnemyList[numberForEnnemy[i]].x].GetComponent<Spawner>().Spwan();
				previousEnnemy = previousEnnemyList[numberForEnnemy[i]];
			}
			for (int i = 0; i < numberForEnnemy.Count; i++)
			{
				
				previousEnnemyList.RemoveAt(numberForEnnemy[i] - i);
			}
		}
		else
			emptyNode++;


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