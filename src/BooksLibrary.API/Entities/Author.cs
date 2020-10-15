namespace BooksLibrary.API.Entities{
    public class Author : EntityBase<Author>{
        public string Name { get; set; }

        public override void MapFrom(Author item)
        {
            this.Name = item.Name;
        }
    }
    
}