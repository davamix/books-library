const ALL_CATEGORIES = "categories";
const ALL_AUTHORS = "authors";
const BOOK_CATEGORIES = "book-categories";
const BOOK_AUTHORS = "book-authors";
const CURRENT_BOOK = "book";

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
    console.log(book);

    localStorage.setItem(CURRENT_BOOK, JSON.stringify(book));
}

function getBook(){
    const book = localStorage.getItem(CURRENT_BOOK);

    return JSON.parse(book);
}

function removeBook(){
    localStorage.removeItem(CURRENT_BOOK);
}

export { getCategories, addCategories, addCategory, addBookCategory, setBook, getBook, removeBook, createBook};