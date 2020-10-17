
const API_BOOK_URL = "https://localhost:5001/api/book/";
const GET_BOOKS_URL = API_BOOK_URL + "GetBooks/";
const SEARCH_BOOK_URL = API_BOOK_URL + "Search?query=";

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

// REQUESTS

async function getRequestTo(url){
    const resp = await fetch(url);

    return resp;
}

async function postRequestTo(url, data){
    const resp = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type":"application/json"
        },
        body: JSON.stringify(data)
    });

    return resp;
}

async function putRequestTo(url, data){
    const resp = await fetch(url, {
        method: "PUT",
        headers: {
            "Content-Type":"application/json"
        },
        body: JSON.stringify(data)
    });

    return resp;
}

async function deleteRequestTo(url){
    const resp = await fetch(url, {
        method: "DELETE"
    });

    return resp;
}

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

        <div class="book-info">
            <h3>${book.title}</h3>
        </div>
    `

    main.appendChild(bookEl);
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

    const bookData = {
        title: bookTitle
    };

    if(bookId == ""){
        postRequestTo(API_BOOK_URL, bookData)
        .then(resp => resp.json())
        .then(data => showBook(data))
        .then(() => closeBookWindow());
    }
}

function openBookWindow(){
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";
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