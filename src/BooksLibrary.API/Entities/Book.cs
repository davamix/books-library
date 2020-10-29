using System.Collections.Generic;
using BooksLibrary.API.Entities;

namespace BooksLibrary.API.Entities
{
    public class Book : EntityBase<Book>
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public IList<Author> Authors { get; set; }

        public Book() : base()
        {
            this.Authors = new List<Author>();
        }

        public override Book MapFrom(Book item)
        {
            this.Title = item.Title;
            this.Authors = item.Authors;

            return this;
        }
    }

}