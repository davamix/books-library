import { getRequestTo, postRequestTo, putRequestTo } from "./requests.js";
import * as urls from "./urls.js";

// ELEMENTS
const saveBookButton = document.getElementById("save");
const closeWindowButton = document.getElementById("close");
const filterAuthorInput = document.getElementById("author-filter-input");
const selectCoverButton = document.getElementById("select-cover");
const selectCoverDialog = document.getElementById("select-cover-dialog");

// EVENTS
closeWindowButton.addEventListener("click", () => {
    closeBookWindow();
});

saveBookButton.addEventListener("click", () => {
    const authorId = document.getElementById("author-filter-id").value;
    const authorName = document.getElementById("author-filter-input").value;

    // If author does not exists, save it before save the book info
    if (!authorId) {
        saveAuthor(authorName)
            .then(data => {
                document.getElementById("author-filter-id").value = data.id;
                saveBook();
            });
    } else {
        saveBook();
    }
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

// FUNCTIONS
function openBookWindow(data = {}) {
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";

    getRequestTo(urls.GET_AUTHORS_URL)
        .then(resp => resp.json())
        .then(data => {
            addAuthorsToList(data);
        })
        .then(() => {
            if (Object.keys(data).length > 0) {
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
            }
        });
}

/**
 * Show the selected image from dialog into the image placeholder on book window
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
            // console.log("FILTER: ", authorName.toUpperCase().indexOf(filterInput.value.toUpperCase()));
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

    const bookData = {
        title: bookTitle,
        image: coverImage,
        authors: [{
            id: authorId,
            name: authorName
        }]
    };

    // Insert new book
    if (bookId == "") {
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
        .then(resp => resp.json());

    return resp;
}

export { openBookWindow };