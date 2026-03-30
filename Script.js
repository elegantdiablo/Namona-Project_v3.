function resolveApiUrl() {
    if (window.location.origin && window.location.origin.indexOf("http") === 0) {
        return window.location.origin.replace(/:\d+$/, ":5222");
    }
    return "http://localhost:5222";
}

const API_URL = resolveApiUrl();
const CURRENT_USER_KEY = "currentUser";
function initEventListeners() {
    const menuBtn = document.getElementById("menuButton");
    const sideMenu = document.getElementById("sideMenu");
    const content = document.getElementById("content");
    const cartButton = document.getElementById("cartButton");
    const profileBtn = document.querySelector(".profile-btn");
    
    menuBtn?.addEventListener("click", () => {
        sideMenu?.classList.toggle("open");
        content?.classList.toggle("shift");
    });

    cartButton?.addEventListener("click", () => {
        window.location.href = "Namonacart.html";
    });

    profileBtn?.addEventListener("click", () => {
        document.getElementById("profileMenu")?.classList.toggle("open");
    });

    document.addEventListener("click", (e) => {
        const menu = document.getElementById("profileMenu");
        if (menu && profileBtn && !menu.contains(e.target) && !profileBtn.contains(e.target)) {
            menu.classList.remove("open");
        }
    });
}

document.addEventListener("DOMContentLoaded", () => {
    initEventListeners();
    checkLoggedInUser();
    initSaveButton();
    initGlobalSearchUi();
    
    if (window.location.pathname.includes('Product')) {
        loadProductDetails();
    }
    
    if (document.getElementById("Tshirts") || document.getElementById("Hoodies") || document.getElementById("Pants")) {
        renderProductsFromDatabase();
    }
    
    document.getElementById("loginForm")?.addEventListener("submit", loginUser);
    document.getElementById("registerForm")?.addEventListener("submit", registerUser);
});


async function renderProductsFromDatabase() {
    const categoryMap = { 'Tshirt': 'tshirt', 'Hoodie': 'hoodie', 'Pant': 'pants' };
    const sectionMap = { 'tshirt': 'Tshirts', 'hoodie': 'Hoodies', 'pants': 'Pants' };
    const currentPage = window.location.pathname.split('/').pop() || 'Namona.html';
    
    let categoryKey = '';
    for (const [key, value] of Object.entries(categoryMap)) {
        if (currentPage.includes(key)) {
            categoryKey = value;
            break;
        }
    }

    try {
        const clothes = await getAllClothes();
        const sectionId = sectionMap[categoryKey] || (currentPage === 'Namona.html' ? 'Hoodies' : '');
        
        if (!sectionId) return;

        const section = document.getElementById(sectionId);
        if (!section) return;

        const filtered = clothes.filter(item => {
            const normalized = normalizeCategoryName(item.categoryName);
            return normalized === categoryKey || (sectionId === 'Hoodies' && categoryKey === '');
        });

        section.innerHTML = filtered.map(item => {
            const normalized = normalizeCategoryName(item.categoryName);
            const frontImg = item.imagePath || getCategoryFallbackImage(normalized, 'front');
            const fallback = getCategoryFallbackImage(normalized, 'front');
            
            return `<a href="${getProductPageUrl(item, clothes)}" class="product-link">
                <div class="flip-card">
                    <div class="flip-inner">
                        <div class="flip-front">
                            <img src="${frontImg}" alt="${item.clothingName} front" onerror="this.src='${fallback}'">
                        </div>
                        <div class="flip-back">
                            <img src="${item.imagePath || getCategoryFallbackImage(normalized, 'back')}" alt="${item.clothingName} back">
                        </div>
                    </div>
                </div>
                <p class="product-name">${item.clothingName}</p>
            </a>`;
        }).join('');

    } catch (error) {
        console.error('Error loading products:', error);
    }
}

