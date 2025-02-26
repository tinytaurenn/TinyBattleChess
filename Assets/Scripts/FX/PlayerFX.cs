using UnityEngine;

public class PlayerFX : EntityFX
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void PlayHurtFX(int choiceIndex)
    {

        m_HurtFX.Play();
    }
    

    public override void PlayParryFX(int choiceIndex)
    {


    }
}
