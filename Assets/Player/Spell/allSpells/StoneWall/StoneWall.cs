using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWall : Spell
{
    private Player _player;
    private Camera _camera;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _camera = FindObjectOfType<Camera>();
    }
    
    public override float CastSpell()
    {
        if (!EnoughMana())
        {
            return -1;
        }
        PlaceWall();
        return base.CastSpell();
    }

    private void PlaceWall()
    {
        var tr = _player.transform;
        var pos = tr.position;
        
        Vector2 relativeMousePos = _camera.ScreenToWorldPoint(Input.mousePosition) - pos;
        Vector2 dir = relativeMousePos / Mathf.Max(Mathf.Abs(relativeMousePos.x), Mathf.Abs(relativeMousePos.y));
        
        Vector3 wPos = pos + new Vector3(dir.x * 1.5f, dir.y * 1.5f, 0);
        float wRot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        var rotation = tr.rotation;

        for (int i = -1; i < 2; i++)
        {
            var wp = new Vector3(wPos.x, wPos.y + i, wPos.z);
            GameObject stoneWall = Instantiate(gameObject, wp, rotation);
        
            Animator animator = stoneWall.GetComponent<Animator>();
            animator.enabled = true;

            stoneWall.GetComponent<Collider2D>().enabled = true;
            SpriteRenderer spriteRenderer = stoneWall.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;
            spriteRenderer.flipX = wRot is < -90 or > 90 ;
            stoneWall.GetComponent<StoneWall>().isMain = false;
        
            StartCoroutine(CastTime(0.4f, animator));
            StartCoroutine(IdleFor(spellData.lifeTime, animator));           
        }
        
    }
        private IEnumerator CastTime(float time, Animator animator)
        {
            yield return new WaitForSeconds(time);
            animator.SetBool("Idle", true);
        }

        private IEnumerator IdleFor(float time, Animator animator)
        {
            yield return new WaitForSeconds(time);
            animator.SetTrigger("Dissepear");
            StartCoroutine(DestroyAfter(0.4f, animator.gameObject));
        }
}
