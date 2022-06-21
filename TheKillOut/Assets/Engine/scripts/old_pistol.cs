using System.Collections;
using UnityEngine;

public class old_pistol : MonoBehaviour
{







    public GameObject Equipment_holder;
    public GameObject shoot_handle;

    public GameObject recyle_particles_performance;

    public void Start()
    {
        shoot_handle = player.GetComponent<player_controller>().shoot_handle;
        recyle_particles_performance = GameObject.FindGameObjectWithTag("rycle");
        
        change_equipment();
        _3rd_view = player.GetComponent<player_controller>()._3rd;


    }
    private void OnEnable()
    {
        in_reload = false;
        in_shoot = false;
    }




    float Power_bolt;
    public GameObject player;

    bool running;
    bool walking;
    bool walking_side;
    bool duck_walk;
    bool reload;

    public bool _3rd_view;
    public Vector3 _3rd_view_cam;

    void Update()
    {
        
        Input_Status();

        // Управление от игрока

        running = player.GetComponent<player_controller>().running;
        walking = player.GetComponent<player_controller>().walking;
        walking_side = player.GetComponent<player_controller>().walking_side;
        duck_walk = player.GetComponent<player_controller>().duck_walk;
        reload = player.GetComponent<player_controller>().reload;
        cam_toggled = player.GetComponent<player_controller>().cam_toggled;


        // Отдача
        if (Power_bolt > -1)
        {
            Power_bolt -= 0.15f;

        }


        // Возврат отдача
        if (Power_bolt < -1)
        {
            Power_bolt = -1;
        }

     

        ani.SetFloat("Power_bolt", Power_bolt);







        // Стрельба
        if (button_shoot && !in_shoot && magazine_current > 0 && !in_reload && !running)
        {
            in_shoot = true;
            finished_shoot = false;
            shooting = StartCoroutine(shoot());


        }
        // Окончание стрельбы
        if (!button_shoot && finished_shoot)
        {
            in_shoot = false;
            current_spread = 0;
        }

       
        if (button_aim && !button_shoot && !in_reload && magazine_current > 0)
        {
            // Прицеливание
            ani.SetInteger("old_pistol", 5);
        }
        
        if (!button_aim && !button_shoot && !in_reload && magazine_current > 0)
        {
            // Покой
            ani.SetInteger("old_pistol", 0);
        }


        // Прицеливание с пустым магазином
        if (button_aim && !button_shoot && !in_reload && magazine_current < 1)
        {
            ani.SetInteger("old_pistol", 6);
        }
        if (!button_aim && !button_shoot && !in_reload && magazine_current < 1)
        {
            ani.SetInteger("old_pistol", 1);
        }






        // Состояние ходьбы
        if (walking && !in_reload && magazine_current > 0)
        {
            if (!button_aim)
            {
                ani.SetInteger("old_pistol", 3);

                if (duck_walk)
                {
                    ani.SetFloat("weapon_speed", 0.5f);
                }
                else
                {
                    ani.SetFloat("weapon_speed", 1);
                }
                if (walking_side)
                {
                    ani.SetFloat("weapon_speed", 0.8f);
                }
            }

            if (button_aim)
            {
                ani.SetInteger("old_pistol", 8);

                if (duck_walk)
                {
                    ani.SetFloat("weapon_speed", 0.5f);
                }
                else
                {
                    ani.SetFloat("weapon_speed", 1);
                }
            }
        }

        if (walking && !in_reload && magazine_current < 1)
        {
            if (!button_aim)
            {
                ani.SetInteger("old_pistol", 4);

                if (duck_walk)
                {
                    ani.SetFloat("weapon_speed", 0.5f);
                }
                else
                {
                    ani.SetFloat("weapon_speed", 1);
                }
                if (walking_side)
                {
                    ani.SetFloat("weapon_speed", 0.8f);
                }
            }

            if (button_aim)
            {
                ani.SetInteger("old_pistol", 9);

                if (duck_walk)
                {
                    ani.SetFloat("weapon_speed", 0.5f);
                }
                else
                {
                    ani.SetFloat("weapon_speed", 1);
                }
            }
        }






        if (running && !in_reload && magazine_current > 0)
        {
            ani.SetInteger("old_pistol", 12);



        }
        if (running && !in_reload && magazine_current < 1)
        {
            ani.SetInteger("old_pistol", 13);



        }



        if (button_aim && button_shoot && !running && !in_reload && magazine_current > 0)
        {
            ani.SetInteger("old_pistol", 7);
        }

        if (!button_aim && button_shoot && !running && !in_reload && magazine_current > 0)
        {
            ani.SetInteger("old_pistol", 2);
        }

        if (reload && !in_reload && stored_bullets > 0 && magazine_current != magazine_max)
        {
            reloading = StartCoroutine(reload_start());

        }



        cam_equipment();
    }


    bool button_shoot;
    bool button_aim;
    bool key_reload;
    bool cam_toggled;


