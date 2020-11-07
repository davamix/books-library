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


export { getRequestTo, postRequestTo, putRequestTo, deleteRequestTo };