using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance 
    {
        get 
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private float _score;
    public float Score
    {
        get
        {
            return _score;
        }
        private set
        {
            _score = value;
        }
    }

    private float _highscore;
    public float Highscore
    {
        get
        {
            return _highscore;
        }
        private set
        {
            _highscore = value;
        }
    }

    public event EventHandler Crash;
    private Gyroscope gyro;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupGyro();
        rigidbody = this.GetComponent<Rigidbody>();
        this.Highscore = PlayerPrefs.GetFloat("highscore");
    }

    private void SetupGyro()
    {
        gyro = Input.gyro;
        gyro.updateInterval = 0.5f;
        gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        Score += 0.01f * Time.deltaTime;
        Move();
    }

    private void Move()
    {
        var zRotation = gyro.rotationRate.z * -1;
        var position = rigidbody.transform.position;
        rigidbody.transform.position += new Vector3(zRotation * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag.Equals("Obstacle"))
        {
            OnCrash(EventArgs.Empty);
        }
    }

    protected virtual void OnCrash(EventArgs e)
    {
        if(this.Score > this.Highscore)
        {
            this.Highscore = this.Score;
            PlayerPrefs.SetFloat("highscore", this.Highscore);
            PlayerPrefs.Save();
        }
        EventHandler handler = Crash;
        handler?.Invoke(this, e);
    }


}







