import { getRequestTo, postRequestTo, putRequestTo } from "./requests.js";
import * as storage from "./storage.js";
import * as urls from "./urls.js";

// ELEMENTS
const saveBookButton = document.getElementById("save");
const closeWindowButton = document.getElementById("close");
const filterAuthorInput = document.getElementById("author-filter-input");
const selectCoverButton = document.getElementById("select-cover");
const selectCoverDialog = document.getElementById("select-cover-dialog");
const inputTag = document.getElementById("input-tag");
const tagBar = document.getElementById("tag-bar");
const tagAuthors = document.getElementById("tag-authors");

// EVENTS
closeWindowButton.addEventListener("click", () => {
    closeBookWindow();
});

saveBookButton.addEventListener("click", (e) => {
    e.preventDefault();

    saveBook();
});

filterAuthorInput.addEventListener("keyup", (e) => {
    filterAuthor();

    if (e.key == "Escape") {
        const filterAuthorList = document.getElementById("author-filter-list");
        filterAuthorList.style.display = "none";
    }
});

selectCoverButton.addEventListener("click", (e) => {
    if (selectCoverDialog) {
        selectCoverDialog.click();
    }

    e.preventDefault();
}, false);

selectCoverDialog.addEventListener("change", showCoverImage, false);

inputTag.addEventListener("keyup", (x) => {
    if (x.key == "Enter") {
        storage.addCategoryToBook(name);
        addCategoryTag(inputTag.value);

        inputTag.value = "";
    }
});

// FUNCTIONS
function openBookWindow(data = {}) {
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";

    addAuthorsToList();

    if (Object.keys(data).length > 0) {
        storage.setBook(data);
        document.getElementById("book-id").value = data["id"];
        document.getElementById("book-title").value = data["title"];
        // Add authors data
        data["authors"].forEach(a => {
            addAuthorTag(a.name);
        });
        // Add categories
        data["categories"].forEach(c => {
            addCategoryTag(c.name);
        });
        // Add cover image data
        document.getElementById("cover-data").value = data["image"];
        if (data["image"]) {
            const img = document.createElement("img");
            img.src = data["image"];
            selectCoverButton.innerHTML = "";
            selectCoverButton.appendChild(img);
        }
    } else {
        storage.createBook();
    }
}

/**
 * Show the selected image from dialog into the image placeholder
 */
function showCoverImage() {
    if (this.files.length) {
        // Clear the current image
        selectCoverButton.innerHTML = ""

        const reader = new FileReader();
        reader.onload = () => {
            // Add the new image to the button
            const img = document.createElement("img");
            img.src = reader.result;
            selectCoverButton.appendChild(img);

            const coverData = document.getElementById("cover-data");
            coverData.value = reader.result;
        }

        reader.readAsDataURL(this.files[0]);
    }
}

/**
 * Show a list of authors that match with author-filter-input value
 */
function filterAuthor() {
    const filterInput = document.getElementById("author-filter-input");
    const filterList = document.getElementById("author-filter-list");
    const filterOption = filterList.getElementsByTagName("button");

    if (filterOption.length <= 0) return; // No authors available

    filterList.style.display = "block";

    for (let x = 0; x < filterOption.length; x++) {
        const authorName = filterOption[x].dataset.name;
        if ((authorName.toUpperCase().indexOf(filterInput.value.toUpperCase()) > -1) && (filterInput.value.length > 0)) {
            filterOption[x].style.display = "block";
        } else {
            filterOption[x].style.display = "none";
        }

        if (filterInput.value.length <= 0) {
            filterList.style.display = "none";
        }
    }
}

/**
 * Add the selected author's name to the current book and create a new tag
 * @param {string} name 
 */
function addAuthorBook(name) {
    storage.addAuthorToBook(name);

    addAuthorTag(name);

    const filterInput = document.getElementById("author-filter-input");
    filterInput.value = "";

    const filterList = document.getElementById("author-filter-list");
    filterList.style.display = "none";
}

