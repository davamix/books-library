import { getRequestTo, postRequestTo, putRequestTo, deleteRequestTo } from "./requests.js";
import { openBookWindow } from "./book-window.js";
import { searchTitle } from "./search.js";
import * as urls from "./urls.js";

// ELEMENTS
const main = document.getElementById("main");
const form = document.getElementById("form");
const clearButton = document.getElementById("clear");

// EVENTS
document.addEventListener("book-saved", (event) => {
    showBook(event.detail.book);
});

document.addEventListener("book-updated", (event) => {
    // Remove existing book element
    const bookDiv = document.getElementById(event.detail.book.id);
    bookDiv.parentNode.removeChild(bookDiv);

    // Show updated book
    showBook(event.detail.book);
});

document.addEventListener("title-found", (event) =>{
    showBooks(event.detail.books);
});

clearButton.addEventListener("click", () => {
    loadBooks();
});

// Search input
form.addEventListener("submit", (e) => {
    e.preventDefault();

    const searchTerm = document.getElementById("search-input").value;

    if (searchTerm) {
        searchTitle(searchTerm);
    }
});

// FUNCTIONS
async function loadBooks() {
    getRequestTo(urls.GET_BOOKS_URL)
        .then(response => response.json())
        .then(data => {
            showBooks(data);
        });
}

/**
 * Loop over the list of books and display info
 * @param {Book[]} books - List of books
 */
function showBooks(books) {
    main.innerHTML = "";

    addPlaceholder();

    books.forEach(book => {
        showBook(book);
    });
}

/**
 * Generate the elements to show the book on the main window
 * @param {Book} book - Book info
 */
function showBook(book) {
    const bookEl = document.createElement("div");
    const coverImage = (book.image) ? book.image : "https://via.placeholder.com/300x410.webp?text=Book+Cover";

    bookEl.classList.add("book");
    bookEl.setAttribute("id", book.id);

    bookEl.innerHTML = `
        <div class="details-top">
            <button class="remove-btn" id="remove-btn" title="Remove book">
                <i class="far fa-trash-alt"></i>
            </button>
        </div>
        <img src="${coverImage}" class="cover" title="${book.title}"/>

        <div class="book-info-panel">
            <h3>${book.title}</h3>
        </div>
    `
    bookEl.appendChild(createDetailsDiv(book.authors));
    bookEl.addEventListener("click", (x) => {
        editBook(book.id);
    });

    const removeButton = bookEl.querySelector(".remove-btn");
    removeButton.addEventListener("click", (x) => {
        x.stopPropagation();
        deleteBook(book.id);
    });

    main.appendChild(bookEl);
}

function createDetailsDiv(authors) {
    const overviewEl = document.createElement("div");
    overviewEl.classList.add("details-bottom");

    const listAuthorEl = document.createElement("ul");

    authors.forEach((x) => {
        const authorItem = createAuthorItem(x.name);
        listAuthorEl.appendChild(authorItem);
    });

    overviewEl.appendChild(listAuthorEl);

    return overviewEl;
}

function createAuthorItem(name) {
    const item = document.createElement("li");
    const itemValue = document.createTextNode(name);
    item.appendChild(itemValue);

    return item;
}

/**
 * Add a placeholder at the beginning that works as a button to allow add a new book.
 */
function addPlaceholder() {
    const bookEl = document.createElement("div");
    bookEl.setAttribute("id", "book-placeholder");
    bookEl.classList.add("book-placeholder");

    bookEl.innerHTML = `
        <button class="add-book" id="add-book" title="Add new book">
            <i class="fas fa-plus fa-10x"></i>
        </button>
    `

    const addButton = bookEl.querySelector(".add-book");
    addButton.addEventListener("click", () => {
        openBookWindow();
    });

    main.appendChild(bookEl);
}

function deleteBook(bookId) {
    const url = urls.API_BOOK_URL + bookId;
    deleteRequestTo(url)
        .then(resp => {
            if (resp.ok) {
                const bookDiv = document.getElementById(bookId);
                bookDiv.parentNode.removeChild(bookDiv);
            }
        });
}

/**
 * Open the book window with the book's info for editing
 * @param {string} bookId 
 */
function editBook(bookId) {
    const url = urls.API_BOOK_URL + bookId;
    getRequestTo(url)
        .then(resp => resp.json())
        .then(data => {
            openBookWindow(data);
        });
}


loadBooks();