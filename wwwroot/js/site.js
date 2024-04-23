// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    function toggleMenu() {
        var menu = document.getElementById("logoutMenu");
        menu.classList.toggle("hidden");
        menu.classList.toggle("visible");
    }

    $(document).ready(function () {
        $('#searchInput').on('input', function () {
            var searchString = $(this).val().toLowerCase();
            var form = $(this).closest('form');
            var actionUrl = form.attr('action');
            var method = form.attr('method');

            $.ajax({
                url: actionUrl,
                type: method,
                data: form.serialize(), // Сериализуем все данные формы
                success: function (data) {
                    var newTBody = $(data).find('tbody');
                    $('tbody').replaceWith(newTBody);
                }
            });
        });
    });


// Функция для переключения темы
function toggleTheme() {
    // Получаем текущее значение выбранной темы из localStorage
    var currentTheme = localStorage.getItem('theme');

    // Если текущая тема - светлая, то переключаем на темную, и наоборот
    if (currentTheme === 'light') {
        applyTheme('dark');
    } else {
        applyTheme('light');
    }
}

// Функция для применения выбранной темы
function applyTheme(theme) {
    // Применяем класс темы к body
    document.body.classList.toggle('dark-theme', theme === 'dark');
    if(theme === "dark") toggleBgClasses('bg-light', 'bg-dark')
    else toggleBgClasses('bg-dark', 'bg-light');
    // Сохраняем выбранную тему в localStorage
    localStorage.setItem('theme', theme);
}

// Функция для переключения классов фона
function toggleBgClasses(oldClass, newClass) {
    var elements = document.querySelectorAll('.' + oldClass);
    elements.forEach(function(element) {
        element.classList.remove(oldClass);
        element.classList.add(newClass);
    });
}

// Применяем сохраненную тему при загрузке страницы
var savedTheme = localStorage.getItem('theme');
if (savedTheme) {
    applyTheme(savedTheme);
}

// Обработчик события для кнопки переключения темы
document.getElementById('theme-toggle').addEventListener('click', function() {
    toggleTheme();
});