function addAuthorsToList() {
    const authors = storage.getAuthors();
    const filterList = document.getElementById("author-filter-list");
    filterList.innerHTML = "";

    for (let x = 0; x < authors.length; x++) {
        const optionElement = document.createElement("button");
        optionElement.setAttribute("data-name", authors[x].name);

        const textElement = document.createTextNode(authors[x].name);
        optionElement.appendChild(textElement);

        optionElement.addEventListener("click", (x) => {
            addAuthorBook(optionElement.dataset.name);
        });

        filterList.appendChild(optionElement);
    }
}

/**
 * Save the book info. Insert (POST) if no Id, otherwise, update (PUT) the info.
 */
function saveBook() {
    const bookId = document.getElementById("book-id").value;
    const bookTitle = document.getElementById("book-title").value;
    const authorId = document.getElementById("author-filter-id").value;
    const authorName = document.getElementById("author-filter-input").value;
    const coverImage = document.getElementById("cover-data").value;

    let bookData = storage.getBook();
    bookData.title = bookTitle;
    bookData.authors = [{ id: authorId, name: authorName }];
    bookData.image = coverImage;
    console.log("Save book: ", bookData);

    // const bookData = {
    //     title: bookTitle,
    //     image: coverImage,
    //     authors: [{
    //         id: authorId,
    //         name: authorName
    //     }]
    // };

    // Insert new book
    // if (bookId == "") {
    if (bookData.id == "") {
        postRequestTo(urls.API_BOOK_URL, bookData)
            .then(resp => resp.json())
            .then(data => {
                document.dispatchEvent(
                    new CustomEvent("book-saved", {
                        detail: {
                            book: data
                        }
                    })
                );
            })
            .then(() => {
                closeBookWindow();
            });
        // Update the book info
    } else {
        const url = urls.API_BOOK_URL + bookId;
        putRequestTo(url, bookData)
            .then(resp => resp.json())
            .then(data => {
                document.dispatchEvent(
                    new CustomEvent("book-updated", {
                        detail: {
                            book: data
                        }
                    })
                );
            })
            .then(() => {
                closeBookWindow();
            });
    }

    // TODO: Reload authors and categories from DB to LS
}

function closeBookWindow() {
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "none";
    cleanBookWindow();
    storage.removeBook();
}

function cleanBookWindow() {
    document.getElementById("book-id").value = "";
    document.getElementById("book-title").value = "";
    // Clear authors data
    document.getElementById("author-filter-input").value = "";
    document.getElementById("tag-authors").innerHTML = "";
    const filterList = document.getElementById("author-filter-list");
    filterList.style.display = "none";
    // Clear cover image data
    document.getElementById("cover-data").value = "";
    selectCoverButton.innerHTML = `<i class="far fa-image fa-3x"></i>`;
    // Clear categories
    document.getElementById("input-tag").value = "";
    document.getElementById("tag-bar").innerHTML = "";
}

function addCategoryTag(name){
    const tag = createTag(name);
    
    tag.addEventListener("click", () =>{
        const category = storage.removeCategoryFromBook(name);

        if(category.id){
            // TODO: Remove from DB table book_category
            console.log("Removing from DB: ", category.id);
        }
    });

    tagBar.appendChild(tag);
}

function addAuthorTag(name){
    const tag = createTag(name);

    tag.addEventListener("click", () =>{
        const author = storage.removeAuthorFromBook(name);

        if(author.id){
            // TODO: Remove from DB table book_author
            console.log("Removing from DB: ", author.id);
        }
    });

    tagAuthors.appendChild(tag);
}

function createTag(text) {
    const spanEl = document.createElement("span");
    spanEl.classList.add("tag");

    const content = document.createTextNode(text);
    spanEl.appendChild(content);

    spanEl.addEventListener("click", (x) => {
        spanEl.parentNode.removeChild(spanEl);
    });

    return spanEl;
}

export { openBookWindow };