// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const EXCHANGE_RATE_CACHE_KEY = "paw.exchangeRate.hacienda";
const EXCHANGE_RATE_CACHE_TTL_MS = 12 * 60 * 60 * 1000; // 12 hours

function getExchangeRateFromCache() {
    try {
        const raw = localStorage.getItem(EXCHANGE_RATE_CACHE_KEY);
        if (!raw) return null;

        const parsed = JSON.parse(raw);
        if (!parsed || !parsed.timestamp || !parsed.data) return null;

        const age = Date.now() - parsed.timestamp;
        if (age > EXCHANGE_RATE_CACHE_TTL_MS) return null;

        return parsed.data;
    }
    catch {
        return null;
    }
}

function saveExchangeRateToCache(data) {
    try {
        const payload = {
            timestamp: Date.now(),
            data
        };
        localStorage.setItem(EXCHANGE_RATE_CACHE_KEY, JSON.stringify(payload));
    }
    catch {
        // ignore storage issues
    }
}

function getExchangeRate() {
    const cached = getExchangeRateFromCache();
    if (cached) {
        return Promise.resolve(cached);
    }

    const myHeaders = new Headers();
    myHeaders.append("UserAgent", "curl/8.7.1");
    myHeaders.append("Accept", "*/*");

    const requestOptions = {
        method: "GET",
        headers: myHeaders,
        redirect: "follow"
    };

    return fetch("https://api.hacienda.go.cr/indicadores/tc", requestOptions)
        .then((response) => {
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            return response.json();
        })
        .then((data) => {
            saveExchangeRateToCache(data);
            return data;
        });
}

function renderExchangeRate(data) {
    const container = document.getElementById("exchangeRateCard");
    if (!container) return;

    const updated = document.getElementById("exchangeRateUpdated");
    const buy = document.getElementById("exchangeRateBuy");
    const sell = document.getElementById("exchangeRateSell");
    const euroUsd = document.getElementById("exchangeRateEuroUsd");
    const euroCrc = document.getElementById("exchangeRateEuroCrc");

    const compra = data?.dolar?.compra?.valor;
    const venta = data?.dolar?.venta?.valor;
    const euroDolares = data?.euro?.dolares;
    const euroColones = data?.euro?.colones;

    const today = new Date();
    if (updated) updated.textContent = `Updated ${today.toLocaleDateString()}`;
    if (buy) buy.textContent = Number.isFinite(compra) ? compra.toFixed(2) : "-";
    if (sell) sell.textContent = Number.isFinite(venta) ? venta.toFixed(2) : "-";
    if (euroUsd) euroUsd.textContent = Number.isFinite(euroDolares) ? euroDolares.toFixed(4) : "-";
    if (euroCrc) euroCrc.textContent = Number.isFinite(euroColones) ? euroColones.toFixed(2) : "-";

    container.classList.remove("d-none");
}

function renderExchangeRateError() {
    const container = document.getElementById("exchangeRateCard");
    if (!container) return;
    const status = document.getElementById("exchangeRateStatus");
    if (status) status.textContent = "Exchange rate data unavailable";
    container.classList.remove("d-none");
}

document.addEventListener("DOMContentLoaded", function () {
    if (!document.getElementById("exchangeRateCard")) return;

    getExchangeRate()
        .then(renderExchangeRate)
        .catch(() => renderExchangeRateError());
});