function getCategoryFallbackImage(categoryKey, side) {
    if (categoryKey === 'tshirt') {
        return side === 'front' ? 'Tshirtimage (2).png' : 'TshirtimageFlipped.png';
    } else if (categoryKey === 'pants') {
        return side === 'front' ? 'pantsimage.png' : 'pantsimageflipped.png';
    } else {
        return side === 'front' ? 'elol2.png' : 'hatul2.png';
    }
}

async function loadProductDetails() {
    var urlParams = new URLSearchParams(window.location.search);
    var productId = urlParams.get('id');
    var currentPage = window.location.pathname.split('/').pop() || '';

    var collectionName = '';
    if (currentPage.includes('Hoodie')) collectionName = 'Hoodies';
    else if (currentPage.includes('Tshirt')) collectionName = 'T-Shirts';
    else if (currentPage.includes('Pant')) collectionName = 'Pants';

    var collectionEl = document.querySelector('.product-collection');
    if (collectionEl && collectionName) {
        collectionEl.textContent = collectionName;
    }
    
    if (!productId) return;

    try {
        var clothes = await getAllClothes();
        var product = null;
        
        for (var i = 0; i < clothes.length; i++) {
            if (clothes[i].clothingId == productId) {
                product = clothes[i];
                break;
            }
        }

        if (!product) return;
        if (collectionEl) {
            collectionEl.textContent = product.categoryName || 'Collection';
        }

        var titleEl = document.querySelector('.product-info h1');
        if (titleEl) titleEl.textContent = product.clothingName || 'Product';

        var priceEl = document.querySelector('.product-price');
        if (priceEl) priceEl.textContent = '$' + (product.price || '0.00');

        var title = document.querySelector('title');
        if (title) title.textContent = 'Namona - ' + (product.clothingName || 'Product');

        var imgEl = document.querySelector('.product-image img');
        if (imgEl) {
            var categoryNorm = normalizeCategoryName(product.categoryName);
            var fallback = getCategoryFallbackImage(categoryNorm, 'front');
            imgEl.src = product.imagePath || fallback;
            imgEl.alt = product.clothingName;
        }

        var descEl = document.querySelector('.product-description');
        if (descEl) {
            descEl.innerHTML = '<p>' + (product.description || 'Premium quality product with excellent craftsmanship.') + '</p>' +
                '<p><strong>Category:</strong> ' + (product.categoryName || 'Clothing') + '</p>' +
                '<p><strong>Color:</strong> ' + (product.color || 'Available') + '</p>';
        }

        var cartBtn = document.querySelector('.add-to-cart');
        if (cartBtn) {
            cartBtn.onclick = function() {
                addToCartBackend(product.clothingId);
            };
        }

        var saveBtn = document.getElementById('saveBtn');
        if (saveBtn) {
            saveBtn.setAttribute('data-id', product.clothingId);
            saveBtn.setAttribute('data-name', product.clothingName);
            saveBtn.setAttribute('data-price', '$' + product.price);
            var categoryNorm = normalizeCategoryName(product.categoryName);
            var fallback = getCategoryFallbackImage(categoryNorm, 'front');
            saveBtn.setAttribute('data-image', product.imagePath || fallback);
        }

    } catch (error) {
        console.error('Error loading product details:', error);
    }
}


async function registerUser(e) {
    e.preventDefault();
    const messageEl = document.getElementById("registerMessage");
    const userName = document.getElementById("regUsername")?.value.trim() || "";
    const email = document.getElementById("regEmail")?.value.trim() || "";
    const password = document.getElementById("regPassword")?.value || "";

    if (!userName || !email || !password) {
        messageEl.textContent = "Please fill in all fields";
        messageEl.style.color = "red";
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/user/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ email, userName, password })
        });

        if (response.ok) {
            messageEl.textContent = "Registration successful!";
            messageEl.style.color = "green";
            e.target.reset();
            return;
        }

        let errorText = "Registration failed";

        try {
            const errorBody = await response.json();
            errorText = errorBody?.message || errorBody?.title || errorBody?.error || errorText;
        } catch (parseError) {
            const fallbackText = await response.text();
            if (fallbackText) errorText = fallbackText;
        }

        messageEl.textContent = errorText;
        messageEl.style.color = "red";
    } catch (error) {
        messageEl.textContent = "Connection failed";
        messageEl.style.color = "red";
    }
}


