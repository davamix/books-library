using System;

namespace BooksLibrary.API.Entities
{
    public abstract class EntityBase<T>{
        public string Id{get;set;}

        public EntityBase(): this(Guid.NewGuid().ToString()){}

        public EntityBase(string id){
            this.Id = id;
        }

        public abstract void MapFrom(T item);
    }
}