    public void Input_Status()
    {



        if (Input.GetButton("Fire1"))
        {
            button_shoot = true;
        }
        else
        {
            button_shoot = false;
        }

        if (cam_toggled)
        {
            _3rd_view = true;
            player.GetComponent<player_controller>()._3rd = false;
            player.GetComponent<player_controller>().head_3rd_status();
        }
        else
        {
            _3rd_view = false;
            player.GetComponent<player_controller>()._3rd = true;
            player.GetComponent<player_controller>().head_3rd_status();
        }


        if (Input.GetButton("Fire2"))
        {
            button_aim = true;
        }
        else
        {
            button_aim = false;
        }


        if (Input.GetKey(KeyCode.R))
        {
            key_reload = true;
        }
        else
        {
            key_reload = false;
        }

       

        if (_3rd_view)
        {
            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, _3rd_view_cam, 15 * Time.deltaTime);

        }
    }





    public GameObject Shoot_start_point;
    public GameObject muzzle;


    float Push_bolt;



    bool in_shoot;
    Coroutine shooting;
    public bool full_auto;
    public float firerate;
    bool finished_shoot;


    public bool silence;
    public AudioClip silence_shoot_sound;
    public AudioClip shoot_sound;




    public int Joule;



    public float vertical_force;
    public float horizontal_force;

    public float spread_height;
    public float current_spread;




    public IEnumerator shoot()
    {

        yield return new WaitForSeconds(0);





        current_spread += spread_height;

        Vector3 Add_spread = Shoot_start_point.transform.forward;

        float hor = Random.Range(-current_spread, current_spread);
        float ver = Random.Range(-current_spread, current_spread);



        Add_spread = new Vector3(hor, ver, 0);




        // Возврат анимации
        Power_bolt = 1;



        bullet_drop();


        // Отдача

        float add_ver_force = UnityEngine.Random.Range(0, vertical_force);
        float add_hor_force = UnityEngine.Random.Range(-horizontal_force, horizontal_force);

        // Полная отдача
        if (!suppressor_a_bool  && !suppressor_c_bool && !suppressor_d_bool)
        {
            player.GetComponent<player_controller>().vertical_float_spread = -add_ver_force;
            player.GetComponent<player_controller>().horizontal_float_spread = add_hor_force;
        }


        //  Меньше отдача
        if (suppressor_a_bool || suppressor_c_bool || suppressor_d_bool)
        {
            player.GetComponent<player_controller>().vertical_float_spread = -add_ver_force / 3;
            player.GetComponent<player_controller>().horizontal_float_spread = add_hor_force / 3;
        }


        GameObject spawned_muzzle = Instantiate(muzzle, Shoot_start_point.transform.position, Shoot_start_point.transform.rotation);
        spawned_muzzle.GetComponent<muzzle_flash>().origin = Shoot_start_point;


        // Звук стрельбы
        if (!suppressor_a_bool && !suppressor_c_bool && !suppressor_d_bool)
        {
            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = shoot_sound;
            g.GetComponent<AudioSource>().maxDistance = 100;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;
        }
        if (suppressor_a_bool || suppressor_c_bool || suppressor_d_bool)
        {
            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = silence_shoot_sound;
            g.GetComponent<AudioSource>().maxDistance = 100;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;

            
            Add_spread -= (Add_spread / 3);
        }





        RaycastHit hit;



        // Стрельба из камеры
        shoot_handle.GetComponent<shoot_handle>().register_shoot(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward - Add_spread), Joule);

        if (full_auto)
        {
            yield return new WaitForSeconds(firerate);
        }



        magazine_current -= 1;



        if (magazine_current > 0 && full_auto && button_shoot)
        {
            StopCoroutine(shoot());
            shooting = StartCoroutine(shoot());
        }

        if (!full_auto)
        {
            finished_shoot = true;
            StopCoroutine(shoot());
        }

        if (!button_shoot)
        {
            finished_shoot = true;
            StopCoroutine(shoot());
        }

    }

    public int magazine_max;
    public int magazine_current;
    public int stored_bullets;
    public AudioClip reload_unempty;
    public AudioClip reload_empty;


    Coroutine reloading;
    bool in_reload;
    float reload_time;
    bool finished_reload_in_reload;

    public GameObject Clip_on_point;  
    public IEnumerator reload_start()
    {


        in_reload = true;
        finished_reload_in_reload = false;


        if (magazine_current > 0)
        {
            reload_time = 1.37f;
            ani.SetInteger("old_pistol", 10); 

            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = reload_unempty;
            g.GetComponent<AudioSource>().maxDistance = 15;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;
        }
        else
        {
            reload_time = 1.16f;
            ani.SetInteger("old_pistol", 11); 

            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = reload_empty;
            g.GetComponent<AudioSource>().maxDistance = 15;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;
        }


        yield return new WaitForSeconds(reload_time);


        if (stored_bullets < magazine_max && !finished_reload_in_reload)
        {
            finished_reload_in_reload = true;
            stored_bullets += magazine_current;
            magazine_current = 0;
            magazine_current = stored_bullets;
            stored_bullets = 0;
        }

 
        if (stored_bullets > magazine_max || stored_bullets == magazine_max && !finished_reload_in_reload)
        {
            finished_reload_in_reload = true;
            stored_bullets += magazine_current;
            magazine_current = 0;
            magazine_current = magazine_max;
            stored_bullets -= magazine_max;
        }
  
        finished_shoot = true;
        in_reload = false;


        StopCoroutine(reload_start());

    }



    public GameObject Cam;
    public Animator ani;


    
    public Vector3 idle_cam;
    public Vector3 aim_cam;
    public Vector3 run_cam;




    public bool lamp_laser_a_bool;
    public bool lamp_laser_b_bool;
    public bool laser_a_bool;


    public bool suppressor_a_bool;
    public bool suppressor_clusterSub_bool;
    public bool suppressor_c_bool;
    public bool suppressor_d_bool;




    public GameObject lamp_laser_a;
    public GameObject lamp_laser_b;
    public GameObject laser_a;


    public GameObject suppressor_a;
    public GameObject suppressor_c;
    public GameObject suppressor_d;


    public Vector3 red_dot_a_cam;
    public bool red_dot_a_bool;
    public GameObject red_dot_a;









    public Vector3 shoot_origin_none_suppressor;
    public Vector3 shoot_origin_suppressor;

    public void cam_equipment()
    {

        // Переключение между видами


        if (_3rd_view)
        {
            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, _3rd_view_cam, 15 * Time.deltaTime);

            return;
        }




        if (in_reload)
        {
            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, idle_cam, 15 * Time.deltaTime);
        }
        if (!button_aim && !running && !in_reload) //Покой
        {

            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, idle_cam, 15 * Time.deltaTime);


        }
        if (button_aim && !running && !in_reload && !red_dot_a_bool) //Прицеливание
        {
            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, aim_cam, 15 * Time.deltaTime);

        }
        if (running && !in_reload)
        {
            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, run_cam, 15 * Time.deltaTime);
        }


        if (red_dot_a_bool && button_aim)
        {
            Cam.transform.localPosition = Vector3.Lerp(Cam.transform.localPosition, red_dot_a_cam, 15 * Time.deltaTime);
        }


    }


    public void change_equipment()
    {




        // Отключение снаряжения


        
        Equipment_holder.SetActive(false);

        lamp_laser_a.SetActive(false);
        lamp_laser_b.SetActive(false);
        laser_a.SetActive(false);


        suppressor_a.SetActive(false);
        suppressor_c.SetActive(false);
        suppressor_d.SetActive(false);

        red_dot_a.SetActive(false);


        

        if (red_dot_a_bool)
        {
            red_dot_a.SetActive(true);
          
            Equipment_holder.SetActive(true);
        }




        if (lamp_laser_a_bool)
        {
            lamp_laser_a.SetActive(true); Equipment_holder.SetActive(true);

        }
        if (lamp_laser_b_bool)
        {
            lamp_laser_b.SetActive(true); Equipment_holder.SetActive(true);

        }
        if (laser_a_bool)
        {
            laser_a.SetActive(true); Equipment_holder.SetActive(true);

        }




        if (suppressor_a_bool)
        {
            suppressor_a.SetActive(true);

        }

        if (suppressor_c_bool)
        {
            suppressor_c.SetActive(true);

        }
        if (suppressor_d_bool)
        {
            suppressor_d.SetActive(true);

        }





        
        if (suppressor_a_bool || suppressor_c_bool || suppressor_d_bool)
        {

            Shoot_start_point.transform.localPosition = shoot_origin_suppressor;
        }
        if (!suppressor_a_bool && !suppressor_c_bool && !suppressor_d_bool)
        {

            Shoot_start_point.transform.localPosition = shoot_origin_none_suppressor;
        }




    }



    public void turn_off_weapon()
    {

        // Отключение снаряжения

        ani.SetInteger("old_pistol", -1);


        Equipment_holder.SetActive(false);

        red_dot_a.SetActive(false);

        lamp_laser_a.SetActive(false);
        lamp_laser_b.SetActive(false);
        laser_a.SetActive(false);


        suppressor_a.SetActive(false);

        suppressor_c.SetActive(false);
        suppressor_d.SetActive(false);




    }


    public GameObject bullet_shell;
    public GameObject Bullet_shell_output;
    public Vector3 Bullet_shell_output_position;

    public void bullet_drop()
    {

        bullet_shell.transform.localPosition = Bullet_shell_output_position;

        GameObject b = Instantiate(bullet_shell, Bullet_shell_output.transform.position, Bullet_shell_output.transform.rotation);

        b.GetComponent<Rigidbody>().AddForce(Bullet_shell_output.transform.right + Bullet_shell_output.transform.up, ForceMode.Impulse);


    }


















}
