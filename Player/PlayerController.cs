//////////////////////////////////////////////////
//// Author:        Lily Raeburn
//// Description:   A player controller for launching the bird and drawing a trajectory
//// Date created:  22/02/2018
//////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{

	public static PlayerController instance;

	private Vector3 m_prevPlayerPos;

	private int m_score;

	// A bool for storing whether the mouse is down
	private bool m_mouseIsDown;
    // Vector3 for the mouse position
    private Vector3 m_cursorPosition;

	public float zLineRenderPos;

    // Trajectory variables

    // The line renderer attached to this component
    // Used to draw a trajectory for the player to use as a guide
    public LineRenderer lr;

    [SerializeField]
    private float m_velocity;
    private float M_v2VelocityX
    {
        get { return -m_velocity * Mathf.Cos(m_radianAngle); }
    }
    private float M_v2VelocityY
    {
        get { return -m_velocity * Mathf.Sin(m_radianAngle); }
    }

    public float velocityModifier = 2f;
    [SerializeField]
    private float M_angle
    {
        get { return Mathf.Rad2Deg * m_radianAngle; }
    }
    // Casting degrees to radians
    [SerializeField]
    private float m_radianAngle;
    [SerializeField]
    private int m_baseResolution = 10;
    private int m_resolution;
    private Vector2 m_grav;
    private Vector3[] m_trajectory;
    private float m_maxDist;

    // A variable to track when the player is grounded
    [SerializeField]
    private bool m_grounded;
	public bool Grounded
	{
		get { return m_grounded; }
	}

    // The amount of times the player can make a gravity jump
    [SerializeField]
    public int m_gravityJumps = 0;

	[SerializeField]
	private Text m_currentJumpsText;

    [SerializeField]
	private Image m_rocket;

	[SerializeField]
	private bool m_bCharge = false;

	private int m_currentGravityJumps;

    //[SerializeField]
    //private float m_platformKnockdownStrength = 15f;
	
    // This is a timer that toggles the _grounded variable, the idea is that we don't want to be able to launch the bird in the air or straight after landing.
    // This timer functionality makes sure that the player has to wait once the bird has landed, this could be to account for any rolling the bird might do

    // Timer variables
    private bool m_startCountdown;   // Bool for starting countdown
    private float m_startTime;       // Bool for tracking the time when the countdown was started, if -1f then the timer needs to be restarted
    private float m_endTime;         // Bool for tracking when the timer needs to end
    [Range(0.5f, 1.5f)]
    public float m_timerAmount = 0.75f;   // The amount of time the timer runs
    public Image m_timerImage;     // An image to help display the timer in effect
    public Image m_lockImage;      // An image of a lock to signify that the player cannot be controlled    

    // The player rigid body
    private Rigidbody2D m_rbdy;

    // The amount of friction applied to the player when rolling on the floor
    [Range(0f, 0.45f)]
    public float floorFriction = 0.025f;

    // The text that will display the player height
    public Text scoreText;
	public Text pauseScoreText;

	// A vector3 for where the player should return to in the menu screen

	private Vector3 m_menuPos;
    public Vector3 menuScale;
    public Vector3 gameScale;

	public void ChargeJump(float a_fillAmount)
	{
		if (m_currentGravityJumps < m_gravityJumps)
		{
			m_rocket.fillAmount += a_fillAmount;
			if (m_rocket.fillAmount >= 1)
			{
				m_currentGravityJumps++;
				m_currentJumpsText.text = m_currentGravityJumps.ToString();
                if (m_currentGravityJumps < m_gravityJumps)
                {
                    m_rocket.fillAmount = 0.0f;
                }

            }
		}
	}

    // When the component wakes
    void Awake () {
		if (instance == null)
			instance = this;

		// Just initialising the variables we will need
		m_grounded = false;
        m_startCountdown = false;

        m_startTime = -1f;   // This variable signifies that the timer needs to be reset once the player has collided with the floor

        m_mouseIsDown = false;
        lr.enabled = false; // Disable the line renderer which draws a trajectory for the bird

        // Getting the player rigid body
        m_rbdy = GetComponent<Rigidbody2D>();

        // Getting the timer image for the timer which will be the child of the player
        m_timerImage.enabled = false;
        m_lockImage.enabled = false;

        // Initialising _resolution
        m_resolution = m_baseResolution;

        // Set the trajectory resolution
        lr.positionCount = m_resolution + 1;

        // Set the correct scale for the player in the menu
        transform.localScale = menuScale;

		m_currentGravityJumps = m_gravityJumps;
		m_currentJumpsText.text = m_currentGravityJumps.ToString();

		m_prevTimeScale = Time.timeScale;
	}

    private void Start()
    {
        // Setting the position of the player in the menu
        m_menuPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start the update loop
        m_updateActivated = true;

        if (other.gameObject.tag == "GoodCloud")
        {
            if (!other.gameObject.GetComponent<PickupController>().particlesPlayed)
            {
                other.gameObject.GetComponent<PickupController>().PlayerCollision();
                m_rbdy.velocity *= 2f;
                m_shakeCamera = true;
            }
            return;
        }

        if (other.gameObject.tag == "BadCloud")
        {
            if (!other.gameObject.GetComponent<PickupController>().particlesPlayed)
            {
                other.gameObject.GetComponent<PickupController>().PlayerCollision();
                m_rbdy.velocity *= -2f;
                m_shakeCamera = true;
            }
            return;
        }

        if (other.gameObject.tag == "Feather")
        {
            if (!other.gameObject.GetComponent<PickupController>().particlesPlayed)
            {
                other.gameObject.GetComponent<PickupController>().PlayerCollision();
                UpdateFeathers.instance.AddFeathers(FeatherAmounts.instance.PickAmount().quantity);
                m_shakeCamera = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Start the update loop
        m_updateActivated = true;

        // When colliding with the floor start the _grounded controlling timer
        if (other.gameObject.tag == "Floor")
        {
            m_startCountdown = true;

            // Reset gravity
            Gravity.Instance.UpdateGravity();

          ////// If we hit a platform hard enough then make it fall
          //if (m_rbdy.velocity.magnitude > m_platformKnockdownStrength)
          //{
          //    other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
          //    other.gameObject.GetComponent<Rigidbody2D>().mass = 15f;
          //    other.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
		  //
          //}
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        // When exiting collision with the floor we need stop the _grounded controlling timer and reset _grounded to false
        // and _startTime to -1f so that the next time the timer is started it can reset itself
        if (other.gameObject.tag == "Floor")
        {
            m_grounded = false;
            m_startTime = -1f;
            m_startCountdown = false;
            m_timerImage.enabled = false;
            m_lockImage.enabled = false;
        }
    }

    // Turns true when we first collide with something, this stops the game launching the player when we click start
    private bool m_updateActivated = false;

    // Controls camera shaking
    private bool m_shakeCamera = false;

	private bool IsPointerOverUIObject()
	{
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	// Each frame
	void Update()
    {
		if (m_bCharge)
		{
			ChargeJump(0.25f);
			m_bCharge = false;
		}
        if (!m_updateActivated)
            return;

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && !m_paused)// When the mouse is down
		{
			m_mouseIsDown = true;
		}

        // Launching the player
        if (m_mouseIsDown)
        {
            //getting cursor position
            m_cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Getting the velocity based off the distance of the mouse cursor from the player
            m_velocity = Vector2.Distance(transform.position, m_cursorPosition) * velocityModifier;
      
            // Getting the angle from the cursor position and position of the player
            m_radianAngle = Mathf.Atan2(
                m_cursorPosition.y - transform.position.y,
                m_cursorPosition.x - transform.position.x
                );

            // From the ground
            if (m_grounded)
            {
                Gravity.Instance.UpdateGravity();
                // Calculate and draw the trajectory
                lr.enabled = true;
                CalculateTrajectory();
            }
            // From the air
            else if (m_currentGravityJumps > 0)
			{
                Time.timeScale = 0;
                Physics2D.gravity = new Vector2(Mathf.Cos(m_radianAngle), Mathf.Sin(m_radianAngle)).normalized * -m_velocity;
                lr.enabled = true;
                // Calculate and draw a straight line
                CalculateLine();
            }

            // Turn on camera shake
            m_shakeCamera = true;
        }

        // Reset camera from previous shaking
        Camera.main.transform.localPosition = Vector3.zero;

        // Camera shake
        if (m_shakeCamera)
        {
            Vector3 shake = new Vector3();

            float shakeAmount = m_velocity * 0.0025f;

            shake.x = UnityEngine.Random.Range(-shakeAmount, shakeAmount);
            shake.y = UnityEngine.Random.Range(-shakeAmount, shakeAmount);
            shake.z = 0f;

            Camera.main.transform.localPosition = shake;

            // Turn off camera shake
            m_shakeCamera = false;
        }

		// When the mouse is released
		if (Input.GetMouseButtonUp(0) && m_mouseIsDown)
		{
            m_mouseIsDown = false;
            lr.enabled = false;

            // Disable the appropriate variables and launch the bird if the mouse was previously pressed
            if (m_grounded)
            {
            	Launch();
            }
            else if (m_currentGravityJumps > 0)
            {
                --m_currentGravityJumps;
				if (m_rocket.fillAmount == 1)
					m_rocket.fillAmount = 0;

				m_currentJumpsText.text = m_currentGravityJumps.ToString();

				Launch();
            }

            Time.timeScale = 1;
        }

        m_score = Mathf.FloorToInt(Vector3.Distance(m_prevPlayerPos, transform.position));
        if (SaveManager.Instance != null)
        {
            if (m_score > SaveManager.Instance.state.HighScore)
            {
                SaveManager.Instance.state.HighScore = m_score;
            }
        }

        // Displaying the player score
        scoreText.text = "SCORE: " + m_score;
		pauseScoreText.text = "SCORE: " + m_score;
    }

    private void FixedUpdate()
    {
        // When we are colliding with the floor we want to slow the rolling of the player down
        m_rbdy.angularDrag = floorFriction;
        m_rbdy.sharedMaterial.friction = floorFriction;

        //The timer for working out if the player is grounded
        if (m_startCountdown && m_rbdy.velocity.magnitude < 0.01)
        {
            // Resetting the timer
            if (m_startTime == -1f)
            {
                m_timerImage.enabled = true;
                m_lockImage.enabled = true;
                m_startTime = Time.time;
                m_endTime = m_startTime + m_timerAmount;
            }

            // Calculating the fill amount for the timer image
            m_timerImage.GetComponent<Image>().fillAmount = (Time.time - m_startTime) / m_timerAmount;

            // If the timer is complete
            if (Time.time >= m_endTime)
            {
                m_grounded = true;
                m_prevPlayerPos = transform.position;
                UpdateFeathers.instance.AddFeathers(m_score);
                m_score = 0;
                m_startCountdown = false;
				m_currentGravityJumps = m_gravityJumps;
				m_currentJumpsText.text = m_currentGravityJumps.ToString();
				m_rocket.fillAmount = 1;
				m_startTime = -1f;
                m_timerImage.CrossFadeAlpha(0f, m_timerAmount * 0.5f, false);
                m_lockImage.CrossFadeAlpha(0f, m_timerAmount * 0.5f, false);

            }
        }
    }

    // If a inspector variable changes then update the variables here
    void OnValidate()
    {
        if (lr != null)
            lr.positionCount = m_baseResolution + 1;
    }

    // Calculating and drawing the trajectory of the players launch
    void CalculateTrajectory()
    {
        m_trajectory = new Vector3[lr.positionCount];
        
        for (int i = 0; i <= m_baseResolution; ++i)
        {
            float t = (float)i / (float)m_baseResolution;
            m_trajectory[i] = CalculateTrajectoryPoint(t);
        }

        lr.SetPositions(m_trajectory);
    }

    // Maths from https://en.wikipedia.org/wiki/Projectile_motion
    Vector3 CalculateTrajectoryPoint(float a_t)
    {
        float x = -m_velocity * a_t * Mathf.Cos(m_radianAngle) - (0.5f * Physics2D.gravity.x * a_t * a_t);
        float y = -m_velocity * a_t * Mathf.Sin(m_radianAngle) - (0.5f * -Physics2D.gravity.y * a_t * a_t);

        return new Vector3(x + transform.position.x, y + transform.position.y, zLineRenderPos);
    }

    // Calculating and drawing a line of the players launch
    void CalculateLine()
    {
        m_trajectory = new Vector3[lr.positionCount];

        for (int i = 0; i <= m_baseResolution; ++i)
        {
            float t = (float)i / (float)m_baseResolution;
            m_trajectory[i] = CalculateLinePoint(t);
        }

        lr.SetPositions(m_trajectory);
    }

    Vector3 CalculateLinePoint(float a_t)
    {
        float x = Physics2D.gravity.x * m_velocity * a_t * 0.02f;
        float y = Physics2D.gravity.y * m_velocity * a_t * 0.02f;

        return new Vector3(x + transform.position.x, y + transform.position.y, zLineRenderPos);
    }

    // Launching the bird
    void Launch()
    {
        m_rbdy.velocity = new Vector2(M_v2VelocityX, M_v2VelocityY);
    }

    public void MenuMode()
    {
		m_grounded = true;
		m_prevPlayerPos = transform.position;
		UpdateFeathers.instance.AddFeathers(m_score);
		m_score = 0;
		m_startCountdown = false;
		m_currentGravityJumps = m_gravityJumps;
		m_currentJumpsText.text = m_currentGravityJumps.ToString();
		m_rocket.fillAmount = 1;
		m_startTime = -1f;
		m_timerImage.CrossFadeAlpha(0f, m_timerAmount * 0.5f, false);
		m_lockImage.CrossFadeAlpha(0f, m_timerAmount * 0.5f, false);


		m_rbdy.simulated = false;
        transform.position = m_menuPos;
        // Reset the camera parent position
        Camera.main.transform.parent.position = new Vector3(0f, 0f, -10f);
        transform.rotation = Quaternion.identity;
        transform.localScale = menuScale;
    }

    public void GameMode()
    {
        m_rbdy.simulated = true;
        transform.localScale = gameScale;

        m_updateActivated = false;

	}

    private bool m_paused = false;
    private float m_prevTimeScale;

    public void PauseGame()
    {
        m_prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        m_paused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = m_prevTimeScale;

        m_paused = false;
    }
}
