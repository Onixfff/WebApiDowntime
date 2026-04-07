// URL для API
const API_URL = '/api/Spslogger/FullData';
const API_URLMonth = '/api/Spslogger/DataMonth';

// Универсальная функция загрузки данных
async function loadData(url, tableId, loaderId, tbodyId, date) {
    const loader = document.getElementById(loaderId);
    const table = document.getElementById(tableId);
    const tbody = document.getElementById(tbodyId);

    try {
        // Формируем URL с параметром даты
        const fullUrl = date ? `${url}?date=${date}` : url;
        
        const response = await fetch(fullUrl);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        renderRows(data, tbody);

        // Показываем таблицу
        loader.style.display = 'none';
        table.style.display = 'table';

    } catch (error) {
        console.error('Ошибка загрузки:', error);
        
        loader.innerHTML = `
            <div class="error">
                ⚠️ Ошибка загрузки данных<br>
                <small>${error.message}</small>
            </div>
        `;
        
        table.style.display = 'none';
    }
}

async function loadDataMonth(url, tableId, loaderId, tbodyId, date) {
    const loader = document.getElementById(loaderId);
    const table = document.getElementById(tableId);
    const tbody = document.getElementById(tbodyId);

    try {
        // Формируем URL с параметром даты
        const fullUrl = date ? `${url}?date=${date}` : url;

        const response = await fetch(fullUrl);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        renderRowsMonth(data, tbody);

        // Показываем таблицу
        loader.style.display = 'none';
        table.style.display = 'table';

    } catch (error) {
        console.error('Ошибка загрузки:', error);

        loader.innerHTML = `
            <div class="error">
                ⚠️ Ошибка загрузки данных<br>
                <small>${error.message}</small>
            </div>
        `;

        table.style.display = 'none';
    }
}

// Функция отрисовки строк таблицы
function renderRows(data, tbody) {
    tbody.innerHTML = '';
    
    if (!data || data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" style="text-align: center; padding: 30px;">📭 Нет данных за выбранный период</td></tr>';
        return;
    }
    
    data.forEach(item => {
        const row = document.createElement('tr');
        
        // Определяем смену для стилизации
        const shiftClass = item.shift === 'день' ? 'day-shift' : 'night-shift';
        row.className = shiftClass;
        
        // Форматируем дату из "06 April 2026" в "2026-04-06"
        const formattedDate = formatDate(item.dateFormatted);
        
        // Создаём ячейки
        row.innerHTML = `
            <td data-label="🔄 Смена">
                <span class="shift-badge ${item.shift === 'день' ? 'shift-day' : 'shift-night'}">
                    ${item.shift === 'день' ? '☀️ День' : '🌙 Ночь'}
                </span>
            </td>
            <td data-label="📅 Дата">${formattedDate}</td>
            <td class="recipe-cell" data-label="🧪 Рецепт">${item.data52}</td>
            <td class="number-cell" data-label="🔢 Количество">${item.count1}</td>
            <td class="mass-cell" data-label="⚖️ Масса">${item.mas.toFixed(2)} м³</td>
        `;
        
        tbody.appendChild(row);
    });
}

// Функция отрисовки строк таблицы (месячная агрегация по рецептам)
function renderRowsMonth(data, tbody) {
    tbody.innerHTML = '';

    if (!data || data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 30px;">📭 Нет данных за выбранный период</td></tr>';
        return;
    }

    // 🔹 Шаг 1: Группируем данные по рецепту (data52)
    const groupedByRecipe = {};

    data.forEach(item => {
        const recipe = item.data52;

        // Если рецепт ещё не встречался — создаём запись
        if (!groupedByRecipe[recipe]) {
            groupedByRecipe[recipe] = {
                recipe: recipe,
                countSum: 0,
                massSum: 0
            };
        }

        // Суммируем значения
        groupedByRecipe[recipe].countSum += item.count1 || 0;
        groupedByRecipe[recipe].massSum += item.mas || 0;
    });

    // 🔹 Шаг 2: Формируем диапазон дат (первый и последний день из данных)
    const validDates = data
        .map(item => item.dateFormatted)
        .filter(d => d && d.trim() !== '');

    let dateRange = '—';
    if (validDates.length > 0) {
        const first = formatDate(validDates[0]);
        const last = formatDate(validDates[validDates.length - 1]);
        dateRange = `${first} | ${last}`;
    }

    // 🔹 Шаг 3: Отрисовываем строки для каждого рецепта
    Object.values(groupedByRecipe).forEach(group => {
        const row = document.createElement('tr');
        row.className = 'day-shift'; // Можно добавить логику цвета при необходимости

        row.innerHTML = `
            <td data-label="📅 Период">${dateRange}</td>
            <td class="recipe-cell" data-label="🧪 Рецепт">${group.recipe}</td>
            <td class="number-cell" data-label="🔢 Количество">${group.countSum}</td>
            <td class="mass-cell" data-label="⚖️ Масса">${group.massSum.toFixed(2)} м³</td>
        `;

        tbody.appendChild(row);
    });
}

// Функция форматирования даты
function formatDate(dateStr) {
    if (!dateStr) return '—';
    
    try {
        // Парсим дату из формата "06 April 2026"
        const months = {
            'January': '01', 'February': '02', 'March': '03',
            'April': '04', 'May': '05', 'June': '06',
            'July': '07', 'August': '08', 'September': '09',
            'October': '10', 'November': '11', 'December': '12'
        };
        
        const parts = dateStr.split(' ');
        if (parts.length !== 3) return dateStr;
        
        const day = parts[0].padStart(2, '0');
        const month = months[parts[1]] || '01';
        const year = parts[2];
        
        // Добавляем время (по умолчанию 00:00:00, так как в данных только дата)
        return `${year}-${month}-${day}`;
        
    } catch (e) {
        console.error('Ошибка форматирования даты:', e);
        return dateStr;
    }
}

function formatDateRange(dateStr) {
    if (!dateStr) return '—';

    try {
        const months = {
            'January': '01', 'February': '02', 'March': '03',
            'April': '04', 'May': '05', 'June': '06',
            'July': '07', 'August': '08', 'September': '09',
            'October': '10', 'November': '11', 'December': '12'
        };

        const parts = dateStr.split(' ');
        if (parts.length !== 3) return dateStr;

        const day = parts[0].padStart(2, '0');
        const month = months[parts[1]] || '01';
        const year = parts[2];

        return `${year}-${month}-${day}`;

    } catch (e) {
        console.error('Ошибка форматирования:', e);
        return dateStr;
    }
}

// Загрузка данных для таблицы 1
function loadDataForDate1() {
    const dateInput = document.getElementById('dateInput1').value;
    loadData(API_URL, 'table1', 'loader1', 'tbody1', dateInput);
}

// Загрузка данных для таблицы 2
function loadDataForDate2() {
    const dateInput2 = document.getElementById('dateInput2').value;
    loadDataMonth(API_URLMonth, 'table2', 'loader2', 'tbody2', dateInput2);
}

// Автоматическая загрузка при старте (сегодняшняя дата)
document.addEventListener('DOMContentLoaded', () => {
    // Устанавливаем сегодняшнюю дату в инпуты
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('dateInput1').value = today;
    document.getElementById('dateInput2').value = today;
    
    // Загружаем данные для первой таблицы
    loadDataForDate1();
    loadDataForDate2();
});