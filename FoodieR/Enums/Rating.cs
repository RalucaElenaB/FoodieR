using System.ComponentModel.DataAnnotations;

namespace FoodieR.Enums;

//un Enumeration cu valori pentru 'stelele' din aplicatie
//pentru steluta voi folosi un emoji nu un jpg
public enum Rating
{
    [Display(Name = "⭐")]//atribut pus pe o proprietate, ce va folosi si va afisa pe ecran valoarea "⭐"
    One = 1,//in baza de date va ajunge cifra; in cod vom folosi One; pe ecran vor aparea stelutele; modificari in View ca sa folosim Enum
    [Display(Name = "⭐⭐")]
    Two = 2,
    [Display(Name = "⭐⭐⭐")]
    Three = 3,
    [Display(Name = "⭐⭐⭐⭐")]
    Four = 4,
    [Display(Name = "⭐⭐⭐⭐⭐")]
    Five = 5
}
