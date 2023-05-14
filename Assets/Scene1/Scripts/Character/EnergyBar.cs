using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Character character;
    public RectTransform energy_Bar, energy_Black,energy_Board;
    private Image Bar, Board;
    float BarWidth,EnergyRatio;
    float BarOriginPosX, BlackOriginPosX;
    void Start()
    {
        BarWidth = energy_Black.rect.width - 65;
        BarOriginPosX = energy_Bar.localPosition.x;
        BlackOriginPosX = energy_Black.localPosition.x;
        BrightBarOrigin_Scale = BarShine_fbx.localScale;
        Bar = energy_Bar.GetComponent<Image>();
        Board = energy_Board.GetComponent<Image>();
        EnergyCache = character.Energy;
        Bar.color = new Vector4(Bar.color.r, Bar.color.g, Bar.color.b, 0);
        Board.color = new Vector4(Board.color.r, Board.color.g, Board.color.b, 0);
    }

    
    void Update()
    {
        EnergyRatio = character.Energy / character.MaxEnergy;
        energy_Black.localPosition = new Vector2(BlackOriginPosX - BarWidth * (1 - EnergyRatio), energy_Black.localPosition.y);
        energy_Bar.localPosition = new Vector2(BarOriginPosX + BarWidth * (1 - EnergyRatio), energy_Bar.localPosition.y);
        BarTransparentFunction();
    }


    public GameObject BarShine;
    public RectTransform BarShine_fbx;
    float BarTransparent_Delay = 0.3f;
    float BarTransparent_Counter = 0;
    float BarOrigin_Alpha = 0.8f;
    float BoardOrigin_Alpha = 0.35f;
    float EnergyCache;
    Vector3 BrightBarOrigin_Scale;
    private void BarTransparentFunction()
    {
        BarTransparent_Counter += Time.deltaTime;

        if (character.Energy < EnergyCache)
        {            
            if (Bar.color.a < BarOrigin_Alpha) 
            {
                Bar.color = new Vector4(Bar.color.r, Bar.color.g, Bar.color.b, Bar.color.a + Time.deltaTime * 3);
            }         
            if (Board.color.a < BoardOrigin_Alpha)
            {
                Board.color = new Vector4(Board.color.r, Board.color.g, Board.color.b, Board.color.a + Time.deltaTime * 2);
            }
            if (Bar.color.a > 0.5f)
            {
                BarShine.SetActive(true);
            }
            else
            {
                BarShine.SetActive(false);
            }
            BarShine_fbx.localScale = BrightBarOrigin_Scale - new Vector3((1-(character.Energy/character.MaxEnergy))* BrightBarOrigin_Scale.x*1.048f, 0,0);
            BarTransparent_Counter = 0;
        }
        else if (character.Energy >= EnergyCache && character.Energy < character.MaxEnergy)
        {
            BarShine.SetActive(false);
            if (Bar.color.a > BarOrigin_Alpha / 2.5f)
            {
                Bar.color = new Vector4(Bar.color.r, Bar.color.g, Bar.color.b, Bar.color.a - Time.deltaTime * 1f);
            }
            if (Board.color.a > BoardOrigin_Alpha / 2.5f)
            {
                Board.color = new Vector4(Board.color.r, Board.color.g, Board.color.b, Board.color.a - Time.deltaTime * 0.6f);
            }
            BarTransparent_Counter = 0;
        }
        else if (character.Energy >= character.MaxEnergy && BarTransparent_Counter > BarTransparent_Delay)
        {
            if (Bar.color.a > 0)
            {
                Bar.color = new Vector4(Bar.color.r, Bar.color.g, Bar.color.b, Bar.color.a - Time.deltaTime * 2f);
            }
            if (Bar.color.a > 0)
            {
                Board.color = new Vector4(Board.color.r, Board.color.g, Board.color.b, Board.color.a - Time.deltaTime * 1.2f);
            }
        }
        EnergyCache = character.Energy;

    }
}
