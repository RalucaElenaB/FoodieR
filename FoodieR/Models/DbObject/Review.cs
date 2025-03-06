using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models.DbObject
{
    public class Review//toate datele(proprietatile de mai jos vin din baza de date prin entitatea Review)
    {
        public Guid Id { get; set; }//Guid identificator unic
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int Rating { get; set; }//stelutele de tip nr intreg
        public string Subject { get; set; }//produsul pentru care se lasa recenzia

        [Required]
        public Product Product { get; set; }//Relație cu produsul
        public IdentityUser CreatedBy { get; set; }//utilizatorul care a creat recenzia; IdentityUser vine din using Microsoft.AspNetCore.Identity; proprietate de tipul IdentityUser- legatura dintre tabela Review si User; doar prop asta nu o vom include in ReviewViewModel
    }
}