async function loginUser(e) {
    e.preventDefault();
    const email = document.getElementById("loginEmail")?.value.trim().toLowerCase() || "";
    const password = document.getElementById("loginPassword")?.value || "";
    const message = document.getElementById("loginMessage");

    if (!email || !password) {
        message.textContent = "Please enter your email and password";
        message.style.color = "red";
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/User/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            try {
                const loginUserData = await response.json();
                if (loginUserData) setCachedUser(loginUserData);
                else await syncCachedUserFromApi();
            } catch (parseError) {
                console.error("Error parsing login response:", parseError);
                await syncCachedUserFromApi();
            }

            message.textContent = "Login successful!";
            message.style.color = "green";
            window.location.href = "Namona.html";
        } else {
            let errorText = response.status === 401 ? "Invalid email or password" : "Login failed";

            try {
                const errorBody = await response.json();
                errorText = errorBody?.message || errorBody?.title || errorBody?.error || errorText;
            } catch (parseError) {
                const fallbackText = await response.text();
                if (fallbackText) errorText = fallbackText;
            }

            message.textContent = errorText;
            message.style.color = "red";
        }
    } catch (error) {
        console.error("Login error:", error);
        message.textContent = "Server error";
        message.style.color = "red";
    }
}


async function checkLoggedInUser() {
    var profileMenu = document.getElementById("profileMenu");
    if (!profileMenu) return;

    var user = await getCurrentUser();
    if (!user) return;

    profileMenu.innerHTML =
        '<span>Welcome, ' + user.userName + '</span>' +
        '<a href="#" onclick="logout()">🚪 Logout</a>' +
        '<a href="SavedProducts.html">💾 Saved products</a>' +
        '<a href="MyOrders.html">📦 My orders</a>' +
        '<a href="AboutUs.html">ℹ️ About us</a>';

}


async function logout() {
    await fetch(API_URL + "/api/User/logout", {
        method: "POST",
        credentials: "include"
    });

    clearCachedUser();

    location.reload();

}


function increaseQty() {
    var input = document.getElementById("quantityInput");
    input.value = parseInt(input.value) + 1;
}

function decreaseQty() {
    var input = document.getElementById("quantityInput");
    if (parseInt(input.value) > 1) {
        input.value = parseInt(input.value) - 1;
    }
}


async function addToCartBackend(clothingId) {

    var size = document.getElementById("sizeSelect").value;
    var amount = parseInt(document.getElementById("quantityInput").value);
    var messageEl = document.getElementById("cartMessage");

    if (!size) {
        messageEl.textContent = "Please select a size!";
        messageEl.style.color = "red";
        return;
    }

    try {

        var user = await getCurrentUser();
        console.log("Current user:", user);

        if (!user || !user.userId) {
            messageEl.textContent = "Please log in to add items to cart!";
            messageEl.style.color = "red";
            console.error("No user or userId found");
            return;
        }

        console.log("Adding to cart - clothingId:", clothingId, "userId:", user.userId, "amount:", amount);

        var response = await fetch(API_URL + "/api/Cart/addCart", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                clothingId: clothingId,
                userId: user.userId,
                amount: amount
            })
        });

        console.log("Cart response status:", response.status);

        if (response.ok) {
            messageEl.textContent = "Added to cart!";
            messageEl.style.color = "green";
        } else if (response.status === 401) {
            messageEl.textContent = "Session expired. Please log in again.";
            messageEl.style.color = "red";
        } else {
            messageEl.textContent = "Failed to add to cart.";
            messageEl.style.color = "red";
            console.error("Cart error - status:", response.status);
        }

    } catch (error) {
        console.error("Cart fetch error:", error);
        messageEl.textContent = "Connection error.";
        messageEl.style.color = "red";
    }
}


