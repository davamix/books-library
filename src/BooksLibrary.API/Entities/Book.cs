using System.Collections.Generic;

namespace BooksLibrary.API.Entities
{
    public class Book : EntityBase<Book>
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public IList<Author> Authors { get; set; }
        public IList<Category> Categories {get;set;}

        public Book() : base()
        {
            this.Authors = new List<Author>();
            this.Categories = new List<Category>();
        }

        public override Book MapFrom(Book item)
        {
            this.Title = item.Title;
            this.Authors = item.Authors;
            this.Categories = item.Categories;

            return this;
        }
    }

}