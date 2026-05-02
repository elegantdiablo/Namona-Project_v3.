document.addEventListener("DOMContentLoaded", () => {
    checkAdminAccess();
    loadDashboard();
    setupAdminModalHandling();
});

function setupAdminModalHandling() {
    window.onclick = function(event) {
        const modal = document.getElementById("editModal");
        if (event.target == modal) {
            modal.style.display = "none";
        }
    };
}

async function checkAdminAccess() {
    try {
        const response = await fetch(API_URL + "/api/User/me", {
            credentials: "include"
        });

        if (!response.ok) {
            showAdminMessage("Not authorized. Please log in as admin.", "error");
            setTimeout(() => {
                window.location.href = "NamonaLogin.html";
            }, 2000);
            return;
        }

        const user = await response.json();
        if (user.role !== "Admin") {
            showAdminMessage("Access denied. Admin only.", "error");
            setTimeout(() => {
                window.location.href = "Namona.html";
            }, 2000);
            return;
        }

        const profileMenu = document.getElementById("profileMenu");
        if (profileMenu) {
            profileMenu.innerHTML = `
                <span>Welcome, ${user.userName}</span>
                <a href="#" onclick="logoutAdmin()">🚪 Logout</a>
            `;
        }
    } catch (error) {
        console.error("Error checking admin access:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

function switchTab(tabName) {
    document.querySelectorAll(".tab-content").forEach(tab => {
        tab.classList.remove("active");
    });
    document.querySelectorAll(".admin-tab").forEach(tab => {
        tab.classList.remove("active");
    });

    const selectedTab = document.getElementById(tabName);
    if (selectedTab) {
        selectedTab.classList.add("active");
    }
    
    if (event?.target) {
        event.target.classList.add("active");
    }

    if (tabName === "dashboard") {
        loadDashboard();
    } else if (tabName === "users") {
        loadAllUsers();
    } else if (tabName === "orders") {
        loadAllOrders();
    } else if (tabName === "clothes") {
        loadAllClothes();
    } else if (tabName === "categories") {
        loadAllCategories();
    }
}

async function loadDashboard() {
    try {
        const [usersRes, ordersRes, revenueRes, clothesRes] = await Promise.all([
            fetch(API_URL + "/api/User/ShowUsers", { credentials: "include" }),
            fetch(API_URL + "/api/Orders/AllOrders", { credentials: "include" }),
            fetch(API_URL + "/api/Orders/GetRevenue", { credentials: "include" }),
            fetch(API_URL + "/api/Clothes/GetAllClothes", { credentials: "include" })
        ]);

        const users = usersRes.ok ? await usersRes.json() : [];
        const orders = ordersRes.ok ? await ordersRes.json() : [];
        const revenue = revenueRes.ok ? await revenueRes.json() : 0;
        const clothes = clothesRes.ok ? await clothesRes.json() : [];

        document.getElementById("totalUsers").textContent = users.length;
        document.getElementById("totalOrders").textContent = orders.length;
        document.getElementById("totalRevenue").textContent = revenue;
        document.getElementById("totalProducts").textContent = clothes.length;

        loadRecentOrders(orders.slice(0, 4));
    } catch (error) {
        console.error("Error loading dashboard:", error);
        showAdminMessage("Error loading dashboard data.", "error");
    }
}

function loadRecentOrders(orders) {
    const container = document.getElementById("recentOrdersContainer");
    
    if (!orders || orders.length === 0) {
        container.innerHTML = '<div class="no-data">No orders found</div>';
        return;
    }

    let tableHTML = `
        <table class="products-table">
            <thead>
                <tr>
                    <th>Order ID</th>
                    <th>User</th>
                    <th>Status</th>
                    <th>Date</th>
                </tr>
            </thead>
            <tbody>
    `;

    orders.forEach(order => {
        tableHTML += `
            <tr>
                <td>${order.orderId}</td>
                <td>${order.userId || 'N/A'}</td>
                <td>${order.status || 'N/A'}</td>
                <td>${new Date(order.orderDate).toLocaleString()}</td>
            </tr>
        `;
    });

    tableHTML += `
            </tbody>
        </table>
    `;

    container.innerHTML = tableHTML;
}

async function loadAllUsers() {
    const container = document.getElementById("usersContainer");
    container.innerHTML = '<div class="no-data">Loading users...</div>';

    try {
        const response = await fetch(API_URL + "/api/User/ShowUsers", {
            credentials: "include"
        });

        if (!response.ok) {
            container.innerHTML = '<div class="no-data">Failed to load users.</div>';
            return;
        }

        const users = await response.json();

        if (!Array.isArray(users) || users.length === 0) {
            container.innerHTML = '<div class="no-data">No users found.</div>';
            return;
        }

        let tableHTML = `
            <table class="products-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Username</th>
                        <th>Email</th>
                        <th>Role</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
        `;

        users.forEach(user => {
            tableHTML += `
                <tr>
                    <td>${user.userId}</td>
                    <td>${user.userName || 'N/A'}</td>
                    <td>${user.email || 'N/A'}</td>
                    <td>${user.role || 'N/A'}</td>
                    <td>
                        <div class="table-actions">
                            <button class="btn-delete" onclick="deleteUser(${user.userId})">Delete</button>
                        </div>
                    </td>
                </tr>
            `;
        });

        tableHTML += `
                </tbody>
            </table>
        `;

        container.innerHTML = tableHTML;
    } catch (error) {
        console.error("Error loading users:", error);
        container.innerHTML = '<div class="no-data">Error loading users. Please try again.</div>';
    }
}

async function deleteUser(userId) {
    if (!confirm("Are you sure you want to delete this user?")) {
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/User/DeleteUser?id=" + userId, {
            method: "DELETE",
            credentials: "include"
        });

        if (response.ok) {
            showAdminMessage("User deleted successfully!", "success");
            loadAllUsers();
        } else {
            showAdminMessage("Failed to delete user. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error deleting user:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function loadAllOrders() {
    const container = document.getElementById("ordersContainer");
    container.innerHTML = '<div class="no-data">Loading orders...</div>';

    try {
        const response = await fetch(API_URL + "/api/Orders/AllOrders", {
            credentials: "include"
        });

        if (!response.ok) {
            container.innerHTML = '<div class="no-data">Failed to load orders.</div>';
            return;
        }

        const orders = await response.json();

        if (!Array.isArray(orders) || orders.length === 0) {
            container.innerHTML = '<div class="no-data">No orders found.</div>';
            return;
        }

        let tableHTML = `
            <table class="products-table">
                <thead>
                    <tr>
                        <th>Order ID</th>
                        <th>User</th>
                        <th>Status</th>
                        <th>Total Price</th>
                        <th>Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
        `;

        orders.forEach(order => {
            tableHTML += `
                <tr>
                    <td>${order.orderId}</td>
                    <td>${order.userId || 'N/A'}</td>
                    <td>${order.status || 'N/A'}</td>
                    <td>$${order.totalPrice || 0}</td>
                    <td>${new Date(order.orderDate).toLocaleString()}</td>
                    <td>
                        <div class="table-actions">
                            <button class="btn-edit" onclick="completeOrder(${order.orderId})">Complete</button>
                            <button class="btn-delete" onclick="deleteOrder(${order.orderId})">Delete</button>
                        </div>
                    </td>
                </tr>
            `;
        });

        tableHTML += `
                </tbody>
            </table>
        `;

        container.innerHTML = tableHTML;
    } catch (error) {
        console.error("Error loading orders:", error);
        container.innerHTML = '<div class="no-data">Error loading orders. Please try again.</div>';
    }
}

async function completeOrder(orderId) {
    try {
        const response = await fetch(API_URL + "/api/Orders/Complete?id=" + orderId, {
            method: "PUT",
            credentials: "include"
        });

        if (response.ok) {
            showAdminMessage("Order completed successfully!", "success");
            loadAllOrders();
        } else {
            showAdminMessage("Failed to complete order. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error completing order:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function deleteOrder(orderId) {
    if (!confirm("Are you sure you want to delete this order?")) {
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/Orders/DeleteOrder?id=" + orderId, {
            method: "DELETE",
            credentials: "include"
        });

        if (response.ok) {
            showAdminMessage("Order deleted successfully!", "success");
            loadAllOrders();
        } else {
            showAdminMessage("Failed to delete order. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error deleting order:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function loadAllClothes() {
    const container = document.getElementById("clothesContainer");
    container.innerHTML = '<div class="no-data">Loading clothes...</div>';

    try {
        const response = await fetch(API_URL + "/api/Clothes/GetAllClothes", {
            credentials: "include"
        });

        if (!response.ok) {
            container.innerHTML = '<div class="no-data">Failed to load clothes.</div>';
            return;
        }

        const clothes = await response.json();

        if (!Array.isArray(clothes) || clothes.length === 0) {
            container.innerHTML = '<div class="no-data">No clothes found.</div>';
            return;
        }

        let tableHTML = `
            <table class="products-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Category</th>
                        <th>Color</th>
                        <th>Size</th>
                        <th>Stock</th>
                        <th>Price</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
        `;

        clothes.forEach(item => {
            tableHTML += `
                <tr>
                    <td>${item.clothingId}</td>
                    <td>${item.clothingName || 'N/A'}</td>
                    <td>${item.categoryName || 'N/A'}</td>
                    <td>${item.color || 'N/A'}</td>
                    <td>${item.size || 'N/A'}</td>
                    <td>${item.stock || 0}</td>
                    <td>$${item.price || 0}</td>
                    <td>
                        <div class="table-actions">
                            <button class="btn-edit" onclick="openEditModal(${item.clothingId})">Edit</button>
                            <button class="btn-delete" onclick="deleteClothes(${item.clothingId})">Delete</button>
                        </div>
                    </td>
                </tr>
            `;
        });

        tableHTML += `
                </tbody>
            </table>
        `;

        container.innerHTML = tableHTML;
    } catch (error) {
        console.error("Error loading clothes:", error);
        container.innerHTML = '<div class="no-data">Error loading clothes. Please try again.</div>';
    }
}

async function deleteClothes(clothingId) {
    if (!confirm("Are you sure you want to delete this clothing item?")) {
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/Clothes/remove?id=" + clothingId, {
            method: "DELETE",
            credentials: "include"
        });

        if (response.ok) {
            showAdminMessage("Clothing deleted successfully!", "success");
            loadAllClothes();
        } else {
            showAdminMessage("Failed to delete clothing. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error deleting clothing:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function loadAllCategories() {
    const container = document.getElementById("categoriesContainer");
    container.innerHTML = '<div class="no-data">Loading categories...</div>';

    try {
        const response = await fetch(API_URL + "/api/Category/GetAllCategories", {
            credentials: "include"
        });

        if (!response.ok) {
            container.innerHTML = '<div class="no-data">Failed to load categories.</div>';
            return;
        }

        const categories = await response.json();

        if (!Array.isArray(categories) || categories.length === 0) {
            container.innerHTML = '<div class="no-data">No categories found.</div>';
            return;
        }

        let tableHTML = `
            <table class="products-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Category Name</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
        `;

        categories.forEach(category => {
            tableHTML += `
                <tr>
                    <td>${category.id}</td>
                    <td>${category.categoryName || 'N/A'}</td>
                    <td>
                        <div class="table-actions">
                            <button class="btn-edit" onclick="openEditCategoryModal(${category.id})">Edit</button>
                            <button class="btn-delete" onclick="deleteCategory(${category.id})">Delete</button>
                        </div>
                    </td>
                </tr>
            `;
        });

        tableHTML += `
                </tbody>
            </table>
        `;

        container.innerHTML = tableHTML;
    } catch (error) {
        console.error("Error loading categories:", error);
        container.innerHTML = '<div class="no-data">Error loading categories. Please try again.</div>';
    }
}

async function handleAddCategory(event) {
    event.preventDefault();

    const categoryName = document.getElementById("newCategoryName").value.trim();

    if (!categoryName) {
        showAdminMessage("Please enter a category name.", "error");
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/Category/AddCategory", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ categoryName: categoryName })
        });

        if (response.ok || response.status === 201) {
            showAdminMessage("Category added successfully!", "success");
            document.getElementById("addCategoryForm").reset();
            switchTab("categories");
        } else {
            showAdminMessage("Failed to add category. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error adding category:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function openEditCategoryModal(categoryId) {
    try {
        const response = await fetch(API_URL + "/api/Category/GetAllCategories", {
            credentials: "include"
        });

        if (!response.ok) return;

        const categories = await response.json();
        const category = categories.find(c => c.id === categoryId);

        if (!category) {
            showAdminMessage("Category not found.", "error");
            return;
        }

        const newName = prompt("Enter new category name:", category.categoryName);
        if (newName && newName.trim()) {
            await updateCategory(categoryId, newName.trim());
        }
    } catch (error) {
        console.error("Error:", error);
        showAdminMessage("Error loading category.", "error");
    }
}

async function updateCategory(categoryId, categoryName) {
    try {
        const response = await fetch(API_URL + "/api/Category/EditCategory", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({ id: categoryId, categoryName: categoryName })
        });

        if (response.ok) {
            showAdminMessage("Category updated successfully!", "success");
            loadAllCategories();
        } else {
            showAdminMessage("Failed to update category. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error updating category:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function deleteCategory(categoryId) {
    if (!confirm("Are you sure you want to delete this category?")) {
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/Category/DeleteCategory?id=" + categoryId, {
            method: "DELETE",
            credentials: "include"
        });

        if (response.ok) {
            showAdminMessage("Category deleted successfully!", "success");
            loadAllCategories();
        } else {
            showAdminMessage("Failed to delete category. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error deleting category:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

// ========== CLOTHES EDIT/ADD MODAL ==========
async function openEditModal(clothingId) {
    try {
        // Fetch clothes, categories, and genders
        const clothesResponse = await fetch(API_URL + "/api/Clothes/GetAllClothes", {
            credentials: "include"
        });
        const categoriesResponse = await fetch(API_URL + "/api/Category/GetAllCategories", {
            credentials: "include"
        });
        const gendersResponse = await fetch(API_URL + "/api/Gender/AllGenders", {
            credentials: "include"
        });

        if (!clothesResponse.ok) return;

        const clothes = await clothesResponse.json();
        const categories = categoriesResponse.ok ? await categoriesResponse.json() : [];
        const genders = gendersResponse.ok ? await gendersResponse.json() : [];

        const item = clothes.find(p => p.clothingId === clothingId);

        if (!item) {
            showAdminMessage("Clothing item not found.", "error");
            return;
        }

        const categorySelect = document.getElementById("editCategorySelect");
        categorySelect.innerHTML = '<option value="">-- Select Category --</option>';
        categories.forEach(cat => {
            const option = document.createElement("option");
            option.value = cat.id;
            option.textContent = cat.categoryName;
            option.setAttribute("data-name", cat.categoryName);
            if (cat.categoryName === item.categoryName) {
                option.selected = true;
            }
            categorySelect.appendChild(option);
        });

        const genderSelect = document.getElementById("editGenderSelect");
        genderSelect.innerHTML = '<option value="">-- Select Gender --</option>';
        genders.forEach(gen => {
            const option = document.createElement("option");
            option.value = gen.id;
            option.textContent = gen.type;
            option.setAttribute("data-name", gen.type);
            if (gen.type === item.genderName) {
                option.selected = true;
            }
            genderSelect.appendChild(option);
        });

        // Populate other fields
        document.getElementById("editProductId").value = item.clothingId;
        document.getElementById("editClothingName").value = item.clothingName || '';
        document.getElementById("editColor").value = item.color || '';
        document.getElementById("editSize").value = item.size || '';
        document.getElementById("editStock").value = item.stock || 0;
        document.getElementById("editPrice").value = item.price || 0;
        document.getElementById("editCollection").value = item.collection || '';
        document.getElementById("editImagePath").value = item.imagePath || '';

        document.getElementById("editModal").classList.add("show");
    } catch (error) {
        console.error("Error opening edit modal:", error);
        showAdminMessage("Error loading clothing details.", "error");
    }
}

function closeEditModal() {
    document.getElementById("editModal").classList.remove("show");
    document.getElementById("editProductForm").reset();
}

async function handleEditProduct(event) {
    event.preventDefault();

    const clothingId = parseInt(document.getElementById("editProductId").value);
    const categorySelect = document.getElementById("editCategorySelect");
    const genderSelect = document.getElementById("editGenderSelect");
    const categoryId = parseInt(categorySelect.value);
    const genderId = parseInt(genderSelect.value);
    
    const clothingName = document.getElementById("editClothingName").value.trim();
    const color = document.getElementById("editColor").value.trim();
    const size = document.getElementById("editSize").value.trim();
    const stock = parseInt(document.getElementById("editStock").value);
    const price = parseFloat(document.getElementById("editPrice").value);
    const collection = document.getElementById("editCollection").value.trim();

    const selectedCategoryOption = categorySelect.options[categorySelect.selectedIndex];
    const selectedGenderOption = genderSelect.options[genderSelect.selectedIndex];
    const category = selectedCategoryOption.getAttribute("data-name") || selectedCategoryOption.text;
    const genderType = selectedGenderOption.getAttribute("data-name") || selectedGenderOption.text;

    if (!clothingName || !color || !size || isNaN(stock) || isNaN(price)) {
        showAdminMessage("Please fill in all required fields (Name, Color, Size, Stock, Price).", "error");
        return;
    }

    if (categoryId <= 0 || genderId <= 0) {
        showAdminMessage("Please select a category and gender.", "error");
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/Clothes/modify", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                clothingId: clothingId,
                categoryId: categoryId,
                genderId: genderId,
                clothingName: clothingName,
                collection: collection,
                category: category,
                size: size,
                genderType: genderType,
                stock: stock,
                color: color,
                price: price
            })
        });

        if (response.ok) {
            showAdminMessage("Clothing updated successfully!", "success");
            closeEditModal();
            loadAllClothes();
        } else {
            showAdminMessage("Failed to update clothing. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error updating clothing:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

async function handleAddProduct(event) {
    event.preventDefault();

    const clothingName = document.getElementById("clothingName").value.trim();
    const color = document.getElementById("color").value.trim();
    const categoryId = parseInt(document.getElementById("categoryId").value);
    const genderId = parseInt(document.getElementById("genderId").value);
    const size = document.getElementById("size").value.trim();
    const stock = parseInt(document.getElementById("stock").value);
    const price = parseFloat(document.getElementById("price").value);
    const collection = document.getElementById("collection").value.trim();
    const imagePath = document.getElementById("imagePath").value.trim();

    const categoryNames = { 1: "T-Shirt", 2: "Hoodie", 3: "Pants" };
    const genderNames = { 1: "Male", 2: "Female", 3: "Unisex" };
    const categoryName = categoryNames[categoryId] || "";
    const genderName = genderNames[genderId] || "";

    if (!clothingName || !color || !categoryId || !genderId || !size || isNaN(stock) || isNaN(price)) {
        showAdminMessage("Please fill in all required fields.", "error");
        return;
    }

    try {
        const response = await fetch(API_URL + "/api/Clothes/AddClothes", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                clothingName: clothingName,
                color: color,
                categoryId: categoryId,
                categoryName: categoryName,
                genderId: genderId,
                genderName: genderName,
                size: size,
                stock: stock,
                price: price,
                collection: collection,
                imagePath: imagePath
            })
        });

        if (response.ok || response.status === 201) {
            showAdminMessage("Clothing added successfully!", "success");
            document.getElementById("addProductForm").reset();
            switchTab("clothes");
        } else {
            const errorData = await response.json().catch(() => ({}));
            showAdminMessage("Failed to add clothing. Status: " + response.status, "error");
        }
    } catch (error) {
        console.error("Error adding clothing:", error);
        showAdminMessage("Connection error. Please try again.", "error");
    }
}

function updateCategoryName() {
    const categoryId = document.getElementById("categoryId").value;
    const categoryNames = { 1: "T-Shirt", 2: "Hoodie", 3: "Pants" };
}

function updateGenderName() {
    const genderId = document.getElementById("genderId").value;
    const genderNames = { 1: "Male", 2: "Female", 3: "Unisex" };
}

function showAdminMessage(message, type) {
    const messageEl = document.getElementById("adminMessage");
    messageEl.textContent = message;
    messageEl.className = "message " + type;
    messageEl.style.display = "block";

    setTimeout(() => {
        messageEl.style.display = "none";
    }, 5000);
}

async function logoutAdmin() {
    try {
        await fetch(API_URL + "/api/User/logout", {
            method: "POST",
            credentials: "include"
        });
    } catch (error) {
        console.error("Logout error:", error);
    }

    window.location.href = "Namona.html";
}
