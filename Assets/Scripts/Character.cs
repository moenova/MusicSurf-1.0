﻿using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class Character : MonoBehaviour
{
    //public PathCreator pathCreatorLeft;
    //public PathCreator pathCreatorMiddle;
    //public PathCreator pathCreatorRight;
    //public PathCreator pathCreatorSelected;
    public int position = 0;
    float distanceTravelled;
    private TMP_Text tone_text;
    public int tone = 0;
    public static int score = 0;
    public bool AxisActive = false;

    // smooth shift 
    public static float move_period;
    private float hori_pos;
    private float curpos;
    private float nextpos;
    public float start_shift_time;


    //speed

    public float speed;
    //jump
    public float jump_height;
    public float jump_period;
    private float start_jump_time;
    private bool jumping;
    private float curjump_height;


    //using logic
    /* 0 : 默认node path 逻辑
     * 1： 撞到block逻辑
     * 2： 撞到gap enter
     */
    public int move_logic;

    public Vector2 idx_hidx;
    public Text energy_txt;
    public static float start_time;
    public static float time_leak;
    public float GameOverTime;
    public bool isGameOver;
    public static int track;
    public Animator charAnim;
    public GameObject snowballs_ui;

    public static int leveltime = 0;
    public bool is_win;
    private void Awake()
    {
    }
    void Start()
    {
        //pathCreatorSelected = pathCreatorMiddle;
        Debug.Log("Starttttttttttt");
        position = 0;
        tone_text = transform.Find("mark").Find("Text (TMP)").GetComponent<TMP_Text>();
        idx_hidx = new Vector2(0, 0);
        
        hori_pos = 0;
        curpos = 0;
        nextpos = 0;

        //jump
        jumping = false;
        curjump_height = 0;
        time_leak = Time.time - start_time;

        isGameOver = false;
        charAnim = GetComponent<Animator>();
        score = 0;
        // charAnim.Play("character_l");

        MusicController mc = GameObject.Find("MusicObject").GetComponent<MusicController>();
        mc.asrc.time = leveltime;
        mc.asrc.Play();

        Debug.Log(mc.asrc.time);
    }
    // Update is called once per frame
    void Update()
    {
        //charAnim.Play("Take 001");
        // energy_txt.text = "Snowballs collected: " + score / 10;
        if (move_logic == 0)
        {
            idx_hidx[0] = (float)(Time.time - start_time - time_leak) * speed + track;
            idx_hidx[1] = hori_pos;
            int idx = Path_Node_scpt.idx_float2int(idx_hidx[0]);
            int hidx = Path_Node_scpt.hidx_float2int(idx_hidx[1]);
            //Debug.Log(idx_hidx);
            Vector3 pos = Path_Node_scpt.queryPos(idx_hidx, idx, hidx);
            
            pos.y += curjump_height;
            this.transform.position = pos;
            this.transform.rotation = Path_Node_scpt.queryRotY(idx_hidx, idx, hidx);

            // Debug.Log(idx_hidx[0]);
            // 如果 idx_hidx[0]> path node 的个数
            //游戏结束

        }
        else if (move_logic == 1)
        {

            //过一段时间之后游戏结束
        }
        else if (move_logic ==2){
            Rigidbody rb = this.transform.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(this.transform.forward*100);

            //过一段时间游戏结束
        }
        //if (is_win){
        //    charAnim.Play("Win");
        //}
        if (isGameOver && Time.time - GameOverTime > 3)
        {
            GameController game_controller = GameObject.FindGameObjectWithTag("end_line").transform.GetComponent<GameController>();
            if (is_win){
                Debug.Log("You win");
                game_controller.GetComponent<GameController>().EndGame(true);
            } else {
                Debug.Log("You die");
                game_controller.EndGame(false);
            }
        }



        if (curpos != nextpos) {

            float time_length = Time.time - start_shift_time;
            float a = time_length / move_period;
            if (a >= 1) {
                curpos = nextpos;
                hori_pos = nextpos;
            }
            else {
                hori_pos = (1-a) * curpos + a * nextpos;
            }

        }


        float jump_time_length = Time.time - start_jump_time;
        float jump_a = jump_time_length / jump_period;
        float logiH = getLogiHeight(jump_a);

        if (jumping && logiH >= 0)
        {
            curjump_height = logiH * jump_height;
        }
        else
        {
            curjump_height = 0;
            jumping = false;
        }


        // keyboard input

        /*
        if (Input.GetAxis("XAxis") < -0.5f)
        {
            nextpos = -1;
        }
        else if (Input.GetAxis("XAxis") > 0.5f)
        {
            nextpos = 1;
        }
        else
        {
            nextpos = 0;
        }
        */

        nextpos = Input.GetAxis("Horizontal");
        //Debug.Log(nextpos);
        //charAnim.SetFloat("nextpos", nextpos);
        if (nextpos != curpos){
            charAnim.Play("character_l");
        }
        //else if (nextpos > 0){
        //    charAnim.Play("character_l");
        //}
        //else{
        //    charAnim.Play("routine");
        //}



        /*
        
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown("left"))
        {
            if (nextpos - 1 >= -1)
            {
                nextpos = nextpos - 1;
                start_shift_time = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton8) || Input.GetKeyDown("right"))
        {
            if (nextpos + 1 <= 1)
            {
                nextpos = nextpos + 1;
                start_shift_time = Time.time;
            }
        }
        */

        //if (Math.Abs(Input.GetAxis("DPadX")) < EPSILON)
        //{
        //    AxisActive = false;
        //}


        if ((Input.GetKeyDown(KeyCode.JoystickButton16) || Input.GetKeyDown(KeyCode.Space)) && !jumping )
        {
            Debug.Log("test jump");
            start_jump_time = Time.time;
            jumping = true;
        }


        // =========================================================================================================


        if ((Input.GetKeyDown(KeyCode.JoystickButton17) || Input.GetKeyDown("up")) && (score/10)>0) {
        // if (Input.GetKeyDown("up") && (score/10)>0  ) {
            charAnim.Play("Throw");
            ThrowSnowBall tscpt = this.transform.Find("Snowballstartpoint").GetComponent<ThrowSnowBall>();
            tscpt.makeSnowBall();

            score -= 10;

        }

       
        // charAnim.SetBool("jumping", jumping);
        // if (jumping){
        //     charAnim.Play("character_jump");
        // }
        /*
        if (Input.inputString != "") Debug.Log(Input.inputString);


       
        float speed = 20;
        distanceTravelled += speed * Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown("left"))
        if (AxisActive == false && ((Input.GetAxis("DPadX") == -1f) || Input.GetKeyDown("left")))
        {
            AxisActive = true;
            if (position == 2)
            {
                pathCreatorSelected = pathCreatorMiddle;
                position = 0;
            } else if (position == 0)
            {
                pathCreatorSelected = pathCreatorLeft;
                position = -2;
            }
        }
        //if (Input.GetKeyDown(KeyCode.JoystickButton8) ||Input.GetKeyDown("right"))
        if (AxisActive == false && ((Input.GetAxis("DPadX") == 1f)|| Input.GetKeyDown("right")))
        {
            AxisActive = true;
            if(position == -2)
            {
                pathCreatorSelected = pathCreatorMiddle;
                position = 0;
            } else if (position == 0)
            {
                pathCreatorSelected = pathCreatorRight;
                position = 2;
            }
        }

        if (Input.GetAxis("DPadX") == 0f)
        {
            AxisActive = false;
        }
        
        transform.position = pathCreatorSelected.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreatorSelected.path.GetRotationAtDistance(distanceTravelled);
    */
        if (Input.GetKeyDown(KeyCode.UpArrow) && tone < 2)
        {
            tone += 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && tone > -2)
        {
            tone -= 1;
        }
        if (tone == 0)
        { 
            tone_text.text = "";
        }
        else if (tone > 0)
        {
            tone_text.text = new String('+', tone);
        }
        else if (tone < 0) {
            tone_text.text = new String('-', -tone);
        }
    }


    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "music_note")
        {
            score += 1;
            // energy_txt.text = "Snowballs collected: " + score/10;
            Music_Note_scpt mns = other.gameObject.GetComponent<Music_Note_scpt>();
            mns.hit();
            int index = Character.score % 11;
            if (score%10 == 0){
                AudioSource snow_ball = GameObject.Find("MusicSnowBall").GetComponent<AudioSource>();
                snow_ball.Play();
            } else {
                AudioSource snow_flake = GameObject.Find("MusicSnowFlake").GetComponent<AudioSource>();
                snow_flake.Play();
            }
            // AudioSource ice_break_long = GameObject.Find("MusicSnowFlake").GetComponent<AudioSource>();
            Snow_balls_ui.hit(index);
        }
        else if (other.tag == "breakable_ice") {
            charAnim.Play("Die");
            Debug.Log("breakable_ice");
            // if (score < 10)
            // {
            Debug.Log("You die");
            GameOverTime = Time.time;
            isGameOver = true;
            // other.transform.parent.gameObject.GetComponent<GameController>().EndGame(false);
            move_logic = 1;
            // } else 
            // {
            //     Debug.Log("breakable_ice: break");
            //     score -= 10;
                
            //     Destroy(other.gameObject);
                //GameObject Tree_w_Icicles= other.gameObject.transform.Find("Tree_w_Icicles").gameObject;
                //Destroy(Tree_w_Icicles);
            // }
        }
        else if (other.tag == "unbreakable_barrier") {
            //Debug.Log("unbreakable_barrier");
            charAnim.Play("Die");
            Debug.Log("You die");
            GameOverTime = Time.time;
            isGameOver = true;
            // other.gameObject.GetComponent<GameController>().EndGame(false);
            move_logic = 1;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ice_fall")
        {

            //撞到ice的
            Debug.Log("ice_fall");
            charAnim.Play("Die");
            GameOverTime = Time.time;
            isGameOver = true;
            move_logic = 1;
            // other.gameObject.GetComponent<GameController>().EndGame(false);
        } else if (other.gameObject.tag == "end_line")
        {
            charAnim.Play("Win");
            isGameOver = true;
            move_logic = 1;
            is_win = true;
            GameOverTime = Time.time;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "gap_enter") {

            //进入gap
            Debug.Log("gap_enter");
            GameOverTime = Time.time;
            isGameOver = true;
            move_logic = 2;
            AudioSource music_fall = GameObject.Find("MusicFall").GetComponent<AudioSource>();
            music_fall.Play();
            // other.gameObject.GetComponent<GameController>().EndGame(false);
        }
    }

    public static float getLogiHeight(float x) {
        return 0.5f * -x * (x - 2);
    }
    

}
