
const main = document.getElementById("main");
const form = document.getElementById("form");
const search = document.getElementById("search");

async function loadBook(){
    const resp = await fetch("https://www.etnassoft.com/api/v1/get/?id=2617");

    return resp;

}

function showBooks(books){
    main.innerHTML = "";

    addPlaceholder();
    return;

    for(var x=0; x<6; x++){
        books.forEach(book => {
        const bookEl = document.createElement("div");
        bookEl.classList.add("book");

        bookEl.innerHTML = `
            <img src="https://via.placeholder.com/300x410.webp?text=Book+Cover" title="${book.title}"/>

            <div class="book-info">
                <h3>${book.title}</h3>
            </div>
            <span>${x}</span>
        `

        main.appendChild(bookEl);
    });
    }

}

function addPlaceholder(){
    const bookEl = document.createElement("div");
    bookEl.classList.add("book-placeholder");

    bookEl.innerHTML = `
        <button class="add-book" id="add-book" title="Add new book">
            <i class="fas fa-plus fa-10x"></i>
        </button>
    `

    const addButton = bookEl.querySelector(".add-book");
    addButton.addEventListener("click", ()=>{
        console.log("Add new book");
    });

    main.appendChild(bookEl);
}


loadBook()
.then(response => response.json())
.then(data => {
    console.log(data);
    showBooks(data);
});