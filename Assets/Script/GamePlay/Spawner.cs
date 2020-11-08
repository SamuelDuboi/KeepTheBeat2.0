using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Spawner : MonoBehaviour
{
    public GameObject popScore;
    public GameObject explosion;
    public GameObject pawn;
    public GameObject direction;
    public GameObject ennemy;
    public int timeForSpawn;
    private double waitForSpawn;
    private Vector3 distanceToMove;
    public Vector3[] positions = new Vector3[6];

    private Animator animator;

    public GameObject[] tiles = new GameObject[6];
   [HideInInspector] public Color currentColor;
    // Start is called before the first frame update
    void Start()
    {
        currentColor = GetComponent<SpriteRenderer>().color;
        StartSpawn();
        
    }


    void StartSpawn()
    {
        distanceToMove.x = (direction.transform.position.x - transform.position.x) / 6;
        distanceToMove.y = (direction.transform.position.y - transform.position.y) / 6;
        distanceToMove.z = (direction.transform.position.z - transform.position.z) / 6;

        for (int i = 0; i < 6; i++)
        {
            positions[i] = new Vector3(transform.position.x + distanceToMove.x * i, transform.position.y + distanceToMove.y * i,transform.position.z + distanceToMove.z*i);
            GameObject _pawn = Instantiate(pawn, positions[i], Quaternion.identity, transform);
        }
    }

    public void Spwan(GameObject ennemy)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z * 100);

        if (SceneManager.GetActiveScene().name != "Tuto")
            SoundDisplay.Instance.AddEnnemy(_ennemy);
        else
            SoundDisplqyTuto.Instance.AddEnnemy(_ennemy);


        _ennemy.GetComponentInChildren<Light>().color = currentColor;
        _ennemy.GetComponent<EnnemyBehavior>().positions = this.positions;
        _ennemy.GetComponent<EnnemyBehavior>().explosion = explosion;
        _ennemy.GetComponentInChildren<TrailRenderer>().startColor = currentColor;
        _ennemy.GetComponent<EnnemyBehavior>().popTextScore = popScore;
        
    }
    /// <summary>
    /// spawn tp Ennemy
    /// </summary>
    /// <param name="ennemy"></param>
    /// <param name="positions"></param>
    /// <param name="secondColor"></param>
    public void Spwan(GameObject ennemy, Vector3[] positions, Color secondColor)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * 100);
        if (SceneManager.GetActiveScene().name != "Tuto")
            SoundDisplay.Instance.AddEnnemy(_ennemy);
        else
            SoundDisplqyTuto.Instance.AddEnnemy(_ennemy); ;
        _ennemy.GetComponent<EnnemyBehavior>().turnOnTrail = true;
        _ennemy.GetComponent<EnnemyBehavior>().explosion = explosion;
        _ennemy.GetComponent<EnnemyBehavior>().popTextScore = popScore;
        _ennemy.GetComponent<EnnemyBehavior>().row1 = currentColor;
        _ennemy.GetComponent<EnnemyBehavior>().row2 = secondColor;
        for (int i = 0; i < positions.Length; i++)
        {
            if(i%2 == 0)
            _ennemy.GetComponent<EnnemyBehavior>().positions[i] = this.positions[i];
            else
                _ennemy.GetComponent<EnnemyBehavior>().positions[i] = positions[i];


        }
    }
    /// <summary>
    /// Spawn Linked ennemy
    /// </summary>
    /// <param name="ennemy"></param>
    /// <param name="firstSpawner"></param>
    /// <param name="secondSpawner"></param>
    public void Spwan(GameObject ennemy, Spawner firstSpawner, Spawner secondSpawner)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].transform.position = new Vector3(firstSpawner.transform.position.x, firstSpawner.transform.position.y, firstSpawner.transform.position.z * 100);
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].transform.position = new Vector3(secondSpawner.transform.position.x, secondSpawner.transform.position.y, secondSpawner.transform.position.z * 100);
        if (SceneManager.GetActiveScene().name != "Tuto")
            SoundDisplay.Instance.AddEnnemy(_ennemy);
        else
            SoundDisplqyTuto.Instance.AddEnnemy(_ennemy);
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponent<EnnemyBehavior>().turnOnTrail = false;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponent<EnnemyBehavior>().turnOnTrail = false;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponent<EnnemyBehavior>().explosion = firstSpawner.explosion;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponent<EnnemyBehavior>().explosion = secondSpawner.explosion;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponent<EnnemyBehavior>().popTextScore = firstSpawner.popScore;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponent<EnnemyBehavior>().popTextScore = secondSpawner.popScore;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponentInChildren<Light>().color = firstSpawner.currentColor;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponentInChildren<Light>().color = secondSpawner.currentColor;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponentInChildren<TrailRenderer>().startColor = firstSpawner.currentColor;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponentInChildren<TrailRenderer>().startColor = secondSpawner.currentColor;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[0].GetComponentInChildren<EnnemyBehavior>().positions = firstSpawner.positions;
        _ennemy.GetComponent<LinkedEnnemy>().hitBox[1].GetComponentInChildren<EnnemyBehavior>().positions = secondSpawner.positions;
      
    }
    /// <summary>
    /// Special spawner
    /// </summary>
    /// <param name="isSPecial"></param>
    public void Spwan(bool isSPecial)
    {
        GameObject _ennemy = Instantiate(ennemy, transform);
        if(isSPecial)
        {
            if (SceneManager.GetActiveScene().name != "Tuto")
                SoundDisplay.Instance.AddEnnemy(_ennemy);
            else
                SoundDisplqyTuto.Instance.AddEnnemy(_ennemy);
            _ennemy.GetComponent<EnnemyBehavior>().positions = this.positions;

        }
    }


    public void Shoot()
    {
        tiles[5].GetComponent<MeshRenderer>().material = tiles[5].GetComponent<TilesBehavior>().on;
        StartCoroutine(ShootFade());
    }

    IEnumerator ShootFade()
    {
        yield return new WaitForSeconds(0.1f);
        tiles[5].GetComponent<MeshRenderer>().material = tiles[5].GetComponent<TilesBehavior>().off;
    }


    public void LeaveOne()
    {
        animator.SetTrigger("LeftOne");
    }

    public void DesaapppearAll()
    {
        animator.SetTrigger("LeaveAll");
    }

    public void AppeartOne()
    {
        animator.SetTrigger("AppearOne");

    }
    public void ApppearAll()
    {
        if(animator == null)
        animator = tiles[0].GetComponentInParent<Animator>();
        animator.SetTrigger("Appear");
    }
}


