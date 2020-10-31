namespace BooksLibrary.API.Entities{
    public class Category : EntityBase<Category>{
        public string Name { get; set; }

        public override Category MapFrom(Category item)
        {
            this.Name = item.Name;

            return this;
        }
    }
    
}