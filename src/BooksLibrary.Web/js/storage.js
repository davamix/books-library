const ALL_CATEGORIES = "categories";
const ALL_AUTHORS = "authors";
const BOOK_CATEGORIES = "book-categories";
const BOOK_AUTHORS = "book-authors";
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

function addCategory(value){
    const categories = getCategories();
    console.log("Category: ", value);

    localStorage.setItem(ALL_CATEGORIES, JSON.stringify([...categories, value]));
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
function getBookCategories(){
    const bookCategories = JSON.parse(localStorage.getItem(BOOK_CATEGORIES));

    return bookCategories === null ? [] : bookCategories;
}

function addBookCategory(value){
    let book = getBook();
    const categories = getCategories();
    // const bookCategories = getBookCategories();

    // console.log("C: ", categories);
    // console.log("BC: ", bookCategories);

    let category = categories.find((c) => c.name === value);
    // console.log(category);
    if(!category){
        category = {
            id: "",
            name: value
        };
    }
    // console.log(category);
    book.categories.push(category);
    setBook(book);
    // localStorage.setItem(BOOK_CATEGORIES, JSON.stringify([...bookCategories, category]));
    // console.log(...bookCategories);
    // console.log(category);
    // console.log(getBookCategories());
}

function removeBookCategory(value){
    const bookCategories = getBookCategories();

    localStorage.setItem(BOOK_CATEGORIES, JSON.stringify(bookCategories.filter((category) => category.name != value)));
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
        setBook, getBook, removeBook, createBook};