function getSavedProducts() {
    var saved = localStorage.getItem("savedProducts");
    if (saved) {
        return JSON.parse(saved);
    } else {
        return [];
    }
}

function setSaveButtonLabel(btn, isSaved) {
    if (!btn) return;

    btn.textContent = isSaved ? "\u2665 Saved" : "\u2661 Save";
    btn.classList.toggle("saved", isSaved);
}

function toggleSaveProduct(btn) {

    var id = parseInt(btn.dataset.id);
    var name = btn.dataset.name;
    var price = btn.dataset.price;
    var image = btn.dataset.image;
    var page = btn.dataset.page;

    var saved = getSavedProducts();

    var found = -1;
    for (var i = 0; i < saved.length; i++) {
        if (saved[i].id === id) {
            found = i;
            break;
        }
    }

    if (found > -1) {
        saved.splice(found, 1);
        setSaveButtonLabel(btn, false);
    } else {
        saved.push({ id: id, name: name, price: price, image: image, page: page });
        setSaveButtonLabel(btn, true);
    }

    localStorage.setItem("savedProducts", JSON.stringify(saved));
}

function initSaveButton() {
    var btn = document.getElementById("saveBtn");
    if (!btn) return;

    var id = parseInt(btn.dataset.id);
    var saved = getSavedProducts();

    setSaveButtonLabel(btn, false);

    for (var i = 0; i < saved.length; i++) {
        if (saved[i].id === id) {
            setSaveButtonLabel(btn, true);
            break;
        }
    }
}


function getOrders() {
    var orders = localStorage.getItem("orderHistory");
    if (orders) {
        return JSON.parse(orders);
    } else {
        return [];
    }
}

async function saveOrder(items) {
    try {
        var user = await getCurrentUser();
        if (!user || !user.userId) {
            console.error("User not authenticated");
            return false;
        }

        var response = await fetch(API_URL + "/api/Orders/Checkout", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                address: "Not provided"
            })
        });

        if (response.ok) {
            return true;
        } else {
            console.error("Failed to save order");
            return false;
        }
    } catch (error) {
        console.error("Error saving order:", error);
        return false;
    }
}

async function getUserOrders(userId) {
    try {
        var response = await fetch(API_URL + "/api/Orders/Orders?userid=" + userId, {
            credentials: "include"
        });

        if (!response.ok) return [];

        var orders = await response.json();
        return Array.isArray(orders) ? orders : [];
    } catch (error) {
        console.error("Error fetching orders:", error);
        return [];
    }
}

function getCachedUser() {
    var rawUser = localStorage.getItem(CURRENT_USER_KEY);
    if (!rawUser) return null;

    try {
        return JSON.parse(rawUser);
    } catch (error) {
        localStorage.removeItem(CURRENT_USER_KEY);
        return null;
    }
}

function setCachedUser(user) {
    if (!user) return;

    var userId = user.userId || user.UserId || user.id || user.Id;
    if (!userId) {
        console.warn("User object missing ID field:", user);
        return;
    }

    var userToCache = {
        userId: userId,
        email: user.email || user.Email || '',
        userName: user.userName || user.UserName || '',
        role: user.role || user.Role || 'User'
    };

    localStorage.setItem(CURRENT_USER_KEY, JSON.stringify(userToCache));
}

function clearCachedUser() {
    localStorage.removeItem(CURRENT_USER_KEY);
}

async function syncCachedUserFromApi() {
    try {
        var response = await fetch(API_URL + "/api/User/me", {
            credentials: "include"
        });

        if (!response.ok) return null;

        var user = await response.json();
        if (user) {
            setCachedUser(user);
            return user;
        }
        return null;
    } catch (error) {
        console.error("Error syncing user from API:", error);
        return null;
    }
}

async function getCurrentUser() {
    var apiUser = await syncCachedUserFromApi();
    if (apiUser) return apiUser;

    return getCachedUser();
}

