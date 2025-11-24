window.logApiCall = (method, url, data) => {
    try { console.log("API Request", { method, url, data }); } catch {}
};

window.logApiResponse = (statusCode, data) => {
    try { console.log("API Response", { statusCode, data }); } catch {}
};
