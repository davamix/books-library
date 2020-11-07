import { getRequestTo } from "./requests.js";
import * as urls from "./urls.js";

// ELEMENTS
const form = document.getElementById("form");

// FUNCTIONS
function searchTitle(term){
    if (term) {
        const url = urls.SEARCH_BOOK_URL + term;

        getRequestTo(url)
            .then(resp => resp.json())
            .then(data => {
                document.dispatchEvent(
                    new CustomEvent("title-found", {
                        detail: {
                            books: data
                        }
                    })
                );
            });
    }
}

export { searchTitle };