async function getCartItemsByUserId(userId) {
    if (!userId) return [];

    try {
        var response = await fetch(API_URL + "/api/Cart/CartContent?userid=" + userId, {
            credentials: "include"
        });

        if (!response.ok) return [];

        var data = await response.json();
        if (!data || !data.carts) return [];

        return data.carts;
    } catch (error) {
        return [];
    }
}

async function deleteCartItem(cartItemId) {
    try {
        var response = await fetch(API_URL + "/api/Cart/DeleteCartItem?id=" + cartItemId, {
            method: "DELETE",
            credentials: "include"
        });

        return response.ok;
    } catch (error) {
        console.error("Error deleting cart item:", error);
        return false;
    }
}

async function editCartItem(cartItemId, newQuantity, size) {
    try {
        var response = await fetch(API_URL + "/api/Cart/EditCart", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                cartItemId: cartItemId,
                amount: newQuantity,
                size: size
            })
        });

        return response.ok;
    } catch (error) {
        console.error("Error editing cart item:", error);
        return false;
    }
}

async function getAllClothes() {
    try {
        var response = await fetch(API_URL + "/api/Clothes/AllClothes", {
            credentials: "include"
        });

        if (!response.ok) return [];

        return await response.json();
    } catch (error) {
        return [];
    }
}

var globalSearchCache = {
    clothes: null,
    categoryCounters: null
};

async function getSearchableClothes() {
    if (globalSearchCache.clothes) return globalSearchCache.clothes;

    var clothes = await getAllClothes();
    globalSearchCache.clothes = clothes || [];
    return globalSearchCache.clothes;
}

function normalizeCategoryName(categoryName) {
    if (!categoryName) return "";
    var text = categoryName.toLowerCase();

    if (text.indexOf("hood") > -1) return "hoodie";
    if (text.indexOf("shirt") > -1 || text.indexOf("tee") > -1) return "tshirt";
    if (text.indexOf("pant") > -1 || text.indexOf("trouser") > -1) return "pants";

    return "";
}

function pageForCategory(categoryKey) {
    if (categoryKey === "tshirt") return "Tshirts.html";
    if (categoryKey === "hoodie") return "Hoodies.html";
    if (categoryKey === "pants") return "Pants.html";
    return "Namona.html";
}

function getCategoryCounters(clothes) {
    if (globalSearchCache.categoryCounters) return globalSearchCache.categoryCounters;

    var grouped = {
        tshirt: [],
        hoodie: [],
        pants: []
    };

    for (var i = 0; i < clothes.length; i++) {
        var item = clothes[i];
        var key = normalizeCategoryName(item.categoryName);
        if (!key || !grouped[key]) continue;
        grouped[key].push(item);
    }

    var keys = Object.keys(grouped);
    for (var j = 0; j < keys.length; j++) {
        grouped[keys[j]].sort(function (a, b) {
            return (a.clothingId || 0) - (b.clothingId || 0);
        });
    }

    globalSearchCache.categoryCounters = grouped;
    return grouped;
}

function getProductPageUrl(item, clothes) {
    var key = normalizeCategoryName(item.categoryName);
    if (!key) return "Namona.html";

    var counters = getCategoryCounters(clothes);
    var group = counters[key] || [];
    var index = 0;

    for (var i = 0; i < group.length; i++) {
        if (group[i].clothingId === item.clothingId) {
            index = i + 1;
            break;
        }
    }

    if (!index) return pageForCategory(key);
    var baseUrl = '';
    if (key === "tshirt") baseUrl = "TshirtProduct" + index + ".html";
    else if (key === "hoodie") baseUrl = "HoodieProduct" + index + ".html";
    else if (key === "pants") baseUrl = "PantsProduct" + index + ".html";

    return baseUrl + "?id=" + item.clothingId;
}

function getItemSearchText(item) {
    return [
        item.clothingName,
        item.categoryName,
        item.color,
        item.size
    ].join(" ").toLowerCase();
}

