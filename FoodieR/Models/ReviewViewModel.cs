using FoodieR.Models.DbObject;
using System.ComponentModel.DataAnnotations;

namespace FoodieR.Models
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int Rating { get; set; }//stelutele de tip nr intreg, impicit au valoarea minim 1, de asta nu e necesar Required

        [Required]
        public string Subject { get; set; }//produsul pentru care se lasa recenzia

        public string CreatedById { get; set; }//Id-ul utilizatorului, nu toate datele lui

        [Display(Name = "Created by")]//afisarea etichetei in mod dezlegat- nu asa CreatedBy
        public string CreatedByUser { get; set; }//user-name-ul= emailul utilizatorului

        public bool HasEditAndDeletePermissions { get; set; }//proprietate care sa verifice daca avem permisiunea sa editam/stergem un Review

        public int ProductId { get; set; }
        //metoda pentru a converti de la ReviewViewModel la Review 
        public static ReviewViewModel FromEntity(Review entity)//de la Review la ReviewViewModel; ce facea Bind, noi facem aici: mapam doar proprietatile care ne sunt necesare- pe cale pe care avem nevoie sa le transmitem de la si la View; dintr-un Review obtinem in ReviewViewModel(adica clasa curenta in care lucram)
        {
            return new ReviewViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                Created = entity.Created,
                Modified = entity.Modified,
                Rating = entity.Rating,
                Subject = entity.Subject,
                CreatedByUser = entity.CreatedBy?.UserName,//? CreatedBy ne asiguram ca nu poate fi null in Input; mail
                CreatedById = entity.CreatedBy?.Id,
                ProductId = entity.Product?.Id ?? 0
            };
        }

        //metoda pt a converti de la ReviewViewModel la o entitate
        public Review ToEntity()//convertim ReviewViewModel intr- o entitate ToEntity(); La baza de date trebuie sa anunga un Review nu un ReviewViewModel, de asta facem conversia folosind metodele punse in Clasa ReviewViewModel
        {
            return new Review
            {
                Id = Id,
                Title = Title,
                Content = Content,
                Created = Created,
                Modified = Modified,
                Rating = Rating,
                Subject = Subject,
            };
        }

    }
}
