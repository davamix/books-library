import { getRequestTo, postRequestTo, putRequestTo, deleteRequestTo } from "./requests.js";

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
const filterAuthorInput = document.getElementById("author-filter-input");

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

filterAuthorInput.addEventListener("keyup", (x)=>{
    filterAuthor();
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
    bookEl.setAttribute("id", book.id);

    bookEl.innerHTML = `
        <div class="details-top">
            <button class="remove-btn" id="remove-btn" title="Remove book">
                <i class="far fa-trash-alt"></i>
            </button>
        </div>
        <img src="https://via.placeholder.com/300x410.webp?text=Book+Cover" title="${book.title}"/>
        
        

        <div class="book-info">
            <h3>${book.title}</h3>
        </div>
    `
    bookEl.appendChild(createDetailsDiv(book.authors));
    bookEl.addEventListener("click", (x)=>{
        editBook(book.id);
    });

    const removeButton = bookEl.querySelector(".remove-btn");
    removeButton.addEventListener("click", (x)=>{
        x.stopPropagation();
        deleteBook(book.id);
    });
    
    main.appendChild(bookEl);
}

function createDetailsDiv(authors){
    const overviewEl = document.createElement("div");
    overviewEl.classList.add("details-bottom");

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

/**
 * Save the book info. Insert (POST) if no Id, otherwise, update (PUT) the info.
 */
function saveBook(){
    const bookId = document.getElementById("book-id").value;
    const bookTitle = document.getElementById("book-title").value;
    const authorId = document.getElementById("author-filter-id").value;
    const authorName = document.getElementById("author-filter-input").value;

    const bookData = {
        title: bookTitle,
        authors: [{
            id: authorId,
            name: authorName
        }]
    };

    // Insert new book
    if(bookId == ""){
        postRequestTo(API_BOOK_URL, bookData)
        .then(resp => resp.json())
        .then(data => showBook(data))
        .then(() => closeBookWindow());
    // Update the book info
    }else{
        const url = API_BOOK_URL + bookId;
        putRequestTo(url, bookData)
        .then(resp => resp.json())
        .then(data => {
            // The book is added at the end
            const bookDiv = document.getElementById(data.id);
            bookDiv.parentNode.removeChild(bookDiv);
            showBook(data);
            // bookDiv.focus();
            closeBookWindow();

        });
    }
}

function deleteBook(bookId){
    const url = API_BOOK_URL + bookId;
    deleteRequestTo(url)
    .then(resp => {
        if(resp.ok){
            const bookDiv = document.getElementById(bookId);
            bookDiv.parentNode.removeChild(bookDiv);
        }
    });
}

// BOOK WINDOW FUNCTIONS

function openBookWindow(data = {}){
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";

    getRequestTo(GET_AUTHORS_URL)
    .then(resp => resp.json())
    .then(data =>{
        addAuthorsToList(data);
    })
    .then(() => {
        if(Object.keys(data).length > 0){
            document.getElementById("book-id").value = data["id"];
            document.getElementById("book-title").value = data["title"];
            document.getElementById("author-filter-id").value = data["authors"][0].id;
            document.getElementById("author-filter-input").value = data["authors"][0].name;
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
    document.getElementById("author-filter-id").value = "";
    document.getElementById("author-filter-input").value = "";
}

function addAuthorsToList(authors){
    const filterList = document.getElementById("author-filter-list");
    filterList.innerHTML = "";

    for(let x=0; x<authors.length; x++){
        const optionElement = document.createElement("button");
        optionElement.setAttribute("data-id", authors[x].id);
        optionElement.setAttribute("data-name", authors[x].name);

        const textElement = document.createTextNode(authors[x].name);
        optionElement.appendChild(textElement);

        optionElement.addEventListener("click", (x)=>{
            setFilterAuthorValue(optionElement.dataset.id, optionElement.dataset.name);
        });

        filterList.appendChild(optionElement);        
    }
}

/**
 * Show a list of author that match with author-filter-input value
 */
function filterAuthor(){
    const filterInputId = document.getElementById("author-filter-id");
    const filterInput = document.getElementById("author-filter-input");
    const filterList = document.getElementById("author-filter-list");
    const filterOption = filterList.getElementsByTagName("button");

    // Remove the author ID while typing
    filterInputId.value = "";

    filterList.style.display = "block";

    for(let x=0; x<filterOption.length; x++){
        const authorName = filterOption[x].dataset.name;
        if((authorName.toUpperCase().indexOf(filterInput.value.toUpperCase()) > -1) && (filterInput.value.length > 0)){
            filterOption[x].style.display = "block";
        }else{
            filterOption[x].style.display = "none";
        }
    }
}

/**
 * Set the selected name from the list into the input
 * @param {string} name 
 */
function setFilterAuthorValue(id, name){
    const filterInput = document.getElementById("author-filter-input");
    filterInput.value = name;

    const filterInputId = document.getElementById("author-filter-id");
    filterInputId.value = id;

    const filterList = document.getElementById("author-filter-list");
    filterList.style.display = "none";
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
        const url = SEARCH_BOOK_URL + searchTerm;

        getRequestTo(url)
        .then(resp => resp.json())
        .then(data=>showBooks(data));
    }
})