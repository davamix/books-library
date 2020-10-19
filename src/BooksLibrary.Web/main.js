import { getRequestTo, postRequestTo, putRequestTo, deleteRequestTo } from "./requests.mjs";

const API_BOOK_URL = "https://localhost:5001/api/book/";
const GET_BOOKS_URL = API_BOOK_URL + "GetBooks/";
const SEARCH_BOOK_URL = API_BOOK_URL + "Search?query=";

const API_AUTHOR_URL = "https://localhost:5001/api/author/";
const GET_AUTHORS_URL = API_AUTHOR_URL + "GetAuthors/"

const main = document.getElementById("main");
const form = document.getElementById("form");
const clearButton = document.getElementById("clear");
const saveBookButton = document.getElementById("save");
const closeWindowButton = document.getElementById("close");

// EVENTS
clearButton.addEventListener("click", ()=>{
    loadBooks();
});

closeWindowButton.addEventListener("click", ()=>{
    closeBookWindow();
});

saveBookButton.addEventListener("click", ()=>{
    saveBook();
});

async function loadBooks(){
    // const resp = await fetch("https://www.etnassoft.com/api/v1/get/?id=2617");
    // const resp = await fetch("https://localhost:5001/api/book/GetBooks")
    getRequestTo(GET_BOOKS_URL)
    .then(response => response.json())
    .then(data => {
        console.log(data);
        showBooks(data);
    });

    // return resp;
}

/**
 * Loop over the list of books and display info
 * @param {Book[]} books - List of books
 */
function showBooks(books){
    main.innerHTML = "";

    addPlaceholder();
    // return;

    books.forEach(book => {
        showBook(book);
    });
}

/**
 * Display book info
 * @param {Book} book - Book info
 */
function showBook(book){
    const bookEl = document.createElement("div");
    bookEl.classList.add("book");

    bookEl.innerHTML = `
        <img src="https://via.placeholder.com/300x410.webp?text=Book+Cover" title="${book.title}"/>
        <input type="hidden" value="${book.id}"/>

        <div class="book-info">
            <h3>${book.title}</h3>
        </div>
    `
    bookEl.appendChild(createDetailsDiv(book.authors));

    const bookId = bookEl.getElementsByTagName("input")[0].value;
    bookEl.addEventListener("click", (x)=>{
        console.log("Click on cover", bookId);
        editBook(bookId);
    });
    

    main.appendChild(bookEl);
}

function createDetailsDiv(authors){
    const overviewEl = document.createElement("div");
    overviewEl.classList.add("details");

    const listAuthorEl = document.createElement("ul");

    authors.forEach((x)=>{
        const authorItem = createAuthorItem(x.name);
        listAuthorEl.appendChild(authorItem);
    });

    overviewEl.appendChild(listAuthorEl);

    return overviewEl;
}

function createAuthorItem(name){
    const item = document.createElement("li");
    const itemValue = document.createTextNode(name);
    item.appendChild(itemValue);

    return item;
}

/**
 * Add a placeholder at the beginning that works as a button to allow add a new book.
 */
function addPlaceholder(){
    const bookEl = document.createElement("div");
    bookEl.setAttribute("id", "book-placeholder");
    bookEl.classList.add("book-placeholder");

    bookEl.innerHTML = `
        <button class="add-book" id="add-book" title="Add new book">
            <i class="fas fa-plus fa-10x"></i>
        </button>
    `

    const addButton = bookEl.querySelector(".add-book");
    addButton.addEventListener("click", ()=>{
        openBookWindow();
    });

    main.appendChild(bookEl);
}

function saveBook(){
    const bookId = document.getElementById("book-id").value;
    const bookTitle = document.getElementById("book-title").value;
    const bookAuthor = document.getElementById("book-author").value;

    // TODO: Check how to save authors in backend.
    const bookData = {
        title: bookTitle,
        author: bookAuthor
    };

    if(bookId == ""){
        postRequestTo(API_BOOK_URL, bookData)
        .then(resp => resp.json())
        .then(data => showBook(data))
        .then(() => closeBookWindow());
    }else{
        const url = API_BOOK_URL + bookId;
        putRequestTo(url, bookData)
        .then(resp => resp.json())
        .then(data => console.log("TODO: Update book info on main screen"));
    }
}

// BOOK WINDOW FUNCTIONS

function openBookWindow(data = {}){
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";

    getRequestTo(GET_AUTHORS_URL)
    .then(resp => resp.json())
    .then(data =>{
        console.log(data);
        addAuthorsToList(data);
    })
    .then(() => {
        if(Object.keys(data).length > 0){
            document.getElementById("book-id").value = data["id"];
            document.getElementById("book-title").value = data["title"];
            document.getElementById("author-list").value = data["authors"][0].name;
        } 
    });    
}

function closeBookWindow(){
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "none";
    cleanBookWindow();
}

function cleanBookWindow(){
    document.getElementById("book-id").value = "";
    document.getElementById("book-title").value = "";
}

function addAuthorsToList(authors){
    const authorList = document.getElementById("author-list");

    authorList.innerHTML = "";

    for(let x=0; x<authors.length; x++){
        const optionElement = document.createElement("option");
        optionElement.setAttribute("value", authors[x].id);
        
        const optionText = document.createTextNode(authors[x].name);
        optionElement.appendChild(optionText);

        authorList.appendChild(optionElement);
    }
}

function editBook(bookId){
    const url = API_BOOK_URL + bookId;
    getRequestTo(url)
    .then(resp => resp.json())
    .then(data => {
        openBookWindow(data);
    });
}


loadBooks();


/**
 * Search book
 */
form.addEventListener("submit", (e)=>{
    e.preventDefault();

    const searchTerm = document.getElementById("search-input").value;

    if(searchTerm){
        url = SEARCH_BOOK_URL + searchTerm;

        getRequestTo(url)
        .then(resp => resp.json())
        .then(data=>showBooks(data));
    }
})