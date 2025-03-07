using FoodieR.Models.DbObject;
using Microsoft.AspNetCore.Identity;
using System;

namespace FoodieR.Test;

public class ReviewTests
{
    [Fact]//rularea de catre runnerul de testare
    //prepare act assert
    public void CreateReview_ReviewProduct_ProductHasAReview()
    {
        //verificare dacă review-ul este creat corect și conține datele așteptate.
       
        //arrange
        var review = new Review//instantiem un review
        {
             Content= "Great product!",
             Rating = 5
        };

        //act=actionare

        //assert=afirmam
        Assert.NotNull(review);
        Assert.Equal("Great product!", review.Content);
        Assert.Equal(5, review.Rating);
    }

    //Testează dacă review-ul este asociat corect unui utilizator.
    [Fact]
    public void CreateReview_AssignedToUser_UserIsSetCorrectly()
    {
        // Arrange
        var user = new IdentityUser//creez un user
        {
            Id = "12345",
            UserName = "testuser"
        };

        var review = new Review//creez un review
        {
            Id = Guid.NewGuid(),
            Title = "Great Experience",
            Content = "Loved the product!",
            Rating = 5,
            CreatedBy = user
        };

        // Act & Assert
        Assert.NotNull(review.CreatedBy);//Verifică că CreatedBy nu este null 
        Assert.Equal(user, review.CreatedBy);//verifica ca referința către utilizator este corectă
        Assert.Equal("12345", review.CreatedBy.Id);//compara id uluser-ului din review
        Assert.Equal("testuser", review.CreatedBy.UserName);//compara userName-ul uluser-ului din review
    }


    //Testarea modificării unui review: simulez modificarea unui review și verifică dacă Modified primește o valoare.
    [Fact]
    public void UpdateReview_ModifiedDateIsSet()
    {
        // Arrange
        var review = new Review
        {
            Content = "Tasty",
            Created = DateTime.Now
        };

        // Act
        review.Content = "Very Tasty";
        review.Modified = DateTime.Now;

        // Assert
        Assert.Equal("Very Tasty", review.Content);
        Assert.NotNull(review.Modified);
    }

}