function ensureGlobalSearchStyles() {
    if (document.getElementById("globalSearchStyles")) return;

    var style = document.createElement("style");
    style.id = "globalSearchStyles";
    style.textContent =
        '.global-search-wrap { position: absolute; left: 80px; width: 280px; display: flex; align-items: center; gap: 8px; z-index: 2; }' +
        '.global-search-input { flex: 1; height: 38px; border-radius: 999px; border: 1px solid rgba(255,255,255,0.5); background: rgba(255,255,255,0.12); color: white; padding: 0 14px; outline: none; }' +
        '.global-search-input::placeholder { color: rgba(255,255,255,0.7); }' +
        '.global-search-input:focus { border-color: #FFC700; }' +
        '.global-search-clear { height: 38px; border: none; border-radius: 999px; padding: 0 12px; font-weight: bold; cursor: pointer; }' +
        '.global-search-results { position: absolute; top: 46px; left: 0; right: 0; background: rgba(0,0,0,0.95); border: 1px solid rgba(255,255,255,0.2); border-radius: 10px; max-height: 260px; overflow-y: auto; display: none; }' +
        '.global-search-result-item { display: flex; justify-content: space-between; gap: 8px; width: 100%; text-align: left; border: none; background: transparent; color: white; padding: 10px 12px; cursor: pointer; }' +
        '.global-search-result-item:hover { background: rgba(255,255,255,0.15); }' +
        '.global-side-nav { display: flex; flex-direction: column; gap: 8px; margin-bottom: 4px; }' +
        '.global-side-nav a { color: white; text-decoration: none; font-size: 22px; padding: 10px 20px; }' +
        '.global-side-nav a:hover { text-decoration: underline; }' +
        '.global-side-filter-panel { display: flex; flex-direction: column; gap: 10px; padding: 8px 16px 8px; border-top: 1px solid rgba(255,255,255,0.25); margin-bottom: 4px; box-sizing: border-box; }' +
        '.global-side-filter-panel h3 { margin: 0; color: white; font-family: Trebuchet MS, sans-serif; font-size: 20px; }' +
        '.global-side-input { width: calc(100% - 12px); box-sizing: border-box; min-height: 34px; border-radius: 6px; border: none; padding: 6px 10px; font-size: 14px; }' +
        '.global-check-group { display: flex; flex-direction: column; gap: 4px; }' +
        '.global-check-group-title { color: #FFC700; font-size: 13px; font-family: Trebuchet MS, sans-serif; margin-bottom: 2px; }' +
        '.global-check-list { display: flex; flex-direction: column; gap: 4px; max-height: 110px; overflow-y: auto; }' +
        '.global-check-item { display: flex; align-items: center; gap: 6px; color: white; font-size: 13px; cursor: pointer; }' +
        '.global-check-item input[type="checkbox"] { width: 13px; height: 13px; min-width: 13px; margin: 0; cursor: pointer; }' +
        '.global-check-item span { flex: 1; }' +
        '.global-side-results { display: flex; flex-direction: column; max-height: 160px; overflow-y: auto; }' +
        '.global-side-results a { color: #f3f3f3; text-decoration: none; font-size: 13px; padding: 5px 0; }' +
        '.global-side-results a:hover { text-decoration: underline; }' +
        '.global-side-reset { width: 100%; min-height: 34px; border: none; border-radius: 6px; font-weight: bold; cursor: pointer; box-sizing: border-box; }' +
        '.product-name { margin: 8px 0 0 0; color: white; text-align: center; font-size: 14px; font-family: Trebuchet MS, sans-serif; }' +
        '.product-link { text-decoration: none; display: flex; flex-direction: column; align-items: center; cursor: pointer; transition: transform 0.3s ease; }';

    document.head.appendChild(style);
}

