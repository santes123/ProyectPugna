using UnityEngine;

public class AnimationControllerScript : MonoBehaviour
{
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        //StartCoroutine(CicloAnimacionBoss());
    }
    
    public System.Collections.IEnumerator CicloAnimacionBoss()
    {
        while (true)
        {
            animator.SetBool("HandLeftUpRightDown", true);
            animator.SetBool("HandRightUpLeftDown", false);
            yield return new WaitForSeconds(1f); // Ajusta el tiempo que deseas que las manos estén arriba

            animator.SetBool("HandLeftUpRightDown", false);
            animator.SetBool("HandRightUpLeftDown", true);
            yield return new WaitForSeconds(1f); // Ajusta el tiempo que deseas que las manos estén abajo
        }
    }
    /*private System.Collections.IEnumerator UpHands()
    {
        yield return new WaitForSeconds(1f); // Ajusta el tiempo que deseas que las manos estén arriba

        animator.SetBool("HandLeftUpRightDown", true);
        animator.SetBool("HandRightUpLeftDown", false);

    }
    private System.Collections.IEnumerator DownHands()
    {
        yield return new WaitForSeconds(1f); // Ajusta el tiempo que deseas que las manos estén arriba

        animator.SetBool("HandLeftUpRightDown", false);
        animator.SetBool("HandRightUpLeftDown", true);

    }*/
    /*public void UpHands()
    {
        animator.SetBool("HandLeftUpRightDown", true);
        animator.SetBool("HandRightUpLeftDown", false);
    }
    public void DownHands()
    {
        animator.SetBool("HandLeftUpRightDown", false);
        animator.SetBool("HandRightUpLeftDown", true);
    }*/
}
