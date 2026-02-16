window.onfocus = function() {
    loadUrls(); 
};

document.addEventListener('DOMContentLoaded', loadUrls);

async function createUrl() {
    const input = document.getElementById('newLongUrl');
    const longUrl = input.value.trim();

    if (!longUrl || !longUrl.startsWith('http')) {
        input.classList.add('is-invalid');
        return;
    }
    input.classList.remove('is-invalid');

    const formData = new FormData();
    formData.append('longUrl', longUrl);

    const response = await fetch('/api/shorten', {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        const modal = bootstrap.Modal.getInstance(document.getElementById('createModal'));
        modal.hide();
        input.value = ''; 
        loadUrls(); 
    } else {
        const errorData = await response.json();
        alert(errorData.message || 'Ошибка при создании ссылки');
    }
}

// Улучшенная функция загрузки (чтобы не перезагружать страницу целиком)
async function loadUrls() {
    try {
        const response = await fetch('/api/urls');
        if (!response.ok) return;

        const urls = await response.json();
        const tableBody = document.getElementById('urlTableBody');

        if (urls.length === 0) {
            tableBody.innerHTML = '<tr><td colspan="5" class="text-center text-muted">Ссылок пока нет</td></tr>';
            return;
        }

        tableBody.innerHTML = urls.map(url => `
            <tr>
                <td class="text-truncate-custom" title="${url.longUrl}">${url.longUrl}</td>
                <td><a href="/${url.shortCode}" target="_blank" class="fw-bold">/${url.shortCode}</a></td>
                <td>${new Date(url.createdAt).toLocaleDateString()}</td>
                <td><span class="badge bg-info text-dark">${url.clickCount}</span></td>
                <td>
                    <div class="btn-group">
                        <button onclick="openEditModal(${url.id}, '${url.longUrl}')" class="btn btn-sm btn-outline-warning">Редактировать</button>
                        <button onclick="deleteUrl(${url.id})" class="btn btn-sm btn-outline-danger">Удалить</button>
                    </div>
                </td>
            </tr>
        `).join('');
    } catch (e) {
        console.error("Ошибка загрузки данных:", e);
    }
}

// Функция для удаления (согласно ТЗ: удаляет запись)
async function deleteUrl(id) {
    if (!confirm('Вы уверены, что хотите удалить эту ссылку?')) return;

    const response = await fetch(`/Home/Delete/${id}`, { method: 'POST' });
    if (response.ok) {
        location.reload(); // Перезагружаем для обновления таблицы
    } else {
        alert('Ошибка при удалении');
    }
}

// Функция для открытия попапа редактирования
function openEditModal(id, currentUrl) {
    document.getElementById('editId').value = id;
    document.getElementById('editLongUrl').value = currentUrl;
    new bootstrap.Modal(document.getElementById('editModal')).show();
}

// Сохранение изменений (Команда Update)
async function saveEdit() {
    const id = document.getElementById('editId').value;
    const newUrl = document.getElementById('editLongUrl').value;

    const formData = new FormData();
    formData.append('Id', id);
    formData.append('NewLongUrl', newUrl);

    const response = await fetch('/Home/Edit', {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        location.reload();
    } else {
        const errorData = await response.json();
        alert(errorData.message || 'Ошибка при обновлении');
    }
}