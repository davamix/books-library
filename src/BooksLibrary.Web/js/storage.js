const ALL_CATEGORIES = "categories";
const ALL_AUTHORS = "authors";
const CURRENT_BOOK = "book";

// CATEGORIES
function getCategories(){
    const categories = JSON.parse(localStorage.getItem(ALL_CATEGORIES));

    return categories === null ? [] : categories;
}

function addCategories(values){
    localStorage.removeItem(ALL_CATEGORIES);

    localStorage.setItem(ALL_CATEGORIES, JSON.stringify(values));
}

function addCategoryToBook(name){
    const book = getBook();
    const categories = getCategories();

    let category = categories.find((a) => a.name === name);

    if(!category){
        category = {
            name: name
        };
    }

    book.categories.push(category);

    setBook(book);
}

function removeCategoryFromBook(name){
    const book = getBook();

    const category = book.categories.find((a) => a.name === name);

    if(category){
        book.categories.splice(book.categories.indexOf(category), 1);
    }

    setBook(book);

    return category;
}

// AUTHORS
function getAuthors(){
    return JSON.parse(localStorage.getItem(ALL_AUTHORS));
}
function addAuthors(data){
    localStorage.removeItem(ALL_AUTHORS);

    localStorage.setItem(ALL_AUTHORS, JSON.stringify(data));
}

function addAuthorToBook(name){
    const book = getBook();
    const authors = getAuthors();

    let author = authors.find((a) => a.name === name);

    if(!author){
        author = {
            name: name
        };
    }

    book.authors.push(author);

    setBook(book);
}

function removeAuthorFromBook(name){
    const book = getBook();

    const author = book.authors.find((a) => a.name === name);

    if(author){
        book.authors.splice(book.authors.indexOf(author), 1);
    }

    setBook(book);

    return author;
}

// BOOKS
function setCover(data){
    const book = getBook();

    if(data){
        book.image = data;
        setBook(book);
    }
}

function createBook(){
    const book = {
        id: "",
        title: "",
        image: "",
        authors: [],
        categories: []
    };

    localStorage.setItem(CURRENT_BOOK, JSON.stringify(book));
}

function setBook(data){
    localStorage.removeItem(CURRENT_BOOK);
    
    const book = {
        id: data["id"],
        title: data["title"],
        image: data["image"],
        authors: data["authors"] ?? [],
        categories: data["categories"] ?? []
    };

    localStorage.setItem(CURRENT_BOOK, JSON.stringify(book));
}

function getBook(){
    return JSON.parse(localStorage.getItem(CURRENT_BOOK));
}

function removeBook(){
    localStorage.removeItem(CURRENT_BOOK);
}

export { getCategories, addCategories, addCategoryToBook, removeCategoryFromBook,
        addAuthors, getAuthors, addAuthorToBook, removeAuthorFromBook,
        setBook, getBook, removeBook, createBook, setCover};