function normalizeSideMenuLayout() {
    var sideMenu = document.getElementById("sideMenu");
    if (!sideMenu) return;

    ensureGlobalSearchStyles();

    var filterPanel = sideMenu.querySelector(".side-filter-panel") || sideMenu.querySelector(".global-side-filter-panel");
    var children = Array.prototype.slice.call(sideMenu.children);

    for (var i = 0; i < children.length; i++) {
        if (children[i].tagName === "A") {
            sideMenu.removeChild(children[i]);
        }
    }

    var navBlock = document.createElement("div");
    navBlock.className = "global-side-nav";
    navBlock.innerHTML =
        '<a href="Namona.html">Main Page</a>' +
        '<a href="Tshirts.html">T-shirts</a>' +
        '<a href="Hoodies.html">Hoodies</a>' +
        '<a href="Pants.html">Pants</a>';

    sideMenu.insertBefore(navBlock, sideMenu.firstChild);

    if (filterPanel) {
        sideMenu.insertBefore(filterPanel, navBlock.nextSibling);
    }
}

function renderGlobalSearchResults(container, items, clothes) {
    container.innerHTML = "";

    if (!items.length) {
        container.style.display = "none";
        return;
    }

    items.forEach(function(item) {
        var pageUrl = getProductPageUrl(item, clothes);
        var button = document.createElement("button");
        button.type = "button";
        button.className = "global-search-result-item";
        button.innerHTML =
            '<span>' + item.clothingName + '</span>' +
            '<span>' + (item.categoryName || "") + '</span>';
        button.dataset.url = pageUrl;
        button.onclick = function() { window.location.href = this.dataset.url; };

        container.appendChild(button);
    });

    container.style.display = "block";
}

function initGlobalHeaderSearch() {
    if (document.getElementById("headerSearchInput")) return;

    var header = document.querySelector("header");
    if (!header) return;

    ensureGlobalSearchStyles();
    header.classList.add("global-search-enabled");

    var wrap = document.createElement("div");
    wrap.className = "global-search-wrap";
    wrap.innerHTML =
        '<input id="globalHeaderSearchInput" class="global-search-input" type="search" placeholder="Search products..." aria-label="Search products">' +
        '<button id="globalHeaderSearchClear" class="global-search-clear" type="button">Clear</button>' +
        '<div id="globalHeaderSearchResults" class="global-search-results"></div>';

    header.appendChild(wrap);

    var input = document.getElementById("globalHeaderSearchInput");
    var clearBtn = document.getElementById("globalHeaderSearchClear");
    var results = document.getElementById("globalHeaderSearchResults");

    input.addEventListener("input", async function () {
        var query = (input.value || "").trim().toLowerCase();
        if (!query) {
            results.style.display = "none";
            results.innerHTML = "";
            return;
        }

        var clothes = await getSearchableClothes();
        var matched = [];

        for (var i = 0; i < clothes.length; i++) {
            if (getItemSearchText(clothes[i]).indexOf(query) > -1) {
                matched.push(clothes[i]);
            }
            if (matched.length >= 8) break;
        }

        renderGlobalSearchResults(results, matched, clothes);
    });

    input.addEventListener("keydown", async function (e) {
        if (e.key !== "Enter") return;

        var query = (input.value || "").trim().toLowerCase();
        if (!query) return;

        var clothes = await getSearchableClothes();
        for (var i = 0; i < clothes.length; i++) {
            if (getItemSearchText(clothes[i]).indexOf(query) > -1) {
                window.location.href = getProductPageUrl(clothes[i], clothes);
                return;
            }
        }
    });

    clearBtn.addEventListener("click", function () {
        input.value = "";
        results.innerHTML = "";
        results.style.display = "none";
    });

    document.addEventListener("click", function (e) {
        if (!wrap.contains(e.target)) {
            results.style.display = "none";
        }
    });
}

function renderSideCheckboxes(container, name, values) {
    container.innerHTML = "";

    for (var i = 0; i < values.length; i++) {
        var value = values[i];
        container.innerHTML +=
            '<label class="global-check-item">' +
                '<input type="checkbox" name="' + name + '" value="' + value + '">' +
                '<span>' + value + '</span>' +
            '</label>';
    }
}

function selectedCheckboxValues(scope, name) {
    var nodes = scope.querySelectorAll('input[name="' + name + '"]:checked');
    var values = [];

    for (var i = 0; i < nodes.length; i++) {
        values.push(nodes[i].value);
    }

    return values;
}

