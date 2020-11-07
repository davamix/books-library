using System;

namespace BooksLibrary.API.Entities
{
    public abstract class EntityBase<T>{
        public string Id{get;set;}

        public EntityBase(){}

        public EntityBase(string id){
            this.Id = id;
        }

        public abstract T MapFrom(T item);
    }
}