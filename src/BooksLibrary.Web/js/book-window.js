import { deleteRequestTo, getRequestTo, postRequestTo, putRequestTo } from "./requests.js";
import * as storage from "./storage.js";
import * as urls from "./urls.js";

// ELEMENTS
const saveBookButton = document.getElementById("save");
const closeWindowButton = document.getElementById("close");
const filterAuthorInput = document.getElementById("author-filter-input");
const selectCoverButton = document.getElementById("select-cover");
const selectCoverDialog = document.getElementById("select-cover-dialog");
const filterCategoryInput = document.getElementById("category-filter-input");
const tagCategories = document.getElementById("tag-categories");
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

    if (e.key == "Enter") {
        storage.addAuthorToBook(filterAuthorInput.value);
        addAuthorTag(filterAuthorInput.value);

        filterAuthorInput.value = "";
    }
});

selectCoverButton.addEventListener("click", (e) => {
    if (selectCoverDialog) {
        selectCoverDialog.click();
    }

    e.preventDefault();
}, false);

selectCoverDialog.addEventListener("change", showCoverImage, false);

filterCategoryInput.addEventListener("keyup", (e) => {
    filterCategory();

    if (e.key == "Escape"){
        const filterList = document.getElementById("category-filter-list");
        filterList.style.display = "none";
    }

    if (e.key == "Enter") {
        storage.addCategoryToBook(filterCategoryInput.value);
        addCategoryTag(filterCategoryInput.value);

        filterCategoryInput.value = "";
    }
});

// FUNCTIONS
function openBookWindow(data = {}) {
    const bookWindow = document.getElementById("book-window");
    bookWindow.style.display = "block";

    addAuthorsToFilterList();
    addCategoriesToFilterList();

    if (Object.keys(data).length > 0) {
        storage.setBook(data);
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

            storage.setCover(reader.result);
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

function filterCategory(){
    const filterInput = document.getElementById("category-filter-input");
    const filterList = document.getElementById("category-filter-list");
    const filterOption = filterList.getElementsByTagName("button");

    if (filterOption.length <= 0) return; // No authors available

    filterList.style.display = "block";

    for (let x = 0; x < filterOption.length; x++) {
        const categoryName = filterOption[x].dataset.name;
        if ((categoryName.toUpperCase().indexOf(filterInput.value.toUpperCase()) > -1) && (filterInput.value.length > 0)) {
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

/**
 * Add the selected category name to the current book and create a new tag
 * @param {string} name 
 */
function addCategoryBook(name) {
    storage.addCategoryToBook(name);

    addCategoryTag(name);

    const filterInput = document.getElementById("category-filter-input");
    filterInput.value = "";

    const filterList = document.getElementById("category-filter-list");
    filterList.style.display = "none";
}

function addAuthorsToFilterList() {
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

function addCategoriesToFilterList() {
    const categories = storage.getCategories();
    const filterList = document.getElementById("category-filter-list");
    filterList.innerHTML = "";

    for(let x=0; x<categories.length; x++){
        let optionElement = createButtonFilter(categories[x].name);

        optionElement.addEventListener("click", (x) =>{
            addCategoryBook(optionElement.dataset.name);
        });

        filterList.appendChild(optionElement);
    }
}

function createButtonFilter(value) {
    const optionElement = document.createElement("button");
    optionElement.setAttribute("data-name", value);

    const textElement = document.createTextNode(value);
    optionElement.appendChild(textElement);

    return optionElement;
}

/**
 * Save the book info. Insert (POST) if no Id, otherwise, update (PUT) the info.
 */
function saveBook() {
    const bookTitle = document.getElementById("book-title").value;

    let bookData = storage.getBook();
    bookData.title = bookTitle;

    // Update book
    if (bookData.id) {
        const url = urls.API_BOOK_URL + bookData.id;
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
        // Insert new book
    } else {
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
    document.getElementById("book-title").value = "";
    // Clear authors data
    document.getElementById("author-filter-input").value = "";
    document.getElementById("tag-authors").innerHTML = "";
    const filterList = document.getElementById("author-filter-list");
    filterList.style.display = "none";
    // Clear cover image data
    selectCoverButton.innerHTML = `<i class="far fa-image fa-3x"></i>`;
    // Clear categories
    document.getElementById("category-filter-input").value = "";
    document.getElementById("tag-categories").innerHTML = "";
}

function addCategoryTag(name) {
    const tag = createTag(name);

    tag.addEventListener("click", () => {
        const category = storage.removeCategoryFromBook(name);
        const book = storage.getBook();

        if (category.id) {
            const deleteUrl = urls.DELETE_BOOK_CATEGORY
                .replace("{book_id}", book.id)
                .replace("{category_id}", category.id);

            deleteRequestTo(deleteUrl);
        }
    });

    tagCategories.appendChild(tag);
}

function addAuthorTag(name) {
    const tag = createTag(name);

    tag.addEventListener("click", () => {
        const author = storage.removeAuthorFromBook(name);
        const book = storage.getBook();

        if (author.id) {
            const deleteUrl = urls.DELETE_BOOK_AUTHOR
                .replace("{book_id}", book.id)
                .replace("{author_id}", author.id);

            deleteRequestTo(deleteUrl);
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