function uniqueValuesFromClothes(clothes, key) {
    var map = {};
    for (var i = 0; i < clothes.length; i++) {
        if (!clothes[i][key]) continue;
        map[clothes[i][key]] = true;
    }

    var values = Object.keys(map);
    values.sort(function (a, b) {
        return a.localeCompare(b);
    });
    return values;
}

function initGlobalSideFilter() {
    var sideMenu = document.getElementById("sideMenu");
    if (!sideMenu) return;

    ensureGlobalSearchStyles();

    var panel = document.querySelector(".side-filter-panel") || document.querySelector(".global-side-filter-panel");

    if (!panel) {
        panel = document.createElement("div");
        panel.className = "global-side-filter-panel";
        var navBlock = sideMenu.querySelector(".global-side-nav");
        if (navBlock && navBlock.nextSibling) {
            sideMenu.insertBefore(panel, navBlock.nextSibling);
        } else {
            sideMenu.appendChild(panel);
        }
    }

    panel.innerHTML =
        '<h3>Find Products</h3>' +
        '<input id="globalSideSearchInput" class="global-side-input" type="search" placeholder="Search by name, color..." aria-label="Filter products">' +
        '<div class="global-check-group"><span class="global-check-group-title">Category</span><div id="globalCategoryChecks" class="global-check-list"></div></div>' +
        '<div class="global-check-group"><span class="global-check-group-title">Color</span><div id="globalColorChecks" class="global-check-list"></div></div>' +
        '<div id="globalSideResults" class="global-side-results"></div>' +
        '<button id="globalSideReset" class="global-side-reset" type="button">Reset filters</button>';

    var input = document.getElementById("globalSideSearchInput");
    var categoryWrap = document.getElementById("globalCategoryChecks");
    var colorWrap = document.getElementById("globalColorChecks");
    var results = document.getElementById("globalSideResults");
    var resetBtn = document.getElementById("globalSideReset");

    function renderResultLinks(items, clothes) {
        results.innerHTML = "";
        var max = items.length > 8 ? 8 : items.length;

        for (var i = 0; i < max; i++) {
            var item = items[i];
            var link = document.createElement("a");
            link.href = getProductPageUrl(item, clothes);
            link.textContent = item.clothingName + " - " + (item.categoryName || "");
            results.appendChild(link);
        }
    }

    function applySideFilters(clothes) {
        var query = (input.value || "").trim().toLowerCase();
        var selectedCategories = selectedCheckboxValues(panel, "globalCategory");
        var selectedColors = selectedCheckboxValues(panel, "globalColor");
        var filtered = [];

        for (var i = 0; i < clothes.length; i++) {
            var item = clothes[i];
            var textPass = !query || getItemSearchText(item).indexOf(query) > -1;
            var catPass = !selectedCategories.length || selectedCategories.indexOf(item.categoryName) > -1;
            var colorPass = !selectedColors.length || selectedColors.indexOf(item.color) > -1;

            if (textPass && catPass && colorPass) {
                filtered.push(item);
            }
        }

        renderResultLinks(filtered, clothes);
    }

    getSearchableClothes().then(function (clothes) {
        renderSideCheckboxes(categoryWrap, "globalCategory", uniqueValuesFromClothes(clothes, "categoryName"));
        renderSideCheckboxes(colorWrap, "globalColor", uniqueValuesFromClothes(clothes, "color"));
        applySideFilters(clothes);

        input.addEventListener("input", function () {
            applySideFilters(clothes);
        });

        panel.addEventListener("change", function () {
            applySideFilters(clothes);
        });

        resetBtn.addEventListener("click", function () {
            input.value = "";
            var nodes = panel.querySelectorAll('input[type="checkbox"]');
            for (var i = 0; i < nodes.length; i++) {
                nodes[i].checked = false;
            }
            applySideFilters(clothes);
        });
    });
}

function initGlobalSearchUi() {
    initGlobalHeaderSearch();
    normalizeSideMenuLayout();
    initGlobalSideFilter();
}