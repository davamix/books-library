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
    // filterTag();

    if (x.key == "Enter") {
        tagBar.appendChild(createTag(inputTag.value));

        storage.addBookCategory(inputTag.value);

        inputTag.value = "";
    }
});

// FUNCTIONS
function openBookWindow(data = {}) {
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";

    const authorsPromise = getRequestTo(urls.GET_AUTHORS_URL)
        .then(resp => resp.json())
        .then(data => addAuthorsToList(data));

    const categoriesPromise = getRequestTo(urls.GET_CATEGORIES_URL)
        .then(resp => resp.json())
        .then(data => storage.addCategories(data));
    // .then(data => addCategoriesToList(data));


    Promise.all([authorsPromise, categoriesPromise])
        .then(() => {
            if (Object.keys(data).length > 0) {
                storage.setBook(data);
                document.getElementById("book-id").value = data["id"];
                document.getElementById("book-title").value = data["title"];
                // Add authors data
                document.getElementById("author-filter-id").value = data["authors"][0].id;
                document.getElementById("author-filter-input").value = data["authors"][0].name;
                // Add cover image data
                document.getElementById("cover-data").value = data["image"];
                if (data["image"]) {
                    const img = document.createElement("img");
                    img.src = data["image"];
                    selectCoverButton.innerHTML = "";
                    selectCoverButton.appendChild(img);
                }
                // Add categories
                data["categories"].forEach(c => {
                    tagBar.appendChild(createTag(c.name));
                });
            } else {
                storage.createBook();
            }
        });
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
    const filterInputId = document.getElementById("author-filter-id");
    const filterInput = document.getElementById("author-filter-input");
    const filterList = document.getElementById("author-filter-list");
    const filterOption = filterList.getElementsByTagName("button");

    if (filterOption.length <= 0) return; // No authors available

    // Remove the author ID while typing
    filterInputId.value = "";

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
 * Set the selected name from the list into the input
 * @param {string} name 
 */
function setFilterAuthorValue(id, name) {
    const filterInput = document.getElementById("author-filter-input");
    filterInput.value = name;

    const filterInputId = document.getElementById("author-filter-id");
    filterInputId.value = id;

    const filterList = document.getElementById("author-filter-list");
    filterList.style.display = "none";
}

function addAuthorsToList(authors) {
    const filterList = document.getElementById("author-filter-list");
    filterList.innerHTML = "";

    for (let x = 0; x < authors.length; x++) {
        const optionElement = document.createElement("button");
        optionElement.setAttribute("data-id", authors[x].id);
        optionElement.setAttribute("data-name", authors[x].name);

        const textElement = document.createTextNode(authors[x].name);
        optionElement.appendChild(textElement);

        optionElement.addEventListener("click", (x) => {
            setFilterAuthorValue(optionElement.dataset.id, optionElement.dataset.name);
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
    document.getElementById("author-filter-id").value = "";
    document.getElementById("author-filter-input").value = "";
    const filterList = document.getElementById("author-filter-list");
    filterList.style.display = "none";
    // Clear cover image data
    document.getElementById("cover-data").value = "";
    selectCoverButton.innerHTML = `<i class="far fa-image fa-3x"></i>`;
}

async function saveAuthor(name) {
    const authorData = {
        name: name
    };

    const resp = await postRequestTo(urls.API_AUTHOR_URL, authorData)
        .then(resp => resp.json())
        .then(err => console.log("ERROR AUTHOR PORT REQUEST: ", err));

    return resp;
}

async function saveCategory(name) {
    const categoryData = {
        name: name
    };

    const resp = await postRequestTo(urls.API_CATEGORY_URL, categoryData)
        .then(resp => resp.json())
        .catch(err => console.log("ERROR CATEGORY POST REQUEST: ", err));

    return resp;
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