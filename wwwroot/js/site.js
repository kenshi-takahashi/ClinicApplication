// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    function toggleMenu() {
        var menu = document.getElementById("logoutMenu");
        menu.classList.toggle("hidden");
        menu.classList.toggle("visible");
    }
    
// $(document).ready(function () {
//     $('#searchInput').on('input', function () {
//         var searchString = $(this).val().toLowerCase();
//         $.ajax({
//             url: '/Appointment/Index',
//             type: 'GET',
//             data: { searchString: searchString },
//             success: function (data) {
//                 var newTBody = $(data).find('tbody');
//                 $('tbody').replaceWith(newTBody);
//             }
//         });
//     });
// });

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





