using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum doorState
{
    intact, // нетронутый
    broken  // сломана
}

public class Door_Scr: MonoBehaviour, IDamageabel
{
    private int             _doorHp = 30;           // Здоровье двери
    public BoxCollider2D    doorCollider;           // Ссылка на коллайдер
    public SpriteRenderer   doorSprite;             // Ссылка на спрайт

    public GameObject       came;                   // Ссылка на главную камеру

    public AudioSource      audioSourse;            // Ссылка на аудио соурс


    doorState               state = doorState.intact;

    public int doorHp
    {
        get
        {
            return (_doorHp);
        }
        set
        {
            _doorHp = value;
            if(_doorHp <= 0)
            {
                DoorDestroy();
            }
        }
    }

    public void Start ()
    {
        this.gameObject.AddComponent<AudioSource>();                // Прикрепить компонент воспроизведения звуков
        audioSourse = this.gameObject.GetComponent<AudioSource>();  // Сделать быструю ссылку на компонент воспроизведения звуков
        audioSourse.volume = 0.3f;                                  // Снизить уровень громкости звуков до 30%
        audioSourse.spatialBlend = 1;                               // Сделать звук 3D

        doorSprite = GetComponentInParent<SpriteRenderer>();
        doorCollider = transform.parent.gameObject.GetComponent<BoxCollider2D>();
        came = GameObject.FindGameObjectWithTag("MainCamera");

    }
    public void OnTriggerStay2D (Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (state == doorState.broken && collision.gameObject.GetComponent<Player_Scr>().coin >= 1)      // Если дверь была разрушена и у игрока есть деньги на ремонт
            {
                if(_doorHp <= 0)
                {
                    collision.gameObject.GetComponent<Player_Scr>().coin -= 1;
                    came.GetComponent<Enemy_Spawner_Scr>().score += 1;
                }
                _doorHp = 30;
                doorCollider.enabled = true;
                doorSprite.enabled = true;
                PLAY_AUDIO(1);                  // Воспроизвести звук ремонта
                state = doorState.intact;       // Перевести в состояние "нетронута"
            }

        }
    }

    // Выполняет звуковое сопровождение
    public void PLAY_AUDIO (int ch)
    {
        switch(ch)
        {
            case 0: // Когда дверь сломалась
                audioSourse.PlayOneShot(came.GetComponent<Create_Map_Scr>().doorFalls[0]);
                break;
            case 1: // Когда дверь отремонтировали
                audioSourse.PlayOneShot(came.GetComponent<Create_Map_Scr>().doorRepair[Random.Range(0, 1)]);
                break;
            case 2: // Когда бьют по двери
                audioSourse.PlayOneShot(came.GetComponent<Create_Map_Scr>().doorHitting[Random.Range(0, came.GetComponent<Create_Map_Scr>().doorHitting.Count - 1)]);
                break;
        }
    }

    public void ApplyDamage (int damageValue)
    {
        doorHp -= damageValue;
        PLAY_AUDIO(2);
    }
    void DoorDestroy ()
    {
        _doorHp = 0;
        doorCollider.enabled = false;       // отключить коллайдер
        doorSprite.enabled = false;         // отключить отображение двери

        if(state == doorState.intact)
        {    // Если до этого было состояние "нетронутый"
            PLAY_AUDIO(0);                  // Произвести звук разрушения
            state = doorState.broken;       // Изменить состояние двери на "сломана"
        }
    }

}
