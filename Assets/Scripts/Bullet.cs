using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip audioExplosion;
    public AudioClip audioFire;
    public GameObject explosionEffect;

    private AudioSource audioSourceBullet;
    private float speed = 8f;            // 탄알 이동 속도

    private Rigidbody bulletRigidbody;  // 이동에 사용할 리지드바디 컴포넌트
    
    // Start is called before the first frame update
    void Start()
    {
        // 게임 오브젝트에서 Rigidbody 컴포넌트를 찾아 bulletRigidbody에 할당
        bulletRigidbody = GetComponent<Rigidbody>();
        // 리지드바디의 속도 = 앞쪽 방향 * 이동 속도
        bulletRigidbody.velocity = transform.forward * speed;

        // 3초 뒤에 자신의 게임 오브젝트 파괴
        Destroy(gameObject, 3f);

        audioSourceBullet = GetComponent<AudioSource>();
        audioSourceBullet.clip = audioFire;
        if (SoundControl.bSoundOn)
            audioSourceBullet.Play();
        
    }

    // 트리거 충돌 시 자동으로 실행되는 메서드
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 상대방 게임 오브젝트가 player 태그를 가진 경우
        if (other.tag == "Player")
        {
            // 상대방 게임 오브젝트에서 PlayerController 컴포넌트 가져오기
            PlayerController playerController = other.GetComponent<PlayerController>();

            // 상대방으로 부터 PlayerController 컴포넌트를 가져오는데 성공했다면
            if (playerController != null)
            {
                // 상대방 PlayerController 컴포넌트의 Die() 메서드 실행
                playerController.Die();
                
                // 폭발 사운드 재생
                if (SoundControl.bSoundOn)
                {
                    audioSourceBullet.clip = audioExplosion;
                    audioSourceBullet.Play();
                }

                // 총알 위치 정지
                bulletRigidbody.velocity = transform.forward * 0;

                // 폭발 효과 재생
                explosionEffect.SetActive(true);
                
            }
        }
    }
}
