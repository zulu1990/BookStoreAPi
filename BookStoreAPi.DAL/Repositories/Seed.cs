using BookStoreApi.Contracts.Entity;
using BookStoreAPi.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreAPi.DAL.Repositories
{
    public static class SeedData
    {
        public static void SeedBooks(ApplicationDbContext context)
        {
            if (!context.Books.Any())
            {

                var books = InitialBooks();
                context.Books.AddRange(books);

                context.SaveChanges();
            }
        }



        public static List<Book> InitialBooks()
        {
            var books = new List<Book>();

            var identifier = Guid.NewGuid();



            for (int i = 0; i < 20; i++)
            {
                var poeBook = new Book()
                {
                    Author = "Allan Poe",
                    Price = 15.45m,
                    BookIdentifier = identifier,
                    Genre = "Horror",
                    Title = "Red Death"
                };

                books.Add(poeBook);
            }

            identifier = Guid.NewGuid();
            

            for (int i = 0; i < 25; i++)
            {
                var shagreenBook = new Book
                {
                    Author = "Honore de Balzac",
                    Price = 20.0m,
                    BookIdentifier = identifier,
                    Genre = "Novel",
                    Title = "The Shagreen Skin"
                };
                books.Add(shagreenBook);
            }

            return books;
        }
    }
}
